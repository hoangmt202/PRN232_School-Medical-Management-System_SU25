using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
