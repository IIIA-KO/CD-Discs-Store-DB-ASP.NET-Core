using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IMusicRepository : IGenericRepository<Music>
    {
        Task<IReadOnlyList<Disc>> GetDiscsAsync(Guid? id);
    }
}