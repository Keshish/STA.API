using Microsoft.AspNetCore.Identity;
using STA.API.Models.Authentication;

namespace STA.API.Models
{
    public static class SeedingExtensions
    {
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).Wait();
                if (!roleManager.RoleExistsAsync(UserRoles.Assistant).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Assistant)).Wait();
                if (!roleManager.RoleExistsAsync(UserRoles.Supervisor).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Supervisor)).Wait();
                if (!roleManager.RoleExistsAsync(UserRoles.Parent).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Parent)).Wait();
            }

            return host;
        }
    }
}