using Microsoft.EntityFrameworkCore;
using server_app.Infrastructure;

namespace server_app.Presentation.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        
        dbContext.Database.Migrate();
    }
}