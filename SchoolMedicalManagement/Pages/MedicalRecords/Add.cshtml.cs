using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.DTOs.Medication;
using BusinessLogic.Services;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public class AddModel : PageModel
    {
        private readonly IMedicationService _medicationService;
        private readonly IStudentService _studentService;

        public AddModel(IMedicationService medicationService, IStudentService studentService)
        {
            _medicationService = medicationService;
            _studentService = studentService;
        }

        [BindProperty]
        public MedicationSubmissionViewModel Medication { get; set; } = new MedicationSubmissionViewModel();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadStudentsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadStudentsAsync();
                return Page();
            }

            try
            {
                var createDto = new Medication
                {
                    StudentId = Medication.StudentId,
                    MedicationName = Medication.MedicationName,
                    Dosage = Medication.Dosage,
                    Frequency = Medication.Frequency
                };

                await _medicationService.AddMedicationAsync(createDto);
                SuccessMessage = "Medication submission successful! The school nurse will review and manage the medication.";

                // Clear form
                Medication = new MedicationSubmissionViewModel();
                await LoadStudentsAsync();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                await LoadStudentsAsync();
                return Page();
            }
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var students = await _studentService.GetStudentsByParentUserIdAsync(userId);
                    Medication.Students = students.Select(s => new StudentOption
                    {
                        Id = s.Id,
                        Name = s.FullName,
                        Class = s.Class ?? "N/A"
                    }).ToList();
                }
            }
            catch (Exception)
            {
                Medication.Students = new List<StudentOption>();
            }
        }
    }
}