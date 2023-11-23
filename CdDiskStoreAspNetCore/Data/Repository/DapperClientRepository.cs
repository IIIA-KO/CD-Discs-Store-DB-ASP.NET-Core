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

            if (currentClient != null && !IsEntityChanged(currentClient, entity))
            {
                return 0;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteAsync("UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, " +
            "ContactPhone = @ContactPhone, ContactMail = @ContactMail, BirthDay = @BirthDay, MarriedStatus = @MarriedStatus, Sex = @Sex, HasChild = @HasChild WHERE Id = @Id", entity);
        }

        public bool IsEntityChanged(Client currentEntity, Client entity)
        {
            return currentEntity.FirstName != entity.FirstName
                || currentEntity.LastName != entity.LastName
                || currentEntity.Address != entity.Address
                || currentEntity.City != entity.City
                || currentEntity.ContactPhone != entity.ContactPhone
                || currentEntity.ContactMail != entity.ContactMail
                || currentEntity.BirthDay != entity.BirthDay
                || currentEntity.MarriedStatus != entity.MarriedStatus
                || currentEntity.Sex != entity.Sex
                || currentEntity.HasChild != entity.HasChild;
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

        public async Task<IReadOnlyList<Client>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<Client>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. Client table does not have such filterable column");
            }

            if (sortField == null || !IndexViewModel<Client>.AllFieldNames.Contains(sortField))
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

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<Client>.FilterableFieldNames.Contains(filterField))
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

        public async Task<int> GetPersonalDiscountAsync(Guid? id)
        {
            Client client;

            try
            {
                client = await this.GetByIdAsync(id);
            }
            catch (NullReferenceException)
            {
                throw;
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.QueryFirstOrDefaultAsync<int>("SELECT TOP 1 PersonalDiscountValue FROM PersonalDiscount WHERE idClient = @Id ORDER BY StartDateTime DESC", new { Id = id });
        }

        public async Task<decimal> GetTotalPurchaseProfitAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>("SELECT dbo.FN_GET_TOTAL_PURCHASE_PROFIT_FROM_CLIENT(@id)", new { Id = id });
        }

        public async Task<decimal> GetTotalRentProfitAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>("SELECT dbo.FN_GET_TOTAL_RENT_PROFIT_FROM_CLIENT(@Id)", new { Id = id });
        }

        public async Task<decimal> GetTotalProfitAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return await dbConnection.ExecuteScalarAsync<decimal>("SELECT dbo.FN_GET_TOTAL_PROFIT_FROM_CLIENT(@Id)", new { Id = id });
        }

        public async Task<IReadOnlyList<string>> GetMailsAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<string>)await dbConnection.QueryAsync<string>("SELECT Mail FROM MailList WHERE IdClient = @Id", new { Id = id });
        }

        public async Task<IReadOnlyList<string>> GetPhonesAsync(Guid? id)
        {
            if (id is null)
            {
                throw new NullReferenceException(CLIENT_NOT_FOUND_BY_ID_ERROR);
            }

            using IDbConnection dbConnection = this._context.CreateConnection();

            return (IReadOnlyList<string>)await dbConnection.QueryAsync<string>("SELECT Phone FROM PhoneList WHERE IdClient = @Id", new { Id = id });
        }
    }
}