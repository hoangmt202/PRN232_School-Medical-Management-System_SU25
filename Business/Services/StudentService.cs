using BusinessObject.Entity;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Parent> _parentRepository;
        public StudentService(IGenericRepository<Student> studentRepository, IGenericRepository<Parent> parentRepository)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync("Parent");
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetAsync(s => s.Id == id, "Parent");
        }

        public async Task AddStudentAsync(Student student)
        {
            await _studentRepository.AddAsync(student);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _studentRepository.Update(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student != null)
            {
                _studentRepository.Delete(student);
            }
        }
        public async Task<IEnumerable<Student>> GetStudentsByParentUserIdAsync(int parentUserId)
        {
            var parent = await _parentRepository.GetAsync(p => p.UserId == parentUserId);
            if (parent == null)
            {
                return Enumerable.Empty<Student>();
            }

            // Step 2: Get students by parent ID
            return await _studentRepository.FindAsync(s => s.ParentId == parent.Id, "Parent");
        }
    }
}
