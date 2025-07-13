using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class StudentVaccinationStatusDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }  // Scheduled / Given / Missed
        public DateTime ScheduledDate { get; set; }
        public DateTime? DateGiven { get; set; }
        public string? ResultNote { get; set; }
    }
}
