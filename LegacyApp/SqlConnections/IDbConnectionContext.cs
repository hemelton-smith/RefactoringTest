using System.Data;

namespace LegacyApp.SqlConnections
{
    public interface IDbConnectionContext
    {
        IDbConnection GetConnection();
        IDbTransaction GetTransaction();
        void Commit();
        void Rollback();
    }
}