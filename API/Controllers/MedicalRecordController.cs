using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        // GET: api/MedicalRecord
        [HttpGet]
        //[Authorize(Policy = "MedicalStaff")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetAll()
        {
            try
            {
                var medicalRecords = await _medicalRecordService.GetAllMedicalRecordsAsync();
                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/MedicalRecord/{id}
        [HttpGet("{id}")]
        //[Authorize(Policy = "MedicalStaff")]
        public async Task<ActionResult<MedicalRecordDto>> GetById(int id)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
                if (medicalRecord == null)
                {
                    return NotFound($"Medical record with ID {id} not found.");
                }
                return Ok(medicalRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/MedicalRecord/student/{studentId}
        [HttpGet("student/{studentId}")]
        //[Authorize(Policy = "MedicalStaff")]
        public async Task<ActionResult<MedicalRecordDto>> GetByStudentId(int studentId)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.GetMedicalRecordByStudentIdAsync(studentId);
                if (medicalRecord == null)
                {
                    return NotFound($"Medical record for student ID {studentId} not found.");
                }
                return Ok(medicalRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/MedicalRecord
        [HttpPost]
        //[Authorize(Policy = "MedicalStaff")]
        public async Task<ActionResult<MedicalRecordDto>> Create([FromBody] CreateMedicalRecordDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var medicalRecord = await _medicalRecordService.CreateMedicalRecordAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = medicalRecord.Id }, medicalRecord);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/MedicalRecord/{id}
        [HttpPut("{id}")]
        //[Authorize(Policy = "MedicalStaff")]
        public async Task<ActionResult<MedicalRecordDto>> Update(int id, [FromBody] UpdateMedicalRecordDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedRecord = await _medicalRecordService.UpdateMedicalRecordAsync(id, updateDto);
                if (updatedRecord == null)
                {
                    return NotFound($"Medical record with ID {id} not found.");
                }

                return Ok(updatedRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/MedicalRecord/{id}
        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _medicalRecordService.DeleteMedicalRecordAsync(id);
                if (!result)
                {
                    return NotFound($"Medical record with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 