using CdDiskStoreAspNetCore.Data;
using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CdDiskStoreAspNetCore.Utilities.ExtensionClasses
{
    public static class ServiceRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDapperContext, DapperContext>();
            services.AddScoped<IClientRepository, DapperClientRepository>();
            services.AddScoped<IDiscRepository, DapperDiscRepository>();
            services.AddScoped<IMusicRepository, DapperMusicRepository>();
            services.AddScoped<IFilmRepository, DapperFilmRepository>();

            services.AddTransient<UserManager<IdentityUser>>();
            services.AddTransient<SignInManager<IdentityUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();

            services.AddScoped<IUserStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, ApplicationDbContext, string>>();
            services.AddScoped<IUserEmailStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, ApplicationDbContext, string>>();
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();

            services.AddScoped<IDebtorsRepository, DapperDebtorRepository>();
            services.AddScoped<IQuarterIncomeRepository, DapperQuarterIncomeRepository>();
            services.AddScoped<IProfitFromClientRepository, DapperProfitFromClientRepository>();
            services.AddScoped<IUselessDiscsRepository, DapperUselessDiscsRepository>();
            services.AddScoped<IAdultFilmRatioRepository, DapperAdultFilmRatioRepository>();
            
            services.AddScoped<IChangeDiscTypePriceRepository, DapperChangeDiscTypePriceRepository>();
            services.AddScoped<IChangeDiscountLevelRepository, DapperChangeDiscountLevelRepository>();
        }
    }
}