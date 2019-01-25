// Copyright (c) 2007 by Larry Smith

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace Boondog2009 {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

    class BogSquare : Label {
        // Note; Deriving from Label gives us all kinds of properties, such as
        //       Text, Location, Size, etc

		[DllImport("user32.dll")]
		static extern bool DrawEdge(IntPtr hdc, ref Rectangle qrc, uint edge,
		  uint grfFlags);

		public static Boondog2009	BogForm;
		public static Font			SqFont;
		public static Board			board;
        public static Game          game;
		public static string		CurrentWord;
        public static int           TrackLen;       // TODO: Not yet set
        public static byte[]        TrackLetters;   // TODO: Not yet set

		public int					VisitCount;	// How many times this square has been
												//   used in this word

        int row, col;               // TODO: I think we'll want these later. I think...

//---------------------------------------------------------------------------------------

		public static void Init(Boondog2009 form, Font fnt) {
			BogForm = form;
			SqFont	= fnt;
			CurrentWord = "";
		}

//---------------------------------------------------------------------------------------

		public static void SetBoard(Board TheBoard) {
			board = TheBoard;
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			// Reset this square to its default values
			SetDefaultColor();
			this.VisitCount = 0;
		}

//---------------------------------------------------------------------------------------

		public void SetDefaultColor() {
			this.BackColor = Color.Blue;
			this.ForeColor = Color.White;
		}

//---------------------------------------------------------------------------------------

		public void SetHighlightColor() {
			this.BackColor = Color.Red;
		}

//---------------------------------------------------------------------------------------

        public BogSquare(int row, int col) {
			Reset();
            this.row = row;
            this.col = col;

			this.AutoSize = false;
			this.BorderStyle = BorderStyle.None;
			this.TextAlign = ContentAlignment.MiddleCenter;

			this.Font = SqFont;
        }

//---------------------------------------------------------------------------------------

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			// Font	fnt = new Font("Arial", 10);
			// Brush	brush = Brushes.AliceBlue;
			Graphics	g = e.Graphics;
			// g.DrawString("Hiya", fnt, brush, x, y);
			Rectangle	r = new Rectangle(0, 0, Size.Width, Size.Height);
			FrameIt(g, r, DrawEdgeFlags.EDGE_RAISED, 3);
		}

//---------------------------------------------------------------------------------------

		void FrameIt(Graphics g, Rectangle rect, uint flag, int level) {
			// <flag> is one of EDGE_BUMP, _ETCHED, _RAISED, _SUNKEN
			// <level> is how wide you want the border
			// TODO: Try to not use _BUMP, since WinCE doesn't support it
			// TODO: Check into ControlPaint.DrawBorder[3D]
			Rectangle	r = rect;
			IntPtr	hDC = g.GetHdc();
			for (int i=0; i<level; i++) {
				DrawEdge(hDC, ref r, flag, DrawEdgeFlags.BF_RECT); 
				++r.X;
				++r.Y;
				--r.Width;
				--r.Height;
			}
			g.ReleaseHdc(hDC);
		}

//---------------------------------------------------------------------------------------

		protected override void OnClick(EventArgs e) {
			base.OnClick(e);
			FlipColor();
			CurrentWord += this.Text;
		}

//---------------------------------------------------------------------------------------

		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick(e);
			// TODO: More here. e.g. Update various count controls, etc.
            byte[] letters = { 1, 2, 3 };   // TODO: Kludge
			// TODO: Added min length check to <NewWord>. Probably should go here.
            BogForm.AddWord(false, CurrentWord, letters, letters.Length);
			board.NewWord();
		}
		 

//---------------------------------------------------------------------------------------

		private void FlipColor() {
			if (this.BackColor == Color.Blue) {
				this.BackColor = Color.Red;
			} else {
				this.BackColor = Color.Blue;
			}
		}
    }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

    // TODO: Should go in separate clsas file
    public class DrawEdgeFlags {
        /* 3D border styles */
        public const uint BDR_RAISEDOUTER = 0x0001;
        public const uint BDR_SUNKENOUTER = 0x0002;
        public const uint BDR_RAISEDINNER = 0x0004;
        public const uint BDR_SUNKENINNER = 0x0008;

        public const uint BDR_OUTER = (BDR_RAISEDOUTER | BDR_SUNKENOUTER);
        public const uint BDR_INNER = (BDR_RAISEDINNER | BDR_SUNKENINNER);
        public const uint BDR_RAISED = (BDR_RAISEDOUTER | BDR_RAISEDINNER);
        public const uint BDR_SUNKEN = (BDR_SUNKENOUTER | BDR_SUNKENINNER);


        public const uint EDGE_RAISED = (BDR_RAISEDOUTER | BDR_RAISEDINNER);
        public const uint EDGE_SUNKEN = (BDR_SUNKENOUTER | BDR_SUNKENINNER);
        public const uint EDGE_ETCHED = (BDR_SUNKENOUTER | BDR_RAISEDINNER);
        public const uint EDGE_BUMP = (BDR_RAISEDOUTER | BDR_SUNKENINNER);

        /* Border flags */
        public const uint BF_LEFT = 0x0001;
        public const uint BF_TOP = 0x0002;
        public const uint BF_RIGHT = 0x0004;
        public const uint BF_BOTTOM = 0x0008;

        public const uint BF_TOPLEFT = (BF_TOP | BF_LEFT);
        public const uint BF_TOPRIGHT = (BF_TOP | BF_RIGHT);
        public const uint BF_BOTTOMLEFT = (BF_BOTTOM | BF_LEFT);
        public const uint BF_BOTTOMRIGHT = (BF_BOTTOM | BF_RIGHT);
        public const uint BF_RECT = (BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM);

        public const uint BF_DIAGONAL = 0x0010;

        // For diagonal lines, the BF_RECT flags specify the end point of the
        // vector bounded by the rectangle parameter.
        public const uint BF_DIAGONAL_ENDTOPRIGHT = (BF_DIAGONAL | BF_TOP | BF_RIGHT);
        public const uint BF_DIAGONAL_ENDTOPLEFT = (BF_DIAGONAL | BF_TOP | BF_LEFT);
        public const uint BF_DIAGONAL_ENDBOTTOMLEFT = (BF_DIAGONAL | BF_BOTTOM | BF_LEFT);
        public const uint BF_DIAGONAL_ENDBOTTOMRIGHT = (BF_DIAGONAL | BF_BOTTOM | BF_RIGHT);


        public const uint BF_MIDDLE = 0x0800;   /* Fill in the middle */
        public const uint BF_SOFT = 0x1000;     /* For softer buttons */
        public const uint BF_ADJUST = 0x2000;   /* Calculate the space left over */
        public const uint BF_FLAT = 0x4000;     /* For flat rather than 3D borders */
        public const uint BF_MONO = 0x8000;     /* For monochrome borders */
    }
}
