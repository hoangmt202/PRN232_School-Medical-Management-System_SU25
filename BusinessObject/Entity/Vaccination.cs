using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Vaccination
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int VaccinePlanId { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime DateScheduled { get; set; }
        public DateTime? DateGiven { get; set; }
        public string Status { get; set; } = null!;
        public string? ResultNote { get; set; }

        public Student Student { get; set; } = null!;
        public VaccinationPlan Plan { get; set; } = null!;
    }
}
