using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolMedicalManagement.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<StudentDto> Students { get; set; } = new();

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            // Adjust the API base URL as needed
            var apiUrl = "https://localhost:5234/api/Student";
            var students = await client.GetFromJsonAsync<List<StudentDto>>(apiUrl);
            if (students != null)
                Students = students;
        }
    }
} 