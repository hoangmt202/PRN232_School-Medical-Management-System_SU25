using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ExternalVaccinationDto
    {
        public int StudentId { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime DateGiven { get; set; }
        public string? ResultNote { get; set; }
    }
}
