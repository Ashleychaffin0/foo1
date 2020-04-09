using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Automation;
using System.Windows.Forms;

// See https://www.codeproject.com/articles/33049/wpf-ui-automation
// See https://docs.microsoft.com/en-us/microsoft-edge/extensions/api-support/supported-apis

namespace SaveBrowserTabLinks {

	class ChromeTabInfo {
		// THis class (or at least the <Tabs> method) returns an IEnumerable with one
		// entry for each tab in the currently (see note below) running instance of
		// Chrome. Each entry is a tuple with the name of the tab and the corresponding
		// URL.
		// Note 1: This routine currently doesn't handle the possibility of more than one
		//		   instance of Chrome running.
		// Note 2: We communicate with Chrome using a combination of the Automation
		//		   classes and SendKeys. If/when I understand Automation better, maybe I
		//		   can do without SendKeys. Until then...

		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

//---------------------------------------------------------------------------------------

		public static IEnumerable<(string Title, string Url)> Tabs(string exeName) {
			var qryTabs = from proc in Process.GetProcessesByName(exeName)
						  where proc.MainWindowHandle != IntPtr.Zero
						  select AutomationElement.FromHandle(proc.MainWindowHandle);

			var root = qryTabs.FirstOrDefault();
			if (root == null) {
				// MessageBox.Show("Chrome not running");
				yield break;
			}

			var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
			var hWnd = new IntPtr(root.Current.NativeWindowHandle);
			var TabBar = root.GetUrlBar();
			var tabs = TabBar.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem));
			SendText("^1", hWnd);

			foreach (var tab in tabs) {
				// Note: I've found (the hard way) that the order of the elements in
				//		 <tabs> is *NOT* necessarily that of the tabs displayed in the
				//		 browser. Maybe this is the result of manually reordering
				//		 (dragging) tabs. Or not. Regardless, I'm going to do my own
				//		 scraping. Maybe some day I'll figure out System.Windows.Automation
				//		 so that I can do all this without SendKeys. But until then I'll
				//		 iterate through <tabs>, but this is only to know how many tabs
				//		 there are; I don't otherwise use <tab>.
				int TitleLength = GetWindowTextLength(new HandleRef(SearchBar, hWnd)) + 1;
				var sbTitle     = new StringBuilder(TitleLength);
				GetWindowText(new HandleRef(sbTitle, hWnd), sbTitle, TitleLength);
				string Title      = sbTitle.ToString();
				int ix            = Title.LastIndexOf(" - Google Chrome");
				if (ix > 0) Title = Title.Substring(0, ix);
				string Url        = (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty);
				SendText("^{TAB}", hWnd);
				yield return (Title, Url);
			}
		}

//---------------------------------------------------------------------------------------

		private static void SendText(string text, IntPtr hWnd) {
			SetForegroundWindow(hWnd);
			SendKeys.SendWait(text);
			System.Threading.Thread.Sleep(200);
		}

//---------------------------------------------------------------------------------------

		internal static void CreateShortcutToUrl(string InThisDirectory, 
			string Title,
			string Url) {
			// Note: This routine is almost identical to CreateShortcutToFile.
			string PageShortCut = $@"[InternetShortCut]
URL={Url}
";
			PageShortCut = HttpUtility.HtmlEncode(PageShortCut).Replace(" ", "%20");
			string Filename = Path.Combine(InThisDirectory, TrimAndCleanFilename(Title)) + ".url";
			WritePageShortcut(PageShortCut, Filename);
		}

//---------------------------------------------------------------------------------------

		internal static void CreateShortcutToFile(
				// See http://www.fmtz.com/formats/url-file-format/article
				string InThisDirectory,
				string RefersToThatDirectory,
				string Title) {
			string PageShortCut = $@"[InternetShortCut]
	URL=file:///{RefersToThatDirectory}/{Title}
	";
			PageShortCut = PageShortCut.Replace('\\', '/').Replace(" ", "%20");
			string Filename = Path.Combine(InThisDirectory, TrimAndCleanFilename($"{Title}") + ".url");
			WritePageShortcut(PageShortCut, Filename);
		}

		//---------------------------------------------------------------------------------------

		internal static string TrimAndCleanFilename(string FullFilename) {
			const int MAXPATHLENGTH = 260 - 4;  // Use GetVolumeInformation if you want
												// Bit of a kludge above. In some cases we want to append ".url" to a
												// filename. So we've cut down the nominal 260 to 256.
			FullFilename = Clean(FullFilename);
			int len = FullFilename.Length;
			if (len < MAXPATHLENGTH) {
				return FullFilename;
			}
			string fn = Path.GetFileNameWithoutExtension(FullFilename);
			int SurroundingCharsLength = len - fn.Length;
			int MaxFnLength = MAXPATHLENGTH - SurroundingCharsLength;
			fn = fn.Substring(0, MaxFnLength);
			// Put things back together
			// Microsoft bug: Path.GetDirectoryName fails on a too-long pathname, even
			// if all it wants to return is the Directory part. We'll have to do it
			// ourselves
			// string Dirname = Path.GetDirectoryName(FullFilename);
			int ix = FullFilename.LastIndexOf('\\');
			string Dirname;
			if (ix == -1) {
				Dirname = FullFilename;
			} else {
				Dirname = FullFilename.Substring(0, ix);
			}
			string Ext     = Path.GetExtension(FullFilename);
			fn             = Path.Combine(Dirname, fn) + "." + Ext;
			return fn;
		}

//---------------------------------------------------------------------------------------

		private static string Clean(string Filename) {
			Filename        = Filename.Replace(":", " --")
				.Replace("?", "$Q");
			foreach (var c in Path.GetInvalidFileNameChars()) {
				Filename = Filename.Replace(c, '-');
			}
			return Filename;
		}

//---------------------------------------------------------------------------------------

		internal static void WritePageShortcut(string PageShortCut, string Filename) {
			if (!File.Exists(Filename)) {
				using (StreamWriter sw = File.CreateText(Filename)) {
					sw.Write(PageShortCut);
				}
			}
		}
	}
}
