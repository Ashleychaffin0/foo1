// Copyright (c) 2005, Bartizan Connects LLP

// TODO: I'm going to start by putting the TAL barcode stuff in here, and hardcoding
//		 PDF417. Later we'll want to generalize this.
// TODO: Most (all?) PrintObj's needs an Align (Left/Center/Right) property. In Text.
//		 Needed in the others.

// #define	ENHMETAFILE_TESTING

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

// TODO: For barcode support, for LayoutKind
using System.Runtime.InteropServices;

namespace Bartizan.Print {

	/// <summary>
	/// A PrintPage certainly *could* be a whole page. But admittedly it did start out
	/// as one label among many on a page. Hopefully it can be used in both ways.
	/// 
	/// One other reason for this class is that Windows printing is essentially
	/// bass-ackwards. You don't have your reporting function say "Give me a page, let
	/// me write/draw on it, now print it and give me another page, until I'm done."
	/// 
	/// Instead, you say "start printing", and Windows says (to a callback routine)
	/// "Here's a nice blank sheet of paper, do whatever you want with it. When you're
	/// done, let me know, and also if you need another sheet of paper." So your print
	/// job is driven inside a callback (event). 
	/// 
	/// For that reason, we decide up front what data is to go on each page, and queue
	/// the items to this object. This makes certain aspects of the printing process
	/// easier to conceptualize.
	/// 
	/// Its main drawback is that the data must be gathered up front, before any actual
	/// printing is done. So there's a storage overhead. So I wouldn't use this for a
	/// million-page report. But for the right types of applications, I think it'll
	/// work out well in practice.
	/// <p>
	/// I suppose that there are other properties of pages that might be relevant (who
	/// knows, maybe glossy paper), but most of these properties are part of the 
	/// LabelPage object. 
	/// </p>
	/// <p>
	/// TODO: Maybe clipping support, so that we can't draw outside the box.
	/// </p>
	/// <p>
	/// TODO: Maybe we should probably move some (most?) of the printing logic into
	/// this class. But since this could represent a sub-page, this might be a bit 
	/// tricky, especially when it comes to signifying the end of a physical page.
	/// We clearly have to think a bit more on this.
	/// </p>
	/// </summary>
	public class PrintPage {
		// This is so simple, leave it as public. But we'll make it readonly,
		// just in case...
		public readonly Rectangle	Rect;

		ArrayList	_data;

//---------------------------------------------------------------------------------------

		public ArrayList Data {
			get { return _data; }
		}

//---------------------------------------------------------------------------------------

		public PrintPage(Rectangle Rect) {
			this.Rect = Rect;
			_data = new ArrayList();
		}

//---------------------------------------------------------------------------------------

		public PrintPage(int top, int left, Size size) {
			Rectangle Rect = new Rectangle(new Point(left, top), size);
			// TODO: What I *want* to code is:
			//	PrintPage(Rect);
			// TODO: But that (and also this(Rect)) doesn't want to work. So for now...
			this.Rect = Rect;
			_data = new ArrayList();
		}

//---------------------------------------------------------------------------------------

		public void Add(PrintObj obj) {
			_data.Add(obj);
		}

//---------------------------------------------------------------------------------------

		public void Clear() {
			_data.Clear();
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// A PrintLabelPage represents what we need to know about a page of labels. Clearly
	/// we need to know how many rows and columns of labels there are per page. We need
	/// to know top and left margin info, and horizontal and vertical gutter sizes, so 
	/// we can calculate the size and location of each label.
	/// <p>
	/// I suppose there could be other properties of a label page that we might need to 
	/// worry about (someday). Glossy paper. Perforations. Whether there is a magstripe
	/// field, and so on. But we'll worry about that later.
	/// </p>
	/// <p>
	/// For now we'll pass the ctor the information we need. An obvious enhancement
	/// would be to pass it a string naming the label paper, e.g. "Avery ABC". This 
	/// information could come from a DLL (using Reflection), a database, an XML file,
	/// etc. (Clearly, in these last two cases we'd have to pass more than just the
	/// name to the ctor.)
	/// </p>
	/// </summary>
	public class PrintLabelPage {
		// These are so simple, I'm making them all public. But readonly.
		public readonly int		rows, cols;
		public readonly int		topMargin;
		public readonly int		leftMargin;
		public readonly int		horizontalGutter, verticalGutter;
		public readonly Size	size;

		// It's wasteful to have a page of, say, 20 labels (5 x 4), and print
		// a single label per page. So we'll support a type of buffering, whereby
		// a request to print a label is queued until either we fill up the page,
		// or else get a FlushPage request.
		int		CurrentRow, CurrentCol;		// Print next label at these coords

		// TODO: In the same vein, we could let the user put a partly-used sheet
		//		 into the printer, and tell us where the x-y coordinates (e.g. 
		//		 "next blank label is at row 3 column 2") where to start printing.

		// TODO: Similarly, we could ask this class where we left off, so the 
		//		 information can be serialized and used when we next start printing.
		//		 That would require CurrentRow/Col to be public, or at least have
		//		 public properties. But not now.

		public PrintLabelPage(int rows, int cols, int topMargin, int leftMargin, 
							  int horizontalGutter, int verticalGutter, Size size) {
			this.rows				= rows;
			this.cols				= cols;
			this.topMargin			= topMargin;
			this.leftMargin			= leftMargin;
			this.horizontalGutter	= horizontalGutter;
			this.verticalGutter		= verticalGutter;
			this.size				= size;

			this.CurrentRow = 0;
			this.CurrentCol = 0;
		}

//---------------------------------------------------------------------------------------

		public PrintPage GetNextPrintLabel(out bool bLast) {
			int		top, left;
			top  = topMargin  + CurrentRow * (size.Height + verticalGutter);
			left = leftMargin + CurrentCol * (size.Width  + horizontalGutter);
			
			// Increment to next label. If we go off the page, set bLast to "true".
			bLast = false;				// Assume the normal case
			if (++CurrentCol >= cols) {
				CurrentCol = 0;
				if (++CurrentRow >= rows) {
					CurrentRow = 0;
					bLast = true;
				}
			}
			return new PrintPage(top, left, size);
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	abstract public class PrintObj {
		protected Point		pt;
		protected Size		size;

		// Note: <p> is the offset on the page where we start. Originally this was used
		// to support Labels, but it's useful in other contexts as well.
		public virtual void Print(Graphics g, Point p) {	// abstract ==> virtual
			Print(g, p, Pens.Red);
		}

		// TODO: This next one is just for debug mostly. But we could replace
		//		 the above method with a call to this
		public virtual void Print(Graphics g, Point p, Pen pen) {	// abstract ==> virtual
#if true		// To turn boxing of PrintObj's on or off	// TODO: Make Conditional
			if (g != null)
				return;
#endif
			if (_DrawSurroundingRectangle) {
				Point p2 = pt;
				p2.Offset(p.X, p.Y);
				g.DrawRectangle(pen, p2.X, p2.Y, size.Width, size.Height);
#if false	// Further debug -- TODO:
				Font	fnt = new Font("Arial", 4);
				string msg = string.Format("pt={0} - p2={1}", pt, p2);
				g.DrawString(msg, fnt, new SolidBrush(pen.Color), p2);
#endif
			}
		}

		public enum FieldAlignment {
			Left,
			Centered,
			Right
		}

		FieldAlignment	_align;
		
		public FieldAlignment	Alignment {
			get { return _align; }
			set { _align = value; }
		}

		// Note: We can, if we want, add an Orientation (TopToBottom, BottomToTOp)

		bool	_DrawSurroundingRectangle;

		// This is mostly a debug property
		public bool DrawSurroundingRectangle {
			get { return DrawSurroundingRectangle; }
			set { DrawSurroundingRectangle = value; }
		}

//---------------------------------------------------------------------------------------

		public PrintObj(Point pt, Size size) {
			this.pt   = pt;
			this.size = size;
			_DrawSurroundingRectangle = true;		// Later, default to false TODO:
		}

//---------------------------------------------------------------------------------------

		protected int GetAlignmentOffset(int width) {
			// Note: The returned value will not be allowed to be negative. For example,
			//		 if we want to center a string inside a <size> that is too narrow for
			//		 it, the naive algorithm will produce a negative offset, which we
			//		 don't want. (Truncating the message on the right is a different
			//		 matter, and doesn't concern this routine.)
			// TODO: Rethink this, in terms of clipping regions. In particular, for
			//		 right-alignment, we probably want to truncate on the left, so a
			//		 negative offset is quite acceptable (verging on mandatory).
			switch (_align) {
			case FieldAlignment.Left:
				return 0;
			case FieldAlignment.Centered:
				return Math.Max(0, (size.Width - width) / 2);	
			case FieldAlignment.Right:
				// return Math.Max(0, (size.Width - width));
				return size.Width - width;	// TODO: See above TODO:
			default:
				return 0;
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public class PrintObjText : PrintObj {
		string		_text;
		Font		_font;
		Color		_color;
		// TODO: We can add a Brush later, if we want. Also Bold, Italic, etc. And, of
		//		 course, font name and size

//---------------------------------------------------------------------------------------

		public PrintObjText(Point pt, Size size, string text) : base(pt, size) {
			_text = text;
			// Set default values for _font and _color. Later we can add these
			// fields to the ctor. Meanwhile, the user can use the properties below.
			_font  = new Font("Arial", 10, GraphicsUnit.World);	// TODO:
			// _font  = new Font("Arial", 10);
			_color = Color.Black;
		}

//---------------------------------------------------------------------------------------

		public string Text {
			get { return _text; }
			set { _text = value; }
		}

//---------------------------------------------------------------------------------------

		public Font Font {
			get { return _font; }
			set { _font = value; }
		}

//---------------------------------------------------------------------------------------

		public Color Color {
			get { return _color; }
			set { _color = value; }
		}

//---------------------------------------------------------------------------------------

		public override void Print(Graphics g, Point p) {
			base.Print(g, p);

			Point	p2 = pt;
			int		xAlignOffset;
	
			/* *********************************************************************** */
			/* I know the following code could be optimized by bypassing the call to   */
			/* MeasureString if we're doingLeft Alignment, in which case we know that  */
			/* the result will be zero. But the overhead isn't really all that much,   */
			/* so we'll keep the code simple.                                          */
			/* *********************************************************************** */
			Size	sz = g.MeasureString(_text, _font).ToSize();
			xAlignOffset = GetAlignmentOffset(sz.Width);

			/* *********************************************************************** */
			/* The exact order of processing here is, oxymoronically, "mildly          */
			/* critical". We need to offset p2 by p, so we get the real area that we   */
			/* want to clip to. Then in DrawString, we offset p2.X by xAlignOffset.    */
			/* But don't fall into the trap of combining the two as p2.Offset(p.X +    */
			/* xAlignOffset, p.Y). This is especially clear if you're right-justifying */
			/* some text which is far too wide to fit. You could then set the clipping */
			/* rectangle to an area that is totally off the page to the left.          */
			/* *********************************************************************** */

			p2.Offset(p.X, p.Y);
#if true	// TODO:
			float gh = _font.GetHeight(g);
			int h  = _font.Height;
#endif
			// base.Print(g, p2, Pens.Green);			// TODO:
			g.SetClip(new Rectangle(p2, size)); 
			RectangleF	rect = new RectangleF(p2.X, p2.Y, size.Width, size.Height);
			// g.DrawString(_text, _font, Brushes.Black, rect, new StringFormat(StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap));
			g.DrawString(_text, _font, Brushes.Black, p2.X + xAlignOffset, p2.Y);
			// g.DrawString(_text, _font, Brushes.Black, p2.X + (xAlignOffset * 72) / 100, p2.Y);		// TODO:
			g.ResetClip();			
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public class PrintObjImage : PrintObj {
		Image		img;

//---------------------------------------------------------------------------------------

		public PrintObjImage(Point pt, Size size, Image img) : base(pt, size) {
			this.img = img;
		}

//---------------------------------------------------------------------------------------

		public override void Print(Graphics g, Point p) {
			base.Print(g, p);

			if (img != null) {
				// TODO: This could use more work. Right now, we stretch the image to
				//		 fit the area defined. So the concept of Centering, etc makes
				//		 no sense. What we need later is an option to *not* stretch it,
				//		 but to indeed center it.
				int		xAlignOffset;
				xAlignOffset = GetAlignmentOffset(img.Width);
				Point p2 = pt;
				p2.Offset(p.X + xAlignOffset, p.Y);
				g.DrawImage(img, new Rectangle(p2, size));
			}
		}
	}



//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/* ******************************************************************************* */
	/* Here's some PDF background info. It's useful in its own right, but it's also    */
	/* here to help explain a formula in the TAL DLL book (page 55) that helps         */
	/* calculate the MaxColumns field (with other concepts from other parts of the     */
	/* manual).                                                                        */
	/*                                                                                 */
	/* 1) Each "pixel" in a PDF417 image is called a "module". The module width is     */
	/* specified in mils (1/1000th of an inch), as is the module height. (Thus each    */
	/* "pixel" may well be more than 1 physical dot wide and/or high. A better way to  */
	/* think of modules isn't so much as pixels, as small vertical bars.) These are    */
	/* the most important parameters affecting the size of the PDF image. The minimum  */
	/* width is 6.53 mils (.17mm) which translates to 2 printer dots when printing at  */
	/* 300 DPI. However, the suggested width is between 10 and 30 mils (.25mm to .76   */
	/* mm). Note that since 10 mils is 1/100th of an inch, at 300 DPI each module      */
	/* would cover 3 dots.                                                             */
	/*                                                                                 */
	/* 2) There are 17 modules in a "code word". Here's where it gets complicated. The */
	/* first 16 modules (vertical bars) encode data. The 17th is always blank,         */
	/* providing an explicit break between one code word and the next. Now with 16     */
	/* modules, you potentially have 2^16 possible values. But (undoubtedly to ensure  */
	/* accurate scanning), a major restriction is placed on the bit patterns the       */
	/* modules can use. Within each 16 bit code word, there must be *exactly* 4        */
	/* instances of consecutive 1's, and *exactly* 4 runs of consecutive 0's, no one   */
	/* run being longer than 6 modules in width. So, for example,                      */
	/* 11-00-111-0-1-0-11111-0 (with hyphens inserted for clarity) would be a valid    */
	/* code word, since there are 16 bits, with 4 runs of 1's, and 4 runs of 0's.      */
	/* Replacing the final 0 by a 1 would not be valid. There would still be 16 bits,  */
	/* and 4 runs of 1's, but only 3 runs of 0's. Similarly, 11111111-00000000 would   */
	/* be invalid. There is only 1 run of 1's, and 1 run of 0's. Also, each run has a  */
	/* length of 8, which violates the "longest run is 6" rule. The documentation      */
	/* implies (I haven't checked) that out of the 65536 possibilities, there are 929  */
	/* valid code words. So a code word can certainly hold more than one byte of data. */
	/* And with the three possible compression options (numeric only (0-9), printable  */
	/* ASCII only (32-126), and byte (0-255)) even more data can be stored in each     */
	/* code word.                                                                      */
	/*                                                                                 */
	/* 3) A PDF417 image is basically a matrix. Data is encoded in multiple rows, with */
	/* multiple columns (aka code words). You can limit the image to a user-specified  */
	/* maximum number of rows (MaxRows) and/or a maximum number of columns (MaxCols).  */
	/* (Note: If you specify both MaxRows and MaxCols, and the data won't fit, the DLL */
	/* will return an error.)                                                          */
	/*                                                                                 */
	/* 4) At the left of each row, there is a Start Pattern and a Left Row Indicator.  */
	/* Similarly, at the right of each row, there is Right Row Indicator and a Stop    */
	/* Pattern. These take up an overhead of 69 modules. (Note: There is a Truncated   */
	/* PDF option that eliminates the semi-redundant data on the right, presumably     */
	/* saving us 69/2=34 modules on each row. But we'll ignore that for now.)          */
	/*                                                                                 */
	/* 5) The width of a row, in modules, is the number of code words (i.e. columns) * */
	/* 17. To this we add 69 more modules, for the edges.                              */
	/*                                                                                 */
	/* 6) So when we multiply through by the module width (e.g. 10 mils), we get the   */
	/* width of a row as RowWidth = ModWidth * (17 * MaxCols + 69)                     */
	/*                                                                                 */
	/* 7) If we need the row to narrower than, say, 2 inches (i.e. 2000 mils), we just */
	/* solve RowWidth <= 2000 for either MaxCols (if ModWidth is given), or ModWidth   */
	/* (if MaxCols is given).                                                          */
	/*                                                                                 */
	/* 8) During development (i.e. today, when I'm typing in these comments), we're    */
	/* going to start with ModWidth = 10, and solve for MaxCols, and see where that    */
	/* gets us.                                                                        */
	/* ******************************************************************************* */

	public class PrintObjBarcode : PrintObj {
	
		// TODO: Don't necessarily need this next routine, nor its struct
		[DllImport("TALC3932.DLL", EntryPoint="TALCODE39")]
		public static extern int TALCode39(ref TALBarCode bc, ref MetaFilePict mf);
	
		[DllImport("TALPDF32.dll", EntryPoint="TALPDFCODE")]
		public static extern int TALPDFCode(ref TALPDFBarCode bc, ref MetaFilePict mf);

			string		text;

		// TODO: Arguably, we need an enum to specify what kind of barcode we'll be
		//		 generating, defaulting to PDF417 on the ctor.

//---------------------------------------------------------------------------------------

		public PrintObjBarcode(Point pt, Size size, string text) : base(pt, size) {
			this.text = text;
		}

//---------------------------------------------------------------------------------------

		public override void Print(Graphics g, Point p) {
			base.Print(g, p);

			// TODO: I don't know how to get the size of the image yet, so I don't
			//		 know how to implement the centering option that the other
			//		 PrintObj's do.

			Point		p2 = pt;
			p2.Offset(p.X, p.Y);
			// TODO: I don't know enough about barcodes to know how to generalize
			//		 things properly. So for now, we'll just support PDF.
			TALPDFBarCode	bc = new TALPDFBarCode();
			MetaFilePict	mfp = new MetaFilePict();
			bc.MessageBuffer = text;
			bc.MessageLength = text.Length;
			// bc.CommentBuffer = "Hello"; bc.CommentLength = 5;	// TODO:


#if ENHMETAFILE_TESTING
			bc.OutputOption = 2;		// Output to in-memory metafile TODO: Make enum
			bc.Preferences = 65536;		// Aldus MF
#endif

			bc.PDFModuleWidth = 30;
			bc.PDFModuleHeight = 3 * bc.PDFModuleWidth;	// Suggested ratio
			// bc.PDFAspect = .1000f;

			// By default, all printing is done in units of 1/100 inches.
			bc.XPosInInches = p2.X / 100f;
			bc.YPosInInches = p2.Y / 100f;

			/* *********************************************************************** */
			/* Limit the number of columns to try to control the width of the barcode. */
			/* Of course, this may well cause the height to expand. But for now we'll  */
			/* assume that width is more important than height.                        */
			/* *********************************************************************** */

			// The <size> member variable is in Display units, which, for a printer, is
			// in units of 1/100th of an inch, so we'll multiply by 10 to get it in mils.
			float		MaxWidth = size.Width * 10;
			int			MaxCols = (int)(((MaxWidth / bc.PDFModuleWidth) - 69f) / 17f);
			MaxCols = 10;				// TODO:
			// bc.PDFMaxRows = 30;			// TODO:
			bc.PDFMaxCols = MaxCols;

			int		rc;

#if true	// TODO: Write PDF to file - Try #1
			bc.OutputOption = 1;		// To file
			string		filename = @"C:\PDF.wmf";
			bc.OutputFilename = filename;
			rc = TALPDFCode(ref bc, ref mfp);
			Image	img = Image.FromFile(filename);
			// TODO: Next line hardcodes 300DPI
			// Size	sz = new Size(size.Width * 300 / 100, size.Height * 300 / 100);
			Bitmap	bmp = new Bitmap(img, size);	
			//IntPtr hbmp = bmp.GetHbitmap();
			// g.DrawImage(bmp, 100, 100, bmp.Width, bmp.Height);
			// g.DrawImage(bmp, pt.X, pt.Y);
			// bmp.Save(filename + ".bmp");
			int		xAlignOffset;
			p2 = pt;			// Duplicate of above
			xAlignOffset = GetAlignmentOffset(bmp.Width);
			p2.Offset(p.X + xAlignOffset, p.Y);
			g.DrawImage(img, new Rectangle(p2, size));
#endif

#if false	// TODO: Write PDF to file - Try #2
			// TODO: Check out new DLLs. Should be able to set hDC = 0 and get dimensions
			//		 out in mfp.
			bc.OutputOption = 3;		// To hDC - I'm hoping that an hBMP is OK too
			Bitmap	bmp = new Bitmap(300, 300, g);
			ImageFormat	ifmt = bmp.RawFormat;
			string ifmts = ifmt.ToString();
			Graphics	gbmp = Graphics.FromImage(bmp);
			IntPtr		ghDC = gbmp.GetHdc();
			float		SaveX = bc.XPosInInches, SaveY = bc.YPosInInches;
			bc.XPosInInches = 0;
			bc.YPosInInches = 0;
			// bc.OutputhDC = ghDC;			// Doesn't print, but fills in mfp
			bc.OutputhDC = IntPtr.Zero;			// Doesn't print, but fills in mfp
			try {
				rc = TALPDFCode(ref bc, ref mfp);
				if (rc != 0) {
					MessageBox.Show("Write PDF to file #2 failed - " + rc.ToString());
					return;
				}
			} finally {
				gbmp.ReleaseHdc(ghDC);
			}
			bmp.Save(@"C:\PDF2.bmp");
			// g.DrawImage(img, 100, 100, img.Width, img.Height);
			bc.XPosInInches = SaveX;
			bc.YPosInInches = SaveY;
#endif

#if false	// TODO: Write PDF to clipboard
			bc.OutputOption = 0;
			rc = TALPDFCode(ref bc, ref mfp);
#endif

#if false	// Output to hDC
			bc.OutputOption = 3;		// Output to hDC TODO: Make enum
			IntPtr	hDC = g.GetHdc();
			try {
				bc.OutputhDC = hDC;
				rc = TALPDFCode(ref bc, ref mfp);
#if ENHMETAFILE_TESTING	
				Metafile	mf = new Metafile(mfp.hMf, true);
				int		xAlignOffset;
				xAlignOffset = GetAlignmentOffset(mf.Width);
				Point p3 = pt;
				p3.Offset(p.X + xAlignOffset, p.Y);
				g.DrawImage(mf, new Rectangle(p3, size));
#endif
			} finally {
				g.ReleaseHdc(hDC);		// Make sure this is in a <finally> clause
			}
#endif
			if (rc != 0) {
				// TODO: Need something better than this
				MessageBox.Show("Barcode failure - rc = " + rc.ToString());
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A Print Request is simply an ArrayList of PrintPage's. Yeah, we could use the
	/// ArrayList directly, but it's conceptually cleaner if we alias the list.
	/// TODO: This class name is no longer quite right.
	/// </summary>
	public class PrintReq {
		public ArrayList	objs;		// In Whidbey, this will be ArrayList<PrintPage>

//---------------------------------------------------------------------------------------

		public PrintReq() {
			objs = new ArrayList();
		}

//---------------------------------------------------------------------------------------

		public void Add(PrintPage obj) {
			objs.Add(obj);
		}

//---------------------------------------------------------------------------------------

		public void Clear() {
			objs.Clear();
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

// Barcode stuff (TAL, PDF, etc) follows. TODO: Generalize this later.

	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct TALPDFBarCode {					// TODO: Put in rest of comments
		public int		MessageLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=2712)] 
		public string	MessageBuffer;
		// char 		messageBuffer[2712];		// That's what the doc says, 2712
		public int		CommentLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=100)] 
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
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=32)] 
		public string	FontName;
		// char fontName[32]
		public int		FontSize;
		public uint		TextColor;
		public int		Orientation;
		public int		Preferences;
		public int		HotizontalDPI;
		public int		VerticalDPI;
		public int		OutputOption;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=260)] 
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
	

	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct MetaFilePict {
		public int		mm;						// Metafile map mode
		public int		xExt;					// Width of the metafile
		public int		yExt;					// Height of the metafile
		public IntPtr	hMf;					// Handle to the actual metafile in memory
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	// TODO: We may not need this later
	[StructLayout(LayoutKind.Sequential)]		// TODO: Pack=1 removed
	public struct TALBarCode {
		public int		MessageLength;				// length of message
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=100)] 
		public string	MessageBuffer;
		// char 		messageBuffer[100];			// That's what the doc says, 100
		public int		CommentLength;
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=100)] 
		public string	CommentBuffer;
		public int		NarrowBarWidth;				// In mils
		public int		BarWidthReduction;			// Percent of NarrowBarWidth
		public int		BarCodeHeight;				// In mils
		public int		FGColor;					// Foreground RGB color
		public int		BGColor;					// Background RGB color
		public int		NarrowToWideRatio;			// Integer 20-30 (=2.0 to 3.0)
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=32)] 
		public string	FontName;
		public int		MyFontSize;					// Font size in points
		public int		TextColor;					// Text color - RGB color
		public int		Orientation;				// 0 - 3 or 0, 90 , 180, 270
		// 0 = horizontal   (normal)
		// 1 = vertical     (rotated 90 degrees)
		// 2 = horizontal   (rotated 180 degrees - i.e. upside down)
		// 3 = vertical     (rotated 270 degrees)
		public int		Preferences;				// Bit values as below
		// DoNotDisplayText       =  1
		// TextOnTop              =  2
		// CommentOnBottom        =  4
		// QuietZones             =  8
		// BearerBars             =  16
		// Code39FullASCII        =  32
		// DisplayStartStopChars  =  64
		// OptionalCheckDigit1    =  128
		// OptionalCheckDigit2    =  256
		// DisplayCheckDigit      =  512
		// MyFontBold             =  1024
		// MyFontItalic           =  2048
		// MyFontUnderLine        =  4096
		// MyFontStrikeOut        =  8192
		// FontScaling            =  16384
		// AdjustToPrinterDPI     =  32768
		public int		HorizontalDPI;
		public int		VerticalDPI;
		public int		OutputOption;				// 0=Clipboard, 1=SaveToFile, 2=MetafilePict
		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst=260)] 
		public string	OutputFilename;
		public IntPtr	OutputhDC;					// Output device context (when outputting to hDC)
		public float	XPosInInches;				// X page position (when outputting to hDC)
		public float	YPosInInches;				// Y page position (when outputting to hDC)
		int				Reserved;					// Reserved for possible future 32 bit use
	}

}
