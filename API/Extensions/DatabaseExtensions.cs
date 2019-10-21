using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace API.Extensions
{
    public static class DatabaseExtensions
    {
        public static void UseDatabaseMigrations(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
                context.Database.Migrate();
                Seed.SeedData(context, userManager).Wait();
            }
        }
    }
}
