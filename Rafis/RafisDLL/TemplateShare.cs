using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RafisDLL
{
    [Serializable]
    public class TemplateShare
    {
        public int opId;
        public int operacao; 
        public string cpf;
        public string personID;
        public string id_dedo;
        public byte[] template;
        public byte[] template_iso;
        public string no_destino;
        public string no_origem;
        public int resultado;
        public int score;
        public int node_dbsize;

    }  


}
