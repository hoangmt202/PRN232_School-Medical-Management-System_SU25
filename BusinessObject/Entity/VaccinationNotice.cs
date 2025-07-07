using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("vaccination_notices")]
    public class VaccinationNotice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        public int VaccinationPlanId { get; set; }
        [Column("date_sent")]
        public DateTime DateSent { get; set; }
        [Column("response")]
        public string Response { get; set; } = null!;
        [Column("follow_up_date")]
        public DateTime? FollowUpDate { get; set; }

        public Student Student { get; set; } = null!;
        public VaccinationPlan Plan { get; set; } = null!;
    }
}
