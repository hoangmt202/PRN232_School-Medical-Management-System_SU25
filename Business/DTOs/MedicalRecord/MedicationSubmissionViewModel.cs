using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.MedicalRecord
{
    public class MedicationSubmissionViewModel
    {
        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Medication Name")]
        [MaxLength(255)]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Dosage")]
        [MaxLength(255)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Frequency")]
        [MaxLength(255)]
        public string Frequency { get; set; } = string.Empty;

        // For display purposes
        public List<StudentOption> Students { get; set; } = new List<StudentOption>();
    }
}
