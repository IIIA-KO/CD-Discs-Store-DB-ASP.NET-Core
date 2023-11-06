using CdDiskStoreAspNetCore.Data.Repository;

namespace CdDiskStoreAspNetCore.Utilities
{
    public static class ServiceRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, DapperClientRepository>();
            services.AddScoped<IDiscRepository, DapperDiscRepository>();
            services.AddScoped<IMusicRepository, DapperMusicRepository>();
            services.AddScoped<IFilmRepository, DapperFilmRepository>();
        }
    }
}