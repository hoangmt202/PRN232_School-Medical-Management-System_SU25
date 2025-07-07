using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DrugStorage
{
    public class InventoryAlertDto
    {
        public int Id { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string Severity { get; set; }
    }
}
