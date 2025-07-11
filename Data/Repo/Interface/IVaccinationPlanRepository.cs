using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo.Interface
{
    public interface IVaccinationPlanRepository : IGenericRepository<VaccinationPlan>
    {
        Task<IEnumerable<VaccinationPlan>> GetAllWithDetailsAsync();
        Task<VaccinationPlan?> GetByIdWithDetailsAsync(int id);
        Task<List<VaccinationPlan>> GetByNurseAsync(int nurseId);

    }
}
