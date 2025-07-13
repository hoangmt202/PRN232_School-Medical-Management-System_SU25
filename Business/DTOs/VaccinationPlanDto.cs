using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class VaccinationPlanDto
    {
        public int Id { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string TargetGroup { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string Response { get; set; } = null!;
    }
}
