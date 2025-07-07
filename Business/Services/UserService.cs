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
        private readonly DataAccess.IUnitOfWorks _unitOfWork;

        public UserService(DataAccess.IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Login(string Email, string password)
        {
            var account = await _unitOfWork.UserRepository.GetAsync(
                account => account.Email == Email && account.PasswordHash == password);
            return account;
        }
    }
}
