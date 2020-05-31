using System;
using System.Collections.Generic;

namespace MAD.Ressourcen.Datenstruktur
{
	public class Einstellung
	{
		public List<String> name { get; set; }
		public List<int> reihenfolge { get; set; }

		public Einstellung()
		{
			name = new List<string>();
			reihenfolge = new List<int>();
		}
	}
}
