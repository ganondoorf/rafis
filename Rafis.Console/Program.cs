using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Rafis.DAL.Repo;
using Rafis.Entidades;
using Rafis.DAO;

namespace Rafis.Console
{
	class MainClass
	{
		static void Main()
		{ 
			System.Console.ReadKey();
			//Adicionar();
			//ExcluirVariosClientes();
			migrate_tp();

		}

		//private static void Adicionar()
		//{
		//	using (var repInd = new IndRepo())
		//	{
		//		new List<Individuo>
		//				{
		//					new Individuo { Nome="Microsoft2", cpfID= 93944343000158,
		//									Pai="Papai", tEleitor=11574739494},
		//					new Individuo { Nome="Oracle2", cpfID= 93944343000159,
		//									Pai="Papai", tEleitor=11574739494},
		//					new Individuo { Nome="IBM2", cpfID= 93944343000160,
		//									Pai="Papai", tEleitor=11574739494}
		//				}.ForEach(repInd.Adicionar);

		//		repInd.SalvarTodos();

		//		System.Console.WriteLine("Clientes adicionadas com sucesso.");

		//		System.Console.WriteLine("=======  clientes cadastrados ===========");
		//		foreach (var c in repInd.GetAll())
		//		{
		//			System.Console.WriteLine("{0} - {1}", c.cpfID, c.Nome);
		//		}

		//		System.Console.ReadKey();
		//	}
		//}

		//private static void ExcluirVariosClientes()
		//{
		//	using (var repTemp = new TempRepo()) 
		//	{
		//		new List<Entidades.Template>
		//		{
		//			new Entidades.Template { ItemID=52685098291, PersonID=2342342342342, Template_bin=null, 
		//						   template_iso=null, Template_xml="", CaminhoImagem=""}

		//		}.ForEach(repTemp.Adicionar);

		//		repTemp.SalvarTodos();
		//		System.Console.WriteLine("Template adicionado");
		//	}

		//	using (var repInd = new IndRepo())
		//	{
		//		new List<Individuo>
		//		{
		//			new Individuo { Nome="Renato Haddad", cpfID= 93944343000161 ,tEleitor=48575757},
		//			new Individuo { Nome="Renato Marcantonio", cpfID= 93944343000162 ,tEleitor=48575757},
		//			new Individuo { Nome="Renato Jose", cpfID= 93944343000163 ,tEleitor=48575757}
		//		}.ForEach(repInd.Adicionar);

		//		repInd.SalvarTodos();
		//		System.Console.WriteLine("Clientes Renato's adicionados");

		//		// lista todos os clientes
		//		foreach (var c in repInd.GetAll())
		//		{
		//			System.Console.WriteLine(c.cpfID + " - " + c.Nome);
		//		}

		//		// excluir vários clientes Renato
		//		try
		//		{
		//			repInd.Excluir(c => c.Nome.StartsWith("Renato", StringComparison.CurrentCulture));
		//			repInd.SalvarTodos();
		//			System.Console.WriteLine("clientes excluidos com sucesso");
		//		}
		//		catch (Exception)
		//		{
		//			System.Console.WriteLine("erro ao excluir um cliente");
		//		}
		//		System.Console.ReadKey();

		//	}

		//}

		private static void migrate_ind()
		{ 
			using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
			{
				con.Open();
				MySqlCommand cmd = new MySqlCommand("select * from prontuario;", con);
				MySqlDataReader item = cmd.ExecuteReader();

				while (item.Read())
				{
					//Recupera dados para o Indivíduo
					string cpf = (string)(item["CPF"]);
					string nome = (string)item["NOME"];
					string pai = (string)item["NOMEPAI"];
					string mae = (string)item["NOMEMAE"];
					string rg = (string)item["RG"];
					string eleitor = (string)item["CPF"];
					string nac = (string)item["NACIONALIDADE"];
					string nat = (string)item["NATURALIDADE"];
					byte sex = ((string)item["SEXO"] == "M") ? (byte)0 : (byte)1;

					//Verifica e configura o formado da data no banco de dados a ser migrado
					DateTime nasc;
					if (!DateTime.TryParse((string)item["DTNASCIMENTO"], out nasc))
			        {
						nasc = DateTime.ParseExact("01/01/1900","dd/MM/yyyy", null);
			        }

					using (var repInd = new IndRepo())
					{
						new List<Individuo>
						{
							new Individuo 
							{
							cpfID = cpf,
							Nome = nome,
							Pai = pai,
							Mae = mae,
							Nasc = nasc,
							Rg = rg,
							tEleitor = eleitor,
							Nac = nac,
							Nat = nat,
							Sex = sex
							}

						}.ForEach(repInd.Adicionar);

						repInd.SalvarTodos();

						System.Console.WriteLine("Indivíduo {0} adicionado com sucesso.", (string)item["CPF"]);
					}

				}
			}
		}


		private static void migrate_tp()
		{
			using (MySqlConnection con = ConnectDB.GetInstancia.GetConnection())
			{
				con.Open();
				MySqlCommand cmd = new MySqlCommand("select * from template;", con);
				MySqlDataReader item = cmd.ExecuteReader();

				while (item.Read())
				{
					//Recupera dados para o Indivíduo
					long itemID = Convert.ToInt64(item["itemID"]);
					long personID = Convert.ToInt64(item["personID"]);
					byte finger = 0;
					byte[] template_bin = (byte[])item["template"];
					string template_xml = (string)item["Template_xml"];
					byte[] isoTemplate = (byte[])item["isoTemplate"];
					string caminhoImagem = (string)item["caminhoImagem"];
					using (var tempRepo = new TempRepo())
					{
						new List<Template_db>
								{
								new Template_db
									{
									ItemID=itemID,
									PersonID=personID,
									Finger=finger,
									Template_bin=template_bin,
									Template_xml=template_xml,
									template_iso=isoTemplate,
									CaminhoImagem=caminhoImagem
									}

								}.ForEach(tempRepo.Adicionar);

						tempRepo.SalvarTodos();

						System.Console.WriteLine("Template {0} adicionado com sucesso.", item["itemID"]);
					}

				}
			}	
		}

	}
}
