using System;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using MySql.Data.MySqlClient;
using DAO;

namespace RafisDLL
{
    public class TemplateServer
    {
        static BinaryFormatter fmtr = new BinaryFormatter();
        static TcpListener server = null;
        string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;

        public void load(IPAddress endereco, int port)
        {
            try
            {
                Byte[] bytes = new Byte[32];
                server = new TcpListener(endereco, port);
                MySqlConnection conn = new MySqlConnection(ConOrigem);
                MySqlConnection conn2 = new MySqlConnection(ConOrigem);

                server.Start();
                RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Servidor de templates ativo, endereço: " + endereco.ToString() + ":" + port.ToString());

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    string ip = client.Client.AddressFamily.ToString();
                    Template template_rec = (Template)fmtr.Deserialize(stream);

                    RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Template " + template_rec.Cpf + " recebido com sucesso: " + template_rec.OpId + ", " + template_rec.No_origem + ", " + template_rec.Operacao + ", " + template_rec.Id_dedo, "//TemplateServer.log");

                    FilaidDAO.InsertFilaID(template_rec);
                    FilaidDAO.UpdateRanking(template_rec.No_origem, false, true, false, template_rec.Node_dbsize);
                  
                    stream.Close();
                    client.Close();
                }
            }
            finally
            { 
                server.Stop();
            }
        }

        public void stop()
        {
            server.Stop();
        }

        #region Envia o resultado ao nó solicitante - REFAZER!
        public void sendReturn()
        {
            throw new NotImplementedException();
        }
        #endregion
        
    }
}