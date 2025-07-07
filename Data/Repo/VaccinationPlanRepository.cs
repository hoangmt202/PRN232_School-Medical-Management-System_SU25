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
    public class VaccinationPlanRepository : GenericRepository<VaccinationPlan>, IVaccinationPlanRepository
    {
        public VaccinationPlanRepository(SchoolMedicalDbContext context) : base(context) { }

        public async Task<IEnumerable<VaccinationPlan>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Vaccinations)
                .Include(p => p.Notices)
                .OrderBy(p => p.ScheduledDate)
                .ToListAsync();
        }

        public async Task<VaccinationPlan?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Vaccinations)
                .Include(p => p.Notices)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
