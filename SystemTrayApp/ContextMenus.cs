using System;
using System.Diagnostics;
using System.Windows.Forms;
using SystemTrayApp.Properties;
using System.Drawing;
using System.ServiceProcess;
using System.ComponentModel;
using System.IO;
using System.Security.Principal;
using Microsoft.Build.Utilities;
using RafisDLL;

namespace SystemTrayApp
{
	/// <summary>
	/// Aplicativo auxiliar para monitoramento e controle do processo Rafis Core
	/// </summary>
	/// 
	class ContextMenus
	{
		

		/// <summary>
		/// About box está ativa
        /// 
		/// </summary>
		bool isAboutLoaded = false;
        bool isSettingsLoaded = false;

		#region Cria o menu contexto do tray
		/// <summary>
		/// Cria esta instância.
		/// </summary>
		/// <returns>ContextMenuStrip</returns>
		public ContextMenuStrip Create()
		{
			// Add menu padrão.
			ContextMenuStrip menu = new ContextMenuStrip();
			ToolStripMenuItem item;
			ToolStripSeparator sep;

			// Indexar arquivos de digitais.
			item = new ToolStripMenuItem();
			item.Text = "Indexar templates";
			item.Click += new EventHandler(Explorer_Click);
			item.Image = Resources.Explorer;
			menu.Items.Add(item);

			// Separador.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);
			
			// Instalar Serviço.
			item = new ToolStripMenuItem();
			item.Text = "Instalar Serviço";
			item.Click += new EventHandler(Install_Click);
			item.Image = Resources.Color_balance;
			menu.Items.Add(item);

			// Desinstalar Serviço.
			item = new ToolStripMenuItem();
			item.Text = "Desinstalar Serviço";
			item.Click += new EventHandler(Uninstall_Click);
			item.Image = Resources.Close_file;
			menu.Items.Add(item);

			// Separador.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Iniciar Serviço.
			item = new ToolStripMenuItem();
			item.Text = "Iniciar serviço";
			item.Click += new EventHandler(Start_Click);
			item.Image = Resources.Play;
			menu.Items.Add(item);

			// Parar Serviço.
			item = new ToolStripMenuItem();
			item.Text = "Para serviço";
			item.Click += new EventHandler(Stop_Click);
			item.Image = Resources.Stop;
			menu.Items.Add(item);

			// Separador.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// About.
			item = new ToolStripMenuItem();
			item.Text = "Sobre";
			item.Click += new EventHandler(About_Click);
			item.Image = Resources.About;
			menu.Items.Add(item);

			// Separador.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Sair.
			item = new ToolStripMenuItem();
			item.Text = "Sair";
			item.Click += new System.EventHandler(Exit_Click);
			item.Image = Resources.Exit;
			menu.Items.Add(item);

			return menu;
		}
		#endregion

		/// <summary>
		/// Recebe o local, Path, em que o programa está rodando.
		/// </summary>
		static System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
		static string fullProcessPath = ass.Location;
		string desiredDir = Path.GetDirectoryName(fullProcessPath);

		#region Define os cliques no Tray
		/// <summary>
		/// Trata o Click event, iniciando o Explorer na pasta do processo.
		/// </summary>
		/// <param name="sender">a origem do evento.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instância contendo a event data.</param>
		void Explorer_Click(object sender, EventArgs e)
		{
            if (!isSettingsLoaded)
            {
                isSettingsLoaded = true;
                new TelaPrincipal().ShowDialog();
                isSettingsLoaded = false;
            }
		}

		/// <summary>
		/// Trata o Click event, instala o serviço monitor Rafis Core.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Install_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("Deseja instalar o serviço monitor do Rafis?", "Instalar Rafis Core Service", MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.OK)
			{
				String dNetDir = ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.VersionLatest);
				dNetDir = dNetDir.Replace("Framework", "Framework").Replace(" ", "\\ ");
				//desiredDir = desiredDir.Replace(" ","\\ ");
				Utilities.ExecuteCommandSync(dNetDir.ToString() + "\\installUtil.exe \"" + desiredDir.ToString() + "\\Rafis.exe\"");
			}
		}

		/// <summary>
		/// Trata o Click event, desinstala o serviço monitor Rafis Core.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Uninstall_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("Deseja desinstalar o serviço monitor do Rafis?", "Desinstalar Rafis Core Service", MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.OK)
			{
				String dNetDir = ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.VersionLatest);
				dNetDir = dNetDir.Replace("Framework", "Framework64");
				Utilities.ExecuteCommandSync(dNetDir.ToString() + "\\installUtil.exe /u \"" + desiredDir.ToString() + "\\Rafis.exe\"");
			}
		}

		/// <summary>
		/// Tratamento do Evento Click para a opção Sobre.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void About_Click(object sender, EventArgs e)
		{
			if (!isAboutLoaded)
			{
				isAboutLoaded = true;
				new AboutBox().ShowDialog();
				isAboutLoaded = false;
			}
		}


		/// <summary>
		/// Tratamento do Evento Click para a opção Iniciar Serviço.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Start_Click(object sender, EventArgs e)
		{

			string nc_port = Properties.Settings.Default.nc_port;
            string nc_server = Properties.Settings.Default.nc_server;


            string[] parametros = { Properties.Settings.Default.nc_port, Properties.Settings.Default.nc_server, Properties.Settings.Default.nc_seed_port };

			   Utilities.StartService("Rafis",parametros, 10000);
			  
			
		}


		/// <summary>
		/// Handles the Click event of the About control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Stop_Click(object sender, EventArgs e)
		{
			if (true)
			{
				Utilities.StopService("Rafis", 100000);
			}
		}

		/// <summary>
		/// Processes a menu item.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
			Application.Exit();
		}
		#endregion
	}
}