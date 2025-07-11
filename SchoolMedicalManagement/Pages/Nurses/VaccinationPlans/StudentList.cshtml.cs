using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.Nurses.VaccinationPlans
{
    public class StudentListModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public StudentListModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("API");
        }

        public List<StudentVaccinationStatusDto> Students { get; set; } = new();
        public VaccinationPlanDto Plan { get; set; } = new();

        public async Task OnGetAsync(int planId)
        {
            var planRes = await _httpClient.GetAsync($"Vaccination/VaccinationPlan/{planId}");
            if (planRes.IsSuccessStatusCode)
            {
                Plan = await planRes.Content.ReadFromJsonAsync<VaccinationPlanDto>() ?? new();
            }

            var studentRes = await _httpClient.GetAsync($"Vaccination/plan/{planId}/students");
            if (studentRes.IsSuccessStatusCode)
            {
                Students = await studentRes.Content.ReadFromJsonAsync<List<StudentVaccinationStatusDto>>() ?? new();
            }
        }
    }
}
