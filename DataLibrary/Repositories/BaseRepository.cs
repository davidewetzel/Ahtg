using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace DataLibrary.Repositories
{
    public class BaseRepository
    {
        private const string CONNECTION_STRING = "DbContext";
        private readonly IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        protected async Task<DbConnection> CreateConnection()
        {
            DbConnection connection = new SqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            await connection.OpenAsync();
            return connection;
        }

        protected string GetPagedSql(int pageNumber, int pageSize)
        {
            return $"OFFSET {(pageNumber - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
        }
    }
}
