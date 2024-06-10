using Microsoft.AspNetCore.Mvc;
using APZ_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using APZ_backend.DTO;

namespace APZ_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarRepo carRepo;
        private IMapper mapper;
        public CarController(CarRepo _carRepo, IMapper mapper)
        {
            carRepo = _carRepo;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cars = mapper.Map<List<CarDto>>(await carRepo.GetCarsAsync());

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Car? car = await carRepo.GetCarAsync(id);
            if (car is not null)
            {
                var carDto = mapper.Map<CarDto>(car);
                return Ok(carDto);
            }

            return NotFound("No such car");
        }

        [HttpGet("byOwner")]
        public async Task<IActionResult> GetCarsByOwnerId(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cars = mapper.Map<List<CarDto>>(await carRepo.GetCarsByUserAsync(ownerId));
            return Ok(cars);
        }

        [HttpPost("insert")]
        [Authorize]
        public async Task<IActionResult> InsertCar(string carName, string type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!await carRepo.InsertCarAsync(
                new Car()
                {
                    OwnerId = ownerId,
                    Type = type,
                    Name = carName
                }
            ))
            {
                ModelState.AddModelError("", "Unable to insert new car");
                return StatusCode(500, ModelState);
            }

            return Ok("Object created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCar([FromForm] Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await carRepo.UpdateCarAsync(car))
            {
                ModelState.AddModelError("", "Unable to update car");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated");
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await carRepo.DeleteCarAsync(id))
            {
                ModelState.AddModelError("", "Unable to delete car");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted");
        }
    }
}
