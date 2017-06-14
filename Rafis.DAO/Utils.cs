using System;
using System.IO;
using System.Drawing;
using Wsq2Bmp;
using Rafis.Entidades;
using SourceAFIS.Simple;
using System.Collections.Generic;

namespace Rafis.DAO
{
	public class Utils
	{
		#region Converte Imagem de Base64 para Bmp 
		public static Image Base64ToImage(string base64String)
		{
			//converter String Base64 para byte[]
			byte[] imageBytes = Convert.FromBase64String(base64String);
			//converter byte[] para Imagem
			MemoryStream ms = new MemoryStream(imageBytes);
			ms.Write(imageBytes, 0, imageBytes.Length);
			Image returnImage = null;

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
				catch (Exception ex)
				{
					throw new System.InvalidOperationException("Erro: {0}", ex);
				}
			}

			return returnImage;
		}
		#endregion

		#region Registra pessoa e suas digitais
		public static MyPerson Enroll(int itemId, int personId, List<Template> template)//Enroll(int itemId,Byte[] template)
		{
			MyPerson person = new MyPerson();

			person.itemId = itemId;
			person.personId = personId;

			foreach (var item in template)
			{
				MyFingerprint fp = new MyFingerprint();
				fp.Template = item.TemplateSA;
				person.Fingerprints.Add(fp);
			}
			return person;
		}
		#endregion

		#region Enroll imagem local
		public static MyPerson Enroll_local(Image image, string name)
		{
			AfisEngine Afis = new AfisEngine();
			Console.WriteLine("Enrolling {0}...", name);

			MyFingerprint fp = new MyFingerprint();
			fp.AsBitmap = new Bitmap(image);

			Console.WriteLine(" Image size = {0} x {1} (width x height)", fp.Image.GetLength(1), fp.Image.GetLength(0));

			MyPerson person = new MyPerson();

			person.Name = name;
			person.Fingerprints.Add(fp);

			Console.WriteLine(" Extracting template...");
			Afis.Extract(person);
			Console.WriteLine(" Template size = {0} bytes", fp.Template.Length);
			return person;
		}
		#endregion

		#region Verificação
		public static void verifica()
		{
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

			//método marca_minucias com Template XML    
			XElement xml1 = fp01.AsXmlTemplate;
			XElement xml2 = fp02.AsXmlTemplate;

			marca_minucias(xml1, xml2);

			//verifica via XML
			float xmlScore = Afis.Verify(pessoa01, pessoa03);

			//verifica via binario
			float score = Afis.Verify(pessoa01, pessoa02);
			if (score > 0)
				label7.Text = "Yes";
			else
				label7.Text = "No";
			label8.Text = score.ToString();
		}
		#endregion

		#region Indetificação
		public static void identifica()
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

									int item_id = (int)dr["itemID"];
									int person_id = (int)dr["personID"];

									byte[] template_byte = (byte[])dr["template"];
									String caminho = (String)dr["caminhoImagem"];

									database.Add(Enroll(item_id, person_id, template_byte, caminho));
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
								if (score[count] > 21)
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
		public void presentResult(int personId, float score)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL_iicd))
				{

					using (MySqlCommand command = new MySqlCommand("select id, nome, rg, cpf from prontuario where id =" + personId + ";", conn))
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
	}
}
