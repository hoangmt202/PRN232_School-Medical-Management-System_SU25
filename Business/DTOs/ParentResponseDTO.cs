using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ParentResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public ICollection<BusinessObject.Entity.Student> Students { get; set; } = new List<BusinessObject.Entity.Student>();
    }
}
