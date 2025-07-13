using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class IncidentStatisticsDto
    {
        public int TotalIncidents { get; set; }
        public Dictionary<string, int> TypeCounts { get; set; } = new();
        public Dictionary<DateTime, int> MonthlyTrends { get; set; } = new();
    }
}
