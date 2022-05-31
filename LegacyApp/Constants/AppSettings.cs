using System.IO;

namespace LegacyApp.Constants
{
    public static class AppSettings
    {
        public const string path = Directory.GetCurrentDirectory();
        var xx = path.Substring(0, path.IndexOf('b') - 1);

        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").SetBasePath(xx)
        .AddEnvironmentVariables()
        .Build();

        var connectionString = config.GetSection("ConnectionStrings").GetValue<string>("appDatabase");
    }
}
