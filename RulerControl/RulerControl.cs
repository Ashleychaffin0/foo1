// Copyright (c) 2009 by Larry Smith

// TODO: Some ideas as to what this thing should support, and how to do it
//	1)	The basic idea is that this control will hook into the Windows Proc for its
//		parent, and use this to update its "pointer".
//			a) 	Application.AddMessageFilter(new UIMonitorMessageFilter());
//			b) 	private class UIMonitorMessageFilter : IMessageFilter {
//				public bool PreFilterMessage(ref Message m) {

//	2)	It will support both horizontal and vertical orientations.
//	3)	It will support Metric and American.
//	4)	It will support a Zoom factor, so when its parent is zoomed, it will zoom
//		as well (but needs to be informed by its parent about the zoom factor).
//	5)	Initially it must be as wide (tall) as its parent, but this could/should be
//		relaxed in a later version.
//	6)	Its height (width) is up to the user. Some kind of AutoFont for the display
//		inside the control would be nice.
//	7)	The actual display will be something like
//			Inches (header optional)
//			-2   |   |   |  -3 ...
//			 ||||||||||||||||| ...
//	8)	Zooming might change this to
//			Inches (header optional)
//			-2.5 |   |   |-2.4 ...
//			 ||||||||||||||||| ...
//	9)	The "pointer" is a vertical (hortizontal) bar, like in Word, xor'ed.
//	10)	The parent (via the ctor and/or properties) will specify how many inches (meters,
//		centimeters, feet, etc) the width of the window represent. The control will
//		figure out how many pixels/inch (etc) there will be. 
//	11)	There must be some provision for tic-marks (e.g. show only 1/4", or even 1/64",
//		or divide into 1/10ths for metric, etc).
//	12)	There must be some way to dynamically turn the ruler pointer on and off.
//	13)	We should probably honor the Font property of the Control.
//	14)	We assume we always run on the GUI thread. Check when we're created, and throw
//		an exception if not.
//	15)	This is for Winforms only. Worry about WPF (and how WndProc interacts with it) 
//		later.
//	16)	Need a way for the parent to query the ruler's current value.
//	17)	The ruler may be placed (docked?) to the top, bottom, left or right of the
//		parent. We should also support non-docked locations.
//	18)	Hmmm. The parent may well be scrollable. So it's *not* true that 

// Bugs:
//	1)	If we lose focus (Activate?), such as Alt-Tab, or clicking on a button and 
//		having a MsgBox pop up, the previous caret doesn't get cleared. Probably have
//		to intercept/handle a few more messages/events.
//	2)	Sigh. Looks like we'll have to do our own double-buffering on the caret.
//	3)	Set opts, then add API to Ruler to pay attention to new opts.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RulerControl {
	class RulerOptions {
		public Orientation	HorizontalOrVertical;
		// The next fields sound like they might be similar to the Top/Bottom/Left/Right
		// properties. But they're not. If the ruler starts at, say, 0, then RulerLeft 
		// would be 0. If we've scrolled and the left margin is now at, say, 2.5, 
		// then RulerLeft would be 2.5.
		// Similarly, RulerWidth is the number of Units (e.g. inches) that the ruler is
		// (for example, RulerLeft = 2.5 and RulerWidth = 7 would show a ruler starting
		// at 2.5 (inches), and going to 9.5 (inches).
		// Note that as the parent control resizes, this will fluctuate.
		// 
		// TODO: Find better names
		public float		RulerLeft;
		public float		RulerWidth;
		public float		TickFactor;		// e.g. Show 10 ticks per "inch"

//---------------------------------------------------------------------------------------

		public RulerOptions() {
			SetDefaultValues();
		}

//---------------------------------------------------------------------------------------

		private void SetDefaultValues() {
			HorizontalOrVertical = Orientation.Horizontal;
			RulerLeft = 2.5f;
			RulerWidth = 6.0f;
			TickFactor = 4;
		}

//---------------------------------------------------------------------------------------

		public enum Orientation {
			Horizontal,
			Vertical
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class RulerControl : UserControl {

//---------------------------------------------------------------------------------------

		public	Control			ParentControl;
		public	RulerOptions	opts;

		// The following fields keep track of where the "caret" (thin vertical/horizontal
		// bar) is, so we can erase it before displaying the new one. Note that they are
		// in screen coordinates, as required by ControlPaint.DrawReversibleLine
		// TODO: We need corresponding fields to support Horizontal/Vertical rulers.
		Point	PreviousCaretVertStart,		// So we can XOR it out
				PreviousCaretVertEnd;
		// We seem to have problems getting innundated with WM_MOUSEMOVE messages even
		// when the mouse is seemingly stationary (and I've tried it on 3 different 
		// mice!). So we get stuff drawn and redrawn constantly. So we'll cache the
		// previous (x,y) mouse coordinates, and if they're the same, ignore the msg.
		uint	Previous_X, Previous_Y;

//---------------------------------------------------------------------------------------

		bool					_IsActive;

		public bool IsActive {
			get { return _IsActive; }
			set { _IsActive = value; }
		}

//---------------------------------------------------------------------------------------

		public RulerControl(Control ParentControl)
			    : this(ParentControl, new RulerOptions()) {
			this.ParentControl = ParentControl;
		}

//---------------------------------------------------------------------------------------

		public RulerControl(Control ParentControl, RulerOptions opts) {
			this.ParentControl = ParentControl;
            this.Parent = ParentControl.Parent;
			this.opts = opts;

			this.DoubleBuffered = true;

			this.Location = new Point(ParentControl.Left, ParentControl.Top - 40);
			this.Size = new Size(ParentControl.Width, 40);
			PreviousCaretVertStart = new Point(-1, -1);
			PreviousCaretVertEnd = new Point(-1, -1);

			this.Paint += new PaintEventHandler(RulerControl_Paint);
			Application.AddMessageFilter(new RulerMonitorMessageFilter(ParentControl, this));
		}

//---------------------------------------------------------------------------------------

		public virtual void xxxxx_Dispose() {
			// TODO: Fill this in. And get it working
		}

//---------------------------------------------------------------------------------------

		~RulerControl() {
			// TODO: Fill this in
		}

//---------------------------------------------------------------------------------------

		protected /*override*/ void xxxx_OnPaint(PaintEventArgs e) {
			// Unused. Would be called before RulerControl_Paint by base.OnPaint(e)
			// Console.WriteLine("In OnPaint");
			// base.OnPaint(e);
		}

//---------------------------------------------------------------------------------------

		void RulerControl_Paint(object sender, PaintEventArgs e) {
			var g = e.Graphics;
			// TODO: Add bool for border, Color for backcolor, bordercolor
			Pen p = new Pen(Color.PowderBlue, 3.0f);
			g.DrawRectangle(p, this.ClientRectangle);
			this.BackColor = Color.Cyan;
			// g.FillRectangle(Brushes.HotPink, this.ClientRectangle);
			DrawRuler(g);
		}

//---------------------------------------------------------------------------------------

		public void DrawCaret(uint x, uint y) {
			// TODO: At the moment, we're supporting only horizontal rulers, so the
			//		 caret is vertical. 
			// TODO: This assumes that the ruler is directly above/beside the parent
			//		 control, so that x,y coordinates correspond.
			if ((x == Previous_X) && (y == Previous_Y))	// See comments on Previous_*
				return;
			Previous_X = x;
			Previous_Y = y;

			// Erase previous caret
			ControlPaint.DrawReversibleLine(PreviousCaretVertStart, PreviousCaretVertEnd,
				this.BackColor);
			// Note: x and y are relative to the ParentControl, but DrawReversibleLine
			//		 needs them in screen coordinates.
			PreviousCaretVertStart = this.PointToScreen(new Point((int)x, this.Height));
			PreviousCaretVertEnd   = this.PointToScreen(new Point((int)x, this.Height / 4));
			ControlPaint.DrawReversibleLine(PreviousCaretVertStart, PreviousCaretVertEnd,
				this.BackColor);
		}

//---------------------------------------------------------------------------------------

		private void DrawRuler(Graphics g) {
			// TODO: The following code assumes that we're doing integral inches.
			// Note: Most calcs done in floating point to minimize roundoff effects.
			// Note: Unlike some of my other code, comments follow the statements.
			float UnitWidth = (float)this.Width / this.opts.RulerWidth;
			// If our control is, say, 200 pixels wide, and our RulerWidth is 10, then
			// an inch occurs every 20 pixels. Our "Unit", as in "UnitWidth" is 20
			// pixels per inch.
			float TickWidth = UnitWidth / opts.TickFactor;
			// If TickFactor is, say, 8, we want 8 tick marks per inch. So we'd have
			// then every 20/8 = 2.5 pixels. Kinda small, I know, but 200 pixels isn't
			// much to work with.
			int nTicks = (int)(this.Width / TickWidth);
			// How many ticks we'll draw
			for (int n = 0; n <= nTicks; n++) {
				Pen pen = new Pen(Color.Brown);
				Point p1, p2;
				p1 = new Point((int)(n * TickWidth), this.Height); // Bottom
				if ((n % opts.TickFactor) == 0) {	// Every "inch"
					p2 = new Point((int)(n * TickWidth), this.Height / 4);	// Top
				} else {
					p2 = new Point((int)(n * TickWidth), this.Height / 2);
				}
				g.DrawLine(pen, p1, p2);

				int nVals = (int)(this.Width / UnitWidth);
				// Number of "inches" to report on
				for (int i = 0; i <= nVals; i++) {
					g.DrawString((opts.RulerLeft + i).ToString(),
						System.Drawing.SystemFonts.DefaultFont,
						System.Drawing.SystemBrushes.WindowText,
						UnitWidth * i, 0.0f);
				}
			}
		}

//---------------------------------------------------------------------------------------

        protected override void WndProc(ref Message m) {
            RulerMonitorMessageFilter.DumpWindowsMessage(m, this.Handle);
            if (m.Msg == (int)WM_Messages.WM_HSCROLL) {
                Console.WriteLine("******** WNDPROC FOUND WM_HSCROLL ********");
            }
            base.WndProc(ref m);
        }
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public enum WM_Messages {
		// Note: Fill in others from LRSS\WindowsMessagesEnums.cs
		WM_CREATE					= 0x0001,   // 1 
		WM_DESTROY					= 0x0002,   // 2 
		WM_MOVE						= 0x0003,   // 3 
		WM_SIZE						= 0x0005,   // 5 
		WM_GETTEXT					= 0x000D,   // 13 
		WM_GETTEXTLENGTH			= 0x000E,   // 14 
		WM_PAINT					= 0x000F,   // 15 
		WM_ERASEBKGND				= 0x0014,   // 20 
		WM_SETCURSOR				= 0x0020,   // 32 
		WM_NCCREATE					= 0x0081,   // 129 
		WM_NCCALCSIZE				= 0x0083,   // 131 
		WM_NCHITTEST				= 0x0084,   // 132 
		WM_NCPAINT					= 0x0085,   // 133 
		WM_NCMOUSEMOVE				= 0x00A0,   // 160 
		WM_NCLBUTTONDOWN			= 0x00A1,   // 161 
		WM_TIMER					= 0x0113,   // 275 
		WM_MOUSEMOVE                = 0x0200,   // 512
        WM_MOUSEHOVER               = 0x02A1,   // 673
		WM_NCMOUSELEAVE				= 0x02A2,   // 674 
        WM_MOUSELEAVE               = 0x02A3,   // 675
        WM_HSCROLL                  = 0x0114,   // 276 
    }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	internal class RulerMonitorMessageFilter : IMessageFilter {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        Control         ParentControl;
		RulerControl	rc;

		IntPtr			hWndParent;

//---------------------------------------------------------------------------------------

		public RulerMonitorMessageFilter(Control ParentControl, RulerControl rc) {
			this.ParentControl = ParentControl;
			this.rc = rc;

			hWndParent = ParentControl.Handle;
		}

		#region IMessageFilter Members

//---------------------------------------------------------------------------------------

		public bool PreFilterMessage(ref Message m) {
            DumpWindowsMessage(m, rc.Handle);
			if (m.HWnd != hWndParent)
				return false;
            switch (m.Msg) {
            case (int)WM_Messages.WM_MOUSEMOVE:
                m = HandleMouseMove(m);
                break;
#if false
            case (int)WM_Messages.WM_HSCROLL:
                // m = HandleHScroll(m);
                break;
#endif
            default:
                break;
			}
			return false;
#if false	// Code from LRSS
			// Check for either the mouse moving or a key being depressed (poor, sad
			// little key...), or the user scrolling with the mouse wheel. Note that
			// we do *not* check for a button being pressed (or ironed). We've gone
			// from two buttons to (currently) five buttons, and who knows how many
			// more in the future. So rather than checking for a click, we'll just
			// assume that any click is (realistically) probably preceded immediately
			// by a mouse move.
			if ((m.Msg == (int)WM_Messages.WM_MOUSEMOVE)
				|| (m.Msg == (int)WM_Messages.WM_MOUSEWHEEL)
				|| (m.Msg == (int)WM_Messages.WM_KEYDOWN)) {
#if false	// We're pretty sure things are working OK, so comment out this code.
					// TODO: The following code is mostly for MOUSEMOVE. I haven't
					//		 checked to see if LParam holds for the others (especially
					//		 KEYDOWN!). But hey, this is only preliminary debug code to
					//		 make sure we're intercepting things properly.
					uint	x, y;
					x = (uint)m.LParam / 0xffffu;
					y = (uint)m.LParam & 0xffffu;
					string s = string.Format("{4}: Msg = {3} -- Mouse at ({0},{1}) - hWnd = {2:X8}", 
						x, y, (int)m.HWnd, (WM_Messages)m.Msg, LastUserInteraction);
					Debug.WriteLine(s);
#endif
			}
			return false;		// Let other users see this message
		}
#endif
		}

//---------------------------------------------------------------------------------------

        public static void DumpWindowsMessage(Message m, IntPtr ThisWindow) {
            if (m.HWnd == ThisWindow)       // Don't recurse if we're debugging
                return;                 
            string s = string.Format("hWnd={0:X8}, Msg={1}/{2:X4}, WParam={3:X8}, LParam={4:X8}",
                (int)m.HWnd, (WM_Messages)m.Msg, m.Msg, (int)m.WParam, (int)m.LParam);
            int length = GetWindowTextLength(m.HWnd);
            if (length > 0) {
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(m.HWnd, sb, sb.Capacity);
                s = string.Format("{0}, Title={1}", s, sb.ToString());
            }
            Console.WriteLine(s);
        }

//---------------------------------------------------------------------------------------

        private Message HandleMouseMove(Message m) {
            uint x, y;
            x = (uint)m.LParam & 0xffffu;
            y = ((uint)(m.LParam) & 0xffff0000u) >> 16;
#if false
				string s = string.Format("Msg = {3} -- Mouse at ({0},{1}) - hWnd = {2:X8}",
					x, y, (int)m.HWnd, (WM_Messages)m.Msg);
				//Console.WriteLine(s);
				//rc.Invalidate(new Rectangle(0, 0, 1, 1));
#endif
            // TODO: Check to see if these are in the Client Area
            rc.DrawCaret(x, y);
            return m;
        }
		#endregion
	}
}
