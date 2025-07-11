using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class VaccinationPlan
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("vaccine_name")]
        public string VaccineName { get; set; } = null!;     // e.g., MMR, DPT
        [Column("scheduled_date")]
        public DateTime ScheduledDate { get; set; }          // When school plans to give it
        [Column("target_group")]
        public string TargetGroup { get; set; } = null!;     // e.g., “Grade 1”, “Age 6–7”
        [Column("notes")]
        public string? Notes { get; set; }
        public int AssignedNurseId { get; set; }

        [ForeignKey("AssignedNurseId")]
        public SchoolNurse Nurse { get; set; } = null!;
        public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
        public ICollection<VaccinationNotice> Notices { get; set; } = new List<VaccinationNotice>();
    }
}
