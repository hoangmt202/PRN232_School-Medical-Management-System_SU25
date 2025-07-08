using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.SchoolNurses
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public SchoolNurseDto SchoolNurse { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:5234/api/SchoolNurse/{id}";
            var nurse = await client.GetFromJsonAsync<SchoolNurseDto>(apiUrl);
            if (nurse == null)
                return NotFound();
            SchoolNurse = nurse;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:5234/api/SchoolNurse/{SchoolNurse.Id}";
            var response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError(string.Empty, "Failed to delete school nurse.");
            return Page();
        }
    }
} 