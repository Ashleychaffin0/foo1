using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

// For EXIF values, see media.mit.edu/pia/Research/deepview/exif.html

namespace BarbPhotoAlbum {
	public partial class BarbPhotoAlbum : Form {

		string[] DirNames = { @"C:\Program Files\Common Files\" };   // More later?
		List<string> msgs;
		List<PhotoInfo> Photos;

		int nImages;
		int nImagesPrintedSoFar;

		int ImageHeight  = 400;
		int ImageWidth   = 400;
		int GutterWidth  = 60;
		int GutterHeight = 0;

		int CaptionHeight;      // Depends on Font height
		Font CaptionFont;

		int PageNumber;

//---------------------------------------------------------------------------------------

		public BarbPhotoAlbum() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			msgs            = new List<string>();
			Photos          = new List<PhotoInfo>();
			nImages         = 0;
			btnPrint.Enabled = false;
			Photos.Clear();

			DirNames[0] = @"C:\Users\Public\Pictures\Ashampoo Pictures";

			foreach (var DirName in DirNames) {
				ProcessDir(DirName);
			}

			Photos.Sort((p1, p2) => p1.Date.CompareTo(p2.Date));
			btnPrint.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		private void msg(string s) {
#if true
			if (s.EndsWith("\0")) {
				s = s.Substring(0, s.Length - 1);
			}
			msgs.Add(s);
			Console.WriteLine(s);
#endif
		}

//---------------------------------------------------------------------------------------

		private void ProcessDir(string DirName) {
			var Filenames = Directory.EnumerateFiles(DirName, "*.jpg", SearchOption.AllDirectories);
			var enc = new ASCIIEncoding();
			foreach (var Filename in Filenames) {
				if (nImages > 920)
					return;             // TODO:
				++nImages;
				// msg($"{Filename}");

				var Photo      = new PhotoInfo();
				Photo.Filename = Filename;
				using (Image img = Image.FromFile(Filename)) {
					Photo.img   = img;
					for (int i = 0; i < img.PropertyItems.Length; i++) {
						var prop = img.PropertyItems[i];
						var id   = img.PropertyIdList[i];
						switch (id) {
#if true
					case 0x0320:            // Image Title
						msg($"Title = {enc.GetString(prop.Value)}");
						break;
					case 0x010e:            // ImageDescription
						msg($"Image Description = {enc.GetString(prop.Value)}");
						break;
					case 0x010F:            // Equipment manufacturer
						msg($"Manufacturer = {enc.GetString(prop.Value)}");
						break;
					case 0x0110:            // Equipment model
						msg($"Model = {enc.GetString(prop.Value)}");
						break;
#endif
						case 0x9003:            // ExifDTOriginal
												// Format is ""2013:06:05 14:25:08\0""
												// Note the trailing null
							string Date = enc.GetString(prop.Value);
							string Time = Date.Substring(11, 8);
							Date = Date.Substring(0, 10).Replace(':', '/');
							Photo.Date = DateTime.Parse(Date + " " + Time);
							msg($"Date = {Photo.Date}");
							break;
#if false
					case 0x201:             // Thumbnail Offset
					case 0x202:             // Thumbmail Length
					case 0x0213:            // YCbCrPositioning
											// Ignore these
						break;
#endif
						default:
#if true
						if (prop.Type == 2) {
							msg($"\tPropID = 0x{id:x}, {enc.GetString(prop.Value)}");
						}
#endif
							break;
						}
					}
					Photos.Add(Photo);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void printDocument1_BeginPrint(object sender, PrintEventArgs e) {
			CaptionFont = new Font("Arial", 10.0f);
			CaptionHeight = CaptionFont.Height + 5;		// A bit more room

			nImagesPrintedSoFar = 0;
		}

//---------------------------------------------------------------------------------------

		private void printDocument1_EndPrint(object sender, PrintEventArgs e) {
			CaptionFont.Dispose();
		}

//---------------------------------------------------------------------------------------

		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
			++PageNumber;
			int nRows, nColumns;
			GetMatrixSize(e.MarginBounds, out nRows, out nColumns);

			var drawFormat         = new StringFormat();
			drawFormat.Alignment   = StringAlignment.Center;
			// drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;

			for (int row = 0; row < nRows; row++) {
				for (int col = 0; col < nColumns; col++) {
					if (++nImagesPrintedSoFar >= nImages) {
						e.HasMorePages = false;
						return;
					}

					PhotoInfo pi = Photos[nImagesPrintedSoFar];
					FileInfo  fi = new FileInfo(pi.Filename);

					using (Image img = Image.FromFile(pi.Filename)) {
						// Calculate image location, row major order
						float x = e.MarginBounds.Left + col * (ImageWidth + GutterWidth);
						float y = e.MarginBounds.Top + row * (ImageHeight + 2 * CaptionHeight * 2 + GutterHeight);

						// Draw caption on top (i.e. date)
						var rect = new RectangleF(x + e.MarginBounds.X, y, ImageWidth, ImageHeight);
						e.Graphics.DrawString(pi.Date.ToShortDateString(), CaptionFont, Brushes.Black, rect, drawFormat);

						// Draw image
						y += CaptionHeight;
						e.Graphics.DrawImage(img, new RectangleF(x, y, ImageWidth * img.Size.Width / img.Size.Height, ImageHeight));

						// Draw filename (aka Description) below image
						y += ImageHeight;
						rect = new RectangleF(x + e.MarginBounds.X, y, ImageWidth, ImageHeight * 2);
						e.Graphics.DrawString(Path.GetFileNameWithoutExtension(fi.Name), CaptionFont, Brushes.Black, rect, drawFormat);
					}
				}
			}

			e.HasMorePages = true;
			e.Graphics.DrawString($"Page {PageNumber}", CaptionFont, Brushes.Black, e.PageBounds.Left + 20, e.PageBounds.Bottom - 2 * CaptionHeight);
		}

//---------------------------------------------------------------------------------------

		private void GetMatrixSize(Rectangle MarginBounds, out int nRows, out int nColumns) {
			int PageWidth = MarginBounds.Width;

			nRows    = (MarginBounds.Height + GutterHeight) / (ImageHeight + CaptionHeight * 2 + GutterHeight);
			nColumns = (MarginBounds.Width  + GutterWidth)  / (ImageWidth  + GutterWidth);
		}

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, EventArgs e) {
			PageNumber = 0;
			printDocument1.PrinterSettings.DefaultPageSettings.Margins = new Margins(100, 0, 0, 0);
			printPreviewDialog1.Document = printDocument1;
			printPreviewDialog1.ShowDialog();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class PhotoInfo {
		public string	Filename;
		public DateTime Date;
		public Image	img;
	}
}
