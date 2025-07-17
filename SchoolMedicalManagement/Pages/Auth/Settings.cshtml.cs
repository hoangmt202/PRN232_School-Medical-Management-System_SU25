using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class SettingsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettingsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin.";
                return Page();
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return Page();
            }

            if (newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = Request.Cookies["AuthToken"];
                
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                var changePasswordDto = new ChangePasswordDto
                {
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                };

                var json = JsonSerializer.Serialize(changePasswordDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/user/change-password", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = "Đổi mật khẩu thất bại. Vui lòng kiểm tra mật khẩu hiện tại.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại.";
            }

            return Page();
        }
    }
} 