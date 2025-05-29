using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
