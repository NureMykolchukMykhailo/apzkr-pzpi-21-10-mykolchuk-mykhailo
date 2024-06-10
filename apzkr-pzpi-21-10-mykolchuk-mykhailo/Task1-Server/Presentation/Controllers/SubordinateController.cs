using APZ_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using APZ_backend.DTO;


namespace APZ_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubordinateController : ControllerBase
    {
        private readonly SubordinateRepo subordinateRepo;
        private readonly IMapper mapper;
        public SubordinateController(SubordinateRepo _subordinateRepo, IMapper mapper)
        {
            subordinateRepo = _subordinateRepo;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subordinates = mapper.Map<List<SubordinateDto>>(await subordinateRepo.GetSubordinatesAsync());

            return Ok(subordinates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubordinate(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Subordinate? subordinate = await subordinateRepo.GetSubordinateAsync(id);
            if (subordinate is not null)
            {
                var subordinateDto = mapper.Map<SubordinateDto>(subordinate);
                return Ok(subordinateDto);
            }

            return NotFound("No such subordinate");
        }

        [HttpGet("byChief")]
        public async Task<IActionResult> GetSubordinatesByChiefId(int chiefId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subordinates = mapper.Map<List<SubordinateDto>>(await subordinateRepo.GetSubordinatesByUserAsync(chiefId));
            return Ok(subordinates);
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateSubordinate(int id, string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await subordinateRepo.UpdateSubordinateAsync(id, name))
            {
                ModelState.AddModelError("", "Unable to update car");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated");
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSubordinate(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await subordinateRepo.DeleteSubordinateAsync(id))
            {
                ModelState.AddModelError("", "Unable to delete subordinate");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted");
        }

        [HttpPost("connectToCar")]
        public async Task<IActionResult> ConnectToCar(int subordinateId, int carId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await subordinateRepo.ConnectSubordinateToCar(subordinateId, carId))
            {
                ModelState.AddModelError("", "Unable to connect subordinate to car");
                return StatusCode(500, ModelState);
            }

            return Ok("Connected");
        }

        [HttpPost("disconnectFromCar")]
        public async Task<IActionResult> DisconnectFromCar(int subordinateId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await subordinateRepo.DisconnectSubordinateFromCar(subordinateId))
            {
                ModelState.AddModelError("", "Unable to disconnect subordinate from car");
                return StatusCode(500, ModelState);
            }

            return Ok("Disconnected");
        }

        [HttpPost("changeLng")]
        public async Task<IActionResult> ChangeSubordinateLanguage(int id, string lng)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string returnedLang = await subordinateRepo.ChangeLanguage(id, lng);

            if (returnedLang == "fail")
                return BadRequest("Fail or no such subordinate");

            return Ok(returnedLang);
        }
    }
}
