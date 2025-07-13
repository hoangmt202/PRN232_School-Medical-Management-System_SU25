using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class VaccinationDto
    {
        public int StudentId { get; set; }
        public int VaccinePlanId { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime DateScheduled { get; set; }
        public DateTime? DateGiven { get; set; }
        public string Status { get; set; } = null!;
        public string? ResultNote { get; set; }
    }
}
