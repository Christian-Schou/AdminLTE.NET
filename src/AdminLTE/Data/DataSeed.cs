using System.Security.Claims;
using AdminLTE.Common;
using AdminLTE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE.Data;

public static class DataSeed
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        using (var scope = scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed database code goes here

            // User Info
            //string userName = "SuperAdmin";
            var firstName = "Super";
            var lastName = "Admin";
            var email = "superadmin@admin.com";
            var password = "Qwaszx123$";
            var role = "SuperAdmins";
            var role2 = "SeniorManagers";
            var role3 = "Managers";

            if (await _userManager.FindByNameAsync(email) == null)
            {
                // Create SuperAdmins role if it doesn't exist
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                if (await roleManager.FindByNameAsync(role2) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role2));
                }

                if (await roleManager.FindByNameAsync(role3) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role3));
                }

                // Create user account if it doesn't exist
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    //extended properties
                    FirstName = firstName,
                    LastName = lastName,
                    AvatarURL = "/images/user.png",
                    DateRegistered = DateTime.UtcNow.ToString(),
                    Position = "",
                    NickName = ""
                };

                var result = await _userManager.CreateAsync(user, password);

                // Assign role to the user
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.GivenName, user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Surname, user.LastName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.AvatarURL, user.AvatarURL));

                    //SignInManager<ApplicationUser> _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}