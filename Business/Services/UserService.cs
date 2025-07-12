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
            try
            {
                Console.WriteLine($"Login attempt - Email: {Email}, Password: {password}");
                
                // First, let's check if the user exists by email only
                var userByEmail = await _unitOfWork.UserRepository.GetAsync(
                    account => account.Email == Email);
                
                if (userByEmail == null)
                {
                    Console.WriteLine($"No user found with email: {Email}");
                    return null;
                }
                
                Console.WriteLine($"User found: {userByEmail.Username} with role: {userByEmail.Role}");
                Console.WriteLine($"Stored password hash: {userByEmail.PasswordHash}");
                Console.WriteLine($"Provided password: {password}");
                
                // Now check with both email and password
                var account = await _unitOfWork.UserRepository.GetAsync(
                    account => account.Email == Email && account.PasswordHash == password);
                
                if (account == null)
                {
                    Console.WriteLine("Password does not match");
                }
                else
                {
                    Console.WriteLine("Login successful");
                }
                
                return account;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                return users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllUsers error: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == email);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserByEmail error: {ex.Message}");
                throw;
            }
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateUser error: {ex.Message}");
                throw;
            }
        }
    }
}
