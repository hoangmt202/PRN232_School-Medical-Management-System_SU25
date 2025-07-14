using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.MedicalRecords
{
    public class MyChildrenModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MyChildrenModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<StudentResponseDTO> Children { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync(int? parentId)
        {
            try
            {
                int? resolvedParentId = parentId;
                if (!resolvedParentId.HasValue)
                {
                    var userIdStr = HttpContext.Session.GetString("UserId");
                    if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                    {
                        ErrorMessage = "User not logged in.";
                        return;
                    }
                    var client = _httpClientFactory.CreateClient("API");
                    var parentResp = await client.GetAsync($"Parent/user/{userId}");
                    if (!parentResp.IsSuccessStatusCode)
                    {
                        ErrorMessage = "Failed to get parent info.";
                        return;
                    }
                    var parentJson = await parentResp.Content.ReadAsStringAsync();
                    var parent = JsonSerializer.Deserialize<ParentResponseDTO>(parentJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (parent == null)
                    {
                        ErrorMessage = "Parent info not found.";
                        return;
                    }
                    resolvedParentId = parent.Id;
                }
                var apiClient = _httpClientFactory.CreateClient("API");
                var response = await apiClient.GetAsync($"Student/children/{resolvedParentId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Children = JsonSerializer.Deserialize<List<StudentResponseDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                else
                {
                    ErrorMessage = "Failed to load children.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
} 