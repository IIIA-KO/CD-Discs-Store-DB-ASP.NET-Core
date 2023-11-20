using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<CdDiskStoreAspNetCore.Data.Models.Film>? Film { get; set; }
        public DbSet<CdDiskStoreAspNetCore.Data.Models.Music>? Music { get; set; }
    }
}