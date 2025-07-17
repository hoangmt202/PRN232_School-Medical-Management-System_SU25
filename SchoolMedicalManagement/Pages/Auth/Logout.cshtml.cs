using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolMedicalManagement.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Remove authentication cookie
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies.Delete("AuthToken");
            }
            
            // Clear any other cookies if needed
            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie.StartsWith("Auth") || cookie.StartsWith("User"))
                {
                    Response.Cookies.Delete(cookie);
                }
            }
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            return await OnGetAsync();
        }
    }
} 