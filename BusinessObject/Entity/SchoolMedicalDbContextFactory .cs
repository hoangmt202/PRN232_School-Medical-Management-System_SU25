using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessObject.Entity
{
    public class SchoolMedicalDbContextFactory : IDesignTimeDbContextFactory<SchoolMedicalDbContext>
    {
        public SchoolMedicalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolMedicalDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost;Database=SchoolMedicalDb;User Id=sa;Password=123456;TrustServerCertificate=True");

            return new SchoolMedicalDbContext(optionsBuilder.Options);
        }
    }
}
