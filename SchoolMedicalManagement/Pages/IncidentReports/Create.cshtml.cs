using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SchoolMedicalManagement.Pages.IncidentReports
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public IncidentReportCreateModel IncidentReport { get; set; } = new IncidentReportCreateModel();

        public SelectList StudentOptions { get; set; } = new SelectList(new List<SelectListItem>());
        public SelectList NurseOptions { get; set; } = new SelectList(new List<SelectListItem>());

        public async Task OnGetAsync()
        {
            await LoadDropdownData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData();
                return Page();
            }

            try
            {
                var incidentReportDTO = new CreateIncidentReportDto
                {
                    StudentId = IncidentReport.StudentId,
                    NurseId = IncidentReport.NurseId,
                    Date = IncidentReport.Date,
                    Type = IncidentReport.Type,
                    Description = IncidentReport.Description,
                    ActionTaken = IncidentReport.ActionTaken
                };

                var client = _httpClientFactory.CreateClient();
                var json = JsonSerializer.Serialize(incidentReportDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("http://localhost:5234/api/IncidentReport", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while creating the incident report. Please try again.");
                    await LoadDropdownData();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the incident report. Please try again.");
                await LoadDropdownData();
                return Page();
            }
        }

        private async Task LoadDropdownData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Load students
                var studentsResponse = await client.GetAsync("http://localhost:5234/api/student");
                if (studentsResponse.IsSuccessStatusCode)
                {
                    var studentsContent = await studentsResponse.Content.ReadAsStringAsync();
                    var students = JsonSerializer.Deserialize<List<StudentResponseDTO>>(studentsContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<StudentResponseDTO>();

                    StudentOptions = new SelectList(students, "Id", "FullName");
                }

                // Load nurses
                var nursesResponse = await client.GetAsync("http://localhost:5234/api/schoolnurse");
                if (nursesResponse.IsSuccessStatusCode)
                {
                    var nursesContent = await nursesResponse.Content.ReadAsStringAsync();
                    var nurses = JsonSerializer.Deserialize<List<SchoolNurseResponseDTO>>(nursesContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<SchoolNurseResponseDTO>();

                    NurseOptions = new SelectList(nurses, "Id", "FullName");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                StudentOptions = new SelectList(new List<SelectListItem>());
                NurseOptions = new SelectList(new List<SelectListItem>());
            }
        }
    }

    public class IncidentReportCreateModel
    {
        [Required(ErrorMessage = "Student is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Nurse is required")]
        public int NurseId { get; set; }

        [Required(ErrorMessage = "Date and time is required")]
        public DateTime Date { get; set; } = DateTime.Now;

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