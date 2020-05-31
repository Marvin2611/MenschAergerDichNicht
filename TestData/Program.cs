using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAD.Ressourcen.Datenstruktur;
using MAD.Ressourcen.Spiellogik;

namespace ConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Controller c = new Controller();

			c.Add_Spieler("Niklas");
			c.Add_Spieler("Kai");
			c.Add_Spieler("Michael");
			c.Add_Spieler("Marvin");
		}
	}
}
