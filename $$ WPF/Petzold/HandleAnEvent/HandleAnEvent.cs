using System;
using System.Windows;
using System.Windows.Input;

namespace Petzold.HandleAnEvent {
	class HandleAnEvent {
		[STAThread]
		public static void Main() {
			var app = new Application();

			var win = new Window();
			win.Title = "Handle and Event";
			win.MouseDown += WindowOnMouseDown;
		}

		static void WindowOnMouseDown(object sender, MouseButtonEventArgs args) {
			var win = sender as Window;
			Point pt = args.GetPosition(win);
			string msg = $"Window clicked with {args.ChangedButton} at point {pt}";
			MessageBox.Show(msg, win.Title);
		}
	}
}
