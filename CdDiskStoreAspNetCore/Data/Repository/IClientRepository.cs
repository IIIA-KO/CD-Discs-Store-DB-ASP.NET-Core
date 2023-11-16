using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<int> GetPersonalDiscountAsync(Guid? clientId);
        Task<decimal> GetTotalPurchaseProfitAsync(Guid? clientId);
        Task<decimal> GetTotalRentProfitAsync(Guid? clientId);
        Task<decimal> GetTotalProfitAsync(Guid? clientId);
    }
}
