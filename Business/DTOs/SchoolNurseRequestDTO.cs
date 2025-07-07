using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class SchoolNurseRequestDTO
    {
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }
    }
} 