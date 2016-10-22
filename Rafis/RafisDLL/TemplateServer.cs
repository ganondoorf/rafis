using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using NChordLib;
using RafisDLL;
using System.Configuration;
using MySql.Data.MySqlClient;

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
                //server = new TcpListener(IPAddress.Parse(endereco), port);
                server = new TcpListener(endereco, port);
                MySqlConnection conn = new MySqlConnection(ConOrigem);
                MySqlConnection conn2 = new MySqlConnection(ConOrigem);

                server.Start();
                RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Servidor de templates ativo, endereço: " + endereco.ToString() + ":" + port.ToString());

                while (true)
                {
                    //Console.WriteLine("Waiting for a connection (ctrl+c to exit)...");
                    
                    
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    string ip = client.Client.AddressFamily.ToString();
                    TemplateShare testObj = (TemplateShare)fmtr.Deserialize(stream);

                    //Console.WriteLine("received test object field1=" + testObj.field1 + ", field2=" + testObj.field2);
                    RafisDLL.Utilities.log("[" + DateTime.Now.ToString() + "] " + "Template " + testObj.cpf + " recebido com sucesso: " + testObj.opId + ", " + testObj.no_origem + ", " + testObj.operacao + ", " + testObj.id_dedo, "//TemplateServer.log");

                    MySqlCommand Commanddestino = new MySqlCommand();
                    MySqlCommand InsertNodes = new MySqlCommand();
                    //MySqlCommand Commanddestino_xml = new MySqlCommand();

                    Commanddestino.Connection = conn;
                    InsertNodes.Connection = conn;

                    //Commanddestino_xml.Connection = Conexaodestino;
                    int result = 0;
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT EXISTS(SELECT 1 FROM afis.ranking_node WHERE name=@Name LIMIT 1)";
                        cmd.Parameters.AddWithValue("@Name", testObj.no_origem);
                        conn.Open();
                        try
                        {
                            result = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        catch (Exception e)
                        {

                            Utilities.log("[" + DateTime.Now.ToString() + "] " + "Erro na inserção do template recebido: " + e);
                        } 
                        ////result will be 1 if it exists, and 0 if it does not..
                        conn.Close();
                    }


                    Commanddestino.CommandText = "INSERT INTO `afis`.`filaid` (`ItemId`, `CPF`, `Template`, `no_orig`) VALUES ('" + testObj.opId + "', '" + testObj.cpf + "', @isoTemplatePar, '" + testObj.no_origem + "');";
                    InsertNodes.CommandText = "INSERT INTO `afis`.`ranking_node` (`Name`, `DispNode`, `ReqNode`, `RespNode`, `NumPront`) VALUES ('" + testObj.no_origem + "', 0, 0, 0, '" + testObj.node_dbsize + "');";
                    
                    MySqlParameter iso_Template = new MySqlParameter("@isoTemplatePar", MySqlDbType.VarBinary);

                    iso_Template.Value =  testObj.template_iso;

                    Commanddestino.Parameters.Add(iso_Template);

                    conn.Open();
                    try
                    {
                        if (0==result)
                        {
                            InsertNodes.ExecuteNonQuery();
                            Commanddestino.ExecuteNonQuery();
                        }
                        Commanddestino.ExecuteNonQuery();
                        Utilities.MySQLCommand("UPDATE `afis`.`ranking_node` SET `ReqNode`=`ReqNode`+1,`NumPront`='" + testObj.node_dbsize + "' where Name='" + testObj.no_origem + "';", conn2);
                        Utilities.updateCost();
                    }
                    catch (Exception e)
                    {
                        Utilities.log("[" + DateTime.Now.ToString() + "] " + "Erro na inserção do template recebido: " + e);
                    }
                    conn.Close();
                    stream.Close();
                    client.Close();

                    //
                }
            }
            finally
            {
                // Stop listening for new clients.  
                server.Stop();
            }
        }

        public void stop() {
            server.Stop();
        }

        #region Envia o resultado ao nó solicitante
        public void sendReturn()
        {
        
            
        }

        #endregion





    }
}