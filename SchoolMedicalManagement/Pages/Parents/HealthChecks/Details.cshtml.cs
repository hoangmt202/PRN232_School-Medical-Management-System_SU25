using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents.HealthChecks
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HealthCheckDto HealthCheck { get; set; } = new HealthCheckDto();
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
                
                // JWT token is automatically added by JwtTokenHandler
                
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
                    ErrorMessage = "Không tìm thấy con của bạn.";
                    return Page();
                }

                var childrenIds = students.Select(s => s.Id).ToList();

                // Get the specific health check
                var healthCheckResponse = await client.GetAsync($"healthcheck/{id}");
                if (!healthCheckResponse.IsSuccessStatusCode)
                {
                    ErrorMessage = "Không tìm thấy kết quả kiểm tra sức khỏe.";
                    return Page();
                }

                var healthCheckJson = await healthCheckResponse.Content.ReadAsStringAsync();
                var healthCheck = JsonSerializer.Deserialize<HealthCheckDto>(healthCheckJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (healthCheck == null)
                {
                    ErrorMessage = "Không tìm thấy kết quả kiểm tra sức khỏe.";
                    return Page();
                }

                // Security check: Ensure parent can only view their children's health checks
                if (!childrenIds.Contains(healthCheck.StudentId))
                {
                    ErrorMessage = "Bạn không có quyền xem kết quả này.";
                    return Page();
                }

                HealthCheck = healthCheck;
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