using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ParentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
