using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace SaveBrowserTabLinks {
	public static class AutomationElementExtensions {
		public static AutomationElement GetUrlBar(this AutomationElement element) {
			try {
				Return InternalGetUrlBar(element);
				Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
				AutomationElement elmNewTab = element.FindFirst(TreeScope.Descendants, condNewTab);
				var all = element.FindAll(TreeScope.Descendants, condNewTab);
				// Get the tabstrip by getting the parent of the 'new tab' button 
				TreeWalker treewalker = TreeWalker.ControlViewWalker;
				AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);
				return elmTabStrip;
			} catch {
				// Chrome has probably changed something, and above walking needs to be modified. :(
				// put an assertion here or something to make sure you don't miss it
				System.Diagnostics.Debugger.Break();
				return null;
			}
		}

		public static string TryGetValue(this AutomationElement urlBar, AutomationPattern[] patterns) {
			try {
				return ((ValuePattern)urlBar.GetCurrentPattern(patterns[0])).Current.Value;
			} catch {
				return "";
			}
		}

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
