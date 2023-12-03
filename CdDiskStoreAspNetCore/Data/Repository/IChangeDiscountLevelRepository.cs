using CdDiskStoreAspNetCore.Models;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public interface IChangeDiscountLevelRepository
    {
        Task Execute(ChangeDiscountLevelViewModel model);
    }
}
