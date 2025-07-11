using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class VaccinationRecordDto
    {
        public int Id { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime? DateGiven { get; set; }
        public string Status { get; set; } = null!;
        public string? ResultNote { get; set; }
        public string StudentName { get; set; } = null!;
    }
}
