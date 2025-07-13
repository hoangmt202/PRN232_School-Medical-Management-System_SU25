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
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API"); 
        }
        public List<DrugStorageDto> Drugs { get; set; } = new();
        public List<InventoryAlertDto> Alerts { get; set; } = new();
        public List<SchoolNurseDto> Nurses { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            try
            {
                var drugsResponse = await _httpClient.GetAsync("drugstorage");
                if (drugsResponse.IsSuccessStatusCode)
                {
                    var json = await drugsResponse.Content.ReadAsStringAsync();
                    Drugs = JsonSerializer.Deserialize<List<DrugStorageDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }

                var alertsResponse = await _httpClient.GetAsync("drugstorage/alerts");
                if (alertsResponse.IsSuccessStatusCode)
                {
                    var json = await alertsResponse.Content.ReadAsStringAsync();
                    Alerts = JsonSerializer.Deserialize<List<InventoryAlertDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }

                var nursesResponse = await _httpClient.GetAsync("schoolnurse");
                if (nursesResponse.IsSuccessStatusCode)
                {
                    var json = await nursesResponse.Content.ReadAsStringAsync();
                    Nurses = JsonSerializer.Deserialize<List<SchoolNurseDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
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

            var response = await _httpClient.PostAsJsonAsync("drugstorage", dto);
            return response.IsSuccessStatusCode
                ? new JsonResult(new { success = true })
                : BadRequest("Failed to add drug.");
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

            var response = await _httpClient.PutAsJsonAsync($"drugstorage/{dto.Id}", dto);
            return response.IsSuccessStatusCode
                ? new JsonResult(new { success = true })
                : BadRequest("Failed to update drug.");
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"drugstorage/{id}");
            return response.IsSuccessStatusCode
                ? new JsonResult(new { success = true })
                : NotFound();
        }
    }
}
