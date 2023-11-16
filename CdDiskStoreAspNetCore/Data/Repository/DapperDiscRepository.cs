using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperDiscRepository : IDiscRepository
    {
        private readonly IDapperContext _context;

        private const string DISC_NOT_FOUND_BY_ID_ERROR = "The client with specified Id was not found";

        public DapperDiscRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Disc> GetByIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (id is null)
            {
                throw new NullReferenceException(DISC_NOT_FOUND_BY_ID_ERROR);
            }

            return await dbConnection.QueryFirstOrDefaultAsync<Disc>($"SELECT * FROM Disc WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException(DISC_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Disc>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var discs = await dbConnection.QueryAsync<Disc>("SELECT * FROM Disc");
            return (IReadOnlyList<Disc>)discs ?? new List<Disc>();
        }

        public async Task<int> AddAsync(Disc entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("INSERT INTO Disc ([Name], Price) VALUES (@Name, @Price)", entity);
        }

        public async Task<int> UpdateAsync(Disc entity)
        {
            Disc currentDisc;
            try
            {
                currentDisc = await this.GetByIdAsync(entity.Id);
            }
            catch(NullReferenceException)
            {
                throw;
            }

            if(currentDisc != null && !IsEntityChanged(currentDisc, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Disc currentEntity, Disc entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Price != entity.Price;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Disc WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Disc WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Disc>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !DiscsIndexViewModel.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. Disc table does not have such filterable column");
            }

            if (sortField == null || !DiscsIndexViewModel.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<Disc> discs;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM Disc WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            discs = (IReadOnlyList<Disc>)await dbConnection.QueryAsync<Disc>(sqlQuery, new { value = "%" + filter + "%" });

            return (discs ?? new List<Disc>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !DiscsIndexViewModel.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM Disc WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Disc");
        }
    }
}