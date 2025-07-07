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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolMedicalDbContext context) : base(context) { }

        public async Task<IEnumerable<Student>> GetByParentIdAsync(int parentId)
        {
            return await _dbSet
                .Where(s => s.ParentId == parentId)
                .ToListAsync();
        }
    }
}
