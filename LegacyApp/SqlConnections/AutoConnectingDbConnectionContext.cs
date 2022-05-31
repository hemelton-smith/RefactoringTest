using System;
using System.Data;
using System.Data.SqlClient;

namespace LegacyApp.SqlConnections
{
    public class AutoConnectingDbConnectionContext : ILegacyAppDbConnectionContext, IDisposable
    {
        private readonly Lazy<IDbConnection> _connection;
        private Lazy<IDbTransaction> _transaction;
        private readonly string _connectionString;

        public AutoConnectingDbConnectionContext(IDbSettings dbSettings, Func<IDbSettings, string> connectionStringFactory)
        {
            _connectionString = connectionStringFactory(dbSettings);
            _connection = new Lazy<IDbConnection>(CreateConnection);
            _transaction = new Lazy<IDbTransaction>(CreateTransaction);
        }

        public IDbConnection GetConnection()
        {
            return _connection.Value;
        }

        public IDbTransaction GetTransaction()
        {
            return _transaction.Value;
        }

        private IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private IDbTransaction CreateTransaction()
        {
            return _connection.Value.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction.IsValueCreated)
            {
                _transaction.Value.Commit();
                _transaction.Value.Dispose();
                _transaction = new Lazy<IDbTransaction>(CreateTransaction);
            }
        }

        public void Rollback()
        {
            if (_transaction.IsValueCreated)
            {
                _transaction.Value.Rollback();
                _transaction.Value.Dispose();
                _transaction = new Lazy<IDbTransaction>(CreateTransaction);
            }
        }

        public void Dispose()
        {
            // TODO, for now we're just commiting on disposal, this can't be the final
            //       implementation as we don't want transactions to commit if there is an error.
            //       This will need to become more sophisticated. Maybe we can make another interface
            //       that this class also implements called IUnitOfWork that has a Commit(), this
            //       can then be resolved where needed and the commit done.
            Commit();

            if (_connection.IsValueCreated)
            {
                _connection.Value.Dispose();
            }
        }
    }
}