using MAD.Ressourcen.Datenstruktur;

namespace MAD.Ressourcen.Schnittstellen
{
	interface IMAD
	{
		//Spiel Initialisierung
		void Add_Spieler(string name);
		void Entf_Spieler();

		//Spiel Aktionen
		void Wuerfeln();
		void Waehle_Figur(Figur f);
		void Bewege_Figur();
	}
}
