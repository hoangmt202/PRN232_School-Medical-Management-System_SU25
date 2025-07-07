using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo.Interface
{
    public interface IVaccinationRepository : IGenericRepository<Vaccination>
    {
        Task<IEnumerable<Vaccination>> GetByPlanIdAsync(int planId);
        Task<IEnumerable<Vaccination>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Vaccination>> GetCompletedByNurseAsync();
    }
}
