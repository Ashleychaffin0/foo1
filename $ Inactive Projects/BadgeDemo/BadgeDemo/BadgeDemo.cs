// Copyright (c) 2005, Bartizan Connects LLP

// TODO:

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;

using Bartizan.Print;

using System.Runtime.InteropServices;	 // TODO: Just for DllImport dbg routine


namespace BadgeDemo {
	/// <summary>
	/// Summary description for BadgeDemo.
	/// </summary>
	public class BadgeDemo : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblDisclaimer;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.Button btnMakeBadge;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.TextBox txtBadgeID;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		PrintDocument	pd;
		PrintLabelPage	labelPage;
		private System.Windows.Forms.Button btn600CharsToClip;
		private System.Windows.Forms.Button btn100CharsToClip;
		PrintReq		pReq = new PrintReq();

		public BadgeDemo() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Hardcode these for the demo
			int		rows = 5, cols = 3;
			int		left = 100, top = 20;
			int		hGut = 30, vGut = 40;
			int		sizeX = 200, sizeY = 300;
#if true
			rows = 3; cols = 2;
			left = 25; top = 100;
			hGut = 0; vGut = 0;
			sizeX = 400; sizeY = 400;
#endif
			labelPage = new PrintLabelPage(rows, cols, left, top, hGut, vGut, new Size(sizeX, sizeY));

			pd = new PrintDocument();
			pd.DocumentName = "Test Labels";
			// pd.PrinterSettings.PrinterName = "Auto RICOH Aficio AP3800C PCL 5c on BARTSBS";
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(this.printDocument1_PrintPage);

			// For Print Preview support
			printPreviewDialog1.Document = pd;
			// Next lines are just a kludge TODO:
			printPreviewDialog1.Left = 0;
			printPreviewDialog1.Width  = SystemInformation.PrimaryMonitorSize.Width  / 2;
			printPreviewDialog1.Height = SystemInformation.PrimaryMonitorSize.Height / 2;

#if false		// Other print-related stuff, not needed for now
			// pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\HP Deskjet 940c series";
			PageSettings ps = pd.PrinterSettings.DefaultPageSettings;
			// ps.Landscape = true;
			pd.DefaultPageSettings = ps;
#endif
		}

		//---------------------------------------------------------------------------------------

		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
#if false		// For now
			const int		rows = 5, cols = 3;
			const int		left = 100, top = 40;
			const int		hGut = 30, vGut = 40;
			const int		sizeX = 150, sizeY = 100;
			PrintLabelPage	plp = new PrintLabelPage(rows, cols, left, top, hGut, vGut, new Size(sizeX, sizeY));
			PrintLabel		pl;
			bool			bLast;
			Pen				pen = new Pen(Brushes.Red);
			Font			fnt = new Font("Arial", 8f);
			string			msg;
			Rectangle		r;
			msg = string.Format("Rows={0}, Cols={1}, Top={2}, Left={3}, hGut={4}, vGut={5}, X={6}, Y={7}",
				rows, cols, top, left, hGut, vGut, sizeX, sizeY);
			e.Graphics.DrawString(msg, fnt, Brushes.Green, new RectangleF(e.PageBounds.Location, e.PageBounds.Size));
			for (int i=0; i<20; i++) {
				pl = plp.GetNextPrintLabel(out bLast);
				Console.WriteLine("Label {0,2}, Loc={1}, Size={2}, bLast={3}",
					i, pl.Rect.Location, pl.Rect.Size, bLast);
				e.Graphics.DrawRectangle(pen, pl.Rect);
				r = pl.Rect;
				r.Offset(10, 10);
				msg = string.Format("Rect={0}", pl.Rect);
				e.Graphics.DrawString(msg, fnt, Brushes.Blue, new RectangleF(r.X, r.Y, r.Width, r.Height));
			}
#else
			PrintRuler(e);
			foreach (PrintPage lbl in pReq.objs) {
				// e.Graphics.DrawRectangle(Pens.Red, lbl.Rect);	// Debug	// TODO:
				foreach (PrintObj obj in lbl.Data) {
					obj.Print(e.Graphics, lbl.Rect.Location);
				}
			} 
			// e.Cancel = true;
#endif
			e.HasMorePages = false;			// End of job 
		}

//---------------------------------------------------------------------------------------

		void PrintRuler(PrintPageEventArgs e) {
			Rectangle	rect = e.PageBounds;	// Units are 1/100" inch
			int			x, y;
			// Horizontal ruler
			int			ht = 32;
			for (int inch = rect.X; inch<rect.Width; inch+=100) {
				e.Graphics.DrawLine(Pens.RoyalBlue, inch, rect.Y, inch, rect.Y + ht);
				for (int dec = 10; dec < 100; dec += 10) {
					x = inch + dec;
					y = rect.Y;
					if (dec == 50)
						e.Graphics.DrawLine(Pens.RoyalBlue, x, y, x, y + ht / 2);
					else
						e.Graphics.DrawLine(Pens.RoyalBlue, x, y, x, y + ht / 4);
				}
			}

			// Vertical ruler
			ht = 64;
			for (int inch = rect.Y; inch<rect.Height; inch += 100) {
				e.Graphics.DrawLine(Pens.RoyalBlue, rect.Width - ht, rect.Y + inch, rect.Width, rect.Y + inch);
				for (int dec = 10; dec < 100; dec += 10) {
					x = rect.Width;
					y = inch + dec;
					if (dec == 50)
						e.Graphics.DrawLine(Pens.RoyalBlue, x - ht / 2, y, x, y);
					else
						e.Graphics.DrawLine(Pens.RoyalBlue, x - ht / 4, y, x, y);
				}

			}
			int	ofs = 0;
			rect.Offset(ofs, ofs);
			rect.Inflate(-ofs, -ofs);
			e.Graphics.DrawRectangle(Pens.Purple, rect);

		}

//---------------------------------------------------------------------------------------

		private void BadgeDemo_Load(object sender, System.EventArgs e) {
			string	msg = "This demo program is, by design, fairly minimal. The point";
			msg += " is to demonstrate how to create badges. Wizards to prompt the";
			msg += " user for the data they want, how they want it laid out, etc";
			msg += " are outside the scope of this project.";
			lblDisclaimer.Text = msg;
		}

//---------------------------------------------------------------------------------------
		
		private void btnBrowse_Click(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog1.ShowDialog();
			if (res == DialogResult.OK) {
				pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnMakeBadge_Click(object sender, System.EventArgs e) {
			// Note: The .NET printing design works with an idealized granularity of
			//		 1/100th of an inch, which is then automatically mapped into 300x300
			//		 or whatever your current print resolution is. One nice side effect
			//		 is that since the default screen resolution is 96 DPI, you get a
			//		 pretty-close-to WYSIWYG experience during Print Preview.
			//		 Anyway, the default PageBounds for an 8 1/2 by 11 piece of paper
			//		 would be Size(850, 1100). This would be truncated to Size(650, 900)
			//		 if there were 1" margins on all sides.
			bool	bLast;				// Last label on page?
			PrintPage	lbl = labelPage.GetNextPrintLabel(out bLast);

			// The Name
			PrintObjText Name = new PrintObjText(new Point(10, 10), new Size(180, 20), txtName.Text);
			Name.Alignment = PrintObj.FieldAlignment.Centered;
			lbl.Add(Name);

			// The Badge ID
			PrintObjText BadgeID = new PrintObjText(new Point(10, 30), new Size(180, 20), txtBadgeID.Text);
			BadgeID.Alignment = PrintObj.FieldAlignment.Right;
			lbl.Add(BadgeID);

			// The Picture (if any)
			if (pictureBox1.Image != null) {
				PrintObjImage	img = new PrintObjImage(new Point(50, 60), new Size(100, 100), pictureBox1.Image);
				lbl.Add(img);
			}

			// The Barcode
#if true
			string		msg = txtName.Text + " - " + txtBadgeID.Text;
#else
			string		msg = @"3010
Frank
Mosher


1600 Boston Providence Highway

Walpole
MA
02081
USA
508 660 6785
508 660 6884
fmosher@leadsetc.com






";		// TODO: To make an explicit barcode
#endif
			PrintObjBarcode	bc = new PrintObjBarcode(new Point(0, 180), new Size(150, 50), msg);
			lbl.Add(bc);
			pReq.Add(lbl);

			txtName.Text = "";
			txtBadgeID.Text = "";
			pictureBox1.Image = null;

			if (bLast) {
				// TODO: Needs try/catch
				// TODO: Shouldn't automatically invoke Print Preview. Maybe put
				//		 checkbox for PP, and just have a single Print button?
				printPreviewDialog1.ShowDialog();	
				pReq.Clear();
			}
		}

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, System.EventArgs e) {
			pd.Print();
		}

//---------------------------------------------------------------------------------------

		private void btnPreview_Click(object sender, System.EventArgs e) {
			printPreviewDialog1.ShowDialog();	
		}

//---------------------------------------------------------------------------------------

		private void btnExit_Click(object sender, System.EventArgs e) {
			Application.Exit();
		}

//---------------------------------------------------------------------------------------

		private void btn100CharsToClip_Click(object sender, System.EventArgs e) {
			string msg = "abcdefghijklmnopqrstuvwxy";		// 25 chars	(no "z")
			msg += msg;			// 50 chars
			msg += msg;			// 100 chars
			txtName.Text = msg;
			Clipboard.SetDataObject(msg, false);
		}

//---------------------------------------------------------------------------------------

		private void btn600CharsToClip_Click(object sender, System.EventArgs e) {
			string alpha = "abcdefghijklmnopqrstuvwxyz";		// 26 chars
			string msg = alpha + alpha.ToUpper() + "01234567";	// 60 chars
			msg += msg;			// 120 chars
			string	msg120 = msg;
			msg += msg;			// 240 chars
			msg += msg;			// 480 chars
			msg += msg120;		// 600 chars
			txtName.Text = msg;
			Clipboard.SetDataObject(msg, false);
		}

//---------------------------------------------------------------------------------------

#if false	// Just a bit of fun. Determine when a CD has been loaded/ejected
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		struct DEV_BROADCAST_HDR {
			int		dbch_size;
			int		dbch_devicetype;
			int		dbch_reserved;
		}

//---------------------------------------------------------------------------------------

		protected override void WndProc(ref Message m) {
			// Place to override everything!
			const int WM_DEVICECHANGE = 0x0219;
			switch (m.Msg) {
			case WM_DEVICECHANGE:
				DEV_BROADCAST_HDR hdr = (DEV_BROADCAST_HDR)m.GetLParam(typeof(DEV_BROADCAST_HDR));
				return;
			default:
				break;
			}
			base.WndProc(ref m);
		}
#endif
//---------------------------------------------------------------------------------------

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BadgeDemo));
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtBadgeID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblDisclaimer = new System.Windows.Forms.Label();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.btnMakeBadge = new System.Windows.Forms.Button();
			this.btnPreview = new System.Windows.Forms.Button();
			this.btn600CharsToClip = new System.Windows.Forms.Button();
			this.btn100CharsToClip = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 136);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(16, 160);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(184, 22);
			this.txtName.TabIndex = 0;
			this.txtName.Text = "";
			// 
			// txtBadgeID
			// 
			this.txtBadgeID.Location = new System.Drawing.Point(232, 160);
			this.txtBadgeID.Name = "txtBadgeID";
			this.txtBadgeID.Size = new System.Drawing.Size(104, 22);
			this.txtBadgeID.TabIndex = 1;
			this.txtBadgeID.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(240, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Badge ID";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDisclaimer
			// 
			this.lblDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDisclaimer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(255)), ((System.Byte)(255)));
			this.lblDisclaimer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDisclaimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDisclaimer.ForeColor = System.Drawing.Color.Red;
			this.lblDisclaimer.Location = new System.Drawing.Point(8, 16);
			this.lblDisclaimer.Name = "lblDisclaimer";
			this.lblDisclaimer.Size = new System.Drawing.Size(432, 104);
			this.lblDisclaimer.TabIndex = 4;
			this.lblDisclaimer.Text = "(msg generated in code)";
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(352, 264);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(88, 32);
			this.btnPrint.TabIndex = 5;
			this.btnPrint.Text = "Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(352, 312);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(88, 32);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "Exit";
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(8, 200);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(152, 32);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse for image";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(184, 200);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(152, 144);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
			this.printPreviewDialog1.Location = new System.Drawing.Point(1, 221);
			this.printPreviewDialog1.MinimumSize = new System.Drawing.Size(375, 250);
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.TransparencyKey = System.Drawing.Color.Empty;
			this.printPreviewDialog1.Visible = false;
			// 
			// btnMakeBadge
			// 
			this.btnMakeBadge.Location = new System.Drawing.Point(352, 152);
			this.btnMakeBadge.Name = "btnMakeBadge";
			this.btnMakeBadge.Size = new System.Drawing.Size(88, 32);
			this.btnMakeBadge.TabIndex = 3;
			this.btnMakeBadge.Text = "Make Badge";
			this.btnMakeBadge.Click += new System.EventHandler(this.btnMakeBadge_Click);
			// 
			// btnPreview
			// 
			this.btnPreview.Location = new System.Drawing.Point(352, 224);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(88, 32);
			this.btnPreview.TabIndex = 4;
			this.btnPreview.Text = "Preview";
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			// 
			// btn600CharsToClip
			// 
			this.btn600CharsToClip.Location = new System.Drawing.Point(8, 296);
			this.btn600CharsToClip.Name = "btn600CharsToClip";
			this.btn600CharsToClip.Size = new System.Drawing.Size(152, 48);
			this.btn600CharsToClip.TabIndex = 10;
			this.btn600CharsToClip.Text = "Put 600 char msg on clipboard and Name";
			this.btn600CharsToClip.Click += new System.EventHandler(this.btn600CharsToClip_Click);
			// 
			// btn100CharsToClip
			// 
			this.btn100CharsToClip.Location = new System.Drawing.Point(8, 240);
			this.btn100CharsToClip.Name = "btn100CharsToClip";
			this.btn100CharsToClip.Size = new System.Drawing.Size(152, 48);
			this.btn100CharsToClip.TabIndex = 11;
			this.btn100CharsToClip.Text = "Put 100 char msg on clipboard and Name";
			this.btn100CharsToClip.Click += new System.EventHandler(this.btn100CharsToClip_Click);
			// 
			// BadgeDemo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.BackColor = System.Drawing.Color.Aqua;
			this.ClientSize = new System.Drawing.Size(448, 568);
			this.Controls.Add(this.btn100CharsToClip);
			this.Controls.Add(this.btn600CharsToClip);
			this.Controls.Add(this.btnPreview);
			this.Controls.Add(this.btnMakeBadge);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.lblDisclaimer);
			this.Controls.Add(this.txtBadgeID);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "BadgeDemo";
			this.Text = "Badge Demo";
			this.Load += new System.EventHandler(this.BadgeDemo_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new BadgeDemo());
		}

	}
}
