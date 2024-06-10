using Microsoft.AspNetCore.Mvc;
using APZ_backend.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace APZ_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserRepo userRepo;
        private readonly SubordinateRepo subordinateRepo;
        public RegistrationController(UserRepo _userRepo, SubordinateRepo subordinateRepo)
        {
            userRepo = _userRepo;
            this.subordinateRepo = subordinateRepo;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(string name, string email, string password, string type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await userRepo.InsertUserAsync(
                new User() 
                { 
                    Name = name,
                    Email = email,
                    Password = password,
                    Type = type
                }
            ))
            {
                ModelState.AddModelError("", "Unable to register");
                return StatusCode(500, ModelState);
            }

            var authController = new AuthorizationController(userRepo, subordinateRepo);

            return await authController.AuthorizeUser(new LoginData(email, password));

        }

        [HttpPost("registerSubordinate")]
        [Authorize]
        public async Task<IActionResult> RegisterSubordinate(string name, string email, string password)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await subordinateRepo.InsertSubordinateAsync(
                new Subordinate()
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    ChiefId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                }
            ))
            {
                ModelState.AddModelError("", "Unable to register");
                return StatusCode(500, ModelState);
            }

            var authController = new AuthorizationController(userRepo, subordinateRepo);

            return await authController.AuthorizeSubordinate(new LoginData(email, password));

        }
    }
}
