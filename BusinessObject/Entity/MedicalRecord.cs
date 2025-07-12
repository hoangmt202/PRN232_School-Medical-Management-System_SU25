using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("medical_records")]
    public class MedicalRecord
    {
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        [Column("allergies")]
        public string? Allergies { get; set; }
        [Column("chronic_diseases")]
        public string? ChronicDiseases { get; set; }
        [Column("treatment_history")]
        public string? TreatmentHistory { get; set; }
        [Column("physical_condition")]
        public string? PhysicalCondition { get; set; }

        public Student Student { get; set; } = null!;
    }
}
