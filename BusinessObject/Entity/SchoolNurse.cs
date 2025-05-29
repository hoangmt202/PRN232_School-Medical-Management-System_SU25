using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class SchoolNurse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Medication> GivenMedications { get; set; } = new List<Medication>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<DrugStorage> ManagedDrugs { get; set; } = new List<DrugStorage>();
    }
}
