﻿using System;
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

namespace MAD.Ressourcen.UserControls
{
	/// <summary>
	/// Interaktionslogik für Credits.xaml
	/// </summary>
	public partial class Credits : UserControl
	{
		public Credits()
		{
			InitializeComponent();
		}

		private void zurueck_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Startbild();
		}
	}
}
