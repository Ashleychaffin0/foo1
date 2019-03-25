#define RESET_PROPERTIES

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using BogDroid_2019_03;

using Xamarin.Forms;

namespace BogDroid_2019_03 {
	public partial class App : Application {
		public static MainPage BogMain;
		public static BogBoard Board;
		public static Boondog2014 BogForm;

//---------------------------------------------------------------------------------------

		public App() {
			InitializeComponent();

			BogMain  = new MainPage();
			MainPage = BogMain;
			Board    = new BogBoard();
			BogForm  = new Boondog2014();
		}

//---------------------------------------------------------------------------------------

		protected override void OnStart() {
			// Handle when your app starts
			const string PgmDict  = "ProgramWords.txt";
			const string UserDict = "UserWords.txt";

#if DEBUG && RESET_PROPERTIES
			Properties.Clear();
#endif

			// var pers = Environment.SpecialFolder.ApplicationData;
			// string dir = Environment.GetFolderPath(pers);

			DownloadFile(PgmDict);
			DownloadFile(UserDict);

			LoadFileFromString(PgmDict, ref Board.PgmDict);
			LoadFileFromString(UserDict, ref Board.UserDict);

			Board.PgmWords  = new List<string>();
			Board.UserWords = new List<string>();
		}

//---------------------------------------------------------------------------------------

		// If file not on device, download it from lrs5.net
		private void DownloadFile(string FileName) {
			// if (!File.Exists(fn)) {
			// var keys = Properties.Keys;		// TODO:
			if (!Properties.ContainsKey(FileName)) { 
				// string fn = Path.Combine(dir, FileName);
				using (var wc = new WebClient()) {
					wc.Headers.Add("Content-Type", "text/plain");
					string name = $"http://lrs5.net/{FileName}";
					// wc.DownloadFile(name, fn);
					string txt = wc.DownloadString(name);
					// Application.Current.Properties[FileName] = txt;
					Properties[FileName] = txt.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				}
			}
		}

//---------------------------------------------------------------------------------------

		// If file not on device, download it from lrs5.net
		private void xxxDownloadFile(string dir, string FileName) {
			string fn = Path.Combine(dir, FileName);
			if (!File.Exists(fn)) {
				using (var wc = new WebClient()) {
					wc.Headers.Add("Content-Type", "text/plain");
					string name = $"http://lrs5.net/{FileName}";
					// wc.DownloadFile(name, fn);
					string txt = wc.DownloadString(name);
					// Application.Current.Properties[FileName] = txt;
					Properties[FileName] = txt.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void xxxLoadFile(string dir, string FileName, ref BoondogDict dict) {
			try {
				dict = new BoondogDict(Path.Combine(dir, FileName));
			} catch (Exception ex) {
				Board.ErrorMessage = ex.Message;
				// Application.Current.Quit();		// TODO:
				System.Diagnostics.Debugger.Break();
			}
		}

//---------------------------------------------------------------------------------------

		private void LoadFileFromString(string dictName, ref BoondogDict dict) {
			try {
				dict = new BoondogDict(dictName, (string[])Properties[dictName]);
			} catch (Exception ex) {
				Board.ErrorMessage = ex.Message;
				// Application.Current.Quit();		// TODO:
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
