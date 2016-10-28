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
    }
}
