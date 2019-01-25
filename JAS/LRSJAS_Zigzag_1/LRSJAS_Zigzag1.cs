// #define	TEST_CONTROLPAINT_DRAWREVERSIBLE_LINE
// #define TEST_SHELL32
// #define TEST_IRONPYTHON

// TODO:	(Mostly BoxParms-related)
//	*	Update BoxParms (and Connectors) on Save/SaveAs
//	*	Ditto for FormClosing
//	*	Serialize Connectors
//	*	Class ZigZagData -- Put Connectors back in
//	*	Implement Load

//	*	A real dialog to support Add New Box (with LinearGradientBrush)

// References
//	*	http://blogs.msdn.com/b/charlie/archive/2009/10/25/hosting-ironpython-in-a-c-4-0-program.aspx
//	*	http://community.devexpress.com/blogs/paulk/archive/2010/06/11/using-the-dynamic-language-runtime-to-call-ironpython-for-vs2010.aspx

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

#if TEST_IRONPYTHON
using IronPython.Hosting;
using Microsoft.CSharp;
//using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
#endif

using LRSUtils;

#if TEST_SHELL32
using Shell32;
#endif

namespace LRSJAS_Zigzag_1 {
	public partial class LRSJAS_Zigzag1 : Form {

		// TODO: Currently the .xml file is saved to the same directory as the
		//		 executable. We probably should put it in My Documents instead.
		string ZigZagDataFileName = "ZigZagData.xml";
		ZigZagData			Data;

		// TODO: Will we wind up with these, or just the BoxParms et al?
		List<Box>			Boxes;
		List<BoxConnector>	Connectors;

		// Right now the IsDirty flag is set only on a right-click to (virtually) create
		// a new Box. Later, we must see if things really have changed
		bool IsDirty;

//---------------------------------------------------------------------------------------

		Panel MainPane {
			get { return this.splitContainer1.Panel1; }
		}

//---------------------------------------------------------------------------------------

		public LRSJAS_Zigzag1() {
			InitializeComponent();

			Boxes      = new List<Box>();
			Connectors = new List<BoxConnector>();

#if TEST_SHELL32
			// http://www.onestopqa.com/resources/Working%20with%20Shell32.pdf
			// http://dotnet.dzone.com/articles/automatic-music-categorization
#if false
			System.IO.FileSystemEventArgs foo = null;
			foo.f
#endif
			string watchPath = @"C:\$ Zune Audio\Walter R. Brooks\Freddy and the Popinjay -- Disk 5 of 5";
			var name = "01 Unknown Track 1.wma";
			Shell shell = new Shell();
			Folder folder = shell.NameSpace(watchPath);
			FolderItem folderItem = folder.ParseName(name);
			//Folder3 f3 = shell.NameSpace(watchPath);
			var dict = new Dictionary<int, string>();
			dict.Add(2, "Title");
			dict.Add(3, "Subject");
			dict.Add(4, "Author");
			dict.Add(5, "Keywords");
			dict.Add(6, "Comments");
			dict.Add(7, "Template");
			dict.Add(8, "Last Saved By");
			dict.Add(9, "Revision Number");
			dict.Add(10, "Total Editing Time");
			dict.Add(11, "Last Printed");
			dict.Add(12, "Create Time/Date");
			dict.Add(13, "Last Saved Time/Date / Album Artist");
			dict.Add(14, "Number of Pages / Album");
			dict.Add(15, "Number of Words / Year");
			dict.Add(16, "Number of Characters / Genre");
			dict.Add(17, "Thumbnail / Conductor");
			dict.Add(18, "Name of Creating Application");
			dict.Add(19, "Security / Rating");
			dict.Add(20, "Artist");
			dict.Add(21, "Song Name");
			dict.Add(24, "Comment");
			dict.Add(26, "Track Number");
			dict.Add(27, "Length");
			dict.Add(28, "Bitrate");
			dict.Add(29, "Protected");
			var artist = folder.GetDetailsOf(folderItem, 13);
			var album  = folder.GetDetailsOf(folderItem, 14);
			Console.WriteLine("".PadRight(80, '-'));
			for (int i = 0; i < 100; i++) {
				try {
					var x = folder.GetDetailsOf(folderItem, i);
					if (x != "") {
						string val;
						bool bOK = dict.TryGetValue(i, out val);
						if (bOK) {
							Console.WriteLine("{0,3}: {1} -- {2}", i, x, val);
						} else {
							Console.WriteLine("{0,3}: {1}", i, x);
						}
					}
				} catch {
					// Ignore
				}
			}
#endif

#if TEST_IRONPYTHON
			var ipy = Python.CreateRuntime();
			dynamic test = ipy.UseFile("Test.py"); 
			test.Simple();
			var sfs = test.GetUri("http://www.sanfransys.com");
#endif
		}

//---------------------------------------------------------------------------------------
		
		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Several parts of this routine will go away once we implement
			//		 File | Load (which should be called File | Open (TODO:).


			// Remove any existing boxes
			foreach (var box in Boxes) {
				MainPane.Controls.Remove(box);
			}
			Boxes.Clear();

			EraseConnectors();
			Connectors.Clear();

			// Add boxes
			AddBoxes();

			// Add them back in to the Form (window)
			foreach (var box in Boxes) {
				MainPane.Controls.Add(box);
			}

			AddConnectors();

			// TODO: Assumes there's at least one box
			Boxes[0].Focus();

			// TODO: This isn't very elegant. In fact it's downright kludgy. Do this
			//		 better at some point.
			List<BoxParms> parms = new List<BoxParms>();
			foreach (var box in Boxes) {
				parms.Add(box.parms);
			}
			Data = new ZigZagData(parms, Connectors);

			this.Invalidate();		// Draw connectors
		}

//---------------------------------------------------------------------------------------

		private void AddConnectors() {
			Connectors.Add(new BoxConnector(
				Boxes[0], Box.Sides.Right, BoxConnector.Middle,
				Boxes[1], Box.Sides.Left, BoxConnector.Middle));

			Connectors.Add(new BoxConnector(
				Boxes[1], Box.Sides.Bottom, BoxConnector.Middle,
				Boxes[2], Box.Sides.Top, BoxConnector.Middle));

		}

//---------------------------------------------------------------------------------------

		private void AddBoxes() {
			var boxParms1 = new BoxParms("Hello world 1", new Point(150, 110), new Size(130, 60), Color.Brown, Color.Black);
			Box box1 = new Box(this, boxParms1);
			Boxes.Add(box1);

			var boxParms2 = new BoxParms("Hello world 2", new Point(320, 210), new Size(160, 60), Color.Green, Color.Black);
			Box box2 = new Box(this, boxParms2);
			Boxes.Add(box2);

			var boxParms3 = new BoxParms("Hello world 3", new Point(50, 310), new Size(390, 260), Color.Cyan, Color.Black);
			Box box3 = new Box(this, boxParms3);
			Boxes.Add(box3);
#if false
			Boxes.Add(new Box(this, new Point(150, 110), new Size(130, 60), "Hello World 1"));
			Boxes.Add(new Box(this, new Point(320, 210), new Size(160, 60), "Hello World 2"));
			Boxes.Add(new Box(this, new Point(50, 310), new Size(390, 260), "Hello World 3"));
#endif
		}

//---------------------------------------------------------------------------------------

		internal void EraseConnectors() {
			// We can approach this is several ways.
			//	* We could find all the connectors and XOR them or something.
			//	* Or we can just do a blanket fill of the window with the current
			//	  brush and assume that fill's are fast. So we'll do that.
			//	* But note that if we just fill the entire form, then we'll wind up
			//	  erasing *all* the connectors, when what we'd prefer is to just erase
			//	  the one that needs to be erased (i.e. the ones from/to the Box that
			//	  just moved. Ah well, this is good enough for the demo, even if there
			//	  is a bit of flickering.
			using (Graphics g = Graphics.FromHwnd(MainPane.Handle)) {
				// TODO: Note that this implies/requires a solid background.
				var bg = new SolidBrush(this.BackColor);
				g.FillRectangle(bg, this.ClientRectangle);
			}
		}

//---------------------------------------------------------------------------------------

		private void LRSJAS_Zigzag1_Paint(object sender, PaintEventArgs e) {
#if TEST_CONTROLPAINT_DRAWREVERSIBLE_LINE
			DrawConnectors();
#else
			EraseConnectors();
			DrawConnectors();
#endif
		}

//---------------------------------------------------------------------------------------

		internal void DrawConnectors() {
			Pen pen           = new Pen(Color.RoyalBlue, 8);
			pen.StartCap      = LineCap.RoundAnchor;
			pen.EndCap        = LineCap.ArrowAnchor;
			// pen.DashStyle     = DashStyle.Dash;
			// using (Graphics g = Graphics.FromHwnd(this.Handle)) {
			using (Graphics g = Graphics.FromHwnd(MainPane.Handle)) {
				foreach (var con in Connectors) {
					PointF pt1 = con.FromBox.ConnectorCoords(con.FromSide, con.FromPct);
					PointF pt2 = con.ToBox.ConnectorCoords(con.ToSide, con.ToPct);
#if TEST_CONTROLPAINT_DRAWREVERSIBLE_LINE
					Point pta = new Point((int)pt1.X, (int)pt1.Y);
					Point ptb = new Point((int)pt2.X, (int)pt2.Y);
					pta = this.PointToScreen(pta);
					ptb = this.PointToScreen(ptb);
					ControlPaint.DrawReversibleLine(pta, ptb, this.BackColor);
#else
					g.DrawLine(pen, pt1, pt2);
#endif
				}
			}
		}

//---------------------------------------------------------------------------------------

		internal PropertyGrid GetPropGrid() {
			return propertyGrid1;
		}

//---------------------------------------------------------------------------------------

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {
			LRSJAS_Zigzag1_Paint(sender, e);
		}

//---------------------------------------------------------------------------------------

		internal void InvalidateMainPane() {
			MainPane.Invalidate();
		}

//---------------------------------------------------------------------------------------

		internal void RefreshPropertyGrid() {
			propertyGrid1.Refresh();
		}

//---------------------------------------------------------------------------------------

		private void btnCollapse_Click(object sender, EventArgs e) {
			Button btn = sender as Button;
			if (btn.Text == "Collapse") {
				this.splitContainer1.Panel2Collapsed = true;
				btn.Text = "Expand";
			} else {
				this.splitContainer1.Panel2Collapsed = false;
				btn.Text = "Collapse";
			}
		}

//---------------------------------------------------------------------------------------

		private void splitContainer1_Panel1_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == System.Windows.Forms.MouseButtons.Right) {
				contextMenuStrip1.Show(this, e.Location);
			}
		}

//---------------------------------------------------------------------------------------

		// To get here, add a ContextMenuStrip to the form. Add one or more entries
		// to the menu (including "New Box"). Then, from the properties if the
		// MenuItem, define its click event handler.
		private void newBoxToolStripMenuItem_Click(object sender, EventArgs e) {
			// TODO: Add dialog to prompt for BoxParms fields
			MessageBox.Show("(Virtually) Added new Box", "ZigDraw");
			IsDirty = true;
		}

//---------------------------------------------------------------------------------------

		private void backgroundToolStripMenuItem_Click(object sender, EventArgs e) {
			MessageBox.Show("Don't do that!", "It tickles");
		}

//---------------------------------------------------------------------------------------

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
			// Note: The preference is to *not* call Application.Exit(). That will not
			//		 fire the Form.Closing and Form.Closed events
		}

//---------------------------------------------------------------------------------------

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			var dlg = new AboutDialog();
			dlg.ShowDialog();
		}

//---------------------------------------------------------------------------------------

		private void LRSJAS_Zigzag1_FormClosing(object sender, FormClosingEventArgs e) {
			if (! IsDirty) {
				return;					// Let the close continue
			}
			DialogResult dr = MessageBox.Show("Form is dirty. Still close?", "ZigZag", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr != DialogResult.Yes) {
				e.Cancel = true;		// Halt the close procedure
				return;
			}
		}

//---------------------------------------------------------------------------------------

		private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
			// TODO: Bring up FileOpen dialog, defaulted to ZigZagDataFileName
			// TODO: Add call to GenericSerialization
			try {
				// GenericSerialization<ZigZagData
				using (StreamReader sr = new StreamReader(ZigZagDataFileName)) {
					XmlSerializer xs   = new XmlSerializer(typeof(ZigZagData));
					Data               = (ZigZagData)xs.Deserialize(sr);
				}
			} catch (Exception ex) {
				MessageBox.Show("Could not load " + ZigZagDataFileName + " - Error text: " + ex.Message, "ZigZag", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} 
		}

//---------------------------------------------------------------------------------------

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			Save(ZigZagDataFileName, Data);
		}

//---------------------------------------------------------------------------------------

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			saveFileDialog1.FileName = ZigZagDataFileName;
			saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			saveFileDialog1.OverwritePrompt = true;
			DialogResult dr = saveFileDialog1.ShowDialog();
			if (dr != DialogResult.Cancel) {
				ZigZagDataFileName = saveFileDialog1.FileName;
				Save(ZigZagDataFileName, Data);
			}
		}

//---------------------------------------------------------------------------------------

		private void Save(string FileName, ZigZagData data) {
			// TODO: This next loop isn't quite correct. If we fail in serialization,
			//		 we'll have updated the BoxParms without saving them.
			foreach (var box in Boxes) {
				box.parms.SaveBoxValues();
			}

			try {
				using (StreamWriter sw = new StreamWriter(FileName)) {
					XmlSerializer xs = new XmlSerializer(typeof(ZigZagData));
					xs.Serialize(sw, data);
				}
			} catch (Exception ex) {
				MessageBox.Show("Error (" + ex.ToString() + ") saving " + FileName, "ZigZag", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
