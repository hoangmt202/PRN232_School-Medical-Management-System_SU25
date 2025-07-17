using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.HealthChecks
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateHealthCheckDto HealthCheck { get; set; } = new UpdateHealthCheckDto();
        
        [BindProperty]
        public int HealthCheckId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                HealthCheckId = id;
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/healthcheck/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var healthCheck = JsonSerializer.Deserialize<HealthCheckDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (healthCheck != null)
                    {
                        HealthCheck = new UpdateHealthCheckDto
                        {
                            Date = healthCheck.Date,
                            CheckType = healthCheck.CheckType,
                            Results = healthCheck.Results,
                            Notes = healthCheck.Notes
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
                var json = JsonSerializer.Serialize(HealthCheck);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync($"http://localhost:5234/api/healthcheck/{HealthCheckId}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật kiểm tra sức khỏe thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi khi cập nhật kiểm tra sức khỏe: {errorContent}");
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