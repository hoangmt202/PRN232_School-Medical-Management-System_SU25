using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("incident_reports")]
    public class IncidentReport
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        [Column("nurse_id")]
        public int NurseId { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("type")]
        public string Type { get; set; } = null!;
        [Column("description")]
        public string Description { get; set; } = null!;
        [Column("action_taken")]
        public string ActionTaken { get; set; } = null!;

        public Student Student { get; set; } = null!;
        public SchoolNurse Nurse { get; set; } = null!;
    }
}
