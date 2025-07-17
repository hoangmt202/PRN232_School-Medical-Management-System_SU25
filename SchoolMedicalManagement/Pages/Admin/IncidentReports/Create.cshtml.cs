using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.IncidentReports
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CreateIncidentReportDto IncidentReport { get; set; } = new CreateIncidentReportDto();

        public List<StudentResponseDTO> Students { get; set; } = new List<StudentResponseDTO>();
        public List<SchoolNurseResponseDTO> Nurses { get; set; } = new List<SchoolNurseResponseDTO>();

        public async Task OnGetAsync()
        {
            await LoadDropdownData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData();
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(IncidentReport);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/incidentreport", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Tạo báo cáo sự cố thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi khi tạo báo cáo sự cố: {errorContent}");
                    await LoadDropdownData();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                await LoadDropdownData();
                return Page();
            }
        }

        private async Task LoadDropdownData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                
                // Load Students
                var studentsResponse = await client.GetAsync("http://localhost:5234/api/student");
                if (studentsResponse.IsSuccessStatusCode)
                {
                    var studentsContent = await studentsResponse.Content.ReadAsStringAsync();
                    Students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(studentsContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<StudentResponseDTO>();
                }

                // Load Nurses
                var nursesResponse = await client.GetAsync("http://localhost:5234/api/schoolnurse");
                if (nursesResponse.IsSuccessStatusCode)
                {
                    var nursesContent = await nursesResponse.Content.ReadAsStringAsync();
                    Nurses = JsonSerializer.Deserialize<List<SchoolNurseResponseDTO>>(nursesContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<SchoolNurseResponseDTO>();
                }
            }
            catch (Exception ex)
            {
                // Log error but continue
                Students = new List<StudentResponseDTO>();
                Nurses = new List<SchoolNurseResponseDTO>();
            }
        }
    }
} 