using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmos_de_Busqueda_v2._0
{
	public class Node
	{
		public string Name { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public Node(string nombre, int x, int y)
		{
			this.Name = nombre;
			this.X = x;
			this.Y = y;
		}
	}
}
