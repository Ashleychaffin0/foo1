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

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ValidateSciamDates {

	public partial class ValidateSciamDates	: Form {

		string BaseDir = @"D:/LRS/$SciAm-Full Issues/";

//---------------------------------------------------------------------------------------
		public ValidateSciamDates() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// var dt = DateTime.Parse("1890-01-04");
			// string fname = @"D:\LRS\$SciAm-Full Issues\Sciam - 1890\Sciam - 1890-01-04 - January 04.pdf";
			for (int decade = 1840; decade <= 1910; decade += 10) {
				DoDecade(decade);
			}
		}

//---------------------------------------------------------------------------------------

		private static void LookForMatchingDate(string fname) {
			// TODO: Change to use DateTime.TryParse
			var dt = DateTime.Parse(fname.Substring(63, 10)).ToString("MMMM d, yyyy");
			Console.WriteLine("Processsing " + dt);
			bool bDate = false;
#if true
			using (var rdr = new PdfReader(fname)) {
				// var info = rdr.Info;
				ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
				for (int page = 1; page < rdr.NumberOfPages; page++) {
					string currentText = PdfTextExtractor.GetTextFromPage(rdr, page, strategy);
					currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
					bDate = currentText.Contains(dt);
					if (bDate) {
						break;
					}
				}

				if (! bDate) {
					Console.WriteLine("*** Failed for " + fname);
				}
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void DoDecade(int decade) {
			string Dirname = System.IO.Path.Combine(BaseDir, $"$SCIAM - {decade}'s");
			// string Dirname = $"{BaseDir}/$SCIAM - {decade}'s";
			ProcessYears(Dirname);
		}

//---------------------------------------------------------------------------------------

		private void ProcessYears(string dirname) {
			var DirNames = Directory.EnumerateDirectories(dirname);
			foreach (var name in DirNames) {
				ProcessYear(name);
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessYear(string dirname) {
			foreach (var filename in Directory.EnumerateFiles(dirname, "*.pdf")) {
				// Console.WriteLine(filename);
				LookForMatchingDate(filename);
			}
		}
	}
}
