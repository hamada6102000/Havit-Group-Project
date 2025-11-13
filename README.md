# FreeLance - ASP.NET Core MVC Website

A professional freelance company website built with ASP.NET Core MVC, featuring a public-facing website and an admin dashboard for managing content.

## Features

### Public Pages
- **Home Page** (`/`) - Welcome page with company overview and featured services
- **About Us** (`/Home/About`) - Company information and values
- **Services** (`/Home/Services`) - Display of all active services
- **Contact Us** (`/Home/Contact`) - Contact form with file upload support and email notifications

### Admin Dashboard (`/Admin/Dashboard`)
- **Dashboard** - Overview with statistics (messages, services)
- **Messages Management** - View, read, and delete contact form submissions
- **Services Management** - Full CRUD operations for services (Create, Read, Update, Delete)

### Technical Features
- Entity Framework Core with SQL Server
- File upload support (saves to `wwwroot/uploads`)
- Email notifications via SMTP
- Responsive Bootstrap 5 design
- RTL (Right-to-Left) support for Arabic language
- Proper async/await patterns with CancellationToken support
- MVC architecture with separation of concerns

## Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) or [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- Visual Studio 2022 or Visual Studio Code (optional, for development)

## Setup Instructions

### 1. Clone or Download the Project

```bash
git clone <repository-url>
cd FreeLance
```

### 2. Configure Database Connection

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FreeLanceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server:**
```json
"DefaultConnection": "Server=localhost;Database=FreeLanceDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
```

### 3. Configure Email Settings

Update the email settings in `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "FreeLance Company",
    "SenderPassword": "your-app-password",
    "EnableSsl": true
  }
}
```

**Note:** For Gmail, you'll need to:
1. Enable 2-Factor Authentication
2. Generate an App Password (not your regular password)
3. Use the App Password in `SenderPassword`

### 4. Restore NuGet Packages

```bash
dotnet restore
```

### 5. Apply Database Migrations

The project uses Entity Framework Core migrations. The database will be created automatically on first run, but you can also apply migrations manually:

```bash
dotnet ef database update
```

### 6. Run the Application

```bash
dotnet run
```

Or use Visual Studio:
- Press `F5` or click "Run"

The application will be available at:
- `https://localhost:5001` (HTTPS)
- `http://localhost:5000` (HTTP)

## Project Structure

```
FreeLance/
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       │   ├── DashboardController.cs
│       │   ├── MessagesController.cs
│       │   └── ServicesController.cs
│       └── Views/
│           ├── Dashboard/
│           ├── Messages/
│           └── Services/
├── Controllers/
│   └── HomeController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── ContactMessage.cs
│   ├── ContactViewModel.cs
│   └── Service.cs
├── Services/
│   ├── IEmailService.cs
│   └── EmailService.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── About.cshtml
│   │   ├── Services.cshtml
│   │   └── Contact.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── lib/
│   └── uploads/
├── appsettings.json
├── Program.cs
└── FreeLance.csproj
```

## Routing

### Public Routes
- `/` - Home page
- `/Home/About` - About Us page
- `/Home/Services` - Services page
- `/Home/Contact` - Contact Us page

### Admin Routes
- `/Admin/Dashboard` - Admin dashboard
- `/Admin/Messages` - View all messages
- `/Admin/Messages/Details/{id}` - View message details
- `/Admin/Services` - View all services
- `/Admin/Services/Create` - Create new service
- `/Admin/Services/Edit/{id}` - Edit service
- `/Admin/Services/Delete/{id}` - Delete service

## Database Schema

### ContactMessage
- `Id` (int, Primary Key)
- `Name` (string, required, max 100)
- `Email` (string, required, max 200)
- `Subject` (string, required, max 200)
- `Message` (string, required, max 5000)
- `AttachmentPath` (string, optional, max 500)
- `OriginalFileName` (string, optional, max 255)
- `SubmittedAt` (DateTime, required)
- `IsRead` (bool, default false)

### Service
- `Id` (int, Primary Key)
- `Title` (string, required, max 200)
- `Description` (string, required, max 2000)
- `IconClass` (string, optional, max 100)
- `CreatedAt` (DateTime, required)
- `UpdatedAt` (DateTime?, optional)
- `IsActive` (bool, default true)

## File Upload

The contact form supports optional file uploads. Files are saved to `wwwroot/uploads/` with unique filenames to prevent conflicts. Supported file types include:
- PDF (.pdf)
- Word Documents (.doc, .docx)
- Text Files (.txt)
- Images (.jpg, .jpeg, .png)

Maximum file size: 10MB (can be configured in the controller)

## RTL (Right-to-Left) Support

The application supports RTL layout for Arabic language. To enable RTL:

1. Set `ViewData["IsRtl"] = true;` in your controller action
2. The layout will automatically switch to RTL mode
3. Bootstrap RTL CSS will be loaded

Example:
```csharp
public IActionResult About()
{
    ViewData["IsRtl"] = true; // Enable RTL
    return View();
}
```

## Publishing the Application

### Using Visual Studio

1. Right-click on the project
2. Select "Publish"
3. Choose your publishing target (Azure, IIS, Folder, etc.)
4. Configure settings and publish

### Using Command Line

```bash
# Publish to a folder
dotnet publish -c Release -o ./publish

# Publish for specific runtime
dotnet publish -c Release -r win-x64 -o ./publish
```

### Deployment Considerations

1. **Database**: Ensure SQL Server is accessible from your deployment environment
2. **Connection String**: Update connection string in production `appsettings.json`
3. **Email Settings**: Configure production email settings
4. **File Uploads**: Ensure `wwwroot/uploads` folder exists and has write permissions
5. **HTTPS**: Configure SSL certificates for production
6. **Environment Variables**: Consider using environment variables for sensitive settings

## Troubleshooting

### Database Connection Issues

If you encounter database connection errors:
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure database server allows connections
4. Verify user permissions

### Email Not Sending

If emails are not being sent:
1. Verify SMTP settings in `appsettings.json`
2. Check firewall settings
3. For Gmail, ensure App Password is used (not regular password)
4. Check application logs for detailed error messages

### File Upload Issues

If file uploads fail:
1. Ensure `wwwroot/uploads` folder exists
2. Check folder permissions (write access required)
3. Verify file size limits
4. Check file type restrictions

## Development Notes

- All async operations use `CancellationToken` for proper cancellation support
- Controllers include comprehensive XML comments
- Models use data annotations for validation
- Views use Bootstrap 5 for responsive design
- Admin area uses a separate layout for better organization

## License

This project is provided as-is for educational and commercial use.

## Support

For issues or questions, please contact the development team or create an issue in the repository.

---

**Version:** 1.0.0  
**Last Updated:** November 2025

