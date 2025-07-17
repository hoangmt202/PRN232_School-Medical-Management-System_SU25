using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.IncidentReports
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateIncidentReportDto IncidentReport { get; set; } = new UpdateIncidentReportDto();
        
        [BindProperty]
        public int IncidentReportId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                IncidentReportId = id;
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/incidentreport/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var incidentReport = JsonSerializer.Deserialize<IncidentReportDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (incidentReport != null)
                    {
                        IncidentReport = new UpdateIncidentReportDto
                        {
                            Date = incidentReport.Date,
                            Type = incidentReport.Type,
                            Description = incidentReport.Description,
                            ActionTaken = incidentReport.ActionTaken
                        };
                        return Page();
                    }
                }
                
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(IncidentReport);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync($"http://localhost:5234/api/incidentreport/{IncidentReportId}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật báo cáo sự cố thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi khi cập nhật báo cáo sự cố: {errorContent}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                return Page();
            }
        }
    }
} 