using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class IncidentReport
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int NurseId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ActionTaken { get; set; } = null!;

        public Student Student { get; set; } = null!;
        public SchoolNurse Nurse { get; set; } = null!;
    }
}
