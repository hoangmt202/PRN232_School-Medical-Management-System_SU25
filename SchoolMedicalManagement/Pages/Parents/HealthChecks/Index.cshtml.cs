using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.Parents.HealthChecks
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public List<HealthCheckDto> HealthChecks { get; set; } = new List<HealthCheckDto>();
        public string ErrorMessage { get; set; } = string.Empty;
        public string DebugInfo { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            // Only allow Parents
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Parent")
            {
                return RedirectToPage("/Auth/Login");
            }

                            try
            {
                var client = _httpClientFactory.CreateClient("API");
                
                // Debug: Check if token exists in cookie
                var token = Request.Cookies["AuthToken"];
                var debugList = new List<string>();
                debugList.Add($"User Role: {userRole}");
                debugList.Add($"AuthToken Cookie: {(!string.IsNullOrEmpty(token) ? "Present" : "Missing")}");
                debugList.Add($"HTTP Client Base Address: {client.BaseAddress}");
                debugList.Add($"HTTP Client Default Headers Count: {client.DefaultRequestHeaders.Count()}");
                
                if (!string.IsNullOrEmpty(token))
                {
                    debugList.Add($"Token Length: {token.Length}");
                    debugList.Add($"Token Start: {token.Substring(0, Math.Min(30, token.Length))}...");
                }
                
                // Test debug endpoint first
                var debugResponse = await client.GetAsync("user/debug-token");
                debugList.Add($"Debug API Status: {debugResponse.StatusCode}");
                
                if (debugResponse.IsSuccessStatusCode)
                {
                    var debugContent = await debugResponse.Content.ReadAsStringAsync();
                    debugList.Add($"Debug Response: {debugContent}");
                }
                else
                {
                    var debugError = await debugResponse.Content.ReadAsStringAsync();
                    debugList.Add($"Debug Error: {debugError}");
                }
                
                // Test with manual Authorization header for comparison
                if (!string.IsNullOrEmpty(token))
                {
                    var manualClient = _httpClientFactory.CreateClient("API");
                    manualClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    
                    var manualResponse = await manualClient.GetAsync("user/debug-token");
                    debugList.Add($"Manual Auth Test Status: {manualResponse.StatusCode}");
                    
                    if (manualResponse.IsSuccessStatusCode)
                    {
                        var manualContent = await manualResponse.Content.ReadAsStringAsync();
                        debugList.Add($"Manual Auth Response: {manualContent}");
                    }
                }
                
                DebugInfo = string.Join("\n", debugList);
                _logger.LogInformation("Debug Info: {DebugInfo}", DebugInfo);
                
                // Try the actual health check endpoint
                var healthCheckResponse = await client.GetAsync("healthcheck/by-parent");
                debugList.Add($"Health Check API Status: {healthCheckResponse.StatusCode}");
                
                if (!healthCheckResponse.IsSuccessStatusCode)
                {
                    var errorContent = await healthCheckResponse.Content.ReadAsStringAsync();
                    debugList.Add($"Health Check Error: {errorContent}");
                    DebugInfo = string.Join("\n", debugList);
                    
                    if (healthCheckResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ErrorMessage = "Invalid or missing user authentication.";
                    }
                    else if (healthCheckResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ErrorMessage = "No children found for this parent.";
                    }
                    else
                    {
                        ErrorMessage = "Unable to load health check results.";
                    }
                    return Page();
                }

                var healthCheckJson = await healthCheckResponse.Content.ReadAsStringAsync();
                debugList.Add($"Health Check Response Length: {healthCheckJson.Length}");
                DebugInfo = string.Join("\n", debugList);
                
                var healthChecks = JsonSerializer.Deserialize<List<HealthCheckDto>>(healthCheckJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (healthChecks != null)
                {
                    HealthChecks = healthChecks
                        .OrderByDescending(healthCheck => healthCheck.Date)
                        .ToList();
                    debugList.Add($"Health Checks Found: {HealthChecks.Count}");
                }

                DebugInfo = string.Join("\n", debugList);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Health Checks page");
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
} 