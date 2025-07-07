using BusinessLogic.DTOs;
using BusinessLogic.DTOs.SchoolNurse;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.Repo;
using DataAccess.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class SchoolNurseService : ISchoolNurseService
    {
        private readonly IUnitOfWorks _repo;

        public SchoolNurseService(IUnitOfWorks repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SchoolNurseDto>> GetAllSchoolNursesAsync()
        {
            var schoolNurses = await _repo.SchoolNurseRepository.GetAllAsync("User");
            return schoolNurses.Select(MapToDto);
        }

        public async Task<SchoolNurseDto?> GetSchoolNurseByIdAsync(int id)
        {
            var schoolNurse = await _repo.SchoolNurseRepository.GetByIdAsync(id);
            if (schoolNurse != null)
            {
                // Load user navigation property
                var nurseWithUser = await _repo.SchoolNurseRepository.GetAsync(sn => sn.Id == id, "User");
                return MapToDto(nurseWithUser);
            }
            return null;
        }

        public async Task<SchoolNurseDto?> GetSchoolNurseByUserIdAsync(int userId)
        {
            var schoolNurse = await _repo.SchoolNurseRepository.GetByUserIdAsync(userId);
            return schoolNurse != null ? MapToDto(schoolNurse) : null;
        }

        public async Task<SchoolNurseDto> CreateSchoolNurseAsync(CreateSchoolNurseDto createDto)
        {
            // Check if school nurse already exists for this user
            if (await _repo.SchoolNurseRepository.ExistsByUserIdAsync(createDto.UserId))
            {
                throw new InvalidOperationException($"School nurse already exists for user ID {createDto.UserId}");
            }

            var schoolNurse = new SchoolNurse
            {
                FullName = createDto.FullName,
                PhoneNumber = createDto.PhoneNumber,
                UserId = createDto.UserId
            };

            await _repo.SchoolNurseRepository.AddAsync(schoolNurse);
            await _repo.SaveChangesAsync();

            // Get the created nurse with navigation properties
            var createdNurse = await _repo.SchoolNurseRepository.GetByUserIdAsync(createDto.UserId);
            return MapToDto(createdNurse!);
        }

        public async Task<SchoolNurseDto?> UpdateSchoolNurseAsync(int id, UpdateSchoolNurseDto updateDto)
        {
            var existingNurse = await _repo.SchoolNurseRepository.GetByIdAsync(id);
            if (existingNurse == null)
            {
                return null;
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateDto.FullName))
                existingNurse.FullName = updateDto.FullName;
            if (updateDto.PhoneNumber != null)
                existingNurse.PhoneNumber = updateDto.PhoneNumber;

            _repo.SchoolNurseRepository.Update(existingNurse);
            await _repo.SaveChangesAsync();

            // Get updated nurse with navigation properties
            var updatedNurse = await _repo.SchoolNurseRepository.GetAsync(sn => sn.Id == id, "User");
            return MapToDto(updatedNurse);
        }

        public async Task<bool> DeleteSchoolNurseAsync(int id)
        {
            var schoolNurse = await _repo.SchoolNurseRepository.GetByIdAsync(id);
            if (schoolNurse == null)
            {
                return false;
            }

            _repo.SchoolNurseRepository.Delete(schoolNurse);
            await _repo.SaveChangesAsync();
            return true;
        }

        private static SchoolNurseDto MapToDto(SchoolNurse schoolNurse)
        {
            return new SchoolNurseDto
            {
                Id = schoolNurse.Id,
                FullName = schoolNurse.FullName,
                PhoneNumber = schoolNurse.PhoneNumber,
                UserId = schoolNurse.UserId,
                Username = schoolNurse.User?.Username,
                Email = schoolNurse.User?.Email
            };
        }
        public async Task<SchoolNurseResponseDTO> CreateSchoolNurse(SchoolNurseRequestDTO nurseDTO)
        {
            var nurse = new SchoolNurse
            {
                FullName = nurseDTO.FullName,
                PhoneNumber = nurseDTO.PhoneNumber,
                UserId = nurseDTO.UserId
            };
            await _repo.SchoolNurseRepository.AddAsync(nurse);
            await _repo.SaveChangesAsync();
            return await GetSchoolNurseById(nurse.Id);
        }

        public async Task<SchoolNurseResponseDTO> Delete(int id)
        {
            var nurse = await _repo.SchoolNurseRepository.GetAsync(
                n => n.Id == id,
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            if (nurse == null)
                throw new ArgumentException("School nurse not found");
            _repo.SchoolNurseRepository.Delete(nurse);
            await _repo.SaveChangesAsync();
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
            var nurses = await _repo.SchoolNurseRepository.GetAllAsync(
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
            var nurse = await _repo.SchoolNurseRepository.GetAsync(
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
            var nurse = await _repo.SchoolNurseRepository.GetAsync(
                n => n.Id == id,
                "User,GivenMedications,IncidentReports,ManagedDrugs");
            if (nurse == null)
                throw new ArgumentException("School nurse not found");
            nurse.FullName = nurseDTO.FullName;
            nurse.PhoneNumber = nurseDTO.PhoneNumber;
            nurse.UserId = nurseDTO.UserId;
            _repo.SchoolNurseRepository.Update(nurse);
            await _repo.SaveChangesAsync();
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
