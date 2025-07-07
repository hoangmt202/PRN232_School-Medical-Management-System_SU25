using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class DrugStorageDto
    {
        public int Id { get; set; }
        public string MedicationName { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Manufacturer { get; set; }
        public string StorageLocation { get; set; }
        public DateTime DateReceived { get; set; }
        public int ManagedBy { get; set; }
        public string? ManagedByName { get; set; }

        // Calculated fields
        public bool IsExpired { get; set; }
        public bool IsExpiringSoon { get; set; }
        public int DaysUntilExpiration { get; set; }
        public bool IsLowStock { get; set; }
        public string Status { get; set; } // Active, Expired, Low Stock, etc.
    }
}
