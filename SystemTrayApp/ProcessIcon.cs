using System;
using System.Diagnostics;
using System.IO;
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
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
		}

		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resources.lupa;
			ni.Text = "Tray Monitor - Rafis Core";
			ni.Visible = true;

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        bool isSettingsLoaded = false;
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
            
			if (e.Button == MouseButtons.Left)
			{
				// Start Windows Explorer.
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