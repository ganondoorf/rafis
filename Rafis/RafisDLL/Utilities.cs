using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Windows.Forms;

namespace RafisDLL
{
    public static class Utilities
    {
        //define o diretório path
        static System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
        static string fullProcessPath = ass.Location;
        static string desiredDir = Path.GetDirectoryName(fullProcessPath);

        #region Manipulação do processo Rafis
        /// <summary>
        /// Inicia o processo Rafis
        /// </summary>
        public static bool StartService(string serviceName, string[] args, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);

            if (service.Status != ServiceControllerStatus.Running)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                    service.Start(args);
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    return true; 
                }
                catch (Exception e)
                {
                    MessageBox.Show("Para execução do aplicativo é necessária permissão de Administrador. Erro:" + e.ToString(),
                    "Rafis Core",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("O Processos Rafis Core já está rodando.",
                "Rafis Core", MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
                return true;
            }
        }

        /// <summary>
        /// Termina o processo Rafis
        /// </summary>
        public static bool StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            service.ServiceName = serviceName;
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Para execução do aplicativo é necessária permissão de Administrador. Erro:" + e.ToString(),
                    "Rafis Core",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("O Processos Rafis Core já esta parado.",
                "Rafis Core", MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
                return true;
            }
        }

        /// <summary>
        /// Reinicia o processo Rafis
        /// </summary>
        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        public static Boolean isRafisRunning()
        {
            ServiceController service = new ServiceController("Rafis");
            if (service.Status == ServiceControllerStatus.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        
        }
        #endregion

        #region Comandos executados sincronamente
        /// <span class="code-SummaryComment"><summary></span>
        /// Executa comando no prompt, de forma sincrona.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="command">string de commando</param></span>
        /// <span class="code-SummaryComment"><returns>Void, entretanto apresenta Mbox com as informações resultantes do comando.</returns></span>
        public static void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                //Console.WriteLine(result);
                MessageBox.Show(result);
            }
            catch (Exception objException)
            {
                MessageBox.Show("Erro no acesso ao serviço Rafis. Erro: " + objException.ToString());
            }
        }
        #endregion

        #region Dpapi - conexão segura - Não Implementado.
        public static void ProtectConnectionString()
        {
            ToggleConnectionStringProtection(desiredDir, true);
        }

        public static void UnprotectConnectionString()
        {
            ToggleConnectionStringProtection(desiredDir, false);
        }

        
        private static void ToggleConnectionStringProtection (string pathName, bool protect)
        {
            // Define the Dpapi provider name.
            string strProvider = "DataProtectionConfigurationProvider";
            // string strProvider = "RSAProtectedConfigurationProvider";

            System.Configuration.Configuration oConfiguration = null;
            System.Configuration.ConnectionStringsSection oSection = null;

            try
            {
                // Open the configuration file and retrieve 
                // the connectionStrings section.

                // For Web!
                // oConfiguration = System.Web.Configuration.
                //                  WebConfigurationManager.OpenWebConfiguration("~");

                // For Windows!
                // Takes the executable file name without the config extension.
                oConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(pathName);

                if (oConfiguration != null)
                {
                    bool blnChanged = false;

                    oSection = oConfiguration.GetSection("ConexaoOrigem") as System.Configuration.ConnectionStringsSection;

                    if (oSection != null)
                    {
                        if ((!(oSection.ElementInformation.IsLocked)) && (!(oSection.SectionInformation.IsLocked)))
                        {
                            if (protect)
                            {
                                if (!(oSection.SectionInformation.IsProtected))
                                {
                                    blnChanged = true;
                                    // Encrypt the section.
                                    oSection.SectionInformation.ProtectSection
                                (strProvider);
                                }
                            }
                            else
                            {
                                if (oSection.SectionInformation.IsProtected)
                                {
                                    blnChanged = true;
                                    // Remove encryption.
                                    oSection.SectionInformation.UnprotectSection();
                                }
                            }
                        }

                        if (blnChanged)
                        {
                            // Indicates whether the associated configuration section 
                            // will be saved even if it has not been modified.
                            oSection.SectionInformation.ForceSave = true;
                            // Save the current configuration.
                            oConfiguration.Save();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }
        #endregion

        #region Métodos de operação
        /// <summary>
        /// Enumera os arquivos biométricos no diretório especificado. Inclui subdiretórios.
        /// </summary>
        /// <param name="raiz">String do caminho do diretório</param>
        /// <returns>Número de arquivos em string.S</returns>
        public static string numArquivos(string raiz)
        {
            try
            {
                var files = Directory.EnumerateFiles(raiz, "*.obj", SearchOption.AllDirectories);
                return files.Count().ToString();
            }
            catch (UnauthorizedAccessException UAEx)
            {
               log(UAEx.Message);
                return null;
            }
            catch (PathTooLongException PathEx)
            {
                log(PathEx.Message);
                return null;
            }
        }

        public static string lastModDate(string raiz) 
        {
            var directory = new DirectoryInfo(raiz);
            var myFile = (from f in directory.GetFiles() orderby f.CreationTime descending select f).First();
            DateTime lastModified = System.IO.File.GetCreationTime(myFile.FullName.ToString());
            return lastModified.ToString("dd/MM/yy HH:mm:ss");   
        }

        public static string rangeDateFiles(string raiz)
        {
            var directory = new DirectoryInfo(raiz);
            DateTime from_date = DateTime.Now.AddMonths(-3);
            DateTime to_date = DateTime.Now;
            var files = (from f in directory.GetFiles().Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date) orderby f.CreationTime descending select f).First();
            return files.ToString();
        }

        //registro de log principal
        public static void log(string msg)
        {
            StreamWriter vWriter = new StreamWriter(desiredDir + "\\Rafis.log", true);
            vWriter.WriteLine(msg);
            vWriter.Flush(); vWriter.Close();
        }

        //registro de log em arquivo específico
        public static void log(string msg, string file)
        {
            StreamWriter vWriter = new StreamWriter(desiredDir + file, true);
            vWriter.WriteLine(msg);
            vWriter.Flush(); vWriter.Close();
        }
        #endregion 

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Endereço IP Local não determinado!");
        }

        

       
    }
}
