using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.HealthChecks
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public HealthCheckEditModel HealthCheck { get; set; } = new HealthCheckEditModel();

        public string StudentName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/HealthCheck/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var healthCheck = JsonSerializer.Deserialize<HealthCheckDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (healthCheck != null)
                    {
                        HealthCheck.Id = healthCheck.Id;
                        HealthCheck.StudentId = healthCheck.StudentId;
                        HealthCheck.Date = healthCheck.Date;
                        HealthCheck.CheckType = healthCheck.CheckType;
                        HealthCheck.Results = healthCheck.Results;
                        HealthCheck.Notes = healthCheck.Notes;

                        StudentName = healthCheck.Student?.FullName ?? "Unknown Student";
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var healthCheckDTO = new UpdateHealthCheckDto
                {
                    Date = HealthCheck.Date,
                    CheckType = HealthCheck.CheckType,
                    Results = HealthCheck.Results,
                    Notes = HealthCheck.Notes
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(healthCheckDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync($"http://localhost:5234/api/HealthCheck/{HealthCheck.Id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the health check. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the health check. Please try again.");
                return Page();
            }
        }
    }

    public class HealthCheckEditModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Check type is required")]
        public string CheckType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Results are required")]
        [StringLength(500, ErrorMessage = "Results cannot exceed 500 characters")]
        public string Results { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Notes cannot exceed 300 characters")]
        public string? Notes { get; set; }
    }
} 