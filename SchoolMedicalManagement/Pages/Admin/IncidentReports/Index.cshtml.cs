using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.IncidentReports
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<IncidentReportDto> IncidentReports { get; set; } = new List<IncidentReportDto>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5234/api/incidentreport");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    IncidentReports = JsonSerializer.Deserialize<List<IncidentReportDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<IncidentReportDto>();
                }
                else
                {
                    IncidentReports = new List<IncidentReportDto>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                IncidentReports = new List<IncidentReportDto>();
            }
        }
    }
} 