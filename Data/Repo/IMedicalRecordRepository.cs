using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<MedicalRecord?> GetByStudentIdAsync(int studentId);
        Task<bool> ExistsByStudentIdAsync(int studentId);
    }
}
