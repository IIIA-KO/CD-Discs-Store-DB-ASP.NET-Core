using Microsoft.Data.SqlClient;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Contexts
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            this._configuration = configuration;

            var connectionString = this._configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(this._connectionString), "Connection string cannot be null or empty");
            }

            this._connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(this._connectionString);
        }
    }
}