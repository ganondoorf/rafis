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
                        MySqlCommand cmd = new MySqlCommand("select * from ver_template WHERE resultado is null order by itemID asc;", con);
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
                            //template.No_origem = ;
                            //template.No_destino = ;
                            //template.PersonId = (int)item["personID"];
                            //template.CaminhoImagem = (string)item["caminhoImagem"];
                            // template.Node_dbsize = ;
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


        public void UpdateVer(Template template, string resultado)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                    try
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE `afis`.`ver_template` SET `no_destino`='" + template.No_destino + "', `resultado`='"+resultado+"' WHERE `itemID`='" + template.ItemId + "';", con);
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
