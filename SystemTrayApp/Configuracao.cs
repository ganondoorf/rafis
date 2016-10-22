using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemTrayApp
{
    public partial class Configuracao : Form
    {
        public Configuracao()
        {
            InitializeComponent();
            maskedTextBox1.ValidatingType = typeof(System.Net.IPAddress);
        }

        private void Configuracao_Load(object sender, EventArgs e)
        {
            
            maskedTextBox1.Text = Properties.Settings.Default.IP.ToString();
            textBox1.Text = Properties.Settings.Default.Porta.ToString();
            textBox2.Text = Properties.Settings.Default.Banco;
            textBox4.Text = Properties.Settings.Default.Login;
            maskedTextBox2.Text = Properties.Settings.Default.Senha;
            textBox3.Text = Properties.Settings.Default.Tabela;
            textBox5.Text = Properties.Settings.Default.campo_cpf;
            textBox6.Text = Properties.Settings.Default.campo_template;
            textBox7.Text = Properties.Settings.Default.nc_port;
            textBox8.Text = Properties.Settings.Default.nc_server;
            textBox9.Text = Properties.Settings.Default.nc_seed_port;
            
            RafisGetConf();


            if (Properties.Settings.Default.Senha!="")
            {
                toolStripStatusLabel1.Text = "Configurações presentes.";
            }
        }

        public void RafisGetConf()
        {

            //Rafis.Properties.Settings.Default.IP = Properties.Settings.Default.IP;
            //Rafis.Properties.Settings.Default.Porta = Properties.Settings.Default.Porta;
            //Rafis.Properties.Settings.Default.Banco = Properties.Settings.Default.Banco;
            //Rafis.Properties.Settings.Default.Login = Properties.Settings.Default.Login;
            //Rafis.Properties.Settings.Default.Senha = Properties.Settings.Default.Senha;
            //Rafis.Properties.Settings.Default.Tabela = Properties.Settings.Default.Tabela;
            //Rafis.Properties.Settings.Default.campo_cpf = Properties.Settings.Default.campo_cpf;
            //Rafis.Properties.Settings.Default.campo_template = Properties.Settings.Default.campo_template;
            //Rafis.Properties.Settings.Default.nc_port = Properties.Settings.Default.nc_port;
            //Rafis.Properties.Settings.Default.nc_server = Properties.Settings.Default.nc_server;
            //Rafis.Properties.Settings.Default.nc_seed_port = Properties.Settings.Default.nc_seed_port;
            //Rafis.Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.IP = maskedTextBox1.Text;
            Properties.Settings.Default.Porta = textBox1.Text;
            Properties.Settings.Default.Banco = textBox2.Text;
            Properties.Settings.Default.Login = textBox4.Text;
            Properties.Settings.Default.Senha = maskedTextBox2.Text;
            Properties.Settings.Default.Tabela = textBox3.Text;
            Properties.Settings.Default.campo_cpf = textBox5.Text;
            Properties.Settings.Default.campo_template = textBox6.Text;
            Properties.Settings.Default.nc_port = textBox7.Text;
            Properties.Settings.Default.nc_server = textBox8.Text;
            Properties.Settings.Default.nc_seed_port = textBox9.Text;
            Properties.Settings.Default.Save();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strCheck = "SELECT count(*) FROM information_schema.COLUMNS WHERE table_schema = '" + textBox2.Text + "' AND table_name = '" + textBox3.Text + "' AND COLUMN_NAME='" + textBox5.Text + "' LIMIT 1;";
            MySqlConnection conn = new MySqlConnection("Server=" + maskedTextBox1.Text + ";Port=" + textBox1.Text + ";Database=" + textBox2.Text + ";Uid=" + textBox4.Text + ";Pwd=" + maskedTextBox2.Text);
            MySqlCommand myCommand = new MySqlCommand(strCheck, conn);
            try
            {
                conn.Open();
                string str = myCommand.ExecuteScalar().ToString();
                if ("1" == myCommand.ExecuteScalar().ToString())
                {
                    toolStripStatusLabel1.Text = "Conectado com sucesso!";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Falha na conexão!";
                }

                conn.Close();
            }
            catch (Exception)
            {
                toolStripStatusLabel1.Text = "Servidor não disponível ou dados incorretos!";
              
            } 
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
