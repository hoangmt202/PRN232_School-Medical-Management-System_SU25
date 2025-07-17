using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.DTOs.Medication;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public class AddModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public MedicationSubmissionViewModel Medication { get; set; } = new MedicationSubmissionViewModel();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadStudentsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadStudentsAsync();
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var createDto = new CreateMedicationDto
                {
                    StudentId = Medication.StudentId,
                    MedicationName = Medication.MedicationName,
                    Dosage = Medication.Dosage,
                    Frequency = Medication.Frequency
                };

                var jsonContent = JsonSerializer.Serialize(createDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("medication", content);
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Medication submission successful! The school nurse will review and manage the medication.";
                    // Clear form
                    Medication = new MedicationSubmissionViewModel();
                    await LoadStudentsAsync();
                }
                else
                {
                    ErrorMessage = "Failed to submit medication. Please try again.";
                    await LoadStudentsAsync();
                }

                return Page();
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
                
                // Get students for the current parent using by-parent endpoint
                var studentsResponse = await client.GetAsync("student/by-parent");
                if (studentsResponse.IsSuccessStatusCode)
                {
                    var studentsJson = await studentsResponse.Content.ReadAsStringAsync();
                    var students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(studentsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (students != null)
                    {
                        Medication.Students = students.Select(s => new StudentOption
                        {
                            Id = s.Id,
                            Name = s.FullName,
                            Class = s.Class ?? "N/A"
                        }).ToList();
                    }
                }
                else
                {
                    Medication.Students = new List<StudentOption>();
                }
            }
            catch (Exception)
            {
                Medication.Students = new List<StudentOption>();
            }
        }
    }
}