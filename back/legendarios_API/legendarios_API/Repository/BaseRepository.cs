using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace legendarios_API.Repository
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration["mysqlStringConnect"];
        }

        protected MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
