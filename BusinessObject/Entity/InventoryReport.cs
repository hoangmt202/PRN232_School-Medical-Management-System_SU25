using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class InventoryReport
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public int ExpiredItems { get; set; }
        public int ExpiringItems { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<DrugStorage> LowStockDrugs { get; set; }
        public IEnumerable<DrugStorage> ExpiredDrugs { get; set; }
        public IEnumerable<DrugStorage> ExpiringDrugs { get; set; }
    }
}
