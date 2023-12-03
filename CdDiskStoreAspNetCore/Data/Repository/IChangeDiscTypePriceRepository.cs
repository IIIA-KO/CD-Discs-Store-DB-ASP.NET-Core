using CdDiskStoreAspNetCore.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IChangeDiscTypePriceRepository
    {
        Task Execute(ChangeDiscTypePriceViewModel model);
    }
}