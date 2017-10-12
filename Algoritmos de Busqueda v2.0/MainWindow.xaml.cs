//*******************************************
// autor: Martín Alejandro Pérez Güendulain *
// email: mperezguendulain@gmail.com        *
//*******************************************

using System;
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

namespace Algoritmos_de_Busqueda_v2._0
{
    public partial class MainWindow : Window
    {
		int ascii_nombre_nodo;
		int puntos_seleccionados;
		Node nodo_ini, nodo_fin;
		List<Node> nodos;
		string str_peso_arista;
		int radio = 35;
		Dictionary<string, List<Tuple<string, int>>> grafo;
		const int PREFERENTE_POR_AMPLITUD = 0;
		const int PREFERENTE_POR_PROFUNDIDAD = 1;
		const int LIMITADA_EN_PROFUNDIDAD = 2;
		const int PROFUNDIZACION_ITERATIVA = 3;
		const int COSTO_UNIFORME = 4;
		const int AVARA = 5;
		const int AASTERISCO = 6;
		const int AASTERISCOCONPROFUNDIZACIONITERATIVA = 7;

		// Colores
		Brush colorLetra = new SolidColorBrush(Colors.White);
		Brush colorNodo = new SolidColorBrush(Color.FromRgb(52, 73, 94));
		Brush colorLinea = new SolidColorBrush(Color.FromRgb(231, 76, 60));

        public MainWindow()
        {
            InitializeComponent();
			cb_metodo_de_busqueda.Items.Insert(PREFERENTE_POR_AMPLITUD, "Preferente por Amplitud");
			cb_metodo_de_busqueda.Items.Insert(PREFERENTE_POR_PROFUNDIDAD, "Preferente por Profundidad");
			cb_metodo_de_busqueda.Items.Insert(LIMITADA_EN_PROFUNDIDAD, "Limitada en Profundidad");
			cb_metodo_de_busqueda.Items.Insert(PROFUNDIZACION_ITERATIVA, "Profundización Iterativa");
			cb_metodo_de_busqueda.Items.Insert(COSTO_UNIFORME, "Costo Uniforme");
			cb_metodo_de_busqueda.Items.Insert(AVARA, "Avara");
			cb_metodo_de_busqueda.Items.Insert(AASTERISCO, "A*");
			cb_metodo_de_busqueda.Items.Insert(AASTERISCOCONPROFUNDIZACIONITERATIVA, "A*PI");
			init();
        }

		private void init()
		{
			puntos_seleccionados = 0;
			ascii_nombre_nodo = Encoding.ASCII.GetBytes("A")[0];
			grafo = new Dictionary<string, List<Tuple<string, int>>>();
			nodos = new List<Node>();
			canvas.Children.Clear();
			cb_nodo_incial.Items.Clear();
			cb_nodo_final.Items.Clear();
		}
		
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point posicion = Mouse.GetPosition(canvas);
			this.Title = "Algoritmos de Busqueda v2.0  -  X : " + posicion.X + ",  Y : " + (canvas.ActualHeight - posicion.Y);
        }

		private void btnAgregarNodo_Click(object sender, RoutedEventArgs e)
		{
			int posX = Int32.Parse(tbX.Text);
			int posY = (int)canvas.ActualHeight - Int32.Parse(tbY.Text);

			agregarNodo(posX, posY);
			tbX.Text = "";
			tbY.Text = "";
			tbX.Focus();
		}

		private void agregarNodo(int posX, int posY)
		{
			string nombre_nodo = Convert.ToChar(ascii_nombre_nodo).ToString();
			nodos.Add(new Node(nombre_nodo, posX, (int)canvas.ActualHeight - posY));
			canvas.Children.Add(getElipse(new Point(posX, posY)));
			canvas.Children.Add(getTextBlock((int)(posX - 7), (int)(posY - 15), Convert.ToChar(ascii_nombre_nodo).ToString(), colorLetra, 20));
			grafo.Add(nombre_nodo, new List<Tuple<string, int>>());

			cb_nodo_incial.Items.Add(Convert.ToChar(ascii_nombre_nodo).ToString());
			cb_nodo_final.Items.Add(Convert.ToChar(ascii_nombre_nodo).ToString());

			if (nombre_nodo == "Z")
				ascii_nombre_nodo += 7;
			else
				ascii_nombre_nodo += 1;
		}

		private TextBlock getTextBlock(int x, int y, string texto, Brush pincel, int font_size)
		{
			TextBlock bloque_de_texto = new TextBlock();
			bloque_de_texto.Text = texto;
			bloque_de_texto.Foreground = pincel;
			bloque_de_texto.FontSize = font_size;
			//bloque_de_texto.FontWeight = FontWeights.Bold;
			bloque_de_texto.Margin = new Thickness(x, y, 0, 0);

			return bloque_de_texto;
		}

		private Ellipse getElipse(Point p)
		{
			Ellipse elipse = new Ellipse();
			elipse.Name = Convert.ToChar(ascii_nombre_nodo).ToString();
			elipse.MouseRightButtonUp += Nodo_MouseRightButtonUp;
			elipse.Fill = colorNodo;
			elipse.Margin = new Thickness(p.X - (radio / 2), p.Y - (radio / 2), 0, 0);
			elipse.Width = radio;
			elipse.Height = radio;

			return elipse;
		}

		private void Nodo_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			puntos_seleccionados++;
			Ellipse elipse = sender as Ellipse;
			if (puntos_seleccionados == 1)
			{
				nodo_ini = new Node(elipse.Name, (int)(elipse.Margin.Left + (radio / 2)), (int)(elipse.Margin.Top + (radio / 2)));
			}
			else if (puntos_seleccionados == 2)
			{
				nodo_fin = new Node(elipse.Name, (int)(elipse.Margin.Left + (radio / 2)), (int)(elipse.Margin.Top + (radio / 2)));
				if (nodo_ini.Name != nodo_fin.Name)
				{
					// Dibuja linea
					Line linea = new Line();
					linea.Stroke = colorLinea;
					linea.StrokeThickness = 3;
					linea.X1 = nodo_ini.X;
					linea.X2 = nodo_fin.X;
					linea.Y1 = nodo_ini.Y;
					linea.Y2 = nodo_fin.Y;
					canvas.Children.Add(linea);

					do
					{
						str_peso_arista = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el peso de la arista: ", "Peso de la Arista", "1");
					} while (str_peso_arista == "");


					// Agregamos la arista al grafo
					grafo[nodo_ini.Name].Add(new Tuple<string, int>(nodo_fin.Name, Int32.Parse(str_peso_arista)));
					grafo[nodo_fin.Name].Add(new Tuple<string, int>(nodo_ini.Name, Int32.Parse(str_peso_arista)));

					// Pintando el peso de la arista
					canvas.Children.Add(getTextBlock((nodo_ini.X + nodo_fin.X) / 2, (nodo_ini.Y + nodo_fin.Y) / 2, str_peso_arista, new SolidColorBrush(Colors.Black), 15));


					puntos_seleccionados = 0;
				}
				else
				{
					MessageBox.Show("Debe de seleccionar nodos diferentes");
					puntos_seleccionados--;
				}
			}
		}

		private void btnLimpiarGrafo_Click(object sender, RoutedEventArgs e)
		{
			init();
		}

		private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Point posicion_clic = Mouse.GetPosition(canvas);

			agregarNodo((int)posicion_clic.X, (int)posicion_clic.Y);
		}

		private Dictionary<string, double> calcularDistancias(Node nodo_final)
		{
			Dictionary<string, double> distancias = new Dictionary<string, double>();
			int num_nodos = nodos.Count;
			double distancia;

			for(int i = 0; i < num_nodos; i++)
			{
				distancia = getDistancia(nodos[i], nodo_final);
				distancias.Add(nodos[i].Name, distancia);
			}

			return distancias;
		}

		private double getDistancia(Node A, Node B)
		{
			return Math.Sqrt(Math.Pow(B.X - A.X, 2.0)+Math.Pow(B.Y - A.Y, 2.0));
		}

		private void btnVerDistancias_Click(object sender, RoutedEventArgs e)
		{
			string str_res = "Diastancias a Nodo: " + cb_nodo_final.SelectedItem.ToString() + "\n";

			if(cb_nodo_final.SelectedItem != null)
			{
				Dictionary<string, double> distancias = calcularDistancias(getNodoFromList(cb_nodo_final.SelectedItem.ToString()));
				foreach(KeyValuePair<string, double> info in distancias)
				{
					str_res += "[ " + info.Key + " ] => " + info.Value + "\n";
				}
				MessageBox.Show(str_res);
			}
			else
			{
				MessageBox.Show("Debe de seleccionar un Nodo Final");
			}
		}

		private Node getNodoFromList(string str_nodo)
		{
			int num_nodos = nodos.Count;
			for(int i = 0; i < num_nodos; i++)
			{
				if (nodos[i].Name == str_nodo)
					return nodos[i];
			}
			return null;
		}

		private void btnBuscar_Click(object sender, RoutedEventArgs e)
		{
			int limite = 0;
			Dictionary<string, string> resultado = new Dictionary<string, string>();
			try { limite = Int32.Parse(tb_limite.Text); }
			catch (Exception) { MessageBox.Show("ingrese un número."); }
			if (cb_metodo_de_busqueda.SelectedItem != null && cb_nodo_incial.SelectedItem != null && cb_nodo_final.SelectedItem != null)
			{
				int metodo_de_busqueda = cb_metodo_de_busqueda.SelectedIndex;
				string str_nodo_ini = cb_nodo_incial.SelectedItem.ToString();
				string str_nodo_final = cb_nodo_final.SelectedItem.ToString();

				switch (metodo_de_busqueda)
				{
					case PREFERENTE_POR_AMPLITUD:
						resultado = AlgoritmosDeBusqueda.busquedaPreferentePorAmplitud(grafo, str_nodo_ini, str_nodo_final);
						break;
					case COSTO_UNIFORME:
						resultado = AlgoritmosDeBusqueda.busquedaPorCostoUniforme(grafo, str_nodo_ini, str_nodo_final);
						break;
					case PREFERENTE_POR_PROFUNDIDAD:
						resultado = AlgoritmosDeBusqueda.busquedaPreferentePorProfundidad(grafo, str_nodo_ini, str_nodo_final);
						break;
					case LIMITADA_EN_PROFUNDIDAD:
						resultado = AlgoritmosDeBusqueda.busquedaLimitadaEnProfundidad(grafo, limite, str_nodo_ini, str_nodo_final);
						break;
					case PROFUNDIZACION_ITERATIVA:
						resultado = AlgoritmosDeBusqueda.busquedaPorProfundizacionIterativa(grafo, str_nodo_ini, str_nodo_final);
						break;
					case AASTERISCO:
						resultado = AlgoritmosDeBusqueda.busquedaAAsterisco(grafo, nodos, str_nodo_ini, str_nodo_final);
						break;
					case AVARA:
						resultado = AlgoritmosDeBusqueda.busquedaAvara(grafo, nodos, str_nodo_ini, str_nodo_final);
						break;
					case AASTERISCOCONPROFUNDIZACIONITERATIVA:
						resultado = AlgoritmosDeBusqueda.busquedaAAsteriscoConProfundidadIterativa(grafo, nodos, str_nodo_ini, str_nodo_final);
						VentanaDeLimites ventana_de_limites = new VentanaDeLimites(resultado["limites"]);
						ventana_de_limites.Show();
						break;
				}
				txtNodosVisitados.Text = resultado["nodos_visitados"];
				txtRuta.Text = resultado["ruta"];
			}
			else
			{
				MessageBox.Show("Faltan campos por seleccionar...");
			}
		}
    }
}
