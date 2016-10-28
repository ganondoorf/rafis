using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Template
    {
        private int _itemId;
        private int _personId;
        private byte[] _template;
        private byte[] _isoTemplate;
        private string _templateXml;
        private string _caminhoImagem;
        private string _cpf;
        private int _opId;
        private int _operacao;
        private string _id_dedo;
        private string _no_destino;
        private string _no_origem;
        private int _resultado;
        private int _score;
        private int node_dbsize;



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

        public int PersonId
        {
            get
            {
                return _personId;
            }

            set
            {
                _personId = value;
            }
        }

        public byte[] TemplateSA
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

        public byte[] IsoTemplate
        {
            get
            {
                return _isoTemplate;
            }

            set
            {
                _isoTemplate = value;
            }
        }

        public string TemplateXml
        {
            get
            {
                return _templateXml;
            }

            set
            {
                _templateXml = value;
            }
        }

        public string CaminhoImagem
        {
            get
            {
                return _caminhoImagem;
            }

            set
            {
                _caminhoImagem = value;
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

        public int OpId
        {
            get
            {
                return _opId;
            }

            set
            {
                _opId = value;
            }
        }

        public int Operacao
        {
            get
            {
                return _operacao;
            }

            set
            {
                _operacao = value;
            }
        }

        public string Id_dedo
        {
            get
            {
                return _id_dedo;
            }

            set
            {
                _id_dedo = value;
            }
        }

        public string No_destino
        {
            get
            {
                return _no_destino;
            }

            set
            {
                _no_destino = value;
            }
        }

        public string No_origem
        {
            get
            {
                return _no_origem;
            }

            set
            {
                _no_origem = value;
            }
        }

        public int Resultado
        {
            get
            {
                return _resultado;
            }

            set
            {
                _resultado = value;
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

        public int Node_dbsize
        {
            get
            {
                return node_dbsize;
            }

            set
            {
                node_dbsize = value;
            }
        }
    }
}