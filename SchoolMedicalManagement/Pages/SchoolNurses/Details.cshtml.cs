using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.SchoolNurses
{
    public class DetailsModel : PageModel
    {
        public SchoolNurseDto SchoolNurse { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var client = new HttpClient();
            var apiUrl = $"https://localhost:5001/api/SchoolNurse/{id}";
            var nurse = await client.GetFromJsonAsync<SchoolNurseDto>(apiUrl);
            if (nurse == null)
                return NotFound();
            SchoolNurse = nurse;
            return Page();
        }
    }
} 