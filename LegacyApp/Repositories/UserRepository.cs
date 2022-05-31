using Dapper;
using LegacyApp.Constants;
using LegacyApp.Models;
using System.Data;
using System.Data.SqlClient;

namespace LegacyApp
{
    public static class UserRepository
    {
        public static void AddUser(User user)
        {
            var connectionString = AppSettings.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string storedProcedureName = "uspAddUser";

                using (var transaction = connection.BeginTransaction())
                {
                    var executedQuery = connection.Execute(storedProcedureName, user, transaction: transaction, commandType: CommandType.StoredProcedure);

                    transaction.Commit();
                }
            }
        }
    }
}
