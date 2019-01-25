using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LRSJAS_Zigzag_1 {
	
	public partial class Box : UserControl {

		Pen DefaultBorderPen;
		Pen BorderPen;					// Current border pen

		int FocusedBorderWidth = 3;
		Size BorderWidth;				// Depends on BorderStyle

		Point DragStartLocation;

		LRSJAS_Zigzag1 BoxParent;

		public BoxParms parms;

//---------------------------------------------------------------------------------------

		// These attributes tie into the Property Grid
		[Category("Appearance")]
		[Description("The text contents of the box")]
		public string Contents {
			get { return this.Text; }
			set {
				this.Text = value;
				Invalidate();
			}
		}

//---------------------------------------------------------------------------------------

		public Box() {
			// Parameter-less ctor needed for Serialization
		}

//---------------------------------------------------------------------------------------

		// "Private", since this isn't called with a BoxParms.
		private Box(LRSJAS_Zigzag1 Parent) {
			InitializeComponent();

			BoxParent = Parent;

			this.BorderStyle = BorderStyle.Fixed3D;
			BorderWidth = SystemInformation.Border3DSize;

			this.BackColor = Color.Yellow;
			DefaultBorderPen = Pens.Black;
			BorderPen = DefaultBorderPen;

			// this.TextAlign = 

			// Here's how you'd add your own event handler. But the parent class has
			// already done it for you.
			// this.KeyDown += new KeyEventHandler(Box_KeyDown);
		}

//---------------------------------------------------------------------------------------

		// TODO: This ctor is deprecated
		[Obsolete]
		public Box(LRSJAS_Zigzag1 Parent, Point Location, Size Size, string Text)
			: this(Parent) {
			throw new NotImplementedException("This Box ctor is deprecated");
#if false
			this.Location = Location;
			this.Size = Size;
			this.Contents = Text;
#endif
		}

//---------------------------------------------------------------------------------------

		public Box(LRSJAS_Zigzag1 Parent, BoxParms parms) : this(Parent) {
			this.Location  = parms.Location;
			this.Size      = parms.size;
			this.Contents  = parms.Text;
			this.BackColor = parms.BackColor;
			this.ForeColor = parms.ForeColor;

			this.parms = parms;
			
			parms.RelatedBox = this;
		}

//---------------------------------------------------------------------------------------

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			this.Capture = false;
			if (e.Button == MouseButtons.Left) {
				this.Capture = true;
				DragStartLocation = e.Location;
			}
		}

//---------------------------------------------------------------------------------------

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (!this.Capture)
				return;
			Point ofs = e.Location;
			ofs.Offset(-DragStartLocation.X, -DragStartLocation.Y);
			Point newLoc = this.Location;
			newLoc.Offset(ofs);
			this.Location = newLoc;
			// BoxParent.RefreshPropertyGrid();		// TODO: Causes problems
			BoxParent.InvalidateMainPane();
		}

//---------------------------------------------------------------------------------------

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if (e.Button == MouseButtons.Left) {
				this.Capture = false;
			}
		}

//---------------------------------------------------------------------------------------

		protected override bool IsInputKey(Keys keyData) {
			// Otherwise, these keys will used, dialog-like, to switch between
			// controls
			if (keyData == Keys.Left || keyData == Keys.Right
				|| keyData == Keys.Up || keyData == Keys.Down) {
				return true;
			}
			return base.IsInputKey(keyData);
		}

//---------------------------------------------------------------------------------------

		protected override void OnEnter(EventArgs e) {
			base.OnEnter(e);
			BoxParent.GetPropGrid().SelectedObject = this;
			BorderPen = Pens.Red;
			Invalidate();
		}

//---------------------------------------------------------------------------------------

		protected override void OnLeave(EventArgs e) {
			base.OnLeave(e);
			BorderPen = DefaultBorderPen;
			Invalidate();
		}

//---------------------------------------------------------------------------------------

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			switch (e.KeyCode) {
			// Note: Many, many Keys.* deleted from <switch> snippet completion
			//		 of the Keys enumeration
			// Note: To get all of them, type in switch<Tab><Tab>e.KeyCode<Enter>
			case Keys.C:
			case Keys.Space:
				FlipBackground();
				break;
			case Keys.D:
			case Keys.Down:
				MoveLeftRightUpDown(0, 50);
				break;
			case Keys.L:
			case Keys.Left:
				MoveLeftRightUpDown(-50);
				break;
			case Keys.R:
			case Keys.Right:
				MoveLeftRightUpDown(50);
				break;
			case Keys.U:
			case Keys.Up:
				MoveLeftRightUpDown(0, -50);
				break;
			default:
				break;
			};
		}

//---------------------------------------------------------------------------------------

		private void MoveLeftRightUpDown(int left, int right = 0) {
			Point pt = this.Location;
			pt.Offset(left, right);
			this.Location = pt;
			this.Parent.Invalidate();
		}

//---------------------------------------------------------------------------------------

		private void FlipBackground() {
			if (this.BackColor == Color.Yellow) {
				this.BackColor = Color.Cyan;
			} else {
				this.BackColor = Color.Yellow;
			}
			this.Invalidate();
		}

//---------------------------------------------------------------------------------------

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			Graphics g = e.Graphics;
			Rectangle rect = DrawBorder(g);

			Font font = new Font("Arial", 12);
			var fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Center;		// Horizontal
			fmt.LineAlignment = StringAlignment.Center;		// Vertical
			g.DrawString(this.Text, font, Brushes.Blue, this.ClientRectangle, fmt);
#if false		// CenteredTextOffset now obsolete
			// var offset = CenteredTextOffset(g, rect, font, this.Text);
			// g.DrawString(this.Text, font, Brushes.Blue, offset);
#endif
		}

//---------------------------------------------------------------------------------------

		private Rectangle DrawBorder(Graphics g) {
			// Could also maybe define a pen with a width of FocusedBorderWidth.
			// Might alternatively use ControlPaint.DrawBorder.
			Rectangle rect = this.ClientRectangle;
			int n = this.Focused ? FocusedBorderWidth : 1;
			for (int i = 0; i < n; i++) {
				rect.Inflate(-1, -1);
				g.DrawRectangle(BorderPen, rect);	// Border
			}
			return rect;
		}

//---------------------------------------------------------------------------------------

#if false		// CenteredTextOffset now obsolete
		PointF CenteredTextOffset(Graphics g, Rectangle rect, Font font, string s) {
			// TODO: I don't think this is taking into account the BorderStyle
			SizeF size = g.MeasureString(s, font);
			var ofs_x  = FocusedBorderWidth + rect.X + (rect.Width - size.Width) / 2;
			// var ofs_x = FocusedBorderWidth + rect.X + BorderWidth.Width + (rect.Width - size.Width - 2 * BorderWidth.Width) / 2;
			ofs_x = Math.Max(ofs_x, 0f) + rect.X;
			var ofs_y  = (rect.Height - size.Height) / 2;
			ofs_y      = Math.Max(ofs_y, 0f) + rect.Y;
			return new PointF(ofs_x, ofs_y);
		}
#endif

//---------------------------------------------------------------------------------------

		public PointF ConnectorCoords(Sides side, float pct) {
			float x, y;
			float h, w;
			var rect = this.ClientRectangle;
			rect.Offset(this.Location);
			h = this.ClientRectangle.Height;
			w = this.ClientRectangle.Width;
			switch (side) {
			case Sides.Top:
				x = rect.X + w * pct;
				y = rect.Top;
				break;
			case Sides.Bottom:
				x = rect.X + w * pct;
				y = rect.Y + h;
				break;
			case Sides.Left:
				x = rect.Left;
				y = rect.Y + h * pct;
				break;
			case Sides.Right:
				x = rect.Left + w;
				y = rect.Y + h * pct;
				break;
			default:
				x = y = 0;
				break;
			}
			return new PointF(x, y);
		}

//---------------------------------------------------------------------------------------

		public enum Sides {
			Top,
			Bottom,
			Left,
			Right
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// All the info we need to (re-)create a Box control.
	// TODO: The BackColor and ForeColor aren't being serialized. I've seen this before,
	//		 but I don't remember how I solved this. For now, let's just represent
	//		 Back- and Fore-Color as an int via BackColor.ToArgb().
	public class BoxParms {

		[XmlIgnore]
		public			Box RelatedBox;

		public Guid		guid;

		public string	Text;
		public Point	Location;
		public Size		size;
		public Color	BackColor;
		public Color	ForeColor;

		// Keep in sync with methods below

		// Candidates for inclusion here include, but aren't limited to...
		// public int	TabIndex;
		// public Control Parent;

//---------------------------------------------------------------------------------------

		public BoxParms() {
			// Empty ctor for serialization
		}

//---------------------------------------------------------------------------------------

		public BoxParms(string Text, Point Location, Size size, Color BackColor, Color ForeColor) {
			guid		   = Guid.NewGuid();

			this.Text      = Text;
			this.Location  = Location;
			this.size      = size;
			this.BackColor = BackColor;
			this.ForeColor = ForeColor;
		}

//---------------------------------------------------------------------------------------

		// Copy real box values back into here before, e.g., serializing.
		// Returns an indication if the relevant Box parameters have changed.
		// TODO: This routine needs a better, more descriptive name.
		public bool SaveBoxValues() {
			bool IsDirty = false;
			IsDirty |= this.Text	  != RelatedBox.Text;
			IsDirty |= this.Location  != RelatedBox.Location;
			IsDirty |= this.size	  != RelatedBox.Size;
			IsDirty |= this.BackColor != RelatedBox.BackColor;
			IsDirty |= this.ForeColor != RelatedBox.ForeColor;

			this.Text      = RelatedBox.Text;
			this.Location  = RelatedBox.Location;
			this.size      = RelatedBox.Size;
			this.BackColor = RelatedBox.BackColor;
			this.ForeColor = RelatedBox.ForeColor;

			return IsDirty;
		}
	}
}
