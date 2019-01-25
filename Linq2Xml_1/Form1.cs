using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Linq2Xml_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			string filename = @"c:\LRS\Subscriptions.xml";
			XDocument	xdoc = XDocument.Load(filename);
			// var q1 = from elem in xdoc.Root.Elements()
			var q1 = from elem in xdoc.Root.Descendants()
					 select new { elem, FeedType = elem.Name, FeedName = elem.Attribute("name") };

			int	LineNo = 2;		// Line 1 = "<?xml...", Line 2 = "<feeds..."
			foreach (var elem in q1) {
				++LineNo;
				Console.WriteLine("{0}: {1} - {2}", LineNo, elem.FeedType, elem.FeedName);
				if (elem.FeedType == "RssFeedsCategory") {	// For </RssFeedsCategory>
					++LineNo;
				}
			}
		}
	}
}
