using System;

using Xamarin.Forms;

namespace nsBoondog2019 {
	public class BogCube : Button {

		internal int     row, col;
		internal int     VisitCount;

		public BogCube() {
			// Not sure why we need this in the Xamarin version
		}

//---------------------------------------------------------------------------------------

		public BogCube(BogCube cube) {
			this.Text  = cube.Text;
			row        = 0;
			col        = 0;
			VisitCount = 9;
		}
	}
}

#if false
// Copyright (c) 2007-2014 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

/*
Originally we had a vector ([], not List<>) of TrackLetters to keep track of the words
the player and program were building. These were int's (row * rank + col). This let us do
things like recall the last word, highlight words by clicking in a listbox, etc.

But it had problems with "Qu", rotating the board, recalling words after the board had
been rotated, etc.

So now we keep things in a BogCube. Once the dice have been shaken (i.e. in a new Game), 
this a (mostly) immutable data structure. The text is kept in its base class (Label)
.Text property and its original "North" orientation defines its row and col location
(used for adjacency testing). The nice thing about this is that rotating the board is now
a matter of taking these BogCube's (aka Label's) and placing them accordingly. Each cube
now retains its .Text value, its initial row,col etc.

Note that this class was originally called BogSquare (and in comments you may still find
comments to Squares). At some later point I may want this class to maintain the entire
six letters ("Qu" = 1 letter) of the physical die so that I can do things (probably
involving WPF) like animate the dice when shaking the box or rotating the board.

See also BogWord.

Note: Here are comments from when I realized I wanted to get rid of TrackLetters...

To be able to recall the previous word, we're using <TrackLetter's>. This is the
wrong approach. Instead, each BogSquare should have its own ID (probably just a
reference to its instance), and we track the BogSquares. These in turn have a
BogLetter inside them (or should we just keep the text inside the BogSquare and do 
away with the BogLetter's totally? I think we should.). Anyway, the BogSquare used to
be a location on the grid, and would have different text inside it, depending on how
the board was rotated. Now we should think about it as a physical cube that always
has the same text (differing from game-to-game, of course). And when we rotate the
board, the only thing that changes is the order in which the cubes are displayed.
Note that this will help displaying the last word properly, even if the board is
rotated.

I hereby declare that BogSquare is now named BogCube and BogLetter is no longer used.
However, in the comments I sometimes use the term "cube" and "square" interchangeably

*/

using System;

using Xamarin.Forms;

using LRSUtils;

namespace BogDroid_2019_03 {

	/// <summary>
	/// A BogCube (derived from Label) represents a physical die and the letter(s -- Qu)
	/// on its face.
	/// </summary>
	public class BogCube : Button { // TODO: GelButtons.GelButton {	// TODO: TODO: TODO: Control {
		public static Boondog2019   BogForm;    // I *could* perhaps refer to a static
												// field in Boondog2019.cs, but hey, I
												// think I'll leave this here for now

		public int                  VisitCount; // How many times this cube has been
												// used in this word. Used for Judie
												// rules support 

		int                         row, col;   // Needed for adjacency testing

		// TODO: Get these from Options
		Color NormalForeColor    = Color.Blue;      // Ivory, Linen, MintCream, Wheat, Moccasin

		Color NormalBackColor    = Color.Blue;
		Color HighlitBackColor   = Color.Red;

		Color NormalRadialColor  = Color.SaddleBrown; // .LimeGreen; .Orange;
		Color HighlitRadialColor = Color.Blue;

		Color CurrentBackColor;

//---------------------------------------------------------------------------------------

		public BogCube() {
			// Used only for XML serialization
		}

//---------------------------------------------------------------------------------------

		public static void Init(Boondog2019 form) {
			BogForm = form;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructs a cube from its row and column
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		public BogCube(int row, int col) {
			BogCubeCtor(row, col);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructs a new cube from an existing one. A clone routine.
		/// </summary>
		/// <param name="bc"></param>
		public BogCube(BogCube bc) {
			BogCubeCtor(bc.row, bc.col);
			this.Text = bc.Text;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Common ctor routine, called from several real ctors
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		private void BogCubeCtor(int row, int col) {
			Reset();
			this.row = row;
			this.col = col;

			// this.AutoSize = false;
			// TODO: Next 2 lines
			// this.BorderStyle = BorderStyle.None;
			// this.TextAlign   = ContentAlignment.MiddleCenter;

			this.Font = Boondog2019.SqFont;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Reset this cube to its default values
		/// </summary>
		public void Reset() {
			SetDefaultColor();
			this.VisitCount = 0;
			// this.Invalidate();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Set the default fore and background colors for this cube
		/// </summary>
		public void SetDefaultColor() {
			// this.BackColor = BogForm.BackColor;   // NormalBackColor;
			// this.ForeColor = NormalForeColor;
			// this.ForeColor = Color.Purple;			// TODO: Remove
			CurrentBackColor = NormalBackColor;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Set the background color to highlight this cube
		/// </summary>
		public void SetHighlightColor() {
			// TODO: TODO: TODO:
			// this.BackColor = HighlitBackColor;
			CurrentBackColor = HighlitBackColor;
			// this.Invalidate();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Draws the BogCube in a slightly 3-D fashion
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e) {
			switch (BogForm.CubeFormat) {
				case Boondog2019.CubeFormats.Rainbow:
					OnPaintRainbow(e);
					break;
				default:
				case Boondog2019.CubeFormats.Glass:
					ShowAsGlass(e);
					break;
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowAsGlass(PaintEventArgs e) {
			// http://blogs.msdn.com/b/cjacks/archive/2006/02/23/538164.aspx
			// http://blogs.msdn.com/b/cjacks/archive/2006/03/01/541384.aspx
			// http://blogs.msdn.com/b/cjacks/archive/2006/03/08/546259.aspx

			// http://techmantium.com/vista-style-glass-buttons/

#if false
			var btn       = new GelButtons.GelButton();
			// var btn    = new Button();
			btn.Text      = this.Text; ;
			btn.BackColor = Color.Red;
			btn.ForeColor = Color.White;
			btn.Location  = this.Location;
			btn.Size      = this.Size;
			BogForm.Controls.Add(btn);
			btn.BringToFront();
			// btn.Invalidate();
#endif
			OnPaintGlass(e, this.Font);
		}

//---------------------------------------------------------------------------------------

		private void OnPaintGlass(PaintEventArgs e, Font fnt) {
			Color gradientTopNormal;
			Color gradientBottomNormal;
			if (true) {                         // TODO: Experimental code follows
												// gradientTopNormal    = Color.Tan;
												// gradientBottomNormal = Color.Tan;
				gradientTopNormal = Color.FromArgb(255, 44, 255 + 0 * 85, 177);
				gradientBottomNormal = Color.FromArgb(255, 153, 198, 241);
			} else {        // "Real" code follows
				gradientTopNormal = Color.FromArgb(255, 44, +32 + 0 * 85, 64 + 0 * 177 + 0 * 255);
				gradientBottomNormal = Color.FromArgb(255, 153, 198, 241);
			}

			Color gradientTopHighlit    = Color.FromArgb(255, 255, 85, 177);
			Color gradientBottomHighlit = Color.FromArgb(255, 153, 198, 241);

			// TODO: Options
			// var xxx = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
			// var yyy = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
			// var zzz = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

			Color gradientTop;
			Color gradientBottom;

			if (CurrentBackColor == HighlitBackColor) {
				gradientTop = gradientTopHighlit;
				gradientBottom = gradientBottomHighlit;
			} else {
				gradientTop = gradientTopNormal;
				gradientBottom = gradientBottomNormal;
			}

			Graphics g = e.Graphics;
			// Fill the background
			using (SolidBrush backgroundBrush = new SolidBrush(this.BackColor)) {
				g.FillRectangle(backgroundBrush, this.ClientRectangle);
			}
			// Paint the outer rounded rectangle
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle outerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
			using (GraphicsPath outerPath = RoundedRect(outerRect, 5, 0)) {
				using (var outerBrush = new LinearGradientBrush(outerRect, gradientTop, gradientBottom, LinearGradientMode.Vertical)) {
					g.FillPath(outerBrush, outerPath);
				}
				using (var outlinePen = new Pen(gradientTop)) {
					g.DrawPath(outlinePen, outerPath);
				}
			}
			// Paint the highlight rounded rectangle
			Rectangle innerRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height / 2 - 1);
			using (GraphicsPath innerPath = RoundedRect(innerRect, 5, 2)) {
				using (var innerBrush = new LinearGradientBrush(innerRect, Color.FromArgb(255, Color.White), Color.FromArgb(0, Color.White), LinearGradientMode.Vertical)) {
					g.FillPath(innerBrush, innerPath);
				}
			}
			// Paint the text
			// var xxx = this.Parent as nsBoondog2019.Boondog2019;
			// TextRenderer.DrawText(g, this.Text, this.Font, outerRect, this.ForeColor, Color.Transparent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
			TextRenderer.DrawText(g, this.Text, fnt, outerRect, this.ForeColor, Color.Transparent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
		}

//---------------------------------------------------------------------------------------

		private GraphicsPath RoundedRect(Rectangle boundingRect, int cornerRadius, int margin) {
			GraphicsPath rr = new GraphicsPath();       // rr = RoundedRect
			rr.AddArc(boundingRect.X + margin, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 180, 90);
			rr.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
			rr.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
			rr.AddArc(boundingRect.X + margin, boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
			rr.CloseFigure();
			return rr;
		}

//---------------------------------------------------------------------------------------

		private void OnPaintRainbow(PaintEventArgs e) {
			// http://www.codeproject.com/Articles/38780/Glass-Effect-Extender-Library-for-your-Application
			// http://code.msdn.microsoft.com/windowsdesktop/CSWinFormExAeroToClient-3b123c56
			// http://www.codeproject.com/KB/GDI-plus/Gradiator.aspx?msg=2732284
			// http://techmantium.com/vista-style-glass-buttons/
			// http://www.codeproject.com/Articles/17695/Creating-a-Glass-Button-using-GDI

			Rectangle r = new Rectangle(0, 0, Size.Width, Size.Height);
#if true
			// this.Margin = new Padding(0);				// TODO:
			/*
			Look into drawing a rounded rectangle. Find RoundedRectangle class?
			http://pages.citebite.com/e1u2t5b7t4bih
			http://msdn.microsoft.com/en-us/library/microsoft.visualbasic.powerpacks.rectangleshape.aspx
			Or even better, http://www.tallcomponents.com/kb000312.aspx
			http://www.codeproject.com/Articles/20879/WindowsVistaRenderer-A-New-Button-Generation
			http://www.codeproject.com/Articles/18000/Enhanced-GlassButton-using-GDI
			http://stackoverflow.com/questions/6479232/how-create-glossy-button-in-c
			http://www.codeproject.com/Articles/17695/Creating-a-Glass-Button-using-GDI
			http://techmantium.com/vista-style-glass-buttons/
			http://www.codeproject.com/Articles/38436/Extended-Graphics-Rounded-rectangles-Font-metrics
				WPF
			http://learnwpf.com/post/2008/01/31/Glossy-Brushes-using-Radial-Gradient-Brush-in-WPF.aspx
				PAINT.NET et al
			http://en.wikibooks.org/wiki/Paint.NET/Simple_Round_Glass_Buttons
			*/
			var path = new GraphicsPath();
			path.AddRectangle(r);
			Color[] colors = new Color[] {  Color.Brown,
											Color.Orange, Color.Yellow, Color.Green,	// TODO:
//											Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet	// TODO:
			};
			var PathBrush = new PathGradientBrush(path) { 
				// PathBrush.CenterColor = this.BackColor == NormalBackColor ? NormalRadialColor : HighlitRadialColor;
				// PathBrush.CenterColor = this.BackColor == BogForm.BackColor ? NormalRadialColor : HighlitRadialColor;
				CenterColor = CurrentBackColor, // Color.Blue;
												// Color[] colors = { Color.FromArgb(255, 255, 0, 0) };
												// Color[] colors = { this.BackColor };
				SurroundColors = colors
			};

			// PathBrush.Blend.Positions = new float[] { 0.9f };

			Graphics g = e.Graphics;
			using (var MyPen = new Pen(Brushes.Snow, 2.0f)) {
				// using (var MyPen = new Pen(PathBrush, 5.0f)) {
				// RoundedRectangle.DrawRoundedRectangle(g, r, 25, MyPen, this.BackColor == BogForm.BackColor ? NormalRadialColor : HighlitRadialColor);
				RoundedRectangle.DrawRoundedRectangle2(g, r, 25, MyPen, PathBrush);
			}

#if true                   // TODO: Test code
			// g.FillPath(PathBrush, path);
#else
			// e.Graphics.FillRectangle(PathBrush, 0, 0, 140, 70);

			e.Graphics.FillRectangle(PathBrush, 0, 0, r.Width, r.Height);
#endif
#endif
			// base.OnPaint(e);		// TODO: Is this needed?

			var xxx = TextRenderer.MeasureText(this.Text, this.Font);   // TODO:

			g.DrawString(this.Text, this.Font, Brushes.White, new PointF((this.Width - xxx.Width) / 2.0f, (this.Height - xxx.Height) / 2.0f));

#if false
			int BorderWidth = 3;
			FrameIt(g, r, DrawEdgeFlags.EDGE_RAISED, BorderWidth);
#endif
#if false
			var TileValue = Board.TileScores[this.Text.ToUpper()].ToString();
			// var xxx = TextRenderer.MeasureText(TileValue, lblPlayerStats.Font);	// TODO:
			var font = new Font("Arial", 8.25f);
			var xxx = TextRenderer.MeasureText(TileValue, font);	// TODO:
			Brush	brush = Brushes.AliceBlue;
			int x = this.Width - BorderWidth - xxx.Width - 2;
			int y = this.Height - BorderWidth - xxx.Height - 3;
			// string msg = string.Format("({0}, {1})", row, col);
			// g.DrawString(msg, fnt, brush, x, y);
			g.DrawString(TileValue, fnt, brush, x, y);
#endif
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Draws a 3-D frame around the edge of the cube
		/// </summary>
		/// <param name="g"></param>
		/// <param name="rect"></param>
		/// <param name="flag"></param>
		/// <param name="level"></param>
		void FrameIt(Graphics g, Rectangle rect, uint flag, int level) {
			// <flag> is one of EDGE_BUMP, _ETCHED, _RAISED, _SUNKEN
			// <level> is how wide you want the border
			// TODO: Check into ControlPaint.DrawBorder[3D]
			Rectangle   r = rect;
			IntPtr  hDC = g.GetHdc();
			for (int i = 0; i < level; i++) {
				NativeMethods.DrawEdge(hDC, ref r, flag, DrawEdgeFlags.BF_RECT);
				++r.X;
				++r.Y;
				--r.Width;
				--r.Height;
			}
			g.ReleaseHdc(hDC);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Processes a click on the cube and routes it depending on whether it was a
		/// left- or right-click
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e) {
			base.OnClick(e);
			var args = e as MouseEventArgs;
			if (args.Button == MouseButtons.Left) {
				LeftClickRoutine();
			} else if (args.Button == MouseButtons.Right) {
				Boondog2019.board.Backspace();
			} else if (args.Button == MouseButtons.Middle) {
				// Recall previous word and backspace
				Boondog2019.board.Backspace();
			}
		}
#endif

#if false

//---------------------------------------------------------------------------------------

/// <summary>
/// Processes a normal left-click on the cube
/// </summary>
public void LeftClickRoutine() {
			if (this.Text == " ") {
				BogForm.SetStatusLine("Can't click the blank square", true);
				return;

			}
			if (!IsAdjacent(Boondog2019.board.CurrentWord)) {
				// TODO: Give error message on status line? For now, just ignore
				BogForm.SetStatusLine("Square clicked must be adjacent to the previous square", true);
				return;
			}

			if (++VisitCount > 1) {
				// TODO: Must decrement VisitCount on backspace
				if (!BogForm.PlayerProf.AllowJudieRules) {
					string msg = "Your Options don't allow you to reuse the same square in a word";
					MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			SetHighlightColor();
			Boondog2019.board.CurrentWord.AppendCube(this);
			BogForm.ShowWordSoFar(Boondog2019.board.CurrentWord);
		}

#if false
		//---------------------------------------------------------------------------------------

		/// <summary>
		/// A double-click on a cube means the putative word is done. Look into it
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick(e);

			var args = e as MouseEventArgs;
			if (args.Button == MouseButtons.Right) {
				return;             // Ignore right-button double click. It doesn't
									// signify the end of a word (or anything else)
			}

			// We've got a bug where if the word we've just double-clicked on isn't
			// valid (e.g. too short) and has a MessageBox displayed, then we try to
			// recall the last word (the invalid one) to try to fix it, we don't get it. 
			// What we get is the last valid word. Which is rather disconcerting to the
			// user. So regardless of whether the word is valid or not, we'll always set
			// LastWord = CurrentWord here.
			// TODO: Check to see if setting LastWord = CurrentWord anyplace else in the
			//		 code is redundant. It probably is
			// Boondog2019.board.LastWord = Boondog2019.board.CurrentWord;

			// Double-clicking on a square that isn't adjacent to the previous square
			// isn't allowed.
			// TODO: Isn't this checked for in LeftClickRoutine???
			if (!IsAdjacent(Boondog2019.board.CurrentWord, true)) {
				BogForm.SetStatusLine("Square clicked must be adjacent to the previous square", true);
				return;
			}

			Boondog2019.board.EndPlayerWord();
		}
#endif
#endif

#if false
//---------------------------------------------------------------------------------------

/// <summary>
/// Checks to see if the cube clicked was next to the previously clicked cube.
/// </summary>
/// <param name="CurrentWord"></param>
/// <param name="bIsDoubleClick"></param>
/// <returns></returns>
private bool IsAdjacent(BogWord CurrentWord, bool bIsDoubleClick = false) {
			if (CurrentWord.Count == 0) {
				return true;                // First letter always OK
			}

			if (bIsDoubleClick) {
				// A double-click is exempt from the adjacency rules
				return true;
			}

			var cube = CurrentWord.LastCube();

			bool bIsAdjacent = true;

			// The following tests are inefficient, in the sense that even if we
			// determine that, say, a cube isn't in an adjacent row, we'll still check
			// for an adjacent column. But testing this way makes the code noticeably
			// clearer.

			if (this.row > cube.row + 1) {
				bIsAdjacent = false;
			}

			if (this.row < cube.row - 1) {
				bIsAdjacent = false;
			}

			if (this.col > cube.col + 1) {
				bIsAdjacent = false;
			}

			if (this.col < cube.col - 1) {
				bIsAdjacent = false;
			}

			if ((this.row == cube.row) && (this.col == cube.col)) {
				if (!BogForm.PlayerProf.AllowJudieRules) {
					bIsAdjacent = false;
				}
			}

			return bIsAdjacent;
		}
	}
}
#endif