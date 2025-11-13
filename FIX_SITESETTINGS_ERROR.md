# Fix: Invalid object name 'SiteSettings' Error

## Problem
You're getting the error: `SqlException: Invalid object name 'SiteSettings'`

This means the `SiteSettings` table doesn't exist in your database yet.

## Solution

### Step 1: Stop Your Application
**IMPORTANT**: You must stop your running application first!

- Press `Ctrl+C` in the terminal where the app is running
- Or close the application window

### Step 2: Apply the Migration

Run this command:
```bash
dotnet ef database update
```

This will create the `SiteSettings` table in your database.

### Step 3: Restart Your Application

```bash
dotnet run
```

The error should now be gone!

## What Happened?

The migration file (`AddSiteSettings`) was created, but it wasn't applied to your database yet. The `dotnet ef database update` command applies the migration and creates the table.

## Alternative: Automatic Fix

I've updated the code so that:
1. Migrations are applied automatically on app startup
2. The Settings controller will try to apply migrations if the table doesn't exist

**But you still need to restart your app** for these changes to take effect!

## Verify It Worked

After applying the migration:

1. **Check your database**: You should see a `SiteSettings` table
2. **Go to `/Admin/Settings`**: It should load without errors
3. **Fill in your data**: Add contact info, company details, etc.
4. **Save**: Click "Save Settings" - your data will be stored!

## Still Having Issues?

If the migration fails:

1. **Check SQL Server is running**
2. **Verify connection string** in `appsettings.json`
3. **Check database permissions**
4. **Run with verbose output**: `dotnet ef database update --verbose`

## Quick SQL Script (If Migration Fails)

If migrations don't work, you can create the table manually:

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

Then restart your app and it should work!

