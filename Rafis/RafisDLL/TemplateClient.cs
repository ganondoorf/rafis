using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using DAO;

namespace RafisDLL
{
    public class TemplateClient
    {
        static BinaryFormatter fmtr = new BinaryFormatter();
        public void SendToServer(Template testObj, string ip,int port)
        {
            port=777;
            RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Enviando template " + testObj.Cpf);
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            fmtr.Serialize(stream, testObj);
            stream.Close();
            client.Close();
        }
    }
}  
