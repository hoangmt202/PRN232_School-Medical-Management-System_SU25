using BusinessObject.Entity;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IGenericRepository<Medication> _medicationRepository;

        public MedicationService(IGenericRepository<Medication> medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
        {
            return await _medicationRepository.GetAllAsync("Student,Nurse");
        }

        public async Task<Medication> GetMedicationByIdAsync(int id)
        {
            return await _medicationRepository.GetAsync(m => m.Id == id, "Student,Nurse");
        }

        public async Task AddMedicationAsync(Medication medication)
        {
            await _medicationRepository.AddAsync(medication);
        }

        public async Task UpdateMedicationAsync(Medication medication)
        {
            _medicationRepository.Update(medication);
        }

        public async Task DeleteMedicationAsync(int id)
        {
            var medication = await _medicationRepository.GetByIdAsync(id);
            if (medication != null)
            {
                _medicationRepository.Delete(medication);
            }
        }
    }
}