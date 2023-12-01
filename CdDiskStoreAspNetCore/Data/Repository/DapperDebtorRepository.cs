using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperDebtorRepository : IDebtorsRepository
    {
        private readonly IDapperContext _context;

        public DapperDebtorRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM ACTIVE_DEBTORS_VIEW");
        }

        public async Task<IReadOnlyList<Debtor>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<Debtor>)await dbConnection.QueryAsync<Debtor>("SELECT * FROM ACTIVE_DEBTORS_VIEW")
                ?? new List<Debtor>();
        }

        public async Task<IReadOnlyList<Debtor>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<Debtor>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition");
            }

            if (sortField == null || !IndexViewModel<Debtor>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<Debtor> debtors;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM ACTIVE_DEBTORS_VIEW WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            debtors = (IReadOnlyList<Debtor>)await dbConnection.QueryAsync<Debtor>(sqlQuery, new { value = "%" + filter + "%" });

            return (debtors ?? new List<Debtor>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<Debtor>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM ACTIVE_DEBTORS_VIEW WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }
    }
}