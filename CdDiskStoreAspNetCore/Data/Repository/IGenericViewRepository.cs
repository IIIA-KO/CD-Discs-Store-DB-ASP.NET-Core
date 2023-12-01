using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IGenericViewRepository<T> where T : class
    {
        Task<int> CountAsync();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize);
        Task<int> GetProcessedDataCountAsync(string? filter, string? filterField);
    }
}
