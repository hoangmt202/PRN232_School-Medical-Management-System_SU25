using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolNurseController : ControllerBase
    {
        private readonly ISchoolNurseService _schoolNurseService;

        public SchoolNurseController(ISchoolNurseService schoolNurseService)
        {
            _schoolNurseService = schoolNurseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SchoolNurseResponseDTO>>> GetAll()
        {
            try
            {
                var nurses = await _schoolNurseService.GetAll();
                return Ok(nurses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolNurseResponseDTO>> GetById(int id)
        {
            try
            {
                var nurse = await _schoolNurseService.GetSchoolNurseById(id);
                if (nurse == null)
                    return NotFound(new { message = "School nurse not found" });
                return Ok(nurse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SchoolNurseResponseDTO>> Create([FromBody] SchoolNurseRequestDTO nurseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdNurse = await _schoolNurseService.CreateSchoolNurse(nurseDTO);
                return CreatedAtAction(nameof(GetById), new { id = createdNurse.Id }, createdNurse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SchoolNurseResponseDTO>> Update(int id, [FromBody] SchoolNurseRequestDTO nurseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedNurse = await _schoolNurseService.Update(id, nurseDTO);
                return Ok(updatedNurse);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SchoolNurseResponseDTO>> Delete(int id)
        {
            try
            {
                var deletedNurse = await _schoolNurseService.Delete(id);
                return Ok(deletedNurse);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
} 