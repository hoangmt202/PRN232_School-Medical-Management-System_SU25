using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<User> Login(string Email, string password);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByEmail(string email);
        Task<User> CreateUser(User user);
    }
}
