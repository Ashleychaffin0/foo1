using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.IO;

namespace ReadOPML {
	public partial class Form1 : Form {

		OPML	op;

		public Form1() {
			InitializeComponent();

			op = new OPML();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			op.Load(txtOPMLFilename.Text);
			Process();
		}

		private void btnBrowseOPML_Click(object sender, EventArgs e) {
			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "OPML files (*.opml)|*.opml|XML files (*.xml)|*.xm|All files (*.*)|*.*";
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtOPMLFilename.Text = openFileDialog1.FileName;
			}
		}

		private void Process() {
			StringBuilder sb = new StringBuilder();
			try {
				foreach (XmlNode node in op.outline) {
					sb.Length = 0;
					for (int i = 0; i < node.Attributes.Count; ++i) {
						if (i > 0)
							sb.Append(", ");
						sb.Append(node.Attributes[i].Name + " = " + node.Attributes[i].Value);
					}
					listBox1.Items.Add(sb.ToString());
				}
			} catch (Exception ex) {
				MessageBox.Show("Exception (" + ex.Message + ") listing outline");
			}
#if false	// TODO: These go somewhere else
			catch (ArgumentNullException) {
				MessageBox.Show("No filename specified. Specify one and try again.",
					"ReadOPML", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (FileNotFoundException) {
				MessageBox.Show("Filename not found. Respecify one and try again.",
					"ReadOPML", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
#endif
		}
	}

	public class OPML {
		private string	Filename;

		private XmlDocument	xdoc = null;
		public XmlNodeList	outline = null;		// TODO: s/b private

		public OPML(string Filename) {
			this.Filename = Filename;
			Load();
		}

		public OPML() {
			Filename = null;
		}

		public void Load(string Filename) {
			this.Filename = Filename;
			Load();
		}

		private void Load() {
			if (Filename == null) {
				throw new ArgumentNullException(Filename);
			}
			xdoc = new XmlDocument	();
			xdoc.Load(Filename);
			outline = xdoc.GetElementsByTagName("outline");
		}
	}
}