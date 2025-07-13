using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IReportService
    {
        Task<HealthTrendReportDto> GetHealthTrendReportAsync();
        Task<IncidentStatisticsDto> GetIncidentStatsAsync();
        Task<ImmunizationCoverageDto> GetImmunizationCoverageAsync();
        Task<ParentalResponseDto> GetParentalResponseStatsAsync();
    }
}
