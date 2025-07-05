using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.Admin.MedicalRecords
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public IndexModel(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public string SearchTerm { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string searchTerm = "")
        {
            SearchTerm = searchTerm ?? "";
            await LoadMedicalRecordsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var deleted = await _medicalRecordService.DeleteMedicalRecordAsync(id);
                if (deleted)
                {
                    SuccessMessage = "Medical record deleted successfully.";
                }
                else
                {
                    ErrorMessage = "Medical record not found or could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while deleting the record: {ex.Message}";
            }

            await LoadMedicalRecordsAsync();
            return Page();
        }
        private async Task LoadMedicalRecordsAsync()
        {
            try
            {
                var records = await _medicalRecordService.GetAllMedicalRecordsAsync();

                MedicalRecords = records.Select(r => new MedicalRecordViewModel
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.StudentName ?? "Unknown",
                    Allergies = r.Allergies,
                    ChronicDiseases = r.ChronicDiseases,
                    TreatmentHistory = r.TreatmentHistory,
                    PhysicalCondition = r.PhysicalCondition
                }).ToList();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    MedicalRecords = MedicalRecords.Where(r =>
                        r.StudentName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(r.Allergies) && r.Allergies.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(r.ChronicDiseases) && r.ChronicDiseases.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while loading medical records: {ex.Message}";
                MedicalRecords = new List<MedicalRecordViewModel>();
            }
        }
    }
}