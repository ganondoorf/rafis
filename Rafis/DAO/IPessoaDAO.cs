using System.Collections.Generic;
using System.Data;


namespace DAO
{
    interface IPessoaDAO<T>
    {
        List<T> ExibirTodos();
        void Gravar(T obj);
        DataTable Consultar(string nome);
    }
}
