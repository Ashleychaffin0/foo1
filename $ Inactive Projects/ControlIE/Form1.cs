using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlIE {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			web.Navigate(txtURL.Text);
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			// sender as WebBrowser
			HtmlElement	tag = web.Document.GetElementById(txtTextName.Text);
			if (tag == null) {
				MessageBox.Show("No element with that ID");
				return;
			}
			tag.InnerText = txtText.Text;

			HtmlElementCollection links = web.Document.Links;
			foreach (HtmlElement elem in links) {
				Console.WriteLine("Link name={0}, Text={1}", elem.Name, elem.InnerText);
				if (elem.InnerText == txtButtonText.Text) {
					elem.InvokeMember("Click");
				}
			}
		}
	}
}