using System.Collections.Generic;

namespace MAD.Ressourcen.Datenstruktur
{
	public class Brett
	{
		public List<Bereich> Haus { get; set; }
		public List<Feld> Weg { get; set; }
		public List<Bereich> Ziel { get; set; }

		public Brett()
		{
			Haus = new List<Bereich>();
			Weg = new List<Feld>();
			Ziel = new List<Bereich>();
		}
	}
}
