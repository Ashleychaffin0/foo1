using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Boondog2014_X;

using Xamarin.Forms;

namespace TestCore2_1 {
	public partial class App : Application {
		public static MainPage		BogMain;
		public static BogBoard		Board;
		public static Boondog2014   BogForm;

//---------------------------------------------------------------------------------------

		public App() {
			InitializeComponent();

			BogMain  = new MainPage();
			MainPage = BogMain;
			Board    = new BogBoard();
			BogForm  = new Boondog2014();
		}

//---------------------------------------------------------------------------------------

		protected override void  OnStart() {
			// Handle when your app starts
			var pers   = Environment.SpecialFolder.Personal;
			string dir = Environment.GetFolderPath(pers);

			DownloadFile(dir, "ProgramWords.DICT");
			DownloadFile(dir, "UserWords.DICT");

			LoadFile(dir, "ProgramWords.DICT", ref Board.PgmDict);
			LoadFile(dir, "UserWords.DICT", ref Board.UserDict);

			Board.PgmWords  = new List<string>();
			Board.UserWords = new List<string>();
		}

//---------------------------------------------------------------------------------------

		// If file not on device, download it from lrs5.net
		private static void DownloadFile(string dir, string FileName) {
			string fn = Path.Combine(dir, FileName);
			if (!File.Exists(fn)) {
				using (var wc = new WebClient()) {
					wc.DownloadFile(new Uri($"http://lrs5.net/{FileName}"), fn);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void LoadFile(string dir, string fname, ref BoondogDict dict) {
			try {
				dict = new BoondogDict(Path.Combine(dir, fname));
			} catch (Exception ex) {
				Board.ErrorMessage = ex.Message;
				System.Diagnostics.Debugger.Break();
			}
		}

//---------------------------------------------------------------------------------------

		protected override void OnSleep() {
			// Handle when your app sleeps
		}

//---------------------------------------------------------------------------------------

		protected override void OnResume() {
			// Handle when your app resumes
		}
	}
}
