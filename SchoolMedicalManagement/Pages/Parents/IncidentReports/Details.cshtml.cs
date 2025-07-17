using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents.IncidentReports
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IncidentReportDto IncidentReport { get; set; } = new IncidentReportDto();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Only allow Parents
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Parent")
            {
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Get current user token from cookie
                var token = Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Get parent's children first to validate access
                var studentsResponse = await client.GetAsync("student/by-parent");
                if (!studentsResponse.IsSuccessStatusCode)
                {
                    ErrorMessage = "Không thể xác thực quyền truy cập.";
                    return Page();
                }

                var studentsJson = await studentsResponse.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(studentsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (students == null || !students.Any())
                {
                    ErrorMessage = "Không tìm thấy thông tin con em.";
                    return Page();
                }

                var childrenIds = students.Select(s => s.Id).ToList();

                // Get incident report details
                var incidentResponse = await client.GetAsync($"incidentreport/{id}");
                if (!incidentResponse.IsSuccessStatusCode)
                {
                    if (incidentResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ErrorMessage = "Không tìm thấy báo cáo sự cố.";
                        return Page();
                    }
                    ErrorMessage = "Không thể tải thông tin báo cáo sự cố.";
                    return Page();
                }

                var incidentJson = await incidentResponse.Content.ReadAsStringAsync();
                var incident = JsonSerializer.Deserialize<IncidentReportDto>(incidentJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (incident == null)
                {
                    ErrorMessage = "Không tìm thấy báo cáo sự cố.";
                    return Page();
                }

                // Security check: Ensure parent can only view their children's incident reports
                if (!childrenIds.Contains(incident.StudentId))
                {
                    ErrorMessage = "Bạn không có quyền xem báo cáo này.";
                    return Page();
                }

                IncidentReport = incident;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
                return Page();
            }
        }
    }
} 