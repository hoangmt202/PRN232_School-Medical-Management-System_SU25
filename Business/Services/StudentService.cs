using BusinessLogic.DTOs;
using BusinessObject.Entity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWorks _unitOfWork;

        public StudentService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();
            return await GetStudentById(student.Id);
        }

        public async Task<StudentResponseDTO> Delete(int id)
        {
            var student = await _unitOfWork.StudentRepository.GetAsync(
                s => s.Id == id,
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            if (student == null)
                throw new ArgumentException("Student not found");
            _unitOfWork.StudentRepository.Delete(student);
            await _unitOfWork.SaveChangesAsync();
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
            var students = await _unitOfWork.StudentRepository.GetAllAsync(
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
            var student = await _unitOfWork.StudentRepository.GetAsync(
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

        public Task<IEnumerable<Student>> GetStudentsByParentUserIdAsync(int parentUserId)
        {
            return _unitOfWork.StudentRepository.GetByParentIdAsync(parentUserId);
        }

        public async Task<StudentResponseDTO> Update(int id, StudentRequestDTO studentDTO)
        {
            var student = await _unitOfWork.StudentRepository.GetAsync(
                s => s.Id == id,
                "Parent,MedicalRecord,Vaccinations,VaccinationNotices,Medications,IncidentReports,HealthChecks");
            if (student == null)
                throw new ArgumentException("Student not found");
            student.FullName = studentDTO.FullName;
            student.DateOfBirth = studentDTO.DateOfBirth;
            student.Gender = studentDTO.Gender;
            student.Class = studentDTO.Class;
            student.ParentId = studentDTO.ParentId;
            _unitOfWork.StudentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
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

