using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class IncidentReportDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int NurseId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ActionTaken { get; set; } = null!;

        // Navigation properties
        public StudentResponseDTO? Student { get; set; }
        public SchoolNurseResponseDTO? Nurse { get; set; }
    }

    public class CreateIncidentReportDto
    {
        public int StudentId { get; set; }
        public int NurseId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ActionTaken { get; set; } = null!;
    }

    public class UpdateIncidentReportDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ActionTaken { get; set; } = null!;
    }
} 