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
using MAD.Ressourcen.Datenstruktur;

namespace MAD.Ressourcen.UserControls
{
	/// <summary>
	/// Interaktionslogik für Einstellungen.xaml
	/// </summary>
	public partial class Einstellungen : UserControl
	{
		Einstellung setting;
		int spieler = 0;

		public Einstellungen()
		{
			setting = new Einstellung();
			InitializeComponent();
		}

		//Funktionalitaet die spaeter gewaehrleistet werden soll
		//Man soll sich die Farbe des Spielers aussuchen koennen
		/*private void Waehle_Spieler_Farbe_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button)sender;

			if(b.Name == "blue")
			{
				tmp.farbe = 1;
			}
			else if(b.Name == "red")
			{
				tmp.farbe = 2;
			}
			else if(b.Name == "green")
			{
				tmp.farbe = 3;
			}
			else if(b.Name == "yellow")
			{
				tmp.farbe = 4;
			}
		}*/

		private void Fuege_Hinzu_Click(object sender, RoutedEventArgs e)
		{
			//Fehler werfen wenn zuviele Spieler hinzugefuegt werden
			if(spieler >= 4)
			{
				MessageBox.Show("Maximale Spieleranzahl erreicht!");
				name.Text = String.Empty;
				return;
			}
			//Fehler werfen wenn kein Name eingegeben wurde
			if (String.IsNullOrEmpty(name.Text))
			{
				MessageBox.Show("Kein Name eingegeben!");
				return;
			}
			//Fehler wenn Name zu lang ist
			if(name.Text.Length > 9)
			{
				MessageBox.Show("Name zu lang!");
				return;
			}

			spieler++;
			setting.name.Add(name.Text);
			name.Text = String.Empty;

			//Den zu uebergebenden Spieler einstellen
			CreateInternalPlayer(spieler - 1);
			//Die visuelle Darstellung der Figur generieren
			CreateVisualPlayer(spieler - 1);
		}

		private void CreateInternalPlayer(int auswahl)
		{
			//Reihenfolge der Spieler festlegen
			switch (auswahl)
			{
				case 0: setting.reihenfolge.Add(0); break;
				case 1: setting.reihenfolge.Add(1); break;
				case 2: setting.reihenfolge.Add(2); break;
				case 3: setting.reihenfolge.Add(3); break;
				default: MessageBox.Show("Ein Fehler ist aufgetreten!"); return;
			}
		}

		private void CreateVisualPlayer(int auswahl)
		{
			Button b = new Button();
			switch (setting.reihenfolge.Count - 1)
			{
				case 0: b.Style = (Style)Application.Current.FindResource("Figur_Blue_Small"); break;
				case 1: b.Style = (Style)Application.Current.FindResource("Figur_Red_Small"); break;
				case 2: b.Style = (Style)Application.Current.FindResource("Figur_Green_Small"); break;
				case 3: b.Style = (Style)Application.Current.FindResource("Figur_Yellow_Small"); break;
				default: MessageBox.Show("Ein Fehler ist aufgetreten!"); break;
			}

			//Die Figur in das Grid einfuegen
			b.Height = Double.NaN;
			b.Width = Double.NaN;
			visuelleSpieler.Children.Add(b);
			Grid.SetColumn(b, auswahl);
			Grid.SetRow(b, 1);

			//Den Namen ueber der Figur anzeigen lassen
			Label l = new Label();
			l.Style = (Style)Application.Current.FindResource("Menu_Label");
			l.Content = setting.name[auswahl];
			l.HorizontalContentAlignment = HorizontalAlignment.Center;
			visuelleSpieler.Children.Add(l);
			Grid.SetColumn(l, auswahl);
			Grid.SetRow(l, 0);
		}

		private void Spiel_Start_Click(object sender, RoutedEventArgs e)
		{
			if(spieler == 0)
			{
				MessageBox.Show("Keine Spieler ausgewählt!");
				return;
			}
			else if(spieler < 2)
			{
				MessageBox.Show("Du kannst nicht alleine Spielen!");
				return;
			}

			Application.Current.MainWindow.Content = new Spiel(setting);
		}

		private void Zuruecksetzen_Click(object sender, RoutedEventArgs e)
		{
			//Modell reinigen
			spieler = 0;
			setting.name.Clear();
			setting.reihenfolge.Clear();
			//Visuell reinigen
			visuelleSpieler.Children.Clear();
		}

		private void zurueck_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Startbild();
		}

		private void enter_Pressed_Click(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
				Fuege_Hinzu_Click(sender, e);
		}
	}
}
