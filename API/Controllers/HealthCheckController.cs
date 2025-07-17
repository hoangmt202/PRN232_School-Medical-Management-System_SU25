using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;
        private readonly IStudentService _studentService;
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(IHealthCheckService healthCheckService, IStudentService studentService, ILogger<HealthCheckController> logger)
        {
            _healthCheckService = healthCheckService;
            _studentService = studentService;
            _logger = logger;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthCheckDto>>> GetAll()
        {
            try
            {
                var healthChecks = await _healthCheckService.GetAllHealthChecksAsync();
                return Ok(healthChecks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HealthCheckDto>> GetById(int id)
        {
            try
            {
                var healthCheck = await _healthCheckService.GetHealthCheckByIdAsync(id);
                if (healthCheck == null)
                    return NotFound(new { message = "Health check not found" });
                return Ok(healthCheck);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<HealthCheckDto>>> GetByStudentId(int studentId)
        {
            try
            {
                var healthChecks = await _healthCheckService.GetHealthChecksByStudentIdAsync(studentId);
                return Ok(healthChecks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("by-parent")]
        public async Task<ActionResult<IEnumerable<HealthCheckDto>>> GetByParent()
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Invalid or missing user ID.");
                }

                // Get parent's children
                var students = await _studentService.GetStudentsByParentUserIdAsync(userId);
                
                if (!students.Any())
                {
                    return Ok(new List<HealthCheckDto>());
                }

                var childrenIds = students.Select(s => s.Id).ToList();
                var allHealthChecks = await _healthCheckService.GetAllHealthChecksAsync();
                
                var parentHealthChecks = allHealthChecks
                    .Where(hc => childrenIds.Contains(hc.StudentId))
                    .OrderByDescending(hc => hc.Date);

                return Ok(parentHealthChecks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<HealthCheckDto>> Create([FromBody] CreateHealthCheckDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdHealthCheck = await _healthCheckService.CreateHealthCheckAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = createdHealthCheck.Id }, createdHealthCheck);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HealthCheckDto>> Update(int id, [FromBody] UpdateHealthCheckDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedHealthCheck = await _healthCheckService.UpdateHealthCheckAsync(id, updateDto);
                if (updatedHealthCheck == null)
                    return NotFound(new { message = "Health check not found" });
                return Ok(updatedHealthCheck);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _healthCheckService.DeleteHealthCheckAsync(id);
                if (!result)
                    return NotFound(new { message = "Health check not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
} 