// Copyright (c) 2007 Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

//---------------------------------------------------------------------------------------

namespace Print90 {
	public partial class Print90 : Form {

		float	LeftMargin		= 50f;			// 1/2"
		float	TopMargin		= 62.5f;		// 5/8"

		float	BadgeWidth		= 375f;			// Should be 4" ??? TODO:
		float	BadgeHeight		= 275f;			// 2 3/4"	Should be 3" ???

		float	CouponHeight	= 150f;
		float	CouponWidth;					// Set to BadgeWidth

		float	AgendaHeight;					// Set to BadgeHeight
		float	AgendaWidth;					// Set to BadgeWidth
		
		float	DocWidth;
		float	DocHeight;

		public Print90() {
			InitializeComponent();

			CouponWidth  = BadgeWidth;
			AgendaHeight = BadgeHeight;
			AgendaWidth  = BadgeWidth;

			DocHeight	 = AgendaWidth * 2;
			DocWidth	 = BadgeWidth + 2 * AgendaHeight;

			pd.DocumentName = "Print90";
			pd.DefaultPageSettings.Landscape = true;
			pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
			pd.BeginPrint += new PrintEventHandler(pd_BeginPrint);
			pd.QueryPageSettings += new QueryPageSettingsEventHandler(pd_QueryPageSettings); 
			printPreviewDialog1.Document = pd;
		}

//---------------------------------------------------------------------------------------

		void pd_QueryPageSettings(object sender, QueryPageSettingsEventArgs e) {
			// throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		void pd_BeginPrint(object sender, PrintEventArgs e) {
			// throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		void pd_PrintPage(object sender, PrintPageEventArgs e) {
#if false
			Graphics	g = e.Graphics;
			float		LastY  = 11f * 100f;
			float		LastX = 8.5f * 100f;
			string		msg;

			Font		font = new Font("Arial", 12.0f);
			Brush		brush = Brushes.Black;
#endif

			DrawAll(e);
			e.HasMorePages = false;		// The default, but let's state it explicitly
			
#if false
			// Draw the rows
			using (Pen redPen = new Pen(Color.Red)) {
				for (float Y = 0f; Y < LastY; Y += 100) {
					g.DrawLine(redPen, 0f, Y, LastX, Y);
					msg = string.Format("Row-({0:F2},{1:F2})",  0f, Y);
					// g.DrawString(msg, font, brush,  0f, Y);
				}
			}
			
			// Draw the Columns
			using (Pen bluePen = new Pen(Color.Blue)) {
				for (float X = 0f; X < LastX; X += 100) {
					g.DrawLine(bluePen, X, 0f, X, LastY);
				}
			}
#endif

#if false
			// Draw the coords
			using (Pen bluePen = new Pen(Color.Blue)) {
				for (float X = 0f; X < LastX; X += 100) {
					if (Math.Floor(X / 100) == 3) {
						// for (int degree = 0; degree >= -90; degree -= 15) {
						for (int degree = -90; degree >= -90; degree -= 15) {
							float	Y = 750f;
							PointF	pt = new PointF(X, Y);
							m = new Matrix();
							m.RotateAt(degree, pt);
							g.Transform = m;
							msg = string.Format("Rotate({0})-({1:F2},{2:F2})", degree, X, Y);
							g.DrawString(msg, font, brush, X, Y);
							g.DrawString("Hello World", font, brush, X, Y + 20);
						}
					}
				}
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void DrawAll(PrintPageEventArgs e) {
			Reset(e.Graphics);
			// Good. Now we can ignore the margins.

			DrawPageGrid(e);

			DrawMainBadge(e);

			for (int n = 0; n < 3; n++) {
				DrawCoupon(e, n);
			}

			for (int n = 0; n < 3; n++) {
				DrawAgenda(e, n);
			}
		}

//---------------------------------------------------------------------------------------

		private void DrawPageGrid(PrintPageEventArgs e) {
			Graphics	g = e.Graphics;
			g.DrawLine(Pens.Black, 0, 0, DocWidth, 0);		// Top
			g.DrawLine(Pens.Black, 0, DocHeight, DocWidth, DocHeight);	// Bottom
			g.DrawLine(Pens.Black, 0, 0, 0, DocHeight);		// Left
			g.DrawLine(Pens.Black, DocWidth, 0, DocWidth, DocHeight);	// Right

			// TODO: The badge/coupons/agendas should draw their own boxes
			g.DrawLine(Pens.Black, BadgeWidth, 0, BadgeWidth, DocHeight);

			// Coupon dividing lines
			for (int i = 0; i < 4; i++) {
				float	Y = BadgeHeight + CouponHeight * i;
				g.DrawLine(Pens.Black, 0, Y, BadgeWidth, Y);
			}

			// Agenda dividing lines
			g.DrawLine(Pens.Black, BadgeWidth, BadgeWidth, 
				DocWidth, BadgeWidth);		// Horizontal
			float	X = BadgeWidth + BadgeHeight;
			g.DrawLine(Pens.Black, X, 0, X, DocHeight);	// Vertical
		}

//---------------------------------------------------------------------------------------

		private void DrawMainBadge(PrintPageEventArgs e) {
			Graphics	g = e.Graphics;
			// TODO: This should really draw its own box
			Font	Large = new Font("Arial", 36, FontStyle.Bold);
			Font	Medium = new Font("Arial", 30, FontStyle.Bold);
			Font	Small  = new Font("Arial", 18);

			float	offset = 10f;			// Offset from top
			g.TranslateTransform(0f, offset);	// Reset below.

			StringFormat	fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Center;
			RectangleF		rect = new RectangleF(0, 10, BadgeWidth, Large.Height);

			// First name
			g.DrawString("Robert", Large, Brushes.Black, rect, fmt);

			// Last name
			rect.Offset(0, Large.Height + 10);
			g.DrawString("Browning", Medium, Brushes.Black, rect, fmt);

			// Company
			rect.Offset(0, Medium.Height);
			g.DrawString("The Dow Chemical Company", Small, Brushes.Black, rect, fmt);

			Reset(g);
			// g.TranslateTransform(0f, -offset);	// Reset offset

		}

//---------------------------------------------------------------------------------------

		private void Reset(Graphics g) {
			//	g.Transform.Reset();		// Doesn't work! Elements[4, 5] not reset
			g.Transform = new Matrix();
			g.TranslateTransform(LeftMargin, TopMargin);
		}

//---------------------------------------------------------------------------------------

		private void DrawCoupon(PrintPageEventArgs e, int n) {
			// TODO: This should really draw its own box
			Graphics	g = e.Graphics; 
			float offset = BadgeHeight + (CouponHeight * n) + 10;
			g.TranslateTransform(0f, offset);	// Offset from top. Reset below.

			Font dayFont = new Font("Arial", 16);
			Font AdmitFont = new Font("Arial", 18, FontStyle.Bold);

			StringFormat fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Center;

			float	LogoWidth = 160;
			string	dayText = "";
			RectangleF rect = new RectangleF(160, 0, BadgeWidth - LogoWidth, dayFont.Height);
			dayText = "Coupon " + (n + 1);
			g.DrawString(dayText, dayFont, Brushes.Black, rect, fmt);

			rect.Offset(0, dayFont.Height);
			g.DrawString("ADMIT ONE", AdmitFont, Brushes.Black, rect, fmt);

			Reset(g);
			// e.Graphics.TranslateTransform(0f, -offset);	// Reset offset from top.
		}
//---------------------------------------------------------------------------------------

		private void DrawAgenda(PrintPageEventArgs e, int n) {
			// TODO: This should really draw its own box
			Graphics g = e.Graphics; 
			// OK, now comes the fun part.
			// First, set the origin of the sub-page
			float	ofsX = 0f, ofsY = 0;
			string	day = "";
			Brush	brush = Brushes.Red;
			float	fudgeY = 0 * 112.5f;
			switch (n) {
			case 0:
				ofsX = BadgeWidth;
				ofsY = BadgeWidth + fudgeY;
				day  = " March 20, 2007 ";
				brush = Brushes.Black;
				break;
			case 1:
				ofsX = BadgeWidth + BadgeHeight;
				ofsY = BadgeWidth + fudgeY;
				day  = " March 21, 2007 ";
				brush = Brushes.Red;
				break;
			case 2:
				ofsX = BadgeWidth + BadgeHeight;
				ofsY = BadgeWidth * 2 + fudgeY;
				day  = " March 22, 2007 ";
				brush = Brushes.Green;
				break;
			default:			// In this sample program, shouldn't happen
				return;
			}
			// g.TranslateTransform(ofsX, ofsY);		// Reset later		// TODO:

			Font TitleFont  = new Font("Arial", 18, FontStyle.Bold);
			Font AgendaFont = new Font("Arial", 8);

			StringFormat fmt = new StringFormat();
			// fmt.Alignment = StringAlignment.Center;		// TODO: Undo comment

			RectangleF	rect = new RectangleF(0, 0, AgendaWidth, TitleFont.Height);
#if false
			for (int deg = 0; deg <= 360; deg += 10) {
				Matrix	mm = new Matrix();
				mm = g.Transform;		// Pick up margins
				mm.Translate(ofsX, ofsY, MatrixOrder.Append);
				// m.Rotate(-90, MatrixOrder.Append);

				mm.RotateAt(-deg, pt, MatrixOrder.Append);
				// m.Rotate(90, MatrixOrder.Append);
				g.Transform = mm;

				g.DrawRectangle(new Pen(brush), rect.X, rect.Y, rect.Width, rect.Height);
				g.DrawString(n + " - Agenda for LRS - xxx - " + n, TitleFont,
					brush, rect, fmt);
				Reset(g);
			}
#endif

#if true	// Real code - restore it later		TODO:
			Matrix	m = new Matrix();
			m = g.Transform;		// Pick up margins
			m.Translate(ofsX, ofsY, MatrixOrder.Append);
			// m.Rotate(-90, MatrixOrder.Append);

			// PointF pt = new PointF(ofsX + LeftMargin, ofsY + TopMargin);
			PointF	pt = new PointF(m.OffsetX, m.OffsetY);

			m.RotateAt(-90, pt, MatrixOrder.Append);
			// m.Rotate(90, MatrixOrder.Append);
			g.Transform = m;

			g.DrawRectangle(new Pen(brush), rect.X, rect.Y, rect.Width, rect.Height);
			g.DrawString(n + " - Agenda for LRS - xxx - " + n, TitleFont,
				brush, rect, fmt);

			rect.Offset(0, TitleFont.Height);
			g.DrawString(n + day + n, TitleFont,
				brush, rect, fmt);

			Reset(g);
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnPrintPreview_Click(object sender, EventArgs e) {
			printPreviewDialog1.ShowDialog();
		}

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, EventArgs e) {
			pd.Print();
		}
	}
}