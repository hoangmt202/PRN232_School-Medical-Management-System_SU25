using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class InventoryAlert
    {
        public int Id { get; set; }
        public string AlertType { get; set; } // "LOW_STOCK", "EXPIRED", "EXPIRING"
        public string Message { get; set; }
        public int DrugId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DrugStorage Drug { get; set; }
    }
}
