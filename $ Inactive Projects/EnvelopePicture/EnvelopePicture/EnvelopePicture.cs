using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace EnvelopePicture {
	/// <summary>
	/// Summary description for EnvelopePicture.
	/// </summary>
	public class EnvelopePicture : System.Windows.Forms.Form {
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnPrint;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		string		filename;

		public EnvelopePicture() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnPrint = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(16, 16);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(104, 48);
			this.btnBrowse.TabIndex = 0;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(16, 88);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(264, 160);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(160, 16);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(104, 48);
			this.btnPrint.TabIndex = 2;
			this.btnPrint.Text = "Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// EnvelopePicture
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(292, 260);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnBrowse);
			this.Name = "EnvelopePicture";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new EnvelopePicture());
		}

		private void btnBrowse_Click(object sender, System.EventArgs e) {
			//openFileDialog1.FileNames = "JPG|*.jpg|BMP|*.bmp|All files|*.*";
			DialogResult	rc = openFileDialog1.ShowDialog();
			if (rc == DialogResult.OK) {
				filename = openFileDialog1.FileNames[0];
				pictureBox1.Image = Image.FromFile(filename);
			}
		}

		private void btnPrint_Click(object sender, System.EventArgs e) {
			if (filename == null) {
				MessageBox.Show("You must select a picture first");
				return;
			}
			PrintDocument pd = new PrintDocument();
			pd.DocumentName = "BartCarte";
			// PrinterSettings.StringCollection	printers = PrinterSettings.InstalledPrinters;	// For later
			//pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-940";
			//pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-DTC510_5";
			// pd.PrinterSettings.PrinterName = "DTC510_515 Card Printer";
			// pd.PrinterSettings.PrintToFile = true;
			PageSettings ps = pd.PrinterSettings.DefaultPageSettings;
			ps.Landscape = true;
			int			ips;
			for (ips=0; ips<pd.PrinterSettings.PaperSizes.Count; ++ips) {
				if (pd.PrinterSettings.PaperSizes[ips].PaperName == "A5 (148 x 210 mm) ")
					break;
			}
			ps.PaperSize = pd.PrinterSettings.PaperSizes[ips];
			pd.DefaultPageSettings = ps;
			// pd.PrintController = new PreviewPrintController();
			// PrintPreviewControl	pvc = new PrintPreviewControl();
			// PrintPreviewDialog	pvd = new PrintPreviewDialog();
			// pvd.Document = pd;
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(this.printDocument1_PrintPage);
			try {
				pd.Print();
				// pvd.ShowDialog();
			} catch (Exception ex) {
				MessageBox.Show("Exception in pd.Print() - " + ex.Message);
			}
		}

//---------------------------------------------------------------------------------------

		public bool ThumbnailCallback() {
			return false;
		}


//---------------------------------------------------------------------------------------

		private void printDocument1_PrintPage(object sender, PrintPageEventArgs e) {
#if true
#if false
			string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			Bitmap	bm = new Bitmap(Image.FromFile(dir + @"\Jersey Shore - Sept 20, 2003\Jersey Shore - Sept 20, 2003 007.jpg"));
#else
			Bitmap	bm  = new Bitmap(Image.FromFile(filename));
			// TODO: Do something with the Thumbnail. Alternatively, as the documentation
			//		 for GetThumbnailImage mentions, you can also do this in DrawImage()
			Image	bm2 = bm.GetThumbnailImage(30, 20, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
#endif
			GraphicsUnit	gu = GraphicsUnit.Pixel;
			RectangleF		rf1 = bm.GetBounds(ref gu);
			Rectangle		r1 = e.PageBounds;
			int				width = bm.Width, 
							height = bm.Height;
			r1.Offset((r1.Width - bm.Width) / 2, 0);
			e.Graphics.DrawImage(bm, r1);
#endif

#if false
			// e.Graphics.DrawString("Bartizan One Stop", new Font("Arial", 32, FontStyle.Italic), Brushes.PowderBlue, 300, 0);

			string name = txtFName.Text.Trim() + " " + txtLName.Text.Trim();
			// Print it centered, 3/4 of the way down
			SizeF	size = e.Graphics.MeasureString(name, norm);
			int		width = e.PageBounds.Width;
			int		height = e.PageBounds.Height;
			int		x, y;
			x = (int)((width - size.Width) / 2);
			y = (int)((height - size.Height) * 3f / 4f);
			e.Graphics.DrawString(name,	norm, Brushes.Black, x, y);
#endif
		}
	}
}
