using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("medications")]
    public class Medication
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        [Column("medication_name")]
        public string MedicationName { get; set; } = null!;
        [Column("dosage")]
        public string Dosage { get; set; } = null!;
        [Column("frequency")]
        public string Frequency { get; set; } = null!;
        [Column("given_by")]
        public int GivenBy { get; set; }

        public Student Student { get; set; } = null!;
        public SchoolNurse Nurse { get; set; } = null!;
    }
}
