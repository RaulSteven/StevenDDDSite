using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Steven.Domain.Repositories.Infrastructure
{
    public class ConnectionFactory
    {
        private static readonly string connString = ConfigurationManager.ConnectionStrings["BeiLinDatabase"].ConnectionString;

        public static IDbConnection CreateConnection()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            return conn;
        }
    }
}
