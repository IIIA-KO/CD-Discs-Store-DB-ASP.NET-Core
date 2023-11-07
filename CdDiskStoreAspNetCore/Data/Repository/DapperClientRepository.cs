using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperClientRepository : IClientRepository
    {
        private readonly IDapperContext _context;

        private const string CLIENT_NOT_FOUND_BY_ID_ERROR = "The client with specified Id was not found";

        public DapperClientRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task<Client> GetByIdAsync(Guid? id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            return await dbConnection.QueryFirstOrDefaultAsync<Client>($"SELECT * FROM Client WHERE Id = @Id", new { Id = id })
                ?? throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
        }

        public async Task<IReadOnlyList<Client>> GetAllAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            var clients = await dbConnection.QueryAsync<Client>("SELECT * FROM Client");
            return (IReadOnlyList<Client>)clients ?? new List<Client>();
        }

        public async Task<int> AddAsync(Client entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("INSERT INTO Client (Id, FirstName, LastName, [Address], City, ContactPhone, ContactMail, BirthDay, MarriedStatus, Sex, HasChild) " +
            "VALUES (@Id, @FirstName, @LastName, @Address, @City, @ContactPhone, @ContactMail, @BirthDay, @MarriedStatus, @Sex, @HasChild)", entity);
        }

        public async Task<int> UpdateAsync(Client entity)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
            "BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync($"DELETE FROM Client WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Client WHERE Id = @Id", new { Id = id });
        }
    }
}