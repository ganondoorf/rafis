using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace BasicAfis
{
    class Class1
    {


        public Image Base64ToBitmap(string fileName)
        {

            //inserido base64_decoder - Abrir Imagem codificada base64
            System.IO.StreamReader inFile = new System.IO.StreamReader(fileName, System.Text.Encoding.ASCII);
            char[] base64CharArray = new char[inFile.BaseStream.Length];
            inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
            string base64String = new string(base64CharArray);

            //converter String Base64 para byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            //Cria stream para converter byte[] para Imagem
            MemoryStream ms = new MemoryStream(imageBytes);
            ms.Write(imageBytes, 0, imageBytes.Length);
           
            //Testa se é imagem base 64
            Image returnImage = null;
            Bitmap imagemBitmap = null;
            try
            {
                //converte Image para Bitmap
                returnImage = Image.FromStream(ms);
                imagemBitmap = new Bitmap(returnImage);

            }
            catch (System.Exception excep)
            {

                MessageBox.Show("O arquivo selecionado não contém o formato correto!");

            }
            //retorna Bitmap
            return imagemBitmap;
                
        }



        


    }
}
