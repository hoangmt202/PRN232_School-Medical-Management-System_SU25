using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Admin.Inventory
{
    public class DetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }
        public DrugStorageDto Drug { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var drugsResponse = await _httpClient.GetAsync($"drugstorage/{id}");
            if (drugsResponse.IsSuccessStatusCode)
            {
                var json = await drugsResponse.Content.ReadAsStringAsync();
                Drug = JsonSerializer.Deserialize<DrugStorageDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            if (Drug == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
