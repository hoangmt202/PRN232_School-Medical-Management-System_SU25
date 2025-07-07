using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents
{
    //[Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ParentResponseDTO Parent { get; set; } = new ParentResponseDTO();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/parent/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Parent = JsonSerializer.Deserialize<ParentResponseDTO>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (Parent == null)
                    {
                        return NotFound();
                    }
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
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
                    UserId = Parent.UserId,
                    User = Parent.User
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(parentDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync($"http://localhost:5234/api/parent/{Parent.Id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the parent. Please try again.");
                    return Page();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the parent. Please try again.");
                return Page();
            }
        }
    }
} 