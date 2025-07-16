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
    public class HealthCheckController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
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