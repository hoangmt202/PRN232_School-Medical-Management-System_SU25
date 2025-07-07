using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class StockMovementDto
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string MedicationName { get; set; }
        public string MovementType { get; set; } // Received, Dispensed, Adjusted, Expired
        public int QuantityBefore { get; set; }
        public int QuantityChanged { get; set; }
        public int QuantityAfter { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string PerformedBy { get; set; }
    }
}
