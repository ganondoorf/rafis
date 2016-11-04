using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using SourceAFIS.Simple;
using RafisDLL;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DAO;
namespace Rafis
{
    public class RafisCore
    {
        #region Classes Serializable, MyPerson e MyFingerprint
        [Serializable]
        class MyFingerprint : Fingerprint
        {
            //public string Filename;
        }

        //Subclasse de Person com campo Name
        [Serializable]
        class MyPerson : Person
        {
            //public string Name;
            public int itemId;
            public int personId;
            public string fileName;
            public string cpf;
        }
        #endregion 

        bool running = true;
        List<MyPerson> database = new List<MyPerson>();
                
        #region Inicia o núcleo Rafis
        public void load()
        {
            AfisEngine Afis = new AfisEngine();
            Afis.Threshold = 5;
            // Base Offline
            List<int> candidates = new List<int>();
            MyPerson probe = new MyPerson();
            Fingerprint fp01 = new Fingerprint();
            
            //Carrega lista de templates do Banco de Dados 
            selectListTemplates();

            Utilities.log("["+DateTime.Now.ToString() + "]"+" Comparando impressões...", "//RafisCore.log");
            while (running)
            {
                try
                {
                    FilaidDAO filaid = new FilaidDAO();
                    DataTable itens = filaid.ConsultarResult();
                    foreach (DataRow item in itens.Rows)
                    {
                        fp01.AsIsoTemplate = (byte[])item["Template"];
                        probe.Fingerprints.Add(fp01);
                        probe.itemId = (int)item["ItemID"];
                        MyPerson match = Afis.Identify(probe, database).FirstOrDefault() as MyPerson;
                        float score = Afis.Verify(probe, match);
                        probe.Fingerprints.Clear();
                        CoMysql.UpdateResultFilaid(probe.itemId.ToString(), score.ToString());

                        if (match == null)
                        {
                            Utilities.log("[" + DateTime.Now.ToString() + "] " + "Sem templates compatíveis.", "//RafisCore.log");
                            continue;
                        }
                        else
                        {
                            Utilities.log("[" + DateTime.Now.ToString() + "] Template compatível encontrado: CPF " + match.cpf + " com score " + score, "//RafisCore.log");
                        }
                    } 
                }
                catch (Exception ex)
                {
                    Utilities.log("[" + DateTime.Now.ToString() + "] " + "RafisCore: Erro ao acessar o banco afis " + ex);
                }

                Utilities.log("[" + DateTime.Now.ToString() + "] " + "Rodando...");
                System.Threading.Thread.Sleep(1000);
            }
            Utilities.log("Encerrando: " + DateTime.Now.ToString());
        }
        public void close()
        {
            running = false;
        }
        #endregion

        #region Conecta no Banco e recupera a base de Templates
        public void selectListTemplates()
        {
            if (true)//!File.Exists("database.dat") conrrigir
            {
                //registra o log com total de templates recuperados.
                Utilities.log("[" + DateTime.Now.ToString() + "] " + "Recuperando " + CoMysql.CountTemplate().ToString() + " templates existentes...", "//RafisCore.log");
                try
                {
                    TemplateDAO templates = new TemplateDAO();
                    List<Template> templateList = new List<Template>();
                    templateList = templates.ListaTodos();
                    foreach (Template item in templateList)
                    {
                        database.Add(Enroll(item.ItemId, item.PersonId, item.TemplateSA, item.CaminhoImagem, item.Cpf));
                        Utilities.log("[" + DateTime.Now.ToString() + "] " + "Recuperando template " + item.ItemId + "...", "//RafisCore.log");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("[" + DateTime.Now.ToString() + "] " + "Erro ao acessar o banco de Templates " + ex.Message);
                }

                BinaryFormatter formatter = new BinaryFormatter();
                Utilities.log("[" + DateTime.Now.ToString() + "] " + "Salvando arquivo stream...", "//RafisCore.log");
                using (Stream stream = File.Open("database.dat", FileMode.Create))
                {
                    formatter.Serialize(stream, database);
                }
                Utilities.log("[" + DateTime.Now.ToString() + "] " + "Stream cache salvo.", "//RafisCore.log");
            }
        }
        #endregion

        #region Enroll templates
        static MyPerson Enroll(int itemId, int personId, Byte[] template, string caminho, string cpf)//Enroll(int itemId,Byte[] template)
        {
            MyFingerprint fp = new MyFingerprint();
            fp.Template = template;
            MyPerson person = new MyPerson();
            person.itemId = itemId;
            person.personId = personId;
            person.fileName = caminho;
            person.cpf = cpf;
            person.Fingerprints.Add(fp);
            
            return person;
        }
        #endregion
    }
}
