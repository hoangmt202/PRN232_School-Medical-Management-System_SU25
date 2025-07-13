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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IParentService _parentService;

        public StudentController(IStudentService studentService, IParentService parentService)
        {
            _studentService = studentService;
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentResponseDTO>>> GetAll()
        {
            try
            {
                var students = await _studentService.GetAll();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDTO>> GetById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                    return NotFound(new { message = "Student not found" });
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponseDTO>> Create([FromBody] StudentRequestDTO studentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdStudent = await _studentService.CreateStudent(studentDTO);
                return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentResponseDTO>> Update(int id, [FromBody] StudentRequestDTO studentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedStudent = await _studentService.Update(id, studentDTO);
                return Ok(updatedStudent);
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
        public async Task<ActionResult<StudentResponseDTO>> Delete(int id)
        {
            try
            {
                var deletedStudent = await _studentService.Delete(id);
                return Ok(deletedStudent);
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
        [HttpGet("by-parent")]
        public async Task<IActionResult> GetByParentUserId()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid or missing user ID.");
            }
            var students = await _studentService.GetStudentsByParentUserIdAsync(userId);
            if (!students.Any())
            {
                return NotFound("No students found for this parent user.");
            }

            return Ok(students);
        }
    }
} 