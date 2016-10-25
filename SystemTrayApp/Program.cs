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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                // Apresenta o icone system tray.					
                using (ProcessIcon pi = new ProcessIcon())
                {
                    pi.Display();
                    Application.Run();
                }
        }
	}
}