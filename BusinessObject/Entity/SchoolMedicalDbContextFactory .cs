using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessObject.Entity
{
    public class SchoolMedicalDbContextFactory : IDesignTimeDbContextFactory<SchoolMedicalDbContext>
    {
        public SchoolMedicalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolMedicalDbContext>();

            optionsBuilder.UseSqlServer("Server=LAPTOP-4EB8UC8S\\SQLEXPRESS;Database=SchoolMedicalDb;User Id=sa;Password=12345;TrustServerCertificate=True");

            return new SchoolMedicalDbContext(optionsBuilder.Options);
        }
    }
}
