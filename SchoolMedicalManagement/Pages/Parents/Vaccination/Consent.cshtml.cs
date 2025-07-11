using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace SchoolMedicalManagement.Pages.Parents.Vaccination
{
    public class ConsentModel : PageModel
    {
		private readonly HttpClient _httpClient;

		public ConsentModel(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("API"); // Register this in Program.cs
		}

		public List<VaccinationPlanDto> Plans { get; set; } = new();
		public string ErrorMessage { get; set; } = string.Empty;
		public string SuccessMessage { get; set; } = string.Empty;

		public async Task OnGetAsync()
		{
            var token = Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
			try
			{
				var response = await _httpClient.GetAsync($"vaccination/upcoming");
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadFromJsonAsync<List<VaccinationPlanDto>>();
					Plans = result ?? new();
				}
				else
				{
					ErrorMessage = "Failed to load vaccination plans.";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error: " + ex.Message;
			}
		}

		public async Task<IActionResult> OnPostConsentAsync(int studentId, int planId, string response)
		{
			try
			{
				var payload = new
				{
					StudentId = studentId,
					PlanId = planId,
					Response = response
				};

				var result = await _httpClient.PostAsJsonAsync("vaccination/consent", payload);

				if (result.IsSuccessStatusCode)
				{
					SuccessMessage = "Consent updated successfully.";
				}
				else
				{
					ErrorMessage = "Failed to submit consent.";
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error: " + ex.Message;
			}

			return RedirectToPage();
		}
	}
}
