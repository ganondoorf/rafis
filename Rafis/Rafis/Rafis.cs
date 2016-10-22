using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RafisDLL;
using NChordLib;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Rafis
{
    public partial class Rafis : ServiceBase
    {
         
        //timer de log
        //Timer timer1;
        //Define path local
        static System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
        static string fullProcessPath = ass.Location;
        string desiredDir = Path.GetDirectoryName(fullProcessPath);

        RafisLoad load = new RafisLoad();
        TemplateServer server = new TemplateServer();
        RafisCore rserver = new RafisCore();
        
        
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private Thread _thread, _thread2, _thread3;
        string localPort; string ip; string seedPort;

            public Rafis()
            {
                InitializeComponent();
            }

            protected override void OnStart(string[] args)
            {
                localPort = args[0];
                ip = args[1];
                seedPort = args[2];
                //Utilities.log("Iniciando Nchord: "+localPort+ip+seedPort);
                //timer1 = new Timer(new TimerCallback(timer1_Tick), null, 15000, 60000);
                _thread = new Thread(ThreadNChord);
                _thread.Name = "Rafis Rchord Thread";
                _thread.IsBackground = true;
                _thread.Start();
                

                _thread2 = new Thread(ThreadTransfer);
                _thread2.Name = "Rafis Template Server Thread";
                _thread2.IsBackground = true;
                _thread2.Start();

                _thread3 = new Thread(ThreadCore);
                _thread3.Name = "Rafis Core Thread";
                _thread3.IsBackground = true;
                _thread3.Start();
            }

            protected override void OnStop()
            {
                _thread.Abort();
                server.stop();
                _thread2.Abort();
                    
                StreamWriter vWriter = new StreamWriter(desiredDir + "\\Rafis.log", true);
                vWriter.WriteLine("Servico Parado: " + DateTime.Now.ToString()); vWriter.Flush(); vWriter.Close();
            }

            //temporizador de log...
            private void timer1_Tick(object sender)
            {
                string files = Utilities.numArquivos(@"C:\Afis");
                string lastDate = RafisTools.loadState();
                Utilities.log("Servico Rodando: " + DateTime.Now.ToString()+". Com "+files+" arquivos listados. Última modificação em: "+lastDate);
            }
            private void ThreadNChord()
            {
                load.load(Int32.Parse(localPort), ip, Int32.Parse(seedPort));
                //Utilities.log(Int32.Parse(localPort)+ ip +  Int32.Parse(seedPort))
                //load.load(10000,"",0);
            }
            private void ThreadTransfer()
            {
                var hostname = System.Net.Dns.GetHostEntry("LocalHost").HostName;
                System.Net.IPAddress iplocal = GetIPAddress(hostname);
                //inicia servidor de template..
                server.load(iplocal,777);
            }
            private void ThreadCore()
            {
                rserver.load();
            
            }

            //método para retornar ip local
            public static System.Net.IPAddress GetIPAddress(string hostName)
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                var replay = ping.Send(hostName);

                if (replay.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return replay.Address;
                }
                return null;
            }
    }
}