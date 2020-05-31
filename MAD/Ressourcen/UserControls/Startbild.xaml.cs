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
	/// Interaktionslogik für Startbild.xaml
	/// </summary>
	public partial class Startbild : UserControl
	{
		public Startbild()
		{
			InitializeComponent();
		}

		private void quick_Click(object sender, RoutedEventArgs e)
		{
			Einstellung set = new Einstellung();
			for (int i = 0; i < 4; i++)
			{
				set.name.Add("Spieler " + i);
				set.reihenfolge.Add(i);
			}
			this.Content = new Spiel(set);
		}

		private void opt_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Einstellungen();
		}

		private void load_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Speichern();
		}

		private void exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void credit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Credits();
		}
	}
}
