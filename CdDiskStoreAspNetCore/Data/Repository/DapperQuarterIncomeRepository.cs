using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperQuarterIncomeRepository : IQuarterIncomeRepository
    {
        private readonly IDapperContext _context;

        public DapperQuarterIncomeRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM INCOME_PER_EACH_FINANCIAL_QUARTER_VIEW");
        }

        public async Task<IReadOnlyList<QuarterIncome>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<QuarterIncome>)await dbConnection.QueryAsync<QuarterIncome>("SELECT * FROM INCOME_PER_EACH_FINANCIAL_QUARTER_VIEW")
                ?? new List<QuarterIncome>();
        }

        public async Task<IReadOnlyList<QuarterIncome>> GetProcessedDataAsync(MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (sortField == null || !IndexViewModel<QuarterIncome>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<QuarterIncome> quarterIncomes;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            quarterIncomes = (IReadOnlyList<QuarterIncome>)await dbConnection.QueryAsync<QuarterIncome>($"SELECT * FROM INCOME_PER_EACH_FINANCIAL_QUARTER_VIEW ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return (quarterIncomes ?? new List<QuarterIncome>());
        }
    }
}