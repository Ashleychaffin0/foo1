using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Id3Lib;

// http://www.nuget.org/packages/taglib
using TagLib;			// Install-Package taglib
using TagLib.Asf;

namespace TryID3Lib_1 {
	public partial class TryID3Lib_1 : Form {
		public TryID3Lib_1() {
			InitializeComponent();

			string DirName = @"D:\$ Zune Master\Al Petteway & Amy White\Groovemasters, Vol. 3- Gratitude";
			string fname = Path.Combine(DirName, "1 Pocket Change.wma");

#if false
			var xxx = new Id3Lib.ID3v1();
			using (var stream = File.Open(fname, FileMode.Open)) {
				xxx.Deserialize(stream);
			}
#endif

			var xx = TagLib.File.Create(fname);
			var yy = xx.Tag as TagLib.Asf.Tag;

#if true
			foreach (DescriptionRecord item in yy.MetadataLibraryObject) {
				
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void TryID3Lib_1_Load(object sender, EventArgs e) {
			var xxBrush = new LinearGradientBrush(new PointF(0.494f, 0.028f), new PointF(0.494f, 0.889f),
				Color.FromArgb(0x99, 255, 255, 255),
				Color.FromArgb(0x33, 255, 255, 255));

			var lgBrush = new LinearGradientBrush();
			
			
		}
	}
}
