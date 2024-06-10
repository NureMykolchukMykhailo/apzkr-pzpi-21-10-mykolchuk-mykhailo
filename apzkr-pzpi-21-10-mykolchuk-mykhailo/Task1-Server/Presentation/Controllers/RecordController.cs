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
    public class RecordController : ControllerBase
    {
        private readonly RecordRepo recordRepo;
        private IMapper mapper;
        public RecordController(RecordRepo _recordRepo, IMapper _mapper)
        {
            recordRepo = _recordRepo;
            mapper = _mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var records = mapper.Map<List<RecordDto>>(await recordRepo.GetRecordsAsync());

            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecord(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Record? record = await recordRepo.GetRecordAsync(id);
            if (record is not null)
            {
                RecordDto recordDto = mapper.Map<RecordDto>(record);
                return Ok(recordDto);
            }
                
            return NotFound("No such record");
        }

        [HttpGet("byCar")]
        public async Task<IActionResult> GetRecordsByCarId(int carId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var records = mapper.Map<List<RecordDto>>(await recordRepo.GetRecordsByCarAsync(carId));
            return Ok(records);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertRecord(RecordDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Record record = mapper.Map<Record>(recordDto);
            if (!await recordRepo.InsertRecordAsync(record))
            {
                ModelState.AddModelError("", "Unable to insert new record");
                return StatusCode(500, ModelState);
            }

            return Ok("Object created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRecord(int recordId, [FromForm] Record record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await recordRepo.UpdateRecordAsync(record))
            {
                ModelState.AddModelError("", "Unable to update record");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated");
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await recordRepo.DeleteRecordAsync(id))
            {
                ModelState.AddModelError("", "Unable to delete record");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted");
        }
    }
}
