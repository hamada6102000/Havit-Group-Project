# Quick Database Setup Guide

## To Store Data in Database - Follow These Steps:

### Option 1: Automatic (Recommended)

The application is now configured to automatically apply migrations on startup. Just:

1. **Stop your running application** (if it's running)
2. **Restart the application**: `dotnet run`
3. The `SiteSettings` table will be created automatically
4. Go to `/Admin/Settings` and start adding your data!

### Option 2: Manual Migration

If you prefer to run migrations manually:

1. **Stop your running application**

2. **Apply the migration**:
   ```bash
   dotnet ef database update
   ```

3. **Start your application**:
   ```bash
   dotnet run
   ```

4. **Go to Admin Settings**: Navigate to `/Admin/Settings`

5. **Add your data**: Fill in contact information, company details, etc.

6. **Save**: Click "Save Settings" - your data will be stored in the database!

### What Gets Stored?

When you fill out the Settings form and click "Save Settings", the following data is stored in the `SiteSettings` table:

- ✅ Contact Information (Email, Phone, Mobile)
- ✅ Address (Full address details)
- ✅ Company Information (Name, Tagline)
- ✅ About Page Content (Mission, Values, Why Choose Us)
- ✅ Social Media Links (Facebook, Twitter, LinkedIn, Instagram, YouTube)

### Verify Data is Stored

1. **Check the website**: Go to `/Home/Contact` - you should see your contact info
2. **Check the database**: 
   - Open SQL Server Management Studio
   - Connect to your database
   - Look for `SiteSettings` table
   - You should see one record with `Id = 1` containing your data

### Troubleshooting

**If migration fails:**
- Make sure SQL Server is running
- Check your connection string in `appsettings.json`
- Ensure database exists

**If table already exists:**
- The migration will skip it
- You can start using Settings immediately

**If you see errors:**
- Check the application logs
- Verify database connection
- Make sure you have proper permissions

### Next Steps

After setting up the database:

1. ✅ Go to `/Admin/Settings`
2. ✅ Fill in your company information
3. ✅ Save the settings
4. ✅ Check `/Home/Contact` to see your data displayed
5. ✅ Check `/Home/About` to see your content displayed

Your data is now stored in the database and will persist across application restarts!

