using BusinessLogic.DTOs.MedicalRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.MedicalRecords
{
    // [Authorize(Policy = "MedicalStaff")] - Temporarily disabled for testing
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<AdminMedicalRecordViewModel> MedicalRecords { get; set; } = new List<AdminMedicalRecordViewModel>();
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            // Check authorization - Admin or Nurse only
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Nurse")
            {
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Get current user token from cookie
                var token = Request.Cookies["AuthToken"];
                
                // Debug: Log token status
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "No AuthToken found in cookies. Please login again.";
                    return Page();
                }
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Get all medical records
                var response = await client.GetAsync("MedicalRecord");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Unable to load medical records. Status: {response.StatusCode}, Error: {errorContent}";
                    return Page();
                }

                var recordsJson = await response.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<MedicalRecordDto>>(recordsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (records != null)
                {
                    MedicalRecords = records.Select(r => new AdminMedicalRecordViewModel
                    {
                        Id = r.Id,
                        StudentId = r.StudentId,
                        StudentName = r.StudentName ?? "Unknown",
                        StudentClass = "N/A", // Will be populated from student info if needed
                        Allergies = r.Allergies,
                        ChronicDiseases = r.ChronicDiseases,
                        TreatmentHistory = r.TreatmentHistory,
                        PhysicalCondition = r.PhysicalCondition
                    }).ToList();
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // Check authorization - Admin only for delete
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                ErrorMessage = "You don't have permission to delete medical records.";
                return RedirectToPage();
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

                var response = await client.DeleteAsync($"MedicalRecord/{id}");
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Medical record deleted successfully.";
                }
                else
                {
                    ErrorMessage = "Unable to delete medical record.";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToPage();
            }
        }
    }

    public class AdminMedicalRecordViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? StudentClass { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? TreatmentHistory { get; set; }
        public string? PhysicalCondition { get; set; }
    }
}