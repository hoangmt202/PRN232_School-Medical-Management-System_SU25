using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Entity
{
        
        public class DrugStorage
        {

            public int Id { get; set; }
            public string MedicationName { get; set; }
            public string DosageForm { get; set; } // Tablet, Syrup, Injection, etc.

            public string Strength { get; set; } // e.g. 500mg, 5mg/ml

            public int Quantity { get; set; }

            public DateTime ExpirationDate { get; set; }

            public string Manufacturer { get; set; }

            public string StorageLocation { get; set; }

            public DateTime DateReceived { get; set; }

            public int ManagedBy { get; set; } // FK to school_nurses.id

            // Navigation properties
            public virtual SchoolNurse Nurse { get; set; }

            // Additional properties for inventory management
            [NotMapped]
            public bool IsExpired => ExpirationDate < DateTime.Now;

            [NotMapped]
            public bool IsExpiringSoon => ExpirationDate <= DateTime.Now.AddDays(30);

            [NotMapped]
            public int DaysUntilExpiration => (ExpirationDate - DateTime.Now).Days;

            [NotMapped]
            public bool IsLowStock { get; set; } // Will be calculated based on threshold
        }
 }
