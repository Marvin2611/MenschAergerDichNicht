using System.Collections.ObjectModel;

namespace MAD.Ressourcen.Datenstruktur
{
	public class Spieler
	{
		public string Name { get; set; }
		public ObservableCollection<Figur> Figuren = new ObservableCollection<Figur>();
		public Pfad Route = new Pfad();

		public Spieler(string name, int figuren)
		{
			Name = name;

			for(int i = 0; i < figuren; i++)
			{
				Figuren.Add(new Figur());
			}
		}
	}
}
