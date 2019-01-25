

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using WMFSDKWrapper;
using WMFSDKSample;

namespace FixWmaTags {
	public partial class Form1 : Form {
		readonly string BasePath;

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();

			// TODO: Get base path out of registry somehow
			BasePath = @"C:\$ Zune Master";
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			string	file = "";
			ushort	stream = 0;
			string	path = "";
			string	x;
			x = "C";

			if (x == "A") {
				path = Path.Combine(BasePath, @"Various Artists\70 Ounces of Gold");
				file = Path.Combine(path, "1 Happy Organ.wma");
			}

			if (x == "B") {
				path = Path.Combine(BasePath, @"Arthur Fiedler & the Boston Pops\White Christmas- A Christmas Festival [1970]");
				file = Path.Combine(path, "1 A Christmas Festival  Joy to the World Deck the Halls God Rest Ye ....wma");
			}

			if (x == "C") {
				path = Path.Combine(BasePath, @"Various Artists\16 Most Requested Songs of the 1950's, Vol. 1");
				file = Path.Combine(path, "1 My Heart Cries for You.wma");
			}

			ProcessFile(file, stream);
		}

//---------------------------------------------------------------------------------------

		private static void ProcessFile(string file, ushort stream) {

			MetadataEdit Editor = new MetadataEdit();

			Console.WriteLine(file);
			Console.WriteLine("ShowAttributes");
			Editor.ShowAttributes(file, stream);
			Console.WriteLine();

			Console.WriteLine("ShowAttributes3");
			Editor.ShowAttributes3(file, stream);
			Console.WriteLine();
		}
	}
}