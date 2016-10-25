using MySql.Data.MySqlClient;
using System.Configuration;
namespace DAO
{
    class MySQLCommand
    {
        #region Comando genérico do MySQL
        private static readonly string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
        public static void GenericCommand(string myExecuteQuery)
        {
           try
            {
                MySqlConnection con = new MySqlConnection(ConOrigem);
                MySqlCommand myCommand = new MySqlCommand(myExecuteQuery, con);
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Insere Templates no Banco
        public static void inserenodestino(int id, int pid, byte[] template, string caminho, string asXml, byte[] isoTemplate)
        {
            MySqlConnection Conexaoorigem, Conexaodestino;
            string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
            string ConDestino = ConfigurationManager.ConnectionStrings["ConexaoDestino"].ConnectionString;

            Conexaoorigem = new MySqlConnection(ConOrigem);
            Conexaodestino = new MySqlConnection(ConDestino);

            try
            {
                Conexaoorigem.Open();
                Conexaodestino.Open();
            }

            catch (MySqlException ex)
            {
                throw ex;
            }

            MySqlCommand Commanddestino = new MySqlCommand();
           
            Commanddestino.Connection = Conexaodestino;
          
            Commanddestino.CommandText = "insert into template (itemId, personId, Template, CaminhoImagem, Template_xml, isoTemplate) values(@idPar, @pidPar, @templatePar, @caminhoPar, @templateXml, @isoTemplatePar)";
            
            MySqlParameter idPar = new MySqlParameter("@idPar", id);
            MySqlParameter pidPar = new MySqlParameter("@pidPar", pid);
            MySqlParameter templatePar = new MySqlParameter("@templatePar", MySqlDbType.VarBinary);
            MySqlParameter caminhoPar = new MySqlParameter("@caminhoPar", caminho);
            MySqlParameter templateXml = new MySqlParameter("@templateXml", asXml);
            MySqlParameter iso_Template = new MySqlParameter("@isoTemplatePar", MySqlDbType.VarBinary);

            templatePar.Value = template;
            iso_Template.Value = isoTemplate;

            Commanddestino.Parameters.Add(idPar);
            Commanddestino.Parameters.Add(pidPar);
            Commanddestino.Parameters.Add(templatePar);
            Commanddestino.Parameters.Add(caminhoPar);
            Commanddestino.Parameters.Add(templateXml);
            Commanddestino.Parameters.Add(iso_Template);

            try
            {
                Commanddestino.ExecuteNonQuery();
            }

            catch (MySqlException ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Atualiza no Banco informação de último inserido
        private void update_state(string ult_arq, string data)
        {
            MySqlConnection Conexaoorigem, Conexaodestino;
            string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
            string ConDestino = ConfigurationManager.ConnectionStrings["ConexaoDestino"].ConnectionString;

            //configura conexoes
            Conexaoorigem = new MySqlConnection(ConOrigem);
            Conexaodestino = new MySqlConnection(ConDestino);

            //configura comandos sql
            //string strcomando_origem = "Select item_id, item_data from biometria where item_type ='1'"; //;AND item_id%" + tbdividir.Text + "=" + tbresto.Text;

            try
            {
                Conexaoorigem.Open();
                Conexaodestino.Open();
            }

            catch (MySqlException ex)
            {
                throw ex;
            }

            MySqlCommand Commanddestino = new MySqlCommand();
            //MySqlCommand Commanddestino_xml = new MySqlCommand();

            Commanddestino.Connection = Conexaodestino;
            //Commanddestino_xml.Connection = Conexaodestino;

            Commanddestino.CommandText = "insert into file_state ( ultimo_arquivo, data_criacao) values(@ult_arq, @data)";
            //Commanddestino_xml.CommandText = "insert into template_xml (ItemId, PersonId, Template_xml) values(@idPar, @pidPar, @templateXml)";

            MySqlParameter ult_arqPar = new MySqlParameter("@ult_arq", ult_arq);
            MySqlParameter dataPar = new MySqlParameter("@datar", data);

            Commanddestino.Parameters.Add(ult_arqPar);
            Commanddestino.Parameters.Add(dataPar);

            try
            {
                Commanddestino.ExecuteNonQuery();
            }

            catch (MySqlException ex)
            {
                throw ex;
            }

          

        }
        #endregion

        #region Seleciona última entrada no banco e a data.
        public static string loadState()
        {

            try
            {
                string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
                string data_criacao = "";
                MySqlConnection conn = new MySqlConnection(ConOrigem);
                MySqlCommand command = new MySqlCommand("select * from file_state where data_criacao=(select max(data_criacao) from file_state);", conn);
                conn.Open();
                
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    data_criacao = dr["data_criacao"].ToString();
                }

                dr.Close();
                return data_criacao;
            }
            catch (MySqlException ex)
            {
                return null;
                throw ex;
            }

        }

        #endregion
    }
}
