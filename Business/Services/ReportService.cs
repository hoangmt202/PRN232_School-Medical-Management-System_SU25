using BusinessLogic.DTOs;
using DataAccess;
using DataAccess.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWorks _unitOfWork;

        public ReportService(IUnitOfWorks unitOfWorks)
        {
            _unitOfWork = unitOfWorks;
        }
        public async Task<HealthTrendReportDto> GetHealthTrendReportAsync()
        {
            var checks = await _unitOfWork.HealthCheckRepository.GetAllAsync();
            return new HealthTrendReportDto
            {
                TotalHealthChecks = checks.Count(),
                CheckTypeCounts = checks.GroupBy(c => c.CheckType ?? "Unknown").ToDictionary(g => g.Key, g => g.Count()),
                ChecksPerMonth = checks.GroupBy(c => new DateTime(c.Date!.Year, c.Date.Month, 1))
                                        .ToDictionary(g => g.Key, g => g.Count())
            };
        }
        public async Task<IncidentStatisticsDto> GetIncidentStatsAsync()
        {
            var incidents = await _unitOfWork.IncidentReportRepository.GetAllAsync();

            return new IncidentStatisticsDto
            {
                TotalIncidents = incidents.Count(),
                TypeCounts = incidents.GroupBy(i => i.Type ?? "Unknown").ToDictionary(g => g.Key, g => g.Count()),
                MonthlyTrends = incidents.GroupBy(i => new DateTime(i.Date!.Year, i.Date.Month, 1))
                                         .ToDictionary(g => g.Key, g => g.Count())
            };
        }
        public async Task<ImmunizationCoverageDto> GetImmunizationCoverageAsync()
        {
            var students = await _unitOfWork.StudentRepository.GetAllAsync();
            var vaccinated = await _unitOfWork.VaccinationRepository.FindAsync(v => v.Status == "Given");

            var vaccinatedStudentIds = vaccinated.Select(v => v.StudentId).Distinct().Count();

            return new ImmunizationCoverageDto
            {
                TotalStudents = students.Count(),
                VaccinatedStudents = vaccinatedStudentIds
            };
        }
        public async Task<ParentalResponseDto> GetParentalResponseStatsAsync()
        {
            var notices = await _unitOfWork.VaccinationNoticeRepository.GetAllAsync();

            return new ParentalResponseDto
            {
                TotalNotices = notices.Count(),
                Accepted = notices.Count(n => n.Response == "Accepted"),
                Rejected = notices.Count(n => n.Response == "Rejected"),
                NoResponse = notices.Count(n => n.Response == "No Response" || string.IsNullOrEmpty(n.Response))
            };
        }
    }
}
