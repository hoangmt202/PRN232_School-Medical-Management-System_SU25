using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.HealthChecks
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public HealthCheckCreateModel HealthCheck { get; set; } = new HealthCheckCreateModel();

        public SelectList StudentOptions { get; set; } = new SelectList(new List<SelectListItem>());

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
                var healthCheckDTO = new CreateHealthCheckDto
                {
                    StudentId = HealthCheck.StudentId,
                    Date = HealthCheck.Date,
                    CheckType = HealthCheck.CheckType,
                    Results = HealthCheck.Results,
                    Notes = HealthCheck.Notes
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(healthCheckDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/HealthCheck", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while creating the health check. Please try again.");
                    await LoadDropdownData();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the health check. Please try again.");
                await LoadDropdownData();
                return Page();
            }
        }

        private async Task LoadDropdownData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Load students
                var studentsResponse = await client.GetAsync("http://localhost:5234/api/student");
                if (studentsResponse.IsSuccessStatusCode)
                {
                    var studentsContent = await studentsResponse.Content.ReadAsStringAsync();
                    var students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(studentsContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<StudentResponseDTO>();

                    StudentOptions = new SelectList(students, "Id", "FullName");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                StudentOptions = new SelectList(new List<SelectListItem>());
            }
        }
    }

    public class HealthCheckCreateModel
    {
        [Required(ErrorMessage = "Student is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Check type is required")]
        public string CheckType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Results are required")]
        [StringLength(500, ErrorMessage = "Results cannot exceed 500 characters")]
        public string Results { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Notes cannot exceed 300 characters")]
        public string? Notes { get; set; }
    }
} 