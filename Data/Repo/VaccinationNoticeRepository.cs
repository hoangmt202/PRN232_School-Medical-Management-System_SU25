using BusinessObject.Entity;
using DataAccess.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class VaccinationNoticeRepository : GenericRepository<VaccinationNotice>  ,IVaccinationNoticeRepository
    {
        public VaccinationNoticeRepository(SchoolMedicalDbContext context) : base(context) { }

        public async Task<VaccinationNotice?> GetByStudentAndPlanAsync(int studentId, int planId)
        {
            return await _dbSet
                .Include(n => n.Plan)
                .Include(n => n.Student)
                .FirstOrDefaultAsync(n => n.StudentId == studentId && n.VaccinationPlanId == planId);
        }

        public async Task<IEnumerable<VaccinationNotice>> GetByParentIdAsync(int parentId)
        {
            return await _dbSet
                .Include(n => n.Plan)
                .Include(n => n.Student)
                .Where(n => n.Student.ParentId == parentId)
                .ToListAsync();
        }
    }
}
