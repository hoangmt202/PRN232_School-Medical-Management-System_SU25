using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ParentalResponseDto
    {
        public int TotalNotices { get; set; }
        public int Accepted { get; set; }
        public int Rejected { get; set; }
        public int NoResponse { get; set; }
    }
}
