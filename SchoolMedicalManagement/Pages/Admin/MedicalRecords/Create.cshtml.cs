using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.DTOs.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.MedicalRecords
{
    // [Authorize(Policy = "MedicalStaff")] - Temporarily disabled for testing
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CreateMedicalRecordViewModel MedicalRecord { get; set; } = new CreateMedicalRecordViewModel();

        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            // Check authorization - Admin, Manager, or SchoolNurse only
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Manager" && userRole != "SchoolNurse")
            {
                return RedirectToPage("/Auth/Login");
            }

            await LoadStudentsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check authorization - Admin, Manager, or SchoolNurse only
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Manager" && userRole != "SchoolNurse")
            {
                return RedirectToPage("/Auth/Login");
            }

            if (!ModelState.IsValid)
            {
                await LoadStudentsAsync();
                return Page();
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

                var createDto = new CreateMedicalRecordDto
                {
                    StudentId = MedicalRecord.StudentId,
                    Allergies = MedicalRecord.Allergies,
                    ChronicDiseases = MedicalRecord.ChronicDiseases,
                    TreatmentHistory = MedicalRecord.TreatmentHistory,
                    PhysicalCondition = MedicalRecord.PhysicalCondition
                };

                var jsonContent = JsonSerializer.Serialize(createDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("MedicalRecord", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ErrorMessage = "A medical record already exists for this student.";
                    }
                    else
                    {
                        ErrorMessage = $"Unable to create medical record. Status: {response.StatusCode}, Error: {errorContent}";
                    }
                    await LoadStudentsAsync();
                    return Page();
                }

                SuccessMessage = "Medical record created successfully!";
                return RedirectToPage("/Admin/MedicalRecords/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                await LoadStudentsAsync();
                return Page();
            }
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Get current user token from cookie
                var token = Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Get all students
                var response = await client.GetAsync("Student");
                if (response.IsSuccessStatusCode)
                {
                    var studentsJson = await response.Content.ReadAsStringAsync();
                    var allStudents = JsonSerializer.Deserialize<List<StudentDto>>(studentsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (allStudents != null)
                    {
                        // Get all existing medical records to filter out students who already have records
                        var recordsResponse = await client.GetAsync("MedicalRecord");
                        if (recordsResponse.IsSuccessStatusCode)
                        {
                            var recordsJson = await recordsResponse.Content.ReadAsStringAsync();
                            var existingRecords = JsonSerializer.Deserialize<List<MedicalRecordDto>>(recordsJson, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            var existingStudentIds = existingRecords?.Select(r => r.StudentId).ToHashSet() ?? new HashSet<int>();

                            // Only show students who don't have medical records yet
                            Students = allStudents.Where(s => !existingStudentIds.Contains(s.Id)).ToList();
                        }
                        else
                        {
                            Students = allStudents;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Students = new List<StudentDto>();
            }
        }
    }

    public class CreateMedicalRecordViewModel
    {
        [Required(ErrorMessage = "Please select a student.")]
        public int StudentId { get; set; }

        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? TreatmentHistory { get; set; }
        public string? PhysicalCondition { get; set; }
    }
} 