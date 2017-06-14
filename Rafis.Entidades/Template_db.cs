using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rafis.Entidades
{
	[Table("Templates")]
	public class Template_db
	{
		[Key]
		public long ItemID { get; set; }
		public long PersonID { get; set; }
		public byte Finger { get; set;}
		public byte[] Template_bin { get; set; }
		public string Template_xml { get; set; }
		public byte[] template_iso { get; set; }
		public string CaminhoImagem { get; set; }

	}

}
