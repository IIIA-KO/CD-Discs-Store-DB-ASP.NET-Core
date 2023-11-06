using CdDiskStoreAspNetCore.Data.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperFilmRepository : IFilmRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperFilmRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            var connectionString = this._configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(this._connectionString), "Connection string cannot be null or empty");
            }

            this._connectionString = connectionString;
        }

        public async Task<Film> GetByIdAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.QueryFirstOrDefaultAsync<Film>("SELECT * FROM Film WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException("The film with specified Id was not found");
        }

        public async Task<IReadOnlyList<Film>> GetAllAsync()
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            var films = await dbConnection.QueryAsync<Film>("SELECT * FROM Film");
            return (IReadOnlyList<Film>)films ?? new List<Film>();
        }

        public async Task<int> AddAsync(Film entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("INSERT INTO Film ([Name], Genre, Producer, MainRole, AgeLimit) VALUES (@Name, @Genre, @Producer, @MainRole, @AgeLimit)", entity);
        }

        public async Task<int> UpdateAsync(Film entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("UPDATE Film SET Name = @Name, Genre = @Genre, Producer = @Producer, MainRole = @MainRole, AgeLimit = @AgeLimit WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync($"DELETE FROM Film WHERE Id = @Id", new { Id = id });
        }
    }
}
