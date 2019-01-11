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

// See http://www.sourcebank.com/dotnet/Article/34644

namespace RichTextBox_1 {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		Brush			CurBrush;
		IInputElement	CurIie;
		HashSet<KeyValuePair<string, string>> hs;
		RunLRS				PrevRunLRS;

		public MainWindow() {
			InitializeComponent();

			var fd = new FlowDocument();
			var para1 = new Paragraph();
			para1.Inlines.Add(new RunLRS("Hello "));
			para1.Inlines.Add(new RunLRS("[[First Name]]"));
			para1.Inlines.Add(new RunLRS(","));
			fd.Blocks.Add(para1);
			var para2 = new Paragraph();
			var RunLRS1 = new RunLRS("I am a flow document. Would you like to edit me? ");
			para2.Inlines.Add(RunLRS1);

			para2.Inlines.Add(new Bold(new RunLRS("Go ahead.")));
			fd.Blocks.Add(para2);
			fd.Blocks.Add(new Paragraph(new RunLRS("Paragraph 1")));
			fd.Blocks.Add(new Paragraph(new RunLRS("Paragraph 2")));
			fd.Blocks.Add(new Paragraph(new RunLRS("Paragraph 3")));
			// Add the paragraph to blocks of paragraph
			rt1.Document = fd;
			para2.Inlines.ElementAt(1).Foreground = Brushes.Red;

			fd.MouseUp += new MouseButtonEventHandler(fd_MouseUp);
			fd.MouseEnter += new MouseEventHandler(fd_MouseEnter);
			fd.MouseLeave += new MouseEventHandler(fd_MouseLeave);

			TextPointer tp;
			TextPointerContext tpc;
			// this.WindowState = System.Windows.WindowState.Maximized;

			hs = new HashSet<KeyValuePair<string, string>>();
		}

//---------------------------------------------------------------------------------------

		void fd_MouseEnter(object sender, MouseEventArgs e) {
			var te = e.MouseDevice.DirectlyOver as IInputElement;
			CurIie = te;
			var typename = te.GetType().Name;
			msg("+++++Mouse Enter - {0}", typename);
			switch (typename) {
			case "RunLRS":
				var ter = te as RunLRS;
				CurBrush = ter.Background;
				ter.Background = Brushes.Yellow;
				break;
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		void fd_MouseLeave(object sender, MouseEventArgs e) {
			var te = e.MouseDevice.DirectlyOver as IInputElement;
			if (te == null)
				return;
			msg("-----Mouse Leave - {0}", te.GetType().Name);
			// te.Background = CurBrush;
		}

//---------------------------------------------------------------------------------------

		void fd_MouseUp(object sender, MouseButtonEventArgs e) {
			//DumpFd();
			// (e.MouseDevice.DirectlyOver as RunLRS).Background = Brushes.Yellow;
		}

//---------------------------------------------------------------------------------------

		private void Bold_Click(object sender, RoutedEventArgs e) {
			rt1.Selection.ApplyPropertyValue(FlowDocument.FontWeightProperty, FontWeights.Bold);
		}
	}
}
