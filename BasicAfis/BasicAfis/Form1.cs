using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SourceAFIS.Simple;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Linq.Expressions;
using Wsq2Bmp;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MySql.Data.MySqlClient;
using System.Xml.Serialization;


namespace BasicAfis
{
   
    public partial class Form1 : Form
    {

        public Form1()
             {
                 InitializeComponent();
            
             }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        //botões
        private void abrirFonte_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.ShowDialog(); //abre janela para seleção da imagem
            
            label9.Text = openFileDialog1.FileName; //apresenta no form o nome do arquivo
            Image imagem = null;
            
            if (label9.Text.Contains(".jpg")) //se for Jpeg
            {
                Image image_size;
                image_size = Image.FromFile(openFileDialog1.FileName);
                imagem = (Image)(new Bitmap(image_size, new Size(808,752)));

                label12.Text = image_size.VerticalResolution.ToString() + " - " + image_size.HorizontalResolution.ToString();
                label11.Text = image_size.Height.ToString() + " - " + image_size.Width.ToString();
            }
            else //caso contrário
            {
                //inserido base64_decoder - Abrir Imagem codificada base64
                System.IO.StreamReader inFile = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.ASCII);
                char[] base64CharArray = new char[inFile.BaseStream.Length];
                inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                string base64String = new string(base64CharArray);
                imagem = Base64ToImage(base64String); //converte string em imagem
            }
                //label12.Text = imagem.PhysicalDimension.ToString();
            //preenche a picturebox                          
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; 
            pictureBox1.Height = 300;
            pictureBox1.Width = 300;
            pictureBox1.Image = imagem;  
            
        }
        private void abrir1n_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();//abre janela para seleção da imagem

            label10.Text = openFileDialog1.FileName;//apresenta no form o nome do arquivo
            Image imagem = null;


            if (label10.Text.Contains(".jpg")) //se for Jpeg
            {

                Image image_size;
                image_size = Image.FromFile(openFileDialog1.FileName);
                imagem = (Image)(new Bitmap(image_size, new Size(808, 752)));
            }
            else //caso contrário
            {
                //inserido base64_decoder - Abrir Imagem codificada base64
                System.IO.StreamReader inFile = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.ASCII);
                char[] base64CharArray = new char[inFile.BaseStream.Length];
                inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                string base64String = new string(base64CharArray);
                imagem = Base64ToImage(base64String); //converte string em imagem
                
            } 
            
            //fim da modificação
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Height = 300;
            pictureBox2.Width = 300;
            pictureBox2.Image = imagem;
            
        }
        
        //Caminho da pasta local imagens
        static readonly string ImagePath = Path.Combine(Path.Combine("..", ".."), "images");        
        
        #region String de conexão MySQL
        String _conexaoMySQL = "Server=127.0.0.1;Port=3306;Database=afis;Uid=root;Pwd=alg@312";
        String _conexaoMySQL_iicd = "Server=127.0.0.1;Port=3306;Database=iiccindice;Uid=root;Pwd=alg@312";
        #endregion

        #region Classes Serializable -->
        [Serializable]
        class MyFingerprint : Fingerprint
        {
            public string Filename;
        }

        //Subclasse de Person com campo Name
        [Serializable]
        class MyPerson : Person
        {
            public string Name;
            public int itemId;
            public int personId;
            public string fileName;
        }
        #endregion 

        #region convertendo os pontos XML em pontos na imagem -->
        public void marca_minucias(XElement xml1, XElement xml2) 
        {
           

            Graphics GraphicsObject1, GraphicsObject2;
            Image imagem1, imagem2;
            imagem1 = pictureBox1.Image;
            imagem2 = pictureBox2.Image;

            Bitmap bm1 = (Bitmap)imagem1;
            Bitmap imagembmp1 = new Bitmap(bm1);
            Bitmap bm2 = (Bitmap)imagem2;
            Bitmap imagembmp2 = new Bitmap(bm2);

            pictureBox1.Image = imagembmp1;
            pictureBox2.Image = imagembmp2;
            //pictureBox1.Size = imagem1.Size;
            //XElement xml1 = fp01.AsXmlTemplate;
            foreach (XElement x in xml1.Elements())
            {
                GraphicsObject1 = Graphics.FromImage(imagembmp1);
                GraphicsObject1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                int px, py;
                px = Convert.ToInt16(x.Attribute("X").Value);
                py = Convert.ToInt16(x.Attribute("Y").Value);
                if (x.Attribute("Type").Value == "Ending")
                {
                    GraphicsObject1.FillEllipse(Brushes.Yellow, px - 3, 749 - py, 8, 8);
                    GraphicsObject1.Dispose();
                }
                else
                {
                    GraphicsObject1.FillEllipse(Brushes.Red, px - 3, 749 - py, 8, 8);
                    GraphicsObject1.Dispose();
                }

            };

            //XElement xml2 = fp02.AsXmlTemplate;
            foreach (XElement x in xml2.Elements())
            {
                GraphicsObject2 = Graphics.FromImage(imagembmp2);
                GraphicsObject2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                int px, py;
                px = Convert.ToInt16(x.Attribute("X").Value);
                py = Convert.ToInt16(x.Attribute("Y").Value);
                if (x.Attribute("Type").Value == "Ending")
                {
                    GraphicsObject2.FillEllipse(Brushes.Yellow, px - 4, 749 - py, 10, 10);
                    GraphicsObject2.Dispose();
                }
                else
                {
                    GraphicsObject2.FillEllipse(Brushes.Red, px - 4, 749 - py, 10, 10);
                    GraphicsObject2.Dispose();
                }

            };   }
            #endregion   
 
        #region Método para conversão da imagem Base64 -->
        public Image Base64ToImage(string base64String)
        {
            //converter String Base64 para byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            //converter byte[] para Imagem
            MemoryStream ms = new MemoryStream(imageBytes);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image returnImage=null;
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
                   MessageBox.Show("Formato não reconhecido!"); 
                    throw;
                }
                
               
            }

             return returnImage;
                       
         }

        #endregion

        #region Método para imprimir imagem na caixa de imagens 1n -->
        private void print1n(string fileName)
        {
            label10.Text = fileName;//apresenta no form o nome do arquivo
            Image imagem = null;
            if (label10.Text.Contains(".jpg")) //se for Jpeg
            {
                Image image_size;
                image_size = Image.FromFile(fileName);
                imagem = (Image)(new Bitmap(image_size, new Size(808, 752)));
            }
            else //caso contrário
            {
                //inserido base64_decoder - Abrir Imagem codificada base64
                System.IO.StreamReader inFile = new System.IO.StreamReader(fileName, System.Text.Encoding.ASCII);
                char[] base64CharArray = new char[inFile.BaseStream.Length];
                inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                string base64String = new string(base64CharArray);
                imagem = Base64ToImage(base64String); //converte string em imagem
            }
            //fim da modificação
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Height = 300;
            pictureBox2.Width = 300;
            pictureBox2.Image = imagem;

        }
        #endregion

        #region   Retorna Caminho do Arquivo de Imagem

        private string selectImageName(int itemId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {

                    using (MySqlCommand command = new MySqlCommand("select CaminhoImagem from template where ItemId ="+itemId+";", conn))
                    {
                        conn.Open();
                        //List<template> listaTemplates = new List<template>()                

                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            
                            dr.Read();
                            string fileName = (string)dr["CaminhoImagem"];  
                            dr.Close();
                            return fileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar o banco de iiccindice " + ex.Message);
            }

        
        }

        #endregion

        #region Método para imprimir imagem na caixa fotografia -->
        private void printPhoto(string personId)
        {
            var firstFile="0";
            try { firstFile = Path.GetFullPath(Directory.GetFiles(@"C:\AFIS\201206231859", personId + "_ptjff_*").FirstOrDefault()); }
            catch { try { firstFile = Path.GetFullPath(Directory.GetFiles(@"C:\AFIS\201206231902", personId + "_ptjff_*").FirstOrDefault()); } catch { firstFile = "0"; } }

            if (firstFile != "0")
            {
                Image imagem = null;
                if (firstFile.Contains(".jpg")) //se for Jpeg
                {
                    Image image_size;
                    image_size = Image.FromFile(firstFile);
                    imagem = (Image)(new Bitmap(image_size, new Size(808, 752)));
                }
                else //caso contrário
                {
                    //inserido base64_decoder - Abrir Imagem codificada base64
                    System.IO.StreamReader inFile = new System.IO.StreamReader(firstFile, System.Text.Encoding.ASCII);
                    char[] base64CharArray = new char[inFile.BaseStream.Length];
                    inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                    string base64String = new string(base64CharArray);
                    imagem = Base64ToImage(base64String); //converte string em imagem
                }
                //fim da modificação
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox3.Height = 120;
                pictureBox3.Width = 90;
                pictureBox3.Image = imagem;
            }
            else { pictureBox3.Image = null; };

        }
        #endregion

        #region Método conta numero de registro -->
        int GetRowsCount(MySqlCommand command)
        {
            int rowsCount = Convert.ToInt32(command.ExecuteScalar());
            return rowsCount;
        }
        #endregion
            
        #region Enroll no Banco -->
        static MyPerson Enroll(int itemId, int personId, Byte[] template, string caminho)//Enroll(int itemId,Byte[] template)
        {
            //MyPerson Enroll(int itemId, int personId, Byte[] template, string caminho)
            //Console.WriteLine("Enrolling {0}...", name);

            // Initialize empty fingerprint object and set properties
            MyFingerprint fp = new MyFingerprint();
           //fp.Filename = caminho;
            //fp.itemId = itemId;
            
            //fp.personId = personId;
            fp.Template = template;
            
            // Load image from the file
            //Console.WriteLine(" Loading image from {0}...", filename);
            //BitmapImage image = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
            //fp.AsBitmapSource = image;
            // Above update of fp.AsBitmapSource initialized also raw image in fp.Image
            // Check raw image dimensions, Y axis is first, X axis is second
            //Console.WriteLine(" Image size = {0} x {1} (width x height)", fp.Image.GetLength(1), fp.Image.GetLength(0));

            // Initialize empty person object and set its properties
            MyPerson person = new MyPerson();
            person.itemId = itemId;
            person.personId = personId;
            person.fileName=caminho;
            //person.personId = personId;
            // Add fingerprint to the person
            person.Fingerprints.Add(fp);
            // Execute extraction in order to initialize fp.Template
            //Console.WriteLine(" Extracting template...");
            //Afis.Extract(person);
            // Check template size
            //Console.WriteLine(" Template size = {0} bytes", fp.Template.Length);
            return person;
        }
        #endregion

        #region Enroll arquivo local -->

        static MyPerson Enroll_local(Image image, string name)
        {
            Console.WriteLine("Enrolling {0}...", name);

            // Initialize empty fingerprint object and set properties
            MyFingerprint fp = new MyFingerprint();
            MyFingerprint fp_test = new MyFingerprint();
            //fp.Filename = filename;
            // Load image from the file
            //Console.WriteLine(" Loading image from {0}...", filename);
            //BitmapImage image = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
            
            
            
            fp.AsBitmap = new Bitmap(image);

            // Above update of fp.AsBitmapSource initialized also raw image in fp.Image
            // Check raw image dimensions, Y axis is first, X axis is second
            Console.WriteLine(" Image size = {0} x {1} (width x height)", fp.Image.GetLength(1), fp.Image.GetLength(0));

            // Initialize empty person object and set its properties
            MyPerson person = new MyPerson();
            MyPerson test = new MyPerson();

            person.Name = name;
            
            // Add fingerprint to the person
            person.Fingerprints.Add(fp);

            // Execute extraction in order to initialize fp.Template
            Console.WriteLine(" Extracting template...");
            Afis.Extract(person);
            fp_test.AsIsoTemplate = fp.AsIsoTemplate;
            test.Fingerprints.Add(fp_test);

            float xmlScore_test = Afis.Verify(person, test);
   



            // Check template size
            Console.WriteLine(" Template size = {0} bytes", fp.Template.Length);
            
            
            return person;
        }
        
        
        #endregion

        #region Classe de template
        public class template
        {
            int itemId;
            int personId;
            Byte[] templateObj;
            String CaminhoImagem;

            public int ID
            {
                get { return itemId; }
                set { itemId = value; }
            }
            public int PID
            {
                get { return personId; }
                set { personId = value; }
            }
            public Byte[] TemplateOBJ
            {
                get { return templateObj; }
                set { templateObj = value; }
            }
            public String caminhoImagem
            {
                get { return CaminhoImagem; }
                set { CaminhoImagem = value; }
            }
        }

        #endregion

        #region Método Seleciona Dados do Resultado -->
        public void loadResult(List<int> person, List<int> itens, List<float> score) 
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL_iicd))
                {

                    using (MySqlCommand command = new MySqlCommand("select id, nome, rg, cpf from prontuario where id =" + person[0] + " or id=" + person[1] + " or id=" + person[2] + " or id=" + person[3] + " or id=" + person[4] + " or id=" + person[5] + "", conn))
                    {
                        conn.Open();
                        //List<template> listaTemplates = new List<template>()                

                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            dataGridView1.Rows.Clear();
                            int count = 0;
                            while (dr.Read())
                            {
                                if (score[count]>21)
                                {
                                    //dataGridView1.Rows.Insert(count, new Object[] { (int)dr["id"], (String)dr["nome"], (String)dr["cpf"], score[count], itens[count] });
                                    dataGridView1.Rows.Insert(count, new Object[] { (int)dr["id"], "*****", "*****", score[count], itens[count] });
                                    count++;    
                                }
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar o banco de iiccindice " + ex.Message);
            }
        
        }
        #endregion

        #region Método Apresenta Resultados -->
        public void presentResult(int personId,float score)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL_iicd))
                {

                    using (MySqlCommand command = new MySqlCommand("select id, nome, rg, cpf from prontuario where id ="+personId+";", conn))
                    {
                        conn.Open();
                        //List<template> listaTemplates = new List<template>()                

                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            dr.Read();

                            textNome.Text = (String)dr["nome"];
                            textRg.Text = (String)dr["rg"];
                            textCpf.Text = (String)dr["cpf"];
                            printPhoto(personId.ToString());
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar o banco de iiccindice " + ex.Message);
            }

        }
        #endregion

        #region Conecta no Banco e seleciona a base de Templates -->

        public void selectListTemplates()
        {
            if (!File.Exists("database.dat"))
            {
            //MessageBox.Show("Acessando dados de Templates", "Processo Afis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            StatusLabel1.Text = "Acessando dados de Templates no banco...";            
            List<MyPerson> database = new List<MyPerson>();
            int totalRows;

            //Lista o número de templates -->

            using (MySqlConnection conn_row = new MySqlConnection(_conexaoMySQL))
            {
                string sSQL = "SELECT COUNT(*) FROM template";
                MySqlCommand myCommand = new MySqlCommand(sSQL, conn_row);
                conn_row.Open();
                totalRows = Convert.ToInt32(myCommand.ExecuteScalar());
                
                conn_row.Close();
            }
            
            StatusLabel1.Text = "" + totalRows + " templates existentes...";
            this.Refresh();
            //recupera -->
            
            StatusLabel1.Text = "Recuperando " + totalRows + " templates...";
            this.Refresh();
            progressBar1.Value = 0;     // Esse é o valor da progress bar ela vai de 0 a Maximum (padrão 100)
            progressBar1.Maximum = totalRows; // Esse é o valor Maximo da progress bar, então quando value for = a 1000 ele vai ter carregado 100% (Por parão o maximum = 100)
            this.Refresh();
            //MessageBox.Show("Recuperando "+totalRows+" dados de Templates", "Processo Afis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {

                    using (MySqlCommand command = new MySqlCommand("Select * from template;", conn))
                    {
                        conn.Open();
                        //List<template> listaTemplates = new List<template>()                
                        
                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //templateUni.ID = (int)dr["ItemId"];
                                //templateUni.PID = (int)dr["PersonId"];
                                //templateUni.TemplateOBJ = (Byte[])dr["Template"];
                                //templateUni.caminhoImagem = (String)dr["CaminhoImagem"];
                                // Enroll some people
                                int item_id=(int)dr["itemID"];
                                int person_id=(int)dr["personID"];
                          
                                byte[] template_byte=(byte[])dr["template"];
                                String caminho=(String)dr["caminhoImagem"];

                                database.Add(Enroll(item_id,person_id,template_byte,caminho));
                                StatusLabel1.Text = "Listando Template: " + progressBar1.Value + "";
                                progressBar1.Value++;
                                

                            }

                            this.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar o banco de Templates " + ex.Message);
            }

            // Salva os dados da base no disco via serializacao
            BinaryFormatter formatter = new BinaryFormatter();
            StatusLabel1.Text = "Salvando arquivo stream...";
            this.Refresh();
                       
            using (Stream stream = File.Open("database.dat", FileMode.Create))
               formatter.Serialize(stream, database);
         }
        }
        #endregion


        //Botão Compara minucias 
        static AfisEngine Afis = new AfisEngine();
        List<MyPerson> database = new List<MyPerson>();
        private void compararMinucias_Click(object sender, EventArgs e)
        {
            #region Verifica Localmente --->
            if (!useBase.Checked)
            {
            
            //string img01, img02;

            Fingerprint fp01 = new Fingerprint();
           // fp01.AsBitmap = new Bitmap(Bitmap.FromFile(img01));
            fp01.AsBitmap = new Bitmap(pictureBox1.Image);
            
            
            Fingerprint fp02 = new Fingerprint();
            Fingerprint fp03 = new Fingerprint();
           // fp02.AsBitmap = new Bitmap(Bitmap.FromFile(img02));
            fp02.AsBitmap = new Bitmap(pictureBox2.Image);
            
            Person pessoa01 = new Person();
            Person pessoa02 = new Person();
            Person pessoa03 = new Person();

            pessoa01.Fingerprints.Add(fp01);
            pessoa02.Fingerprints.Add(fp02);

            Afis.Extract(pessoa01);
            Afis.Extract(pessoa02);


            textBox1.Text = fp01.AsXmlTemplate.ToString();
            fp03.AsXmlTemplate = fp01.AsXmlTemplate;
            pessoa03.Fingerprints.Add(fp03);

            XmlSerializer serializer = new XmlSerializer(fp01.AsXmlTemplate.GetType());
            XmlSerializer serializer2 = new XmlSerializer(serializer.GetType());
            

            //Afis.Extract(pessoa03);
            
            //método marca_minucias com Template XML    
            XElement xml1 = fp01.AsXmlTemplate;
            XElement xml2 = fp02.AsXmlTemplate;
            marca_minucias(xml1, xml2);

            //verifica via XML
            float xmlScore = Afis.Verify(pessoa01, pessoa03);

            //verifica via binario
            float score = Afis.Verify(pessoa01,pessoa02);
            if(score > 0)
                label7.Text = "Yes";
            else
                label7.Text = "No";
            label8.Text = score.ToString();
            }
            #endregion

            #region Verifica Afis --->

            if (useBase.Checked)
            {
                DateTime DataHoraAbertura;
                TimeSpan DiferencaDataHora;
                DataHoraAbertura = DateTime.Now;

                //Initialize SourceAFIS
                Afis = new AfisEngine();

                // Enroll visitor with unknown identity
               //MyPerson probe = Enroll_local(Path.Combine(ImagePath, "probe.jpg"), "Teste");
                MyPerson probe = Enroll_local(pictureBox1.Image, "Teste");
                // Look up the probe using Threshold = 10
                Afis.Threshold = 5;
                StatusLabel1.Text = "Comparando impressões...";
                progressBar1.Value = 75;
                this.Refresh();

                
                List<int> candidates = new List<int>();
                List<int> itens = new List<int>();

                MyPerson match = Afis.Identify(probe, database).FirstOrDefault() as MyPerson;

                DiferencaDataHora = (DateTime.Now).Subtract(DataHoraAbertura);


                MyPerson match1 = Afis.Identify(probe, database).ElementAt(1) as MyPerson;
                MyPerson match2 = Afis.Identify(probe, database).ElementAtOrDefault(2) as MyPerson;
                MyPerson match3 = Afis.Identify(probe, database).ElementAtOrDefault(3) as MyPerson;
                MyPerson match4 = Afis.Identify(probe, database).ElementAtOrDefault(4) as MyPerson;
                MyPerson match5 = Afis.Identify(probe, database).ElementAtOrDefault(5) as MyPerson;

               

                candidates.Add(match.personId);
                itens.Add(match.itemId);
                candidates.Add(match1.personId);
                itens.Add(match1.itemId);
                candidates.Add(match2.personId);
                itens.Add(match2.itemId);
                candidates.Add(match3.personId);
                itens.Add(match3.itemId);
                candidates.Add(match4.personId);
                itens.Add(match4.itemId);
                candidates.Add(match5.personId);
                itens.Add(match5.itemId);

                //candidates = Afis.Identify(probe,database).
                // Null result means that there is no candidate with similarity score above threshold
                if (match == null)
                {
                    StatusLabel1.Text = "Sem impressão compatível!";
                    MessageBox.Show("Sem impressão compatível", "Afis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Console.WriteLine("No matching person found.");
                    return;
                }
                
                // Print out any non-null result
                print1n(match.fileName);
                StatusLabel1.Text = "Impressão compatível encontrada! Tempo: "+DiferencaDataHora.Milliseconds.ToString() + " ms";
                           

                // Compute similarity score
                List<float> score = new List<float>();
                score.Add(Afis.Verify(probe, match));
                score.Add(Afis.Verify(probe, match1));
                score.Add(Afis.Verify(probe, match2));
                score.Add(Afis.Verify(probe, match3));
                score.Add(Afis.Verify(probe, match4));
                score.Add(Afis.Verify(probe, match5));


                //MessageBox.Show("Correspondência com:" + match.personId + ", impressão " + match.itemId + "Score:" + score, "Afis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                label8.Text = score[0].ToString();
                if (score[0] > 0)
                    label7.Text = "Yes";
                else
                    label7.Text = "No";

                //seleciona dados do Id correspondente
                loadResult(candidates, itens, score);
                presentResult(match.personId, score[0]);
                printPhoto(match.personId.ToString());
                
               #region Gambiarra para marcar minucias - Mudar futuramente -->
                //método marca_minucias com Template XML
                Fingerprint fp01 = new Fingerprint();
                // fp01.AsBitmap = new Bitmap(Bitmap.FromFile(img01));
                fp01.AsBitmap = new Bitmap(pictureBox1.Image);


                Fingerprint fp02 = new Fingerprint();
                // fp02.AsBitmap = new Bitmap(Bitmap.FromFile(img02));
                fp02.AsBitmap = new Bitmap(pictureBox2.Image);

                Person pessoa01 = new Person();
                Person pessoa02 = new Person();

                pessoa01.Fingerprints.Add(fp01);
                pessoa02.Fingerprints.Add(fp02);

                AfisEngine Afis2 = new AfisEngine();
                Afis2.Extract(pessoa01);
                Afis2.Extract(pessoa02);

                XElement xml1 = fp01.AsXmlTemplate;
                XElement xml2 = fp02.AsXmlTemplate;
                marca_minucias(xml1, xml2);
                #endregion

                System.Media.SystemSounds.Beep.Play();
                progressBar1.Value = 100;
                
                //Console.WriteLine("Probe {0} matches registered person {1}", probe.Name, match.itemId);
                //Console.WriteLine("Similarity score between {0} and {1} = {2:F3}", probe.Name, match.itemId, score);
                //Console.Read();
                
            }

            #endregion

        }
        
        //Botão Form de conversão
        private void button4_Click(object sender, EventArgs e)
        {
           Form2 novo_form = new Form2();
           novo_form.ShowDialog();
        }

        #region RadioButtons - Seleção do método de comparação
        private void useBase_CheckedChanged(object sender, EventArgs e)
        {
            if (useBase.Checked) abrir1n.Enabled = false;
        }

        private void use1N_CheckedChanged(object sender, EventArgs e)
        {
            if (use1N.Checked) abrir1n.Enabled = true; 
        }

        #endregion

        private void saveXml_Click(object sender, EventArgs e)
        {

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Template XML|*.xml";
            saveFileDialog1.Title = "Salva Template XML";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {

                File.WriteAllText(saveFileDialog1.FileName, textBox1.Text.ToString());
            }
        }

        private void consultarBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectListTemplates();
            StatusLabel1.Text = "Stream salvo no disco.";
        }

        private void consultarStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //database.Add(Enroll_local(Path.Combine(ImagePath, "candidate1.tif"), "Fred Flintstone"));

            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
            BinaryFormatter formatter = new BinaryFormatter();
            StatusLabel1.Text = "Recuperando arquivo stream...";
            this.Refresh();

            FileStream stream = File.OpenRead("database.dat");
            progressBar1.Value = 50;
            database = (List<MyPerson>)formatter.Deserialize(stream);

            progressBar1.Value = 100;
            StatusLabel1.Text = "Carregado.";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(dataGridView1.SelectedRows.ToString(), "Afis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           
            var rowIndex = e.RowIndex;
            if (rowIndex>=0)
            {
                
                var currentRow = dataGridView1.Rows[rowIndex];
                var currentRowPid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                var currentRowIid = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
                presentResult(Convert.ToInt32(currentRowPid), 0);
                printPhoto(currentRowPid);
                string localImagem = selectImageName(Convert.ToInt32(currentRowIid));
                print1n(localImagem);

            }
            //var currentCellValue = dataGridView1.Rows[rowIndex].Cells[e.ColumnIndex].Value.ToString();
          
            //textNome.Text = currentCellValue;
          
        }
        
        
        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            int xCoordinate = e.X;
            int yCoordinate = e.Y;
            label_coord.Text = Convert.ToString(xCoordinate)+" x "+Convert.ToString(yCoordinate);

            pictureBox1.Cursor = Cursors.Cross;
            toolTip1.Show(Convert.ToString(xCoordinate) + " x " + Convert.ToString(yCoordinate), pictureBox1, e.X+10, e.Y+10, 1000);
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //ZoomPanApp.ZoomPanApp novo_form = new ZoomPanApp.ZoomPanApp(label9.Text);
            //novo_form.ShowDialog();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            //Form3 novo_form = new Form3(label10.Text);
            //novo_form.ShowDialog();
        }
            
        }
        
                
    }

