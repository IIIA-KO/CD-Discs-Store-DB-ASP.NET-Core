using CdDiskStoreAspNetCore.Data;
using CdDiskStoreAspNetCore.Utilities.ExtensionClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CdDiskStoreAspNetCore
{
    public class Program
    {
        const string DEFAULT_ADMIN_ROLE_NAME = "Administrator";
        const string DEFAULT_MANAGER_ROLE_NAME = "Manager";
        const string DEFAULT_USER_ROLE_NAME = "User";
        const string DEFAULT_GUEST_ROLE_NAME = "Guest";

        static async Task InitialDatabase(IServiceProvider host)
        {
            using (var serviceScope = host.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var userStore = services.GetRequiredService<IUserStore<IdentityUser>>();
                var emailStore = (IUserEmailStore<IdentityUser>)userStore;

                var adminRole = await roleManager.FindByNameAsync(DEFAULT_ADMIN_ROLE_NAME);
                var managerRole = await roleManager.FindByNameAsync(DEFAULT_MANAGER_ROLE_NAME);
                var userRole = await roleManager.FindByNameAsync(DEFAULT_USER_ROLE_NAME);
                var guestRole = await roleManager.FindByNameAsync(DEFAULT_GUEST_ROLE_NAME);

                const string adminUser = "admin@host.com";
                const string adminPass = "Aa1234567890-";

                if (adminRole == null)
                {
                    var adminR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_ADMIN_ROLE_NAME));
                    if (adminR.Succeeded == false)
                    {
                        throw new Exception("Error create Administrator role");
                    }
                    adminRole = await roleManager.FindByNameAsync(DEFAULT_ADMIN_ROLE_NAME);
                }

                if (managerRole == null)
                {
                    var managerR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_MANAGER_ROLE_NAME));
                    if (managerR.Succeeded == false)
                    {
                        throw new Exception("Error create Manager role");
                    }
                    managerRole = await roleManager.FindByNameAsync(DEFAULT_MANAGER_ROLE_NAME);
                }

                if (userRole == null)
                {
                    var userR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_USER_ROLE_NAME));
                    if (userR.Succeeded == false)
                    {
                        throw new Exception("Error create User role");
                    }
                    userRole = await roleManager.FindByNameAsync(DEFAULT_USER_ROLE_NAME);
                }

                if (guestRole == null)
                {
                    var guestR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_GUEST_ROLE_NAME));
                    if (guestR.Succeeded == false)
                    {
                        throw new Exception("Error create Guest role");
                    }
                    guestRole = await roleManager.FindByNameAsync(DEFAULT_GUEST_ROLE_NAME);
                }

                var userAdmin = await userManager.FindByNameAsync(adminUser);
                if (userAdmin == null)
                {
                    var user = Activator.CreateInstance<IdentityUser>();

                    await userStore.SetUserNameAsync(user, adminUser, CancellationToken.None);
                    await emailStore.SetEmailAsync(user, adminUser, CancellationToken.None);

                    var result = await userManager.CreateAsync(user, adminPass);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error create user {adminUser} with password {adminPass}");
                    }

                    var userId = await userManager.GetUserIdAsync(user);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var resultConfirm = await userManager.ConfirmEmailAsync(user, code);
                    if (!resultConfirm.Succeeded)
                    {
                        throw new Exception("");
                    }

                    userAdmin = await userManager.FindByNameAsync(adminUser);
                }

                if (!await userManager.IsInRoleAsync(userAdmin, DEFAULT_ADMIN_ROLE_NAME))
                {
                    await userManager.AddToRoleAsync(userAdmin, DEFAULT_ADMIN_ROLE_NAME);
                }
            }
        }


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            // Authorization:
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication();

            // Model Repositories:
            builder.Services.RegisterRepositories();


            // Identity:
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();


            // Build:
            var app = builder.Build();


            // Middleware:
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Authorization:
            if (app.Environment.IsDevelopment())
            {
                InitialDatabase(app.Services).Wait();
            }

            app.Run();
        }
    }
}