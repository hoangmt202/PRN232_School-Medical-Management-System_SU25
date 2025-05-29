using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class HealthCheck
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string CheckType { get; set; } = null!;
        public string Results { get; set; } = null!;
        public string? Notes { get; set; }

        public Student Student { get; set; } = null!;
    }
}
