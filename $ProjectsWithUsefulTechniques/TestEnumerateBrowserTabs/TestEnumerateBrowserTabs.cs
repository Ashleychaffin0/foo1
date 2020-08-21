using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

// See https://stackoverflow.com/questions/31967430/microsoft-edge-get-window-url-and-title
// See https://www.codeproject.com/articles/33049/wpf-ui-automation
// See https://stackoverflow.com/questions/32344691/get-urls-of-pages-opened-in-ms-edge-browser-is-not-working-in-windows-10-home-ed

namespace TestEnumerateBrowserTabs {
	public partial class TestEnumerateBrowserTabs : Form {

		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetDesktopWindow();

		public TestEnumerateBrowserTabs() {

			InitializeComponent();

			// Console.WriteLine("foo"); foo();
			// Console.WriteLine("goo"); goo();
			Console.WriteLine("hoo"); hoo();
			// Console.WriteLine("loo"); loo();
			// Console.WriteLine("noo"); noo();
		}

//---------------------------------------------------------------------------------------

		private void noo() {
			var main = AutomationElement.FromHandle(new IntPtr(GetDesktopWindow()));
			foreach (AutomationElement child in main.FindAll(TreeScope.Children, PropertyCondition.TrueCondition)) {
				try {
					var window = GetEdgeCommandsWindow(child);
					if (window == null) // not Edge
						continue;

					DumpElement(window);
					string Url = GetEdgeUrl(window);
					var hWnd = new IntPtr(child.Current.NativeWindowHandle);
					// string Title = GetEdgeTitle(window);
					// int TitleLength = GetWindowTextLength(new HandleRef(SearchBar, hWnd)) + 1;
					int TitleLength = GetWindowTextLength(new HandleRef(window, hWnd)) + 1;
					var sbTitle = new StringBuilder(TitleLength);
					GetWindowText(new HandleRef(sbTitle, hWnd), sbTitle, TitleLength);
					String Title = sbTitle.ToString();
				} catch (Exception ex) {
					;	// Do nothing, i.e. continue
				}

			}
		}

//---------------------------------------------------------------------------------------

	public static AutomationElement GetEdgeCommandsWindow(AutomationElement edgeWindow) {
		return edgeWindow.FindFirst(TreeScope.Children, new AndCondition(
			new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
			new PropertyCondition(AutomationElement.NameProperty, "Microsoft Edge")));
	}

//---------------------------------------------------------------------------------------

	public static string GetEdgeUrl(AutomationElement edgeCommandsWindow) {
		var addressEditBox = edgeCommandsWindow.FindFirst(TreeScope.Children,
			new PropertyCondition(AutomationElement.AutomationIdProperty, "addressEditBox"));

		return ((TextPattern)addressEditBox.GetCurrentPattern(TextPattern.Pattern)).DocumentRange.GetText(int.MaxValue);
	}

//---------------------------------------------------------------------------------------

		public static string GetEdgeTitle(AutomationElement edgeWindow) {
			var addressEditBox = edgeWindow.FindFirst(TreeScope.Children,
				new PropertyCondition(AutomationElement.AutomationIdProperty, "m_tabContentDCompVisualElement"));
				// new PropertyCondition(AutomationElement.AutomationIdProperty, "TitleBar"));

			return addressEditBox.Current.Name;
		}

//---------------------------------------------------------------------------------------

		private void loo() {
			// var p = Process.GetProcessesByName("MicrosoftEdgeCP");
			var pAll = Process.GetProcesses();
			var pEdge = Process.GetProcessesByName("MicrosoftEdge");
			var elems = from proc in Process.GetProcessesByName("MicrosoftEdgeCP")
							// var elems = from proc in Process.GetProcessesByName("MicrosoftEdge")
							// where proc.MainWindowTitle == "Microsoft Edge"
							// select proc;
						select AutomationElement.FromHandle(proc.MainWindowHandle);

			foreach (var elem in elems) {
				var elemKids = elem.FindAll(TreeScope.Subtree, Condition.TrueCondition);
				foreach (var kid in elemKids) {

				}
			}

			var root = elems.FirstOrDefault();

			AutomationElement rootElement = AutomationElement.RootElement;
			// var yyy = root.FindAll(TreeScope.Subtree, Condition.TrueCondition);

			DumpElement(rootElement);

			var xxx = root.FindFirst(TreeScope.Children, new AndCondition(
		new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
		new PropertyCondition(AutomationElement.NameProperty, "Microsoft Edge")));


#if false
			foreach (var item in ChromeTabInfo.Tabs()) {
				Console.WriteLine($"\r\n{item.Title}\r\n\t{item.Url}");
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private static void DumpElement(AutomationElement rootElement) {
			var zzz = rootElement.FindAll(TreeScope.Subtree, Condition.TrueCondition);
			int n = 0;
			string nl = "";
			foreach (AutomationElement elem in zzz) {
				try {
					++n;
					var p = Process.GetProcessById(elem.Current.ProcessId);
					Console.WriteLine($"{nl}[{n,4}] Process: {p.ProcessName} (ID = {p.Id})");
					if (!p.ProcessName.Contains("Edge")) continue;
					nl = "\r\n";
					Console.WriteLine($"\tAcceleratorKey:         {elem.Current.AcceleratorKey}");
					Console.WriteLine($"\tAutomationId:           {elem.Current.AutomationId}");
					Console.WriteLine($"\tClass Name:             {elem.Current.ClassName}");
					Console.WriteLine($"\tControlType:            {elem.Current.ControlType}");
					Console.WriteLine($"\tFramework Id:           {elem.Current.FrameworkId}");
					Console.WriteLine($"\tHelp Text:              {elem.Current.HelpText}");
					Console.WriteLine($"\tLocalized Control Type: {elem.Current.LocalizedControlType}");
					Console.WriteLine($"\tName:                   {elem.Current.Name}");
				} catch (Exception) {
					Console.WriteLine($"[{n,4}] -----------------");
					continue;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void hoo() {
			// As far as I can tell, the Chrome window consists (logically) of the tab
			// bar and the main window. But the main window is actually owned by a 
			// different Windows process. So at the moment I have 37 tabs open, and 
			// there are 41 Chrome processes running. Anyway, if you click a different
			// tab, then what happens (again, logically, but maybe even physically),
			// the child window of the main window will be hidden and the child window
			// for that tab will be displayed. (But this all happens so fast that it's
			// seamless.) So our approach will be as follows.
			//	*	Get the list of Chrome processes (which may be empty if Chrome isn't
			//		running). Filter the list so we see the process that has a non-zero
			//		MainWindowHandle
			// TODO: Continue commenting
			// Need list of tabs in tab bar, 
			var qryTabs = from proc in Process.GetProcessesByName("chrome")
						  where proc.MainWindowHandle != IntPtr.Zero
						  select AutomationElement.FromHandle(proc.MainWindowHandle);
			var root = qryTabs.FirstOrDefault();
			if (root == null) {
				MessageBox.Show("Chrome not running");
				// TODO: Return some flavor of null/false
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
				int TitleLength = GetWindowTextLength(new HandleRef(SearchBar, hWnd));
				var sbTitle = new StringBuilder(TitleLength);
				GetWindowText(new HandleRef(sbTitle, hWnd), sbTitle, TitleLength);
				Console.WriteLine("tab3 -- " + sbTitle.ToString());
				Console.WriteLine("        MAGIC: " + (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
				SendText("^{TAB}", hWnd);
			}
		}

//---------------------------------------------------------------------------------------

		private static void SendText(string text, IntPtr hWnd) {
			SetForegroundWindow(hWnd);
			SendKeys.SendWait(text);
			System.Threading.Thread.Sleep(200);
		}

//---------------------------------------------------------------------------------------

		private void goo() {
			foreach (var Url in GetTabs()) {
				Console.WriteLine(Url);
			}
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<string> GetTabs() {
			// From https://stackoverflow.com/questions/18897070/getting-the-current-tabs-url-from-google-chrome-using-c-sharp

			// there are always multiple chrome processes, so we have to loop through all of them to find the
			// process with a Window Handle and an automation element of name "Address and search bar"
			var processes = Process.GetProcessesByName("chrome");
			var automationElements = from chrome in processes
									 where chrome.MainWindowHandle != IntPtr.Zero
									 select AutomationElement.FromHandle(chrome.MainWindowHandle);

			var qryFindChromeHwnd = from chrome in processes
									where chrome.MainWindowHandle != IntPtr.Zero
									select chrome.MainWindowHandle;
#if true
			var root = automationElements.First();
			var hWnd = qryFindChromeHwnd.FirstOrDefault(); ;   // IntPtr
			SetForegroundWindow(hWnd);
			SendKeys.SendWait("^1");

			var SearchBar2 = root.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
			foreach (AutomationElement item in SearchBar2) {
				Console.WriteLine($"Searchbar: {item}");
			}
#endif

			Console.WriteLine("\r\n********************\r\n");

			var tabs = from element in automationElements
					   select element.GetUrlBar()
					   into elmUrlBar
					   where elmUrlBar != null
					   where !((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
					   let tabitems = elmUrlBar.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem))
					   let patterns = elmUrlBar.GetSupportedPatterns()
					   /*
					   where patterns.Length == 1
					   // select elmUrlBar.TryGetValue(patterns)
					   select elmUrlBar.GetCurrentPattern(patterns[0])
					   into ret
					   where ret != ""
					   where Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$")
					   select ret.StartsWith("http") ? ret : "http://" + ret;
					   */
					   select new { elmUrlBar, patterns, tabitems }
					 // select new { tabitems }
					 ;

			var info = from tab in tabs
					   select tab
					   ;

			// Only 1 SearchBar
			var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
			foreach (var collection in info) {
				foreach (AutomationElement tab in collection.tabitems) {
					// (tab.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern).Invoke();
					// tab.SetFocus();
					Console.WriteLine();
					Console.WriteLine("tab  -- " + tab.Current.Name);
					// Console.WriteLine("tab2 --  " + (string)tab.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));

					var qryFindChromeHwnd2 = from chrome in processes
											 where chrome.MainWindowHandle != IntPtr.Zero
											 select chrome.MainWindowHandle;
					var hWnd2 = qryFindChromeHwnd2.FirstOrDefault(); ;   // IntPtr

					var sbTitle = new StringBuilder(1000);
					GetWindowText(new HandleRef(tab, hWnd2), sbTitle, 1000);
					Console.WriteLine("tab2 -- " + sbTitle.ToString());

					// Console.WriteLine($"Searchbar: {tab.TryGetValue(collection.patterns)}, ");
#if true
					Console.WriteLine("        MAGIC: " + (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
					SendKeys.SendWait("^{TAB}");
					System.Threading.Thread.Sleep(200);

					// foreach (AutomationElement item in SearchBar) {
						// Console.WriteLine($"Searchbar: {item.TryGetValue(collection.patterns)}, ");
					// }
#endif
				}
			}



			foreach (AutomationElement RootElem in automationElements) {
				var elmUrlBar = RootElem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
				if (elmUrlBar != null) {
					var patterns = elmUrlBar.GetSupportedPatterns();
					if (patterns.Length > 0) {
						var valpat = elmUrlBar.GetCurrentPattern(patterns[0]) as ValuePattern;
						var HasFocus = (bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty);
						if (!HasFocus) {
							string url = valpat.Current.Value;
							Console.WriteLine($"{RootElem.Current.Name} => {url}");
						} else {
							continue;
						}

					}
				}
			}

			return from element in automationElements
				   select element.GetUrlBar()
				   into elmUrlBar
				   where elmUrlBar != null
				   where !((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
				   let patterns = elmUrlBar.GetSupportedPatterns()
				   where patterns.Length == 1
				   select elmUrlBar.TryGetValue(patterns)
				   into ret
				   where ret != ""
				   where Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$")
				   select ret.StartsWith("http") ? ret : "http://" + ret;
		}

//---------------------------------------------------------------------------------------

		private void foo() {
			Process[] procsChrome = Process.GetProcessesByName("chrome");
			if (procsChrome.Length <= 0) {
				Console.WriteLine("Chrome is not running");
			} else {
				foreach (Process proc in procsChrome) {
					// the chrome process must have a window 
					if (proc.MainWindowHandle == IntPtr.Zero) {
						continue;
					}
					// From https://stackoverflow.com/questions/40070703/how-to-get-a-list-of-open-tabs-from-chrome-c-sharp
					// to find the tabs we first need to locate something reliable - the 'New Tab' button 
					AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
					Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
					AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);
					// get the tabstrip by getting the parent of the 'new tab' button 
					TreeWalker treewalker = TreeWalker.ControlViewWalker;
					AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
					// loop through all the tabs and get the names which is the page title 
					Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
					foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem)) {
						Console.WriteLine(tabitem.Current.Name);
					}
#if true
					Condition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
					var tabs = root.FindAll(TreeScope.Descendants, condition);
					foreach (var t in tabs) {

					}

#if true
					AutomationElement SearchBar = null;
					SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
					if (SearchBar != null) {
						/********** Magic Line Follows *************/
						Console.WriteLine((string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
					} else {
						Console.WriteLine("SearchBar == null");
					}
#endif
					var SearchBar2 = root.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
					foreach (AutomationElement item in SearchBar2) {
						Console.WriteLine($"Searchbar: {item}");
					}
				}

#endif
			}
		}

	}



	public static class AutomationElementExtensions {

//---------------------------------------------------------------------------------------

		public static AutomationElement GetUrlBar(this AutomationElement element) {
			try {
				// return InternalGetUrlBar(element);
				Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
				AutomationElement elmNewTab = element.FindFirst(TreeScope.Descendants, condNewTab);
				// get the tabstrip by getting the parent of the 'new tab' button 
				TreeWalker treewalker = TreeWalker.ControlViewWalker;
				AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
				return elmTabStrip;
			} catch {
				// Chrome has probably changed something, and above walking needs to be modified. :(
				// put an assertion here or something to make sure you don't miss it
				return null;
			}
		}

//---------------------------------------------------------------------------------------

		public static string TryGetValue(this AutomationElement urlBar, AutomationPattern[] patterns) {
			try {
				return ((ValuePattern)urlBar.GetCurrentPattern(patterns[0])).Current.Value;
			} catch {
				return "";
			}
		}

//---------------------------------------------------------------------------------------

		private static AutomationElement InternalGetUrlBar(AutomationElement element) {
			// walking path found using inspect.exe (Windows SDK) for Chrome 29.0.1547.76 m (currently the latest stable)
			var elm1 = element.FindFirst(TreeScope.Children,
			  new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
			var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1); // I don't know a Condition for this for finding :(
			var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
			var elm4 = elm3.FindFirst(TreeScope.Children,
			  new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
			var result = elm4.FindFirst(TreeScope.Children,
			  new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));

			return result;
		}
	}
}
