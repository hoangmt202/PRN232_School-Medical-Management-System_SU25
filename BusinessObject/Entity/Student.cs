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
        [Column("full_name")]
        public string FullName { get; set; } = null!;
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        [Column("gender")]
        public string Gender { get; set; } = null!;
        [Column("class")]
        public string Class { get; set; } = null!;
        [Column("parent_id")]
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
