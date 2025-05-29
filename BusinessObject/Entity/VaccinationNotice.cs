using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class VaccinationNotice
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string VaccineName { get; set; } = null!;
        public DateTime DateSent { get; set; }
        public string Response { get; set; } = null!;
        public DateTime? FollowUpDate { get; set; }

        public Student Student { get; set; } = null!;
    }
}
