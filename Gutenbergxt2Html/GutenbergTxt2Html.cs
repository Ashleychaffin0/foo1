using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gutenbergxt2Html {
	public partial class GutenbergTxt2Html : Form {

		enum State {
			InPrefix,
			InBody,
			InSuffix
		};

//---------------------------------------------------------------------------------------

		public GutenbergTxt2Html() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------


		private void btnBrowse_Click(object sender, EventArgs e) {
			var ofd = new OpenFileDialog();
			var rc = ofd.ShowDialog();
			if (rc == DialogResult.OK) {
				txtFilename.Text = ofd.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Add some error checking
			string TitleLine, ChapterLine, EndOfDocument;
			TitleLine = txtTitle.Text;
			ChapterLine = "CHAPTER ";
			EndOfDocument = "End of Project Gutenberg";

			string OutputFilename = txtFilename.Text;
			OutputFilename = Path.ChangeExtension(OutputFilename, "html");

			StreamWriter wtr = new StreamWriter(OutputFilename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>");
			wtr.WriteLine("<HEAD>");
			wtr.WriteLine("<TITLE>" + TitleLine + "</TITLE>");
			wtr.WriteLine("</HEAD>");
			wtr.WriteLine("<BODY>");

			State state = State.InPrefix;
			foreach (string line in FileNet3.ReadLines(txtFilename.Text)) {
				if (line.StartsWith(TitleLine, StringComparison.CurrentCultureIgnoreCase)) {
					wtr.WriteLine("<H1>" + line + "</H1>");
					state = State.InBody;
					continue;
				} else if (line.StartsWith(ChapterLine, StringComparison.CurrentCultureIgnoreCase)) {
					wtr.WriteLine("<H2>" + line + "</H2>");
					continue;
				} else if (line.StartsWith(EndOfDocument, StringComparison.CurrentCultureIgnoreCase)) {
					state = State.InSuffix;
				}

				switch (state) {
				case State.InPrefix:
				case State.InSuffix:
					wtr.WriteLine(line + "<br>");
					break;
				case State.InBody:
					if (line.Trim().Length == 0) {
						wtr.WriteLine("<p>");
					} else {
						wtr.WriteLine(line);
					}
					break;
				default:
					break;
				}
			}
			wtr.WriteLine("</BODY>");
			wtr.WriteLine("</HTML>");
			wtr.Close();

			Process.Start(OutputFilename);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class FileNet3 {						// Surrogate for .Net 4.0 feature
		public static IEnumerable<string> ReadLines(string filename) {
			string line;
			// int     LineNo = 0;
			using (var fs = new StreamReader(filename)) {
				while ((line = fs.ReadLine()) != null) {
					// Console.WriteLine("Line {0}: {1}", ++LineNo, line);
					yield return line;
				}
			}
		}
	}


}
