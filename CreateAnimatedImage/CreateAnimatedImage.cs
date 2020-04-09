using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

// See https://stackoverflow.com/questions/1196322/how-to-create-an-animated-gif-in-net


namespace CreateAnimatedImage {
	public partial class CreateAnimatedImage : Form {
		public CreateAnimatedImage() {
			InitializeComponent();

			var Circles = GenerateCircles(200, 200, Color.Blue);


			CreateGifFile(Circles, @"c:\lrs\MyGif.gif"); ;
		}

//-----------------------------------------------------------------------------

		private static void CreateGifFile(IEnumerable<Bitmap> frames, string filename) {
			// The following code is semi-magic. I wouldn't change it if I were you...
			var gEnc = new GifBitmapEncoder();
			foreach (Bitmap bmpImage in frames) {
				var bmp = bmpImage.GetHbitmap();
				var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					bmp,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());
				gEnc.Frames.Add(BitmapFrame.Create(src));
			}
			using (FileStream fs = new FileStream(filename, FileMode.Create)) {
				gEnc.Save(fs);
			}
		}

//-----------------------------------------------------------------------------

		// An animated GIF consists of a series of individual images (frames)
		// that are displayed in rapid succession. Replace this routine with
		// your own that generates the frames you want.

		// This method repeatedly calls the DrawCircle method, increasing the
		// radius of the circle drawn for each frame. So when the GIF is
		// displayed, it will show an expanding (filled in) circle.
		private IEnumerable<Bitmap> GenerateCircles(int width, int height, Color c) {
			for (int r = 1; r < width / 2; r += 5) {
				yield return DrawCircle(width, height, r, c);
			}
		}

//-----------------------------------------------------------------------------

		private static Bitmap DrawCircle(int w, int h, int r, Color c) {
			// Not a perfect circle drawing algorithm. Check out
			// https://en.wikipedia.org/wiki/Midpoint_circle_algorithm
			var bmp = new Bitmap(w, h);

			int r2 = r * r;
			int ox = w/2, oy = w/2; // origin

			for (int x = -r; x < r; x++) {
				int height = (int)Math.Sqrt(r2 - x * x);

				for (int y = -height; y < height; y++)
					bmp.SetPixel(x + ox, y + oy, c);
			}
			return bmp;
		}
	}
}
