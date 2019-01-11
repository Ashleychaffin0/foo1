using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace SciAmToc {
	// TODO: Probably should go into a generic PdfUtils class
	class PdfFindText {

//---------------------------------------------------------------------------------------
#if false
		private void CountTensors(DateRange dr) {
			foreach (var item in dr.Dates()) {
				int Decade = (item.Year / 10) * 10;
				SciAmDecadeDir = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");
				CountTensor(item.Year, item.Month);
			}
		}
#endif

//---------------------------------------------------------------------------------------
#if false
		public void CountTensor(int year, int month) {
			string dir = Path.Combine(SciAmDecadeDir, $"Sciam - {year}");
			if (!Directory.Exists(dir)) return;
			string FNamePrefix = $"Sciam - {year}-{month:d2}";
			var FullPath       = Directory.GetFiles(dir, $"{FNamePrefix}*.pdf");
			if (FullPath.Length == 0) return;   // Shouldn't happen
			string fname = Path.GetFileNameWithoutExtension(FullPath[0]);
			int nTens = 0;
			using (var rdr = new PdfReader(FullPath[0])) {
				// TODO: Need try/catch around next 'using'
				using (var pdoc = new PdfDocument(rdr)) {
					var Lines = TensorGetPageText(pdoc, 1, pdoc.GetNumberOfPages());
					foreach (var line in Lines) {
						if (line.ToUpper().Contains("TENSOR")) ++nTens;
					}
				}
			}
			if (nTens > 0) {
				string msg = $"{nTens:d2}: {fname}<br/>";
				OutFile.WriteLine(msg);
				Console.WriteLine(msg);
			}
		}
#endif

		//---------------------------------------------------------------------------------------

		public static List<string> TensorGetPageText(PdfDocument pdoc, int FirstPage, int LastPage) {
			char[]   CrLf = new char[] {'\r', '\n'};
			var AllLines = new List<string>();
			for (int i = FirstPage; i <= LastPage; i++) {
				AllLines.Add($"<error>Page {i}</error><br/>");
				var page = pdoc.GetPage(i);
				try {
					var text = PdfTextExtractor.GetTextFromPage(page);
					var lines = text.Split(CrLf, StringSplitOptions.RemoveEmptyEntries);
					AllLines.AddRange(lines);
				} catch { /* Ignore the error */}
			}
			return AllLines;
		}
	}
}
