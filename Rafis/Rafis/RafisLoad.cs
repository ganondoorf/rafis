
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
                templateList = tempDAO.ListaVerNull();
                TemplateClient sender = new TemplateClient();
                foreach (Template item in templateList)
                {
                    item.No_destino = retornaNo(item.Cpf);
                    item.No_origem = fqdn;
                    item.Resultado=0;
                    item.Node_dbsize = CoMysql.CountTemplate();
                    tempDAO.UpdateSend(item);

                    
                    string[] connect = item.No_destino.Split(':');
                    //se o CPF não encontrado, encaminhar para todos... 1:n
                    if (item.No_destino=="")
                    {
                        //TODO
                    }
                    else
                    {
                        sender.SendToServer(item, connect[0], Int32.Parse(connect[1]));
                    }
                    tempDAO.UpdateSendResult(item.ItemId,1);
                }
            }
            catch (Exception ex)
            {
                Utilities.log("Falha no envio do template: "+ex);
            }
        }
        #endregion
        
    }
}
