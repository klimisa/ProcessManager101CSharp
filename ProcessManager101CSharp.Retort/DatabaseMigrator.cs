namespace ProcessManager101CSharp.Retort
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentMigrator.Runner;
    using Infrastructure.Postgres;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using Polly;

    public class DatabaseMigrator : IHostedService
    {
        private readonly PostgresOptions postgresOptions;
        private readonly PostgresOptions postgresMigratorOptions;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DatabaseMigrator> logger;

        public DatabaseMigrator
        (
            List<PostgresOptions> postgresOptionsList,
            IServiceProvider serviceProvider,
            ILogger<DatabaseMigrator> logger
        )
        {
            postgresOptions = postgresOptionsList[0];
            postgresMigratorOptions = postgresOptionsList[1];
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("---> Migration started <---");
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            await Policy
                .Handle<NpgsqlException>()
                .WaitAndRetryAsync(60, _ => TimeSpan.FromMilliseconds(500))
                .ExecuteAsync(async () =>
                {
                    var postgresMigrationFactory = new PostgresMigrationFactory(runner);
                    var database = postgresOptions.Database;
                    await postgresMigrationFactory.MigrateUp(postgresMigratorOptions, database);
                });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
