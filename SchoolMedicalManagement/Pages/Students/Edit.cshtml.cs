using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.Students
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public StudentDto Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:5001/api/Student/{id}";
            var student = await client.GetFromJsonAsync<StudentDto>(apiUrl);
            if (student == null)
                return NotFound();
            Student = student;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:5001/api/Student/{Student.Id}";
            var response = await client.PutAsJsonAsync(apiUrl, Student);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError(string.Empty, "Failed to update student.");
            return Page();
        }
    }
} 