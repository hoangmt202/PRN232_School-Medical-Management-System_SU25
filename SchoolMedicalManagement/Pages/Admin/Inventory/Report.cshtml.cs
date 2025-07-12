using BusinessLogic.DTOs.DrugStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.Inventory
{
    public class ReportModel : PageModel
    {
            private readonly HttpClient _httpClient;

            public ReportModel(IHttpClientFactory factory)
            {
                _httpClient = factory.CreateClient("API");
            }

            public InventoryReportDto Report { get; set; } = new();

            public async Task OnGetAsync()
            {
                var token = Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync("drugstorage/report");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Report = await response.Content.ReadFromJsonAsync<InventoryReportDto>(options) ?? new();
                }
            }
    }
}
