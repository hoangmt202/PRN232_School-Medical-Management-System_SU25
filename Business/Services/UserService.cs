using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly SchoolMedicalDbContext _context;

        public UserService(SchoolMedicalDbContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string Email, string password)
        {
            var account = await _context.Users.FirstOrDefaultAsync(account => account.Email == Email && account.PasswordHash == password);
            return account;
        }
    }
}
