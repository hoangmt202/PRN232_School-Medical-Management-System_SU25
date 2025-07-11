using AutoMapper;
using BusinessLogic.DTOs;
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
        public async Task<IEnumerable<VaccinationPlanDto>> GetAllPlansAsync()
        {
            var plans = await _unitOfWorks.VaccinationPlanRepository.GetAllAsync("Nurse");
            return _mapper.Map<IEnumerable<VaccinationPlanDto>>(plans);
        }
        public async Task<bool> AssignNurseToPlanAsync(int planId, int nurseId)
        {
            var plan = await _unitOfWorks.VaccinationPlanRepository.GetByIdAsync(planId);
            if (plan == null)
                return false;

            plan.AssignedNurseId = nurseId;
            _unitOfWorks.VaccinationPlanRepository.Update(plan);
            await _unitOfWorks.SaveChangesAsync();
            return true;
        }
        public async Task<VaccinationPlanDto?> GetByIdAsync(int id)
        {
            var plan = await _unitOfWorks.VaccinationPlanRepository.GetByIdAsync(id);
            return plan == null ? null : _mapper.Map<VaccinationPlanDto>(plan);
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
                    Id = n.VaccinationPlanId,
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
        public async Task<bool> UpdateVaccinationStatusAsync(int studentId, int planId, string status, DateTime? dateGiven)
        {
            var vaccination = await _unitOfWorks.VaccinationRepository.GetByStudentAndPlanAsync(studentId, planId);
            if (vaccination == null)
                return false;

            // Update status
            vaccination.Status = status;

            // If marked as Given, set DateGiven
            if (status == "Given")
            {
                vaccination.DateGiven = dateGiven ?? DateTime.Now;
            }
            else if (status == "Missed")
            {
                vaccination.DateGiven = null; // clear it if nurse switches back to missed
            }

            _unitOfWorks.VaccinationRepository.Update(vaccination);
            await _unitOfWorks.SaveChangesAsync();

            return true;
        }
        public async Task<bool> SubmitConsentAsync(int studentId, int planId, string response)
        {
            var notice = await _unitOfWorks.VaccinationNoticeRepository.GetByStudentAndPlanAsync(studentId, planId);
            if (notice == null) return false;

            notice.Response = response;
            _unitOfWorks.VaccinationNoticeRepository.Update(notice);
            await _unitOfWorks.VaccinationNoticeRepository.SaveChangesAsync();
            if (response == "Accepted")
            {
                var plan = await _unitOfWorks.VaccinationPlanRepository.GetByIdAsync(planId);
                if (plan == null) return false;

                // Check if already exists to avoid duplicates
                var exists = await _unitOfWorks.VaccinationRepository
                    .AnyAsync(v => v.StudentId == studentId && v.VaccinationPlanId == planId);

                if (!exists)
                {
                    var vaccination = new Vaccination
                    {
                        StudentId = studentId,
                        VaccinationPlanId = planId,
                        VaccineName = plan.VaccineName,
                        DateScheduled = plan.ScheduledDate,
                        Status = "Scheduled"
                    };

                    await _unitOfWorks.VaccinationRepository.AddAsync(vaccination);
                    await _unitOfWorks.VaccinationRepository.SaveChangesAsync();
                }
            }
                return true;
        }
        public async Task<bool> AddResultNoteAsync(int vaccinationId, string note)
        {
            var vaccination = await _unitOfWorks.VaccinationRepository.GetByIdAsync(vaccinationId);
            if (vaccination == null || string.IsNullOrWhiteSpace(note))
                return false;

            vaccination.ResultNote = note;
            _unitOfWorks.VaccinationRepository.Update(vaccination);
            await _unitOfWorks.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<StudentVaccinationStatusDto>> GetStudentsByPlanAsync(int planId)
        {
            var vaccinations = await _unitOfWorks.VaccinationRepository
                .FindAsync(v => v.VaccinationPlanId == planId, includeProperties: "Student");

            return vaccinations.Select(v => new StudentVaccinationStatusDto
            {
                StudentId = v.StudentId,
                StudentName = v.Student.FullName,
                Class = v.Student.Class,
                Status = v.Status,
                ScheduledDate = v.DateScheduled,
                DateGiven = v.DateGiven,
                ResultNote = v.ResultNote
            });
        }
        public async Task<bool> AddExternalVaccinationAsync(ExternalVaccinationDto dto)
        {
            var vaccine = new Vaccination
            {
                StudentId = dto.StudentId,
                VaccineName = dto.VaccineName,
                DateScheduled = dto.DateGiven,
                DateGiven = dto.DateGiven,
                ResultNote = dto.ResultNote,
                Status = "Given"
            };

            await _unitOfWorks.VaccinationRepository.AddAsync(vaccine);
            return await _unitOfWorks.VaccinationRepository.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<VaccinationRecordDto>> GetStudentVaccinationHistoryAsync(int parentId)
        {
            var students = await _unitOfWorks.StudentRepository.GetByParentIdAsync(parentId);
            var studentIds = students.Select(s => s.Id).ToList();

            var vaccinations = new List<Vaccination>();
            foreach (var id in studentIds)
            {
                var records = await _unitOfWorks.VaccinationRepository.GetByStudentIdAsync(id);
                vaccinations.AddRange(records);
            }

            var result = vaccinations.Select(v => new VaccinationRecordDto
            {
                Id = v.Id,
                VaccineName = v.VaccineName,
                DateGiven = v.DateGiven,
                Status = v.Status,
                ResultNote = v.ResultNote,
                StudentName = v.Student?.FullName ?? "Unknown"
            });

            return result;
        }
        public async Task<List<VaccinationPlanDto>> GetPlansByNurseAsync(int nurseId)
        {
            var plans = await _unitOfWorks.VaccinationPlanRepository.GetByNurseAsync(nurseId);
            return _mapper.Map<List<VaccinationPlanDto>>(plans);
        }

        public async Task<bool> SendNoticesForPlanAsync(int planId)
        {
            var plan = await _unitOfWorks.VaccinationPlanRepository.GetByIdAsync(planId);
            if (plan == null) return false;

            // Get students eligible (e.g., Grade or Age match)
            var allStudents = await _unitOfWorks.StudentRepository.GetEligibleStudentsForPlanAsync(plan);

            foreach (var student in allStudents)
            {
                var existing = await _unitOfWorks.VaccinationNoticeRepository
                    .GetByStudentAndPlanAsync(student.Id, planId);
                if (existing != null) continue;

                var notice = new VaccinationNotice
                {
                    StudentId = student.Id,
                    DateSent = DateTime.Now,
                    Response = "No Response",
                    VaccinationPlanId = plan.Id
                };

                await _unitOfWorks.VaccinationNoticeRepository.AddAsync(notice);
            }

            await _unitOfWorks.VaccinationNoticeRepository.SaveChangesAsync();
            return true;
        }
    }
        
}
