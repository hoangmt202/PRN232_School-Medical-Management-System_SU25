using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class UpdateDrugStorageDto
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string MedicationName { get; set; }

        [StringLength(255)]
        public string DosageForm { get; set; }

        [StringLength(255)]
        public string Strength { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int? Quantity { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [StringLength(255)]
        public string Manufacturer { get; set; }

        [StringLength(255)]
        public string StorageLocation { get; set; }

        public int? ManagedBy { get; set; }

        public string Notes { get; set; }
    }
}
