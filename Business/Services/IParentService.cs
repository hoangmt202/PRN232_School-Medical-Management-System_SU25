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
        Task<Parent> GetParentByUserId(int UserId);
        Task<List<Parent>> GetAll();
        Task<Parent> CreateParentWithUser(AddParentDTO parentDTO);
        Task<Parent> Update(ParentDTO parentDTO);
        Task<Parent> Delete(int id);
        
    }
}
