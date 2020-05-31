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
	public class WuerfelTests
	{
		[TestMethod()]
		public void WuerfelnTest()
		{
			Wuerfel wurf = new Wuerfel();
			for (int i = 0; i < 50; i++)
			{
				int zahl = wurf.Wuerfeln();

				Assert.IsTrue(zahl >= 1 && zahl <= 6);
			}
		}
	}
}