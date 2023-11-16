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
        public DbSet<CdDiskStoreAspNetCore.Data.Models.Client>? Client { get; set; }
        public DbSet<CdDiskStoreAspNetCore.Data.Models.Disc>? Disc { get; set; }
    }
}