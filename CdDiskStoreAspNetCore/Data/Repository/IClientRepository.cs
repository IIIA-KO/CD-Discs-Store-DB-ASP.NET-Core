using CdDiskStoreAspNetCore.Data.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<int> GetPersonalDiscountAsync(Guid? id);
        Task<decimal> GetTotalPurchaseProfitAsync(Guid? cliidentId);
        Task<decimal> GetTotalRentProfitAsync(Guid? id);
        Task<decimal> GetTotalProfitAsync(Guid? id);
        Task<IReadOnlyList<string>> GetMailsAsync(Guid? id);
        Task<IReadOnlyList<string>> GetPhonesAsync(Guid? id);
    }
}
