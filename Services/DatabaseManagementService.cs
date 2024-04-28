using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Services;

public static class DatabaseManagementService
{
    public static void MigrationsInitialization(this IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            var serviceDb = serviceScope.ServiceProvider
                .GetService<ApplicationDbContext>();
            serviceDb!.Database.Migrate();
        }
    }
}
