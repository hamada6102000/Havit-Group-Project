# How to Store Data in Database - Migration Instructions

## Step 1: Stop the Running Application

If your application is currently running, stop it first:
- Press `Ctrl+C` in the terminal where it's running
- Or close the application window

## Step 2: Create the Migration

Run this command in your project directory:

```bash
dotnet ef migrations add AddSiteSettings
```

This will create a new migration file that adds the SiteSettings table to your database.

## Step 3: Apply the Migration to Database

Run this command to update your database:

```bash
dotnet ef database update
```

This will:
- Create the `SiteSettings` table in your database
- Set up all the columns with proper data types and constraints

## Step 4: Verify the Migration

After running the migration, you can verify it worked by:

1. **Check SQL Server**: Open SQL Server Management Studio or your database tool
2. **Look for SiteSettings table**: You should see a new table called `SiteSettings`
3. **Check columns**: The table should have all the fields (Email, Phone, AddressLine1, etc.)

## Alternative: Manual SQL Script

If migrations don't work, you can run this SQL script directly in your database:

```sql
CREATE TABLE [dbo].[SiteSettings] (
    [Id] INT NOT NULL PRIMARY KEY,
    [Email] NVARCHAR(200) NULL,
    [Phone] NVARCHAR(50) NULL,
    [Mobile] NVARCHAR(50) NULL,
    [AddressLine1] NVARCHAR(200) NULL,
    [AddressLine2] NVARCHAR(200) NULL,
    [City] NVARCHAR(100) NULL,
    [State] NVARCHAR(100) NULL,
    [PostalCode] NVARCHAR(20) NULL,
    [Country] NVARCHAR(100) NULL,
    [CompanyName] NVARCHAR(200) NULL,
    [Tagline] NVARCHAR(500) NULL,
    [Mission] NVARCHAR(2000) NULL,
    [Values] NVARCHAR(2000) NULL,
    [WhyChooseUs] NVARCHAR(2000) NULL,
    [FacebookUrl] NVARCHAR(500) NULL,
    [TwitterUrl] NVARCHAR(500) NULL,
    [LinkedInUrl] NVARCHAR(500) NULL,
    [InstagramUrl] NVARCHAR(500) NULL,
    [YouTubeUrl] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NULL
);
```

## After Migration

Once the migration is applied:

1. **Start your application**: `dotnet run`
2. **Go to Admin Settings**: Navigate to `/Admin/Settings`
3. **Fill in your data**: Enter your contact information, company details, etc.
4. **Save**: Click "Save Settings" - data will be stored in the database
5. **View on website**: Check `/Home/Contact` and `/Home/About` to see your data displayed

## Troubleshooting

### If migration fails:
- Make sure SQL Server is running
- Check your connection string in `appsettings.json`
- Ensure you have proper database permissions
- Try running: `dotnet ef database update --verbose` for detailed error messages

### If table already exists:
- The migration will skip creating it if it already exists
- Or you can manually insert a record with ID = 1

## Testing

After applying the migration:

1. Go to `/Admin/Settings`
2. Fill in some test data (e.g., Email, Phone, Address)
3. Click "Save Settings"
4. Check `/Home/Contact` - you should see your data displayed
5. Check the database - you should see a record in SiteSettings table with Id = 1

