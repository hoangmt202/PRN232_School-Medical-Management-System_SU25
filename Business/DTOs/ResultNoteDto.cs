using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ResultNoteDto
    {
        public int VaccinationId { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
