using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entity
{
    public class Parent
    {
        public int Id { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
