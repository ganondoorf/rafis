using System.Configuration;
using MySql.Data.MySqlClient;
namespace DAO
{
    class ConnectDB
    {
        private static readonly string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
        
        public static class GetInstancia
        {

            public static MySqlConnection GetConnection()
            {
            MySqlConnection con = new MySqlConnection(ConOrigem);
            return con;
            }
        
        }

    }
}
