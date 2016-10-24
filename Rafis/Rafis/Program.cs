using System.ServiceProcess;

namespace Rafis
{
    static class Program
    {
        /// <summary>
        /// Ponto inicial do serviço.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Rafis() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
