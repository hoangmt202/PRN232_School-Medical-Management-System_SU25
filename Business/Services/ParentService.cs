using BusinessLogic.DTOs;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ParentService : IParentService
    {
        private readonly SchoolMedicalDbContext _context;

        public ParentService(SchoolMedicalDbContext context)
        {
            _context = context;
        }

        public Task<Parent> CreateParentWithUser(AddParentDTO parentDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Parent> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Parent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Parent> GetParentByUserId(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<Parent> Update(ParentDTO parentDTO)
        {
            throw new NotImplementedException();
        }
    }
}
