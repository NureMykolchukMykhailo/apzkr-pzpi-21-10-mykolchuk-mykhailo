using Microsoft.AspNetCore.Mvc;
using APZ_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using APZ_backend.DTO;


namespace APZ_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly SensorRepo sensorRepo;
        private IMapper mapper;
        public SensorController(SensorRepo _sensorRepo, IMapper mapper)
        {
            sensorRepo = _sensorRepo;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sensors = mapper.Map<List<SensorDto>>(await sensorRepo.GetSensorsAsync());

            return Ok(sensors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensor(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Sensor? sensor = await sensorRepo.GetSensorAsync(id);
            if (sensor is not null)
            {
                var sensorDto = mapper.Map<SensorDto>(sensor);
                return Ok(sensorDto);

            }
            return NotFound("No such sensor");
        }

        [HttpGet("byOwner")]
        public async Task<IActionResult> GetSensorsByOwnerId(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sensors = mapper.Map<List<SensorDto>>(await sensorRepo.GetSensorsByUserAsync(ownerId));
            return Ok(sensors);
        }

        [HttpGet("freebyOwner")]
        public async Task<IActionResult> GetFreeSensorsByOwnerId(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sensors = mapper.Map<List<SensorDto>>(await sensorRepo.GetFreeSensorsByUserId(ownerId));
            return Ok(sensors);
        }

        [HttpPost("insert")]
        [Authorize]
        public async Task<IActionResult> InsertSensor(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!await sensorRepo.InsertSensorAsync(
                new Sensor()
                {
                    Name = name,
                    OwnerId = ownerId,
                    CarId = null
                }
            ))
            {
                ModelState.AddModelError("", "Unable to insert new sensor");
                return StatusCode(500, ModelState);
            }

            return Ok("Object created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSensor(int id, string newName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await sensorRepo.UpdateSensorAsync(id, newName))
            {
                ModelState.AddModelError("", "Unable to update sensor");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated");
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await sensorRepo.DeleteSensorAsync(id))
            {
                ModelState.AddModelError("", "Unable to delete sensor");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted");
        }

        [HttpPost("connectToCar")]
        public async Task<IActionResult> ConnectToCar(int sensorId, int carId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await sensorRepo.ConnectSensorToCar(carId, sensorId))
            {
                ModelState.AddModelError("", "Unable to connect sensor to car");
                return StatusCode(500, ModelState);
            }

            return Ok("Connected");
        }

        [HttpPost("disconnectFromCar")]
        public async Task<IActionResult> DisconnectFromCar(int carId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await sensorRepo.DisconnectSensorFromCar(carId))
            {
                ModelState.AddModelError("", "Unable to disconnect sensor from car");
                return StatusCode(500, ModelState);
            }

            return Ok("Disconnected");
        }
    }
}
