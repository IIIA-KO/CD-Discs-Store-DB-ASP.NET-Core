using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IFilmRepository : IGenericRepository<Film>
    {
        Task<IReadOnlyList<Disc>> GetDiscsAsync(Guid? id);
    }
}
