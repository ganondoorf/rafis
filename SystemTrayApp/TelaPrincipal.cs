﻿using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SourceAFIS.Simple;
using DAO;
using System.Collections.Generic;

namespace SystemTrayApp
{
    public partial class TelaPrincipal : Form
    {
        string dedo = "";
        string ip = Properties.Settings.Default.IP;
        string porta = Properties.Settings.Default.Porta.ToString();
        string banco = Properties.Settings.Default.Banco;
        string login = Properties.Settings.Default.Login;
        string senha = Properties.Settings.Default.Senha;
        string tabela = Properties.Settings.Default.Tabela;
        string campo_cpf = Properties.Settings.Default.campo_cpf;
        string campo_template = Properties.Settings.Default.campo_template;

        public TelaPrincipal()
        {
            InitializeComponent();
        }

        private void TelaPrincipal_Load(object sender, EventArgs e)
        {
            refresh();
        }

        #region define comportamento dos componentes visuais...
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".jpg";
            openFileDialog1.Title = "Selecione a imagem a ser enviada.";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog(); //abre janela para seleção da imagem
            textBox1.Text = openFileDialog1.FileName;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dedo = "";
            maskedTextBox1.Enabled = false;
            comboBox1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dedo = "";
            maskedTextBox1.Enabled = true;
            comboBox1.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void conectarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Configuracao().ShowDialog();
        }
        #endregion

        #region Define comportamento botões de seleção do dedo.

        private void button3_Click(object sender, EventArgs e)
        {
            dedo = "ll";
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dedo = "lr";
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dedo = "lm";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dedo = "li";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dedo = "lt";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dedo = "ri";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = false;
            button12.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dedo = "rm";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = false;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dedo = "rr";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = false;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dedo = "rl";
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = false;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            dedo = "rt"; 
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = false;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
        }
        
    #endregion

        #region Carrega DataGridViews
        public void loadGridResult()
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();

                TemplateDAO templates = new TemplateDAO();
                List<Template> templateList = new List<Template>();
                templateList = templates.ListaSendTemplate();
                int count = 0;
                foreach (Template item in templateList)
                {
                    dataGridView1.Rows.Insert(count, new Object[] { item.ItemId, item.Operacao, item.Cpf, item.Id_dedo.ToString(), item.IsoTemplate.ToString(), item.No_destino, item.Resultado, item.Score });
                    count++;
                }

                count = 0;
                templateList = templates.ListaProcTemplate();
                foreach (Template item in templateList)
                {
                    dataGridView2.Rows.Insert(count, new Object[] { item.Ordem, item.ItemId, item.IsoTemplate.ToString(), item.Cpf, item.Resultado, item.Score, item.No_origem, item.Custo });
                    count++;
                }
                templateList = null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro no carregamento dos Datagrids: " + e);
                throw;
            }
        }
        #endregion

        private void SelectPath_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="")
            {
                Fingerprint fp01 = RafisDLL.RafisTools.ConverteTemplate(textBox1.Text);
                maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                RafisDLL.TemplateShare templateObj = new RafisDLL.TemplateShare();
                templateObj.cpf = maskedTextBox1.Text;
                templateObj.id_dedo = dedo;
                templateObj.no_destino = "ND";
                templateObj.no_origem = Properties.Settings.Default.Estado;
                templateObj.operacao = 0;
                if (radioButton1.Checked&&!radioButton2.Checked)
	            {
                    templateObj.operacao = 1;
	            }      
                templateObj.template = fp01.Template;
                templateObj.template_iso = fp01.AsIsoTemplate;
                RafisDLL.TemplateClient cliente = new RafisDLL.TemplateClient();
                try
                {
                    //cliente.SendToServer(templateObj, "srv-ro.politec.ro.gov.br", 777);
                }
                catch (Exception g)
                {
                    MessageBox.Show("Erro no envio do template: "+g);
                    throw;
                }
                
                try
                {
                CoMysql.insereTransfer(maskedTextBox1.Text, dedo, fp01.Template, fp01.AsIsoTemplate, fp01.AsXmlTemplate.ToString(), templateObj.no_destino, templateObj.operacao);
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.ToString());
                }
                maskedTextBox1.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            }
            else
            {
                MessageBox.Show("Selecione um arquivo para ser enviado.");
            }
        }

        #region Refresh na Tela
        private void refresh()
        {
            try
            {
                if (RafisDLL.Utilities.isRafisRunning())
                {
                    toolStripStatusLabel1.Text = "Serviço Rafis ativo.";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Serviço Rafis parado.";
                }
            }
            catch (Exception)
            {
                toolStripStatusLabel1.Text = "Serviço Rafis não instalado.";
            }
            try
            {
                loadGridResult();
            }
            catch (Exception)
            {   
                throw;
            }
            comboBox1.DataSource = new[] { "Todos", "MT.rafis.net", "RJ.rafis.net", "RO.rafis.net", "RR.rafis.net" };
        }
        #endregion
    }
}
