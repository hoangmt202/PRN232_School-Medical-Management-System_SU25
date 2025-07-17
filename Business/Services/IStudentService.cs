using BusinessLogic.DTOs;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IStudentService
    {
        Task<List<StudentResponseDTO>> GetAllStudentsAsync();
        Task<StudentResponseDTO> GetStudentByIdAsync(int id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task<List<StudentResponseDTO>> GetStudentsByParentUserIdAsync(int parentUserId);
        Task<StudentResponseDTO> GetStudentById(int id);
        Task<List<StudentResponseDTO>> GetAll();
        Task<StudentResponseDTO> CreateStudent(StudentRequestDTO studentDTO);
        Task<StudentResponseDTO> Update(int id, StudentRequestDTO studentDTO);
        Task<StudentResponseDTO> Delete(int id);

    }
}
