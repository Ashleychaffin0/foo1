// #define DBG_EXISTING_URL

using System;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace SaveBrowserTabLinks {
	public partial class SaveBrowserTabLinks : Form {
		string SavedLinksDir;

//---------------------------------------------------------------------------------------

		public SaveBrowserTabLinks() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void BtnSave_Click(object sender, EventArgs e) {
			string DesktopDir    = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			SavedLinksDir        = Path.Combine(DesktopDir, "$Saved Links");
			string BaseDir       = Path.Combine(SavedLinksDir, DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss"));
			if (ChkChrome.Checked) SaveChrome(BaseDir);
			if (ChkEdge.Checked)   SaveEdge(BaseDir);	
			// TODO: Support FireFox, I suppose
			}

//---------------------------------------------------------------------------------------

		private void SaveChrome(string BaseDir) {
			string ChromeDir = Path.Combine(BaseDir, "Chrome");
			foreach (var item in ChromeTabInfo.Tabs("chrome")) {
				// This Url might already be in $Saved Links. If so, don't add it again
				if (ExistingUrl(item)) continue;
#if true
			MessageBox.Show("Save DISABLED!");
#else
				Directory.CreateDirectory(ChromeDir);
				ChromeTabInfo.CreateShortcutToUrl(ChromeDir, item.Title, item.Url);
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private void SaveEdge(string baseDir) {
			// MessageBox.Show("Nonce on Microsoft Edge");
			// TODO: Basically same code as SaveChrome. Consolidate.
			string EdgeDir = Path.Combine(baseDir, "Edge");
			foreach (var item in ChromeTabInfo.Tabs("msedge")) {
				// This Url might already be in $Saved Links. If so, don't add it again
				if (ExistingUrl(item)) continue;
#if true
			MessageBox.Show("Save DISABLED!");
#else
				Directory.CreateDirectory(ChromeDir);
				ChromeTabInfo.CreateShortcutToUrl(ChromeDir, item.Title, item.Url);
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private bool ExistingUrl((string Title, string Url) item) {
			string TargetUrl = HttpUtility.HtmlDecode(item.Url).Replace("%20", " ");
#if DBG_EXISTING_URL
			Console.WriteLine($"\r\nLooking for URL <{TargetUrl}>");
#endif
			var Dirs = Directory.EnumerateFiles(SavedLinksDir, "*.url", SearchOption.AllDirectories);
			foreach (var filename in Dirs) {
#if true || DBG_EXISTING_URL
				Console.WriteLine($"\tin {filename}");
#endif
				string txt     = File.ReadAllText(filename);
				int ix         = txt.IndexOf("\r\nURL=");
				string LinkUrl = HttpUtility.HtmlDecode(txt.Substring(ix + 6).Trim());
#if DBG_EXISTING_URL
				Console.WriteLine("\t\tCompare <" + TargetUrl + ">");
				Console.WriteLine("\t\t   to   <" + LinkUrl + ">");
#endif
				if (LinkUrl == TargetUrl) {
#if DBG_EXISTING_URL
					Console.WriteLine("***** Found it! *****");
#endif
					return true;
				}

			}
#if DBG_EXISTING_URL
			Console.WriteLine($"Return false - {item.Url}\r\n");
#endif
			return false;
		}
	}
}
