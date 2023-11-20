using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;

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
        }
    }
}