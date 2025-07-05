using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public class EditModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public EditModel(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [BindProperty]
        public EditMedicalRecordViewModel MedicalRecord { get; set; } = new EditMedicalRecordViewModel();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var record = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
                if (record == null)
                {
                    return NotFound();
                }

                MedicalRecord = new EditMedicalRecordViewModel
                {
                    Id = record.Id,
                    StudentId = record.StudentId,
                    StudentName = record.StudentName ?? "Unknown",
                    Allergies = record.Allergies,
                    ChronicDiseases = record.ChronicDiseases,
                    TreatmentHistory = record.TreatmentHistory,
                    PhysicalCondition = record.PhysicalCondition
                };

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var updateDto = new UpdateMedicalRecordDto
                {
                    Allergies = MedicalRecord.Allergies,
                    ChronicDiseases = MedicalRecord.ChronicDiseases,
                    TreatmentHistory = MedicalRecord.TreatmentHistory,
                    PhysicalCondition = MedicalRecord.PhysicalCondition
                };

                var updatedRecord = await _medicalRecordService.UpdateMedicalRecordAsync(MedicalRecord.Id, updateDto);
                if (updatedRecord == null)
                {
                    ErrorMessage = "Medical record not found.";
                    return Page();
                }

                SuccessMessage = "Medical record updated successfully!";
                return RedirectToPage("/Parent/MedicalRecord");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}