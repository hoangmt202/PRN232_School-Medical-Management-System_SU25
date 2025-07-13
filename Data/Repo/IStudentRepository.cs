using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetByParentIdAsync(int parentId);
        Task<List<Student>> GetEligibleStudentsForPlanAsync(VaccinationPlan plan);
    }
}
