using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperAdultFilmRatioRepository : IAdultFilmRatioRepository
    {
        private readonly IDapperContext _context;

        public DapperAdultFilmRatioRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM ADULT_FILM_RATIO_BY_MONTH_VIEW");
        }

        public async Task<IReadOnlyList<AdultFilmRatio>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<AdultFilmRatio>)await dbConnection.QueryAsync<AdultFilmRatio>("SELECT * FROM ADULT_FILM_RATIO_BY_MONTH_VIEW")
                ?? new List<AdultFilmRatio>();
        }

        public async Task<IReadOnlyList<AdultFilmRatio>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<AdultFilmRatio>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition");
            }

            if (sortField == null || !IndexViewModel<AdultFilmRatio>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<AdultFilmRatio> adultFilmRatios;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM ADULT_FILM_RATIO_BY_MONTH_VIEW WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            adultFilmRatios = (IReadOnlyList<AdultFilmRatio>)await dbConnection.QueryAsync<AdultFilmRatio>(sqlQuery, new { value = "%" + filter + "%" });

            return (adultFilmRatios ?? new List<AdultFilmRatio>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<AdultFilmRatio>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM ADULT_FILM_RATIO_BY_MONTH_VIEW WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }
    }
}
