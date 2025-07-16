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
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IUnitOfWorks _unitOfWork;

        public HealthCheckService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HealthCheckDto>> GetAllHealthChecksAsync()
        {
            var healthChecks = await _unitOfWork.HealthCheckRepository.GetAllAsync("Student");
            return healthChecks.Select(MapToDto);
        }

        public async Task<HealthCheckDto?> GetHealthCheckByIdAsync(int id)
        {
            var healthCheck = await _unitOfWork.HealthCheckRepository.GetAsync(hc => hc.Id == id, "Student");
            return healthCheck != null ? MapToDto(healthCheck) : null;
        }

        public async Task<IEnumerable<HealthCheckDto>> GetHealthChecksByStudentIdAsync(int studentId)
        {
            var healthChecks = await _unitOfWork.HealthCheckRepository.FindAsync(hc => hc.StudentId == studentId, "Student");
            return healthChecks.Select(MapToDto);
        }

        public async Task<HealthCheckDto> CreateHealthCheckAsync(CreateHealthCheckDto createDto)
        {
            var healthCheck = new HealthCheck
            {
                StudentId = createDto.StudentId,
                Date = createDto.Date,
                CheckType = createDto.CheckType,
                Results = createDto.Results,
                Notes = createDto.Notes
            };

            await _unitOfWork.HealthCheckRepository.AddAsync(healthCheck);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var createdCheck = await _unitOfWork.HealthCheckRepository.GetAsync(hc => hc.Id == healthCheck.Id, "Student");
            return MapToDto(createdCheck);
        }

        public async Task<HealthCheckDto?> UpdateHealthCheckAsync(int id, UpdateHealthCheckDto updateDto)
        {
            var healthCheck = await _unitOfWork.HealthCheckRepository.GetByIdAsync(id);
            if (healthCheck == null)
                return null;

            healthCheck.Date = updateDto.Date;
            healthCheck.CheckType = updateDto.CheckType;
            healthCheck.Results = updateDto.Results;
            healthCheck.Notes = updateDto.Notes;

            _unitOfWork.HealthCheckRepository.Update(healthCheck);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var updatedCheck = await _unitOfWork.HealthCheckRepository.GetAsync(hc => hc.Id == id, "Student");
            return MapToDto(updatedCheck);
        }

        public async Task<bool> DeleteHealthCheckAsync(int id)
        {
            var healthCheck = await _unitOfWork.HealthCheckRepository.GetByIdAsync(id);
            if (healthCheck == null)
                return false;

            _unitOfWork.HealthCheckRepository.Delete(healthCheck);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static HealthCheckDto MapToDto(HealthCheck healthCheck)
        {
            return new HealthCheckDto
            {
                Id = healthCheck.Id,
                StudentId = healthCheck.StudentId,
                Date = healthCheck.Date,
                CheckType = healthCheck.CheckType,
                Results = healthCheck.Results,
                Notes = healthCheck.Notes,
                Student = healthCheck.Student
            };
        }
    }
} 