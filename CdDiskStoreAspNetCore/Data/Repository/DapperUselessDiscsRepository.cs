using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperUselessDiscsRepository : IUselessDiscsRepository
    {
        private readonly IDapperContext _context;

        public DapperUselessDiscsRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM USELESS_DISCS_VIEW");
        }

        public async Task<IReadOnlyList<UselessDisc>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<UselessDisc>)await dbConnection.QueryAsync<UselessDisc>("SELECT * FROM USELESS_DISCS_VIEW")
                ?? new List<UselessDisc>();
        }

        public async Task<IReadOnlyList<UselessDisc>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<UselessDisc>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition");
            }

            if (sortField == null || !IndexViewModel<UselessDisc>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<UselessDisc> uselessDiscs;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM USELESS_DISCS_VIEW WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            uselessDiscs = (IReadOnlyList<UselessDisc>)await dbConnection.QueryAsync<UselessDisc>(sqlQuery, new { value = "%" + filter + "%" });

            return (uselessDiscs ?? new List<UselessDisc>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<UselessDisc>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM USELESS_DISCS_VIEW WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }
    }
}
