using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IIdentityUserRepository
    {
        Task<IdentityUser> GetByIdAsync(string? id);

        Task<IReadOnlyList<IdentityUser>> GetAllAsync();

        Task<bool> AddAsync(IdentityUser entity, string password);

        Task<bool> UpdateAsync(IdentityUser entity, IReadOnlyList<string> roles);

        Task<bool> DeleteAsync(string id);

        Task<bool> ExistsAsync(string id);

        Task<int> CountAsync();
        Task<int> GetProcessedDataCountAsync(string? filter, string? filterField);
        Task<IReadOnlyList<IdentityUser>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize);

        Task<IReadOnlyList<string>> GetAllRolesAsync();
        Task<IReadOnlyList<string>> GetRolesAsync(string? id);
    }
}
