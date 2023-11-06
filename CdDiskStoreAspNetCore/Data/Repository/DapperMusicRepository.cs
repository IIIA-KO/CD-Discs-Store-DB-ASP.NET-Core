using CdDiskStoreAspNetCore.Data.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperMusicRepository : IMusicRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperMusicRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            var connectionString = this._configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(this._connectionString), "Connection string cannot be null or empty");
            }

            this._connectionString = connectionString;
        }

        public async Task<Music> GetByIdAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.QueryFirstOrDefaultAsync<Music>("SELECT * FROM Music WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException("The music with specified Id was not found");
        }

        public async Task<IReadOnlyList<Music>> GetAllAsync()
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            var music = await dbConnection.QueryAsync<Music>("SELECT * FROM Music");
            return (IReadOnlyList<Music>)music ?? new List<Music>();
        }

        public async Task<int> AddAsync(Music entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("INSERT INTO Music ([Name], Genre, Artist, [Language]) VALUES (@Name, @Genre, @Artist, @Language)", entity);
        }

        public async Task<int> UpdateAsync(Music entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("UPDATE Music SET Name = @Name, Genre = @Genre, @Artist = Artist, @Language = Language WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync($"DELETE FROM Music WHERE Id = @Id", new { Id = id });
        }
    }
}
