using MAD.Ressourcen.Datenstruktur;
using MAD.Ressourcen.Schnittstellen;
using System.Collections.Generic;
using System.ComponentModel;

namespace MAD.Ressourcen.Spiellogik
{
	public class Controller : INotifyPropertyChanged, IMAD
	{
		public event PropertyChangedEventHandler PropertyChanged; 

		//Datenstruktur
		public Brett b;
		public Wuerfel w;
		public List<Spieler> p;

		//Sichtbar außerhalb der Klasse
		private int am_zug;
		private int wurf;
		private int gewaehlt;
		public int Am_Zug
		{
			get { return am_zug; }
			private set
			{
				am_zug = value;
				OnPropertyChanged("Am_Zug");
			}
		}		//Der aktuelle Spieler (0 - 3)
		public int Wurf
		{
			get { return wurf; }
			private set
			{
				wurf = value;
				OnPropertyChanged("Wurf");
			}
		}			//Der gewuerfelte Wert   (1 - 6 | 0 wenn zurueckgesetzt oder unmoeglich)
		public int Gewaehlt
		{
			get { return gewaehlt; }
			private set
			{
				gewaehlt = value;
				OnPropertyChanged("Gewaehlt");
			}
		}	    //Die ausgewaehlte Figur (0 - 3 | -1 wenn zurueckgesetzt oder unmoeglich)

		public Figur ZuKicken;		//Die Figur die im aktuellen Zug gekickt wird
		public Figur ZuBewegen;		//Die Figur die im aktuellen Zug bewegt wird

		//Notwendiges um einen Zug zu strukturieren
		private int rest_wuerfe { get; set; }				//Die Wuerfe die der Spieler noch machen kann
		private bool zug_moeglich { get; set; }				//True, wenn Zug NACH dem wuerfeln moeglich
		private bool aus_haus_gesetzt { get; set; }			//True, wenn Figur gerade aus dem Haus
		public bool Spiel_zuende { get; private set; }		//True, wenn alle Figuren eines Spielers im Haus
		public string Gewinner { get; private set; }		//Gefuehlt, wenn Gewinner feststeht

		public Controller()
		{
			b = new Brett();
			b = BaueBrett.Spielbrett(0);
			w = new Wuerfel();
			p = new List<Spieler>();

			Am_Zug = 0;
			Wurf = 0;
			Gewaehlt = -1;

			ZuKicken = null;
			ZuBewegen = null;

			rest_wuerfe = 3;
			zug_moeglich = false;
			aus_haus_gesetzt = false;
			Spiel_zuende = false;
		}

		#region INotifyPropertyChanged Schnittstelle
		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, e);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region Spiel Initialisierung
		public void Add_Spieler(string name)
		{
			p.Add(new Spieler(name, 4));

			int spieler = p.Count - 1;

			//Initialisiere die Figuren vom Spieler
			for (int i = 0; i < 4; i++)
			{
				p[spieler].Figuren[i].Aktuell = b.Haus[spieler].F[i];
				p[spieler].Figuren[i].Aktuell.Besetzt = spieler;
			}
			
			Setze_Pfad(spieler);
		}

		//Setze die Route zum Ziel vom Startfeld aus
		private void Setze_Pfad(int spieler)
		{
			for (int i = 0; i < 44; i++)
			{
				if(i > 39)
				{
					p[spieler].Route.F.Add(b.Ziel[spieler].F[i - 40]);
				}

				if(spieler == 0 && i <= 39)
				{
					p[spieler].Route.F.Add(b.Weg[i]);
				}

				if(spieler == 1 && i <= 39)
				{
					if(i > 29)
						p[spieler].Route.F.Add(b.Weg[i - 30]);
					else
						p[spieler].Route.F.Add(b.Weg[i + 10]);
				}

				if(spieler == 2 && i <= 39)
				{
					if(i > 19)
						p[spieler].Route.F.Add(b.Weg[i - 20]);
					else
						p[spieler].Route.F.Add(b.Weg[i + 20]);
				}

				if(spieler == 3 && i <= 39)
				{
					if(i > 9)
						p[spieler].Route.F.Add(b.Weg[i - 10]);
					else
						p[spieler].Route.F.Add(b.Weg[i + 30]);
				}
			}
		}

		public void Entf_Spieler()
		{
			if(p.Count != 0)
			{
				int z = p.Count - 1;

				p.RemoveAt(z);
				for (int i = 0; i < 4; i++)
				{
					b.Haus[z].F[i].Besetzt = -1;
				}
			}
		}
		#endregion

		#region Spieler Aktionen
		public void Wuerfeln()
		{
			if (Spiel_zuende)
				return;

			if(rest_wuerfe > 0)
			{
				if (!zug_moeglich || aus_haus_gesetzt)
				{
					Wurf = w.Wuerfeln();
					rest_wuerfe--;

					zug_moeglich = Zug_Moeglich();

					if (!zug_moeglich && rest_wuerfe == 0)
						Naechster_Zug();
				}
			}
		}

		public void Waehle_Figur(Figur f)
		{
			Gewaehlt = -1;
			for (int i = 0; i < 4; i++)
			{
				if (f == p[Am_Zug].Figuren[i])
				{
					Gewaehlt = i;
					ZuBewegen = p[Am_Zug].Figuren[i];
				}
			}
		}
		public void Waehle_Figur(int figur)
		{
			Gewaehlt = figur;
		}

		public void Bewege_Figur()
		{
			//Ist keine Figur ausgewaehlt oder das Spiel zuende springe raus
			if (Gewaehlt == -1 || Spiel_zuende || Wurf == 0)
				return;

			//Ist Zielfeld == -1, dann ist die Bewegung nicht moeglich
			int zielfeld = Setze_Zielfeld(Gewaehlt);
			if (zielfeld == -1)
				return;

			//Ist Zielfeld == 0, dann wird aus dem Haus gesetzt
			if (zielfeld == 0)
			{
				if(p[Am_Zug].Route.F[0].Besetzt != -1)
					Kicke_Figur(p[Am_Zug].Route.F[0]);
				Setze_Aus_Haus();

				//WICHTIG: Da man die Figur nach dem raussetzen noch einmal
				//		   bewegen darf wird der Status "nur" resettet
				//		   und nicht der naechste Zug gestartet
				Reset_Status();

			}	//Wenn die Figur nicht im Haus ist
			else if (p[Am_Zug].Figuren[Gewaehlt].Route_Pos != -1)
			{
				if (p[Am_Zug].Route.F[zielfeld].Besetzt != -1)
					Kicke_Figur(p[Am_Zug].Route.F[zielfeld]);
				Einfache_Bewegung(zielfeld);

				if (Gewonnen())
				{
					Beende_Spiel();
				}
				else
				{
					Naechster_Zug();
				}
			}
		}
		#endregion

		#region Bewegungsfunktionen
		private void Einfache_Bewegung(int zielfeld)
		{
			p[Am_Zug].Figuren[Gewaehlt].Aktuell.Unbesetzen();					//Momentanige Position
			p[Am_Zug].Figuren[Gewaehlt].Aktuell = p[Am_Zug].Route.F[zielfeld];	//Neue Position
			p[Am_Zug].Figuren[Gewaehlt].Aktuell.Besetzt = Am_Zug;				//Feld besetzen vom Spieler
			p[Am_Zug].Figuren[Gewaehlt].Route_Pos = zielfeld;					//Routenposition erhoehen
		}

		private void Kicke_Figur(Feld feld)
		{
			int figur = Gib_Figur(feld);

			if (figur == -1)
				return;

			ZuKicken = p[feld.Besetzt].Figuren[figur];

			Setze_Ins_Haus(feld.Besetzt, figur);
		} 

		private void Setze_Ins_Haus(int spieler, int figur)
		{
			if (p[spieler].Figuren[figur].Route_Pos != -1)
			{
				p[spieler].Figuren[figur].Aktuell.Unbesetzen();
				p[spieler].Figuren[figur].Aktuell = b.Haus[spieler].F[figur];
				p[spieler].Figuren[figur].Aktuell.Besetzt = spieler;
				p[spieler].Figuren[figur].Route_Pos = -1;
			}
		}

		private void Setze_Aus_Haus()
		{
			if (p[Am_Zug].Route.F[0].Besetzt != Am_Zug)
			{
				p[Am_Zug].Figuren[Gewaehlt].Aktuell.Unbesetzen();
				p[Am_Zug].Figuren[Gewaehlt].Aktuell = p[Am_Zug].Route.F[0];
				p[Am_Zug].Figuren[Gewaehlt].Aktuell.Besetzt = Am_Zug;
				p[Am_Zug].Figuren[Gewaehlt].Route_Pos = 0;

				aus_haus_gesetzt = true;
			}
		}

		private int Setze_Zielfeld(int figur)
		{
			//Wurde garnicht gewuerfelt, ist kein Zug moeglich
			if (Wurf == 0)
				return -1;

			int zielfeld = p[Am_Zug].Figuren[figur].Route_Pos + Wurf;

			//Fall 1: Aus dem Haus setzen
			if (p[Am_Zug].Figuren[figur].Route_Pos == -1 && Wurf == 6 && p[Am_Zug].Route.F[0].Besetzt != Am_Zug)
			{
				return 0;

			}   //Fall 2 & 3:  Uebers Ziel hinaus, Feld besetzt vom selben Spieler
			else if (zielfeld >= 44 || p[Am_Zug].Route.F[zielfeld].Besetzt == Am_Zug)
			{
				return -1;

			}   //Ausnahme die Auftritt wenn das Zielfeld genau 0 ist
			else if (p[Am_Zug].Figuren[figur].Route_Pos == -1 && Wurf == 1)
			{
				return -1;
			}
			else
			{
				return zielfeld;
			}
		}
		#endregion

		#region Bedingungsfunktionen
		private bool Zug_Moeglich()
		{
			for (int i = 0; i < 4; i++)
			{
				int zielfeld = Setze_Zielfeld(i);

				if(zielfeld == 0)
				{
					return true;
				}

				if (zielfeld != -1 && p[Am_Zug].Figuren[i].Route_Pos != -1)
					return true;
			}

			return false;
		}
		private bool Max_Init()
		{
			for (int i = 0; i < 4; i++)
			{
				if (p[Am_Zug].Figuren[i].Route_Pos != -1)
					return false;
			}

			return true;
		} 
		private bool Gewonnen()
		{
			for (int i = 0; i < 4; i++)
			{
				if (p[Am_Zug].Figuren[i].Route_Pos < 39)
					return false;
			}

			return true;
		}
		#endregion

		#region Statusfunktionen
		private void Beende_Spiel()
		{
			Spiel_zuende = true;
			Gewinner = p[Am_Zug].Name;
		}

		private void Naechster_Zug()
		{
			if(Am_Zug < p.Count - 1)
			{
				Am_Zug++;
			}
			else
			{
				Am_Zug = 0;
			}

			aus_haus_gesetzt = false;

			Reset_Status();
		}

		private void Reset_Status()
		{
			Wurf = 0;
			Gewaehlt = -1;
			zug_moeglich = false;

			bool alle_im_haus = Max_Init();
			if (alle_im_haus)
			{
				rest_wuerfe = 3;
			}
			else
			{
				rest_wuerfe = 1;
			}
		}
		public int Gib_Figur(Feld pos)
		{
			if (pos.Besetzt == -1)
				return -1;

			for (int i = 0; i < 4; i++)
			{
				if (p[pos.Besetzt].Figuren[i].Aktuell == pos)
				{
					return i;
				}
			}

			return -1;
		}
		#endregion
	}
}
