using Dapper;
using LegacyApp.Constants;
using LegacyApp.Enums;
using LegacyApp.Interfaces;
using LegacyApp.Models;
using System.Data;
using System.Data.SqlClient;

namespace LegacyApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public Client GetById(int id)
        {
            Client client = null;

            var connectionString = AppSettings.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {            
                string storedProcedureName = "uspGetClientById";

                var parameter = new
                {
                    Id = id
                };

                var returnResult = connection.QueryFirstOrDefault<Client>(storedProcedureName, parameter, commandType: CommandType.StoredProcedure);

                client = new Client
                {
                    Id = returnResult.Id,
                    Name = returnResult.Name,
                    ClientStatus = returnResult.ClientStatus
                };
            }

            return client;
        }
    }
}
