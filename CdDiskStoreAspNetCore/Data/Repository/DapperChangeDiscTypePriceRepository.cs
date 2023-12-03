using CdDiskStoreAspNetCore.Data.Contexts;
using CdDiskStoreAspNetCore.Models;
using Dapper;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class DapperChangeDiscTypePriceRepository : IChangeDiscTypePriceRepository
    {
        private readonly IDapperContext _context;

        public DapperChangeDiscTypePriceRepository(IDapperContext context)
        {
            this._context = context;
        }

        public async Task Execute(ChangeDiscTypePriceViewModel model)
        {
            using IDbConnection dbConnection = this._context.CreateConnection();

            await dbConnection.ExecuteAsync("EXEC SP_CHANGE_DISC_TYPE_PRICE @DiscType, @Percent, @Increase", 
                new { DiscType = model.DiscType, Percent = model.Percent, Increase = model.Increase ? 1 : 0 });
        }
    }
}