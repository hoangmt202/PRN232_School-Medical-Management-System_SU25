using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SchoolNurse
{
    public class CreateSchoolNurseDto
    {
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? PhoneNumber { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
