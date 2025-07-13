using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class VaccinationStatusUpdateDto
    {
        public int StudentId { get; set; }
        public int PlanId { get; set; }
        public string Status { get; set; } = null!; // "Given" or "Missed"
        public DateTime? DateGiven { get; set; }
    }
}
