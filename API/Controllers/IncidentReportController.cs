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
    public class IncidentReportController : ControllerBase
    {
        private readonly IIncidentReportService _incidentReportService;

        public IncidentReportController(IIncidentReportService incidentReportService)
        {
            _incidentReportService = incidentReportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentReportDto>>> GetAll()
        {
            try
            {
                var incidentReports = await _incidentReportService.GetAllIncidentReportsAsync();
                return Ok(incidentReports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentReportDto>> GetById(int id)
        {
            try
            {
                var incidentReport = await _incidentReportService.GetIncidentReportByIdAsync(id);
                if (incidentReport == null)
                    return NotFound(new { message = "Incident report not found" });
                return Ok(incidentReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("nurse/{nurseId}")]
        public async Task<ActionResult<IEnumerable<IncidentReportDto>>> GetByNurseId(int nurseId)
        {
            try
            {
                var incidentReports = await _incidentReportService.GetIncidentReportsByNurseIdAsync(nurseId);
                return Ok(incidentReports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<IncidentReportDto>>> GetByStudentId(int studentId)
        {
            try
            {
                var incidentReports = await _incidentReportService.GetIncidentReportsByStudentIdAsync(studentId);
                return Ok(incidentReports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<IncidentReportDto>> Create([FromBody] CreateIncidentReportDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdIncidentReport = await _incidentReportService.CreateIncidentReportAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = createdIncidentReport.Id }, createdIncidentReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IncidentReportDto>> Update(int id, [FromBody] UpdateIncidentReportDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedIncidentReport = await _incidentReportService.UpdateIncidentReportAsync(id, updateDto);
                if (updatedIncidentReport == null)
                    return NotFound(new { message = "Incident report not found" });
                return Ok(updatedIncidentReport);
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
                var result = await _incidentReportService.DeleteIncidentReportAsync(id);
                if (!result)
                    return NotFound(new { message = "Incident report not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
} 