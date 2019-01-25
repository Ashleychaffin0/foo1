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

namespace FindInZip {
	public partial class FindInZip : Form {
		string Filename = "*PortableDevice*";

//---------------------------------------------------------------------------------------

		public FindInZip() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void FindInZip_Load(object sender, EventArgs e) {
			this.Text = $"Find in Zip - {Filename}";
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			string StartPath = @"C:\";
			var zips = Directory.EnumerateFileSystemEntries(StartPath, "*.zip", SearchOption.AllDirectories);
			foreach (var zip in zips) {
				
			}
		}
	}
}
