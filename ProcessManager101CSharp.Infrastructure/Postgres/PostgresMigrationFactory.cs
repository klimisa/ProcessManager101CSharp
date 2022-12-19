namespace ProcessManager101CSharp.Infrastructure.Postgres
{
    using System.Threading.Tasks;
    using Dapper;
    using FluentMigrator.Runner;
    using Npgsql;
    using Utile.Configuration.Settings;

    public class PostgresMigrationFactory
    {
        private readonly IMigrationRunner runner;

        public PostgresMigrationFactory(IMigrationRunner runner)
        {
            this.runner = runner;
        }

        public async Task MigrateUp(PostgresSettings settings, string database)
        {
            await EnsureDatabase(settings.GetConnectionString(), database);

            using var scope = runner.BeginScope();
            runner.MigrateUp();
            scope.Complete();
        }

        private async Task EnsureDatabase(string connectionString, string databaseName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("name", databaseName);
            await using var connection = new NpgsqlConnection(connectionString);

            var count = await connection.QueryFirstAsync<int>("SELECT count(*) FROM pg_database WHERE datname = @name", parameters);

            if (count == 0)
                await connection.ExecuteAsync($"CREATE DATABASE \"{databaseName}\"");
        }
    }
}
