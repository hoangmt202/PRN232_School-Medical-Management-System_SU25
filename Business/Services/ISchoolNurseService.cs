
using BusinessLogic.DTOs.SchoolNurse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ISchoolNurseService
    {
        Task<SchoolNurseResponseDTO> GetSchoolNurseById(int id);
        Task<List<SchoolNurseResponseDTO>> GetAll();
        Task<SchoolNurseResponseDTO> CreateSchoolNurse(SchoolNurseRequestDTO nurseDTO);
        Task<SchoolNurseResponseDTO> Update(int id, SchoolNurseRequestDTO nurseDTO);
        Task<SchoolNurseResponseDTO> Delete(int id);
    }
}
