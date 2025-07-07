using BusinessLogic.DTOs.MedicalRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync();
        Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(int id);
        Task<MedicalRecordDto?> GetMedicalRecordByStudentIdAsync(int studentId);
        Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto);
        Task<MedicalRecordDto?> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDto updateDto);
        Task<bool> DeleteMedicalRecordAsync(int id);
    }
}
