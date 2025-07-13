using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class HealthTrendReportDto
    {
        public int TotalHealthChecks { get; set; }
        public Dictionary<string, int> CheckTypeCounts { get; set; } = new();
        public Dictionary<DateTime, int> ChecksPerMonth { get; set; } = new();
    }
}
