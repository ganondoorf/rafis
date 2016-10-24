using System;
using System.Windows.Forms;

namespace SystemTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            //var wi = WindowsIdentity.GetCurrent();
            //var wp = new WindowsPrincipal(wi);
 
            //bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            //if (!runAsAdmin)
            //if(false)
            //{
            //    // It is not possible to launch a ClickOnce app as administrator directly,
            //    // so instead we launch the app as administrator in a new process.
            //    var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

            //    // The following properties run the new process as administrator
            //    processInfo.UseShellExecute = true;
            //    processInfo.Verb = "runas";

            //    // Start the new process
            //    try
            //    {
            //        Process.Start(processInfo);
            //    }
            //    catch (Exception)
            //    {
            //        // The user did not allow the application to run as administrator
            //        MessageBox.Show("Você não pode executar esta aplicação como administrador ");
            //    }

            //    // Shut down the current process
            //    Application.Exit();
            //}
            //else 
            //{

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Show the system tray icon.					
                using (ProcessIcon pi = new ProcessIcon())
                {
                    pi.Display();

                    // Make sure the application runs!
                    Application.Run();
                }
            //}
		}
	}
}