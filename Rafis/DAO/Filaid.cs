using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Filaid
    {
        private int _ordem;
        private byte[] _template;
        private int _itemId;
        private string _cpf;
        private int _score;
        private string _result; //converter bit to string neste caso.
        private string _no_orig;
        private double custo;

     public int Ordem
        {
            get
            {
                return _ordem;
            }

            set
            {
                _ordem = value;
            }
        }

        public byte[] Template
        {
            get
            {
                return _template;
            }

            set
            {
                _template = value;
            }
        }

        public int ItemId
        {
            get
            {
                return _itemId;
            }

            set
            {
                _itemId = value;
            }
        }

        public string Cpf
        {
            get
            {
                return _cpf;
            }

            set
            {
                _cpf = value;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                _score = value;
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
            }
        }

        public string No_orig
        {
            get
            {
                return _no_orig;
            }

            set
            {
                _no_orig = value;
            }
        }

        public double Custo
        {
            get
            {
                return custo;
            }

            set
            {
                custo = value;
            }
        }
    }
}
