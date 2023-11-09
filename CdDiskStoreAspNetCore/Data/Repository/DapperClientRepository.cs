using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;
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
            Client currentClient;
            try
            {
                currentClient = await this.GetByIdAsync(entity.Id);
            }
            catch (NullReferenceException)
            {
                throw;
            }

            if (currentClient != null && !IsClientChanged(currentClient, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
            "BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
        }

        public bool IsClientChanged(Client currentClient, Client client)
        {
            return currentClient.FirstName != client.FirstName
                || currentClient.LastName != client.LastName
                || currentClient.Address != client.Address
                || currentClient.City != client.City
                || currentClient.ContactPhone != client.ContactPhone
                || currentClient.ContactMail != client.ContactMail
                || currentClient.BirthDay != client.BirthDay
                || currentClient.MarriedStatus != client.MarriedStatus
                || currentClient.Sex != client.Sex
                || currentClient.HasChild != client.HasChild;
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


        private bool ValidateFieldName(string? fieldName)
        {
            if (fieldName == null || !ClientsIndexViewModel.AvailableFilterFieldNames.Contains(fieldName))
            {
                return false;
            }
            return true;
        }

        public async Task<IReadOnlyList<Client>> GetData(string? filter, string? filterField, MySortOrder sortOrder, string? sortField)
        {
            if (filterField == null || !ClientsIndexViewModel.AvailableFilterFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. Client table does not have such filterable column");
            }

            if (sortField == null)
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<Client> clients;
            switch (sortOrder)
            {
                case MySortOrder.Ascending:
                    clients = (IReadOnlyList<Client>)await dbConnection.QueryAsync<Client>($"SELECT * FROM Client WHERE {filterField} LIKE @value ORDER BY {sortField} ASC", new { value = "%" + filter + "%" });
                    break;

                case MySortOrder.Descending:
                    clients = (IReadOnlyList<Client>)await dbConnection.QueryAsync<Client>($"SELECT * FROM Client WHERE {filterField} LIKE @value ORDER BY {sortField} DESC", new { value = "%" + filter + "%" });
                    break;

                case MySortOrder.NotSorted:
                    clients = await this.GetAllAsync();
                    break;

                default:
                    clients = (IReadOnlyList<Client>)await dbConnection.QueryAsync<Client>($"SELECT * FROM Client WHERE {filterField} LIKE @value ORDER BY {sortField} ASC", new { value = "%" + filter + "%" });
                    break;
            }

            return (clients ?? new List<Client>());
        }
    }
}