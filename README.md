# School Medical Management System

A comprehensive healthcare management system designed for educational institutions to monitor student health, manage medical records, and ensure the well-being of the school community.

## Features

### 🔐 Authentication & Authorization
- Role-based access control (Admin, School Nurse, Manager, Parent)
- Secure login system with password hashing
- Session management

### 👥 User Management
- **Parent Management**: Full CRUD operations for parent records
- Student records management
- Medical staff management
- Administrative controls

### 🏥 Health Records
- Digital medical records for all students
- Vaccination tracking and scheduling
- Health check management
- Incident reporting system

### 📊 Reporting & Analytics
- Health statistics and analytics
- Monthly reports generation
- Data visualization

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Frontend**: Razor Pages with Bootstrap 5
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: Cookie-based authentication
- **Password Hashing**: BCrypt.Net-Next

## Project Structure

```
PRN232_School-Medical-Management-System_SU25/
├── API/                          # Web API Controllers
├── Business/                     # Business Logic Layer
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Mapper/                  # AutoMapper configurations
│   └── Services/                # Business services
├── BusinessObject/              # Data Access Layer
│   ├── Entity/                  # Entity models
│   └── Migrations/              # Database migrations
├── Data/                        # Repository pattern
├── SchoolMedicalManagement/     # Web Application
│   ├── Pages/                   # Razor Pages
│   │   ├── Auth/               # Authentication pages
│   │   ├── Parents/            # Parent management pages
│   │   └── Shared/             # Shared layouts
│   └── wwwroot/                # Static files
└── README.md                   # Project documentation
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd PRN232_School-Medical-Management-System_SU25
   ```

2. **Update connection string**
   Edit `SchoolMedicalManagement/appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=SchoolMedicalDB;Trusted_Connection=true;"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   cd BusinessObject
   dotnet ef database update
   ```

4. **Build and run the application**
   ```bash
   cd SchoolMedicalManagement
   dotnet build
   dotnet run
   ```

5. **Access the application**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## API Endpoints

### Parent Management (Admin Only)
- `GET /api/parent` - Get all parents
- `GET /api/parent/{id}` - Get parent by ID
- `GET /api/parent/user/{userId}` - Get parent by user ID
- `POST /api/parent` - Create new parent
- `PUT /api/parent/{id}` - Update parent
- `DELETE /api/parent/{id}` - Delete parent

## User Roles

### Admin
- Full access to all features
- Can manage all users (parents, students, staff)
- Can view and generate reports
- Can manage system settings

### School Nurse
- Access to medical records
- Can perform health checks
- Can create incident reports
- Limited administrative access

### Manager
- Can view reports and statistics
- Can manage basic user information
- Limited medical record access

### Parent
- Can view their own information
- Can view their children's medical records
- Limited system access

## Security Features

- **Password Hashing**: All passwords are hashed using BCrypt
- **Role-based Authorization**: Access control based on user roles
- **Session Management**: Secure session handling with timeout
- **Input Validation**: Comprehensive form validation
- **SQL Injection Protection**: Entity Framework prevents SQL injection

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions, please contact:
- Email: health@school.edu
- Phone: (555) 123-4567
- Office Hours: Monday-Friday 8AM-4PM

## Screenshots

### Homepage
The homepage features a modern healthcare-themed design with statistics, quick actions, and feature highlights.

### Parent Management
Admin-only access to manage parent records with full CRUD operations.

### Navigation
Professional navigation with dropdown menus for different management areas.

---

**Note**: This is a school project for PRN232 - Advanced Programming with .NET and C#.