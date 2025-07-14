using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public StudentDto Student { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId.HasValue)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync($"http://localhost:5234/api/Parent/user/{UserId.Value}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var parent = JsonSerializer.Deserialize<ParentDto>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        
                        if (parent != null)
                        {
                            ParentName = parent.FullName;
                            Student = new StudentDto
                            {
                                ParentId = parent.Id
                            };
                        }
                    }
                }
                catch
                {
                    // If we can't fetch parent info, we'll still allow creation with the provided UserId
                    ModelState.AddModelError(string.Empty, "Failed to fetch parent information. Please try again.");
                    return Page();
                }
            }
            else if (ParentId.HasValue)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync($"http://localhost:5234/api/Parent/{ParentId.Value}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var parent = JsonSerializer.Deserialize<ParentDto>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        
                        if (parent != null)
                        {
                            ParentName = parent.FullName;
                            Student = new StudentDto
                            {
                                ParentId = parent.Id
                            };
                        }
                    }
                }
                catch
                {
                    // If we can't fetch parent info, we'll still allow creation with the provided ParentId
                    Student = new StudentDto
                    {
                        ParentId = ParentId.Value
                    };
                }
            }
            else
            {
                Student = new StudentDto();
            }

            return Page();
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
                var response = await client.PostAsync("http://localhost:5234/api/Student", content);
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to parent's detail page after successful add
                    return RedirectToPage("/Parents/Details", new { id = Student.ParentId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create student.");
                    return Page();
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to create student.");
                return Page();
            }
        }
    }

    public class ParentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
    }
} 