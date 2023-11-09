using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Data.Models;
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
            using IDbConnection dbConnection = this._context.CreateConnection();
            return await dbConnection.ExecuteAsync("UPDATE Disc SET Name = @Name, Price = @Price WHERE Id = @Id", entity);
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
    }
}