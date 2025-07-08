using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.SchoolNurses
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public SchoolNurseDto SchoolNurse { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            using var client = new HttpClient();
            var apiUrl = "https://localhost:5001/api/SchoolNurse";
            var response = await client.PostAsJsonAsync(apiUrl, SchoolNurse);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError(string.Empty, "Failed to create school nurse.");
            return Page();
        }
    }
} 