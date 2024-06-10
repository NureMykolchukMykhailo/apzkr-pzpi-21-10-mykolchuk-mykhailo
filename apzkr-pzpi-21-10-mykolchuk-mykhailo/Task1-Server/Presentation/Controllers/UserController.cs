using Microsoft.AspNetCore.Mvc;
using APZ_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using APZ_backend.DTO;
using AutoMapper;


namespace APZ_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepo userRepo;
        private readonly IMapper mapper;

        public UserController(UserRepo userRepo, IMapper mapper)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = mapper.Map<List<UserDto>>(await userRepo.GetUsersAsync());

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User? user = await userRepo.GetUserAsync(id);
            if(user is not null)
            {
                var userDto = mapper.Map<UserDto>(user);
                return Ok(userDto);
            }

            return NotFound("No such user");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] int id, string name, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = new User()
            {
                Name = name,
                Password = password
            };
            if(!await userRepo.UpdateUserAsync(id, user))
            {
                ModelState.AddModelError("", "Unable to update info");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await userRepo.DeleteUserAsync(id))
            {
                ModelState.AddModelError("", "Unable to delete user");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted");
        }

        [HttpPost("changeLng")]
        public async Task<IActionResult> ChangeUserLanguage(int id, string lng)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string returnedLang = await userRepo.ChangeLanguage(id, lng);

            if (returnedLang == "fail")
                return BadRequest("Fail or no such user");

            return Ok(returnedLang);
        }
    }
}
