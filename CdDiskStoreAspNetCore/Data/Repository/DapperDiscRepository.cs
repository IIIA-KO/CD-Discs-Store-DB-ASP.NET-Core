using CdDiskStoreAspNetCore.Data.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperDiscRepository : IDiscRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperDiscRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            var connectionString = this._configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(this._connectionString), "Connection string cannot be null or empty");
            }

            this._connectionString = connectionString;
        }

        public async Task<Disc> GetByIdAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.QueryFirstOrDefaultAsync<Disc>($"SELECT * FROM Disc WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException("The client with specified Id was not found");
        }

        public async Task<IReadOnlyList<Disc>> GetAllAsync()
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            var discs = await dbConnection.QueryAsync<Disc>("SELECT * FROM Disc");
            return (IReadOnlyList<Disc>)discs ?? new List<Disc>();
        }

        public async Task<int> AddAsync(Disc entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("INSERT INTO Disc ([Name], Price) VALUES (@Name, @Price)", entity);
        }

        public async Task<int> UpdateAsync(Disc entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync($"DELETE FROM Disc WHERE Id = @Id", new { Id = id });
        }
    }
}