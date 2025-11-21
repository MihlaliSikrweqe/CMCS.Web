using Microsoft.AspNetCore.Identity;

namespace CMCS.Web.Data
{
    public static class RoleAndUserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<CMCS.Web.Models.User>>();

            string[] roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Create a sample lecturer
            var lecturerEmail = "lecturer@cmcs.local";
            if (await userManager.FindByEmailAsync(lecturerEmail) == null)
            {
                var user = new CMCS.Web.Models.User
                {
                    UserName = lecturerEmail,
                    Email = lecturerEmail,
                    FullName = "Dev Lecturer"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1");
                if (result.Succeeded) await userManager.AddToRoleAsync(user, "Lecturer");
            }

            // sample coordinator
            var coordEmail = "coord@cmcs.local";
            if (await userManager.FindByEmailAsync(coordEmail) == null)
            {
                var user = new CMCS.Web.Models.User
                {
                    UserName = coordEmail,
                    Email = coordEmail,
                    FullName = "Dev Coordinator"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1");
                if (result.Succeeded) await userManager.AddToRoleAsync(user, "Coordinator");
            }

            // manager
            var managerEmail = "manager@cmcs.local";
            if (await userManager.FindByEmailAsync(managerEmail) == null)
            {
                var user = new CMCS.Web.Models.User
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FullName = "Dev Manager"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1");
                if (result.Succeeded) await userManager.AddToRoleAsync(user, "Manager");
            }

            // HR
            var hrEmail = "hr@cmcs.local";
            if (await userManager.FindByEmailAsync(hrEmail) == null)
            {
                var user = new CMCS.Web.Models.User
                {
                    UserName = hrEmail,
                    Email = hrEmail,
                    FullName = "Dev HR"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1");
                if (result.Succeeded) await userManager.AddToRoleAsync(user, "HR");
            }
        }
    }
}
