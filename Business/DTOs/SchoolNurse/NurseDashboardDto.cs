using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SchoolNurse
{
    public class NurseDashboardDto
    {
        public int AssignedStudentCount { get; set; }
        public int MedicationsDueToday { get; set; }
        public int OpenIncidentsCount { get; set; }
        public int LowStockAlerts { get; set; }
        public int UpcomingVaccinations { get; set; }
        public int WeeklyCheckups { get; set; }
    }
}
