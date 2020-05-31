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
using System.Windows.Shapes;
using MAD.Ressourcen.Datenstruktur;
using MAD.Ressourcen.Spiellogik;
using MAD.Ressourcen.UserControls;

namespace MAD.Ressourcen.UserControls
{
	/// <summary>
	/// Interaktionslogik für GameMenu.xaml
	/// </summary>
	public partial class GameMenu : Window
	{
		public GameMenu()
		{
			InitializeComponent();
		}

		private void fortsetzen_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Sorry, not implemented :(");
		}

		private void menu_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Startbild();
			DialogResult = true;
		}


		private void beenden_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
