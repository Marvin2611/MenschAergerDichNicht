namespace MAD.Ressourcen.Datenstruktur
{
	public class Feld
	{
		public int Position { get; set; }
		public int Besetzt { get; set; }

		public Feld(int position, int besetzt)
		{
			Position = position;
			Besetzt = besetzt;
		}

		public void Unbesetzen()
		{
			Besetzt = -1;
		}
	}
}
