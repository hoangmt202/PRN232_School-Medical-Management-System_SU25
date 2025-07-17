using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class HealthCheckDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string CheckType { get; set; } = null!;
        public string Results { get; set; } = null!;
        public string? Notes { get; set; }

        // Navigation properties
        public StudentResponseDTO? Student { get; set; }
    }

    public class CreateHealthCheckDto
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string CheckType { get; set; } = null!;
        public string Results { get; set; } = null!;
        public string? Notes { get; set; }
    }

    public class UpdateHealthCheckDto
    {
        public DateTime Date { get; set; }
        public string CheckType { get; set; } = null!;
        public string Results { get; set; } = null!;
        public string? Notes { get; set; }
    }
} 