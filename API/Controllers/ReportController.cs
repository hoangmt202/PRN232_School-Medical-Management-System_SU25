using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "AdminOnly")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("health-trends")]
        public async Task<IActionResult> GetHealthTrends()
        {
            var report = await _reportService.GetHealthTrendReportAsync();
            return Ok(report);
        }
        [HttpGet("incidents")]
        public async Task<IActionResult> GetIncidentReport()
        {
            var result = await _reportService.GetIncidentStatsAsync();
            return Ok(result);
        }
        [HttpGet("immunization")]
        public async Task<IActionResult> GetImmunizationCoverage()
        {
            return Ok(await _reportService.GetImmunizationCoverageAsync());
        }
        [HttpGet("parental-responses")]
        public async Task<IActionResult> GetParentalResponseReport()
        {
            return Ok(await _reportService.GetParentalResponseStatsAsync());
        }
    }
}
