using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO
{
    public class TemplateDAO : ITempate<Template>
    {
        private static readonly TemplateDAO instancia = new TemplateDAO();

        public static TemplateDAO GetInstance()
        {
            return instancia;
        }

        public DataTable GetTemplates() 
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                {
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("Select * from template_view;", con);
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

        public List<Template> ListaTodos()
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("Select * from template_view;", con);
                        List<Template> listaTemplates = new List<Template>();
                        MySqlDataReader item = cmd.ExecuteReader();

                        while (item.Read())
                        {
                            Template template = new Template();
                            template.ItemId = (int)item["itemID"];
                            template.PersonId = (int)item["personID"];
                            template.TemplateSA = (byte[])item["template"];
                            template.CaminhoImagem = (string)item["caminhoImagem"];
                            template.Cpf = (string)item["CPF"];
                            listaTemplates.Add(template);
                        }
                        return listaTemplates;
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

        public List<Template> ListaVerNull()
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("select * from send_template WHERE resultado is null order by itemID asc;", con);
                        List<Template> listaTemplates = new List<Template>();
                        MySqlDataReader item = cmd.ExecuteReader();

                        while (item.Read())
                        {
                            Template template = new Template();
                            template.OpId = Convert.ToInt32(item["ItemID"]);
                            template.Operacao = Convert.ToInt32(item["op"]);
                            template.TemplateSA = (byte[])item["template"];
                            template.IsoTemplate = (byte[])item["isoTemplate"];
                            template.Cpf = (string)item["CPF"];
                            template.Id_dedo = (string)item["dedoID"];
                            template.No_origem = null ;
                            template.No_destino = null;
                            //template.PersonId = (int)item["personID"];
                            //template.CaminhoImagem = (string)item["caminhoImagem"];
                            template.Node_dbsize = 0;
                            listaTemplates.Add(template);
                        }
                        return listaTemplates;
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

        public void UpdateSend(Template template)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        //ver_template = send_template | filaid = proc_template
                        string sql = "UPDATE `afis`.`send_template` SET `op`=@op,`personID`=@personID,`template`=@template,`no_destino`=@no_destino,`Template_xml`=@Template_xml,`isoTemplate`=@isoTemplate,`CPF`=@cpf,`dedoID`=@dedoID,`resultado`=@resultado,`score`=@score WHERE `itemID`=@itemID;";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@itemID", template.ItemId);
                        cmd.Parameters.AddWithValue("@op", template.Operacao);
                        cmd.Parameters.AddWithValue("@personID", template.PersonId);
                        cmd.Parameters.AddWithValue("@template", template.TemplateSA);
                        cmd.Parameters.AddWithValue("@no_destino", template.No_destino);
                        cmd.Parameters.AddWithValue("@Template_xml", template.TemplateXml);
                        cmd.Parameters.AddWithValue("@isoTemplate", template.IsoTemplate);
                        cmd.Parameters.AddWithValue("@cpf", template.Cpf);
                        cmd.Parameters.AddWithValue("@dedoID", template.Id_dedo);
                        cmd.Parameters.AddWithValue("@resultado", template.Resultado);
                        cmd.Parameters.AddWithValue("@score", template.Score);
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

        public void UpdateSendResult(int itemID, int result)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        //ver_template = send_template | filaid = proc_template
                        string sql = "UPDATE `afis`.`send_template` SET `resultado`=@resultado WHERE `itemID`=@itemID;";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@itemID", itemID);
                        cmd.Parameters.AddWithValue("@resultado", result);
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

        public void Gravar(Template obj)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCPF()
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT distinct(CPF),personID FROM template_view where CPF!=0;", con);
                        List<string> cpfs = new List<string>();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            cpfs.Add((string)dr["CPF"]);
                        }
                        return cpfs;
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
    }
}
