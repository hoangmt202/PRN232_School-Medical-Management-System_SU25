using BusinessLogic.DTOs.Vaccination;
using BusinessLogic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.Parent.Vaccination
{
    public class ConsentModel : PageModel
    {
        private readonly IVaccinationParentService _vaccinationParentService;
        //private readonly IUserContextService _userContextService;

        public ConsentModel(IVaccinationParentService vaccinationParentService)
        {
            _vaccinationParentService = vaccinationParentService;
            //_userContextService = userContextService;
        }

        public List<VaccinationPlanDto> Plans { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            int parentId = 1;
            Plans = (await _vaccinationParentService.GetUpcomingPlansAsync(parentId)).ToList();
        }

        public async Task<IActionResult> OnPostConsentAsync(int studentId, int planId, string response)
        {
            var success = await _vaccinationParentService.SubmitConsentAsync(studentId, planId, response);
            if (success)
                SuccessMessage = "Consent updated successfully.";
            else
                ErrorMessage = "Unable to update consent.";

            return RedirectToPage();
        }
    }
}
