using System.Configuration;
using MySql.Data.MySqlClient;

namespace Rafis.DAO
{
    public static class ConnectDB
    {
        private static readonly string ConIICC = ConfigurationManager.ConnectionStrings["ConIICCindice"].ConnectionString;
		private static readonly string ConAfis = ConfigurationManager.ConnectionStrings["ConAfis"].ConnectionString;
        
		public static class GetInstancia
        {
            public static MySqlConnection GetConnection()
            {
            MySqlConnection con = new MySqlConnection(ConAfis);
            return con;
            }
        
        }

    }
}
