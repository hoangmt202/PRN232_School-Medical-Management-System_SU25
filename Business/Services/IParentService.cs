using BusinessLogic.DTOs;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IParentService
    {
        Task<ParentResponseDTO> GetParentById(int id);
        Task<ParentResponseDTO> GetParentByUserId(int UserId);
        Task<List<ParentResponseDTO>> GetAll();
        Task<ParentResponseDTO> CreateParentWithUser(ParentRequestDTO parentDTO);
        Task<ParentResponseDTO> Update(int id, ParentRequestDTO parentDTO);
        Task<ParentResponseDTO> Delete(int id);
        
    }
}
