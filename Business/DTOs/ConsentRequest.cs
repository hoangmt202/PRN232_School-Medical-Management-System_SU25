using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ConsentRequest
    {
        public int StudentId { get; set; }
        public int PlanId { get; set; }
        public string Response { get; set; } = string.Empty;
    }
}
