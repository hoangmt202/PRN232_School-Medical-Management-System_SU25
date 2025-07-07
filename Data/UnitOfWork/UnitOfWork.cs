using BusinessObject.Entity;
using DataAccess.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWorks, IDisposable
    {
        private readonly SchoolMedicalDbContext _context;

        public UnitOfWork(SchoolMedicalDbContext context)
        {
            _context = context;
        }

        private IGenericRepository<Admin> _admins;
        private IGenericRepository<DrugStorage> _drugStorages;
        private IGenericRepository<HealthCheck> _healthChecks;
        private IGenericRepository<IncidentReport> _incidentReports;
        private IGenericRepository<Manager> _managers;
        private IGenericRepository<MedicalRecord> _medicalRecords;
        private IGenericRepository<Medication> _medications;
        private IGenericRepository<Parent> _parents;
        private IGenericRepository<SchoolNurse> _schoolNurses;
        private IGenericRepository<Student> _students;
        private IGenericRepository<User> _users;
        private IGenericRepository<Vaccination> _vaccinations;
        private IGenericRepository<VaccinationNotice> _vaccinationNotices;

        public IGenericRepository<Admin> AdminRepository => _admins ??= new GenericRepository<Admin>(_context);
        public IGenericRepository<DrugStorage> DrugStorageRepository => _drugStorages ??= new GenericRepository<DrugStorage>(_context);
        public IGenericRepository<HealthCheck> HealthCheckRepository => _healthChecks ??= new GenericRepository<HealthCheck>(_context);
        public IGenericRepository<IncidentReport> IncidentReportRepository => _incidentReports ??= new GenericRepository<IncidentReport>(_context);
        public IGenericRepository<Manager> ManagerRepository => _managers ??= new GenericRepository<Manager>(_context);
        public IGenericRepository<MedicalRecord> MedicalRecordRepository => _medicalRecords ??= new GenericRepository<MedicalRecord>(_context);
        public IGenericRepository<Medication> MedicationRepository => _medications ??= new GenericRepository<Medication>(_context);
        public IGenericRepository<Parent> ParentRepository => _parents ??= new GenericRepository<Parent>(_context);
        public IGenericRepository<SchoolNurse> SchoolNurseRepository => _schoolNurses ??= new GenericRepository<SchoolNurse>(_context);
        public IGenericRepository<Student> StudentRepository => _students ??= new GenericRepository<Student>(_context);
        public IGenericRepository<User> UserRepository => _users ??= new GenericRepository<User>(_context);
        public IGenericRepository<Vaccination> VaccinationRepository => _vaccinations ??= new GenericRepository<Vaccination>(_context);
        public IGenericRepository<VaccinationNotice> VaccinationNoticeRepository => _vaccinationNotices ??= new GenericRepository<VaccinationNotice>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
