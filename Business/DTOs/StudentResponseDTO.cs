using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class StudentResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Class { get; set; } = null!;
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
        public BusinessObject.Entity.MedicalRecord? MedicalRecord { get; set; }
        public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
        public ICollection<VaccinationNotice> VaccinationNotices { get; set; } = new List<VaccinationNotice>();
        public ICollection<BusinessObject.Entity.Medication> Medications { get; set; } = new List<BusinessObject.Entity.Medication>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();
    }
} 