using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Medication
{
    public class CreateMedicationDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string MedicationName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public int GivenBy { get; set; }
    }
}
