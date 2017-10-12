//*******************************************
// autor: Martín Alejandro Pérez Güendulain *
// email: mperezguendulain@gmail.com        *
//*******************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueue;
using System.Windows;

namespace Algoritmos_de_Busqueda_v2._0
{
	public static class AlgoritmosDeBusqueda
	{
		#region Algoritmos de Busqueda
		public static Dictionary<string, string> busquedaPreferentePorAmplitud(Dictionary<string, List<Tuple<string, int>>> grafo, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			Queue<ArbolNario<string>> cola = new Queue<ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			ArbolNario<string> nodo;
			string str_res = "";

			nodo = new ArbolNario<string>(str_nodo_inicial);
			cola.Enqueue(nodo);
			nodos_expandidos.Add(nodo);

			while (cola.Count != 0)
			{
				nodo = cola.Dequeue();
				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				str_res += nodo.Dato;

				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
					{
						cola.Enqueue(nodo_vecino);
						nodos_expandidos.Add(nodo_vecino);
					}
				}

			}
			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}


		public static Dictionary<string, string> busquedaPreferentePorProfundidad(Dictionary<string, List<Tuple<string, int>>> grafo, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			List<ArbolNario<string>> nodos_actuales = new List<ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			ArbolNario<string> nodo;
			string str_res = "";

			nodos_actuales.Insert(0, new ArbolNario<string>(str_nodo_inicial));

			while (nodos_actuales.Count != 0)
			{
				nodo = nodos_actuales.First();
				nodos_actuales.RemoveAt(0);

				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;


				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
						nodos_actuales.Insert(0, nodo_vecino);
				}
			}
			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}

		public static Dictionary<string, string> busquedaPreferentePorProfundidad(Dictionary<string, List<Tuple<string, int>>> grafo, int limite, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			List<ArbolNario<string>> nodos_actuales = new List<ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			ArbolNario<string> nodo;
			string str_res = "";

			nodos_actuales.Insert(0, new ArbolNario<string>(str_nodo_inicial));

			while (nodos_actuales.Count != 0)
			{
				nodo = nodos_actuales.First();
				nodos_actuales.RemoveAt(0);

				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;

				if (nodo.Altura < limite)
				{
					// Expandimos
					foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
					{
						ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
						nodo.addChild(nodo_vecino);
						nodo_vecino.Altura = nodo.Altura + 1;
						if (!contieneNodo(nodos_expandidos, nodo_vecino))
							nodos_actuales.Insert(0, nodo_vecino);
					}
				}
			}
			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}

		public static Dictionary<string, string> busquedaLimitadaEnProfundidad(Dictionary<string, List<Tuple<string, int>>> grafo, int limite, string str_nodo_inicial, string str_nodo_final)
		{
			return busquedaPreferentePorProfundidad(grafo, limite, str_nodo_inicial, str_nodo_final);
		}

		public static Dictionary<string, string> busquedaPorProfundizacionIterativa(Dictionary<string, List<Tuple<string, int>>> grafo, string str_nodo_inicial, string str_nodo_final)
		{
			int limite = 0;
			const int PROFUNDIDAD_MAXIMA = 10;
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			while (limite <= PROFUNDIDAD_MAXIMA)
			{
				resultado = busquedaPreferentePorProfundidad(grafo, limite, str_nodo_inicial, str_nodo_final);
				if (resultado["ruta"] != "No hay camino.")
					return resultado;
				limite++;
			}
			return resultado;
		}

		public static Dictionary<string, string> busquedaPorCostoUniforme(Dictionary<string, List<Tuple<string, int>>> grafo, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			PriorityQueue<int, ArbolNario<string>> cola = new PriorityQueue<int, ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			string str_res = "";

			cola.Add(new KeyValuePair<int, ArbolNario<string>>(0, new ArbolNario<string>(str_nodo_inicial)));

			while (!cola.IsEmpty)
			{
				ArbolNario<string> nodo = cola.DequeueValue();
				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;

				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					nodo_vecino.CostoRuta = nodo.CostoRuta + vecino.Item2;
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
						cola.Add(new KeyValuePair<int, ArbolNario<string>>(nodo_vecino.CostoRuta, nodo_vecino));
				}

			}
			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}

		public static Dictionary<string, string> busquedaAvara(Dictionary<string, List<Tuple<string, int>>> grafo, List<Node> nodos, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			PriorityQueue<double, ArbolNario<string>> cola = new PriorityQueue<double, ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			string str_res = "";

			Dictionary<string, double> distancias = calcularDistancias(nodos, getNodoFromList(nodos, str_nodo_final));
			cola.Add(new KeyValuePair<double, ArbolNario<string>>(distancias[str_nodo_inicial], new ArbolNario<string>(str_nodo_inicial)));

			while (!cola.IsEmpty)
			{
				ArbolNario<string> nodo = cola.DequeueValue();
				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;

				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
						cola.Add(new KeyValuePair<double, ArbolNario<string>>(distancias[nodo_vecino.Dato], nodo_vecino));
				}

			}
			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}

		public static Dictionary<string, string> busquedaAAsterisco(Dictionary<string, List<Tuple<string, int>>> grafo, List<Node> nodos, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			PriorityQueue<double, ArbolNario<string>> cola = new PriorityQueue<double, ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			string str_res = "";

			Dictionary<string, double> distancias = calcularDistancias(nodos, getNodoFromList(nodos, str_nodo_final));
			cola.Add(new KeyValuePair<double, ArbolNario<string>>(distancias[str_nodo_inicial] , new ArbolNario<string>(str_nodo_inicial)));

			while (!cola.IsEmpty)
			{
				ArbolNario<string> nodo = cola.DequeueValue();
				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;

				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					nodo_vecino.CostoRuta += nodo.CostoRuta + vecino.Item2;
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
						cola.Add(new KeyValuePair<double, ArbolNario<string>>(nodo_vecino.CostoRuta + distancias[nodo_vecino.Dato], nodo_vecino));
				}
			}

			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			return resultado;
		}

		public static Dictionary<string, string> busquedaAAsterisco(Dictionary<string, List<Tuple<string, int>>> grafo, double limite_de_costo, List<Node> nodos, string str_nodo_inicial, string str_nodo_final)
		{
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			PriorityQueue<double, ArbolNario<string>> cola = new PriorityQueue<double, ArbolNario<string>>();
			List<ArbolNario<string>> nodos_expandidos = new List<ArbolNario<string>>();
			List<double> costos_exedidos_del_limite = new List<double>();
			double f;
			string str_res = "";

			Dictionary<string, double> distancias = calcularDistancias(nodos, getNodoFromList(nodos, str_nodo_final));
			cola.Add(new KeyValuePair<double, ArbolNario<string>>(distancias[str_nodo_inicial], new ArbolNario<string>(str_nodo_inicial)));

			while (!cola.IsEmpty)
			{
				ArbolNario<string> nodo = cola.DequeueValue();
				if (esMeta(nodo, str_nodo_final))
				{
					str_res += nodo.Dato;
					resultado.Add("ruta", getStringReverse(getRuta(nodo)));
					resultado.Add("nodos_visitados", str_res);
					return resultado;
				}

				// Marcamos como visitado
				nodos_expandidos.Add(nodo);
				str_res += nodo.Dato;

				// Expandimos
				foreach (Tuple<string, int> vecino in grafo[nodo.Dato])
				{
					ArbolNario<string> nodo_vecino = new ArbolNario<string>(vecino.Item1);
					nodo.addChild(nodo_vecino);
					nodo_vecino.CostoRuta += nodo.CostoRuta + vecino.Item2;
					if (!contieneNodo(nodos_expandidos, nodo_vecino))
					{
						f = TruncateDecimal(nodo_vecino.CostoRuta + distancias[nodo_vecino.Dato], 4);
						if (f <= limite_de_costo)
							cola.Add(new KeyValuePair<double, ArbolNario<string>>(f, nodo_vecino));
						else
							costos_exedidos_del_limite.Add(f);
					}
				}
			}

			resultado.Add("ruta", "No hay camino.");
			resultado.Add("nodos_visitados", "No hay camino.");
			resultado.Add("nuevo_limite_de_costo", getMinimoFromList(costos_exedidos_del_limite).ToString());
			return resultado;
		}
		
		public static Dictionary<string, string> busquedaAAsteriscoConProfundidadIterativa(Dictionary<string, List<Tuple<string, int>>> grafo, List<Node> nodos, string str_nodo_inicial, string str_nodo_final)
		{
			double limite_de_costo = getDistancia(getNodoFromList(nodos, str_nodo_inicial), getNodoFromList(nodos, str_nodo_final));
			const int COSTO_MAXIMO = 10000;
			string str_limites = "";
			int iteracion = 0;
			Dictionary<string, string> resultado = new Dictionary<string, string>();

			while (limite_de_costo <= COSTO_MAXIMO)
			{
				str_limites += "Iteración: " + iteracion++ + "\n";
				str_limites += "Nuevo Limite: " + limite_de_costo + "\n\n\n";
				resultado = busquedaAAsterisco(grafo, limite_de_costo, nodos, str_nodo_inicial, str_nodo_final);
				if (resultado["ruta"] != "No hay camino.")
				{
					resultado.Add("limites", str_limites);
					return resultado;
				}
				limite_de_costo = Double.Parse(resultado["nuevo_limite_de_costo"]);
			}
			return resultado;
		}


		#endregion


		#region Métodos Auxiliares
		private static double TruncateDecimal(double value, int precision)
		{
			double step = Math.Pow(10, precision);
			int tmp = (int)Math.Truncate(step * value);
			return tmp / step;
		}

		private static double getMinimoFromList(List<double> costos)
		{
			double costo_minimo = costos[0];
			int num_de_costos = costos.Count;
			for(int i = 1; i < num_de_costos; i++)
				if (costos[i] < costo_minimo)
					costo_minimo = costos[i];
			return costo_minimo;
		}

		private static bool esMeta(ArbolNario<string> nodo, string nodo_final)
		{
			return nodo.Dato == nodo_final;
		}

		private static string getStringReverse(string cadena)
		{
			string str_res = "";

			for (int i = cadena.Length - 1; i >= 0; i--)
			{
				str_res += cadena[i];
			}
			return str_res;
		}

		private static string getRuta(ArbolNario<string> nodo)
		{
			string ruta = "";

			while (true)
			{
				ruta += nodo.Dato.ToString();
				if (nodo.Padre == null)
					return ruta;
				nodo = nodo.Padre;
			}
		}

		private static bool contieneNodo(List<ArbolNario<string>> lista_de_nodos, ArbolNario<string> nodo)
		{
			foreach (ArbolNario<string> nodo_temp in lista_de_nodos)
			{
				if (nodo.Dato == nodo_temp.Dato)
					return true;
			}
			return false;
		}

		private static Dictionary<string, double> calcularDistancias(List<Node> nodos, Node nodo_final)
		{
			Dictionary<string, double> distancias = new Dictionary<string, double>();
			int num_nodos = nodos.Count;
			double distancia;

			for (int i = 0; i < num_nodos; i++)
			{
				distancia = getDistancia(nodos[i], nodo_final);
				distancias.Add(nodos[i].Name, distancia);
			}

			return distancias;
		}

		private static double getDistancia(Node A, Node B)
		{
			return Math.Sqrt(Math.Pow(B.X - A.X, 2.0) + Math.Pow(B.Y - A.Y, 2.0));
		}

		private static Node getNodoFromList(List<Node> nodos, string str_nodo)
		{
			int num_nodos = nodos.Count;
			for (int i = 0; i < num_nodos; i++)
			{
				if (nodos[i].Name == str_nodo)
					return nodos[i];
			}
			return null;
		}
		#endregion
	}
}
