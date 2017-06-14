using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SourceAFIS.Simple;
using MySql.Data.MySqlClient;
using Wsq2Bmp;

namespace BasicAfis
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }




        #region Método para inserir os dados de Template -->
        private void inserenodestino(int id, int pid, byte[] template, string caminho, string asXml, byte[] isoTemplate)
        {
            MySqlCommand Commanddestino = new MySqlCommand();
            //MySqlCommand Commanddestino_xml = new MySqlCommand();

            Commanddestino.Connection = Conexaodestino;
            //Commanddestino_xml.Connection = Conexaodestino;

            Commanddestino.CommandText = "insert into template (itemId, personId, Template, CaminhoImagem, Template_xml, isoTemplate) values(@idPar, @pidPar, @templatePar, @caminhoPar, @templateXml, @isoTemplatePar)";
            //Commanddestino_xml.CommandText = "insert into template_xml (ItemId, PersonId, Template_xml) values(@idPar, @pidPar, @templateXml)";

            MySqlParameter idPar = new MySqlParameter("@idPar", id);
            MySqlParameter pidPar = new MySqlParameter("@pidPar", pid);
            MySqlParameter templatePar = new MySqlParameter("@templatePar", MySqlDbType.VarBinary);
            MySqlParameter caminhoPar = new MySqlParameter("@caminhoPar", caminho);
            MySqlParameter templateXml = new MySqlParameter("@templateXml", asXml);
            MySqlParameter iso_Template = new MySqlParameter("@isoTemplatePar", MySqlDbType.VarBinary);

            templatePar.Value = template;
            iso_Template.Value = isoTemplate;

            Commanddestino.Parameters.Add(idPar);
            Commanddestino.Parameters.Add(pidPar);
            Commanddestino.Parameters.Add(templatePar);
            Commanddestino.Parameters.Add(caminhoPar);
            Commanddestino.Parameters.Add(templateXml);
            Commanddestino.Parameters.Add(iso_Template);

            //Commanddestino_xml.Parameters.Add(idPar);
            //Commanddestino_xml.Parameters.Add(pidPar);
            //Commanddestino_xml.Parameters.Add(templateXml);
            
            
            try
            {
                Commanddestino.ExecuteNonQuery();
            }

            catch (Exception eXCP)
            {
                MessageBox.Show(eXCP.Message);
            }

            try
            {
                //Commanddestino_xml.ExecuteNonQuery(); //inserir na tabela de template_xml
            }

            catch (Exception eXCP)
            {
                MessageBox.Show(eXCP.Message);
            }

        }
        #endregion


        #region Converte  Base64 para Imagem -->
        public Image Base64ToImage(string base64String)
        {
            //converter String Base64 para byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);

            //converter byte[] para Imagem
            MemoryStream ms = new MemoryStream(imageBytes);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image returnImage = null;
            //Bitmap imagemBitmap = null;

            try
            {
                returnImage = Image.FromStream(ms);
            }
            catch (Exception)
            {
                try
                {
                    //converte WSQ para BMP;
                    WsqDecoder decoder = new WsqDecoder();
                    Bitmap bmp = decoder.Decode(imageBytes);
                    returnImage = (Image)bmp;
                }
                catch (Exception)
                {
                    return null;
                }


            }

            return returnImage;

        }
        #endregion

        static AfisEngine Afis = new AfisEngine();
        MySqlConnection Conexaoorigem, Conexaodestino;

        private void MoverProgressBar(object sender, EventArgs e)
        {

            #region Abre Banco e configura select origem -->

            //formata string de conexao
            string strcon_origem = "Server=127.0.0.1;Port=3306;Database=iiccindice;Uid=root;Pwd=alg@312";
            string strcon_destino = "Server=127.0.0.1;Port=3306;Database=afis;Uid=root;Pwd=alg@312";

            //configura conexoes
            Conexaoorigem = new MySqlConnection(strcon_origem);
            Conexaodestino = new MySqlConnection(strcon_destino);


            //configura comandos sql
            //string strcomando_origem = "Select item_id, item_data from biometria where item_type ='1'"; //;AND item_id%" + tbdividir.Text + "=" + tbresto.Text;
            
            try
            {
                Conexaoorigem.Open();
                Conexaodestino.Open();
            }

            catch (Exception eXCP)
            {
                MessageBox.Show(eXCP.Message);
            }
            #endregion

            //
            // Mover a progress bar atravéz da alteração do VALUE
            //
            //Marca o diretório a ser listado
            //openFileDialog1.ShowDialog();
            //FileInfo[] Arquivos = openFileDialog1.FileNames;
                      
            DateTime DataHoraAbertura;
            TimeSpan DiferencaDataHora;
            DataHoraAbertura = DateTime.Now;
            timer1.Start();

            DirectoryInfo diretorio = new DirectoryInfo(@"C:\AFIS");
            //Executa função GetFile(Lista os arquivos desejados de acordo com o parametro)
            FileInfo[] Arquivos = diretorio.GetFiles("*.*");

            progressBar1.Value = 0;     // Esse é o valor da progress bar ela vai de 0 a Maximum (padrão 100)
            progressBar1.Maximum = Arquivos.Length; // Esse é o valor Maximo da progress bar, então quando value for = a 1000 ele vai ter carregado 100% (Por parão o maximum = 100)
                      
            //Começamos a listar os arquivos
           
            int count = 0;
            foreach (FileInfo fileinfo in Arquivos)
            {
               
                DiferencaDataHora = (DateTime.Now).Subtract(DataHoraAbertura);
                this.label2.Text = String.Format("Dias: {0}, Horas: {1}, Minutos: {2}, Segundos: {3}, Milisegundos {4}, Ticks: {5}", DiferencaDataHora.Days, DiferencaDataHora.Hours, DiferencaDataHora.Minutes, DiferencaDataHora.Seconds, DiferencaDataHora.Milliseconds, DiferencaDataHora.Ticks);
                progressBar1.Value = count;
                //dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, fileinfo.LastWriteTime });
                label1.Text = "Índice: " + count;
                Image imagem;
                if (fileinfo.FullName.Contains(".jpg")) //se for Jpeg
                {
                    Image image_size;
                    image_size = Image.FromFile(fileinfo.FullName);
                    imagem = (Image)(new Bitmap(image_size, new Size(808, 752)));
                }
                else //caso contrário
                {
                    if (fileinfo.FullName.Contains(".obj")) //se for Jpeg
                    {
                        System.IO.StreamReader inFile = new System.IO.StreamReader(fileinfo.FullName, System.Text.Encoding.ASCII);
                        char[] base64CharArray = new char[inFile.BaseStream.Length];
                        inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                        string base64String = new string(base64CharArray);
                        imagem = Base64ToImage(base64String);
                    }
                    else
                    {
                        imagem = null;
                    }
                }
                if (imagem!=null)
                {
                    pictureBox1.Refresh();
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Height = 300;
                    pictureBox1.Width = 300;
                    pictureBox1.Image = imagem;
                    Fingerprint fp01 = new Fingerprint();
                    // fp01.AsBitmap = new Bitmap(Bitmap.FromFile(img01));
                    fp01.AsBitmap = new Bitmap(imagem);
                    //Fingerprint fp02 = new Fingerprint();
                    // fp02.AsBitmap = new Bitmap(Bitmap.FromFile(img02));
                    Person pessoa01 = new Person();
                    pessoa01.Fingerprints.Add(fp01);
                    Afis.Extract(pessoa01);
                    dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, fp01.AsXmlTemplate.ToString() });
                    
                    //divide nome de arquivo com caracter "_" e depois o "." do .obj
                    String[] index = fileinfo.Name.ToString().Split('_');
                    String[] index2 = index[2].Split('.');

                    int itemid = -1;
                    int personid = -1;
                    int.TryParse(index2[0], out itemid);
                    int.TryParse(index[0], out personid);

                    //insere no banco
                    inserenodestino(itemid,personid, fp01.Template, fileinfo.FullName, fp01.AsXmlTemplate.ToString(), fp01.AsIsoTemplate);



                }
                else
                {
                    dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name,"Formato incorreto" });
                    
                }

                count++;
            
            }

            //label3.Text = DateTime.Now.ToLongTimeString();
             
                   
            
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
