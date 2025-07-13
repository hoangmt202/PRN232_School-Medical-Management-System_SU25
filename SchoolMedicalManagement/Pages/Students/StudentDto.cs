using System;
using System.Collections.Generic;

namespace SchoolMedicalManagement.Pages.Students
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int ParentId { get; set; }
        // Add more properties as needed to match API response
    }
} 