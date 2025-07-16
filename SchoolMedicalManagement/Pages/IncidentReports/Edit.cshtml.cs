using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.IncidentReports
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public IncidentReportEditModel IncidentReport { get; set; } = new IncidentReportEditModel();

        public string StudentName { get; set; } = string.Empty;
        public string NurseName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5234/api/IncidentReport/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var incidentReport = JsonSerializer.Deserialize<IncidentReportDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (incidentReport != null)
                    {
                        IncidentReport.Id = incidentReport.Id;
                        IncidentReport.StudentId = incidentReport.StudentId;
                        IncidentReport.NurseId = incidentReport.NurseId;
                        IncidentReport.Date = incidentReport.Date;
                        IncidentReport.Type = incidentReport.Type;
                        IncidentReport.Description = incidentReport.Description;
                        IncidentReport.ActionTaken = incidentReport.ActionTaken;

                        StudentName = incidentReport.Student?.FullName ?? "Unknown Student";
                        NurseName = incidentReport.Nurse?.FullName ?? "Unknown Nurse";
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var incidentReportDTO = new UpdateIncidentReportDto
                {
                    Date = IncidentReport.Date,
                    Type = IncidentReport.Type,
                    Description = IncidentReport.Description,
                    ActionTaken = IncidentReport.ActionTaken
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(incidentReportDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync($"http://localhost:5234/api/IncidentReport/{IncidentReport.Id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the incident report. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the incident report. Please try again.");
                return Page();
            }
        }
    }

    public class IncidentReportEditModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int NurseId { get; set; }

        [Required(ErrorMessage = "Date and time is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Incident type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Action taken is required")]
        [StringLength(500, ErrorMessage = "Action taken cannot exceed 500 characters")]
        public string ActionTaken { get; set; } = string.Empty;
    }
} 