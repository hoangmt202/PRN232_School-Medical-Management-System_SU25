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
    public class VaccinationRepository : GenericRepository<Vaccination>, IVaccinationRepository
    {
        public VaccinationRepository(SchoolMedicalDbContext context) : base(context) { }

        public async Task<IEnumerable<Vaccination>> GetByPlanIdAsync(int planId)
        {
            return await _dbSet
                .Include(v => v.Student)
                .Include(v => v.Plan)
                .Where(v => v.VaccinationPlanId == planId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vaccination>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet
                .Include(v => v.Plan)
                .Where(v => v.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vaccination>> GetCompletedByNurseAsync()
        {
            return await _dbSet
                .Include(v => v.Student)
                .Include(v => v.Plan)
                .Where(v => v.Status == "Completed")
                .ToListAsync();
        }
        public async Task<Vaccination?> GetByStudentAndPlanAsync(int studentId, int planId)
        {
            return await _dbSet
                .Include(n => n.Plan)
                .Include(n => n.Student)
                .FirstOrDefaultAsync(n => n.StudentId == studentId && n.VaccinationPlanId == planId);
        }
    }
}
