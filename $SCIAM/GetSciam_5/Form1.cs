using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSciam_5 {
	public partial class GetSciam_5 : Form {

		string Target = "http://www.scientificamerican.com/index.cfm/_api/render/file/?method=attachment&fileID=F221B334-E7FF-4A72-87B023B085ED0BF4";

		public GetSciam_5() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			var wc = new WebClient();
			wc.DownloadFile(Target, @"D:\LRS\foo.pdf");
			MessageBox.Show("Done");
		}
	}
}
