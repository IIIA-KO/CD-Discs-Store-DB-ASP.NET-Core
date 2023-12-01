using CdDiskStoreAspNetCore.Models.Enums;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public record QuarterIncome(int? OrderYear, decimal? Quarter1, decimal? Quarter2, decimal? Quarter3, decimal? Quarter4);

    public interface IQuarterIncomeRepository
    {
        Task<int> CountAsync();
        Task<IReadOnlyList<QuarterIncome>> GetAllAsync();
        Task<IReadOnlyList<QuarterIncome>> GetProcessedDataAsync(MySortOrder sortOrder, string? sortField, int skip, int pageSize);
    }
}