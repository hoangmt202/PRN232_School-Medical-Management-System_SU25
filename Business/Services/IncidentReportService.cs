using BusinessLogic.DTOs;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class IncidentReportService : IIncidentReportService
    {
        private readonly IUnitOfWorks _unitOfWork;

        public IncidentReportService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IncidentReportDto>> GetAllIncidentReportsAsync()
        {
            var incidentReports = await _unitOfWork.IncidentReportRepository.GetAllAsync("Student,Nurse");
            return incidentReports.Select(MapToDto);
        }

        public async Task<IncidentReportDto?> GetIncidentReportByIdAsync(int id)
        {
            var incidentReport = await _unitOfWork.IncidentReportRepository.GetAsync(ir => ir.Id == id, "Student,Nurse");
            return incidentReport != null ? MapToDto(incidentReport) : null;
        }

        public async Task<IEnumerable<IncidentReportDto>> GetIncidentReportsByNurseIdAsync(int nurseId)
        {
            var incidentReports = await _unitOfWork.IncidentReportRepository.FindAsync(ir => ir.NurseId == nurseId, "Student,Nurse");
            return incidentReports.Select(MapToDto);
        }

        public async Task<IEnumerable<IncidentReportDto>> GetIncidentReportsByStudentIdAsync(int studentId)
        {
            var incidentReports = await _unitOfWork.IncidentReportRepository.FindAsync(ir => ir.StudentId == studentId, "Student,Nurse");
            return incidentReports.Select(MapToDto);
        }

        public async Task<IncidentReportDto> CreateIncidentReportAsync(CreateIncidentReportDto createDto)
        {
            var incidentReport = new IncidentReport
            {
                StudentId = createDto.StudentId,
                NurseId = createDto.NurseId,
                Date = createDto.Date,
                Type = createDto.Type,
                Description = createDto.Description,
                ActionTaken = createDto.ActionTaken
            };

            await _unitOfWork.IncidentReportRepository.AddAsync(incidentReport);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var createdReport = await _unitOfWork.IncidentReportRepository.GetAsync(ir => ir.Id == incidentReport.Id, "Student,Nurse");
            return MapToDto(createdReport);
        }

        public async Task<IncidentReportDto?> UpdateIncidentReportAsync(int id, UpdateIncidentReportDto updateDto)
        {
            var incidentReport = await _unitOfWork.IncidentReportRepository.GetByIdAsync(id);
            if (incidentReport == null)
                return null;

            incidentReport.Date = updateDto.Date;
            incidentReport.Type = updateDto.Type;
            incidentReport.Description = updateDto.Description;
            incidentReport.ActionTaken = updateDto.ActionTaken;

            _unitOfWork.IncidentReportRepository.Update(incidentReport);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var updatedReport = await _unitOfWork.IncidentReportRepository.GetAsync(ir => ir.Id == id, "Student,Nurse");
            return MapToDto(updatedReport);
        }

        public async Task<bool> DeleteIncidentReportAsync(int id)
        {
            var incidentReport = await _unitOfWork.IncidentReportRepository.GetByIdAsync(id);
            if (incidentReport == null)
                return false;

            _unitOfWork.IncidentReportRepository.Delete(incidentReport);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static IncidentReportDto MapToDto(IncidentReport incidentReport)
        {
            return new IncidentReportDto
            {
                Id = incidentReport.Id,
                StudentId = incidentReport.StudentId,
                NurseId = incidentReport.NurseId,
                Date = incidentReport.Date,
                Type = incidentReport.Type,
                Description = incidentReport.Description,
                ActionTaken = incidentReport.ActionTaken,
                Student = incidentReport.Student,
                Nurse = incidentReport.Nurse
            };
        }
    }
} 