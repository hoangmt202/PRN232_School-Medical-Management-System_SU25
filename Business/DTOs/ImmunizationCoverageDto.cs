using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ImmunizationCoverageDto
    {
        public int TotalStudents { get; set; }
        public int VaccinatedStudents { get; set; }
        public double CoverageRate => TotalStudents == 0 ? 0 : (double)VaccinatedStudents / TotalStudents * 100;
    }
}
