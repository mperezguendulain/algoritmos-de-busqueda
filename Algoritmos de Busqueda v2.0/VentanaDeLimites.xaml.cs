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
using System.Windows.Shapes;

namespace Algoritmos_de_Busqueda_v2._0
{
	/// <summary>
	/// Lógica de interacción para VentanaDeLimites.xaml
	/// </summary>
	public partial class VentanaDeLimites : Window
	{
		public VentanaDeLimites(string limites)
		{
			InitializeComponent();
			this.tbLimites.Text = limites;
		}
	}
}
