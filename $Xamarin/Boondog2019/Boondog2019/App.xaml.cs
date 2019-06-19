using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using nsBoondog2019.Views;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using System.IO;

/*
https://xamgirl.com/grids-xamarin-forms-made-simple/
https://docs.microsoft.com/en-us/xamarin/tools/live-player/
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/xaml/xaml-previewer?pivots=windows
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/layouts/grid

*/

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace nsBoondog2019 {
	public partial class App : Application {
		public static MainPage BogMain;
		public static BogBoard Board;
		public static BoondogForm BogForm;

		public static string MusicDir;

//---------------------------------------------------------------------------------------

		public App() {
			InitializeComponent();

			MainPage = new MainPage();

			// TODO: string path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).ToString();
		}

//---------------------------------------------------------------------------------------

		protected override void OnStart() {
			// Handle when your app starts
			const string PgmDict = "ProgramWords.txt";
			const string UserDict = "UserWords.txt";

			BogMain  = new MainPage();		// TODO: Not MainPage, Boondog view
			MainPage = BogMain;
			Board    = new BogBoard();
			BogForm  = new BoondogForm();

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

		// If file not on device, download it from lrs5.net
		// TODO: Save/Read from disk
		// TODO: Clean this up
		private void DownloadFile(string FileName) {
			// if (!File.Exists(fn)) {
			if (!Properties.ContainsKey(FileName)) {
				// string fn = Path.Combine(dir, FileName);
				// using (var wc = new WebClient()) {
				using (var hc = new HttpClient()) {
					// wc.Headers.Add("Content-Type", "text/plain");
					string name = $"http://lrs5.net/{FileName}";
					// wc.DownloadFile(name, fn);
					var resp = hc.GetAsync(name).Result;
					if (resp.IsSuccessStatusCode) {
						var content = resp.Content.ReadAsStringAsync().Result;
						Properties[FileName] = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
						string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FileName);
						
						File.WriteAllText(fileName, content);
					} else {
						// TODO: And if not?
					}

					// string txt = wc.DownloadString(name);
					// Application.Current.Properties[FileName] = txt;
					// Properties[FileName] = txt.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				}
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
