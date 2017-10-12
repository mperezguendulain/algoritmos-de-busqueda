using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmos_de_Busqueda_v2._0
{
	class ArbolNario<T>
	{
		#region Atributos
		public T Dato { get; set; }
		public ArbolNario<T> Padre { get; set; }
		public List<ArbolNario<T>> Hijos { get; set; }

		public int Altura { get; set; }
		public int CostoRuta { get; set; }
		#endregion

		#region Constructores
		public ArbolNario(T elemento)
		{
			this.Dato = elemento;
			this.Padre = null;
			this.Hijos = null;
			this.CostoRuta = 0;
			this.Altura = 0;
		}

		#endregion

		#region Métodos del Árbol N-ario
		public void addChild(ArbolNario<T> nodo_hijo)
		{
			nodo_hijo.Padre = this;
			if (this.Hijos == null)
				this.Hijos = new List<ArbolNario<T>>();
			this.Hijos.Add(nodo_hijo);
		}
		#endregion
	}
}
