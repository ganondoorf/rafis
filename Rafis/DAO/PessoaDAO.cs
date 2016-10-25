using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAO
{
    public class PessoaDAO : IPessoaDAO<Pessoa>
    {
        private static readonly PessoaDAO instancia = new PessoaDAO();

        public static PessoaDAO GetInstance()
        {
            return instancia;
        }
        public DataTable Consultar(string cpf)
        {
            try
            {
                using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
                {
                    throw new NotImplementedException();
                }
            }
            catch (MySqlException ex)
            {

                throw ex;
            }




        }

        public List<Pessoa> ExibirTodos()
        {
            throw new NotImplementedException();
        }

        public void Gravar(Pessoa obj)
        {
            throw new NotImplementedException();
        }
    }
}
