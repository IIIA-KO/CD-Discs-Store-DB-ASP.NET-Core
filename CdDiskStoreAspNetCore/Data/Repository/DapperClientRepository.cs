using CdDiskStoreAspNetCore.Data.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperClientRepository(IConfiguration configuration)
        {
            this._configuration = configuration;

            var connectionString = this._configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(this._connectionString), "Connection string cannot be null or empty");
            }

            this._connectionString = connectionString;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.QueryFirstOrDefaultAsync<Client>($"SELECT * FROM Client WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException("The client with specified Id was not found");
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            var clients = await dbConnection.QueryAsync<Client>("SELECT * FROM Client");
            return (IReadOnlyList<Client>)clients ?? new List<Client>();
        }

        public async Task<int> AddAsync(Client entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.ExecuteAsync("INSERT INTO Client (Id, FirstName, LastName, [Address], City, BirthDay, MarriedStatus, Sex, HasChild) " +
            "VALUES (@Id, @FirstName, @LastName, @Address, @City, @BirthDay, @MarriedStatus, @Sex, @HasChild)", entity);
        }

        public async Task<int> UpdateAsync(Client entity)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);

            return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
            "BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = new SqlConnection(this._connectionString);
            return await dbConnection.ExecuteAsync($"DELETE FROM Client WHERE Id = @Id", new { Id = id });
        }
    }
}