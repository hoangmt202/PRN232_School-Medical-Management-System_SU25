using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? TreatmentHistory { get; set; }
        public string? PhysicalCondition { get; set; }

        public Student Student { get; set; } = null!;
    }
}
