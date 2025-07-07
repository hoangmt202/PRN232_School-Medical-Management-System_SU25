using BusinessLogic.DTOs.MedicalRecord;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public partial class IndexModel : PageModel
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IStudentService _studentService;

        public IndexModel(IMedicalRecordService medicalRecordService, IStudentService studentService)
        {
            _medicalRecordService = medicalRecordService;
            _studentService = studentService;
        }

        public List<MedicalRecordViewModel> MedicalRecords { get; set; } = new List<MedicalRecordViewModel>();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get current user ID from claims
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                //{
                //    ErrorMessage = "User not authenticated.";
                //    return Page();
                //}

                // Get parent's children
                var children = await _studentService.GetStudentsByParentUserIdAsync(101);

                if (!children.Any())
                {
                    ErrorMessage = "No children found for this parent.";
                    return Page();
                }

                // Get medical records for each child
                foreach (var child in children)
                {
                    var medicalRecord = await _medicalRecordService.GetMedicalRecordByStudentIdAsync(child.Id);
                    if (medicalRecord != null)
                    {
                        MedicalRecords.Add(new MedicalRecordViewModel
                        {
                            Id = medicalRecord.Id,
                            StudentId = child.Id,
                            StudentName = child.FullName,
                            StudentClass = child.Class,
                            StudentDateOfBirth = child.DateOfBirth,
                            Allergies = medicalRecord.Allergies,
                            ChronicDiseases = medicalRecord.ChronicDiseases,
                            TreatmentHistory = medicalRecord.TreatmentHistory,
                            PhysicalCondition = medicalRecord.PhysicalCondition
                        });
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
