using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.IncidentReports
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<IncidentReportDto> IncidentReports { get; set; } = new List<IncidentReportDto>();

        public async Task OnGetAsync(int? studentId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string url = "http://localhost:5234/api/IncidentReport";
                if (studentId.HasValue)
                {
                    url = $"http://localhost:5234/api/IncidentReport/student/{studentId.Value}";
                }
                var response = await client.GetAsync(url);
                
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