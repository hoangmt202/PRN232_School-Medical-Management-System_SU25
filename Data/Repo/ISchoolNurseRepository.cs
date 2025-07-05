using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public interface ISchoolNurseRepository  : IGenericRepository<SchoolNurse>
    {
        Task<SchoolNurse?> GetByUserIdAsync(int userId);
        Task<bool> ExistsByUserIdAsync(int userId);
    }
}
