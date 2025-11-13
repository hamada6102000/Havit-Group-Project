# Site Settings Feature

## Overview

A comprehensive Settings management system has been added to the Admin Dashboard, allowing you to manage all displayed data on your website including contact information, company details, and page content.

## Features

### What You Can Manage

1. **Contact Information**
   - Email address
   - Phone number
   - Mobile number

2. **Address Information**
   - Address Line 1
   - Address Line 2
   - City
   - State/Province
   - Postal/ZIP Code
   - Country

3. **Company Information**
   - Company Name
   - Tagline/Slogan

4. **About Page Content**
   - Mission Statement
   - Company Values
   - Why Choose Us section

5. **Social Media Links**
   - Facebook URL
   - Twitter URL
   - LinkedIn URL
   - Instagram URL
   - YouTube URL

## How to Use

### Accessing Settings

1. Navigate to Admin Dashboard: `/Admin/Dashboard`
2. Click on "Settings" in the sidebar navigation
3. Or click "Site Settings" button in the Quick Actions section

### Editing Settings

1. Fill in the fields you want to update
2. All fields are optional - only fill what you need
3. Click "Save Settings" to update
4. Changes will be reflected immediately on the public website

## Where Settings Are Displayed

### Contact Page (`/Home/Contact`)
- Email, Phone, Mobile numbers (as clickable links)
- Full address information
- If settings are not configured, a placeholder message is shown

### About Page (`/Home/About`)
- Mission Statement (if configured)
- Company Values (if configured)
- Why Choose Us section (if configured)
- Falls back to default content if not configured

## Database

### SiteSettings Table
- Uses singleton pattern (only one record with ID = 1)
- Automatically created when first accessed
- All fields are optional and nullable

### Migration
Run the following command to create the database table:
```bash
dotnet ef database update
```

Or the migration will be applied automatically on first run (development mode).

## Technical Details

### Model: `SiteSettings`
- Located in: `Models/SiteSettings.cs`
- Singleton pattern ensures only one settings record exists
- All fields have proper validation attributes

### Controller: `SettingsController`
- Located in: `Areas/Admin/Controllers/SettingsController.cs`
- Accessible at: `/Admin/Settings`
- GET: Displays settings form (creates default if none exist)
- POST: Updates settings

### Views
- Settings form: `Areas/Admin/Views/Settings/Index.cshtml`
- Organized into sections with Bootstrap cards
- Responsive design

### Integration
- `HomeController` loads settings for Contact and About pages
- Settings are passed via ViewBag to views
- Views check for null/empty values before displaying

## Files Created/Modified

### New Files
1. `Models/SiteSettings.cs` - Settings model
2. `Areas/Admin/Controllers/SettingsController.cs` - Settings controller
3. `Areas/Admin/Views/Settings/Index.cshtml` - Settings form view

### Modified Files
1. `Data/ApplicationDbContext.cs` - Added SiteSettings DbSet
2. `Controllers/HomeController.cs` - Load settings for Contact/About pages
3. `Views/Home/Contact.cshtml` - Display dynamic contact information
4. `Views/Home/About.cshtml` - Display dynamic About page content
5. `Areas/Admin/Views/Shared/_AdminLayout.cshtml` - Added Settings link to sidebar
6. `Areas/Admin/Views/Dashboard/Index.cshtml` - Added Settings button

## Security

- Settings controller has `[Authorize]` placeholder (commented out)
- Only accessible via Admin area
- No public navigation links to Settings

## Future Enhancements

Possible additions:
- Logo/Image upload for company logo
- SEO settings (meta tags, descriptions)
- Email template customization
- Working hours display
- Map integration for address
- Additional contact methods (WhatsApp, Telegram, etc.)

## Notes

- Settings are created automatically on first access if they don't exist
- All fields are optional - you can configure only what you need
- Changes take effect immediately after saving
- The singleton pattern ensures only one settings record exists

