using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.DTOs.SchoolNurse;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.Inventory
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IInventoryService _drugStorageService;
        private readonly ISchoolNurseService _schoolNurseService;

        public IndexModel(IInventoryService drugStorageService, ISchoolNurseService schoolNurseService)
        {
            _drugStorageService = drugStorageService;
            _schoolNurseService = schoolNurseService;
        }

        public List<DrugStorageDto> Drugs { get; set; } = new();
        public List<InventoryAlertDto> Alerts { get; set; } = new();
        public List<SchoolNurseDto> Nurses { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            try
            {
                Drugs = (await _drugStorageService.GetAllDrugsAsync()).ToList();
                Alerts = await _drugStorageService.GetInventoryAlertsAsync();
                Nurses = (await _schoolNurseService.GetAllSchoolNursesAsync()).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load inventory: " + ex.Message;
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var dto = JsonSerializer.Deserialize<CreateDrugStorageDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.MedicationName) ||
                dto.Quantity <= 0 ||
                dto.ManagedBy <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            await _drugStorageService.AddDrugAsync(dto);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var dto = JsonSerializer.Deserialize<UpdateDrugStorageDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.MedicationName) ||
                dto.Quantity <= 0 ||
                dto.ManagedBy <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            await _drugStorageService.UpdateDrugAsync(dto.Id, dto);
            return new JsonResult(new { success = true });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {

                var deleted = await _drugStorageService.DeleteDrugAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return new JsonResult(new { success = true });
        }                
    }
}
