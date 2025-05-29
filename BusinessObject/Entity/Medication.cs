using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Medication
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string MedicationName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public int GivenBy { get; set; }

        public Student Student { get; set; } = null!;
        public SchoolNurse Nurse { get; set; } = null!;
    }
}
