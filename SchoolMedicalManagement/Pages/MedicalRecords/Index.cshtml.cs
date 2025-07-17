using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.DTOs.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public partial class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            // Check authorization - Parent only
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Parent")
            {
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Get current user token from session
                var token = Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Get parent's children using JWT token
                var studentsResponse = await client.GetAsync("student/by-parent");
                if (!studentsResponse.IsSuccessStatusCode)
                {
                    ErrorMessage = "Unable to load student information.";
                    return Page();
                }

                var studentsJson = await studentsResponse.Content.ReadAsStringAsync();
                var students = JsonSerializer.Deserialize<List<StudentDto>>(studentsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (students == null || !students.Any())
                {
                    ErrorMessage = "No children found for this parent.";
                    return Page();
                }

                // Get medical records for each child
                foreach (var child in students)
                {
                    var medicalRecordResponse = await client.GetAsync($"MedicalRecord/student/{child.Id}");
                    if (medicalRecordResponse.IsSuccessStatusCode)
                    {
                        var recordJson = await medicalRecordResponse.Content.ReadAsStringAsync();
                        var medicalRecord = JsonSerializer.Deserialize<MedicalRecordDto>(recordJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (medicalRecord != null)
                        {
                            MedicalRecords.Add(new MedicalRecordViewModel
                            {
                                Id = medicalRecord.Id,
                                StudentId = child.Id,
                                StudentName = child.FullName,
                                StudentClass = child.Class ?? "N/A",
                                StudentDateOfBirth = child.DateOfBirth,
                                Allergies = medicalRecord.Allergies,
                                ChronicDiseases = medicalRecord.ChronicDiseases,
                                TreatmentHistory = medicalRecord.TreatmentHistory,
                                PhysicalCondition = medicalRecord.PhysicalCondition
                            });
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
