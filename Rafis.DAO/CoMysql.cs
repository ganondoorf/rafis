using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace Rafis.DAO
{
    public static class CoMysql
    {
        public static void GenericCommand(string myExecuteQuery)
        {
           try
            {
                MySqlConnection con = ConnectDB.GetInstancia.GetConnection();
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

		#region Retorna Caminho do Arquivo de Imagem
		public static string selectImageName(int itemId)
		{
			try
			{
				using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
				{
					using (MySqlCommand command = new MySqlCommand("select CaminhoImagem from template where ItemId =" + itemId + ";", con))
					{
						con.Open();
						//List<template> listaTemplates = new List<template>()                
						using (MySqlDataReader dr = command.ExecuteReader())
						{
							dr.Read();
							string fileName = (string)dr["CaminhoImagem"];
							dr.Close();
							return fileName;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao acessar o banco de dados: " + ex.Message);
			}
		}
		#endregion

        public static void UpdateResultFilaid(string itemId, string score)
        {
            try
            {
                MySqlConnection con = ConnectDB.GetInstancia.GetConnection();
                MySqlCommand myCommand = new MySqlCommand("UPDATE `afis`.`filaid` SET `resultado`='4', `score`='" + score + "' WHERE `itemID`='" + itemId + "';", con);
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }
        }
        
        public static int CountTemplate()
        {
            try
            {
                MySqlConnection con;
                int totalRows;
                //Lista o número de templates -->
                using (con = ConnectDB.GetInstancia.GetConnection())
                {
                    MySqlCommand myCommand = new MySqlCommand("SELECT COUNT(*) FROM template", con);
                    con.Open();
                    totalRows = Convert.ToInt32(myCommand.ExecuteScalar());
                    con.Close();
                    return totalRows;
                }
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }
        }
        
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
        public static void update_state(string ult_arq, string data)
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

        #region Atualiza o custo 
        public static void updateCost()
        {
            try
            {
                string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
                //Utilities.log("Banco: "+Properties.Settings.Default.Banco+"\n");
                MySqlConnection conn = new MySqlConnection(ConOrigem);
                MySqlConnection conn2 = new MySqlConnection(ConOrigem);
                MySqlCommand command = new MySqlCommand("SELECT * from `afis`.`filaid` where custo is null;", conn);

                conn.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    int Ordem = (int)dr["Ordem"];
                    string no_origem = dr["no_orig"].ToString();

                    MySqlCommand command2 = new MySqlCommand("SELECT * FROM afis.ranking_node where Name='" + no_origem + "';", conn2);
                    conn2.Open();
                    MySqlDataReader dr2 = command2.ExecuteReader();
                    dr2.Read();
                    double result = costCalc(Ordem, (int)dr2["DispNode"], (int)dr2["ReqNode"], (int)dr2["RespNode"], (int)dr2["NumPront"]);
                    dr2.Close();
                    conn2.Close();
                    CoMysql.GenericCommand("UPDATE `afis`.`filaid` SET `custo`='" + result.ToString().Replace(",", ".") + "' where Ordem='" + Ordem + "';");

                }
                dr.Close();
                conn.Close();
            }
            catch (Exception)
            {
                //Utilities.log("Utilities: Erro ao calcular o custo: " + ex);
            }
        }
        #endregion

		//#region Método conta numero de registro -->
		//int GetRowsCount(MySqlCommand command)
		//{
		//    int rowsCount = Convert.ToInt32(command.ExecuteScalar());
		//    return rowsCount;
		//}
		//#endregion

        #region Calcula o custo
        private static double costCalc(int OC, int DN, int NQ, int NR, int NP)
        {
            double resultado = OC - ((double)DN / 2) - ((double)NR / 2) + ((double)NQ / 100) - (5 * (double)NP / 1000000);
            return resultado;
        }
        #endregion

        #region Insere Templates nas tabelas de transferência
        public static void insereTransfer(string cpf, string dedoID, byte[] template, byte[] isoTemplate, string xmlTemplate, string destino, int op)
        {
            //configura conexoes
            MySqlConnection con = ConnectDB.GetInstancia.GetConnection();
            try
            {
                con.Open();
            }
            catch (Exception)
            {
                throw;
            }
            MySqlCommand Commanddestino = new MySqlCommand();
            Commanddestino.Connection = con;
            Commanddestino.CommandText = "insert into ver_template (CPF, no_destino, template, isoTemplate, Template_xml, dedoID, op) values (@cpf,@destinoPar,@templatePar,@isoTemplatePar,@templateXml, @dedoIDPar, " + op + ");";
            MySqlParameter cpfPar = new MySqlParameter("@cpf", cpf);
            MySqlParameter destinoPar = new MySqlParameter("@destinoPar", destino);
            MySqlParameter templatePar = new MySqlParameter("@templatePar", MySqlDbType.VarBinary);
            MySqlParameter iso_Template = new MySqlParameter("@isoTemplatePar", MySqlDbType.VarBinary);
            MySqlParameter templateXml = new MySqlParameter("@templateXml", xmlTemplate);
            MySqlParameter dedoIDPar = new MySqlParameter("@dedoIDPar", dedoID);
            templatePar.Value = template;
            iso_Template.Value = isoTemplate;
            Commanddestino.Parameters.Add(cpfPar);
            Commanddestino.Parameters.Add(destinoPar);
            Commanddestino.Parameters.Add(templatePar);
            Commanddestino.Parameters.Add(templateXml);
            Commanddestino.Parameters.Add(iso_Template);
            Commanddestino.Parameters.Add(dedoIDPar);
            try
            {
                Commanddestino.ExecuteNonQuery();
            }
            catch (Exception e)
            {
				throw e;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion 
    }
}
