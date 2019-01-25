using System.Windows;
using System.Windows.Input;

namespace HandleEvent {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
			var win = sender as Window;
			string msg = $"Windows clicked with {e.ChangedButton} button" +
				$" at point ({e.GetPosition(win)})";
			MessageBox.Show(msg, win.Title);
		}
	}
}
