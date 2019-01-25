using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;


namespace GetVSActivityLog {
	public partial class GetVSActiityLog : Form {
		public GetVSActiityLog() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void GetVSActiityLog_Load(object sender, EventArgs e) {
			// TODO: Find this in registry
			// TODO: Tag each item with full path/filename
			var HKCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
			var vs = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio");
			foreach (var entry in vs.GetSubKeyNames()) {
				// Console.WriteLine($"{entry}");
				var VsLoc = vs.OpenSubKey(entry).GetValue("VisualStudioLocation");
				if (VsLoc == null) {
					continue;
				}
				Console.WriteLine($"{entry} Visual Studio Location = {VsLoc}");
				var kvp = new KeyValuePair<string, object>(entry, VsLoc);
				CmbVersion.Items.Add(kvp);
			}
			CmbVersion.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void CmbVersion_SelectedIndexChanged(object sender, EventArgs e) {
			// TODO: Get filename form combo box tag
			string filename = @"C:\Users\LRS8500\AppData\Roaming\Microsoft\VisualStudio\15.0\ActivityLog.xml";
			var lines = File.ReadAllLines(filename).ToList<string>();
			// Some XML files start with "<?xml ". Which LoadXml doesn't like. Delete these
			for (int i = lines.Count - 1; i >= 0; i--) {
				if (lines[i].StartsWith("<?xml")) {
					lines.RemoveAt(i);
				}
			}
			var xml = new XmlDocument();
			xml.Load(filename);
			foreach (XmlNode elem in xml.GetElementsByTagName("entry")) {
				Console.WriteLine($"<{elem.Name}>");
				foreach (XmlElement child in elem.ChildNodes) {
					Console.WriteLine($"\t<{child.Name}>{child.InnerText}</{child.Name}>");
				}
				Console.WriteLine($"</{elem.Name}>");
				int i = 1;
			}
		}
	}
}
