using BCrypt.Net;
using BusinessLogic.DTOs;
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
                account => account.Email == Email);
            if (account == null)
                return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);

            return isValidPassword ? account : null;
        }
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _unitOfWork.UserRepository.AnyAsync(u => u.Email == dto.Email))
                return (false, "Email already registered.");

            if (await _unitOfWork.UserRepository.AnyAsync(u => u.Username == dto.Username))
                return (false, "Username already taken.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "Parent"
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.UserRepository.SaveChangesAsync(); // GenericRepo provides this

            return (true, "User registered successfully.");
        }
        public async Task<UserProfileDto?> GetProfileByUserIdAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var dto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                RoleName = user.Role ?? "Unknown"
            };

            if (dto.RoleName == "Parent")
            {
                var parent = await _unitOfWork.ParentRepository.GetAsync(p => p.UserId == userId);
                if (parent != null)
                {
                    dto.FullName = parent.FullName;
                    dto.PhoneNumber = parent.PhoneNumber;
                    dto.Address = parent.Address;
                }
            }
            else if (dto.RoleName == "SchoolNurse")
            {
                var nurse = await _unitOfWork.SchoolNurseRepository.GetAsync(n => n.UserId == userId);
                if (nurse != null)
                {
                    dto.FullName = nurse.FullName;
                    dto.PhoneNumber = nurse.PhoneNumber;
                }
            }

            return dto;
        }
        public async Task<ServiceResult> UpdateProfileAsync(int userId, UserProfileDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return ServiceResult.FailureResult("User not found.");

            var role = user.Role?.ToLower();
            user.Username = dto.Username ?? user.Username;
            user.Email = dto.Email ?? user.Email;

            if (role == "Parent")
            {
                var parent = await _unitOfWork.ParentRepository.GetAsync(p => p.UserId == userId);
                if (parent != null)
                {
                    parent.FullName = dto.FullName ?? parent.FullName;
                    parent.PhoneNumber = dto.PhoneNumber ?? parent.PhoneNumber;
                    parent.Address = dto.Address ?? parent.Address;
                    _unitOfWork.ParentRepository.Update(parent);
                }
            }
            else if (role == "SchoolNurse")
            {
                var nurse = await _unitOfWork.SchoolNurseRepository.GetAsync(n => n.UserId == userId);
                if (nurse != null)
                {
                    nurse.FullName = dto.FullName ?? nurse.FullName;
                    nurse.PhoneNumber = dto.PhoneNumber ?? nurse.PhoneNumber;
                    _unitOfWork.SchoolNurseRepository.Update(nurse);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessResult("Profile updated.");
        }

        public async Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return ServiceResult.FailureResult("User not found.");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
            if (!isValid) return ServiceResult.FailureResult("Current password is incorrect.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.SuccessResult("Password updated successfully.");
        }
    }
}
