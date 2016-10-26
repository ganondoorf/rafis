using System.Collections.Generic;
using System.Data;


namespace DAO
{
    interface IfilaidDAO<T>
    {
        List<T> ExibirTodos();
        void Gravar(T obj);
        DataTable ConsultarResult(string resultado);
    }
}
