using Microsoft.Extensions.Configuration;
using System.IO;

namespace LegacyApp.Constants
{
    public static class AppSettings
    {
        public static string GetConnectionString()
        {
            string path = Directory.GetCurrentDirectory();
            var basePath = path.Substring(0, path.IndexOf('b') - 1);

            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(basePath)
            .AddEnvironmentVariables()
            .Build();

            var connectionString = config.GetSection("ConnectionStrings").GetValue<string>("appDatabase");

            return connectionString;
        }

    }
}
