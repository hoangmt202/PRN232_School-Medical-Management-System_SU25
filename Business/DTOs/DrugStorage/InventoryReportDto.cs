using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class InventoryReportDto
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public int ExpiredItems { get; set; }
        public int ExpiringItems { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime GeneratedAt { get; set; }
        public IEnumerable<DrugStorageDto> LowStockDrugs { get; set; }
        public IEnumerable<DrugStorageDto> ExpiredDrugs { get; set; }
        public IEnumerable<DrugStorageDto> ExpiringDrugs { get; set; }
        public Dictionary<string, int> LocationSummary { get; set; }
        public Dictionary<string, int> DosageFormSummary { get; set; }
    }
}
