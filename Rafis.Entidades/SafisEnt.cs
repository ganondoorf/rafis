using System;
using SourceAFIS.Simple;

namespace Rafis.Entidades           
{
		#region Classes Serializable -->
		[Serializable]
		public class MyFingerprint : Fingerprint
		{
		    //atributos adicionais podem ser inseridos aqui
		}

		//Subclasse de Person com campo Name
		[Serializable]
		public class MyPerson : Person
		{
		    public string Name;
		    public int itemId;
		    public int personId;
		    public string fileName;
			public float score;
		}
		#endregion 

}
