using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<IReadOnlyList<Client>> GetFiltered(string? filter, string? fieldName);
    }
}
