using System.Linq;
using System.Windows;

namespace NMoneys.Demo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			lstCurrencies.DataContext = Currency.FindAll().Select(c => new RowModel(c));
		}
	}
}
