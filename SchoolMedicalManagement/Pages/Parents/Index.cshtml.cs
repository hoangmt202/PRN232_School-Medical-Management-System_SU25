using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents
{
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ParentResponseDTO> Parents { get; set; } = new List<ParentResponseDTO>();

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5234/api/parent");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Parents = JsonSerializer.Deserialize<List<ParentResponseDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<ParentResponseDTO>();
                }
                else
                {
                    Parents = new List<ParentResponseDTO>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Parents = new List<ParentResponseDTO>();
            }
        }
    }
} 