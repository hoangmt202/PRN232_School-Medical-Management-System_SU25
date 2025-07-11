using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Vaccination
    {
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        public int? VaccinationPlanId { get; set; }
        [Column("vaccine_name")]
        public string VaccineName { get; set; } = null!;
        [Column("date_scheduled")]
        public DateTime DateScheduled { get; set; }
        [Column("date_given")]
        public DateTime? DateGiven { get; set; }
        [Column("status")]
        public string Status { get; set; } = null!;
        [Column("result_note")]
        public string? ResultNote { get; set; }

        public Student Student { get; set; } = null!;
        public VaccinationPlan Plan { get; set; } = null!;
    }
}
