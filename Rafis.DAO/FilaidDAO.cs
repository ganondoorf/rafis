using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Rafis.DAO
{
    public class FilaidDAO : IfilaidDAO<Filaid>
    {
        private static readonly FilaidDAO instancia = new FilaidDAO();

        public static FilaidDAO GetInstance()
        {
            return instancia;
        }

        public DataTable ConsultarResult(string resultado)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                {
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT * from proc_template where resultado='" + resultado +"' ORDER by custo ASC;", con);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable itens = new DataTable();
                        da.Fill(itens);
                        return itens;
                    }
                    catch (MySqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public DataTable ConsultarResult()
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                {
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT * from proc_template where resultado is null ORDER by custo ASC;", con);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable itens = new DataTable();
                        da.Fill(itens);
                        return itens;
                    }
                    catch (MySqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public static void InsertFilaID(Template template)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        //ver_template = send_template | filaid = proc_template
                        //"INSERT INTO `afis`.`filaid` (`ItemId`, `CPF`, `Template`, `no_orig`) VALUES ('" + testObj.opId + "', '" + testObj.cpf + "', @isoTemplatePar, '" + testObj.no_origem + "');"
                        string sql = "INSERT INTO `afis`.`proc_template` (`itemID`,`template`,`CPF`,`no_orig`) VALUES (@itemID, @isoTemplate, @cpf, @no_origem);";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@itemID", template.ItemId);
                        //cmd.Parameters.AddWithValue("@op", template.Operacao);
                        //cmd.Parameters.AddWithValue("@personID", template.PersonId);
                        //cmd.Parameters.AddWithValue("@template", template.TemplateSA);
                        cmd.Parameters.AddWithValue("@no_origem", template.No_origem);
                        //cmd.Parameters.AddWithValue("@Template_xml", template.TemplateXml);
                        cmd.Parameters.AddWithValue("@isoTemplate", template.IsoTemplate);
                        cmd.Parameters.AddWithValue("@cpf", template.Cpf);
                        //cmd.Parameters.AddWithValue("@dedoID", template.Id_dedo);
                        //cmd.Parameters.AddWithValue("@resultado", template.Resultado);
                        //cmd.Parameters.AddWithValue("@score", template.Score);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        
        }

        public static void UpdateRanking(string Name, int DispNode, int ReqNode, int RespNode, int Node_dbsize)
        { 
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                    MySqlCommand Commanddestino = new MySqlCommand();
                    MySqlCommand InsertNodes = new MySqlCommand();

                    Commanddestino.Connection = con;
                    InsertNodes.Connection = con;

                    int result = TemplateDAO.ifNodeExist(Name);
                    if (result==1)
	                        {
                            string sql = "UPDATE `afis`.`ranking_node` SET `ReqNode`=`ReqNode`+"+ReqNode+", `DispNode`=`DispNode`+"+DispNode+ ", `RespNode`=`RespNode`+"+RespNode+" ,`NumPront`=@Node_dbsize where `Name`=@Name;";
                            MySqlCommand cmd = new MySqlCommand(sql, con);
                            cmd.Parameters.AddWithValue("@Name", Name);
                            cmd.Parameters.AddWithValue("@Node_dbsize", Node_dbsize);
                            con.Open();
                            cmd.ExecuteNonQuery();
	                        } 
                    else
	                        {
                            string sql = "INSERT INTO `afis`.`ranking_node` (`Name`, `DispNode`, `ReqNode`, `RespNode`, `NumPront`) VALUES (@Name, 0, 0, 0, @Node_dbzise);";
                            MySqlCommand cmd = new MySqlCommand(sql, con);
                            cmd.Parameters.AddWithValue("@Name", Name);
                            cmd.Parameters.AddWithValue("@Node_dbsize", Node_dbsize);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            }
                    CoMysql.updateCost();
                        
                    }
                    catch (MySqlException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

                    






        }

        public List<Filaid> ExibirTodos()
        {
            throw new NotImplementedException();
        }

        public void Gravar(Filaid obj)
        {
            throw new NotImplementedException();
        }
    }
}
