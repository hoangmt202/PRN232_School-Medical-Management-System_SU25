using BusinessLogic.DTOs.MedicalRecord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    // No authorization needed - already checking in controller logic
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public EditMedicalRecordViewModel MedicalRecord { get; set; } = new EditMedicalRecordViewModel();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Authorization disabled for testing
            // var userRole = HttpContext.Session.GetString("UserRole");
            // if (userRole != "Parent" && userRole != "Admin" && userRole != "Manager" && userRole != "SchoolNurse")
            // {
            //     return RedirectToPage("/Auth/Login");
            // }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Get current user token from cookie
                var token = Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync($"MedicalRecord/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    ErrorMessage = "Unable to load medical record.";
                    return Page();
                }

                var recordJson = await response.Content.ReadAsStringAsync();
                var record = JsonSerializer.Deserialize<MedicalRecordDto>(recordJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (record == null)
                {
                    return NotFound();
                }

                MedicalRecord = new EditMedicalRecordViewModel
                {
                    Id = record.Id,
                    StudentId = record.StudentId,
                    StudentName = record.StudentName ?? "Unknown",
                    Allergies = record.Allergies,
                    ChronicDiseases = record.ChronicDiseases,
                    TreatmentHistory = record.TreatmentHistory,
                    PhysicalCondition = record.PhysicalCondition
                };

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Authorization disabled for testing
            // var userRole = HttpContext.Session.GetString("UserRole");
            // if (userRole != "Parent" && userRole != "Admin" && userRole != "Manager" && userRole != "SchoolNurse")
            // {
            //     return RedirectToPage("/Auth/Login");
            // }

            if (!ModelState.IsValid)
            {
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

                var updateDto = new UpdateMedicalRecordDto
                {
                    Allergies = MedicalRecord.Allergies,
                    ChronicDiseases = MedicalRecord.ChronicDiseases,
                    TreatmentHistory = MedicalRecord.TreatmentHistory,
                    PhysicalCondition = MedicalRecord.PhysicalCondition
                };

                var jsonContent = JsonSerializer.Serialize(updateDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"MedicalRecord/{MedicalRecord.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ErrorMessage = "Medical record not found.";
                        return Page();
                    }
                    ErrorMessage = "Unable to update medical record.";
                    return Page();
                }

                SuccessMessage = "Medical record updated successfully!";
                return RedirectToPage("/MedicalRecords/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}