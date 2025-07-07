using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class ReceiveStockDto
    {
        [Required]
        public int DrugId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [StringLength(255)]
        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [StringLength(255)]
        public string Supplier { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cost must be non-negative")]
        public decimal? Cost { get; set; }

        [Required]
        public int ReceivedBy { get; set; }

        public string Notes { get; set; }
    }
}
