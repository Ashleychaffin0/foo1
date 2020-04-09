using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestBrowserUpload {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			webBrowser1.Navigate("www.leadslightning.com");
		}

		private void button1_Click(object sender, EventArgs e) {
			HtmlElement elem = webBrowser1.Document.GetElementById("ctl00_MainContent_FileUploadMapFile");
			elem.Focus();
			elem.InvokeMember("Click");
			System.Threading.Thread.Sleep(2000);
			SendKeys.Send(@"C:\foo.txt{Enter}");
		}
	}
}