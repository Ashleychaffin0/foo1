using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace AlphaBlend1 {
	public partial class AlphaBlend1 : Form {
		string FromImageName	= @"S:\LRS-S-Drive\CAS\03-11-02 Rhinecliff 009.jpg";	// In house
		string ToImageName		= @"S:\LRS-S-Drive\CAS\03-11-02 Rhinecliff 001.jpg";	// Purple

		Image	FromImage, ToImage;

		int		ticks;
		int		Duration;		// In tick counts

		Bitmap			Blended;
		ColorMatrix		cm;
		ImageAttributes	ia;

//---------------------------------------------------------------------------------------
		
		public AlphaBlend1() {
			InitializeComponent();

			Duration = 30;
			timer1.Interval = 25;
		
			cm = new ColorMatrix();
			ia = new ImageAttributes();

			try {
				SetImages();
				Blended = (Bitmap)ToImage;
			} catch (Exception) {
				
				throw;
			}
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			if (--ticks == 0) {
				timer1.Enabled = false;
				// MessageBox.Show("Done", "Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Bitmap		Blended = (Bitmap)FromImage.Clone();
			Graphics	g = Graphics.FromImage(Blended);
			// g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

			float		AlphaFrom = ((100f / Duration) * ticks) / 100f;
			cm.Matrix33 = AlphaFrom;			
			ia.SetColorMatrix(cm);

			Rectangle		rect = new Rectangle(0, 0, Blended.Width, Blended.Height);
			g.DrawImage(ToImage, rect, 0, 0, FromImage.Width, FromImage.Height, GraphicsUnit.Pixel, ia);

			// Console.WriteLine("\nComparing bitmaps - ticks = {0}, AlphaFrom = {1}", ticks, AlphaFrom);
			// CompareBitmaps(Blended, b2);

			pic1.Image = Blended;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			pic1.Image = FromImage;
			ticks = Duration;
			timer1.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		private void SetImages() {
			FromImage = Image.FromFile(FromImageName);
			ToImage = Image.FromFile(ToImageName);

			FromImage = new Bitmap(FromImage, pic1.Size);
			ToImage = new Bitmap(ToImage, pic1.Size);
		}

//---------------------------------------------------------------------------------------

		private void AlphaBlend1_ResizeEnd(object sender, EventArgs e) {
			SetImages();
		}

//---------------------------------------------------------------------------------------

		private bool CompareBitmaps(Bitmap b1, Bitmap b2) {
			int		MaxDiffs = 10;		// TODO: Arbitrary. Make parm?
			if ((b1.Width != b2.Width) || (b1.Height != b1.Height)) {
				Console.WriteLine("CompareBitMaps - Dimensions different");
				// TODO: Give dimensions in msg
				return false;
			}
			Color	c1, c2;
			for (int row = 0; row < b1.Height; row++) {
				for (int col = 0; col < b1.Width; col++) {
					c1 = b1.GetPixel(col, row);
					c2 = b2.GetPixel(col, row);
					if (c1 != c2) {
						if (--MaxDiffs == 0) {
							return false;
						}
						Console.WriteLine("\tCompareBitMaps[{0}, {1}] - {2}\t{3}", row, col, c1, c2);
					}
				}
			}
			return true;
		}
	}
}