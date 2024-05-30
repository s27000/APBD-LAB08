using System.Data.SqlClient;

namespace TripApp.Repositories
{
    public interface IUnitOfWork
    {
        Task InitializeAsync(CancellationToken cancellationToken);
        Task CommitAsync(CancellationToken cancellationToken);
        SqlCommand CreateCommand();
    }
}
