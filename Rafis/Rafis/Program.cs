using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
