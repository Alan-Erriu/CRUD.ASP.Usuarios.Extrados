

using AccesData.Interfaces;

namespace CRUD.ASP.Usuarios.Extrados.GateWays
{
    public class ConfigSqlConnect : IConfigSqlConnect
    {
        private string connectionStringDb = string.Empty;


        public ConfigSqlConnect()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
            connectionStringDb = builder.GetSection("ConnectionStrings:connectionDb").Value;
        }
        public string chainSQL()
        {
            return connectionStringDb;
        }
    }
}
