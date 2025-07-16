using BusinessLogic.DTOs;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IIncidentReportService
    {
        Task<IEnumerable<IncidentReportDto>> GetAllIncidentReportsAsync();
        Task<IncidentReportDto?> GetIncidentReportByIdAsync(int id);
        Task<IEnumerable<IncidentReportDto>> GetIncidentReportsByNurseIdAsync(int nurseId);
        Task<IEnumerable<IncidentReportDto>> GetIncidentReportsByStudentIdAsync(int studentId);
        Task<IncidentReportDto> CreateIncidentReportAsync(CreateIncidentReportDto createDto);
        Task<IncidentReportDto?> UpdateIncidentReportAsync(int id, UpdateIncidentReportDto updateDto);
        Task<bool> DeleteIncidentReportAsync(int id);
    }
} 