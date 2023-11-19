using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperFilmRepository : IFilmRepository
    {
        private readonly IDapperContext _context;
        private const string FILM_NOT_FOUND_BY_ID_ERROR = "The film with specified Id was not found";

        public DapperFilmRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Film> GetByIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (id is null)
            {
                throw new NullReferenceException(FILM_NOT_FOUND_BY_ID_ERROR);
            }

            return await dbConnection.QueryFirstOrDefaultAsync<Film>("SELECT * FROM Film WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException(FILM_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Film>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var films = await dbConnection.QueryAsync<Film>("SELECT * FROM Film");
            return (IReadOnlyList<Film>)films ?? new List<Film>();
        }

        public async Task<int> AddAsync(Film entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("INSERT INTO Film ([Name], Genre, Producer, MainRole, AgeLimit) VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit)", entity);
        }

        public async Task<int> UpdateAsync(Film entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE Film SET Name = @Name, Genre = @Genre, Producer = @Producer, MainRole = @MainRole, AgeLimit = @AgeLimit WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Film currentEntity, Film entity)
        {
            return currentEntity.Name != entity.Name
                || currentEntity.Genre != entity.Genre
                || currentEntity.Producer != entity.Producer
                || currentEntity.MainRole != entity.MainRole
                || currentEntity.AgeLimit != entity.AgeLimit;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Film WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Film WHERE Id = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<Film>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<Film>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. Film table does not have such filterable column");
            }

            if (sortField == null || !IndexViewModel<Film>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<Film> films;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM Film WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            films = (IReadOnlyList<Film>)await dbConnection.QueryAsync<Film>(sqlQuery, new { value = "%" + filter + "%" });

            return (films ?? new List<Film>());
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<Film>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM Film WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Film");
        }
    }
}
