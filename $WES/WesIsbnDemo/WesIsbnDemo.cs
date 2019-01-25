using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WesIsbnDemo {
	public partial class WesIsbnDemo : Form {
		public WesIsbnDemo() {
			InitializeComponent();

			// Create columns for the items and subitems.
			// Width of -2 indicates auto-size.
			lvOtherData.Columns.Add("Name",  -2, HorizontalAlignment.Center);
			lvOtherData.Columns.Add("Value", -2, HorizontalAlignment.Left);

			//lvOtherData.HeaderStyle = ColumnHeaderStyle.
		}

		//---------------------------------------------------------------------------------------

		private void WesIsbnDemo_Load(object sender, EventArgs e) {
			// Nothing here at the moment
		}

//---------------------------------------------------------------------------------------

		private void GetBookInfo() {
			string ISBN = txtIsbn.Text.Trim();
			if (ISBN.Length == 0) {
				MessageBox.Show("Please enter an ISBN", "Wes ISBN Demo", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			var data = new IsbnLookup();
			bool bOK = data.Read(ISBN);
			if (! bOK) {
				MessageBox.Show("ISBN not found", "Wes ISBN Demo", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			lblTitle.Text     = data.Title;
			lblAuthor.Text    = data.AuthorFirstName + " " + data.AuthorLastName;
			lblPublisher.Text = data.PublisherName;

			lvOtherData.Items.Clear();

			int		MaxNameWidth = 0;

			foreach (var item in data.OtherInfo) {
				var lvi = new ListViewItem(item.Key);
				Size size = TextRenderer.MeasureText(item.Key, lvOtherData.Font);
				if (size.Width > MaxNameWidth) {
					MaxNameWidth = size.Width;
					lvOtherData.Columns[0].Width = MaxNameWidth + 10;	// A few extra pixels
				}
				lvi.SubItems.Add(item.Value);
				lvOtherData.Items.Add(lvi);
			}
		}

		//---------------------------------------------------------------------------------------

		private void WesIsbnDemo_KeyDown(object sender, KeyEventArgs e) {
			// Needs the KeyPreview property set to true on the Form
			if (e.KeyValue == '\r') {
				e.Handled = true;
				GetBookInfo();
			}
		}
	}
}
