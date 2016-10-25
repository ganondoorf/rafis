using System;
using System.Windows.Forms;
using SystemTrayApp.Properties;

namespace SystemTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
	{
		/// <summary>
		/// O Objeto NotifyIcon.
		/// </summary>
		NotifyIcon ni;
		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			//Instancia objeto NotifyIcon
			ni = new NotifyIcon();
		}
		/// <summary>
		/// Apresenta o icone no system tray.
		/// </summary>
		public void Display()
		{
			// Coloca o ícone no tray.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resources.lupa;
			ni.Text = "Tray Monitor - Rafis Core";
			ni.Visible = true;
			// Anexa o menu contexto.
			ni.ContextMenuStrip = new ContextMenus().Create();
		}
		/// <summary>
		/// Libera os recursos do tray.
		/// </summary>
		public void Dispose()
		{
			// Quando a aplicação fecha, remove o icone do tray.
			ni.Dispose();
		}
		/// <summary>
		/// Manipula o evento do mouse no ni control.
		/// </summary>
		/// <param name="sender">fonte do evento.</param>
		/// <param name="e">A <see cref="System.Windows.Forms.MouseEventArgs"/> instância contendo os dados do evento.</param>
        bool isSettingsLoaded = false;
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                if (!isSettingsLoaded)
                {
                    isSettingsLoaded = true;
                    new TelaPrincipal().ShowDialog();
                    isSettingsLoaded = false;
                }
			}
		}
	}
}