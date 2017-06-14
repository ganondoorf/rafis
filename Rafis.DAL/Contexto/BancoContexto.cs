using System.Data.Entity;
using Rafis.Entidades;

namespace Rafis.DAL.Contexto
{
    class BancoContexto : DbContext
    {
        public BancoContexto() : base("ConnDB") { }

		//cria a tabela no banco
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//A criação da base de dados deve ser feita a partir do código, de uma vez, todas as tabelas.
			modelBuilder.Entity<Individuo>().ToTable("Individuo");
			modelBuilder.Entity<Template_db>().ToTable("Templates");
		}
    }

}
