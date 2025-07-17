using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents.IncidentReports
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<IncidentReportDto> IncidentReports { get; set; } = new List<IncidentReportDto>();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            // Only allow Parents
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Parent")
            {
                return RedirectToPage("/Auth/Login");
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

                // Get incident reports for parent's children using new by-parent endpoint
                var incidentResponse = await client.GetAsync("incidentreport/by-parent");
                if (!incidentResponse.IsSuccessStatusCode)
                {
                    if (incidentResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ErrorMessage = "Invalid or missing user authentication.";
                    }
                    else if (incidentResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ErrorMessage = "No children found for this parent.";
                    }
                    else
                    {
                        ErrorMessage = "Unable to load incident reports.";
                    }
                    return Page();
                }

                var incidentJson = await incidentResponse.Content.ReadAsStringAsync();
                var incidents = JsonSerializer.Deserialize<List<IncidentReportDto>>(incidentJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (incidents != null)
                {
                    // Data is already filtered by API
                    IncidentReports = incidents
                        .OrderByDescending(incident => incident.Date)
                        .ToList();
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