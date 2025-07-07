using BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> GetStudentById(int id);
        Task<List<StudentResponseDTO>> GetAll();
        Task<StudentResponseDTO> CreateStudent(StudentRequestDTO studentDTO);
        Task<StudentResponseDTO> Update(int id, StudentRequestDTO studentDTO);
        Task<StudentResponseDTO> Delete(int id);
    }
} 