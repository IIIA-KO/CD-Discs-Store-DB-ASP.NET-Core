using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperChangeDiscountLevelRepository : IChangeDiscountLevelRepository
    {
        private readonly IDapperContext _context;

        public DapperChangeDiscountLevelRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task Execute(ChangeDiscountLevelViewModel model)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            await dbConnection.ExecuteAsync("EXEC SP_CHANGE_DISCOUNT_LEVEL_FOR_CLIENT @IdClient, @Increase",
                new { IdClient = model.IdClient, Increase = model.Increase ? 1 : 0 });
        }
    }
}