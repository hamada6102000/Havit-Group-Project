using HavitGroup.Data;
using HavitGroup.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure email service (using a simple implementation)
builder.Services.AddSingleton<IEmailService, EmailService>();

var app = builder.Build();

// Apply database migrations on startup
// This ensures the SiteSettings table and other tables are created automatically
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        // Check if database exists, create if not
        if (!context.Database.CanConnect())
        {
            logger.LogInformation("Database does not exist. Creating database...");
            context.Database.EnsureCreated();
        }
        
        // Apply pending migrations - this will create SiteSettings table if migration exists
        // This runs in both Development and Production to ensure database is up to date
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrations.Count());
            context.Database.Migrate();
            logger.LogInformation("Migrations applied successfully.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying database migrations.");
        logger.LogWarning("Application will continue, but SiteSettings table may not exist.");
        logger.LogWarning("To fix: Stop the app and run 'dotnet ef database update', or run CreateSiteSettingsTable.sql");
        // Don't throw - allow app to continue even if migration fails
        // Admin can fix migration issues separately
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Admin Area routing - Admin area is accessible only via direct URL (/Admin/Dashboard)
// No navigation links are displayed in the main menu to prevent discovery by regular users.
// The Admin area routes are mapped before the default routes to ensure proper routing.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default routes for public pages (Home, About, Services, Contact)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
