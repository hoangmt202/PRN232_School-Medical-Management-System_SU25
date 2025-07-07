using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(SchoolMedicalDbContext context) : base(context)
        {
        }

        public async Task<MedicalRecord?> GetByStudentIdAsync(int studentId)
        {
            return await GetAsync(mr => mr.StudentId == studentId, "Student");
        }

        public async Task<bool> ExistsByStudentIdAsync(int studentId)
        {
            return await AnyAsync(mr => mr.StudentId == studentId);
        }
    }
}
