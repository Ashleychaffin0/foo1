using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

// https://alexandrebrisebois.wordpress.com/2015/11/18/download-microsoft-connect-2015-sessions-using-powershell/
// http://www.codeproject.com/Articles/820669/How-to-Parse-RSS-Feeds-in-NET

namespace DownloadConnect2015 {
	public partial class DownloadConnect2015 : Form {
		public DownloadConnect2015() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			var filename = @"D:\Downloads\Video\Connect 2015\rss.xml";
			var doc = new XmlDocument();
			doc.Load(filename);
			var items = doc.SelectNodes("rss/channel/item");
		}
	}
}
