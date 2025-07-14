using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public class MyChildrenModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MyChildrenModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<StudentResponseDTO> Children { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? parentId)
        {
            if (!parentId.HasValue)
            {
                return NotFound();
            }
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/Student/children/{parentId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Children = JsonSerializer.Deserialize<List<StudentResponseDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
} 