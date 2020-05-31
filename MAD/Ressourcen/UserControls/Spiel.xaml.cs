using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MAD.Properties;
using MAD.Ressourcen.Datenstruktur;
using MAD.Ressourcen.Spiellogik;

namespace MAD.Ressourcen.UserControls
{
	/// <summary>
	/// Interaktionslogik für Spiel.xaml
	/// </summary>
	public partial class Spiel : UserControl
	{
		Controller c;

		Button TmpFigurButton;				//Die gewaehlte Figur wird hier gespeichert
		Dictionary<Feld, Point> Brett;		//Den Punkt fuer das Feld finden
		Dictionary<Figur, Button> Figuren;  //Den Button zur zugehoerigen Figur finden

		//Zur Anzeige der Zeit und Animationen
		DispatcherTimer timer;
		private DateTime TimerStart;

		//WICHTIG: Entwickler Tool, somit nicht spielrelevant
		Window Analyse;	
		public Spiel(Einstellung set)
		{
			c = new Controller();

			Brett = new Dictionary<Feld, Point>();
			Figuren = new Dictionary<Figur, Button>();

			InitializeComponent();

			//Baue das Dictionary fuer das Brett
			Setze_Brett();

			//Hier muessen die Spieler hinzugefuegt werden!
			for (int i = 0; i < set.name.Count; i++)
			{
				Fuege_Spieler_Hinzu(set.name[i]);
			}
			//Initialisiere die Figuren
			Initialisiere_Figuren(c.p.Count);

			//Baue das Dictionary fuer die Figuren
			Setze_Figuren();

			//Setze den Datenkontext vom Fenster auf den Controller
			DataContext = c;

			//Aktiviere Analyse-Tool fuer Entwickler
			//Analyse_Start();

			//Initialisiere Styles
			Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Empty");

			//Starte den Timer
			this.TimerStart = DateTime.Now;
			timer = new DispatcherTimer();
			timer.Tick += new EventHandler(Interface_Update);
			timer.Tick += new EventHandler(Interface_Zeit);
			timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
			timer.Start();
		}

		#region Dispatcher Funktionen
		private void Interface_Update(object sender, EventArgs e)
		{
			//Aktualisiere den Spieler der dran ist
			switch (c.Am_Zug)
			{
				case 0: Spieler_Am_Zug.Style = (Style)Application.Current.FindResource("Figur_Blue_Small"); break;
				case 1: Spieler_Am_Zug.Style = (Style)Application.Current.FindResource("Figur_Red_Small"); break;
				case 2: Spieler_Am_Zug.Style = (Style)Application.Current.FindResource("Figur_Green_Small"); break;
				case 3: Spieler_Am_Zug.Style = (Style)Application.Current.FindResource("Figur_Yellow_Small"); break;
			}

			//Wenn der Controller gewonnen zurueckgibt, zeige das Gewinnerfenster
			if (c.Spiel_zuende)
			{
				Spieler_Gewonnen(c.Gewinner);
			}
		}

		private void Interface_Zeit(object sender, EventArgs e)
		{
			//Zeige die Spielzeit
			var tmp = DateTime.Now - this.TimerStart;
			TimeSpan spielzeit = new TimeSpan(tmp.Hours, tmp.Minutes, tmp.Seconds);
			Spielzeit.Text = spielzeit.ToString();
		}

		public void Analyse_Start()
		{
			Analyse = new SpielAnalyseTool();
			Analyse.DataContext = c;
			Analyse.Owner = Application.Current.MainWindow;
			Analyse.Show();
		}
		#endregion

		#region Spiel initialisieren
		public void Fuege_Spieler_Hinzu(string name)
		{
			c.Add_Spieler(name);
		}

		public void Entf_Spieler()
		{
			c.Entf_Spieler();
		}

		private void Initialisiere_Figuren(int spieler)
		{
			for (int i = 0; i < spieler; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					//Setze die Grafik fuer die Figuren
					Button b = new Button();
					switch (i)
					{
						case 0: b.Style = (Style)Application.Current.FindResource("Figur_Blue_Small"); break;
						case 1: b.Style = (Style)Application.Current.FindResource("Figur_Red_Small"); break;
						case 2: b.Style = (Style)Application.Current.FindResource("Figur_Green_Small"); break;
						case 3: b.Style = (Style)Application.Current.FindResource("Figur_Yellow_Small"); break;
					}
					//Bereite den Button vor und Bewege ihn im Grid an die Initialposition
					b.DataContext = c.p[i].Figuren[j];
					b.Content = j + 1;
					b.Click += Auswahl_Click;
					GBrett.Children.Add(b);
					Bewege_Grid(b, Brett[c.b.Haus[i].F[j]]);
				}
			}
		}
		#endregion

		#region Figuren erstellen
		private List<Button> Setze_Buttons()
		{
			//ANMERKUNG:
			//Die Liste wird dem XAML-Code folgend befuellt.
			//Das heisst in diesem Fall:
			//Erst die 4 Figuren von Spieler 1, dann Spieler 2 usw.

			List<Button> buttons = new List<Button>();

			foreach (UIElement b in GBrett.Children)
			{
				try
				{
					Button graphicalFigur = (Button)b;
					buttons.Add(graphicalFigur);
				}
				catch (Exception)
				{
					continue;
				}
			}

			return buttons;
		}

		private void Setze_Figuren()
		{
			List<Button> visuell = Setze_Buttons();

			//Fuelle das Dictionary fuer die Figuren
			int bCount = 0;
			for (int i = 0; i < c.p.Count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Figuren.Add(c.p[i].Figuren[j], visuell[bCount]);
					bCount++;
				}
			}
		}

		#endregion

		#region Brett erstellen
		private List<Point> Setze_Punkte()
		{
			//ANMERKUNG:
			//Die Liste wird dem XAML-Code folgend befuellt.
			//Das heisst in diesem Fall:
			//Erst die 16 Häuser in der Reihenfolge Blau-Rot-Gruen-Gelb
			//Dann die 16 Ziele in der selben Reihenfolge
			//Dann die Wege vom blauen Startfeld im Uhrzeigersinn

			List<Point> punkte = new List<Point>();

			foreach (UIElement f in GBrett.Children)
			{
				try
				{
					Ellipse feld = (Ellipse)f;
					punkte.Add(new Point(Grid.GetColumn(f), Grid.GetRow(f)));
				}
				catch (Exception)
				{
					continue;
				}
			}

			return punkte;
		}

		private void Setze_Brett()
		{
			List<Point> visuell = Setze_Punkte();

			//Fuelle das Dictionary fuer die Haeuser
			int haus = 0;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Brett.Add(c.b.Haus[i].F[j], visuell[haus]);
					haus++;
				}
			}
			//Fuelle das Dictionary fuer die Ziele
			int ziel = 16;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Brett.Add(c.b.Ziel[i].F[j], visuell[ziel]);
					ziel++;
				}
			}
			//Fuelle das Dictionary fuer die Wege
			for (int i = 0; i < 40; i++)
			{
				Brett.Add(c.b.Weg[i], visuell[i + 32]);
			}
		}
		#endregion

		#region Spieler Aktionen
		private void Wuerfeln_Click(object sender, RoutedEventArgs e)
		{
			c.Wuerfeln();

			//Wuerfel Animation
			switch (c.Wurf)
			{
				case 1: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_One");break;
				case 2: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Two"); break;
				case 3: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Three"); break;
				case 4: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Four"); break;
				case 5: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Five"); break;
				case 6: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Six"); break;
				default: Wuerfel.Style = (Style)Application.Current.FindResource("Dice_Empty"); break;
			}
		}

		private void Bewege_Click(object sender, RoutedEventArgs e)
		{
			c.Bewege_Figur();

			if (TmpFigurButton != null)
			{
				Figur tmp = (Figur)TmpFigurButton.DataContext;
				Bewege_Grid(TmpFigurButton, Brett[tmp.Aktuell]);

				//Keine Figur mehr gewaehlt
				Figur_Gewaehlt.Style = (Style)Application.Current.FindResource("Figur_Grey_Small");
				Figur_Gewaehlt.Content = "None";

				if (c.ZuKicken != null)
				{
					Figur kick = c.ZuKicken;
					Bewege_Grid(Figuren[kick], Brett[kick.Aktuell]);
				}
			}
		}

		private void Auswahl_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button)sender;
			Figur f = (Figur)b.DataContext;

			//Wenn f eine der Figuren ist waehle diese aus (0, 1, 2, 3)
			c.Waehle_Figur(f);

			//Wichtig fuer die Bewege Funktion!
			TmpFigurButton = b;

			//Figur Animation
			Figur_Gewaehlt.Style = b.Style;
			Figur_Gewaehlt.Content = b.Content;

		}
		#endregion

		#region Bewege Figuren
		public void Bewege_Grid(Button figur, Point pos)
		{
			Grid.SetColumn(figur, (int)pos.X);
			Grid.SetRow(figur, (int)pos.Y);
		}

		public void Bewege_Grid(Button figur, int zeile, int reihe)
		{
			Grid.SetColumn(figur, zeile);
			Grid.SetRow(figur, reihe);
		}
		#endregion

		private void Game_Menu_Click(object sender, RoutedEventArgs e)
		{
			GameMenu menu = new GameMenu();
			menu.Owner = Application.Current.MainWindow;
			menu.ShowDialog();
		}

		private void Spieler_Gewonnen(string spieler)
		{
			Gewonnen menu = new Gewonnen(spieler);
			menu.Owner = Application.Current.MainWindow;
			menu.ShowDialog();
		}
	}
}
