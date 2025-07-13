using BusinessObject.Entity;
using DataAccess.Repo;
using DataAccess.Repo.Interface;
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
        IParentRepository ParentRepository { get; }
        ISchoolNurseRepository SchoolNurseRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IStudentRepository StudentRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IVaccinationRepository VaccinationRepository { get; }
        IVaccinationNoticeRepository VaccinationNoticeRepository { get; }
        IVaccinationPlanRepository VaccinationPlanRepository { get; }
        IGenericRepository<Student> StudentRepo {  get; }
        Task<int> SaveChangesAsync();
        void Save();
    }
}
