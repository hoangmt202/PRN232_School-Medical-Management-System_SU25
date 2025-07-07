using BusinessLogic.DTOs.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interface
{
    public interface IVaccinationParentService
    {
        Task<IEnumerable<VaccinationPlanDto>> GetUpcomingPlansAsync(int parentId);
        Task<bool> SubmitConsentAsync(int studentId, int planId, string response);
        Task<IEnumerable<VaccinationRecordDto>> GetStudentVaccinationHistoryAsync(int parentId);
    }
}
