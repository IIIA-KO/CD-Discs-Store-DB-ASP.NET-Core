using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperProfitFromClientRepository : IProfitFromClientRepository
    {
        private IDapperContext _context;

        public DapperProfitFromClientRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW");
        }

        public async Task<IReadOnlyList<ProfitFromClient>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<ProfitFromClient>)await dbConnection.QueryAsync<ProfitFromClient>("SELECT * FROM PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW")
                ?? new List<ProfitFromClient>();
        }

        public async Task<IReadOnlyList<ProfitFromClient>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<ProfitFromClient>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition");
            }

            if (sortField == null || !IndexViewModel<ProfitFromClient>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<ProfitFromClient> profitFromClients;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            profitFromClients = (IReadOnlyList<ProfitFromClient>)await dbConnection.QueryAsync<ProfitFromClient>(sqlQuery, new { value = "%" + filter + "%" });

            return (profitFromClients ?? new List<ProfitFromClient>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<ProfitFromClient>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM PROFIT_FROM_EACH_CLIENT_STATICTICS_VIEW WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }
    }
}
