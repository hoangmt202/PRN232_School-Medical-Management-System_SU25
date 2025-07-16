using BusinessLogic.DTOs;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IHealthCheckService
    {
        Task<IEnumerable<HealthCheckDto>> GetAllHealthChecksAsync();
        Task<HealthCheckDto?> GetHealthCheckByIdAsync(int id);
        Task<IEnumerable<HealthCheckDto>> GetHealthChecksByStudentIdAsync(int studentId);
        Task<HealthCheckDto> CreateHealthCheckAsync(CreateHealthCheckDto createDto);
        Task<HealthCheckDto?> UpdateHealthCheckAsync(int id, UpdateHealthCheckDto updateDto);
        Task<bool> DeleteHealthCheckAsync(int id);
    }
} 