using BusinessLogic.DTOs;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.Repo;
using DataAccess.UnitOfWorks;

namespace BusinessLogic.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Parent> _parentRepository;
        private readonly IUnitOfWorks _unitOfWorks;
        public StudentService(IGenericRepository<Student> studentRepository, IGenericRepository<Parent> parentRepository, IUnitOfWorks unitOfWorks)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
            _unitOfWorks = unitOfWorks;
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
        public async Task<StudentResponseDTO> CreateStudent(StudentRequestDTO studentDTO)
        {
            var student = new Student
            {
                FullName = studentDTO.FullName,
                DateOfBirth = studentDTO.DateOfBirth,
                Gender = studentDTO.Gender,
                Class = studentDTO.Class,
                ParentId = studentDTO.ParentId
            };
            await _unitOfWorks.StudentRepository.AddAsync(student);
            await _unitOfWorks.SaveChangesAsync();
            return await GetStudentById(student.Id);
        }

        public async Task<StudentResponseDTO> Delete(int id)
        {
            var student = await _unitOfWorks.StudentRepository.GetAsync(
                s => s.Id == id,
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            if (student == null)
                throw new ArgumentException("Student not found");
            _unitOfWorks.StudentRepository.Delete(student);
            await _unitOfWorks.SaveChangesAsync();
            return new StudentResponseDTO
            {
                Id = student.Id,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Class = student.Class,
                ParentId = student.ParentId,
                Parent = student.Parent,
                MedicalRecord = student.MedicalRecord,
                Vaccinations = student.Vaccinations,
                VaccinationNotices = student.VaccinationNotices,
                Medications = student.Medications,
                IncidentReports = student.IncidentReports,
                HealthChecks = student.HealthChecks
            };
        }

        public async Task<List<StudentResponseDTO>> GetAll()
        {
            var students = await _unitOfWorks.StudentRepository.GetAllAsync(
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            return students.Select(s => new StudentResponseDTO
            {
                Id = s.Id,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Gender = s.Gender,
                Class = s.Class,
                ParentId = s.ParentId,
                Parent = s.Parent,
                MedicalRecord = s.MedicalRecord,
                Vaccinations = s.Vaccinations,
                VaccinationNotices = s.VaccinationNotices,
                Medications = s.Medications,
                IncidentReports = s.IncidentReports,
                HealthChecks = s.HealthChecks
            }).ToList();
        }

        public async Task<StudentResponseDTO> GetStudentById(int id)
        {
            var student = await _unitOfWorks.StudentRepository.GetAsync(
                s => s.Id == id,
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            if (student == null)
                return null;
            return new StudentResponseDTO
            {
                Id = student.Id,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Class = student.Class,
                ParentId = student.ParentId,
                Parent = student.Parent,
                MedicalRecord = student.MedicalRecord,
                Vaccinations = student.Vaccinations,
                VaccinationNotices = student.VaccinationNotices,
                Medications = student.Medications,
                IncidentReports = student.IncidentReports,
                HealthChecks = student.HealthChecks
            };
        }

        public async Task<StudentResponseDTO> Update(int id, StudentRequestDTO studentDTO)
        {
            var student = await _unitOfWorks.StudentRepository.GetAsync(
                s => s.Id == id,
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            if (student == null)
                throw new ArgumentException("Student not found");
            student.FullName = studentDTO.FullName;
            student.DateOfBirth = studentDTO.DateOfBirth;
            student.Gender = studentDTO.Gender;
            student.Class = studentDTO.Class;
            student.ParentId = studentDTO.ParentId;
            _unitOfWorks.StudentRepository.Update(student);
            await _unitOfWorks.SaveChangesAsync();
            return new StudentResponseDTO
            {
                Id = student.Id,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Class = student.Class,
                ParentId = student.ParentId,
                Parent = student.Parent,
                MedicalRecord = student.MedicalRecord,
                Vaccinations = student.Vaccinations,
                VaccinationNotices = student.VaccinationNotices,
                Medications = student.Medications,
                IncidentReports = student.IncidentReports,
                HealthChecks = student.HealthChecks
            };
        }

    }
}
