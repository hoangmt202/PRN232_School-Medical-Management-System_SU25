using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class SchoolNurseResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<BusinessObject.Entity.Medication> GivenMedications { get; set; } = new List<BusinessObject.Entity.Medication>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<BusinessObject.Entity.DrugStorage> ManagedDrugs { get; set; } = new List<BusinessObject.Entity.DrugStorage>();
    }
} 