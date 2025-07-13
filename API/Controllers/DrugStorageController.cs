using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.DTOs.Medication;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugStorageController : ControllerBase
    {
        private readonly IInventoryService _drugService;
        private readonly ILogger<DrugStorageController> _logger;

        public DrugStorageController(IInventoryService drugService, ILogger<DrugStorageController> logger)
        {
            _drugService = drugService;
            _logger = logger;
        }

        // ============ CRUD ==================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drugs = await _drugService.GetAllDrugsAsync();
            return Ok(drugs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var drug = await _drugService.GetDrugByIdAsync(id);
            return drug == null ? NotFound() : Ok(drug);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrugStorageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _drugService.AddDrugAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDrugStorageDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid update data");

            var updated = await _drugService.UpdateDrugAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _drugService.DeleteDrugAsync(id);
            return success ? NoContent() : NotFound();
        }

        // ========== Stock Management ==========

        [HttpPost("stock/update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockDto dto)
        {
            var result = await _drugService.UpdateStockAsync(dto);
            return result ? Ok() : BadRequest("Update failed");
        }

        [HttpPost("stock/dispense")]
        public async Task<IActionResult> Dispense([FromBody] DispenseMedicationDto dto)
        {
            var result = await _drugService.DispenseMedicationAsync(dto);
            return result ? Ok() : BadRequest("Dispense failed");
        }

        [HttpPost("stock/receive")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockDto dto)
        {
            var result = await _drugService.ReceiveStockAsync(dto);
            return result ? Ok() : BadRequest("Receive failed");
        }

        // ========== Monitoring & Analytics ==========

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpired()
        {
            var drugs = await _drugService.GetExpiredDrugsAsync();
            return Ok(drugs);
        }

        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiring([FromQuery] int daysAhead = 30)
        {
            var drugs = await _drugService.GetExpiringDrugsAsync(daysAhead);
            return Ok(drugs);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        {
            var drugs = await _drugService.GetLowStockDrugsAsync(threshold);
            return Ok(drugs);
        }

        [HttpGet("by-location")]
        public async Task<IActionResult> GetByLocation([FromQuery] string location)
        {
            var drugs = await _drugService.GetDrugsByLocationAsync(location);
            return Ok(drugs);
        }

        [HttpGet("by-nurse/{nurseId}")]
        public async Task<IActionResult> GetByNurse(int nurseId)
        {
            var drugs = await _drugService.GetDrugsByNurseAsync(nurseId);
            return Ok(drugs);
        }

        // ========== Alerts ==========

        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _drugService.GetInventoryAlertsAsync();
            return Ok(alerts);
        }

        [HttpPost("alerts/{alertId}/read")]
        public async Task<IActionResult> MarkAlertAsRead(int alertId)
        {
            var success = await _drugService.MarkAlertAsReadAsync(alertId);
            return success ? Ok() : NotFound();
        }

        [HttpGet("alerts/pending-count")]
        public async Task<IActionResult> GetPendingAlertsCount()
        {
            var count = await _drugService.GetPendingAlertsCountAsync();
            return Ok(count);
        }

        // ========== Reports ==========

        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            var report = await _drugService.GenerateInventoryReportAsync();
            return Ok(report);
        }

        [HttpGet("stock-movement/{drugId}")]
        public async Task<IActionResult> GetStockMovement(int drugId)
        {
            var movements = await _drugService.GetStockMovementHistoryAsync(drugId);
            return Ok(movements);
        }

        // ========== Utilities ==========

        [HttpGet("names")]
        public async Task<IActionResult> GetMedicationNames()
        {
            var names = await _drugService.GetMedicationNamesAsync();
            return Ok(names);
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetStorageLocations()
        {
            var locations = await _drugService.GetStorageLocationsAsync();
            return Ok(locations);
        }
    }
}

