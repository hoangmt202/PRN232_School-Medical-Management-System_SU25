using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Entity
{
        [Table("drug_storage")]
        public class DrugStorage
        {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Column("medication_name")]
            [Required]
            [StringLength(255)]
            public string MedicationName { get; set; }

            [Column("dosage_form")]
            [StringLength(255)]
            public string DosageForm { get; set; } // Tablet, Syrup, Injection, etc.

            [Column("strength")]
            [StringLength(255)]
            public string Strength { get; set; } // e.g. 500mg, 5mg/ml

            [Column("quantity")]
            public int Quantity { get; set; }

            [Column("expiration_date")]
            public DateTime ExpirationDate { get; set; }

            [Column("manufacturer")]
            [StringLength(255)]
            public string Manufacturer { get; set; }

            [Column("storage_location")]
            [StringLength(255)]
            public string StorageLocation { get; set; }

            [Column("date_received")]
            public DateTime DateReceived { get; set; }

            [Column("managed_by")]
            public int ManagedBy { get; set; } // FK to school_nurses.id

            // Navigation properties
            [ForeignKey("ManagedBy")]
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
