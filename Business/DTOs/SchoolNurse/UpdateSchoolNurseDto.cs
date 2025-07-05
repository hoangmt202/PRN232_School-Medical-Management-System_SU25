using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SchoolNurse
{
    public class UpdateSchoolNurseDto
    {
        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string? PhoneNumber { get; set; }
    }
}
