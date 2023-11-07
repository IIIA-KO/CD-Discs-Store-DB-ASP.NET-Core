using System.Data;

namespace CdDiskStoreAspNetCore.Data.Contexts
{
    public interface IDapperContext
    {
        public IDbConnection CreateConnection();
    }
}