#if true

// See http://itextsupport.com/apidocs/itext7/latest/

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using iText;
using iText.IO.Codec;		// Might be relevant
using iText.IO.Image;
using iText.IO.Log;
using iText.Kernel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace SciAmToc {
	class LRS_PdfUtils {

//---------------------------------------------------------------------------------------

		public static void CreateArticle(string fnIn, string fnOut, int FirstPage, int LastPage, bool bSinglePage = false, bool bCoverPage = false) {
			var rdr    = new PdfReader(fnIn);
			var pdocIn = new PdfDocument(rdr);

			string fnDir = Path.GetDirectoryName(fnOut);
			// This kind of sucks. Path.GetFileName doesn't give the expected answer if
			// the filename has a ":" in it if the colon isn't the second character. In
			// our case, from 1955-06, "ALBERT EINSTEIN: 1979-1955". GetFilename returns
			// " 1879-1955"! So we have to special-case a colon.
			fnOut = fnOut.Replace(":", " --");
			fnOut = Path.GetFileName(fnOut);

			fnOut = fnOut.Replace("/", "!")
				.Replace('"', '\'')
				.Replace("\\", "!")
				.Replace("?", "$Q");
			foreach (char c in Path.GetInvalidFileNameChars()) {
				fnOut = fnOut.Replace(c, '$');
			}
			fnOut = Path.Combine(fnDir, fnOut);
			var wtr  = new PdfWriter(fnOut);

			// www.onlineocr.net seems to limit you (unless you register) to OCRing
			// only a single page. Support that here.
#if false
			if (bSinglePage && (FirstPage < LastPage)) {
				for (int i = FirstPage; i <= LastPage; i++) {
					pdocIn.CopyPagesTo(i, i, pdocOut);
				// pdocOut.RemovePage();pdocOut.Close();pdoc
				}
			} else {
			}
#endif

				using (var pdocOut = new PdfDocument(wtr)) {
					if (bCoverPage) pdocIn.CopyPagesTo(1, 1, pdocOut);
					pdocIn.CopyPagesTo(FirstPage, LastPage, pdocOut);
				}

			Process.Start(fnOut);
		}

//---------------------------------------------------------------------------------------

		public static void Test() {
			// var x        = new iText.IO.Image;
			// var y        = iText.IO.Image
			// string filename = @"G:\LRS\SciAmFoo.pdf";
			string filename = @"G:\LRS\sampele_pdf_2.pdf";
			PdfReader rdr   = new PdfReader(filename);
			var pdoc        = new PdfDocument(rdr);
			PdfCatalog cat  = pdoc.GetCatalog();
			var info        = pdoc.GetDocumentInfo();
			PdfPage page    = pdoc.GetFirstPage();
#if true
			// var extractor = new PdfTextExtractor(;
			// string xxx = extractor.GetTextFromPage(000001);
			var xxx = PdfTextExtractor.GetTextFromPage(page);
#if false
			// From https://www.aspforums.net/Threads/132819/Read-and-extract-searched-text-from-pdf-file-using-iTextSharp-in-ASPNet/
			iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
			string currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
			currentText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert
				(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.UTF8.GetBytes(currentText)));
#endif
#endif
			// var map      = page.GetPdfObject().map[PdfName.MediaBox];
			var keys        = page.GetPdfObject().KeySet();
			var MediaBoxVal = page.GetPdfObject().Values();
			var xx          = MediaBoxVal;
			// var y        = new iText.Kernel.Pdf.Canvas.Parser.PdfCanvasProcessor();
			/*
			PdfReaderContentParser parser = new PdfReaderContentParser(rdr);
			MyImageRenderListener listener = new MyImageRenderListener(RESULT);
			*/
			for (int i = 1; i <= pdoc.GetNumberOfPages(); i++) {
				// parser.processContent(i, listener);
			}
			rdr.Close();
		}
	}
}
#else
			using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using iText.Kernel.Pdf;

namespace SciAmToc {
	class PdfUtils {
		private IList<System.Drawing.Image> GetImagesFromPdfDict(PdfDictionary dict, PdfReader doc) {
			List<System.Drawing.Image> images = new List<Image>();

			PdfDictionary res = (PdfDictionary)(PdfReader.GetPdfObject(dict.Get(PdfName.Resources)));
			PdfDictionary xobj = (PdfDictionary)(PdfReader.GetPdfObject(res.Get(PdfName.XObject)));

			if (xobj != null) {
				foreach (PdfName name in xobj.Keys) {
					PdfObject obj = xobj.Get(name);
					if (obj.IsIndirect()) {
						PdfDictionary tg = (PdfDictionary)(PdfReader.GetPdfObject(obj));
						PdfName subtype = (PdfName)(PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE)));
						if (PdfName.Image.Equals(subtype)) {
							int xrefIdx = ((PRIndirectReference)obj).Number;
							PdfObject pdfObj = doc.GetPdfObject(xrefIdx);
							PdfStream str = (PdfStream)(pdfObj);

							iTextSharp.text.pdf.parser.PdfImageObject pdfImage =
						new iTextSharp.text.pdf.parser.PdfImageObject((PRStream)str);
							System.Drawing.Image img = pdfImage.GetDrawingImage();

							images.Add(img);
						} else if (PdfName.Form.Equals(subtype) || PdfName.Group.Equals(subtype)) {
							images.AddRange(GetImagesFromPdfDict(tg, doc));
						}
					}
				}
			}

			return images;
		}
	}
}
#endif