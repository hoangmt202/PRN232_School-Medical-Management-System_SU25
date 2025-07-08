using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.SchoolNurses
{
    public class IndexModel : PageModel
    {
        public List<SchoolNurseDto> SchoolNurses { get; set; } = new();

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            // Adjust the API base URL as needed
            var apiUrl = "https://localhost:5234/api/SchoolNurse";
            var nurses = await client.GetFromJsonAsync<List<SchoolNurseDto>>(apiUrl);
            if (nurses != null)
                SchoolNurses = nurses;
        }
    }
} 