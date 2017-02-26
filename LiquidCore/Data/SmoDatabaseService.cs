using Liquid.IoC;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Liquid.Data
{
    public interface ISmoDatabaseService
    {
        DataSet ExecuteQuery(string query);
        void ExecuteNonQuery(string query);
    }

    [DefaultImplementation(typeof(ISmoDatabaseService))]
    class SmoDatabaseService : ISmoDatabaseService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionString;

        public SmoDatabaseService()
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            var connectionString = connectionStrings["DefaultConnection"].ConnectionString;
            _sqlConnectionString = new SqlConnectionStringBuilder(connectionString);
        }

        public DataSet ExecuteQuery(string query)
        {
            var connection = new ServerConnection(_sqlConnectionString.DataSource);
            var server = new Server(connection);
            var database = server.Databases[_sqlConnectionString.InitialCatalog];
            return database.ExecuteWithResults(query);
        }

        public void ExecuteNonQuery(string query)
        {
            var connection = new ServerConnection(_sqlConnectionString.DataSource);
            var server = new Server(connection);
            var database = server.Databases[_sqlConnectionString.InitialCatalog];
            database.ExecuteNonQuery(query);
        }
    }
}
