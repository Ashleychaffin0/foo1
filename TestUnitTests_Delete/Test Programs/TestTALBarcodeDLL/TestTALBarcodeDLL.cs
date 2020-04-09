// Copywright (c) 2007 Bartizan Connects LLC

// #define OUTPUT_TO_CLIPBOARD
#define OUTPUT_TO_DISK
// #define OUTPUT_TO_MEMORYMETAFILE
// #define OUTPUT_TO_DC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Net;

namespace TestTALBarcodeDLL {

	public partial class TestTALBarcodeDLL : Form {

		[DllImport("TALPDF32.dll", EntryPoint = "TALPDFCODE")]
		public static extern int TALPDFCode(ref TALPDFBarCodeParms bc, ref MetaFilePict mf);

		Bitmap	bmp = null;
		PropertyGrid	PropGrid;
		TALPDFBarCodeParms bp = new TALPDFBarCodeParms();	// bp = Barcode Parms
		const char	ModuleBlack = '1';
		const char	ModuleWhite = '0';

		bool		bColorBitmap = false;
		Color[]		RowColors = new Color[] { Color.Red, Color.Green, Color.Blue };

//---------------------------------------------------------------------------------------

		public TestTALBarcodeDLL() {
			InitializeComponent();

#if false
			string	IPAddr = "129.42.17.103";		// IBM
			IPHostEntry he = Dns.Resolve(IPAddr);
			Console.WriteLine("Host name for {0} is {1}", IPAddr, he.HostName);
#endif

#if false
			GenerateAllCodewords();
#endif

#if false
			// 
			// PropGrid
			// 
			PropGrid = new PropertyGrid();
			this.PropGrid.Dock = System.Windows.Forms.DockStyle.Right;
			this.PropGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.PropGrid.Location = new System.Drawing.Point(564, 0);
			this.PropGrid.Name = "PropGrid";
			this.PropGrid.Size = new System.Drawing.Size(183, 1588);
			this.PropGrid.TabIndex = 0;
			this.Controls.Add(PropGrid);
			this.PropGrid.SelectedObject = bp;
			// this.PropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropGrid_PropertyValueChanged);
#endif

			if (PropGrid == null) {
				lbMsgs.Width = this.Width - lbMsgs.Left - lbMsgs.Margin.Right;
				lbDecodeBMP.Width = this.Width - lbDecodeBMP.Left - lbDecodeBMP.Margin.Right;
			}
		}

//---------------------------------------------------------------------------------------

		private void GenerateAllCodewords() {
			char [] modules;
			int		ValidCount = 0;
			for (int i = 0; i < (int)Math.Pow(2, 17); i++) {
				modules = GetBits(i);
				if (i == 66090) {
					System.Diagnostics.Debugger.Break();
				}
				ValidCount += CheckForValidCodeword(modules, i);
			}
			Console.WriteLine("ValidCount = {0}", ValidCount);
		}

//---------------------------------------------------------------------------------------

		int CheckForValidCodeword(char[] modules, int i) {
			if (modules[0] != ModuleBlack) {		// Leftmost module must be black
				return 0;
			}
			if (modules[16] != ModuleWhite) {		// Rightmost module must be white
				return 0;
			}
			bool	bInvalid;
#if false
			int		nRunsBlack, nRunsWhite;
#endif
			char []	xSequence = new char[8];
			// bTooLong = CountRuns(modules, out nRunsBlack, out nRunsWhite, ref xSequence);
			bInvalid = CountRuns(modules, ref xSequence);
			if (bInvalid) {
				return 0;
			}
#if false
			if ((nRunsBlack != 4) || (nRunsWhite != 4)) {
				return 0;
			}
#endif
#if false
			Console.Write("{0}: i = {1} / 0x{1:X}  ", ++nValidCodewords, i);
			foreach (char c in modules) {
				Console.Write(c);
			}
			Console.Write("  ");
			foreach (char c in xSequence) {
				Console.Write(c);
			}
			Console.WriteLine();
#endif
			return 1;
		}

//---------------------------------------------------------------------------------------

		private bool CountRuns(char[] modules, ref char [] xSequence) {
		// private bool CountRuns(char[] modules, out int nRunsBlack, out int nRunsWhite, ref char [] xSequence) {
			// TODO: No longer need nRunsBlack/nRunsWhite
			byte		State;				// Should really be an enum
			const byte	White = 0, Black = 1;
			const int	MaxRun = 6;	
			int			TotalWidth = 0;
			// We've already filtered out cases where modules[0] != ModuleBlack
			int			nRunsBlack = 1;
			int			nRunsWhite = 0;
			State = Black;
			int		CurrentRunCount = 1;				// i.e. the first black module
			int		ixSeq = 0;
			for (int i = 1; i < 17; i++) {				// Note we start at 1
				char	CurChar = modules[i];
				switch (State) {
				case Black:
					if (CurChar == ModuleBlack) {
						++CurrentRunCount;
						if (CurrentRunCount > MaxRun) {	// Can't have run more than 6 modules
							return true;
						}
					} else {
						++nRunsWhite;					// Start of new run of Whites
						if (nRunsWhite > 4) {
							return true;
						}
						TotalWidth += CurrentRunCount;
						xSequence[ixSeq++] = Convert.ToChar(0x30 + CurrentRunCount);	// Kludge
						State = White;
						CurrentRunCount = 1;
					}
					break;
				case White:
					if (CurChar == ModuleWhite) {
						++CurrentRunCount;
						if (CurrentRunCount > MaxRun) {	// Can't have run more than 6 modules
							return true;
						}
					} else {
						++nRunsBlack;					// Start of new run of Blacks
						if (nRunsBlack > 4) {
							return true;
						}
						TotalWidth += CurrentRunCount;
						xSequence[ixSeq++] = Convert.ToChar(0x30 + CurrentRunCount);	// Kludge
						State = Black;
						CurrentRunCount = 1;
					}
					break;
				}
			}

			if ((nRunsBlack != 4) || (nRunsWhite != 4)) 
				return true;

			TotalWidth += CurrentRunCount;
			xSequence[ixSeq] = Convert.ToChar(0x30 + CurrentRunCount);	// Kludge
			System.Diagnostics.Debug.Assert(TotalWidth == 17);
			return false;								// Each run is a reasonable length
		}

//---------------------------------------------------------------------------------------

		private char[] GetBits(int num) {
			char [] bits = new char[17];
			for (int i = 0; i < 17; i++) {
				bits[i] = ModuleWhite;		// Default to spaces
			}
			int	j = 16;				// Right-most bit
			while (num > 0) {
				if ((num & 1) == 1) {
					bits[j] = ModuleBlack;
				}
				num >>= 1;
				j--;
			}
			return bits;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			lbMsgs.Items.Clear();
			lbDecodeBMP.Items.Clear();
			string	text = txtBarcodeText.Text;
			// text = text.PadRight(200);
			TALBarcodePDF417	pdf = new TALBarcodePDF417(300, 300);
			// TALBarcodePDF417	pdf = new TALBarcodePDF417(100, 100);
			MetaFilePict mfp;
			bmp = pdf.CreateBitMap(text, ref bp, out mfp);
			double		nPixelsWidth, nPixelsHeight;
			PDFGetPixelCount(bp, out nPixelsWidth, out nPixelsHeight);
			lbMsgs.Items.Add(string.Format("In .01mm, Module Width={0}, Height={1}", bp.PDFModuleWidth, bp.PDFModuleHeight));
			lbMsgs.Items.Add(string.Format("HorizontalDPI={0}, VerticalDPI={1}", bp.HorizontalDPI, bp.VerticalDPI));
			lbMsgs.Items.Add(string.Format("In Pixels, Module Width={0:F3}, Height={1:F3}", nPixelsWidth, nPixelsHeight));
			if (bmp != null) {
				lbMsgs.Items.Add(string.Format("{0} x {1}", bmp.Width, bmp.Height));
			}
			lbMsgs.Items.Add(string.Format("mfp.xExt = {0}, .yExt = {1}", mfp.xExt, mfp.yExt));
			Application.DoEvents();
			
			// ColorBitmap(bmp);
			// DecodeBMP(bmp);

			// Bitmap	bmp2 = new Bitmap(bmp, new Size(bmp.Width * 2, bmp.Height * 2));
			// pictureBox1.Size  = bmp.Size;
			// pictureBox1.BorderStyle = BorderStyle.Fixed3D;
			// pictureBox1.BackColor = Color.Red;
			// pictureBox1.Image = bmp;
			// pictureBox1.Scale(new SizeF(2.0f, 2.0f));
			pictureBox1.Image = new Bitmap(bmp, pictureBox1.Size);
			// pictureBox1.Image = bmp;

			this.Invalidate();			// Force paint event
		}

//---------------------------------------------------------------------------------------

		private void ColorBitmap(Bitmap bmp) {
			if (! bColorBitmap) {
				return;
			}
			Color		RowColor;
			int Black = Color.Black.ToArgb();
			for (int row = 0; row < bmp.Height; row++) {
				RowColor = RowColors[row % 3];
				for (int col = 0; col < bmp.Width; col++) {
					Color c = bmp.GetPixel(col, row);
					if (c.ToArgb() == Black) {
						bmp.SetPixel(col, row, RowColor);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void PDFGetPixelCount(TALPDFBarCodeParms bp, out double nPixelsWidth, out double nPixelsHeight) {
			// Module width is in .01mm units, as is Module Height. Use HorizontalDPI
			// and VerticalDPI to calculate how many pixels should be used for a single
			// module's width and height.

			// To convert from .01mm to inches (which we'll then divide by DPI), we
			// calculate as follows.
			//		1 inch = 2.54 cm = 25.4mm = 2540 (.01mm)
			//	or	.01mm = 1 / 2540 inches.
			
			double WidthInInches = bp.PDFModuleWidth / 2540.0;
			double HeightInInches = bp.PDFModuleHeight / 2540.0;
			// nPixelsWidth  = (double)Math.Round(WidthInInches * bp.HorizontalDPI);
			// nPixelsHeight = (double)Math.Round(HeightInInches * bp.VerticalDPI);
			nPixelsWidth  = WidthInInches * bp.HorizontalDPI;
			nPixelsHeight = HeightInInches * bp.VerticalDPI;
		}

//---------------------------------------------------------------------------------------

		private void DecodeBMP(Bitmap bmp) {
			lbDecodeBMP.BeginUpdate();
			this.Cursor = Cursors.WaitCursor;
			int		Black = Color.Black.ToArgb();

			bool	IsStateBlack;
			int		ModuleCount;
			int		TotalForRow;
			for (int row = 0; row < bmp.Height; row++) {
				string	msg = string.Format("row[{0}] ", row).PadRight(10);
				IsStateBlack = true;		// Assume each row starts with black
				ModuleCount = 0;
				TotalForRow = 0;
				if (bColorBitmap) {
					Black = RowColors[row % 3].ToArgb();
				}
				for (int col = 0; col < bmp.Width; col++) {
					Color	c = bmp.GetPixel(col, row);
					// Console.WriteLine("bmp[{0}, {1}] = {2}", row, col, c);
					bool	IsCurrentPixelBlack = c.ToArgb() == Black;
#if false
					msg += IsCurrentPixelBlack ? "|" : "<";
#else
					if (IsStateBlack && IsCurrentPixelBlack) {
						// Scanning black and found another black
						++ModuleCount;		
					} else if (IsStateBlack && (!IsCurrentPixelBlack)) {
						// Scanning black and found white
						msg += string.Format("B={0},", ModuleCount);
						TotalForRow += ModuleCount;
						ModuleCount = 1;
					} else if ((!IsStateBlack) && IsCurrentPixelBlack) {
						// Scanning white and found black
						msg += string.Format("W={0},", ModuleCount);
						TotalForRow += ModuleCount;
						ModuleCount = 1;
					} else {
						// Scanning white and found white
						++ModuleCount;
					}
					IsStateBlack = IsCurrentPixelBlack;		// Update (maybe) state
#endif
				}
				// Handle last group on this row
				if (IsStateBlack) {
					msg += string.Format("B={0}", ModuleCount);
				} else {
					msg += string.Format("W={0}", ModuleCount);
				}
				msg += string.Format(",T={0}", TotalForRow);
				lbDecodeBMP.Items.Add(msg);
			}
			lbDecodeBMP.EndUpdate();
			this.Cursor = Cursors.Arrow;
		}

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, EventArgs e) {
			if (bmp == null) {
				MessageBox.Show("Must click Go first", "Test Barcode");
				return;
			}
			pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
			printPreviewDialog1.Document = pd;
			printPreviewDialog1.ShowDialog();
		}

//---------------------------------------------------------------------------------------

		void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			e.Graphics.DrawImage(bmp, 100, 100);
			// Rectangle	r = new Rectangle(new Point(100, 100), bmp.Size);
			// e.Graphics.DrawRectangle(Pens.Red, r);
		}

//---------------------------------------------------------------------------------------

		private void btnPrintWindow_Click(object sender, EventArgs e) {
			pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintWindowPage);
			printPreviewDialog1.Document = pd;
			printPreviewDialog1.ShowDialog();
		}

//---------------------------------------------------------------------------------------
		
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

		void pd_PrintWindowPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			bool bOK = PrintWindow(this.Handle, e.Graphics.GetHdc(), 0);
		}

//---------------------------------------------------------------------------------------

		private void panel1_Paint(object sender, PaintEventArgs e) {
			if (bmp == null) {
				return;
			}
			int		x = 0;
			int		y = pictureBox1.Bottom + 20;
			Font	fnt = new Font("Arial", 12);
			e.Graphics.DrawString("Hello world, how are you today?", fnt, Brushes.BlueViolet, new PointF(x, y - 10));
			float		Scale = 96.0f / 300f;
			int		w = (int)(bmp.Width * Scale);
			int		h = (int)(bmp.Height * Scale);
			// Point	pt = new Point(x, y);
			//  e.Graphics.DrawImage(bmp, pt);
			e.Graphics.DrawImage(bmp, x, y, w, h);
		}
	}



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

		public static int[][] CLUSTERS;

		Bitmap		offScreenBmp;
		Graphics	offScreenDC;
		Metafile	mf2;

//---------------------------------------------------------------------------------------

		[DllImport("TALPDF32.dll", EntryPoint = "TALPDFCODE")]
		public static extern int TALPDFCode(ref TALPDFBarCodeParms bc, ref MetaFilePict mf);

//---------------------------------------------------------------------------------------

		public TALBarcodePDF417(int hRes, int vRes) {
			this.hRes = hRes;
			this.vRes = vRes;

			CLUSTERS = new int[3][];
			CLUSTERS[0] = new int[] {0x1d5c0, 0x1eaf0, 0x1f57c, 0x1d4e0, 0x1ea78, 0x1f53e, 0x1a8c0, 0x1d470, 0x1a860, 0x15040, 0x1a830, 0x15020, 0x1adc0, 0x1d6f0, 0x1eb7c, 0x1ace0, 0x1d678, 0x1eb3e, 0x158c0, 0x1ac70, 0x15860, 0x15dc0, 0x1aef0, 0x1d77c, 0x15ce0, 0x1ae78, 0x1d73e, 0x15c70, 0x1ae3c, 0x15ef0, 0x1af7c, 0x15e78, 0x1af3e, 0x15f7c, 0x1f5fa, 0x1d2e0, 0x1e978, 0x1f4be, 0x1a4c0, 0x1d270, 0x1e93c, 0x1a460, 0x1d238, 0x14840, 0x1a430, 0x1d21c, 0x14820, 0x1a418, 0x14810, 0x1a6e0, 0x1d378, 0x1e9be, 0x14cc0, 0x1a670, 0x1d33c, 0x14c60, 0x1a638, 0x1d31e, 0x14c30, 0x1a61c, 0x14ee0, 0x1a778, 0x1d3be, 0x14e70, 0x1a73c, 0x14e38, 0x1a71e, 0x14f78, 0x1a7be, 0x14f3c, 0x14f1e, 0x1a2c0, 0x1d170, 0x1e8bc, 0x1a260, 0x1d138, 0x1e89e, 0x14440, 0x1a230, 0x1d11c, 0x14420, 0x1a218, 0x14410, 0x14408, 0x146c0, 0x1a370, 0x1d1bc, 0x14660, 0x1a338, 0x1d19e, 0x14630, 0x1a31c, 0x14618, 0x1460c, 0x14770, 0x1a3bc, 0x14738, 0x1a39e, 0x1471c, 0x147bc, 0x1a160, 0x1d0b8, 0x1e85e, 0x14240, 0x1a130, 0x1d09c, 0x14220, 0x1a118, 0x1d08e, 0x14210, 0x1a10c, 0x14208, 0x1a106, 0x14360, 0x1a1b8, 0x1d0de, 0x14330, 0x1a19c, 0x14318, 0x1a18e, 0x1430c, 0x14306, 0x1a1de, 0x1438e, 0x14140, 0x1a0b0, 0x1d05c, 0x14120, 0x1a098, 0x1d04e, 0x14110, 0x1a08c, 0x14108, 0x1a086, 0x14104, 0x141b0, 0x14198, 0x1418c, 0x140a0, 0x1d02e, 0x1a04c, 0x1a046, 0x14082, 0x1cae0, 0x1e578, 0x1f2be, 0x194c0, 0x1ca70, 0x1e53c, 0x19460, 0x1ca38, 0x1e51e, 0x12840, 0x19430, 0x12820, 0x196e0, 0x1cb78, 0x1e5be, 0x12cc0, 0x19670, 0x1cb3c, 0x12c60, 0x19638, 0x12c30, 0x12c18, 0x12ee0, 0x19778, 0x1cbbe, 0x12e70, 0x1973c, 0x12e38, 0x12e1c, 0x12f78, 0x197be, 0x12f3c, 0x12fbe, 0x1dac0, 0x1ed70, 0x1f6bc, 0x1da60, 0x1ed38, 0x1f69e, 0x1b440, 0x1da30, 0x1ed1c, 0x1b420, 0x1da18, 0x1ed0e, 0x1b410, 0x1da0c, 0x192c0, 0x1c970, 0x1e4bc, 0x1b6c0, 0x19260, 0x1c938, 0x1e49e, 0x1b660, 0x1db38, 0x1ed9e, 0x16c40, 0x12420, 0x19218, 0x1c90e, 0x16c20, 0x1b618, 0x16c10, 0x126c0, 0x19370, 0x1c9bc, 0x16ec0, 0x12660, 0x19338, 0x1c99e, 0x16e60, 0x1b738, 0x1db9e, 0x16e30, 0x12618, 0x16e18, 0x12770, 
				0x193bc, 0x16f70, 0x12738, 0x1939e, 0x16f38, 0x1b79e, 0x16f1c, 0x127bc, 0x16fbc, 0x1279e, 0x16f9e, 0x1d960, 0x1ecb8, 0x1f65e, 0x1b240, 0x1d930, 0x1ec9c, 0x1b220, 0x1d918, 0x1ec8e, 0x1b210, 0x1d90c, 0x1b208, 0x1b204, 0x19160, 0x1c8b8, 0x1e45e, 0x1b360, 0x19130, 0x1c89c, 0x16640, 0x12220, 0x1d99c, 0x1c88e, 0x16620, 0x12210, 0x1910c, 0x16610, 0x1b30c, 0x19106, 0x12204, 0x12360, 0x191b8, 0x1c8de, 0x16760, 0x12330, 0x1919c, 0x16730, 0x1b39c, 0x1918e, 0x16718, 0x1230c, 0x12306, 0x123b8, 0x191de, 0x167b8, 0x1239c, 0x1679c, 0x1238e, 0x1678e, 0x167de, 0x1b140, 0x1d8b0, 0x1ec5c, 0x1b120, 0x1d898, 0x1ec4e, 0x1b110, 0x1d88c, 0x1b108, 0x1d886, 0x1b104, 0x1b102, 0x12140, 0x190b0, 0x1c85c, 0x16340, 0x12120, 0x19098, 0x1c84e, 0x16320, 0x1b198, 0x1d8ce, 0x16310, 0x12108, 0x19086, 0x16308, 0x1b186, 0x16304, 0x121b0, 0x190dc, 0x163b0, 0x12198, 0x190ce, 0x16398, 0x1b1ce, 0x1638c, 0x12186, 0x16386, 0x163dc, 0x163ce, 0x1b0a0, 0x1d858, 0x1ec2e, 0x1b090, 0x1d84c, 0x1b088, 0x1d846, 0x1b084, 0x1b082, 0x120a0, 0x19058, 0x1c82e, 0x161a0, 0x12090, 0x1904c, 0x16190, 0x1b0cc, 0x19046, 0x16188, 0x12084, 0x16184, 0x12082, 0x120d8, 0x161d8, 0x161cc, 0x161c6, 0x1d82c, 0x1d826, 0x1b042, 0x1902c, 0x12048, 0x160c8, 0x160c4, 0x160c2, 0x18ac0, 0x1c570, 0x1e2bc, 0x18a60, 0x1c538, 0x11440, 0x18a30, 0x1c51c, 0x11420, 0x18a18, 0x11410, 0x11408, 0x116c0, 0x18b70, 0x1c5bc, 0x11660, 0x18b38, 0x1c59e, 0x11630, 0x18b1c, 0x11618, 0x1160c, 0x11770, 0x18bbc, 0x11738, 0x18b9e, 0x1171c, 0x117bc, 0x1179e, 0x1cd60, 0x1e6b8, 0x1f35e, 0x19a40, 0x1cd30, 0x1e69c, 0x19a20, 0x1cd18, 0x1e68e, 0x19a10, 0x1cd0c, 0x19a08, 0x1cd06, 0x18960, 0x1c4b8, 0x1e25e, 0x19b60, 0x18930, 0x1c49c, 0x13640, 0x11220, 0x1cd9c, 0x1c48e, 0x13620, 0x19b18, 0x1890c, 0x13610, 0x11208, 0x13608, 0x11360, 0x189b8, 0x1c4de, 0x13760, 0x11330, 0x1cdde, 0x13730, 0x19b9c, 0x1898e, 0x13718, 0x1130c, 0x1370c, 0x113b8, 0x189de, 0x137b8, 0x1139c, 0x1379c, 0x1138e, 0x113de, 0x137de, 0x1dd40, 0x1eeb0, 0x1f75c, 0x1dd20, 0x1ee98, 0x1f74e, 0x1dd10, 0x1ee8c, 0x1dd08, 0x1ee86, 0x1dd04, 0x19940, 0x1ccb0, 
				0x1e65c, 0x1bb40, 0x19920, 0x1eedc, 0x1e64e, 0x1bb20, 0x1dd98, 0x1eece, 0x1bb10, 0x19908, 0x1cc86, 0x1bb08, 0x1dd86, 0x19902, 0x11140, 0x188b0, 0x1c45c, 0x13340, 0x11120, 0x18898, 0x1c44e, 0x17740, 0x13320, 0x19998, 0x1ccce, 0x17720, 0x1bb98, 0x1ddce, 0x18886, 0x17710, 0x13308, 0x19986, 0x17708, 0x11102, 0x111b0, 0x188dc, 0x133b0, 0x11198, 0x188ce, 0x177b0, 0x13398, 0x199ce, 0x17798, 0x1bbce, 0x11186, 0x13386, 0x111dc, 0x133dc, 0x111ce, 0x177dc, 0x133ce, 0x1dca0, 0x1ee58, 0x1f72e, 0x1dc90, 0x1ee4c, 0x1dc88, 0x1ee46, 0x1dc84, 0x1dc82, 0x198a0, 0x1cc58, 0x1e62e, 0x1b9a0, 0x19890, 0x1ee6e, 0x1b990, 0x1dccc, 0x1cc46, 0x1b988, 0x19884, 0x1b984, 0x19882, 0x1b982, 0x110a0, 0x18858, 0x1c42e, 0x131a0, 0x11090, 0x1884c, 0x173a0, 0x13190, 0x198cc, 0x18846, 0x17390, 0x1b9cc, 0x11084, 0x17388, 0x13184, 0x11082, 0x13182, 0x110d8, 0x1886e, 0x131d8, 0x110cc, 0x173d8, 0x131cc, 0x110c6, 0x173cc, 0x131c6, 0x110ee, 0x173ee, 0x1dc50, 0x1ee2c, 0x1dc48, 0x1ee26, 0x1dc44, 0x1dc42, 0x19850, 0x1cc2c, 0x1b8d0, 0x19848, 0x1cc26, 0x1b8c8, 0x1dc66, 0x1b8c4, 0x19842, 0x1b8c2, 0x11050, 0x1882c, 0x130d0, 0x11048, 0x18826, 0x171d0, 0x130c8, 0x19866, 0x171c8, 0x1b8e6, 0x11042, 0x171c4, 0x130c2, 0x171c2, 0x130ec, 0x171ec, 0x171e6, 0x1ee16, 0x1dc22, 0x1cc16, 0x19824, 0x19822, 0x11028, 0x13068, 0x170e8, 0x11022, 0x13062, 0x18560, 0x10a40, 0x18530, 0x10a20, 0x18518, 0x1c28e, 0x10a10, 0x1850c, 0x10a08, 0x18506, 0x10b60, 0x185b8, 0x1c2de, 0x10b30, 0x1859c, 0x10b18, 0x1858e, 0x10b0c, 0x10b06, 0x10bb8, 0x185de, 0x10b9c, 0x10b8e, 0x10bde, 0x18d40, 0x1c6b0, 0x1e35c, 0x18d20, 0x1c698, 0x18d10, 0x1c68c, 0x18d08, 0x1c686, 0x18d04, 0x10940, 0x184b0, 0x1c25c, 0x11b40, 0x10920, 0x1c6dc, 0x1c24e, 0x11b20, 0x18d98, 0x1c6ce, 0x11b10, 0x10908, 0x18486, 0x11b08, 0x18d86, 0x10902, 0x109b0, 0x184dc, 0x11bb0, 0x10998, 0x184ce, 0x11b98, 0x18dce, 0x11b8c, 0x10986, 0x109dc, 0x11bdc, 0x109ce, 0x11bce, 0x1cea0, 0x1e758, 0x1f3ae, 0x1ce90, 0x1e74c, 0x1ce88, 0x1e746, 0x1ce84, 0x1ce82, 0x18ca0, 0x1c658, 0x19da0, 0x18c90, 0x1c64c, 0x19d90, 0x1cecc, 0x1c646, 0x19d88, 
				0x18c84, 0x19d84, 0x18c82, 0x19d82, 0x108a0, 0x18458, 0x119a0, 0x10890, 0x1c66e, 0x13ba0, 0x11990, 0x18ccc, 0x18446, 0x13b90, 0x19dcc, 0x10884, 0x13b88, 0x11984, 0x10882, 0x11982, 0x108d8, 0x1846e, 0x119d8, 0x108cc, 0x13bd8, 0x119cc, 0x108c6, 0x13bcc, 0x119c6, 0x108ee, 0x119ee, 0x13bee, 0x1ef50, 0x1f7ac, 0x1ef48, 0x1f7a6, 0x1ef44, 0x1ef42, 0x1ce50, 0x1e72c, 0x1ded0, 0x1ef6c, 0x1e726, 0x1dec8, 0x1ef66, 0x1dec4, 0x1ce42, 0x1dec2, 0x18c50, 0x1c62c, 0x19cd0, 0x18c48, 0x1c626, 0x1bdd0, 0x19cc8, 0x1ce66, 0x1bdc8, 0x1dee6, 0x18c42, 0x1bdc4, 0x19cc2, 0x1bdc2, 0x10850, 0x1842c, 0x118d0, 0x10848, 0x18426, 0x139d0, 0x118c8, 0x18c66, 0x17bd0, 0x139c8, 0x19ce6, 0x10842, 0x17bc8, 0x1bde6, 0x118c2, 0x17bc4, 0x1086c, 0x118ec, 0x10866, 0x139ec, 0x118e6, 0x17bec, 0x139e6, 0x17be6, 0x1ef28, 0x1f796, 0x1ef24, 0x1ef22, 0x1ce28, 0x1e716, 0x1de68, 0x1ef36, 0x1de64, 0x1ce22, 0x1de62, 0x18c28, 0x1c616, 0x19c68, 0x18c24, 0x1bce8, 0x19c64, 0x18c22, 0x1bce4, 0x19c62, 0x1bce2, 0x10828, 0x18416, 0x11868, 0x18c36, 0x138e8, 0x11864, 0x10822, 0x179e8, 0x138e4, 0x11862, 0x179e4, 0x138e2, 0x179e2, 0x11876, 0x179f6, 0x1ef12, 0x1de34, 0x1de32, 0x19c34, 0x1bc74, 0x1bc72, 0x11834, 0x13874, 0x178f4, 0x178f2, 0x10540, 0x10520, 0x18298, 0x10510, 0x10508, 0x10504, 0x105b0, 0x10598, 0x1058c, 0x10586, 0x105dc, 0x105ce, 0x186a0, 0x18690, 0x1c34c, 0x18688, 0x1c346, 0x18684, 0x18682, 0x104a0, 0x18258, 0x10da0, 0x186d8, 0x1824c, 0x10d90, 0x186cc, 0x10d88, 0x186c6, 0x10d84, 0x10482, 0x10d82, 0x104d8, 0x1826e, 0x10dd8, 0x186ee, 0x10dcc, 0x104c6, 0x10dc6, 0x104ee, 0x10dee, 0x1c750, 0x1c748, 0x1c744, 0x1c742, 0x18650, 0x18ed0, 0x1c76c, 0x1c326, 0x18ec8, 0x1c766, 0x18ec4, 0x18642, 0x18ec2, 0x10450, 0x10cd0, 0x10448, 0x18226, 0x11dd0, 0x10cc8, 0x10444, 0x11dc8, 0x10cc4, 0x10442, 0x11dc4, 0x10cc2, 0x1046c, 0x10cec, 0x10466, 0x11dec, 0x10ce6, 0x11de6, 0x1e7a8, 0x1e7a4, 0x1e7a2, 0x1c728, 0x1cf68, 0x1e7b6, 0x1cf64, 0x1c722, 0x1cf62, 0x18628, 0x1c316, 0x18e68, 0x1c736, 0x19ee8, 0x18e64, 0x18622, 0x19ee4, 0x18e62, 0x19ee2, 0x10428, 0x18216, 0x10c68, 0x18636, 
				0x11ce8, 0x10c64, 0x10422, 0x13de8, 0x11ce4, 0x10c62, 0x13de4, 0x11ce2, 0x10436, 0x10c76, 0x11cf6, 0x13df6, 0x1f7d4, 0x1f7d2, 0x1e794, 0x1efb4, 0x1e792, 0x1efb2, 0x1c714, 0x1cf34, 0x1c712, 0x1df74, 0x1cf32, 0x1df72, 0x18614, 0x18e34, 0x18612, 0x19e74, 0x18e32, 0x1bef4};
			CLUSTERS[1] = new int[] {0x1f560, 0x1fab8, 0x1ea40, 0x1f530, 0x1fa9c, 0x1ea20, 0x1f518, 0x1fa8e, 0x1ea10, 0x1f50c, 0x1ea08, 0x1f506, 0x1ea04, 0x1eb60, 0x1f5b8, 0x1fade, 0x1d640, 0x1eb30, 0x1f59c, 0x1d620, 0x1eb18, 0x1f58e, 0x1d610, 0x1eb0c, 0x1d608, 0x1eb06, 0x1d604, 0x1d760, 0x1ebb8, 0x1f5de, 0x1ae40, 0x1d730, 0x1eb9c, 0x1ae20, 0x1d718, 0x1eb8e, 0x1ae10, 0x1d70c, 0x1ae08, 0x1d706, 0x1ae04, 0x1af60, 0x1d7b8, 0x1ebde, 0x15e40, 0x1af30, 0x1d79c, 0x15e20, 0x1af18, 0x1d78e, 0x15e10, 0x1af0c, 0x15e08, 0x1af06, 0x15f60, 0x1afb8, 0x1d7de, 0x15f30, 0x1af9c, 0x15f18, 0x1af8e, 0x15f0c, 0x15fb8, 0x1afde, 0x15f9c, 0x15f8e, 0x1e940, 0x1f4b0, 0x1fa5c, 0x1e920, 0x1f498, 0x1fa4e, 0x1e910, 0x1f48c, 0x1e908, 0x1f486, 0x1e904, 0x1e902, 0x1d340, 0x1e9b0, 0x1f4dc, 0x1d320, 0x1e998, 0x1f4ce, 0x1d310, 0x1e98c, 0x1d308, 0x1e986, 0x1d304, 0x1d302, 0x1a740, 0x1d3b0, 0x1e9dc, 0x1a720, 0x1d398, 0x1e9ce, 0x1a710, 0x1d38c, 0x1a708, 0x1d386, 0x1a704, 0x1a702, 0x14f40, 0x1a7b0, 0x1d3dc, 0x14f20, 0x1a798, 0x1d3ce, 0x14f10, 0x1a78c, 0x14f08, 0x1a786, 0x14f04, 0x14fb0, 0x1a7dc, 0x14f98, 0x1a7ce, 0x14f8c, 0x14f86, 0x14fdc, 0x14fce, 0x1e8a0, 0x1f458, 0x1fa2e, 0x1e890, 0x1f44c, 0x1e888, 0x1f446, 0x1e884, 0x1e882, 0x1d1a0, 0x1e8d8, 0x1f46e, 0x1d190, 0x1e8cc, 0x1d188, 0x1e8c6, 0x1d184, 0x1d182, 0x1a3a0, 0x1d1d8, 0x1e8ee, 0x1a390, 0x1d1cc, 0x1a388, 0x1d1c6, 0x1a384, 0x1a382, 0x147a0, 0x1a3d8, 0x1d1ee, 0x14790, 0x1a3cc, 0x14788, 0x1a3c6, 0x14784, 0x14782, 0x147d8, 0x1a3ee, 0x147cc, 0x147c6, 0x147ee, 0x1e850, 0x1f42c, 0x1e848, 0x1f426, 0x1e844, 0x1e842, 0x1d0d0, 0x1e86c, 0x1d0c8, 0x1e866, 0x1d0c4, 0x1d0c2, 0x1a1d0, 0x1d0ec, 0x1a1c8, 0x1d0e6, 0x1a1c4, 0x1a1c2, 0x143d0, 0x1a1ec, 0x143c8, 0x1a1e6, 0x143c4, 0x143c2, 0x143ec, 0x143e6, 0x1e828, 0x1f416, 0x1e824, 0x1e822, 0x1d068, 0x1e836, 0x1d064, 
				0x1d062, 0x1a0e8, 0x1d076, 0x1a0e4, 0x1a0e2, 0x141e8, 0x1a0f6, 0x141e4, 0x141e2, 0x1e814, 0x1e812, 0x1d034, 0x1d032, 0x1a074, 0x1a072, 0x1e540, 0x1f2b0, 0x1f95c, 0x1e520, 0x1f298, 0x1f94e, 0x1e510, 0x1f28c, 0x1e508, 0x1f286, 0x1e504, 0x1e502, 0x1cb40, 0x1e5b0, 0x1f2dc, 0x1cb20, 0x1e598, 0x1f2ce, 0x1cb10, 0x1e58c, 0x1cb08, 0x1e586, 0x1cb04, 0x1cb02, 0x19740, 0x1cbb0, 0x1e5dc, 0x19720, 0x1cb98, 0x1e5ce, 0x19710, 0x1cb8c, 0x19708, 0x1cb86, 0x19704, 0x19702, 0x12f40, 0x197b0, 0x1cbdc, 0x12f20, 0x19798, 0x1cbce, 0x12f10, 0x1978c, 0x12f08, 0x19786, 0x12f04, 0x12fb0, 0x197dc, 0x12f98, 0x197ce, 0x12f8c, 0x12f86, 0x12fdc, 0x12fce, 0x1f6a0, 0x1fb58, 0x16bf0, 0x1f690, 0x1fb4c, 0x169f8, 0x1f688, 0x1fb46, 0x168fc, 0x1f684, 0x1f682, 0x1e4a0, 0x1f258, 0x1f92e, 0x1eda0, 0x1e490, 0x1fb6e, 0x1ed90, 0x1f6cc, 0x1f246, 0x1ed88, 0x1e484, 0x1ed84, 0x1e482, 0x1ed82, 0x1c9a0, 0x1e4d8, 0x1f26e, 0x1dba0, 0x1c990, 0x1e4cc, 0x1db90, 0x1edcc, 0x1e4c6, 0x1db88, 0x1c984, 0x1db84, 0x1c982, 0x1db82, 0x193a0, 0x1c9d8, 0x1e4ee, 0x1b7a0, 0x19390, 0x1c9cc, 0x1b790, 0x1dbcc, 0x1c9c6, 0x1b788, 0x19384, 0x1b784, 0x19382, 0x1b782, 0x127a0, 0x193d8, 0x1c9ee, 0x16fa0, 0x12790, 0x193cc, 0x16f90, 0x1b7cc, 0x193c6, 0x16f88, 0x12784, 0x16f84, 0x12782, 0x127d8, 0x193ee, 0x16fd8, 0x127cc, 0x16fcc, 0x127c6, 0x16fc6, 0x127ee, 0x1f650, 0x1fb2c, 0x165f8, 0x1f648, 0x1fb26, 0x164fc, 0x1f644, 0x1647e, 0x1f642, 0x1e450, 0x1f22c, 0x1ecd0, 0x1e448, 0x1f226, 0x1ecc8, 0x1f666, 0x1ecc4, 0x1e442, 0x1ecc2, 0x1c8d0, 0x1e46c, 0x1d9d0, 0x1c8c8, 0x1e466, 0x1d9c8, 0x1ece6, 0x1d9c4, 0x1c8c2, 0x1d9c2, 0x191d0, 0x1c8ec, 0x1b3d0, 0x191c8, 0x1c8e6, 0x1b3c8, 0x1d9e6, 0x1b3c4, 0x191c2, 0x1b3c2, 0x123d0, 0x191ec, 0x167d0, 0x123c8, 0x191e6, 0x167c8, 0x1b3e6, 0x167c4, 0x123c2, 0x167c2, 0x123ec, 0x167ec, 0x123e6, 0x167e6, 0x1f628, 0x1fb16, 0x162fc, 0x1f624, 0x1627e, 0x1f622, 0x1e428, 0x1f216, 0x1ec68, 0x1f636, 0x1ec64, 0x1e422, 0x1ec62, 0x1c868, 0x1e436, 0x1d8e8, 0x1c864, 0x1d8e4, 0x1c862, 0x1d8e2, 0x190e8, 0x1c876, 0x1b1e8, 0x1d8f6, 0x1b1e4, 0x190e2, 0x1b1e2, 0x121e8, 0x190f6, 
				0x163e8, 0x121e4, 0x163e4, 0x121e2, 0x163e2, 0x121f6, 0x163f6, 0x1f614, 0x1617e, 0x1f612, 0x1e414, 0x1ec34, 0x1e412, 0x1ec32, 0x1c834, 0x1d874, 0x1c832, 0x1d872, 0x19074, 0x1b0f4, 0x19072, 0x1b0f2, 0x120f4, 0x161f4, 0x120f2, 0x161f2, 0x1f60a, 0x1e40a, 0x1ec1a, 0x1c81a, 0x1d83a, 0x1903a, 0x1b07a, 0x1e2a0, 0x1f158, 0x1f8ae, 0x1e290, 0x1f14c, 0x1e288, 0x1f146, 0x1e284, 0x1e282, 0x1c5a0, 0x1e2d8, 0x1f16e, 0x1c590, 0x1e2cc, 0x1c588, 0x1e2c6, 0x1c584, 0x1c582, 0x18ba0, 0x1c5d8, 0x1e2ee, 0x18b90, 0x1c5cc, 0x18b88, 0x1c5c6, 0x18b84, 0x18b82, 0x117a0, 0x18bd8, 0x1c5ee, 0x11790, 0x18bcc, 0x11788, 0x18bc6, 0x11784, 0x11782, 0x117d8, 0x18bee, 0x117cc, 0x117c6, 0x117ee, 0x1f350, 0x1f9ac, 0x135f8, 0x1f348, 0x1f9a6, 0x134fc, 0x1f344, 0x1347e, 0x1f342, 0x1e250, 0x1f12c, 0x1e6d0, 0x1e248, 0x1f126, 0x1e6c8, 0x1f366, 0x1e6c4, 0x1e242, 0x1e6c2, 0x1c4d0, 0x1e26c, 0x1cdd0, 0x1c4c8, 0x1e266, 0x1cdc8, 0x1e6e6, 0x1cdc4, 0x1c4c2, 0x1cdc2, 0x189d0, 0x1c4ec, 0x19bd0, 0x189c8, 0x1c4e6, 0x19bc8, 0x1cde6, 0x19bc4, 0x189c2, 0x19bc2, 0x113d0, 0x189ec, 0x137d0, 0x113c8, 0x189e6, 0x137c8, 0x19be6, 0x137c4, 0x113c2, 0x137c2, 0x113ec, 0x137ec, 0x113e6, 0x137e6, 0x1fba8, 0x175f0, 0x1bafc, 0x1fba4, 0x174f8, 0x1ba7e, 0x1fba2, 0x1747c, 0x1743e, 0x1f328, 0x1f996, 0x132fc, 0x1f768, 0x1fbb6, 0x176fc, 0x1327e, 0x1f764, 0x1f322, 0x1767e, 0x1f762, 0x1e228, 0x1f116, 0x1e668, 0x1e224, 0x1eee8, 0x1f776, 0x1e222, 0x1eee4, 0x1e662, 0x1eee2, 0x1c468, 0x1e236, 0x1cce8, 0x1c464, 0x1dde8, 0x1cce4, 0x1c462, 0x1dde4, 0x1cce2, 0x1dde2, 0x188e8, 0x1c476, 0x199e8, 0x188e4, 0x1bbe8, 0x199e4, 0x188e2, 0x1bbe4, 0x199e2, 0x1bbe2, 0x111e8, 0x188f6, 0x133e8, 0x111e4, 0x177e8, 0x133e4, 0x111e2, 0x177e4, 0x133e2, 0x177e2, 0x111f6, 0x133f6, 0x1fb94, 0x172f8, 0x1b97e, 0x1fb92, 0x1727c, 0x1723e, 0x1f314, 0x1317e, 0x1f734, 0x1f312, 0x1737e, 0x1f732, 0x1e214, 0x1e634, 0x1e212, 0x1ee74, 0x1e632, 0x1ee72, 0x1c434, 0x1cc74, 0x1c432, 0x1dcf4, 0x1cc72, 0x1dcf2, 0x18874, 0x198f4, 0x18872, 0x1b9f4, 0x198f2, 0x1b9f2, 0x110f4, 0x131f4, 0x110f2, 0x173f4, 0x131f2, 0x173f2, 0x1fb8a, 
				0x1717c, 0x1713e, 0x1f30a, 0x1f71a, 0x1e20a, 0x1e61a, 0x1ee3a, 0x1c41a, 0x1cc3a, 0x1dc7a, 0x1883a, 0x1987a, 0x1b8fa, 0x1107a, 0x130fa, 0x171fa, 0x170be, 0x1e150, 0x1f0ac, 0x1e148, 0x1f0a6, 0x1e144, 0x1e142, 0x1c2d0, 0x1e16c, 0x1c2c8, 0x1e166, 0x1c2c4, 0x1c2c2, 0x185d0, 0x1c2ec, 0x185c8, 0x1c2e6, 0x185c4, 0x185c2, 0x10bd0, 0x185ec, 0x10bc8, 0x185e6, 0x10bc4, 0x10bc2, 0x10bec, 0x10be6, 0x1f1a8, 0x1f8d6, 0x11afc, 0x1f1a4, 0x11a7e, 0x1f1a2, 0x1e128, 0x1f096, 0x1e368, 0x1e124, 0x1e364, 0x1e122, 0x1e362, 0x1c268, 0x1e136, 0x1c6e8, 0x1c264, 0x1c6e4, 0x1c262, 0x1c6e2, 0x184e8, 0x1c276, 0x18de8, 0x184e4, 0x18de4, 0x184e2, 0x18de2, 0x109e8, 0x184f6, 0x11be8, 0x109e4, 0x11be4, 0x109e2, 0x11be2, 0x109f6, 0x11bf6, 0x1f9d4, 0x13af8, 0x19d7e, 0x1f9d2, 0x13a7c, 0x13a3e, 0x1f194, 0x1197e, 0x1f3b4, 0x1f192, 0x13b7e, 0x1f3b2, 0x1e114, 0x1e334, 0x1e112, 0x1e774, 0x1e332, 0x1e772, 0x1c234, 0x1c674, 0x1c232, 0x1cef4, 0x1c672, 0x1cef2, 0x18474, 0x18cf4, 0x18472, 0x19df4, 0x18cf2, 0x19df2, 0x108f4, 0x119f4, 0x108f2, 0x13bf4, 0x119f2, 0x13bf2, 0x17af0, 0x1bd7c, 0x17a78, 0x1bd3e, 0x17a3c, 0x17a1e, 0x1f9ca, 0x1397c, 0x1fbda, 0x17b7c, 0x1393e, 0x17b3e, 0x1f18a, 0x1f39a, 0x1f7ba, 0x1e10a, 0x1e31a, 0x1e73a, 0x1ef7a, 0x1c21a, 0x1c63a, 0x1ce7a, 0x1defa, 0x1843a, 0x18c7a, 0x19cfa, 0x1bdfa, 0x1087a, 0x118fa, 0x139fa, 0x17978, 0x1bcbe, 0x1793c, 0x1791e, 0x138be, 0x179be, 0x178bc, 0x1789e, 0x1785e, 0x1e0a8, 0x1e0a4, 0x1e0a2, 0x1c168, 0x1e0b6, 0x1c164, 0x1c162, 0x182e8, 0x1c176, 0x182e4, 0x182e2, 0x105e8, 0x182f6, 0x105e4, 0x105e2, 0x105f6, 0x1f0d4, 0x10d7e, 0x1f0d2, 0x1e094, 0x1e1b4, 0x1e092, 0x1e1b2, 0x1c134, 0x1c374, 0x1c132, 0x1c372, 0x18274, 0x186f4, 0x18272, 0x186f2, 0x104f4, 0x10df4, 0x104f2, 0x10df2, 0x1f8ea, 0x11d7c, 0x11d3e, 0x1f0ca, 0x1f1da, 0x1e08a, 0x1e19a, 0x1e3ba, 0x1c11a, 0x1c33a, 0x1c77a, 0x1823a, 0x1867a, 0x18efa, 0x1047a, 0x10cfa, 0x11dfa, 0x13d78, 0x19ebe, 0x13d3c, 0x13d1e, 0x11cbe, 0x13dbe, 0x17d70, 0x1bebc, 0x17d38, 0x1be9e, 0x17d1c, 0x17d0e, 0x13cbc, 0x17dbc, 0x13c9e, 0x17d9e, 0x17cb8, 0x1be5e, 0x17c9c, 0x17c8e, 
				0x13c5e, 0x17cde, 0x17c5c, 0x17c4e, 0x17c2e, 0x1c0b4, 0x1c0b2, 0x18174, 0x18172, 0x102f4, 0x102f2, 0x1e0da, 0x1c09a, 0x1c1ba, 0x1813a, 0x1837a, 0x1027a, 0x106fa, 0x10ebe, 0x11ebc, 0x11e9e, 0x13eb8, 0x19f5e, 0x13e9c, 0x13e8e, 0x11e5e, 0x13ede, 0x17eb0, 0x1bf5c, 0x17e98, 0x1bf4e, 0x17e8c, 0x17e86, 0x13e5c, 0x17edc, 0x13e4e, 0x17ece, 0x17e58, 0x1bf2e, 0x17e4c, 0x17e46, 0x13e2e, 0x17e6e, 0x17e2c, 0x17e26, 0x10f5e, 0x11f5c, 0x11f4e, 0x13f58, 0x19fae, 0x13f4c, 0x13f46, 0x11f2e, 0x13f6e, 0x13f2c, 0x13f26};
			CLUSTERS[2] = new int[] {0x1abe0, 0x1d5f8, 0x153c0, 0x1a9f0, 0x1d4fc, 0x151e0, 0x1a8f8, 0x1d47e, 0x150f0, 0x1a87c, 0x15078, 0x1fad0, 0x15be0, 0x1adf8, 0x1fac8, 0x159f0, 0x1acfc, 0x1fac4, 0x158f8, 0x1ac7e, 0x1fac2, 0x1587c, 0x1f5d0, 0x1faec, 0x15df8, 0x1f5c8, 0x1fae6, 0x15cfc, 0x1f5c4, 0x15c7e, 0x1f5c2, 0x1ebd0, 0x1f5ec, 0x1ebc8, 0x1f5e6, 0x1ebc4, 0x1ebc2, 0x1d7d0, 0x1ebec, 0x1d7c8, 0x1ebe6, 0x1d7c4, 0x1d7c2, 0x1afd0, 0x1d7ec, 0x1afc8, 0x1d7e6, 0x1afc4, 0x14bc0, 0x1a5f0, 0x1d2fc, 0x149e0, 0x1a4f8, 0x1d27e, 0x148f0, 0x1a47c, 0x14878, 0x1a43e, 0x1483c, 0x1fa68, 0x14df0, 0x1a6fc, 0x1fa64, 0x14cf8, 0x1a67e, 0x1fa62, 0x14c7c, 0x14c3e, 0x1f4e8, 0x1fa76, 0x14efc, 0x1f4e4, 0x14e7e, 0x1f4e2, 0x1e9e8, 0x1f4f6, 0x1e9e4, 0x1e9e2, 0x1d3e8, 0x1e9f6, 0x1d3e4, 0x1d3e2, 0x1a7e8, 0x1d3f6, 0x1a7e4, 0x1a7e2, 0x145e0, 0x1a2f8, 0x1d17e, 0x144f0, 0x1a27c, 0x14478, 0x1a23e, 0x1443c, 0x1441e, 0x1fa34, 0x146f8, 0x1a37e, 0x1fa32, 0x1467c, 0x1463e, 0x1f474, 0x1477e, 0x1f472, 0x1e8f4, 0x1e8f2, 0x1d1f4, 0x1d1f2, 0x1a3f4, 0x1a3f2, 0x142f0, 0x1a17c, 0x14278, 0x1a13e, 0x1423c, 0x1421e, 0x1fa1a, 0x1437c, 0x1433e, 0x1f43a, 0x1e87a, 0x1d0fa, 0x14178, 0x1a0be, 0x1413c, 0x1411e, 0x141be, 0x140bc, 0x1409e, 0x12bc0, 0x195f0, 0x1cafc, 0x129e0, 0x194f8, 0x1ca7e, 0x128f0, 0x1947c, 0x12878, 0x1943e, 0x1283c, 0x1f968, 0x12df0, 0x196fc, 0x1f964, 0x12cf8, 0x1967e, 0x1f962, 0x12c7c, 0x12c3e, 0x1f2e8, 0x1f976, 0x12efc, 0x1f2e4, 0x12e7e, 0x1f2e2, 0x1e5e8, 0x1f2f6, 0x1e5e4, 0x1e5e2, 0x1cbe8, 0x1e5f6, 0x1cbe4, 0x1cbe2, 0x197e8, 0x1cbf6, 0x197e4, 0x197e2, 0x1b5e0, 0x1daf8, 
				0x1ed7e, 0x169c0, 0x1b4f0, 0x1da7c, 0x168e0, 0x1b478, 0x1da3e, 0x16870, 0x1b43c, 0x16838, 0x1b41e, 0x1681c, 0x125e0, 0x192f8, 0x1c97e, 0x16de0, 0x124f0, 0x1927c, 0x16cf0, 0x1b67c, 0x1923e, 0x16c78, 0x1243c, 0x16c3c, 0x1241e, 0x16c1e, 0x1f934, 0x126f8, 0x1937e, 0x1fb74, 0x1f932, 0x16ef8, 0x1267c, 0x1fb72, 0x16e7c, 0x1263e, 0x16e3e, 0x1f274, 0x1277e, 0x1f6f4, 0x1f272, 0x16f7e, 0x1f6f2, 0x1e4f4, 0x1edf4, 0x1e4f2, 0x1edf2, 0x1c9f4, 0x1dbf4, 0x1c9f2, 0x1dbf2, 0x193f4, 0x193f2, 0x165c0, 0x1b2f0, 0x1d97c, 0x164e0, 0x1b278, 0x1d93e, 0x16470, 0x1b23c, 0x16438, 0x1b21e, 0x1641c, 0x1640e, 0x122f0, 0x1917c, 0x166f0, 0x12278, 0x1913e, 0x16678, 0x1b33e, 0x1663c, 0x1221e, 0x1661e, 0x1f91a, 0x1237c, 0x1fb3a, 0x1677c, 0x1233e, 0x1673e, 0x1f23a, 0x1f67a, 0x1e47a, 0x1ecfa, 0x1c8fa, 0x1d9fa, 0x191fa, 0x162e0, 0x1b178, 0x1d8be, 0x16270, 0x1b13c, 0x16238, 0x1b11e, 0x1621c, 0x1620e, 0x12178, 0x190be, 0x16378, 0x1213c, 0x1633c, 0x1211e, 0x1631e, 0x121be, 0x163be, 0x16170, 0x1b0bc, 0x16138, 0x1b09e, 0x1611c, 0x1610e, 0x120bc, 0x161bc, 0x1209e, 0x1619e, 0x160b8, 0x1b05e, 0x1609c, 0x1608e, 0x1205e, 0x160de, 0x1605c, 0x1604e, 0x115e0, 0x18af8, 0x1c57e, 0x114f0, 0x18a7c, 0x11478, 0x18a3e, 0x1143c, 0x1141e, 0x1f8b4, 0x116f8, 0x18b7e, 0x1f8b2, 0x1167c, 0x1163e, 0x1f174, 0x1177e, 0x1f172, 0x1e2f4, 0x1e2f2, 0x1c5f4, 0x1c5f2, 0x18bf4, 0x18bf2, 0x135c0, 0x19af0, 0x1cd7c, 0x134e0, 0x19a78, 0x1cd3e, 0x13470, 0x19a3c, 0x13438, 0x19a1e, 0x1341c, 0x1340e, 0x112f0, 0x1897c, 0x136f0, 0x11278, 0x1893e, 0x13678, 0x19b3e, 0x1363c, 0x1121e, 0x1361e, 0x1f89a, 0x1137c, 0x1f9ba, 0x1377c, 0x1133e, 0x1373e, 0x1f13a, 0x1f37a, 0x1e27a, 0x1e6fa, 0x1c4fa, 0x1cdfa, 0x189fa, 0x1bae0, 0x1dd78, 0x1eebe, 0x174c0, 0x1ba70, 0x1dd3c, 0x17460, 0x1ba38, 0x1dd1e, 0x17430, 0x1ba1c, 0x17418, 0x1ba0e, 0x1740c, 0x132e0, 0x19978, 0x1ccbe, 0x176e0, 0x13270, 0x1993c, 0x17670, 0x1bb3c, 0x1991e, 0x17638, 0x1321c, 0x1761c, 0x1320e, 0x1760e, 0x11178, 0x188be, 0x13378, 0x1113c, 0x17778, 0x1333c, 0x1111e, 0x1773c, 0x1331e, 0x1771e, 0x111be, 0x133be, 0x177be, 0x172c0, 0x1b970, 
				0x1dcbc, 0x17260, 0x1b938, 0x1dc9e, 0x17230, 0x1b91c, 0x17218, 0x1b90e, 0x1720c, 0x17206, 0x13170, 0x198bc, 0x17370, 0x13138, 0x1989e, 0x17338, 0x1b99e, 0x1731c, 0x1310e, 0x1730e, 0x110bc, 0x131bc, 0x1109e, 0x173bc, 0x1319e, 0x1739e, 0x17160, 0x1b8b8, 0x1dc5e, 0x17130, 0x1b89c, 0x17118, 0x1b88e, 0x1710c, 0x17106, 0x130b8, 0x1985e, 0x171b8, 0x1309c, 0x1719c, 0x1308e, 0x1718e, 0x1105e, 0x130de, 0x171de, 0x170b0, 0x1b85c, 0x17098, 0x1b84e, 0x1708c, 0x17086, 0x1305c, 0x170dc, 0x1304e, 0x170ce, 0x17058, 0x1b82e, 0x1704c, 0x17046, 0x1302e, 0x1706e, 0x1702c, 0x17026, 0x10af0, 0x1857c, 0x10a78, 0x1853e, 0x10a3c, 0x10a1e, 0x10b7c, 0x10b3e, 0x1f0ba, 0x1e17a, 0x1c2fa, 0x185fa, 0x11ae0, 0x18d78, 0x1c6be, 0x11a70, 0x18d3c, 0x11a38, 0x18d1e, 0x11a1c, 0x11a0e, 0x10978, 0x184be, 0x11b78, 0x1093c, 0x11b3c, 0x1091e, 0x11b1e, 0x109be, 0x11bbe, 0x13ac0, 0x19d70, 0x1cebc, 0x13a60, 0x19d38, 0x1ce9e, 0x13a30, 0x19d1c, 0x13a18, 0x19d0e, 0x13a0c, 0x13a06, 0x11970, 0x18cbc, 0x13b70, 0x11938, 0x18c9e, 0x13b38, 0x1191c, 0x13b1c, 0x1190e, 0x13b0e, 0x108bc, 0x119bc, 0x1089e, 0x13bbc, 0x1199e, 0x13b9e, 0x1bd60, 0x1deb8, 0x1ef5e, 0x17a40, 0x1bd30, 0x1de9c, 0x17a20, 0x1bd18, 0x1de8e, 0x17a10, 0x1bd0c, 0x17a08, 0x1bd06, 0x17a04, 0x13960, 0x19cb8, 0x1ce5e, 0x17b60, 0x13930, 0x19c9c, 0x17b30, 0x1bd9c, 0x19c8e, 0x17b18, 0x1390c, 0x17b0c, 0x13906, 0x17b06, 0x118b8, 0x18c5e, 0x139b8, 0x1189c, 0x17bb8, 0x1399c, 0x1188e, 0x17b9c, 0x1398e, 0x17b8e, 0x1085e, 0x118de, 0x139de, 0x17bde, 0x17940, 0x1bcb0, 0x1de5c, 0x17920, 0x1bc98, 0x1de4e, 0x17910, 0x1bc8c, 0x17908, 0x1bc86, 0x17904, 0x17902, 0x138b0, 0x19c5c, 0x179b0, 0x13898, 0x19c4e, 0x17998, 0x1bcce, 0x1798c, 0x13886, 0x17986, 0x1185c, 0x138dc, 0x1184e, 0x179dc, 0x138ce, 0x179ce, 0x178a0, 0x1bc58, 0x1de2e, 0x17890, 0x1bc4c, 0x17888, 0x1bc46, 0x17884, 0x17882, 0x13858, 0x19c2e, 0x178d8, 0x1384c, 0x178cc, 0x13846, 0x178c6, 0x1182e, 0x1386e, 0x178ee, 0x17850, 0x1bc2c, 0x17848, 0x1bc26, 0x17844, 0x17842, 0x1382c, 0x1786c, 0x13826, 0x17866, 0x17828, 0x1bc16, 0x17824, 0x17822, 0x13816, 0x17836, 
				0x10578, 0x182be, 0x1053c, 0x1051e, 0x105be, 0x10d70, 0x186bc, 0x10d38, 0x1869e, 0x10d1c, 0x10d0e, 0x104bc, 0x10dbc, 0x1049e, 0x10d9e, 0x11d60, 0x18eb8, 0x1c75e, 0x11d30, 0x18e9c, 0x11d18, 0x18e8e, 0x11d0c, 0x11d06, 0x10cb8, 0x1865e, 0x11db8, 0x10c9c, 0x11d9c, 0x10c8e, 0x11d8e, 0x1045e, 0x10cde, 0x11dde, 0x13d40, 0x19eb0, 0x1cf5c, 0x13d20, 0x19e98, 0x1cf4e, 0x13d10, 0x19e8c, 0x13d08, 0x19e86, 0x13d04, 0x13d02, 0x11cb0, 0x18e5c, 0x13db0, 0x11c98, 0x18e4e, 0x13d98, 0x19ece, 0x13d8c, 0x11c86, 0x13d86, 0x10c5c, 0x11cdc, 0x10c4e, 0x13ddc, 0x11cce, 0x13dce, 0x1bea0, 0x1df58, 0x1efae, 0x1be90, 0x1df4c, 0x1be88, 0x1df46, 0x1be84, 0x1be82, 0x13ca0, 0x19e58, 0x1cf2e, 0x17da0, 0x13c90, 0x19e4c, 0x17d90, 0x1becc, 0x19e46, 0x17d88, 0x13c84, 0x17d84, 0x13c82, 0x17d82, 0x11c58, 0x18e2e, 0x13cd8, 0x11c4c, 0x17dd8, 0x13ccc, 0x11c46, 0x17dcc, 0x13cc6, 0x17dc6, 0x10c2e, 0x11c6e, 0x13cee, 0x17dee, 0x1be50, 0x1df2c, 0x1be48, 0x1df26, 0x1be44, 0x1be42, 0x13c50, 0x19e2c, 0x17cd0, 0x13c48, 0x19e26, 0x17cc8, 0x1be66, 0x17cc4, 0x13c42, 0x17cc2, 0x11c2c, 0x13c6c, 0x11c26, 0x17cec, 0x13c66, 0x17ce6, 0x1be28, 0x1df16, 0x1be24, 0x1be22, 0x13c28, 0x19e16, 0x17c68, 0x13c24, 0x17c64, 0x13c22, 0x17c62, 0x11c16, 0x13c36, 0x17c76, 0x1be14, 0x1be12, 0x13c14, 0x17c34, 0x13c12, 0x17c32, 0x102bc, 0x1029e, 0x106b8, 0x1835e, 0x1069c, 0x1068e, 0x1025e, 0x106de, 0x10eb0, 0x1875c, 0x10e98, 0x1874e, 0x10e8c, 0x10e86, 0x1065c, 0x10edc, 0x1064e, 0x10ece, 0x11ea0, 0x18f58, 0x1c7ae, 0x11e90, 0x18f4c, 0x11e88, 0x18f46, 0x11e84, 0x11e82, 0x10e58, 0x1872e, 0x11ed8, 0x18f6e, 0x11ecc, 0x10e46, 0x11ec6, 0x1062e, 0x10e6e, 0x11eee, 0x19f50, 0x1cfac, 0x19f48, 0x1cfa6, 0x19f44, 0x19f42, 0x11e50, 0x18f2c, 0x13ed0, 0x19f6c, 0x18f26, 0x13ec8, 0x11e44, 0x13ec4, 0x11e42, 0x13ec2, 0x10e2c, 0x11e6c, 0x10e26, 0x13eec, 0x11e66, 0x13ee6, 0x1dfa8, 0x1efd6, 0x1dfa4, 0x1dfa2, 0x19f28, 0x1cf96, 0x1bf68, 0x19f24, 0x1bf64, 0x19f22, 0x1bf62, 0x11e28, 0x18f16, 0x13e68, 0x11e24, 0x17ee8, 0x13e64, 0x11e22, 0x17ee4, 0x13e62, 0x17ee2, 0x10e16, 0x11e36, 0x13e76, 0x17ef6, 0x1df94, 
				0x1df92, 0x19f14, 0x1bf34, 0x19f12, 0x1bf32, 0x11e14, 0x13e34, 0x11e12, 0x17e74, 0x13e32, 0x17e72, 0x1df8a, 0x19f0a, 0x1bf1a, 0x11e0a, 0x13e1a, 0x17e3a, 0x1035c, 0x1034e, 0x10758, 0x183ae, 0x1074c, 0x10746, 0x1032e, 0x1076e, 0x10f50, 0x187ac, 0x10f48, 0x187a6, 0x10f44, 0x10f42, 0x1072c, 0x10f6c, 0x10726, 0x10f66, 0x18fa8, 0x1c7d6, 0x18fa4, 0x18fa2, 0x10f28, 0x18796, 0x11f68, 0x18fb6, 0x11f64, 0x10f22, 0x11f62, 0x10716, 0x10f36, 0x11f76, 0x1cfd4, 0x1cfd2, 0x18f94, 0x19fb4, 0x18f92, 0x19fb2, 0x10f14, 0x11f34, 0x10f12, 0x13f74, 0x11f32, 0x13f72, 0x1cfca, 0x18f8a, 0x19f9a, 0x10f0a, 0x11f1a, 0x13f3a, 0x103ac, 0x103a6, 0x107a8, 0x183d6, 0x107a4, 0x107a2, 0x10396, 0x107b6, 0x187d4, 0x187d2, 0x10794, 0x10fb4, 0x10792, 0x10fb2, 0x1c7ea};
		}

//---------------------------------------------------------------------------------------

		public TALBarcodePDF417() : this(300, 300) {
		}

//---------------------------------------------------------------------------------------

		[DllImport("user32.dll")]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);
		
		[DllImport("user32.dll")]
		static extern bool CloseClipboard();

		[DllImport("user32.dll")] 
		static extern int IsClipboardFormatAvailable(int uFormat);

		[DllImport("user32.dll")]
		static extern IntPtr GetClipboardData(uint uFormat);

//---------------------------------------------------------------------------------------

		public static int GetValue(int codeword, int cluster) {
			int	n = Array.BinarySearch<int>(CLUSTERS[cluster], codeword);
			return n;				// If not found, returns -1
		}

//---------------------------------------------------------------------------------------

		public Bitmap CreateBitMap(string s, ref TALPDFBarCodeParms bp, out MetaFilePict mfp) {
			// TODO: We don't seem to be using the newSize parameter
			// OK, for now we're going to brute-force it. Later we may optimize a bit.
			// TALPDFBarCodeParms bp = new TALPDFBarCodeParms();	// bp = Barcode Parms
			mfp = new MetaFilePict();
			bp.MessageBuffer = s;
			bp.MessageLength = s.Length;
			Bitmap	bmp = null;
			SetBarcodeParms(ref bp);

#if OUTPUT_TO_CLIPBOARD
			bp.OutputOption = 0;			// Clipboard
			Clipboard.Clear();				// Debug
#endif
#if OUTPUT_TO_DISK
			bp.OutputOption = 1;			// Disk file
			string	TempFilename = Path.GetTempFileName();
			string filename = TempFilename + ".wmf";
			// Note: GetTempFileName doesn't just return a filename, it actually
			//       creates a zero-length file as well. So we have to delete it
			//       whether the call to the barcode DLL succeeds or not.
			bp.OutputFilename = filename;
#endif
#if OUTPUT_TO_MEMORYMETAFILE
			bp.OutputOption = 2;			// Memory Metafile
			PrintDocument	pdoc = new PrintDocument();
			PrinterSettings ps = new PrinterSettings();
			pdoc.PrinterSettings = ps;		// Comes set to default printer, etc
			PrinterResolution pr = new PrinterResolution();
			pr.Kind = PrinterResolutionKind.Custom;
			pr.X = 300;
			pr.Y = 300;
			pdoc.DefaultPageSettings.PrinterResolution = pr;
			pdoc.PrinterSettings.PrintFileName = @"C:\lrs.dat";
			pdoc.PrinterSettings.PrintToFile = true;
			pdoc.BeginPrint += new PrintEventHandler(pdoc_BeginPrint);
			pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
			pdoc.Print();
#if false
			foreach (string PrinterName in System.Drawing.Printing.PrinterSettings.InstalledPrinters) {
				pdoc.PrinterSettings.PrinterName = PrinterName;
				Console.WriteLine(PrinterName);
				if (pdoc.PrinterSettings.IsDefaultPrinter) {
					break;
				}
			}
#endif
			// pdoc.PrinterSettings.PrinterResolutions = PrinterResolutionKind.High;
#endif
#if OUTPUT_TO_DC
			bp.OutputOption = 3;			// Device Context
			bmp = new Bitmap(1540, 660);		// TODO:
			Graphics g = Graphics.FromImage(bmp);
			bp.OutputhDC = g.GetHdc();
#endif
			int rc = TALPDFCode(ref bp, ref mfp);
			try {
				if (rc != 0) {
					MessageBox.Show(string.Format("PDF not created. Return code={0}", rc), "TestTALBarcodeDLL");
					return null;
					// Arguably we should have an <out> parameter to return the actual
					// error code. TODO:
				}
#if OUTPUT_TO_DC
			bmp = new Bitmap(mfp.xExt, mfp.yExt);		// TODO:
			bmp.SetResolution(bp.HorizontalDPI, bp.VerticalDPI);
			g = Graphics.FromImage(bmp);
			bp.OutputhDC = g.GetHdc();
			rc = TALPDFCode(ref bp, ref mfp);
#endif
#if OUTPUT_TO_DISK
				LRSMetafile16	mf = new LRSMetafile16();
				mf.Open(bp.OutputFilename);
				// DecodeWMFtoPDF(mf);
				mf.Close();

				mf2 = new Metafile(bp.OutputFilename);
				Graphics.EnumerateMetafileProc metafileDelegate = new Graphics.EnumerateMetafileProc(MetafileCallback);
				MetafileHeader	hdr = mf2.GetMetafileHeader();
				// ImageFormat	imf = mf2.RawFormat;

				bmp = new Bitmap(bp.OutputFilename);

				double WidthInInches = mfp.xExt / 2540.0;
				double HeightInInches = mfp.yExt / 2540.0;
				int WidthInPixels = (int)(WidthInInches * bp.HorizontalDPI);
				int HeightInPixels = (int)(HeightInInches * bp.VerticalDPI);

				// offScreenBmp = new Bitmap(mfp.xExt, mfp.yExt);		// TODO:
				offScreenBmp = new Bitmap(WidthInPixels, HeightInPixels);		// TODO:
				offScreenBmp.SetResolution(bp.HorizontalDPI, bp.VerticalDPI);
				offScreenDC = Graphics.FromImage(offScreenBmp);
				// Icon iApp = SystemIcons.Application;
				// Brush brush = SystemBrushes.ButtonFace;
				// Color buttonface = SystemColors.ButtonFace;
				// Font font = SystemFonts.CaptionFont;
				// int menuheight = SystemInformation.MenuHeight;
				// SystemParameter dropshadow = SystemParameter.DropShadow;
				// Pen penbuttonface = SystemPens.ButtonFace;
				// offScreenDC.PageUnit = GraphicsUnit.World;
				offScreenDC.EnumerateMetafile(mf2, new Point(0, 0), metafileDelegate);
				// offScreenDC.ReleaseHdc();
				bmp = offScreenBmp;

#endif
#if OUTPUT_TO_CLIPBOARD
#if false
				IntPtr henmetafile;
				System.Drawing.Imaging.Metafile metaFile;

				if (OpenClipboard(IntPtr.Zero)) {
					const uint CF_ENHMETAFILE = 14;
					if (IsClipboardFormatAvailable(CF_ENHMETAFILE) != 0) {
						henmetafile = GetClipboardData(CF_ENHMETAFILE);
						metaFile = new Metafile(henmetafile, true);
						CloseClipboard();
					}
				}
#endif

				DataObject	data;					// From clipboard
				data = (DataObject)Clipboard.GetDataObject();
				string[] fmts = data.GetFormats();	
				Metafile	mf;
				object o = data.GetData(DataFormats.Bitmap, true);	
				object o2 = data.GetData("MetaFilePict", false);			
				o2 = data.GetData(fmts[1], false);
				byte [] buf = new byte[10000];
				object o4 = data.GetData(buf.GetType());
				ImageConverter	ic = new ImageConverter();
				bool b = ic.CanConvertFrom(data.GetType());
				object o3 = data.GetDataPresent(DataFormats.EnhancedMetafile, true);
				mf = (Metafile)data.GetData(DataFormats.EnhancedMetafile);
				Bitmap	bm = (Bitmap)data.GetData(typeof(Bitmap));
				return (Bitmap)data.GetData(typeof(Bitmap));
#endif
#if OUTPUT_TO_MEMORYMETAFILE
#if false
				Graphics.EnumerateMetafileProc metafileDelegate; Graphics.EnumerateMetafileProc
				metafileDelegate = new Graphics.EnumerateMetafileProc(MetafileCallback);
				Bitmap b = new Bitmap(); b.GetHbitmap
#endif

				// byte [] buf = GetMemory(mfp.hMf, 100);	// Sigh. hMf isn't an addr we can see
				WmfPlaceableFileHeader	ph = new WmfPlaceableFileHeader();
				Metafile mf = new Metafile(mfp.hMf, ph);
				// IntPtr	ip = new IntPtr(77767432);
				// short h1 = Marshal..ReadInt16(ip);
				// Metafile mf2 = new Metafile(mfp.hMf, EmfType.EmfPlusOnly);
				GraphicsUnit	gu = GraphicsUnit.Millimeter;
				RectangleF rf = mf.GetBounds(ref gu);
				// mf.GetMetafileHeader(mfp.hMf);

				// Convert mfp results to inches, then to pixels (based on DPI).
				// Each value is in units of .01mm, so divide by 1000 to get them
				// into centimeters, then by 2.54 to get them into inches. 
				// Then multiply by DPI
				double	w = mfp.xExt / (1000 * 2.54);	
				double	h = mfp.yExt / (1000 * 2.54);
				// w *= mf.HorizontalResolution;
				// h *= mf.VerticalResolution;
				w *= bp.HorizontalDPI;
				h *= bp.VerticalDPI;
				bmp = new Bitmap(mf, (int)w, (int)h);
				// TODO: Close metafile handle
				return bmp;					
#endif
#if OUTPUT_TO_DC
				Clipboard.SetData("Bitmap", bmp);
				g.ReleaseHdc(bp.OutputhDC);
#endif
				// string	msg = string.Format("Barcode .bmp dimensions = {0}", bmp.Size);	// TODO:
				// MessageBox.Show(msg, "BadgeMax");			// TODO:
				return bmp;
			} finally {
#if OUTPUT_TO_DISK
				File.Delete(TempFilename);
				File.Delete(filename);
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private bool MetafileCallback(
						   EmfPlusRecordType recordType,
						   int flags,
						   int dataSize,
						   IntPtr data,
						   PlayRecordCallback callbackData) {
			byte []	buf = new byte[dataSize];
			Marshal.Copy(data, buf, 0, dataSize);
			int [] wbuf = new int[dataSize / 4];
			for (int i = 0; i < wbuf.Length; i++) {
				wbuf[i] = BitConverter.ToInt32(buf, i * 4);
			}
#if false
			Console.Write("MetafileCallback - RecType={0}, Flags={1}, DataSize={2}, dataPtr={3}\n\t",
				recordType, flags, dataSize, data);
			int	nInts = dataSize / 4;
			for (int i = 0; i < nInts; i++) {
				// Console.Write("{0:x} ", buf[i]);
				Console.Write("{0} ", BitConverter.ToInt32(buf, i * 4));
			}
			Console.WriteLine();
#endif

#if false
			mf2.PlayRecord(recordType, flags, dataSize, buf);
#endif

#if true
			switch (recordType) {
#if true	// Don't care (for now) about the other types
#if false
			case EmfPlusRecordType.EmfMax:					// case 122 - same as EmfCreateColorSpaceW
				break;
#endif
			case EmfPlusRecordType.EmfHeader:				// case 1
			// case EmfPlusRecordType.EmfMin:					// case 1
				break;
			case EmfPlusRecordType.BeginContainer:
				break;
			case EmfPlusRecordType.BeginContainerNoParams:
				break;
			case EmfPlusRecordType.Clear:
				break;
			case EmfPlusRecordType.Comment:
				break;
			case EmfPlusRecordType.DrawArc:
				break;
			case EmfPlusRecordType.DrawBeziers:
				break;
			case EmfPlusRecordType.DrawClosedCurve:
				break;
			case EmfPlusRecordType.DrawCurve:
				break;
			case EmfPlusRecordType.DrawDriverString:
				break;
			case EmfPlusRecordType.DrawEllipse:
				break;
			case EmfPlusRecordType.DrawImage:
				break;
			case EmfPlusRecordType.DrawImagePoints:
				break;
			case EmfPlusRecordType.DrawLines:
				break;
			case EmfPlusRecordType.DrawPath:
				break;
			case EmfPlusRecordType.DrawPie:
				break;
			case EmfPlusRecordType.DrawRects:
				break;
			case EmfPlusRecordType.DrawString:
				break;
			case EmfPlusRecordType.EmfAbortPath:
				break;
			case EmfPlusRecordType.EmfAlphaBlend:
				break;
			case EmfPlusRecordType.EmfAngleArc:
				break;
			case EmfPlusRecordType.EmfArcTo:
				break;
			case EmfPlusRecordType.EmfBeginPath:
				break;
			case EmfPlusRecordType.EmfBitBlt:
				break;
			case EmfPlusRecordType.EmfChord:
				break;
			case EmfPlusRecordType.EmfCloseFigure:
				break;
			case EmfPlusRecordType.EmfColorCorrectPalette:
				break;
			case EmfPlusRecordType.EmfColorMatchToTargetW:
				break;
			case EmfPlusRecordType.EmfCreateBrushIndirect:
				break;
			case EmfPlusRecordType.EmfCreateColorSpace:
				break;
			case EmfPlusRecordType.EmfCreateColorSpaceW:		// case 122
				break;
			case EmfPlusRecordType.EmfCreateDibPatternBrushPt:
				break;
			case EmfPlusRecordType.EmfCreateMonoBrush:
				break;
			case EmfPlusRecordType.EmfCreatePalette:
				break;
			case EmfPlusRecordType.EmfCreatePen:
				break;
			case EmfPlusRecordType.EmfDeleteColorSpace:
				break;
			case EmfPlusRecordType.EmfDeleteObject:
				break;
			case EmfPlusRecordType.EmfDrawEscape:
				break;
			case EmfPlusRecordType.EmfEllipse:
				break;
			case EmfPlusRecordType.EmfEndPath:
				break;
			case EmfPlusRecordType.EmfEof:
				break;
			case EmfPlusRecordType.EmfExcludeClipRect:
				break;
			case EmfPlusRecordType.EmfExtCreateFontIndirect:
				break;
			case EmfPlusRecordType.EmfExtCreatePen:
				break;
			case EmfPlusRecordType.EmfExtEscape:
				break;
			case EmfPlusRecordType.EmfExtFloodFill:
				break;
			case EmfPlusRecordType.EmfExtSelectClipRgn:
				break;
			case EmfPlusRecordType.EmfExtTextOutA:
				break;
			case EmfPlusRecordType.EmfExtTextOutW:
				break;
			case EmfPlusRecordType.EmfFillPath:
				break;
			case EmfPlusRecordType.EmfFillRgn:
				break;
			case EmfPlusRecordType.EmfFlattenPath:
				break;
			case EmfPlusRecordType.EmfForceUfiMapping:
				break;
			case EmfPlusRecordType.EmfFrameRgn:
				break;
			case EmfPlusRecordType.EmfGdiComment:
				break;
			case EmfPlusRecordType.EmfGlsBoundedRecord:
				break;
			case EmfPlusRecordType.EmfGlsRecord:
				break;
			case EmfPlusRecordType.EmfGradientFill:
				break;
			case EmfPlusRecordType.EmfIntersectClipRect:
				break;
			case EmfPlusRecordType.EmfInvertRgn:
				break;
			case EmfPlusRecordType.EmfLineTo:
				break;
			case EmfPlusRecordType.EmfMaskBlt:
				break;
			case EmfPlusRecordType.EmfModifyWorldTransform:
				break;
			case EmfPlusRecordType.EmfMoveToEx:
				break;
			case EmfPlusRecordType.EmfNamedEscpae:
				break;
			case EmfPlusRecordType.EmfOffsetClipRgn:
				break;
			case EmfPlusRecordType.EmfPaintRgn:
				break;
			case EmfPlusRecordType.EmfPie:
				break;
			case EmfPlusRecordType.EmfPixelFormat:
				break;
			case EmfPlusRecordType.EmfPlgBlt:
				break;
			case EmfPlusRecordType.EmfPlusRecordBase:
				break;
			case EmfPlusRecordType.EmfPolyBezier:
				break;
			case EmfPlusRecordType.EmfPolyBezier16:
				break;
			case EmfPlusRecordType.EmfPolyBezierTo:
				break;
			case EmfPlusRecordType.EmfPolyBezierTo16:
				break;
			case EmfPlusRecordType.EmfPolyDraw:
				break;
			case EmfPlusRecordType.EmfPolyDraw16:
				break;
			case EmfPlusRecordType.EmfPolyLineTo:
				break;
			case EmfPlusRecordType.EmfPolyPolygon:
				break;
			case EmfPlusRecordType.EmfPolyPolygon16:
				break;
			case EmfPlusRecordType.EmfPolyPolyline:
				break;
			case EmfPlusRecordType.EmfPolyPolyline16:
				break;
			case EmfPlusRecordType.EmfPolyTextOutA:
				break;
			case EmfPlusRecordType.EmfPolyTextOutW:
				break;
			case EmfPlusRecordType.EmfPolygon:
				break;
			case EmfPlusRecordType.EmfPolygon16:
				break;
			case EmfPlusRecordType.EmfPolyline:
				break;
			case EmfPlusRecordType.EmfPolyline16:
				break;
			case EmfPlusRecordType.EmfPolylineTo16:
				break;
			case EmfPlusRecordType.EmfRealizePalette:
				break;
#endif
			case EmfPlusRecordType.EmfRectangle:
				offScreenDC.DrawRectangle(Pens.Green, wbuf[0], wbuf[1], wbuf[2], wbuf[3]);
				break;
#if false	// Don't care (for now) about the other types
			case EmfPlusRecordType.EmfReserved069:
				break;
			case EmfPlusRecordType.EmfReserved117:
				break;
			case EmfPlusRecordType.EmfResizePalette:
				break;
			case EmfPlusRecordType.EmfRestoreDC:
				break;
			case EmfPlusRecordType.EmfRoundArc:
				break;
			case EmfPlusRecordType.EmfRoundRect:
				break;
			case EmfPlusRecordType.EmfSaveDC:
				break;
			case EmfPlusRecordType.EmfScaleViewportExtEx:
				break;
			case EmfPlusRecordType.EmfScaleWindowExtEx:
				break;
			case EmfPlusRecordType.EmfSelectClipPath:
				break;
			case EmfPlusRecordType.EmfSelectObject:
				break;
			case EmfPlusRecordType.EmfSelectPalette:
				break;
			case EmfPlusRecordType.EmfSetArcDirection:
				break;
			case EmfPlusRecordType.EmfSetBkColor:
				break;
			case EmfPlusRecordType.EmfSetBkMode:
				break;
			case EmfPlusRecordType.EmfSetBrushOrgEx:
				break;
			case EmfPlusRecordType.EmfSetColorAdjustment:
				break;
			case EmfPlusRecordType.EmfSetColorSpace:
				break;
			case EmfPlusRecordType.EmfSetDIBitsToDevice:
				break;
			case EmfPlusRecordType.EmfSetIcmMode:
				break;
			case EmfPlusRecordType.EmfSetIcmProfileA:
				break;
			case EmfPlusRecordType.EmfSetIcmProfileW:
				break;
			case EmfPlusRecordType.EmfSetLayout:
				break;
			case EmfPlusRecordType.EmfSetLinkedUfis:
				break;
			case EmfPlusRecordType.EmfSetMapMode:
				break;
			case EmfPlusRecordType.EmfSetMapperFlags:
				break;
			case EmfPlusRecordType.EmfSetMetaRgn:
				break;
			case EmfPlusRecordType.EmfSetMiterLimit:
				break;
			case EmfPlusRecordType.EmfSetPaletteEntries:
				break;
			case EmfPlusRecordType.EmfSetPixelV:
				break;
			case EmfPlusRecordType.EmfSetPolyFillMode:
				break;
			case EmfPlusRecordType.EmfSetROP2:
				break;
			case EmfPlusRecordType.EmfSetStretchBltMode:
				break;
			case EmfPlusRecordType.EmfSetTextAlign:
				break;
			case EmfPlusRecordType.EmfSetTextColor:
				break;
			case EmfPlusRecordType.EmfSetTextJustification:
				break;
			case EmfPlusRecordType.EmfSetViewportExtEx:
				break;
			case EmfPlusRecordType.EmfSetViewportOrgEx:
				break;
			case EmfPlusRecordType.EmfSetWindowExtEx:
				break;
			case EmfPlusRecordType.EmfSetWindowOrgEx:
				break;
			case EmfPlusRecordType.EmfSetWorldTransform:
				break;
			case EmfPlusRecordType.EmfSmallTextOut:
				break;
			case EmfPlusRecordType.EmfStartDoc:
				break;
			case EmfPlusRecordType.EmfStretchBlt:
				break;
			case EmfPlusRecordType.EmfStretchDIBits:
				break;
			case EmfPlusRecordType.EmfStrokeAndFillPath:
				break;
			case EmfPlusRecordType.EmfStrokePath:
				break;
			case EmfPlusRecordType.EmfTransparentBlt:
				break;
			case EmfPlusRecordType.EmfWidenPath:
				break;
			case EmfPlusRecordType.EndContainer:
				break;
			case EmfPlusRecordType.EndOfFile:
				break;
			case EmfPlusRecordType.FillClosedCurve:
				break;
			case EmfPlusRecordType.FillEllipse:
				break;
			case EmfPlusRecordType.FillPath:
				break;
			case EmfPlusRecordType.FillPie:
				break;
			case EmfPlusRecordType.FillPolygon:
				break;
			case EmfPlusRecordType.FillRects:
				break;
			case EmfPlusRecordType.FillRegion:
				break;
			case EmfPlusRecordType.GetDC:
				break;
			case EmfPlusRecordType.Header:
				break;
#if false
			case EmfPlusRecordType.Invalid:
				break;
			case EmfPlusRecordType.Max:
				break;
			case EmfPlusRecordType.Min:
				break;
#endif
			case EmfPlusRecordType.MultiFormatEnd:
				break;
			case EmfPlusRecordType.MultiFormatSection:
				break;
			case EmfPlusRecordType.MultiFormatStart:
				break;
			case EmfPlusRecordType.MultiplyWorldTransform:
				break;
			case EmfPlusRecordType.Object:
				break;
			case EmfPlusRecordType.OffsetClip:
				break;
			case EmfPlusRecordType.ResetClip:
				break;
			case EmfPlusRecordType.ResetWorldTransform:
				break;
			case EmfPlusRecordType.Restore:
				break;
			case EmfPlusRecordType.RotateWorldTransform:
				break;
			case EmfPlusRecordType.Save:
				break;
			case EmfPlusRecordType.ScaleWorldTransform:
				break;
			case EmfPlusRecordType.SetAntiAliasMode:
				break;
			case EmfPlusRecordType.SetClipPath:
				break;
			case EmfPlusRecordType.SetClipRect:
				break;
			case EmfPlusRecordType.SetClipRegion:
				break;
			case EmfPlusRecordType.SetCompositingMode:
				break;
			case EmfPlusRecordType.SetCompositingQuality:
				break;
			case EmfPlusRecordType.SetInterpolationMode:
				break;
			case EmfPlusRecordType.SetPageTransform:
				break;
			case EmfPlusRecordType.SetPixelOffsetMode:
				break;
			case EmfPlusRecordType.SetRenderingOrigin:
				break;
			case EmfPlusRecordType.SetTextContrast:
				break;
			case EmfPlusRecordType.SetTextRenderingHint:
				break;
			case EmfPlusRecordType.SetWorldTransform:
				break;
			case EmfPlusRecordType.Total:
				break;
			case EmfPlusRecordType.TranslateWorldTransform:
				break;
			case EmfPlusRecordType.WmfAnimatePalette:
				break;
			case EmfPlusRecordType.WmfArc:
				break;
			case EmfPlusRecordType.WmfBitBlt:
				break;
			case EmfPlusRecordType.WmfChord:
				break;
			case EmfPlusRecordType.WmfCreateBrushIndirect:
				break;
			case EmfPlusRecordType.WmfCreateFontIndirect:
				break;
			case EmfPlusRecordType.WmfCreatePalette:
				break;
			case EmfPlusRecordType.WmfCreatePatternBrush:
				break;
			case EmfPlusRecordType.WmfCreatePenIndirect:
				break;
			case EmfPlusRecordType.WmfCreateRegion:
				break;
			case EmfPlusRecordType.WmfDeleteObject:
				break;
			case EmfPlusRecordType.WmfDibBitBlt:
				break;
			case EmfPlusRecordType.WmfDibCreatePatternBrush:
				break;
			case EmfPlusRecordType.WmfDibStretchBlt:
				break;
			case EmfPlusRecordType.WmfEllipse:
				break;
			case EmfPlusRecordType.WmfEscape:
				break;
			case EmfPlusRecordType.WmfExcludeClipRect:
				break;
			case EmfPlusRecordType.WmfExtFloodFill:
				break;
			case EmfPlusRecordType.WmfExtTextOut:
				break;
			case EmfPlusRecordType.WmfFillRegion:
				break;
			case EmfPlusRecordType.WmfFloodFill:
				break;
			case EmfPlusRecordType.WmfFrameRegion:
				break;
			case EmfPlusRecordType.WmfIntersectClipRect:
				break;
			case EmfPlusRecordType.WmfInvertRegion:
				break;
			case EmfPlusRecordType.WmfLineTo:
				break;
			case EmfPlusRecordType.WmfMoveTo:
				break;
			case EmfPlusRecordType.WmfOffsetCilpRgn:
				break;
			case EmfPlusRecordType.WmfOffsetViewportOrg:
				break;
			case EmfPlusRecordType.WmfOffsetWindowOrg:
				break;
			case EmfPlusRecordType.WmfPaintRegion:
				break;
			case EmfPlusRecordType.WmfPatBlt:
				break;
			case EmfPlusRecordType.WmfPie:
				break;
			case EmfPlusRecordType.WmfPolyPolygon:
				break;
			case EmfPlusRecordType.WmfPolygon:
				break;
			case EmfPlusRecordType.WmfPolyline:
				break;
			case EmfPlusRecordType.WmfRealizePalette:
				break;
			case EmfPlusRecordType.WmfRecordBase:
				break;
			case EmfPlusRecordType.WmfRectangle:
				break;
			case EmfPlusRecordType.WmfResizePalette:
				break;
			case EmfPlusRecordType.WmfRestoreDC:
				break;
			case EmfPlusRecordType.WmfRoundRect:
				break;
			case EmfPlusRecordType.WmfSaveDC:
				break;
			case EmfPlusRecordType.WmfScaleViewportExt:
				break;
			case EmfPlusRecordType.WmfScaleWindowExt:
				break;
			case EmfPlusRecordType.WmfSelectClipRegion:
				break;
			case EmfPlusRecordType.WmfSelectObject:
				break;
			case EmfPlusRecordType.WmfSelectPalette:
				break;
			case EmfPlusRecordType.WmfSetBkColor:
				break;
			case EmfPlusRecordType.WmfSetBkMode:
				break;
			case EmfPlusRecordType.WmfSetDibToDev:
				break;
			case EmfPlusRecordType.WmfSetLayout:
				break;
			case EmfPlusRecordType.WmfSetMapMode:
				break;
			case EmfPlusRecordType.WmfSetMapperFlags:
				break;
			case EmfPlusRecordType.WmfSetPalEntries:
				break;
			case EmfPlusRecordType.WmfSetPixel:
				break;
			case EmfPlusRecordType.WmfSetPolyFillMode:
				break;
			case EmfPlusRecordType.WmfSetROP2:
				break;
			case EmfPlusRecordType.WmfSetRelAbs:
				break;
			case EmfPlusRecordType.WmfSetStretchBltMode:
				break;
			case EmfPlusRecordType.WmfSetTextAlign:
				break;
			case EmfPlusRecordType.WmfSetTextCharExtra:
				break;
			case EmfPlusRecordType.WmfSetTextColor:
				break;
			case EmfPlusRecordType.WmfSetTextJustification:
				break;
			case EmfPlusRecordType.WmfSetViewportExt:
				break;
			case EmfPlusRecordType.WmfSetViewportOrg:
				break;
			case EmfPlusRecordType.WmfSetWindowExt:
				break;
			case EmfPlusRecordType.WmfSetWindowOrg:
				break;
			case EmfPlusRecordType.WmfStretchBlt:
				break;
			case EmfPlusRecordType.WmfStretchDib:
				break;
			case EmfPlusRecordType.WmfTextOut:
				break;
#endif
			default:
				break;
			}	
#endif
			return true;
		}

//---------------------------------------------------------------------------------------

		void pdoc_PrintPage(object sender, PrintPageEventArgs e) {
			throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		void pdoc_BeginPrint(object sender, PrintEventArgs e) {
			// PrintDocument pdoc = sender as PrintDocument;
			e.Cancel = true;
		}

//---------------------------------------------------------------------------------------

		private byte [] GetMemory(IntPtr ptr, int len) {
			byte [] buf = new byte[len];
			Marshal.Copy(ptr, buf, 0, len);
			return buf;
		}

//---------------------------------------------------------------------------------------

		private void DecodeWMFtoPDF(LRSMetafile16 mf) {
#if false		// Set to false to get function-by-function formatted dump instead
			// Makes all kinds of assumptions about the GDI functions that are called,
			// and in what order. We also assume that the module width is 10 and height
			// is 3 times that.
			// Note: Parameters are in stack (i.e. reversed) order, so process them
			//		 right-to-left. 
			int		mw = 10, mh = 3 * mw;		// Module width and height
			int		TotalWidthInModules = 0, TotalRows = 0;	
			bool []	CurRow = null;				// CurRow[n] == true -> black module
			int []	CurCols = null;
			int		row = -1;
			bool	FirstRectSwitch = false;
			foreach (LRSMetafile16.Metafile16Entry entry in mf.Entries()) {
				if (entry.RecordType == WMF_Defines.META_SETWINDOWEXT) {
					TotalWidthInModules = entry.Parms[1] / mw;
					TotalRows = entry.Parms[0] / mh;
					CurRow = new bool[TotalWidthInModules];	// false by default
					// Find number of codewords wide. Note that the stop pattern
					// has 18 modules instead of 17. So if we have a total width
					// of (say) 86 modules, then there are 86-35=51 modules left,
					// which when divided by 17 gives 3 codewords (of which two
					// are the Row Indicators).
					int	nCols = (TotalWidthInModules - 17 - 18) / 17;
					CurCols = new int[nCols];
					continue;
				}
				if (entry.RecordType == WMF_Defines.META_RECTANGLE) {
					if (! FirstRectSwitch) {
						FirstRectSwitch = true;
						continue;		// Ignore first one
					}
					int	x = entry.Parms[3];		// Left-most x coord
					if (x == 0) {				// Check for start of new line
						if (row >= 0) {			// Make sure we *have* a prev row
							RowToNumerics(CurRow, row, CurCols);
							DisplayPreviousRow(CurRow, row);
						}
						++row;
						Console.WriteLine("row[{4}] - Rect({0}, {1}, {2}, {3})", entry.Parms[3], entry.Parms[2], entry.Parms[1], entry.Parms[0], row);
					}
					int	w = Math.Abs(entry.Parms[3] - entry.Parms[1]) / mw;
					int	start = x / mw;
					for (int i = start; i < start + w; i++) {
						CurRow[i] = true;
					}
				}
			}
			RowToNumerics(CurRow, row, CurCols);
			DisplayPreviousRow(CurRow, row);
#else
			foreach (LRSMetafile16.Metafile16Entry entry in mf.Entries()) {
				Console.Write("{0} - {1:X}", entry.RecordSize, GetWMFName(entry.RecordType));
				for (int i = entry.Parms.Length - 1; i >= 0; i--) {
					Console.Write("{0}", entry.Parms[i]);
					if (i > 0) {
						Console.Write(", ");
					}
				}
				if (entry.RecordType == WMF_Defines.META_RECTANGLE) {
					Console.Write(" [W={0}, H={1}]",
						Math.Abs(entry.Parms[3] - entry.Parms[1]),
						Math.Abs(entry.Parms[2] - entry.Parms[0]));
				}
				Console.WriteLine(");");
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void RowToNumerics(bool[] CurRow, int row, int[] CurCols) {
			for (int i = 0; i < CurCols.Length; i++) {
				int	start = 17;				// Skip over Start pattern
				start += i * 17;			// Skip over previous codeword
				int	mult = 1;
				int	sum = 0;
				for (int j = start + 17 - 1; j >= start; j--) {
					if (CurRow[j] == true) {
						sum += mult;
					}
					mult *= 2;
				}
				CurCols[i] = sum;
				Console.Write("0x{0:X}   ", sum);
			}
			Console.Write(" -> ");
			for (int n = 0; n < CurCols.Length; ++n) {
				int [] clust = CLUSTERS[row % 3];
				bool	bFound = false;
				int		val = CurCols[n];
				if (n > 0)
					Console.Write(", ");
				for (int j = 0; j < clust.Length; j++) {
					if (val == clust[j]) {
						Console.Write("{0}", j);
						bFound = true;
						break;
					}
				}
				if (! bFound)
					Console.Write("N/A");
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private void DisplayPreviousRow(bool[] CurRow, int row) {
			// TODO: Put in better code later to consolidate things. But for now, just
			//		 display the bars and blanks (as dashes)
			for (int i = 0; i < CurRow.Length; i++) {
				if ((i > 0) && ((i % 17) == 0))
					Console.Write("   ");
				bool	b = CurRow[i];
				Console.Write(b ? "|" : "-");
				// Might as well clear the entry for next time
				CurRow[i] = false;
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private string GetWMFName(short type) {
			// Awfully inefficient, getting the reflection data every time. Optimize later
			Type	t = typeof(WMF_Defines);
			FieldInfo [] flds = t.GetFields();
			foreach (FieldInfo fld in flds) {
				if (type == (int)fld.GetRawConstantValue()) {
					return string.Format("0x{0:X} - {1}(", type, fld.Name.Substring(5));
				}
			}
			return string.Format("0x{0:X} - (", type);
		}

//---------------------------------------------------------------------------------------

		private void SetBarcodeParms(ref TALPDFBarCodeParms bp) {
			bp.PDFModuleWidth = 0;
			bp.PDFModuleHeight = 0;
			bp.PDFAspect = 0;
			bp.PDFSecurityLevel = 1;
			bp.PDFMaxRows = 0;
			bp.PDFMaxCols = 0;
			bp.Orientation = 0;
			bp.Preferences = 0;
			bp.HorizontalDPI = hRes;
			bp.VerticalDPI = vRes;
			// bp.PDFCompactionMode = 3;
#if true
			bp.PDFModuleWidth	= 10;
			// bp.PDFModuleHeight  = bp.PDFModuleWidth;	// s/b 3 * PDFModuleWidth
			// bp.HorizontalDPI	= 120;
			// bp.VerticalDPI		= 120;
			// bp.Preferences		= 32768;		// AdjustToPrinterDPI
#endif
			// Set some defaults, in case the user didn't
			if (bp.PDFModuleWidth == 0) {
				bp.PDFModuleWidth = 33;				// The default is .33mm (13 mils)
			}
			if (bp.PDFModuleHeight == 0) {
				bp.PDFModuleHeight = 3 * bp.PDFModuleWidth;	// Suggested ratio
			}
		}

//---------------------------------------------------------------------------------------

		private void DisplayBarcodeParms(ref TALPDFBarCodeParms bp, string msg, int? rc) {
#if false
			bool bFound = (null != Environment.GetEnvironmentVariable("SCOTT_PDFPARMS"));
			// Note: We don't currently care about the value of the variable, just 
			//		 whether it exists or not.
			if (!bFound) {
				return;
			}
#endif
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendFormat("Barcode parms - {0}", msg);
			if (rc.HasValue) {
				sb.AppendFormat(", return code = {0}", rc.Value);
			}
			sb.Append("\n");
			sb.AppendFormat("\nModule Height = {0}", bp.PDFModuleHeight);
			sb.AppendFormat("\nModule Width = {0}", bp.PDFModuleWidth);
			sb.AppendFormat("\nCompaction Mode = {0}", bp.PDFCompactionMode);
			sb.AppendFormat("\nSecurity Level = {0}", bp.PDFSecurityLevel);
			sb.AppendFormat("\nAspect = {0}", bp.PDFAspect);
			sb.AppendFormat("\nMax Rows = {0}", bp.PDFMaxRows);
			sb.AppendFormat("\nMax Cols = {0}", bp.PDFMaxCols);
			sb.AppendFormat("\nOrientation = {0}", bp.Orientation);
			sb.AppendFormat("\nPreferences = 0x{0:X}", bp.Preferences);
			sb.AppendFormat("\nHorizontal BPI = {0}", bp.HorizontalDPI);
			sb.AppendFormat("\nVertical BPI = {0}", bp.VerticalDPI);

			MessageBox.Show(sb.ToString(), "BadgeMax", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// Barcode stuff (TAL, PDF, etc) follows. TODO: Generalize this later.

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TALPDFBarCodeParms {					// TODO: Put in rest of comments
		public int MessageLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2712)]
		public string MessageBuffer;
		// char 		messageBuffer[2712];		// That's what the doc says, 2712
		public int CommentLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 100)]
		public string CommentBuffer;
		// char	commentBuffer[100];
		public int PDFModuleWidth;
		public int BarWidthReduction;
		public int PDFModuleHeight;
		public float PDFAspect;
		public int PDFSecurityLevel;
		public int PDFCompactionMode;
		public int PDFPctOverhead;
		public int PDFMaxRows;
		public int PDFMaxCols;
		public uint FgColor;			// Foreground color. Maybe COLOR later. TODO:
		public uint BgColor;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
		public string FontName;
		// char fontName[32]
		public int FontSize;
		public uint TextColor;
		public int Orientation;
		public int Preferences;
		public int HorizontalDPI;
		public int VerticalDPI;
		public int OutputOption;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
		public string OutputFilename;
		// char outputFilename[260]
		public IntPtr OutputhDC;				// hDC for output device, when going to hDC
		public float XPosInInches;
		public float YPosInInches;
		int Reserved;
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MetaFilePict {
		public int mm;						// Metafile map mode
		public int xExt;					// Width of the metafile
		public int yExt;					// Height of the metafile
		public IntPtr hMf;					// Handle to the actual metafile in memory
	}

	
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// Very specialized class for analyzing PDF417 barcodes from the TAL DLL.

	public class LRSMetafile16 {

		BinaryReader			rdr = null;
		public Metafile16Header	hdr;
		public long				pos;
		
//---------------------------------------------------------------------------------------

		public IEnumerable<Metafile16Entry> Entries() {
			Metafile16Entry	entry;
			while (true) {
				entry = new Metafile16Entry(rdr);
				if (entry.RecordType == 0)
					yield break;
				yield return entry;
			}
		}
		
//---------------------------------------------------------------------------------------

		public void Open(string filename) {
			rdr = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read));
			hdr.Read(rdr);
			pos = rdr.BaseStream.Position;
		}
		
//---------------------------------------------------------------------------------------

		public void Close() {
			rdr.Close();
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Metafile16Header {
			public short	mtType;			// Metafile type, memory or disk
			public short	mtHeaderLen;	// Header length in words
			public short	mtVersion;
			public int		mtSize;			// File length in words
			public short	mtNoObj;		// Number of objects
			public int		mtMaxRec;		// Maximum record length
			public short	mtResv;			// Reserved

			public Metafile16Entry []	Entries;
		
//---------------------------------------------------------------------------------------

			public void Read(BinaryReader rdr) {
				mtType		= rdr.ReadInt16();
				mtHeaderLen = rdr.ReadInt16();
				mtVersion	= rdr.ReadInt16();
				mtSize		= rdr.ReadInt32();
				mtNoObj		= rdr.ReadInt16();
				mtMaxRec	= rdr.ReadInt32();
				mtResv		= rdr.ReadInt16();
			}
		}
		
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Metafile16Entry {
			public int		RecordSize;				// In words
			public short	RecordType;
			public short []	Parms;
		
//---------------------------------------------------------------------------------------

			public Metafile16Entry(BinaryReader rdr) {
				RecordSize = rdr.ReadInt32();
				RecordType = rdr.ReadInt16();
				Parms = new short[RecordSize - 3];
				for (int i = 0; i < RecordSize - 3; i++) {
					Parms[i] = rdr.ReadInt16();
				}
			}
		}
	}
		
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	static class WMF_Defines {
		/* Metafile Functions */
		public const int  META_SETBKCOLOR              = 0x0201;
		public const int  META_SETBKMODE               = 0x0102;
		public const int  META_SETMAPMODE              = 0x0103;
		public const int  META_SETROP2                 = 0x0104;
		public const int  META_SETRELABS               = 0x0105;
		public const int  META_SETPOLYFILLMODE         = 0x0106;
		public const int  META_SETSTRETCHBLTMODE       = 0x0107;
		public const int  META_SETTEXTCHAREXTRA        = 0x0108;
		public const int  META_SETTEXTCOLOR            = 0x0209;
		public const int  META_SETTEXTJUSTIFICATION    = 0x020A;
		public const int  META_SETWINDOWORG            = 0x020B;
		public const int  META_SETWINDOWEXT            = 0x020C;
		public const int  META_SETVIEWPORTORG          = 0x020D;
		public const int  META_SETVIEWPORTEXT          = 0x020E;
		public const int  META_OFFSETWINDOWORG         = 0x020F;
		public const int  META_SCALEWINDOWEXT          = 0x0410;
		public const int  META_OFFSETVIEWPORTORG       = 0x0211;
		public const int  META_SCALEVIEWPORTEXT        = 0x0412;
		public const int  META_LINETO                  = 0x0213;
		public const int  META_MOVETO                  = 0x0214;
		public const int  META_EXCLUDECLIPRECT         = 0x0415;
		public const int  META_INTERSECTCLIPRECT       = 0x0416;
		public const int  META_ARC                     = 0x0817;
		public const int  META_ELLIPSE                 = 0x0418;
		public const int  META_FLOODFILL               = 0x0419;
		public const int  META_PIE                     = 0x081A;
		public const int  META_RECTANGLE               = 0x041B;
		public const int  META_ROUNDRECT               = 0x061C;
		public const int  META_PATBLT                  = 0x061D;
		public const int  META_SAVEDC                  = 0x001E;
		public const int  META_SETPIXEL                = 0x041F;
		public const int  META_OFFSETCLIPRGN           = 0x0220;
		public const int  META_TEXTOUT                 = 0x0521;
		public const int  META_BITBLT                  = 0x0922;
		public const int  META_STRETCHBLT              = 0x0B23;
		public const int  META_POLYGON                 = 0x0324;
		public const int  META_POLYLINE                = 0x0325;
		public const int  META_ESCAPE                  = 0x0626;
		public const int  META_RESTOREDC               = 0x0127;
		public const int  META_FILLREGION              = 0x0228;
		public const int  META_FRAMEREGION             = 0x0429;
		public const int  META_INVERTREGION            = 0x012A;
		public const int  META_PAINTREGION             = 0x012B;
		public const int  META_SELECTCLIPREGION        = 0x012C;
		public const int  META_SELECTOBJECT            = 0x012D;
		public const int  META_SETTEXTALIGN            = 0x012E;
		public const int  META_CHORD                   = 0x0830;
		public const int  META_SETMAPPERFLAGS          = 0x0231;
		public const int  META_EXTTEXTOUT              = 0x0a32;
		public const int  META_SETDIBTODEV             = 0x0d33;
		public const int  META_SELECTPALETTE           = 0x0234;
		public const int  META_REALIZEPALETTE          = 0x0035;
		public const int  META_ANIMATEPALETTE          = 0x0436;
		public const int  META_SETPALENTRIES           = 0x0037;
		public const int  META_POLYPOLYGON             = 0x0538;
		public const int  META_RESIZEPALETTE           = 0x0139;
		public const int  META_DIBBITBLT               = 0x0940;
		public const int  META_DIBSTRETCHBLT           = 0x0b41;
		public const int  META_DIBCREATEPATTERNBRUSH   = 0x0142;
		public const int  META_STRETCHDIB              = 0x0f43;
		public const int  META_EXTFLOODFILL            = 0x0548;
		// #if(WINVER >= = 0x0500)
		public const int  META_SETLAYOUT               = 0x0149;
		// #endif /* WINVER >= = 0x0500 */
		public const int  META_DELETEOBJECT            = 0x01f0;
		public const int  META_CREATEPALETTE           = 0x00f7;
		public const int  META_CREATEPATTERNBRUSH      = 0x01F9;
		public const int  META_CREATEPENINDIRECT       = 0x02FA;
		public const int  META_CREATEFONTINDIRECT      = 0x02FB;
		public const int  META_CREATEBRUSHINDIRECT     = 0x02FC;
		public const int  META_CREATEREGION            = 0x06FF;
	}
}