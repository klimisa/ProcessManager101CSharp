namespace ProcessManager101CSharp.Infrastructure.Postgres
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Npgsql;

    public class PostgreSqlConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public PostgreSqlConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<DbConnection> OpenConnection()
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
