using System;

namespace MAD.Ressourcen.Datenstruktur
{
	public class BaueBrett
	{
		public static Brett Spielbrett(int spielmodus)
		{
			if (spielmodus == 0)
			{
				Brett Standart = new Brett();

				for (int i = 0; i < 40; i++)
				{
					Standart.Weg.Add(new Feld(i, -1));
				}

				for (int i = 0; i < 4; i++)
				{
					Bereich h = new Bereich();
					Bereich z = new Bereich();
					for (int j = 0; j < 4; j++)
					{
						h.F.Add(new Feld(j, -1));
						z.F.Add(new Feld(j, -1));
					}
					Standart.Haus.Add(h);
					Standart.Ziel.Add(z);
				}
				return Standart;
			}

			throw new ArgumentException("Kein Brett ausgewaehlt!");
		}
	}
}
