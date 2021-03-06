// Copyright (c) 2005-2006 Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

using System.Runtime.InteropServices;

// TODO: Additional ideas

// * Save user attributes (e.g. default text, default barcode size, etc). Probably
//	 save this in the database, rather than as a local XML file.

namespace TestBarcodes {
	public partial class TestBarcodes : Form {

		float				BarcodeWidth, BarcodeHeight, ShrinkFactor;

		TALBarcodePDF417	pdf;

		const string		DBFilename = "TestBarcodes.mdb";

		PrintDocument		pd;
		PrinterSettings		ps;

		Regex				reScanStats;		// e.g. 3.00 x 0.90

//---------------------------------------------------------------------------------------
		
		public TestBarcodes() {
			InitializeComponent();
			
			pdf = new TALBarcodePDF417(300, 300);		// TODO: Hardcoded 300 DPI
			cmbScanningEase.SelectedIndex = 0;			// Assume "Wouldn't Scan" is first

			if (!SetupPrinters())
				return;

			reScanStats = new Regex(@"(?<Width>\d+\.\d+)\s*x\s*(?<Height>\d+.\d+)");
		}

//---------------------------------------------------------------------------------------

		private bool SetupPrinters() {
			pd = new PrintDocument();
			pd.DocumentName = "TestBarcodes";
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(printDocument1_PrintPage);
			pd.BeginPrint += new PrintEventHandler(pd_BeginPrint);
			pd.QueryPageSettings += new QueryPageSettingsEventHandler(pd_QueryPageSettings);
			printPreviewDialog1.Document = pd;

			ps = new PrinterSettings();
			PrinterSettings.StringCollection printers = PrinterSettings.InstalledPrinters;
			if (printers.Count == 0) {
				MessageBox.Show("There are no printers installed. TestBarcodes cannot run",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			// TODO: Get databinding to work on cmbPrinters
			string[] prts = new string[printers.Count];
			printers.CopyTo(prts, 0);
			Array.Sort(prts);
			// TODO: Can we set the printer resolution?
			int n = 0;
			int ixDefault = 0;						// Index of default printer
			foreach (string prtname in prts) {
				cmbPrinters.Items.Add(prtname);
				ps.PrinterName = prtname;			// Implicitly gets all the settings
													//	for this printer
				if (ps.IsDefaultPrinter)
					ixDefault = n;
				++n;
			}
			cmbPrinters.SelectedIndex = ixDefault;	// Note: This code chooses either the
													//		 default printer, or the
													//		 first printer, if somehow we
													//		 don't have a default.

			pd.PrinterSettings.PrinterName = (string)cmbPrinters.SelectedItem;
			return true;
		}

//---------------------------------------------------------------------------------------

		private void btnFillWithRandomData_Click(object sender, EventArgs e) {
			FillWithRandomData();
		}

//---------------------------------------------------------------------------------------

		void FillWithRandomData() {
			int		RandomDataSize;

			bool OK = int.TryParse(txtRandomDataSize.Text, out RandomDataSize);
			if ((!OK) || RandomDataSize <= 0) {
				MessageBox.Show("The Random Data Size field is not a positive integer. Fix it and try again.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Random			rand = new Random();
			StringBuilder	sb = new StringBuilder();
			char			c;
			for (int i = 0; i < RandomDataSize; i++) {
				c=Convert.ToChar(rand.Next(32, 127));		// ASCII-7
				sb.Append(c);	
			}
			txtOriginal.Text = sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private void CheckOriginalVsScanned() {
			if (txtOriginal.Text == txtScanned.Text) {
				lblOriginalMatchesScanned.Text = "True";
				lblOriginalMatchesScanned.ForeColor = Color.Red;
			} else {
				lblOriginalMatchesScanned.Text = "False";
				lblOriginalMatchesScanned.ForeColor = Color.Black;
			}
		}

//---------------------------------------------------------------------------------------

		private void txtOriginal_TextChanged(object sender, EventArgs e) {
			CheckOriginalVsScanned();
		}

//---------------------------------------------------------------------------------------

		private void txtScanned_TextChanged(object sender, EventArgs e) {
			string s = txtScanned.Text;
			if (s.EndsWith("\n")) {
				Match m = reScanStats.Match(s);
				string w = m.Groups["Width"].Value;
				string h = m.Groups["Height"].Value;
				txtScannedWidth.Text = w;
				txtScannedHeight.Text = h;
				txtScanned.Clear();
				return;
			}
			CheckOriginalVsScanned();
		}

//---------------------------------------------------------------------------------------

		private void txtRandomDataSize_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == 13) {
				FillWithRandomData();
				e.Handled = true;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnFilenameBrowse_Click(object sender, EventArgs e) {
			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtFilename.Text = openFileDialog1.FileName;
				FillFromFile();
			}
		}

//---------------------------------------------------------------------------------------

		private void btnFillFromFile_Click(object sender, EventArgs e) {
			if (txtFilename.Text.Trim().Length == 0) {
				MessageBox.Show("No filename has been specified. Use the Browse button.",			
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			FillFromFile();
		}

//---------------------------------------------------------------------------------------

		private void FillFromFile() {
			StreamReader myStream = null;

			try {
				if ((myStream = new StreamReader(txtFilename.Text)) != null) {
					txtOriginal.Text = myStream.ReadToEnd();
				}
			} catch (Exception ex) {
				MessageBox.Show("There was an error (" + ex.Message + ") opening the file. Fix it and try again.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				if (myStream != null) {
					myStream.Close();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnClearOriginalText_Click(object sender, EventArgs e) {
			txtOriginal.Text = "";
			txtOriginal.Focus();
		}

//---------------------------------------------------------------------------------------

		private void btnClearScannedText_Click(object sender, EventArgs e) {
			txtScanned.Text = "";
			txtScanned.Focus();
		}

//---------------------------------------------------------------------------------------

		private void btnPrintPreview_Click(object sender, EventArgs e) {
			if (!CheckPrintParms())
				return;
			printPreviewDialog1.ShowDialog();
			AfterPrint();
		} 

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, EventArgs e) {
			if (! CheckPrintParms())
				return;

			// OK, the values are (more-or-less) valid. On with the printing.
			pd.Print();
			AfterPrint();
		}

//---------------------------------------------------------------------------------------

		private void AfterPrint() {

			// Set the focus to the Scanned box, on the assumption that once
			// we've printed a barcode, the next thing we want to do is to 
			// scan it.
			txtScanned.Text = "";
			txtScanned.Focus();

			// Reset the logging fields to their defaults
			cmbScanningEase.SelectedIndex = 0;	// Assumes Wouldn't Scan is first
		}

//---------------------------------------------------------------------------------------

		private bool CheckPrintParms() {
			if (txtOriginal.Text.Length == 0) {
				MessageBox.Show("There is no text to put on the barcode. Fix it and try again.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			bool OK;
			OK = float.TryParse(txtBarcodeWidth.Text, out BarcodeWidth);
			if ((!OK) || BarcodeWidth <= 0) {
				MessageBox.Show("Barcode width not non-negative float. Fix it and try again.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			OK = float.TryParse(txtBarcodeHeight.Text, out BarcodeHeight);
			if ((!OK) || BarcodeHeight <= 0) {
				MessageBox.Show("Barcode height not non-negative float. Fix it and try again.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			if (txtShrinkFactor.Text.Trim().Length == 0) {
				ShrinkFactor = 1f;
			} else {
				OK = float.TryParse(txtShrinkFactor.Text, out ShrinkFactor);
				if ((!OK) || ShrinkFactor <= 0) {
					MessageBox.Show("Shrink factor not non-negative float. Fix it and try again.",
						"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				ShrinkFactor /= 100f;
				ShrinkFactor = 1 - ShrinkFactor;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		void pd_QueryPageSettings(object sender, QueryPageSettingsEventArgs e) {
			e.PageSettings.PrinterResolution.Kind = (PrinterResolutionKind)cmbPrinterResolution.SelectedItem;
		}

//---------------------------------------------------------------------------------------

		private void pd_BeginPrint(object sender, PrintEventArgs e) {
			// No processing for now
		}

//---------------------------------------------------------------------------------------

		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
			const float	PageHeight	= 1100;		// Assume standard 8.5x11
			const float PageWidth	= 850;
			const int	vGutter		= 100;		// 1" vertically between barcodes

			// First, show the text at the top of the page
			RectangleF		rectf;
			float			y;
			Font			fnt = new Font("Arial", 12);
			float			bcWidth = BarcodeWidth * 100f;
			float			bcHeight = BarcodeHeight * 100f;
			//y = rectf.Bottom + 100;			// Put text 1 inch below barcode
			rectf = new RectangleF(50, 50, 800, 1050);
			StringFormat	sfmt = new StringFormat();	// No flags needed right now.
			string			msg = string.Format("Msg length = {0} - {1}", 
				txtOriginal.Text.Length, txtOriginal.Text);
			SizeF szf = e.Graphics.MeasureString(msg, fnt, rectf.Size, sfmt);
			e.Graphics.DrawString(msg, fnt, Brushes.Black,
				rectf, sfmt);
			y = szf.Height + 50;			// Bottom of text

			y += 25;						// Add a quarter inch between barcodes

			float		HeightLeft = PageHeight - y - 50;	// 50 = 1/2" at bottom

			// Now show one or more barcodes underneath
			Size	szComment = new Size(150, 50);
			bool	bBarcodeOnLeft = true;
			while (bcHeight <= HeightLeft) {
				if (bcHeight <= 10f)			// Arbitrary cutoff of 1/10" high
					break;	
				Size sz = new Size((int)bcWidth, (int)bcHeight);
				if (bBarcodeOnLeft) {
					rectf = new RectangleF(50, y, (int)bcWidth, (int)bcHeight);
				} else {
					rectf = new RectangleF(PageWidth - 50 - bcWidth, y, (int)bcWidth, bcHeight);
				}
				string	comment = string.Format("{0:F2} x {1:F2}", bcWidth * .01f, bcHeight * .01f);
				Bitmap	bm = pdf.MakeBarCode(txtOriginal.Text, "Data - " + comment, sz);
				if (bm == null) {
					MessageBox.Show("The barcode could not be created, but the reason is unknown. Change something and try again.",
						"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				e.Graphics.DrawImage(bm, rectf);

				// Now show sizing barcode
				bm = pdf.MakeBarCode(comment + "\n", comment, szComment);
				if (bm == null) {
					MessageBox.Show("The sizing barcode could not be created, but the reason is unknown. Change something and try again.",
						"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (bBarcodeOnLeft) {
					rectf = new RectangleF(PageWidth - 50 - szComment.Width, y, szComment.Width, szComment.Height);
				} else {
					// Right-align with barcode above. Otherwise the scanner can pick up
					// the smaller dimensions barcode, rather than the data.
					rectf = new RectangleF(50 + bcWidth - szComment.Width, y, szComment.Width, szComment.Height);
				}
				e.Graphics.DrawImage(bm, rectf);

				y += bcHeight + vGutter;
				HeightLeft -= bcHeight + vGutter;
				bcHeight *= ShrinkFactor;

				bBarcodeOnLeft = ! bBarcodeOnLeft;
			}
		}
	
//---------------------------------------------------------------------------------------

		public static string FindFileUpInHierarchy(string filename) {
			// TODO: Need overload - with and without default starting directory
			string	path = Application.StartupPath + @"\";
			bool	bFound = false;

			// TODO: Not 16. Should stop when it gets to merely a drive letter
			for (int i=0; i<16; ++i) {				// Sixteen levels should be enough!
				if (File.Exists(path + filename)) {
					bFound = true;
					break;
				} else {
					filename = @"..\" + filename;
				}
			}
			if (! bFound) {
				return null;
			}
			FileInfo	fi = new FileInfo(path + filename);
			return fi.FullName;			// TODO: Note: in FileOps, this returns just the directory name
		}

//---------------------------------------------------------------------------------------

		private void btnLogToDatabase_Click(object sender, EventArgs e) {
			bool	bOK;
			float	ScannedWidth, ScannedHeight;
			bOK  = float.TryParse(txtScannedWidth.Text,  out ScannedWidth);
			bOK &= float.TryParse(txtScannedHeight.Text, out ScannedHeight);
			if (! bOK) {
				MessageBox.Show("The barcode dimensions are invalid. Either fix them, or else dimensions barcode wasn't scanned.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			System.Data.OleDb.OleDbConnectionStringBuilder builder =
				new System.Data.OleDb.OleDbConnectionStringBuilder();
			builder["Provider"] = "Microsoft.Jet.OLEDB.4.0";
			builder["Data Source"] = FindFileUpInHierarchy(DBFilename);
			// builder["Share"] = "";
			// builder["User Id"] = "Admin;NewValue=Bad";
			string	cs = builder.ConnectionString;
			try {
				using (OleDbConnection conn = new OleDbConnection(cs)) {
					OleDbDataAdapter adapt  = new OleDbDataAdapter("SELECT * FROM ScanResults WHERE 0=1", conn);
					OleDbCommandBuilder CmdBuilder = new OleDbCommandBuilder(adapt);
					// These next lines are needed if any of the field names have embedded blanks
					CmdBuilder.QuotePrefix = "[";
					CmdBuilder.QuoteSuffix = "]";
					DataTable tblScans = new DataTable("ScanResults");
					adapt.Fill(tblScans);
					DataRow row = tblScans.NewRow();
					row["When"]					= DateTime.Now;
					row["SystemName"]			= Environment.MachineName;
					row["Text"]					= txtOriginal.Text;
					row["TextLen"]				= txtOriginal.Text.Length;
					row["ScannedText"]			= txtScanned.Text;
					row["DataComparedOK"]		= txtOriginal.Text == txtScanned.Text;
					row["BarcodeWidth"]			= ScannedWidth;
					row["BarcodeHeight"]		= ScannedHeight;
					row["EaseOfScanning"]		= cmbScanningEase.SelectedItem;
					row["Comments"]				= txtComments.Text;
					row["Printer"]				= cmbPrinters.SelectedItem;
					row["PrinterResolution"]	= cmbPrinterResolution.SelectedItem;
					row["PrinterResolutionX"]	= txtPrinterCustomWidth.Text;
					row["PrinterResolutionY"]	= txtPrinterCustomHeight.Text;
					tblScans.Rows.Add(row);
					adapt.Update(tblScans);

					txtScannedWidth.Text	= "";
					txtScannedHeight.Text	= "";
					txtScanned.Text			= "";
					txtScanned.Focus();
				}
			} catch (Exception ex) {
				MessageBox.Show("Unexpected error (" + ex.Message + ") writing out database record.",
					"TestBarCodes", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnOpenDatabase_Click(object sender, EventArgs e) {
			string filename = FindFileUpInHierarchy(DBFilename);
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private void btnSaveAs_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Save As");	// TODO:
		}

//---------------------------------------------------------------------------------------

		private void cmbPrinters_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox	cmb = (ComboBox)sender;
			string		PrinterName = (string)cmb.SelectedItem;
			ps.PrinterName = PrinterName;
			pd.PrinterSettings.PrinterName = PrinterName;
			cmbPrinterResolution.Items.Clear();
			PrinterResolutionKind DefaultKind = ps.DefaultPageSettings.PrinterResolution.Kind;
			int		n = 0;
			int		ixDefaultKind = 0;
			this.Cursor = Cursors.WaitCursor;
			foreach (PrinterResolution pr in ps.PrinterResolutions) {
				cmbPrinterResolution.Items.Add(pr.Kind);
				txtPrinterCustomWidth.Text = pr.X.ToString();
				txtPrinterCustomHeight.Text = pr.Y.ToString();
				if (pr.Kind == DefaultKind) {
					ixDefaultKind = n;
				}
				++n;
			} 
			cmbPrinterResolution.SelectedIndex = ixDefaultKind;
			this.Cursor = Cursors.Default;

			// Now set the Custom X, Y fields (and their visibility)
			bool	vis = DefaultKind.ToString() == "Custom";
			SetPrinterXYVisibility(vis);
		}

//---------------------------------------------------------------------------------------

		private void SetPrinterXYVisibility(bool vis) {
			lblPrinterCustomWidth.Visible = vis;
			txtPrinterCustomWidth.Visible = vis;
			lblPrinterCustomHeight.Visible = vis;
			txtPrinterCustomHeight.Visible = vis;
		}

//---------------------------------------------------------------------------------------

		private void cmbPrinterResolution_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox	cmb = (ComboBox)sender;
			PrinterResolutionKind Kind = (PrinterResolutionKind)cmb.SelectedItem;
			bool vis = Kind.ToString() == "Custom";

			this.Cursor = Cursors.WaitCursor;
			SetPrinterXYVisibility(vis);

			foreach (PrinterResolution pr in ps.PrinterResolutions) {
				if (pr.Kind == Kind) {
					txtPrinterCustomWidth.Text = pr.X.ToString();
					txtPrinterCustomHeight.Text = pr.Y.ToString();
					break;
				}
			}
			this.Cursor = Cursors.Default;
		}
	}




	// We ***REALLY*** need a way to hook these things in. At the moment, this
	// has been copied and pasted from BartPrint.cs. Sigh.

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// This is essentially a factory. Once instantiated with fairly constant information
	/// (e.g. the printer resolution we're targeting), we can repeatedly call the
	/// MakeBarCode method to return a new barcode image each time.
	/// </summary>
	public class TALBarcodePDF417 {

		int hRes, vRes;		// Horizontal and Vertical resolution in DPI

//---------------------------------------------------------------------------------------

		[DllImport("TALPDF32.dll", EntryPoint = "TALPDFCODE")]
		private static extern int TALPDFCode(ref TALPDFBarCodeParms bc, ref MetaFilePict mf);

//---------------------------------------------------------------------------------------

		public TALBarcodePDF417(int hRes, int vRes) {
			this.hRes = hRes;
			this.vRes = vRes;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Makes a PDF barcode (using the TAL DLL) that fits into the specified size.
		/// </summary>
		/// <param name="s">The text of the message to be encoded in the barcode.</param>
		/// <param name="sz">The size of the area into which it has to fit.</param>
		/// <param name="unit">The units that the "sz" parameter is in (e.g. pixels,
		/// inches, etc). Note: Barcodes on screens are pretty useless, so if we see
		/// a unit type of .Display, which is either in pixels for the screen, or 
		/// 1/100th of an inch for printers, we'll assume the latter.</param>
		/// <returns>Either the image of the barcode, or null indicating there
		/// was a problem creating it. </returns>
		public Bitmap MakeBarCode(string s, string comment, Size sz, GraphicsUnit unit) {
			// TODO: Maybe add an out param with the error code. Or return an enum
			//		 and make the Image an out param.
			Size newSize = GetSizeInPixels(sz, unit);
			return CreateBitMap(s, comment, newSize);
		}

//---------------------------------------------------------------------------------------

		public Bitmap MakeBarCode(string s, string comment, Size sz) {
			return MakeBarCode(s, comment, sz, GraphicsUnit.Display);
		}

//---------------------------------------------------------------------------------------

		private static Bitmap CreateBitMap(string s, string comment, Size newSize) {
			// OK, for now we're going to brute-force it. Later we may optimize a bit.
			// TODO: newSize parameter never used
			TALPDFBarCodeParms bc = new TALPDFBarCodeParms();
			MetaFilePict mfp = new MetaFilePict();
			bc.MessageBuffer = s;
			bc.MessageLength = s.Length;
			bc.CommentBuffer = comment;
			bc.CommentLength = comment.Length;
			bc.PDFModuleWidth = 30;
			bc.PDFModuleHeight = 3 * bc.PDFModuleWidth;	// Suggested ratio
			bc.OutputOption = 1;			// Disk file
			string filename = Path.GetTempFileName() + ".emf";
			// Note: GetTempFileName doesn't just return a filename, it actually
			//       creates a zero-length file as well. So we have to delete it
			//       whether the call to the barcode DLL succeeds or not.
			bc.OutputFilename = filename;
			int rc = TALPDFCode(ref bc, ref mfp);
			if (rc != 0) {
				File.Delete(filename);
				return null;
				// Arguably we should have an <out> parameter to return the actual
				// error code. TODO:
			}
			Bitmap bmp = new Bitmap(bc.OutputFilename);
			File.Delete(filename);
			return bmp;
#if false
			WmfPlaceableFileHeader	ph = new WmfPlaceableFileHeader();
			Metafile mf2 = new Metafile(mfp.hMf, ph);
			Metafile mf = new Metafile(mfp.hMf, EmfType.EmfPlusOnly);		
			return null;					// TODO:
#endif
#if false
			IDataObject	data;					// From clipboard
			data = Clipboard.GetDataObject();
			string[] fmts = data.GetFormats();	// TODO:
			Metafile	mf;
			object o = data.GetData(DataFormats.EnhancedMetafile);	// TODO:
			object o2 = data.GetData("EnhancedMetafile");			// TODO:
			o2 = data.GetData(fmts[1], true);			// TODO:
			mf = (Metafile)data.GetData(DataFormats.EnhancedMetafile);
			return (Bitmap)data.GetData(typeof(Bitmap));
			
#endif
		}

//---------------------------------------------------------------------------------------

		Size GetSizeInPixels(Size sz, GraphicsUnit unit) {
			// TODO: For the demo, support nothing but unit=Display.
			// TODO: Later we can/should probably do this as a transform
			Size scaledSize;
			int h, w;				// Height, Width

			h = sz.Height;
			w = sz.Width;
			switch (unit) {
			case GraphicsUnit.Display:
				// Assume we're printing, so Display units are 1/100th of an inch.
				// Conceptually, we divide the width by 100 (to get inches), then
				// multiply by the DPI. But to minimize rounding errors, we'll
				// multiply first, then divide afterwards.
				// So, for example, if we have w=300, h=100 (i.e. 3" by 1"), and
				// 300 DPI, 
				h *= hRes;
				w *= vRes;
				// At this point, w=300*300 = 90000, h=100*300 = 30000
				h /= 100;
				w /= 100;
				// At this point, w=90000/100 = 900 (=3 inches in pixels at
				// 300 DPI). Similarly, h=30000/100 = 300 (= 1 inch at 300 DPI)
				scaledSize = new Size(w, h);
				break;
			case GraphicsUnit.Document:
			case GraphicsUnit.Inch:
			case GraphicsUnit.Millimeter:
			case GraphicsUnit.Pixel:
			case GraphicsUnit.Point:
			case GraphicsUnit.World:
			default:
				scaledSize = sz;
				break;
			}
			return scaledSize;
		}
	}

	// Barcode stuff (TAL, PDF, etc) follows. TODO: Generalize this later.

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TALPDFBarCodeParms {					// TODO: Put in rest of comments
		public int		MessageLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2712)]
		public string	MessageBuffer;
		// char 		messageBuffer[2712];		// That's what the doc says, 2712
		public int		CommentLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 100)]
		public string	CommentBuffer;
		// char	commentBuffer[100];
		public int		PDFModuleWidth;
		public int		BarWidthReduction;
		public int		PDFModuleHeight;
		public float	PDFAspect;
		public int		PDFSecurityLevel;
		public int		PDFCompactionMode;
		public int		PDFPctOverhead;
		public int		PDFMaxRows;
		public int		PDFMaxCols;
		public uint		FgColor;			// Foreground color. Maybe COLOR later. TODO:
		public uint		BgColor;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
		public string	FontName;
		// char fontName[32]
		public int		FontSize;
		public uint		TextColor;
		public int		Orientation;
		public int		Preferences;
		public int		HotizontalDPI;
		public int		VerticalDPI;
		public int		OutputOption;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string	OutputFilename;
		// char outputFilename[260]
		public IntPtr	OutputhDC;				// hDC for output device, when going to hDC
		public float	XPosInInches;
		public float	YPosInInches;
		int				Reserved;
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MetaFilePict {
		public int		mm;					// Metafile map mode
		public int		xExt;				// Width of the metafile
		public int		yExt;				// Height of the metafile
		public IntPtr	hMf;				// Handle to the actual metafile in memory
	}

}