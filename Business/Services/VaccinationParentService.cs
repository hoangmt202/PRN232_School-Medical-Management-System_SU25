using AutoMapper;
using BusinessLogic.DTOs.Vaccination;
using BusinessLogic.Services.Interface;
using BusinessObject.Entity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class VaccinationParentService : IVaccinationParentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public VaccinationParentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<IEnumerable<VaccinationPlanDto>> GetUpcomingPlansAsync(int parentId)
        {
            var students = await _unitOfWorks.StudentRepository.GetByParentIdAsync(parentId);
            var studentIds = students.Select(s => s.Id).ToList();

            var notices = await _unitOfWorks.VaccinationNoticeRepository.GetByParentIdAsync(parentId);
            var plans = notices
                .Where(n => studentIds.Contains(n.StudentId))
                .Select(n => new VaccinationPlanDto
                {
                    PlanId = n.VaccinationPlanId,
                    VaccineName = n.Plan?.VaccineName ?? "",
                    ScheduledDate = n.Plan?.ScheduledDate ?? DateTime.MinValue,
                    TargetGroup = n.Plan?.TargetGroup ?? "",
                    Notes = n.Plan?.Notes ?? "",
                    StudentId = n.StudentId,
                    StudentName = n.Student?.FullName ?? "",
                    Response = n.Response
                }).ToList();

            return plans;
        }

        public async Task<bool> SubmitConsentAsync(int studentId, int planId, string response)
        {
            var notice = await _unitOfWorks.VaccinationNoticeRepository.GetByStudentAndPlanAsync(studentId, planId);
            if (notice == null) return false;

            notice.Response = response;
            _unitOfWorks.VaccinationNoticeRepository.Update(notice);
            await _unitOfWorks.VaccinationNoticeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VaccinationRecordDto>> GetStudentVaccinationHistoryAsync(int parentId)
        {
            var students = await _unitOfWorks.StudentRepository.GetByParentIdAsync(parentId);
            var studentIds = students.Select(s => s.Id).ToList();

            var vaccinations = new List<Vaccination>();
            foreach (var id in studentIds)
            {
                var records = await _unitOfWorks.Vaccination.GetByStudentIdAsync(id);
                vaccinations.AddRange(records);
            }

            return _mapper.Map<IEnumerable<VaccinationRecordDto>>(vaccinations);
        }
    }
}
