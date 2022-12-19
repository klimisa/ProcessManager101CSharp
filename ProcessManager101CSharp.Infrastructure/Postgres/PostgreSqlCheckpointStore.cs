namespace ProcessManager101CSharp.Infrastructure.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper.Contrib.Extensions;
    using Utile.EventStore.Projections;

    public class PostgreSqlCheckpointStore : ICheckpointStore
    {
        private readonly IConnectionFactory connectionFactory;

        public PostgreSqlCheckpointStore(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<string>> GetCheckpoints()
        {
            await using var conn = await connectionFactory.OpenConnection();
            var checkpoints = await conn.GetAllAsync<Data.checkpoint>();
            return checkpoints.Select(x => x.id);
        }
        public async Task<Checkpoint> GetCheckpoint(string checkpointName)
        {
            await using var conn = await connectionFactory.OpenConnection();
            var checkpoint = await conn.GetAsync<Data.checkpoint>(checkpointName);
            return checkpoint is null
                ? default
                : new Checkpoint
                {
                    Id = checkpointName,
                    CommitPosition = Convert.ToUInt64(checkpoint.commit_position),
                    PreparePosition = Convert.ToUInt64(checkpoint.prepare_position)
                };
        }

        public async Task StoreCheckpoint(Checkpoint checkpoint)
        {
            await using var conn = await connectionFactory.OpenConnection();
            // await using var ts = await conn.BeginTransactionAsync();

            var checkpointData = await conn.GetAsync<Data.checkpoint>(checkpoint.Id);
            if (checkpointData is null)
            {
                await conn.InsertAsync(new Data.checkpoint
                {
                    id = checkpoint.Id,
                    commit_position = Convert.ToInt64(checkpoint.CommitPosition),
                    prepare_position = Convert.ToInt64(checkpoint.PreparePosition)
                });
            }
            else
            {
                checkpointData.commit_position = Convert.ToInt64(checkpoint.CommitPosition);
                checkpointData.prepare_position = Convert.ToInt64(checkpoint.PreparePosition);
                await conn.UpdateAsync(checkpointData);
            }

            // await ts.CommitAsync();
        }
    }

    public static class Data
    {
        public class checkpoint
        {
            [ExplicitKey] public string id { get; set; }
            public long commit_position { get; set; }
            public long prepare_position { get; set; }
        }
    }
}
