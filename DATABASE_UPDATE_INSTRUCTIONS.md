# Database Update Instructions

## Current Situation

The migration file has been created, but EF Core needs the application to be stopped to apply it properly.

## Solution: Restart Your Application

Since `Program.cs` is now configured to automatically apply migrations on startup, you have two options:

### Option 1: Automatic Migration (Easiest)

1. **Stop your running application**:
   - Press `Ctrl+C` in the terminal where it's running
   - Or close the application window

2. **Restart the application**:
   ```bash
   dotnet run
   ```

3. **The migration will be applied automatically** when the app starts!

4. **Verify**: Check your database - the `SiteSettings` table should now exist.

### Option 2: Manual Migration (If Option 1 doesn't work)

1. **Stop your running application**

2. **Run the migration command**:
   ```bash
   dotnet ef database update
   ```

3. **Start your application**:
   ```bash
   dotnet run
   ```

## What Will Happen

When the migration is applied, it will:
- ✅ Create the `SiteSettings` table in your database
- ✅ Set up all columns with proper data types
- ✅ The table will be ready to store your settings data

## After Migration

1. Go to `/Admin/Settings` in your browser
2. Fill in your contact information, company details, etc.
3. Click "Save Settings"
4. Your data will be stored in the database!

## Verify It Worked

You can verify the migration succeeded by:

1. **Check the application logs** - you should see migration messages
2. **Check your database** - look for `SiteSettings` table
3. **Try accessing Settings** - go to `/Admin/Settings` - it should work without errors

## Troubleshooting

**If migration still fails:**
- Make sure SQL Server is running
- Check your connection string in `appsettings.json`
- Ensure you have database permissions
- Try: `dotnet ef database update --verbose` for detailed output

**If you see "table already exists":**
- That's fine! The migration will skip it
- You can start using Settings immediately

