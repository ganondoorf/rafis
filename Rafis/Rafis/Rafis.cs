using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using RafisDLL;
using DAO;

namespace Rafis
{
    public partial class Rafis : ServiceBase
    {
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
                vWriter.WriteLine("Servico Parado: " + DateTime.Now); vWriter.Flush(); vWriter.Close();
            }
            //temporizador de log...
            private void timer1_Tick(object sender)
            {
                string files = Utilities.numArquivos(@"C:\Afis");
                string lastDate = CoMysql.loadState();
                Utilities.log("Servico Rodando: " + DateTime.Now+". Com "+files+" arquivos listados. Última modificação em: "+lastDate);
            }
            private void ThreadNChord()
            {
                //load.load(Convert.ToInt32(localPort), ip, Convert.ToInt32(seedPort));
                load.load(1000, "", 0);
            }
            private void ThreadTransfer()
            {
                var hostname = System.Net.Dns.GetHostEntry("LocalHost").HostName;
                System.Net.IPAddress iplocal = Utilities.GetLocalIPAddress();
                server.load(iplocal,777);
            }
            private void ThreadCore()
            {
                rserver.load();
            }
            
    }
}