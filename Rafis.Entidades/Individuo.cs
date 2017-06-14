using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rafis.Entidades
{
	[Serializable]
	[Table("Individuo")]
	public class Individuo
	{
		[Key]
		public int indID { get; set;}
		public string cpfID { get; set; }
        public string Nome { get; set; }
        public string Pai { get; set; }
        public string Mae { get; set; }
        public DateTime Nasc { get; set; }
        public string Rg { get; set; }
        public string tEleitor { get; set; }
        public string Nac { get; set; }
        public string Nat { get; set; }
        public byte Sex { get; set; }
    }
    
}
