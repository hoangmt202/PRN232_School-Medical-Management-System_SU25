using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.HealthChecks
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HealthCheckDto HealthCheck { get; set; } = new HealthCheckDto();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Block Parent from deleting health checks
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Parent")
            {
                return RedirectToPage("/Admin/HealthChecks/Index");
            }
            
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/healthcheck/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    HealthCheck = JsonSerializer.Deserialize<HealthCheckDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (HealthCheck != null)
                    {
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Block Parent from deleting health checks
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Parent")
            {
                return RedirectToPage("/Admin/HealthChecks/Index");
            }
            
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"http://localhost:5234/api/healthcheck/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa kiểm tra sức khỏe thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi khi xóa kiểm tra sức khỏe!";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
} 