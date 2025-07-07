using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents
{
    //[Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ParentCreateModel Parent { get; set; } = new ParentCreateModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var parentDTO = new ParentRequestDTO
                {
                    FullName = Parent.FullName,
                    PhoneNumber = Parent.PhoneNumber,
                    Address = Parent.Address,
                    User = new BusinessObject.Entity.User
                    {
                        Username = Parent.Username,
                        Email = Parent.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Parent.Password),
                        Role = "Parent",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(parentDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/parent", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while creating the parent. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the parent. Please try again.");
                return Page();
            }
        }
    }

    public class ParentCreateModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }
} 