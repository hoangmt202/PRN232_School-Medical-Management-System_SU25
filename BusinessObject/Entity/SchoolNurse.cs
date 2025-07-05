using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    [Table("school_nurses")]
    public class SchoolNurse
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("full_name")]
        public string FullName { get; set; } = null!;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;
        [Column("user_id")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Medication> GivenMedications { get; set; } = new List<Medication>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<DrugStorage> ManagedDrugs { get; set; } = new List<DrugStorage>();
    }
}
