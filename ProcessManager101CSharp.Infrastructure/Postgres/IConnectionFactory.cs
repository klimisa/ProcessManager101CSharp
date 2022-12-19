namespace ProcessManager101CSharp.Infrastructure.Postgres
{
    using System.Data.Common;
    using System.Threading.Tasks;

    public interface IConnectionFactory
    {
        Task<DbConnection> OpenConnection();
    }
}
