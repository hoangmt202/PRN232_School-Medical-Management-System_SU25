using BusinessLogic.DTOs;
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
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDto dto);
        Task<UserProfileDto?> GetProfileByUserIdAsync(int userId);
        Task<ServiceResult> UpdateProfileAsync(int userId, UserProfileDto dto);
        Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
