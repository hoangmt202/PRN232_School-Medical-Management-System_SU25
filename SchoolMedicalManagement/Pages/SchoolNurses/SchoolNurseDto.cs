using System;
using System.Collections.Generic;

namespace SchoolMedicalManagement.Pages.SchoolNurses
{
    public class SchoolNurseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        // Add more properties as needed to match API response
    }
} 