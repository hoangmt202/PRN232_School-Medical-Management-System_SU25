using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class SchoolNurseRepository : GenericRepository<SchoolNurse>, ISchoolNurseRepository
    {
        public SchoolNurseRepository(SchoolMedicalDbContext context) : base(context)
        {
        }

        public async Task<SchoolNurse?> GetByUserIdAsync(int userId)
        {
            return await GetAsync(sn => sn.UserId == userId, "User");
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await AnyAsync(sn => sn.UserId == userId);
        }
    }
}
