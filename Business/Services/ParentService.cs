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
        private readonly DataAccess.IUnitOfWorks _unitOfWork;

        public ParentService(DataAccess.IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ParentResponseDTO> CreateParentWithUser(ParentRequestDTO parentDTO)
        {
            var parent = new Parent
            {
                FullName = parentDTO.FullName,
                PhoneNumber = parentDTO.PhoneNumber,
                Address = parentDTO.Address,
                UserId = parentDTO.UserId
            };

            await _unitOfWork.ParentRepository.AddAsync(parent);
            await _unitOfWork.SaveChangesAsync();

            return await GetParentById(parent.Id);
        }

        public async Task<ParentResponseDTO> Delete(int id)
        {
            var parent = await _unitOfWork.ParentRepository.GetAsync(
                p => p.Id == id,
                "User,Students");

            if (parent == null)
                throw new ArgumentException("Parent not found");

            _unitOfWork.ParentRepository.Delete(parent);
            await _unitOfWork.SaveChangesAsync();

            return new ParentResponseDTO
            {
                Id = parent.Id,
                FullName = parent.FullName,
                PhoneNumber = parent.PhoneNumber,
                Address = parent.Address,
                UserId = parent.UserId,
                User = parent.User,
                Students = parent.Students
            };
        }

        public async Task<List<ParentResponseDTO>> GetAll()
        {
            var parents = await _unitOfWork.ParentRepository.GetAllAsync("User,Students");

            return parents.Select(p => new ParentResponseDTO
            {
                Id = p.Id,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                UserId = p.UserId,
                User = p.User,
                Students = p.Students
            }).ToList();
        }

        public async Task<ParentResponseDTO> GetParentByUserId(int UserId)
        {
            var parent = await _unitOfWork.ParentRepository.GetAsync(
                p => p.UserId == UserId,
                "User,Students");

            if (parent == null)
                return null;

            return new ParentResponseDTO
            {
                Id = parent.Id,
                FullName = parent.FullName,
                PhoneNumber = parent.PhoneNumber,
                Address = parent.Address,
                UserId = parent.UserId,
                User = parent.User,
                Students = parent.Students
            };
        }

        public async Task<ParentResponseDTO> Update(int id, ParentRequestDTO parentDTO)
        {
            var parent = await _unitOfWork.ParentRepository.GetAsync(
                p => p.Id == id,
                "User,Students");

            if (parent == null)
                throw new ArgumentException("Parent not found");

            parent.FullName = parentDTO.FullName;
            parent.PhoneNumber = parentDTO.PhoneNumber;
            parent.Address = parentDTO.Address;
            parent.UserId = parentDTO.UserId;

            _unitOfWork.ParentRepository.Update(parent);
            await _unitOfWork.SaveChangesAsync();

            return new ParentResponseDTO
            {
                Id = parent.Id,
                FullName = parent.FullName,
                PhoneNumber = parent.PhoneNumber,
                Address = parent.Address,
                UserId = parent.UserId,
                User = parent.User,
                Students = parent.Students
            };
        }

        public async Task<ParentResponseDTO> GetParentById(int id)
        {
            var parent = await _unitOfWork.ParentRepository.GetAsync(
                p => p.Id == id,
                "User,Students");

            if (parent == null)
                return null;

            return new ParentResponseDTO
            {
                Id = parent.Id,
                FullName = parent.FullName,
                PhoneNumber = parent.PhoneNumber,
                Address = parent.Address,
                UserId = parent.UserId,
                User = parent.User,
                Students = parent.Students
            };
        }
    }
}
