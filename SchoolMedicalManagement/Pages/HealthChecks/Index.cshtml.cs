using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.HealthChecks
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<HealthCheckDto> HealthChecks { get; set; } = new List<HealthCheckDto>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5234/api/HealthCheck");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    HealthChecks = JsonSerializer.Deserialize<List<HealthCheckDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<HealthCheckDto>();
                }
                else
                {
                    HealthChecks = new List<HealthCheckDto>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                HealthChecks = new List<HealthCheckDto>();
            }
        }
    }
} 