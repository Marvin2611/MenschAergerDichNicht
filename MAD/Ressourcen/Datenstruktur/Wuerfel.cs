using System;

namespace MAD.Ressourcen.Datenstruktur
{
	public class Wuerfel
	{
		public int Wuerfeln()
		{
			Random Rnd = new Random();
			return Rnd.Next(1, 7);
		}

		public int Wuerfeln_Manipuliert(int wert)
		{
			return wert;
		}
	}
}
