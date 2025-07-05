using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Medication
{
    public class DispenseMedicationDto
    {
        [Required]
        public int DrugId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(255)]
        public string Dosage { get; set; }

        [StringLength(255)]
        public string Frequency { get; set; }

        [Required]
        public int AdministeredBy { get; set; }

        public string Notes { get; set; }

        public DateTime? DateAdministered { get; set; }
    }
}
