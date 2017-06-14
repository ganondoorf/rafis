using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Rafis.DAO;
using Rafis.Entidades;

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
                imagem = Utils.Base64ToImage(base64String); //converte string em imagem
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
                imagem = Utils.Base64ToImage(base64String); //converte string em imagem
                
            } 
            
            //fim da modificação
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Height = 300;
            pictureBox2.Width = 300;
            pictureBox2.Image = imagem;
            
        }
        
        //Caminho da pasta local imagens
        static readonly string ImagePath = Path.Combine(Path.Combine("..", ".."), "images");        

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

            }

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

            }   
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
                imagem = Utils.Base64ToImage(base64String); //converte string em imagem
            }
            //fim da modificação
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Height = 300;
            pictureBox2.Width = 300;
            pictureBox2.Image = imagem;

        }
        #endregion

        #region Método para imprimir imagem na caixa fotografia -->
        private void printPhoto(string person)
        {
            var firstFile="0";
            try { firstFile = Path.GetFullPath(Directory.GetFiles(@"C:\AFIS\201206231859", person + "_ptjff_*").FirstOrDefault()); }
            catch { try { firstFile = Path.GetFullPath(Directory.GetFiles(@"C:\AFIS\201206231902", person + "_ptjff_*").FirstOrDefault()); } catch { firstFile = "0"; } }

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
                    imagem = Utils.Base64ToImage(base64String); //converte string em imagem
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

        //Botão Compara minucias 
        
        List<MyPerson> database = new List<MyPerson>();
        private void compararMinucias_Click(object sender, EventArgs e)
        {
			#region Verifica ou identifica

			if (useBase.Checked)
            {
                
            }
			else
			{	

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

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }
            
     }
        
                
 }

