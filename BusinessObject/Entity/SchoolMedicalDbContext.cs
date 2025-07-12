using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class SchoolMedicalDbContext : DbContext
{
    public SchoolMedicalDbContext(DbContextOptions<SchoolMedicalDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<SchoolNurse> SchoolNurses => Set<SchoolNurse>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
    public DbSet<VaccinationNotice> VaccinationNotices => Set<VaccinationNotice>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<IncidentReport> IncidentReports => Set<IncidentReport>();
    public DbSet<HealthCheck> HealthChecks => Set<HealthCheck>();
    public DbSet<DrugStorage> DrugStorages => Set<DrugStorage>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        optionsBuilder.EnableSensitiveDataLogging();
    }
    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnection"];
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Parent>()
            .HasIndex(p => p.UserId)
            .IsUnique();

        modelBuilder.Entity<SchoolNurse>()
            .HasIndex(n => n.UserId)
            .IsUnique();

        modelBuilder.Entity<Manager>()
            .HasIndex(m => m.UserId)
            .IsUnique();

        modelBuilder.Entity<Admin>()
            .HasIndex(a => a.UserId)
            .IsUnique();

        modelBuilder.Entity<Parent>()
            .HasOne(p => p.User)
            .WithOne(u => u.Parent)
            .HasForeignKey<Parent>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SchoolNurse>()
            .HasOne(n => n.User)
            .WithOne(u => u.SchoolNurse)
            .HasForeignKey<SchoolNurse>(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Manager>()
            .HasOne(m => m.User)
            .WithOne(u => u.Manager)
            .HasForeignKey<Manager>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Parent)
            .WithMany(p => p.Students)
            .HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .ToTable("medical_records")
            .HasOne(m => m.Student)
            .WithOne(s => s.MedicalRecord)
            .HasForeignKey<MedicalRecord>(m => m.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vaccination>()
            .HasOne(v => v.Student)
            .WithMany(s => s.Vaccinations)
            .HasForeignKey(v => v.StudentId);

        modelBuilder.Entity<VaccinationNotice>()
            .HasOne(vn => vn.Student)
            .WithMany(s => s.VaccinationNotices)
            .HasForeignKey(vn => vn.StudentId);

        modelBuilder.Entity<Medication>()
            .HasOne(m => m.Student)
            .WithMany(s => s.Medications)
            .HasForeignKey(m => m.StudentId);

        modelBuilder.Entity<Medication>()
            .HasOne(m => m.Nurse)
            .WithMany(n => n.GivenMedications)
            .HasForeignKey(m => m.GivenBy);

        modelBuilder.Entity<IncidentReport>()
            .HasOne(ir => ir.Student)
            .WithMany(s => s.IncidentReports)
            .HasForeignKey(ir => ir.StudentId);

        modelBuilder.Entity<IncidentReport>()
            .HasOne(ir => ir.Nurse)
            .WithMany(n => n.IncidentReports)
            .HasForeignKey(ir => ir.NurseId);

        modelBuilder.Entity<HealthCheck>()
            .HasOne(h => h.Student)
            .WithMany(s => s.HealthChecks)
            .HasForeignKey(h => h.StudentId);

        modelBuilder.Entity<DrugStorage>()
            .HasOne(d => d.Nurse)
            .WithMany(n => n.ManagedDrugs)
            .HasForeignKey(d => d.ManagedBy);
    }
}