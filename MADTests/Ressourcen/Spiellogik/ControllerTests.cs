using Microsoft.VisualStudio.TestTools.UnitTesting;
using MAD.Ressourcen.Spiellogik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAD.Ressourcen.Datenstruktur;

namespace MAD.Ressourcen.Spiellogik.Tests
{
	[TestClass()]
	public class ControllerTests
	{
		#region Spiel Initialisierung

		#region Add_Spieler
		[TestMethod()] //Erwartet: 0, 1, 2, 3
		public void Add_Spieler_Name()
		{
			List<Spieler> test = new List<Spieler>();
			Controller c = new Controller();

			for (int i = 0; i < 4; i++)
			{
				test.Add(new Spieler("" + i, 4));

				c.Add_Spieler("" + i);

				Assert.IsTrue(c.p[i].Name == test[i].Name);
			}
		}

		[TestMethod()] //Erwartet: Spieler.Figur.Feld == Brett.Haus.Feld
		public void Add_Spieler_Feld_Verweis()
		{
			Controller c = new Controller();

			for (int i = 0; i < 4; i++)
			{
				c.Add_Spieler("" + i);

				Assert.IsTrue(c.p[0].Figuren[i].Aktuell == c.b.Haus[0].F[i]);
			}
		}

		[TestMethod()] //Erwartet: Spieler.Figur.Feld.Besetzt == 0
		public void Add_Spieler_Feld_Besetzt()
		{
			Controller c = new Controller();
			c.Add_Spieler("");

			for (int i = 0; i < 4; i++)
			{
				Assert.IsTrue(c.p[0].Figuren[i].Aktuell.Besetzt == 0);
			}
		}

		[TestMethod()] //Erwartet: Spieler.Figur.Feld.Aktuell
		public void Add_Spieler_Feld_Position()
		{
			Controller c = new Controller();
			c.Add_Spieler("");

			for (int i = 0; i < 4; i++)
			{
				Assert.IsTrue(c.p[0].Figuren[i].Aktuell.Position == i);
			}
		}

		[TestMethod()] //Erwartet: Anzahl der Figuren = 4
		public void Add_Spieler_Figuren_Anzahl()
		{
			Controller c = new Controller();
			c.Add_Spieler("Test");

			Assert.IsTrue(4 == c.p[0].Figuren.Count);
		}
		#endregion

		#region Setze_Pfad
		[TestMethod()] //Erwartet 0 - 39 Weg, 0 - 4 Ziel
		public void Setze_Pfad_Spieler_0()
		{
			Controller c = new Controller();
			c.Add_Spieler("0");
			bool[] con1 = new bool[40];
			bool[] con2 = new bool[4];

			for (int i = 0; i < 40; i++)
			{
				if (c.p[0].Route.F[i] == c.b.Weg[i])
					con1[i] = true;
			}

			for (int i = 0; i < 4; i++)
			{
				if (c.p[0].Route.F[i + 40] == c.b.Ziel[0].F[i])
					con2[i] = true;
			}

			for (int i = 0; i < 40; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Assert.IsTrue(con1[i] && con2[j]);
				}
			}
		}

		[TestMethod()] //Erwartet 10 - 39 Weg, 0 - 9 Weg, 0 - 4 Ziel
		public void Setze_Pfad_Spieler_1()
		{
			Controller c = new Controller();
			c.Add_Spieler("0");
			c.Add_Spieler("1");
			bool[] con1 = new bool[40];
			bool[] con2 = new bool[4];

			//Verweis auf Wege von 10 - 39 pruefen
			for (int i = 0; i < 30; i++)
			{
				if (c.p[1].Route.F[i] == c.b.Weg[i + 10])
					con1[i] = true;
			}

			//Verweis auf Wege von 0 - 9 puefen
			for (int i = 0; i < 10; i++)
			{
				if (c.p[1].Route.F[i + 30] == c.b.Weg[i])
					con1[i + 30] = true;
			}

			//Verweis auf Ziele von 0 - 4 pruefen
			for (int i = 0; i < 4; i++)
			{
				if (c.p[1].Route.F[i + 40] == c.b.Ziel[1].F[i])
					con2[i] = true;
			}

			//Bools pruefen
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Assert.IsTrue(con1[i] && con2[j]);
				}
			}
		}

		[TestMethod()] //Erwartet 20 - 39 Weg, 0 - 19 Weg, 0 - 4 Ziel
		public void Setze_Pfad_Spieler_2()
		{
			Controller c = new Controller();
			c.Add_Spieler("0");
			c.Add_Spieler("1");
			c.Add_Spieler("2");
			bool[] con1 = new bool[40];
			bool[] con2 = new bool[4];

			//Verweis auf Wege von 20 - 39 pruefen
			for (int i = 0; i < 20; i++)
			{
				if (c.p[2].Route.F[i] == c.b.Weg[i + 20])
					con1[i] = true;
			}

			//Verweis auf Wege von 0 - 19 puefen
			for (int i = 0; i < 20; i++)
			{
				if (c.p[2].Route.F[i + 20] == c.b.Weg[i])
					con1[i + 20] = true;
			}

			//Verweis auf Ziele von 0 - 4 pruefen
			for (int i = 0; i < 4; i++)
			{
				if (c.p[2].Route.F[i + 40] == c.b.Ziel[2].F[i])
					con2[i] = true;
			}

			//Bools pruefen
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Assert.IsTrue(con1[i] && con2[j]);
				}
			}
		}

		[TestMethod()] //Erwartet 30 - 39 Weg, 0 - 29 Weg, 0 - 4 Ziel
		public void Setze_Pfad_Spieler_3()
		{
			Controller c = new Controller();
			c.Add_Spieler("0");
			c.Add_Spieler("1");
			c.Add_Spieler("2");
			c.Add_Spieler("3");
			bool[] con1 = new bool[40];
			bool[] con2 = new bool[4];

			//Verweis auf Wege von 30 - 39 pruefen
			for (int i = 0; i < 10; i++)
			{
				if (c.p[3].Route.F[i] == c.b.Weg[i + 30])
					con1[i] = true;
			}

			//Verweis auf Wege von 0 - 29 puefen
			for (int i = 0; i < 20; i++)
			{
				if (c.p[3].Route.F[i + 10] == c.b.Weg[i])
					con1[i + 10] = true;
			}

			//Verweis auf Ziele von 0 - 4 pruefen
			for (int i = 0; i < 4; i++)
			{
				if (c.p[3].Route.F[i + 40] == c.b.Ziel[3].F[i])
					con2[i] = true;
			}

			//Bools pruefen
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Assert.IsTrue(con1[i] && con2[j]);
				}
			}
		}
		#endregion

		#region Entf_Spieler
		[TestMethod()] //Erwartet: 0 Spieler
		public void Entf_Spieler_1_Spieler_Count()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Entf_Spieler();

			Assert.IsTrue(0 == c.p.Count);
		}


		[TestMethod()] //Erwartet: Häuser alle -1 (unbesetzt)
		public void Entf_Spieler_Haus_Besetzt()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Entf_Spieler();

			for (int i = 0; i < 4; i++)
			{
				Assert.IsTrue(c.b.Haus[0].F[i].Besetzt == -1);
			}
		}

		[TestMethod()] //Erwartet: 0, keine Spieler
		public void Entf_Spieler_Keine_Spieler()
		{
			Controller c = new Controller();
			c.Entf_Spieler();

			Assert.IsTrue(0 == c.p.Count);
		}
		#endregion

		#endregion

		#region Spieler Aktionen

		#region Waehle_Figur
		[TestMethod()] //Erwartet: Gewaehlt == 0
		public void Waehle_Figur_0()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Waehle_Figur(0);
			Assert.IsTrue(c.Gewaehlt == 0);
		}

		[TestMethod()] //Erwartet: Gewaehlt == 1
		public void Waehle_Figur_1()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Waehle_Figur(1);
			Assert.IsTrue(c.Gewaehlt == 1);
		}

		[TestMethod()] //Erwartet: Gewaehlt == 2
		public void Waehle_Figur_2()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Waehle_Figur(2);
			Assert.IsTrue(c.Gewaehlt == 2);
		}

		[TestMethod()] //Erwartet: Gewaehlt == 3
		public void Waehle_Figur_3()
		{
			Controller c = new Controller();
			c.Add_Spieler("1");
			c.Waehle_Figur(3);
			Assert.IsTrue(c.Gewaehlt == 3);
		}
		#endregion

		#endregion

		#region Pruefe Funktionen

		#region Gib_Figur
		[TestMethod()] //Erwartet: Spieler.Figur 0
		public void Gib_Figur_Spieler_0_Figur_0()
		{
			Controller c = new Controller();
			c.Add_Spieler("");

			Assert.IsTrue(c.Gib_Figur(c.b.Haus[0].F[0]) == 0);
		}

		[TestMethod()] //Erwartet: Spieler.Figur 1
		public void Gib_Figur_Spieler_0_Figur_1()
		{
			Controller c = new Controller();
			c.Add_Spieler("");

			Assert.IsTrue(c.Gib_Figur(c.b.Haus[0].F[1]) == 1);
		}

		[TestMethod()] //Erwartet: Keine Figur
		public void Gib_Figur_Keine()
		{
			Controller c = new Controller();

			Assert.IsTrue(c.Gib_Figur(c.b.Weg[0]) == -1);
		}
		#endregion

		#endregion

		#region Naechster_Zug
		[TestMethod()] //Erwartet: Nächster Spieler ist Spieler 0, 1 Zug
		public void Naechster_Zug_1_Spieler_Ein_Zug()
		{
			Controller c = new Controller();
			PrivateObject cT = new PrivateObject(c);
			c.Add_Spieler("1");
			cT.Invoke("Naechster_Zug");
			Assert.IsTrue(c.Am_Zug == 0);
		}

		[TestMethod()] //Erwartet: Nächster Spieler ist Spieler 3, 3 Züge
		public void Naechster_Zug_4_Spieler_Drei_Zuege()
		{
			Controller c = new Controller();
			PrivateObject cT = new PrivateObject(c);
			for (int i = 0; i < 4; i++)
			{
				c.Add_Spieler("" + i);
			}
			for (int i = 0; i < 3; i++)
			{
				cT.Invoke("Naechster_Zug");
			}
			Assert.IsTrue(c.Am_Zug == 3);
		}

		[TestMethod()] //Erwartet: Nächster Spieler ist Spieler 0, 4 Züge
		public void Naechster_Zug_4_Spieler_Eine_Runde()
		{
			Controller c = new Controller();
			PrivateObject cT = new PrivateObject(c);
			for (int i = 0; i < 4; i++)
			{
				c.Add_Spieler("" + i);
			}
			for (int i = 0; i < 4; i++)
			{
				cT.Invoke("Naechster_Zug");
			}
			Assert.IsTrue(c.Am_Zug == 0);
		}
		#endregion
	}
}