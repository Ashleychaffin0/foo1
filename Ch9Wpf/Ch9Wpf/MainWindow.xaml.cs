using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

using mshtml;

namespace Ch9Wpf {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, RoutedEventArgs e) {
			tbOutput.Text = "Hello world";
			web1.Navigate("http://channel9.msdn.com/Browse/AllContent?page=2");
		}

//---------------------------------------------------------------------------------------

		private void web1_Navigated(object sender, NavigationEventArgs e) {
		}

//---------------------------------------------------------------------------------------

		private void web1_LoadCompleted(object sender, NavigationEventArgs e) {
			var w = sender as WebBrowser;
			var doc = w.Document as IHTMLDocument2;

			var doc1 = w.Document as IHTMLDocument;
			var doc2 = w.Document as IHTMLDocument2;
			var doc3 = w.Document as IHTMLDocument3;
			var doc4 = w.Document as IHTMLDocument4;
			var doc5 = w.Document as IHTMLDocument5;

			var body = doc.body;

			var divs = doc3.getElementsByTagName("DIV");

			foreach (var item in divs) {
				var div = item as IHTMLDivElement;
				Console.WriteLine("Item = {0}", item);
			}

		}
	}
}
