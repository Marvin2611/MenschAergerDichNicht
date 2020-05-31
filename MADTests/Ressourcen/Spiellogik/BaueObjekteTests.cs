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
	public class BaueObjekteTests
	{
		[TestMethod()]
		public void Spielfeld_Weg_Position()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			int[] Weg = new int[40];

			for (int i = 0; i < 40; i++)
			{
				Weg[i] = i;

				Assert.IsTrue(Weg[i] == Standart.Weg[i].Position);
			}
		}

		[TestMethod()]
		public void Spielfeld_Weg_Besetzt()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			int[] Weg = new int[40];

			for (int i = 0; i < 40; i++)
			{
				Weg[i] = -1;

				Assert.IsTrue(Weg[i] == Standart.Weg[i].Besetzt);
			}
		}

		[TestMethod()]
		public void Spielfeld_Weg_Anzahl()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			Assert.IsTrue(40 == Standart.Weg.Count);
		}

		[TestMethod()]
		public void Spielfeld_Haus_Position()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			int[] Haus = new int[4];

			for (int i = 0; i < 4; i++)
			{
				Haus[i] = i;

				Assert.IsTrue(Haus[i] == Standart.Haus[0].F[i].Position);
			}
		}

		[TestMethod()]
		public void Spielfeld_Haus_Anzahl()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			Assert.IsTrue(4 == Standart.Haus.Count);
		}

		[TestMethod()]
		public void Spielfeld_Haus_Felder_Anzahl()
		{
			Brett Standart = BaueBrett.Spielbrett(0);

			int[] HausF = new int[4];

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					HausF[j] = j;
					Assert.IsTrue(HausF[j] == Standart.Haus[i].F[j].Position);
				}
			}
		}
	}
}