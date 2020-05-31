using MAD.Ressourcen.Datenstruktur;

namespace MAD.Ressourcen.Spiellogik
{
	public class MADKI
	{
		public Spieler Modell;
		public Feld pruefe;
		public int wahl_zaehler;

		public MADKI(Spieler modell)
		{
			Modell = modell;
			pruefe = new Feld(-1,-1);
			wahl_zaehler = 0;
		}

		public void Computer_Zug(Controller c)
		{
			Wuerfeln(c);

			do {
				Waehle_Figur(c);
				pruefe = Modell.Figuren[c.Gewaehlt].Aktuell;
				Bewege_Figur(c);
			} while (Modell.Figuren[c.Gewaehlt].Aktuell == pruefe);
		}

		private void Wuerfeln(Controller c)
		{
			c.Wuerfeln();
		}
		private void Waehle_Figur(Controller c)
		{
			c.Waehle_Figur(Modell.Figuren[wahl_zaehler]);
			wahl_zaehler++;

			if (wahl_zaehler >= 4)
				wahl_zaehler = 0;
		}
		private void Bewege_Figur(Controller c)
		{
			c.Bewege_Figur();
		}
	}
}
