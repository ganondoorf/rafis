
using NChordLib;
using RafisDLL;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using DAO;

namespace Rafis
{
    class RafisLoad
    {
        bool running = true;
        string fqdn = System.Net.Dns.GetHostEntry("LocalHost").HostName;
        int DBsize = 0;

        #region Inicia servidor Nchord
        public void load(int localport, string seedip, int seedport)
        {
            //inicia novo anel
            int portNum = localport;
            ChordServer.LocalNode = new ChordNode(fqdn, portNum);
            Utilities.log("Iniciando instância "+ fqdn + " : "+DateTime.Now.ToString());
            ChordInstance instance;

            //Se o Nchord não está rodando...
            if (ChordServer.RegisterService(portNum))
            {
                instance = ChordServer.GetInstance(ChordServer.LocalNode);
                //Verifica se o processo Nchord vai ser seed ou nó cliente.
                if (seedip=="")
                {
                    instance.Join(null, ChordServer.LocalNode.Host, ChordServer.LocalNode.PortNumber);
                }
                else
                {
                    instance.Join(new ChordNode(seedip, seedport), ChordServer.LocalNode.Host, ChordServer.LocalNode.PortNumber);
                }
                try
                {
                    List<string> cpfs = new List<string>();
                    foreach (string item in cpfs)
                    {
                        instance.AddKey(item, instance.Host.ToString());
                        Utilities.log("[" + DateTime.Now.ToString() + "] " + "Protuário " + item + " do Estado " + instance.Host.ToString() + " inserido na base.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao recuperar os CPFs do banco " + ex.Message);
                }
            }

            instance = ChordServer.GetInstance(ChordServer.LocalNode);
            running = true;
            while (running)
            {
                int count = 0;
                foreach (var item in instance.PrintKeys())
                {
                    instance.syncKey(item.Value, instance.PrintRegion(item.Key));
                    count++;
                }
                //captura tamanho da base.
                DBsize = count;
                //envia templates
                sendTemplates();
                Utilities.log("[" + DateTime.Now.ToString() + "] " + "Rodando: " + count + " prontuarios disponíveis.");
                System.Threading.Thread.Sleep(10000);
            }
            Utilities.log("Encerrando: " + DateTime.Now.ToString());
        }
  
        public void close() 
        {
            running = false;
        }
        #endregion

        public string retornaNo(string cpf) 
        {
            ChordInstance instance = ChordServer.GetInstance(ChordServer.LocalNode);
            ulong id = ChordServer.GetHash(cpf);
            return instance.FindHost(id);
        }

        #region Envia templates
        private void sendTemplates() 
        {
            try
            {
                TemplateDAO tempDAO = new TemplateDAO();
                List<Template> templateList = new List<Template>();
                templateList = tempDAO.ListaTodos();
                foreach (Template item in templateList)
                {
                    item.No_destino = retornaNo(item.Cpf);
                    item.No_origem = fqdn;
                    item.Node_dbsize = CoMysql.CountTemplate();
                    tempDAO.UpdateVer(item,"0");

                    MySqlCommand command_update = new MySqlCommand("UPDATE `afis`.`ver_template` SET `no_destino`='" + no_destino + "', `resultado`=0 WHERE `itemID`='" + dr["itemID"].ToString() + "';", conn2);
                    command_update.ExecuteNonQuery();



                    TemplateClient sender = new TemplateClient();
                    string[] connect = no_destino.Split(':');
                    sender.SendToServer(templateSend, connect[0], Int32.Parse(connect[1]));



                    MySqlCommand two = new MySqlCommand("UPDATE `afis`.`ver_template` SET `resultado`=1 WHERE `itemID`='" + dr["itemID"].ToString() + "';", conn2);
                    two.ExecuteNonQuery();
                    
                    database.Add(Enroll(item.ItemId, item.PersonId, item.TemplateSA, item.CaminhoImagem, item.Cpf));
                    Utilities.log("[" + DateTime.Now.ToString() + "] " + "Recuperando template " + item.ItemId + "...", "//RafisCore.log");
                }













                MySqlConnection conn2 = new MySqlConnection(ConOrigem);
                MySqlCommand command_select = new MySqlCommand(, conn);
                conn.Open();
                conn2.Open();

                TemplateShare templateSend = new TemplateShare();
                MySqlDataReader dr = command_select.ExecuteReader();

                while (dr.Read())
                {
                   
                }
                conn2.Close(); 
                dr.Close();
                conn.Clone();      
            }
            catch (Exception ex)
            {
                Utilities.log("Falha no envio do template: "+ex);
            }

        }
        #endregion
        
    }
}
