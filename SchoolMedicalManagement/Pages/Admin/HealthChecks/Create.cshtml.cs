using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.HealthChecks
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CreateHealthCheckDto HealthCheck { get; set; } = new CreateHealthCheckDto();

        public List<StudentResponseDTO> Students { get; set; } = new List<StudentResponseDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Block Parent from creating health checks
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Parent")
            {
                return RedirectToPage("/Admin/HealthChecks/Index");
            }
            
            await LoadStudents();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Block Parent from creating health checks
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Parent")
            {
                return RedirectToPage("/Admin/HealthChecks/Index");
            }
            
            if (!ModelState.IsValid)
            {
                await LoadStudents();
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(HealthCheck);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/healthcheck", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Tạo kiểm tra sức khỏe thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi khi tạo kiểm tra sức khỏe: {errorContent}");
                    await LoadStudents();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                await LoadStudents();
                return Page();
            }
        }

        private async Task LoadStudents()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5234/api/student");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<StudentResponseDTO>();
                }
            }
            catch (Exception ex)
            {
                Students = new List<StudentResponseDTO>();
            }
        }
    }
} 