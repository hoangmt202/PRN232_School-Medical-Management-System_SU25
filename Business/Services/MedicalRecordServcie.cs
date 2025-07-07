using BusinessLogic.DTOs.MedicalRecord;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWorks _repo;

        public MedicalRecordService(IUnitOfWorks repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
        {
            var medicalRecords = await _repo.MedicalRecordRepository.GetAllAsync("Student");
            return medicalRecords.Select(MapToDto);
        }

        public async Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(int id)
        {
            var medicalRecord = await _repo.MedicalRecordRepository.GetByIdAsync(id);
            if (medicalRecord != null)
            {
                // Load student navigation property
                var recordWithStudent = await _repo.MedicalRecordRepository.GetAsync(mr => mr.Id == id, "Student");
                return MapToDto(recordWithStudent);
            }
            return null;
        }

        public async Task<MedicalRecordDto?> GetMedicalRecordByStudentIdAsync(int studentId)
        {
            var medicalRecord = await _repo.MedicalRecordRepository.GetByStudentIdAsync(studentId);
            return medicalRecord != null ? MapToDto(medicalRecord) : null;
        }

        public async Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto)
        {
            // Check if medical record already exists for this student
            if (await _repo.MedicalRecordRepository.ExistsByStudentIdAsync(createDto.StudentId))
            {
                throw new InvalidOperationException($"Medical record already exists for student ID {createDto.StudentId}");
            }

            var medicalRecord = new MedicalRecord
            {
                StudentId = createDto.StudentId,
                Allergies = createDto.Allergies,
                ChronicDiseases = createDto.ChronicDiseases,
                TreatmentHistory = createDto.TreatmentHistory,
                PhysicalCondition = createDto.PhysicalCondition
            };

            await _repo.MedicalRecordRepository.AddAsync(medicalRecord);
            await _repo.SaveChangesAsync();

            // Get the created record with navigation properties
            var createdRecord = await _repo.MedicalRecordRepository.GetByStudentIdAsync(createDto.StudentId);
            return MapToDto(createdRecord!);
        }

        public async Task<MedicalRecordDto?> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDto updateDto)
        {
            var existingRecord = await _repo.MedicalRecordRepository.GetByIdAsync(id);
            if (existingRecord == null)
            {
                return null;
            }

            // Update only provided fields
            if (updateDto.Allergies != null)
                existingRecord.Allergies = updateDto.Allergies;
            if (updateDto.ChronicDiseases != null)
                existingRecord.ChronicDiseases = updateDto.ChronicDiseases;
            if (updateDto.TreatmentHistory != null)
                existingRecord.TreatmentHistory = updateDto.TreatmentHistory;
            if (updateDto.PhysicalCondition != null)
                existingRecord.PhysicalCondition = updateDto.PhysicalCondition;

            _repo.MedicalRecordRepository.Update(existingRecord);
            await _repo.SaveChangesAsync();

            // Get updated record with navigation properties
            var updatedRecord = await _repo.MedicalRecordRepository.GetAsync(mr => mr.Id == id, "Student");
            return MapToDto(updatedRecord);
        }

        public async Task<bool> DeleteMedicalRecordAsync(int id)
        {
            var medicalRecord = await _repo.MedicalRecordRepository.GetByIdAsync(id);
            if (medicalRecord == null)
            {
                return false;
            }

            _repo.MedicalRecordRepository.Delete(medicalRecord);
            await _repo.SaveChangesAsync();
            return true;
        }

        private static MedicalRecordDto MapToDto(MedicalRecord medicalRecord)
        {
            return new MedicalRecordDto
            {
                Id = medicalRecord.Id,
                StudentId = medicalRecord.StudentId,
                StudentName = medicalRecord.Student?.FullName,
                Allergies = medicalRecord.Allergies,
                ChronicDiseases = medicalRecord.ChronicDiseases,
                TreatmentHistory = medicalRecord.TreatmentHistory,
                PhysicalCondition = medicalRecord.PhysicalCondition
            };
        }
    }
}
