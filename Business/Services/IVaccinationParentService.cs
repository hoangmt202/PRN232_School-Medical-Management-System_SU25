using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IVaccinationParentService
    {
        Task<IEnumerable<VaccinationPlanDto>> GetAllPlansAsync();
        Task<VaccinationPlanDto?> GetByIdAsync(int id);
        Task<bool> AssignNurseToPlanAsync(int planId, int nurseId);
        Task<IEnumerable<VaccinationPlanDto>> GetUpcomingPlansAsync(int parentId);
        Task<bool> SubmitConsentAsync(int studentId, int planId, string response);
        Task<IEnumerable<VaccinationRecordDto>> GetStudentVaccinationHistoryAsync(int parentId);
        Task<List<VaccinationPlanDto>> GetPlansByNurseAsync(int nurseId);
        Task<bool> SendNoticesForPlanAsync(int planId);
        Task<IEnumerable<StudentVaccinationStatusDto>> GetStudentsByPlanAsync(int planId);
        Task<bool> AddExternalVaccinationAsync(ExternalVaccinationDto dto);
        Task<bool> AddResultNoteAsync(int vaccinationId, string note);
        Task<bool> UpdateVaccinationStatusAsync(int studentId, int planId, string status, DateTime? dateGiven);
    }
}
