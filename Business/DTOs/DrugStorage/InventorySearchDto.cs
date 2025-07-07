using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class InventorySearchDto
    {
        public string SearchTerm { get; set; }
        public string Location { get; set; }
        public string DosageForm { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsLowStock { get; set; }
        public int? ManagedBy { get; set; }
        public DateTime? ExpirationDateFrom { get; set; }
        public DateTime? ExpirationDateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "MedicationName";
        public string SortDirection { get; set; } = "ASC";
    }
}
