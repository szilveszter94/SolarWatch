using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private UserManager<IdentityUser> userManager;
    private Dictionary<string, string> adminInfo;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, Dictionary<string, string> adminInfo)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.adminInfo = adminInfo;
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(roleManager);
        tUser.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = adminInfo["adminEmail"] };
            var adminCreated = await userManager.CreateAsync(admin, adminInfo["adminPassword"]);

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}