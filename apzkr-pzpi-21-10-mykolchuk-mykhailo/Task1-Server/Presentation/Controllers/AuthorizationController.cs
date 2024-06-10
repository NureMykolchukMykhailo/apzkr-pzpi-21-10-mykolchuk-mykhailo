using Microsoft.AspNetCore.Mvc;
using APZ_backend.Repositories;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APZ_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserRepo userRepo;
        private readonly SubordinateRepo subordinateRepo;
        public AuthorizationController(UserRepo _userRepo, SubordinateRepo subordinateRepo)
        {
            userRepo = _userRepo;
            this.subordinateRepo = subordinateRepo;
        }

        [HttpPost("authorizeUser")]
        public async Task<IActionResult> AuthorizeUser([FromForm] LoginData loginData)
        {
            User? user = await userRepo.GetUserByEmailAndPasswordAsync(loginData.email, loginData.password);
            if(user is not null)
            {
                var response = GenerateToken(user.Id, user.Language, loginData);
                return Ok(response);
            }
            return Content("Invalid login or password");
        }

        [HttpPost("authorizeSubordinate")]
        public async Task<IActionResult> AuthorizeSubordinate([FromForm] LoginData loginData)
        {
            Subordinate? subordinate = 
                await subordinateRepo.GetSubordinateByEmailAndPasswordAsync(loginData.email, loginData.password);
            if (subordinate is not null)
            {
                var response = GenerateToken(subordinate.Id, subordinate.Language, loginData);
                return Ok(response);
            }
            return Content("Invalid login or password");
        }

        private static object GenerateToken(int userId, string lang, LoginData loginData)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, loginData.email),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Locality, lang)
            };

            var jwt = new JwtSecurityToken(
                    issuer: Environment.GetEnvironmentVariable("Auth-Issuer"),
                    audience: Environment.GetEnvironmentVariable("Auth-Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(10)),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Auth-Key"))
                            ), 
                        SecurityAlgorithms.HmacSha256)
                    );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = loginData.email,
                lang = lang
            };

            return response;
        }
    }
}
