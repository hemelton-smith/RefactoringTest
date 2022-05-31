using LegacyApp.Enums;
using LegacyApp.Interfaces;
using LegacyApp.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace LegacyApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private IConfiguration _configuration;
        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Client GetById(int id)
        {
            Client client = null;

            string path = Directory.GetCurrentDirectory();
            var xx = path.Substring(0, path.IndexOf('b') - 1);

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").SetBasePath(xx)
            .AddEnvironmentVariables()
            .Build();

            var connectionString = config.GetSection("ConnectionStrings").GetValue<string>("appDatabase");

            //string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("appDatabase").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "uspGetClientById"
                };

                var parameter = new SqlParameter("@ClientId", SqlDbType.Int) { Value = id };
                command.Parameters.Add(parameter);

                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    client = new Client
                                      {
                                          Id = int.Parse(reader["ClientId"].ToString()),
                                          Name = reader["Name"].ToString(),
                                          ClientStatus = (ClientStatus)int.Parse(reader["ClientStatusId"].ToString())
                                      };
                }
            }

            return client;
        }
    }
}
