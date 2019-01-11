// #define TEST_MODE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TrimSharpReaderCache {
	public partial class TrimSharpReaderCache : Form {

		List<XElement> NodesToBeRemoved = new List<XElement>();
		Regex re = new Regex(@"&#x[0-9A-Fa-f]+;", RegexOptions.Compiled);

		int nFeeds = 0;

//---------------------------------------------------------------------------------------

		public TrimSharpReaderCache() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Browse", "Trim SharpReaderCache");
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			SetCachePath();
			SetDefaultMonthsToKeep();
			this.WindowState = FormWindowState.Maximized;
		}

//---------------------------------------------------------------------------------------

		private void SetDefaultMonthsToKeep() {
			cbMonthsToKeep.Text = "12";
		}

//---------------------------------------------------------------------------------------

		private void SetCachePath() {
			string CachePath;
#if TEST_MODE
			CachePath = @"C:\LRS\Faux SharpReaderCache";
#else
			var AppPath = System.Environment.SpecialFolder.ApplicationData;
			CachePath = Environment.GetFolderPath(AppPath);
			CachePath = Path.Combine(CachePath, @"SharpReader\cache");
#endif
			txtCachePath.Text = CachePath;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Should have try/catch in case directory not found
			string CachePath = txtCachePath.Text;

			int months = int.Parse(cbMonthsToKeep.Text);	// TODO: try/catch
			DateTime KeepAllEntriesAfterThisDate = DateTime.Now.AddMonths(-months).Date;

			string filter;					// TODO:
			// filter = "rss.slash*.xml";
			// filter = "www.sells*.xml";
			// filter = "news*.xml";
			filter = "*.xml";
			foreach (string FileName in Directory.GetFiles(CachePath, filter)) {
				ProcessFile(FileName, KeepAllEntriesAfterThisDate);
			}

			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void ProcessFile(string FileName, DateTime KeepAllEntriesAfterThisDate) {
			Show(Path.GetFileName(FileName));
			ProcessTheXml(FileName, KeepAllEntriesAfterThisDate);
			string s = string.Format("{0:N3} ", ++nFeeds);
			Show("");
			Show(s + "------------------------------------------------------");
			Show("");
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void ProcessTheXml(string FileName, DateTime KeepAllEntriesAfterThisDate) {
			//System.Diagnostics.Process.Start("Notepad++.exe", FileName);
			string s = File.ReadAllText(FileName);
			s = CleanHexChars(s);
			XDocument xdoc = XDocument.Parse(s); // XDocument.Load(FileName);
			var RssNode = xdoc.FirstNode as XElement;
			// var CurNode = RssNode.FirstNode as XElement;		// Probably <TITLE>
			// lbMsgs.Items.Clear();
			int nItems = 0;
			int nDeleted = 0;
			for (var CurNode = RssNode.FirstNode as XElement; CurNode != null; CurNode = CurNode.NextNode as XElement) {
				if (CurNode.Name.LocalName.ToUpper() == "ITEMS") {
					++nItems;
					// See if this is an error message node. If so, ignore.
					var nError = GetNodeNamed(CurNode, "ISERRORMESSAGE");
					if ((nError != null) && (nError.Value == "true"))
						continue;
					// Get the Item title, for display purposes
					var nTitle = GetNodeNamed(CurNode, "TITLE");
					if (nTitle != null) {
						// Show("Node title = {0}", nTitle.Value);
					}
					// Get the PubDate element (if any), and check to see if it's old
					// enough to be deleted
					XElement nPubDate;
					bool bPurgeByDate = ShouldBePurgedByDate(CurNode, KeepAllEntriesAfterThisDate, out nPubDate);
					if (!bPurgeByDate)
						continue;
					// Show("       CurNode is old. Value is " + CurNode.Value);
					// Item is old enough to be purged. But don't do so if it's Flagged
					var nFlag = GetNodeNamed(CurNode, "Flag");
					// *Could* check value for empty, but not yet
					if (nFlag != null)
						continue;
					// TODO: OK, the CurNode is too old, but not Flagged. Go for it
					// Show("       Node should be deleted");
					++nDeleted;
					DeleteRssItem(CurNode);
				}
			}
			RemoveRssNodes();
			// xdoc.Save(@"C:\lrs\foorss.xml");
			xdoc.Save(FileName);
			// MessageBox.Show("Filename = " + FileName, "Items count = " + nItems + ", deleted = " + nDeleted);
		}

//---------------------------------------------------------------------------------------

		private string CleanHexChars(string s) {
			while (true) {
				if (!s.Contains(";&#x")) {
					return s;
				}
				s = re.Replace(s, "");
			}
		}

//---------------------------------------------------------------------------------------

		private void RemoveRssNodes() {
			for (int i = NodesToBeRemoved.Count - 1; i >= 0; i--) {
				XElement xNode = NodesToBeRemoved[i];
				var nTitle = GetNodeNamed(xNode, "TITLE");
				if (nTitle != null) {
					Show(1, "Removing {0}", nTitle.Value);
				}
				xNode.Remove();
			}
			NodesToBeRemoved.Clear();
		}

//---------------------------------------------------------------------------------------

		private void DeleteRssItem(XElement CurNode) {
			// CurNode.Remove();
			NodesToBeRemoved.Add(CurNode);
		}

//---------------------------------------------------------------------------------------

		private bool ShouldBePurgedByDate(XElement CurNode, DateTime KeepAllEntriesAfterThisDate, out XElement nPubDate) {
			nPubDate = GetNodeNamed(CurNode, "PubDate");
			// If we can't find its date, leave it. Shouldn't happen often, if at all
			if (nPubDate == null) 
				return false;
			string sPubDate = nPubDate.Value;
			// Show("       PubDate = {0}", sPubDate);
			DateTime PubDate;
			bool bDateOK = DateTime.TryParse(sPubDate, out PubDate);
			// If we can't parse the date. Leave it. Again, shouldn't happen often
			if (!bDateOK)
				return false;
			// OK, the big test. If KeepAllEntriesAfterThisDate is, say, 2007, and
			// PubDate is, say, 2008, then we do *not* want to purge it, so return false
			if (PubDate >= KeepAllEntriesAfterThisDate)
				return false;
			// The PubDate is too old. It's a candidate for purging. Return true.
			return true;
		}

//---------------------------------------------------------------------------------------

		private XElement GetNodeNamed(XElement CurNode, string Name) {
			var node = CurNode.FirstNode as XElement;
			while (node != null) {
				if (node.Name.LocalName.ToUpper() == Name.ToUpper()) {
					return node;
				}
				node = node.NextNode as XElement;
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		private void Show(string txt) {
			lbMsgs.Items.Add(txt);
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void Show(int nIndent, string txt) {
			Show(nIndent, "{0}", txt);
		}

//---------------------------------------------------------------------------------------

		private void Show(string fmt, params object[] parms) {
			string txt = string.Format(fmt, parms);
			Show(txt);
		}

//---------------------------------------------------------------------------------------

		private void Show(int nIndent, string fmt, params object[] parms) {
			const int IndentValue = 8;
			string txt = string.Format(fmt, parms);
			Show("".PadRight(nIndent * IndentValue) + txt);
		}
	}
}
