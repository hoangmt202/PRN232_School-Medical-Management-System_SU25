using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.MedicalRecord
{
    public class MedicalRecordViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? TreatmentHistory { get; set; }
        public string? PhysicalCondition { get; set; }

        public string StudentName { get; set; } = string.Empty;
        public string StudentClass { get; set; } = string.Empty;
        public DateTime StudentDateOfBirth { get; set; }
    }
}
