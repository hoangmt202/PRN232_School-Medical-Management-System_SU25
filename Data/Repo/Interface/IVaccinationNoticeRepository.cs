using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo.Interface
{
    public interface IVaccinationNoticeRepository : IGenericRepository<VaccinationNotice>
    {
        Task<VaccinationNotice?> GetByStudentAndPlanAsync(int studentId, int planId);
        Task<IEnumerable<VaccinationNotice>> GetByParentIdAsync(int parentId);
    }
}
