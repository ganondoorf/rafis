using System;
using System.Linq;
using System.Drawing;
using System.IO;
using Wsq2Bmp;
using SourceAFIS.Simple;
using DAO;

namespace RafisDLL
{
    public class RafisTools
    {
        #region Converter Base64 para Image
        /// <summary>
        /// Método para converter arquivo base64 para Image.
        /// </summary>
        /// 
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
            DateTime from_date = DateTime.Parse(CoMysql.loadState());
            DateTime to_date = DateTime.Now;
            var files = directory.GetFiles().Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
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
                Image imagem;
                if (fileinfo.FullName.Contains(".jpg")) //se for Jpeg
                {
                    //tratar imagem caso seja JPG.
                    Image image_size;
                    image_size = Image.FromFile(fileinfo.FullName);
                    imagem = new Bitmap(image_size, new Size(808, 752));
                }
                else //caso contrário
                {
                    if (fileinfo.FullName.Contains(".obj")) //se for .obj, WSQ armazenado em Base64.
                    {
                        StreamReader inFile = new StreamReader(fileinfo.FullName, System.Text.Encoding.ASCII);
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

                    //Utiliza o nome do arquivo como metadados, divide nome de arquivo com caracter "_" e depois o "." do .obj
                    string[] index = fileinfo.Name.ToString().Split('_');
                    string[] index2 = index[2].Split('.');
                    int itemid = -1;
                    int personid = -1;
                    int.TryParse(index2[0], out itemid);
                    int.TryParse(index[0], out personid);

                    //insere no banco com o método inserenodestino
                    CoMysql.inserenodestino(itemid, personid, fp01.Template, fileinfo.FullName, fp01.AsXmlTemplate.ToString(), fp01.AsIsoTemplate);
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
        public static Fingerprint ConverteTemplate(string file)
        {
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
                //Utiliza o nome do arquivo como metadados, divide nome de arquivo com caracter "_" e depois o "." do .obj
                return fp01;
            }
            else
            {
                //log - dataGridView1.Rows.Insert(count, new Object[] { count, fileinfo.Name, "Formato incorreto" });
                return null;
            }
        }
        #endregion
    }
}