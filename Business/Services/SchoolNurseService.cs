using BusinessLogic.DTOs;
using BusinessObject.Entity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class SchoolNurseService : ISchoolNurseService
    {
        private readonly IUnitOfWorks _unitOfWork;

        public SchoolNurseService(IUnitOfWorks unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SchoolNurseResponseDTO> CreateSchoolNurse(SchoolNurseRequestDTO nurseDTO)
        {
            var nurse = new SchoolNurse
            {
                FullName = nurseDTO.FullName,
                PhoneNumber = nurseDTO.PhoneNumber,
                UserId = nurseDTO.UserId
            };
            await _unitOfWork.SchoolNurseRepository.AddAsync(nurse);
            await _unitOfWork.SaveChangesAsync();
            return await GetSchoolNurseById(nurse.Id);
        }

        public async Task<SchoolNurseResponseDTO> Delete(int id)
        {
            var nurse = await _unitOfWork.SchoolNurseRepository.GetAsync(
                n => n.Id == id,
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            if (nurse == null)
                throw new ArgumentException("School nurse not found");
            _unitOfWork.SchoolNurseRepository.Delete(nurse);
            await _unitOfWork.SaveChangesAsync();
            return new SchoolNurseResponseDTO
            {
                Id = nurse.Id,
                FullName = nurse.FullName,
                PhoneNumber = nurse.PhoneNumber,
                UserId = nurse.UserId,
                User = nurse.User,
                GivenMedications = nurse.GivenMedications,
                IncidentReports = nurse.IncidentReports,
                ManagedDrugs = nurse.ManagedDrugs
            };
        }

        public async Task<List<SchoolNurseResponseDTO>> GetAll()
        {
            var nurses = await _unitOfWork.SchoolNurseRepository.GetAllAsync(
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            return nurses.Select(n => new SchoolNurseResponseDTO
            {
                Id = n.Id,
                FullName = n.FullName,
                PhoneNumber = n.PhoneNumber,
                UserId = n.UserId,
                User = n.User,
                GivenMedications = n.GivenMedications,
                IncidentReports = n.IncidentReports,
                ManagedDrugs = n.ManagedDrugs
            }).ToList();
        }

        public async Task<SchoolNurseResponseDTO> GetSchoolNurseById(int id)
        {
            var nurse = await _unitOfWork.SchoolNurseRepository.GetAsync(
                n => n.Id == id,
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            if (nurse == null)
                return null;
            return new SchoolNurseResponseDTO
            {
                Id = nurse.Id,
                FullName = nurse.FullName,
                PhoneNumber = nurse.PhoneNumber,
                UserId = nurse.UserId,
                User = nurse.User,
                GivenMedications = nurse.GivenMedications,
                IncidentReports = nurse.IncidentReports,
                ManagedDrugs = nurse.ManagedDrugs
            };
        }

        public async Task<SchoolNurseResponseDTO> Update(int id, SchoolNurseRequestDTO nurseDTO)
        {
            var nurse = await _unitOfWork.SchoolNurseRepository.GetAsync(
                n => n.Id == id,
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            if (nurse == null)
                throw new ArgumentException("School nurse not found");
            nurse.FullName = nurseDTO.FullName;
            nurse.PhoneNumber = nurseDTO.PhoneNumber;
            nurse.UserId = nurseDTO.UserId;
            _unitOfWork.SchoolNurseRepository.Update(nurse);
            await _unitOfWork.SaveChangesAsync();
            return new SchoolNurseResponseDTO
            {
                Id = nurse.Id,
                FullName = nurse.FullName,
                PhoneNumber = nurse.PhoneNumber,
                UserId = nurse.UserId,
                User = nurse.User,
                GivenMedications = nurse.GivenMedications,
                IncidentReports = nurse.IncidentReports,
                ManagedDrugs = nurse.ManagedDrugs
            };
        }
    }
} 