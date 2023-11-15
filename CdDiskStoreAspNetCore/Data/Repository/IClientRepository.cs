using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        bool IsClientChanged(Client currentClient, Client client);

        Task<int> CountAsync();

        Task<int> GetProcessedDataCount(string? filter, string? filterField);

        Task<IReadOnlyList<Client>> GetProcessedData(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize);

        Task<int> GetPersonalDiscountAsync(Guid? clientId);


        Task<decimal> GetTotalPurchaseProfit(Guid? clientId);
        Task<decimal> GetTotalRentProfit(Guid? clientId);
        Task<decimal> GetTotalProfit(Guid? clientId);
    }
}
