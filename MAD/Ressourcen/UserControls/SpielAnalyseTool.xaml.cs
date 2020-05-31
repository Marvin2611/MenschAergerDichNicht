using System;
using System.Windows;
using System.Windows.Threading;
using MAD.Ressourcen.Datenstruktur;
using MAD.Ressourcen.Spiellogik;

namespace MAD.Ressourcen.UserControls
{
	/// <summary>
	/// Interaktionslogik für SpielAnalyseTool.xaml
	/// </summary>
	public partial class SpielAnalyseTool : Window
	{
		public Controller c;
		public Figur aktiveFigur;
		public SpielAnalyseTool()
		{
			InitializeComponent();

			DispatcherTimer time = new DispatcherTimer();
			time.Tick += new EventHandler(timeTick_tick);
			time.Interval = new TimeSpan(0, 0, 0, 1);
			time.Start();
		}

		public void Bereiche()
		{
			if (c.ZuBewegen == null)
				return;

			aktiveFigur = c.ZuBewegen;

			if (aktiveFigur.Route_Pos == -1)
				Bereich.DataContext = "Haus";
			else if (aktiveFigur.Route_Pos >= 4 || aktiveFigur.Route_Pos <= 39)
				Bereich.DataContext = "Weg";
			else if (aktiveFigur.Route_Pos >= 40)
				Bereich.DataContext = "Ziel";
			else
				Bereich.DataContext = "Null";

			Position.DataContext = aktiveFigur.Route_Pos;
		}

		private void timeTick_tick(object sender, EventArgs e)
		{
			c = (Controller)DataContext;

			Bereiche();
		}
	}
}
