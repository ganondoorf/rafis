using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Pessoa
    {
        private int opId;
        private int operacao;
        private string _cpf;
        private string _personID;
        
        public string cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }
        public string personId
        {
            get { return _personID; }
            set { _personID = value;}
        }
    }
}
