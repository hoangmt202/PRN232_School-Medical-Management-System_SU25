using BusinessObject.Entity;
using DataAccess.Repo;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWorks
    {
        IGenericRepository<Admin> AdminRepository { get; }
        IGenericRepository<DrugStorage> DrugStorageRepository { get; }
        IGenericRepository<HealthCheck> HealthCheckRepository { get; }
        IGenericRepository<IncidentReport> IncidentReportRepository { get; }
        IGenericRepository<Manager> ManagerRepository { get; }
        IMedicalRecordRepository MedicalRecordRepository { get; }
        IGenericRepository<Medication> MedicationRepository { get; }
        IGenericRepository<Parent> ParentRepository { get; }
        ISchoolNurseRepository SchoolNurseRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Vaccination> VaccinationRepository { get; }
        IGenericRepository<VaccinationNotice> VaccinationNoticeRepository { get; }
        Task<int> SaveChangesAsync();
        void Save();
    }
}
