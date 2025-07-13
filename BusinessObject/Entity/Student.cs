using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Class { get; set; } = null!;
        public int ParentId { get; set; }

        public Parent Parent { get; set; } = null!;
        public MedicalRecord? MedicalRecord { get; set; }
        public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
        public ICollection<VaccinationNotice> VaccinationNotices { get; set; } = new List<VaccinationNotice>();
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();
    }
}
