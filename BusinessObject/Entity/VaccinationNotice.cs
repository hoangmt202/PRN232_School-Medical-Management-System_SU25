using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class VaccinationNotice
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int VaccinationPlanId { get; set; }
        public DateTime DateSent { get; set; }
        public string Response { get; set; } = null!;
        public DateTime? FollowUpDate { get; set; }

        public Student Student { get; set; } = null!;
        public VaccinationPlan Plan { get; set; } = null!;
    }
}
