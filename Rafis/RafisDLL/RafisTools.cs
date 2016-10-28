using System;
using System.Linq;
using System.Drawing;
using System.IO;
using Wsq2Bmp;
using MySql.Data.MySqlClient;
using System.Configuration;
using SourceAFIS.Simple;

namespace RafisDLL
{
    public class RafisTools
    {
        //configura conexoes
        MySqlConnection Conexaoorigem, Conexaodestino;
        
        #region Converter Base64 para Image
        /// <summary>
        /// Método para converter arquivo base64 para Image.
        /// </summary>
        public static Image Base64ToImage(string base64String)
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

        #region Converte templates
        private void ConverteTemplates(string dirBase)
        {
            var directory = new DirectoryInfo(dirBase);
            DateTime from_date = DateTime.Parse(loadState());
            DateTime to_date = DateTime.Now;
            var files = directory.GetFiles()
              .Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);


            AfisEngine Afis = new AfisEngine();
            DateTime DataHoraAbertura;
            TimeSpan DiferencaDataHora;
            DataHoraAbertura = DateTime.Now;
            //timer1.Start();
            DirectoryInfo diretorio = new DirectoryInfo(dirBase);
            
            //Executa função GetFile(Lista os arquivos desejados de acordo com o parametro)
            FileInfo[] Arquivos = diretorio.GetFiles("*.*");
            string lastDate = Utilities.lastModDate(diretorio.ToString());
            
            //Começamos a listar os arquivos
            int count = 0;
            foreach (FileInfo fileinfo in Arquivos)
            {
                
                    
                DiferencaDataHora = (DateTime.Now).Subtract(DataHoraAbertura);
                //this.label2.Text = String.Format("Dias: {0}, Horas: {1}, Minutos: {2}, Segundos: {3}, Milisegundos {4}, Ticks: {5}", DiferencaDataHora.Days, DiferencaDataHora.Hours, DiferencaDataHora.Minutes, DiferencaDataHora.Seconds, DiferencaDataHora.Milliseconds, DiferencaDataHora.Ticks);
                //progressBar1.Value = count;
                //dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, fileinfo.LastWriteTime });
                //label1.Text = "Índice: " + count;
                Image imagem;
                if (fileinfo.FullName.Contains(".jpg")) //se for Jpeg
                {
                    //tratar imagem caso seja JPG.
                    Image image_size;
                    image_size = Image.FromFile(fileinfo.FullName);
                    imagem = (Image)(new Bitmap(image_size, new Size(808, 752)));
                }
                else //caso contrário
                {
                    if (fileinfo.FullName.Contains(".obj")) //se for .obj, WSQ armazenado em Base64.
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
                if (imagem != null)
                {
                    Fingerprint fp01 = new Fingerprint();
                    fp01.AsBitmap = new Bitmap(imagem);
                    Person pessoa01 = new Person();
                    pessoa01.Fingerprints.Add(fp01);
                    Afis.Extract(pessoa01);
                    //dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, fp01.AsXmlTemplate.ToString() });

                    //Utiliza o nome do arquivo como metadados, divide nome de arquivo com caracter "_" e depois o "." do .obj
                    String[] index = fileinfo.Name.ToString().Split('_');
                    String[] index2 = index[2].Split('.');
                    int itemid = -1;
                    int personid = -1;
                    int.TryParse(index2[0], out itemid);
                    int.TryParse(index[0], out personid);

                    //insere no banco com o método inserenodestino
                    inserenodestino(itemid, personid, fp01.Template, fileinfo.FullName, fp01.AsXmlTemplate.ToString(), fp01.AsIsoTemplate);
                }
                else
                {
                    //log - dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, "Formato incorreto" });
                }
                count++;

            }
            //label3.Text = DateTime.Now.ToLongTimeString();
        }
        #endregion 

        #region Converte template de uma imagem
        public static Fingerprint ConverteTemplate(string file) {
            AfisEngine Afis = new AfisEngine();
            Image imagem;
            if (file.Contains(".jpg")) //se for Jpeg
            {
                //tratar imagem caso seja JPG.
                Image image_size;
                image_size = Image.FromFile(file);
                imagem = new Bitmap(image_size, new Size(808, 752));
            }
            else //caso contrário
            {
                if (file.Contains(".obj")) //se for .obj, WSQ armazenado em Base64.
                {
                    StreamReader inFile = new StreamReader(file, System.Text.Encoding.ASCII);
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
            if (imagem != null)
            {
                Fingerprint fp01 = new Fingerprint();
                fp01.AsBitmap = new Bitmap(imagem);
                Person pessoa01 = new Person();
                pessoa01.Fingerprints.Add(fp01);
                Afis.Extract(pessoa01);
                //dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, fp01.AsXmlTemplate.ToString() });

                //Utiliza o nome do arquivo como metadados, divide nome de arquivo com caracter "_" e depois o "." do .obj
                return fp01;
            }
            else
            {
                return null;//log - dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, "Formato incorreto" });
            }
           
        
        }
        #endregion

        #region Insere Templates no Banco --ok
        public static void inserenodestino(int id, int pid, byte[] template, string caminho, string asXml, byte[] isoTemplate)
        {
            MySqlConnection Conexaoorigem, Conexaodestino;
            string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
            string ConDestino = ConfigurationManager.ConnectionStrings["ConexaoDestino"].ConnectionString;
            //configura conexoes
            Conexaoorigem = new MySqlConnection(ConOrigem);
            Conexaodestino = new MySqlConnection(ConDestino);
            
            //configura comandos sql
            //string strcomando_origem = "Select item_id, item_data from biometria where item_type ='1'"; //;AND item_id%" + tbdividir.Text + "=" + tbresto.Text;

            try
            {
                Conexaoorigem.Open();
                Conexaodestino.Open();
            }

            catch (Exception)
            {
               
            }

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

            catch (Exception)
            {
                //
            }

            try
            {
                //Commanddestino_xml.ExecuteNonQuery(); //inserir na tabela de template_xml
            }

            catch (Exception)
            {
                //MessageBox.Show(eXCP.Message);
            }

        }
        #endregion
        
        #region Seleciona última entrada no banco e a data. --ok
        public static string loadState()
        {

            try
            {
                string ConOrigem = ConfigurationManager.ConnectionStrings["ConexaoOrigem"].ConnectionString;
                string data_criacao = "";
                MySqlConnection conn = new MySqlConnection(ConOrigem);
                MySqlCommand command = new MySqlCommand("select * from file_state where data_criacao=(select max(data_criacao) from file_state);", conn);
                conn.Open();
                //List<template> listaTemplates = new List<template>()                
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    data_criacao = dr["data_criacao"].ToString();
                }

                dr.Close();
                return data_criacao;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("Erro ao acessar o banco afis " + ex.Message);
            }

        }

        #endregion


    }
}