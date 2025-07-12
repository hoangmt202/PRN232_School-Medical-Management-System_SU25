using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DashboardModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public HealthTrendReportDto HealthTrends { get; set; } = new();
        public IncidentStatisticsDto IncidentStats { get; set; } = new();
        public ImmunizationCoverageDto Immunization { get; set; } = new();
        public InventoryReportDto Inventory { get; set; } = new();
        public ParentalResponseDto ParentalResponse { get; set; } = new();

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HealthTrends = await _httpClient.GetFromJsonAsync<HealthTrendReportDto>("report/health-trends") ?? new();
            IncidentStats = await _httpClient.GetFromJsonAsync<IncidentStatisticsDto>("report/incidents") ?? new();
            Immunization = await _httpClient.GetFromJsonAsync<ImmunizationCoverageDto>("report/immunization") ?? new();
            Inventory = await _httpClient.GetFromJsonAsync<InventoryReportDto>("drugstorage/report") ?? new();
            ParentalResponse = await _httpClient.GetFromJsonAsync<ParentalResponseDto>("report/parental-responses") ?? new();
        }
    }
}
