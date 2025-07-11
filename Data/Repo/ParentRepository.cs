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
    public class ParentRepository : GenericRepository<Parent>, IParentRepository
    {
        public ParentRepository(SchoolMedicalDbContext context) : base(context)
        {
        }
        public async Task<Parent?> GetByUserIdAsync(int userId)
        {
            return await _context.Parents
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
