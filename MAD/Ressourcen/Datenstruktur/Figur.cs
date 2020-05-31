namespace MAD.Ressourcen.Datenstruktur
{
	public class Figur
	{
		public Feld Aktuell;
		public int Route_Pos { get; set; }
		public Figur()
		{
			Route_Pos = -1;
		}
	}
}
