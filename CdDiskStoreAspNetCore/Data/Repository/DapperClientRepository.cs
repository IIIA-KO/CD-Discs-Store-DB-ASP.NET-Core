using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Drawing.Printing;

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
            "ContactPhone = @ContactPhone, ContactMail = @ContactMail, BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
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

        public async Task<IReadOnlyList<Client>> GetProcessedData(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !ClientsIndexViewModel.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. Client table does not have such filterable column");
            }

            if (sortField == null || !ClientsIndexViewModel.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();
            IReadOnlyList<Client> clients;
            string sortOrderString = sortOrder switch
            {
                MySortOrder.Ascending => "ASC",
                MySortOrder.Descending => "DESC",
                _ => "ASC",
            };

            string sqlQuery = $"SELECT * FROM Client WHERE {filterField} LIKE @value ORDER BY {sortField} {sortOrderString} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            clients = (IReadOnlyList<Client>)await dbConnection.QueryAsync<Client>(sqlQuery, new { value = "%" + filter + "%" });

            return (clients ?? new List<Client>());
        }

        public async Task<int> GetProcessedDataCount(string? filter, string? filterField)
        {
            if (filterField == null || !ClientsIndexViewModel.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            string countQuery = $"SELECT COUNT(*) FROM Client WHERE {filterField} LIKE @value";

            return await dbConnection.ExecuteScalarAsync<int>(countQuery, new { value = "%" + filter + "%" });
        }

        public async Task<int> CountAsync()
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Client");
        }

        public async Task<int> GetPersonalDiscountAsync(Guid? clientId)
        {
            Client client;

            try
            {
                client = await this.GetByIdAsync(clientId);
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<int>("SELECT TOP 1 PersonalDiscountValue FROM PersonalDiscount WHERE idClient = @id ORDER BY StartDateTime DESC", new { id = clientId });
        }

        public async Task<decimal> GetTotalPurchaseProfit(Guid? clientId)
        {
            if (clientId is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>($"SELECT dbo.FN_GET_TOTAL_PURCHASE_PROFIT_FROM_CLIENT(@id)", new {id = clientId});
        }

        public async Task<decimal> GetTotalRentProfit(Guid? clientId)
        {
            if (clientId is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>($"SELECT dbo.FN_GET_TOTAL_RENT_PROFIT_FROM_CLIENT(@id)", new { id = clientId });
        }

        public async Task<decimal> GetTotalProfit(Guid? clientId)
        {
            if (clientId is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>($"SELECT dbo.FN_GET_TOTAL_PROFIT_FROM_CLIENT(@id)", new { id = clientId });
        }
    }
}