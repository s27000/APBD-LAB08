using System.Data.SqlClient;

namespace TripApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction? _transaction;

        public UnitOfWork(SqlConnection sqlConnection)
        {
            _connection = sqlConnection;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await _connection.OpenAsync(cancellationToken);
            _transaction = (SqlTransaction)await _connection.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (_transaction == null)
            {
                throw new Exception("Task not initialized");
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await _transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public SqlCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }
    }
}
