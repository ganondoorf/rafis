using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAO
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
                        MySqlCommand cmd = new MySqlCommand("SELECT * from filaid where resultado='" + resultado +"' ORDER by custo ASC;", con);
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
                        MySqlCommand cmd = new MySqlCommand("SELECT * from filaid where resultado is null ORDER by custo ASC;", con);
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
