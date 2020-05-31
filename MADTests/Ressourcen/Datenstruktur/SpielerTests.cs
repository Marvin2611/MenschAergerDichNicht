using Microsoft.VisualStudio.TestTools.UnitTesting;
using MAD.Ressourcen.Datenstruktur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Ressourcen.Datenstruktur.Tests
{
	[TestClass()]
	public class SpielerTests
	{
		[TestMethod()]
		public void Spieler_Figuren_Anzahl()
		{
			Spieler spieler1 = new Spieler("", 4);

			Assert.IsTrue(4 == spieler1.Figuren.Count);
		}
	}
}