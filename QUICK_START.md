# Quick Start Guide

## First Time Setup

1. **Update Connection String** in `appsettings.json`
2. **Update Email Settings** in `appsettings.json` (optional, but required for email functionality)
3. **Run the application**: `dotnet run`
4. **Access the site**: Navigate to `https://localhost:5001`

## Quick Commands

```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Apply database migrations
dotnet ef database update

# Create a new migration
dotnet ef migrations add MigrationName

# Remove last migration
dotnet ef migrations remove
```

## Default Routes

- **Home**: `/` or `/Home/Index`
- **About**: `/Home/About`
- **Services**: `/Home/Services`
- **Contact**: `/Home/Contact`
- **Admin Dashboard**: `/Admin/Dashboard`
- **Admin Messages**: `/Admin/Messages`
- **Admin Services**: `/Admin/Services`

## Testing the Application

1. **Test Contact Form**: 
   - Go to `/Home/Contact`
   - Fill out the form and submit
   - Check `/Admin/Messages` to see the submission

2. **Add a Service**:
   - Go to `/Admin/Services/Create`
   - Fill in the service details
   - View it on `/Home/Services`

3. **Enable RTL**:
   - Add `ViewData["IsRtl"] = true;` in any controller action
   - The page will automatically switch to RTL layout

## Important Notes

- The database is automatically created on first run (development mode)
- Email functionality requires proper SMTP configuration
- File uploads are saved to `wwwroot/uploads/`
- Admin area is accessible without authentication (consider adding authentication for production)

