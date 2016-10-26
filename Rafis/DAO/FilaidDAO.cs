using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAO
{
    public class PessoaDAO : IfilaidDAO<Filaid>
    {
        private static readonly PessoaDAO instancia = new PessoaDAO();

        public static PessoaDAO GetInstance()
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
                        DataTable cliente = new DataTable();
                        da.Fill(cliente);
                        return cliente;
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
                        DataTable cliente = new DataTable();
                        da.Fill(cliente);
                        return cliente;
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
