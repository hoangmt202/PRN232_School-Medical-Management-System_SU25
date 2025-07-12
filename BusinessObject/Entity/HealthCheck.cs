using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("health_checks")]
    public class HealthCheck
    {
        public int Id { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        [Column("check_type")]
        public string CheckType { get; set; } = null!;
        public string Results { get; set; } = null!;
        public string? Notes { get; set; }

        public Student Student { get; set; } = null!;
    }
}
