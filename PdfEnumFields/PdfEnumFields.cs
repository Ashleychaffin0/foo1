using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Kernel.Pdf;

namespace PdfEnumFields {
	public partial class PdfEnumFields : Form {
		public PdfEnumFields() {
			InitializeComponent();

			string fnIn = @"g:\lrs\SciAmFoo.pdf";
			string fnOut = @"G:\lrs\SciAmFooCopy.pdf";

			CreateArticle(fnIn, fnOut, 1, 1);
		}

		private static void CreateArticle(string fnIn, string fnOut, int FirstPage, int LastPage, bool bCoverPage = false) {
			var rdr = new PdfReader(fnIn);
			var pdoc = new PdfDocument(rdr);
			var wtr = new PdfWriter(fnOut);
			using (var pdocOut = new PdfDocument(wtr)) {
				if (bCoverPage)	pdoc.CopyPagesTo(1, 1, pdocOut);
				pdoc.CopyPagesTo(1, 1, pdocOut);
				pdoc.CopyPagesTo(FirstPage, LastPage, pdocOut);
			}
			Process.Start(fnOut);
		}

		private static void Test
			(string fnIn, string fnOut) {
			var rdr = new PdfReader(fnIn);
			var pdoc = new PdfDocument(rdr);
			var cat = pdoc.GetCatalog();
			var obj = cat.GetPdfObject() as PdfDictionary;
			var objKeys = obj.KeySet();
			var props = new WriterProperties();
			var wtr = new PdfWriter(fnOut, props);
			var pdocOut = new PdfDocument(wtr);
			pdoc.CopyPagesTo(1, 1, pdocOut);
			var page = pdocOut.AddNewPage();
			// page.Put(PdfName.Text, new PdfObject());
			pdoc.CopyPagesTo(11, 12, pdocOut);
			pdocOut.Close();
			Process.Start(fnOut);
		}
	}
}
