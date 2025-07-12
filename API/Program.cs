using BusinessLogic.Services;
using BusinessObject.Entity;
using DataAccess;
using DataAccess.Repo;
using DataAccess.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
var modelBuilder = new ODataConventionModelBuilder();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    })
    .AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWorks, UnitOfWork>();
builder.Services.AddScoped<IGenericRepository<Student>, GenericRepository<Student>>();
builder.Services.AddScoped<IGenericRepository<Parent>,  GenericRepository<Parent>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddDbContext<SchoolMedicalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
        };
    });

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MedicalStaff", policy => policy.RequireRole("Admin", "SchoolNurse", "Manager"));
    options.AddPolicy("ParentAccess", policy => policy.RequireRole("Admin", "Parent"));
});

// Add SWAGGER JWT
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed the database with test data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolMedicalDbContext>();
    try
    {
        // Apply migrations
        try
        {
            context.Database.Migrate();
            Console.WriteLine("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration failed: {ex.Message}");
            // Fallback to EnsureCreated if migrations fail
            var created = context.Database.EnsureCreated();
            Console.WriteLine($"Database created: {created}");
        }
        
        // Test database connection
        try
        {
            var canConnect = context.Database.CanConnect();
            Console.WriteLine($"Can connect to database: {canConnect}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
        }
        
        // Check if there are any users in the database
        var userCount = context.Users.Count();
        Console.WriteLine($"Current user count in database: {userCount}");
        
        if (!context.Users.Any())
        {
            Console.WriteLine("No users found, creating test users...");
            
            // Create a test admin user
            var testUser = new BusinessObject.Entity.User
            {
                Username = "admin",
                Email = "admin@test.com",
                PasswordHash = "admin123", // In production, this should be hashed
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            Console.WriteLine($"Admin user created with ID: {testUser.Id}");
            
            // Create a test parent user
            var parentUser = new BusinessObject.Entity.User
            {
                Username = "parent",
                Email = "parent@test.com",
                PasswordHash = "parent123", // In production, this should be hashed
                Role = "Parent",
                CreatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(parentUser);
            await context.SaveChangesAsync();
            Console.WriteLine($"Parent user created with ID: {parentUser.Id}");
            
            // Create a parent record
            var parent = new BusinessObject.Entity.Parent
            {
                FullName = "Test Parent",
                PhoneNumber = "1234567890",
                Address = "123 Test Street",
                UserId = parentUser.Id
            };
            
            context.Parents.Add(parent);
            await context.SaveChangesAsync();
            Console.WriteLine($"Parent record created with ID: {parent.Id}");
            
            // Create a test nurse user
            var nurseUser = new BusinessObject.Entity.User
            {
                Username = "nurse",
                Email = "nurse@test.com",
                PasswordHash = "nurse123", // In production, this should be hashed
                Role = "Nurse",
                CreatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(nurseUser);
            await context.SaveChangesAsync();
            Console.WriteLine($"Nurse user created with ID: {nurseUser.Id}");
            
            // Create a nurse record
            var nurse = new BusinessObject.Entity.SchoolNurse
            {
                FullName = "Test Nurse",
                PhoneNumber = "0987654321",
                UserId = nurseUser.Id
            };
            
            context.SchoolNurses.Add(nurse);
            await context.SaveChangesAsync();
            Console.WriteLine($"Nurse record created with ID: {nurse.Id}");
            
            Console.WriteLine("Test users created successfully");
        }
        else
        {
            Console.WriteLine("Users already exist in database");
            // List existing users for debugging
            var existingUsers = context.Users.ToList();
            foreach (var user in existingUsers)
            {
                Console.WriteLine($"User: {user.Username} ({user.Email}) - Role: {user.Role}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding error: {ex.Message}");
    }
}

app.Run();
