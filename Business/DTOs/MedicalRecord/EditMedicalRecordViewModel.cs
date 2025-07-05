using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.MedicalRecord
{
    public class EditMedicalRecordViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "Allergies")]
        public string? Allergies { get; set; }

        [Display(Name = "Chronic Diseases")]
        public string? ChronicDiseases { get; set; }

        [Display(Name = "Treatment History")]
        public string? TreatmentHistory { get; set; }

        [Display(Name = "Physical Condition")]
        public string? PhysicalCondition { get; set; }
    }
}
