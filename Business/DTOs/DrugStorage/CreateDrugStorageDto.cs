using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class CreateDrugStorageDto
    {
        [Required]
        [StringLength(255)]
        public string MedicationName { get; set; }

        [StringLength(255)]
        public string DosageForm { get; set; }

        [StringLength(255)]
        public string Strength { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [StringLength(255)]
        public string Manufacturer { get; set; }

        [StringLength(255)]
        public string StorageLocation { get; set; }

        [Required]
        public int ManagedBy { get; set; }

        public string Notes { get; set; }
    }
}
