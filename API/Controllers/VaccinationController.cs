using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController : ControllerBase
    {
        private readonly IVaccinationParentService _vaccinationService;
        private readonly ISchoolNurseService _schoolNurseService;
        private readonly IParentService _parentService;

        public VaccinationController(IVaccinationParentService vaccinationService, ISchoolNurseService schoolNurseService, IParentService parentService)
        {
            _vaccinationService = vaccinationService;
            _schoolNurseService = schoolNurseService;
            _parentService = parentService;
        }
        [HttpGet("VaccinationPlan")]
        public async Task<ActionResult<IEnumerable<VaccinationPlanDto>>> GetAllPlans()
        {
            var plans = await _vaccinationService.GetAllPlansAsync();
            return Ok(plans);
        }

        // GET: api/vaccinationplan/{id}
        [HttpGet("VaccinationPlan/{id}")]
        public async Task<ActionResult<VaccinationPlanDto>> GetPlanById(int id)
        {
            var plan = await _vaccinationService.GetByIdAsync(id);
            if (plan == null)
            {
                return NotFound(new { message = $"Vaccination plan with ID {id} not found." });
            }
            return Ok(plan);
        }
        [HttpPost("result-note")]
        public async Task<IActionResult> AddResultNote([FromBody] ResultNoteDto dto)
        {
            var success = await _vaccinationService.AddResultNoteAsync(dto.VaccinationId, dto.Note);
            if (!success)
                return BadRequest(new { message = "Could not save note." });

            return Ok(new { success = true });
        }
        [HttpPost("mark-status")]
        public async Task<IActionResult> MarkVaccinationStatus([FromBody] VaccinationStatusUpdateDto dto)
        {
            var success = await _vaccinationService.UpdateVaccinationStatusAsync(dto.StudentId, dto.PlanId, dto.Status, dto.DateGiven);
            return success ? Ok(new { success = true }) : NotFound(new { error = "Vaccination not found." });
        }
        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] List<VaccinationStatusUpdateDto> updates)
        {
            foreach (var dto in updates)
            {
                await _vaccinationService.UpdateVaccinationStatusAsync(dto.StudentId, dto.PlanId, dto.Status, dto.DateGiven);
            }

            return Ok(new { success = true });
        }
        // GET: api/Vaccination/Upcoming?parentId=1
        [HttpGet("Upcoming")]
        public async Task<IActionResult> GetUpcomingPlans()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid or missing user ID.");
            }

            var parent = await _parentService.GetParentByUserIdAsync(userId);
            var plans = await _vaccinationService.GetUpcomingPlansAsync(parent.Id);
            return Ok(plans);
        }

        // POST: api/Vaccination/Consent
        [HttpPost("Consent")]
        public async Task<IActionResult> SubmitConsent([FromBody] ConsentRequest request)
        {
            var success = await _vaccinationService.SubmitConsentAsync(request.StudentId, request.PlanId, request.Response);
            if (!success) return NotFound("Consent notice not found.");
            return Ok(new { success = true });
        }

        // GET: api/Vaccination/History?parentId=1
        [HttpGet("History")]
        public async Task<IActionResult> GetHistory()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid or missing user ID.");
            }

            var parent = await _parentService.GetParentByUserIdAsync(userId);
            var history = await _vaccinationService.GetStudentVaccinationHistoryAsync(parent.Id);
            return Ok(history);
        }
        [HttpGet("nurse/plans")]
        public async Task<IActionResult> GetPlansByNurse()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid or missing user ID.");
            }

            var nurse = await _schoolNurseService.GetSchoolNurseByUserIdAsync(userId);
            var plans = await _vaccinationService.GetPlansByNurseAsync(nurse.Id);
            return Ok(plans);
        }
        [HttpPost("{planId}/assign-nurse")]
        public async Task<IActionResult> AssignNurseToPlan(int planId, [FromBody] int nurseId)
        {
            var success = await _vaccinationService.AssignNurseToPlanAsync(planId, nurseId);
            if (!success)
                return NotFound(new { message = "Plan not found" });

            return Ok(new { success = true });
        }
        [HttpPost("send-notices")]
        public async Task<IActionResult> SendNotices(int planId)
        {
            var result = await _vaccinationService.SendNoticesForPlanAsync(planId);
            return Ok(new { success = result });
        }
        [HttpGet("plan/{planId}/students")]
        public async Task<IActionResult> GetStudentsByPlan(int planId)
        {
            var result = await _vaccinationService.GetStudentsByPlanAsync(planId);
            return Ok(result);
        }

        [HttpGet("parent/history")]
        public async Task<IActionResult> GetParentVaccinationHistory()
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid or missing user ID.");
            }

            var parent = await _parentService.GetParentByUserIdAsync(userId);
            var result = await _vaccinationService.GetStudentVaccinationHistoryAsync(parent.Id);
            return Ok(result);
        }

        [HttpPost("external")]
        public async Task<IActionResult> AddExternalVaccination([FromBody] ExternalVaccinationDto dto)
        {
            var success = await _vaccinationService.AddExternalVaccinationAsync(dto);
            if (!success)
                return BadRequest("Failed to save vaccination.");
            return Ok(new { success = true });
        }
    }
}

