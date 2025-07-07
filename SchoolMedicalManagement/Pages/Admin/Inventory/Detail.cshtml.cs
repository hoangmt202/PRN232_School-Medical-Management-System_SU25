using BusinessLogic.DTOs.DrugStorage;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.Admin.Inventory
{
    public class DetailModel : PageModel
    {
        private readonly IInventoryService _drugStorageService;

        public DetailModel(IInventoryService drugStorageService)
        {
            _drugStorageService = drugStorageService;
        }

        public DrugStorageDto Drug { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Drug = await _drugStorageService.GetDrugByIdAsync(id);
            if (Drug == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
