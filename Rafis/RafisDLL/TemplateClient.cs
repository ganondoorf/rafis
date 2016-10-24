using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace RafisDLL
{

    public class TemplateClient
    {
        static BinaryFormatter fmtr = new BinaryFormatter();

        //static void load(string[] args)
        //{
        //    Random rnd = new Random();

        //    for (int i = 0; i < 10; i++)
        //    {
        //        TemplateShare testObj = new TemplateShare();
        //        testObj.field1 = rnd.Next();
        //        testObj.field2 = "field" + testObj.field1;

        //        SendToServer(testObj);
        //    }
        //}

        public void SendToServer(TemplateShare testObj, string ip,int port)
        {
            port=777;
            RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Enviando template " + testObj.cpf);
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            fmtr.Serialize(stream, testObj);
            //Console.WriteLine("Sent TestObject field1=" + testObj.field1 + ", field2=" + testObj.field2);
            stream.Close();
            client.Close();
        }
    }
}  
