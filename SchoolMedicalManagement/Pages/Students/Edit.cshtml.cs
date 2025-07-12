using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public StudentDto Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/Student/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Student = JsonSerializer.Deserialize<StudentDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (Student == null)
                        return NotFound();
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(Student);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"http://localhost:5234/api/Student/{Student.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update student.");
                    return Page();
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to update student.");
                return Page();
            }
        }
    }
} 