using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo.Interface
{
    public interface IParentRepository : IGenericRepository<Parent>
    {
        Task<Parent?> GetByUserIdAsync(int userId);
    }
}
