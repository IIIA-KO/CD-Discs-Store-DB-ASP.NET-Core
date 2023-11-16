using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid? id);
        
        Task<IReadOnlyList<T>> GetAllAsync();
        
        Task<int> AddAsync(T entity);
        
        Task<int> UpdateAsync(T entity);
        bool IsEntityChanged(T currentEntity, T entity);

        Task<int> DeleteAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);


        Task<int> CountAsync();
        Task<int> GetProcessedDataCountAsync(string? filter, string? filterField);
        Task<IReadOnlyList<T>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize);
    }
}
