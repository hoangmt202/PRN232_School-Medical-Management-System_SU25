using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class DrugStorage
    {
        public int Id { get; set; }
        public string MedicationName { get; set; } = null!;
        public string DosageForm { get; set; } = null!;
        public string Strength { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Manufacturer { get; set; } = null!;
        public string StorageLocation { get; set; } = null!;
        public DateTime DateReceived { get; set; }
        public int ManagedBy { get; set; }

        public SchoolNurse Nurse { get; set; } = null!;
    }
}
