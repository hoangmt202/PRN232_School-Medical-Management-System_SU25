using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Medication
{
    public class MedicationUsageDto
    {
        public int Id { get; set; }
        public string MedicationName { get; set; }
        public string StudentName { get; set; }
        public string StudentClass { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime DateAdministered { get; set; }
        public string AdministeredBy { get; set; }
        public string Notes { get; set; }
    }
}
