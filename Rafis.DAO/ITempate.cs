using System.Collections.Generic;
using System.Data;

namespace Rafis.DAO
{
    interface ITempate<T>
    {
        List<T> ListaTodos();
        void Gravar(T obj);
        DataTable GetTemplates();
    }
}
