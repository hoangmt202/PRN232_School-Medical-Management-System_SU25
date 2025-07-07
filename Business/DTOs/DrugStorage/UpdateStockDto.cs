using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class UpdateStockDto
    {
        [Required]
        public int DrugId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int NewQuantity { get; set; }

        [Required]
        public string Reason { get; set; }

        public string Notes { get; set; }

        [Required]
        public int UpdatedBy { get; set; }
    }
}
