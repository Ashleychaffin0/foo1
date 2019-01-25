using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.IO;

namespace LRS.Programs {
	/// <summary>
	/// Summary description for SnarfG.
	/// </summary>
	public class SnarfG : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblFilesize;
		private System.Windows.Forms.Label lblDownloaded;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblLeft;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblPercent;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnGo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;



		string URL;
		private Button btnBrowseForDirectory;
		private TextBox txtFolderName;
		private Label label6;
		private TextBox txtPrefix1;
		private Label label8;
		private TextBox txtPrefix2;
		private Label label9;
		private TextBox txtSuffix;
		private Label label10;
		private Label lblDownloadSpeed;
		private Label label12;
		private FolderBrowserDialog folderBrowserDialog1;
		bool		StopFlag = false;

//---------------------------------------------------------------------------------------

		public SnarfG() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// TODO: Set tab stops

			// txtURL.Text = @"http://bethe.cornell.edu/media/lecture_1_small.mov";
			// txtFilename.Text = "bethe_lecture_1_small.mov";

			// var wc = new WebClient();
			// var xxx = wc.DownloadData("http://www.cs.berkeley.edu/~fateman/temp/for-jeremy/");
			// var yyy = System.Text.ASCIIEncoding.ASCII.GetString(xxx);

			txtURL.Text = "http://www.dzone.com/articles/built-in-dotnet-csv-parser";
			txtFolderName.Text = @"D:\LRS";
			txtFilename.Text = "foo.xxx";
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			lblFilesize.Text   = "";
			lblDownloaded.Text = "";
			lblLeft.Text	   = "";
			lblPercent.Text	   = "";

			URL = txtURL.Text;
			if (! URL.StartsWith("http://")) {
				URL = "http://" + URL;
			}
			// TODO: Check if URL is valid, etc
			// TODO: Check if filename exists, is valid, etc.
			btnGo.Enabled                 = false;
			btnCancel.Enabled             = true;
			btnBrowse.Enabled             = false;
			btnBrowseForDirectory.Enabled = false;
			try {
				DownloadFile();
			} catch (Exception ex) {
				MessageBox.Show("An unexpected error (" + ex.Message + ") occurred. Please try again.", "Snarf", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			btnGo.Enabled                 = true;
			btnCancel.Enabled             = false;
			btnBrowse.Enabled             = true;
			btnBrowseForDirectory.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		void DownloadFile() {
			StopFlag = false;
			var		req  = WebRequest.Create(URL) as HttpWebRequest;

#if false
			var jar = new CookieContainer();
			var link = new Uri("http://www.dzone.com");
			// jar.GetCookies(link);
			jar.Add(link, new Cookie("optimizelyEndUserId", "oeu1350055241531r0.8533763203611484"));
			jar.Add(link, new Cookie("optimizelyBuckets", "%7B%7D"));
			jar.Add(link, new Cookie("ACEGI_SECURITY_HASHED_REMEMBER_ME_COOKIE", "lrs5%21v3%21693148042"));
			jar.Add(link, new Cookie("optimizelySegments", "%7B%22177785857%22%3A%22false%22%2C%22177840837%22%3A%22campaign%22%2C%22177932055%22%3A%22ie%22%7D"));
			jar.Add(link, new Cookie("optimizelySegments", "%7B%221595101009%22%3A%22referral%22%2C%221596781023%22%3A%22ie%22%2C%221600230999%22%3A%22false%22%7D"));
			jar.Add(link, new Cookie("optimizelyEndUserId", "oeu1383852739121r0.7944368909013229"));
			jar.Add(link, new Cookie("optimizelyBuckets", "%7B%7D"));
			jar.Add(link, new Cookie("__gads", "ID=099d4fe97d7e97fc:T=1383852739:S=ALNI_MZWfDxrqK_RhiQIf4nluhR7t--Ckw"));
			jar.Add(link, new Cookie("SESS374e8db54ec3033c25a586b1d093b1d1", "44o23h4vr10jea4vgidgbhgkd4"));
			jar.Add(link, new Cookie("__utma", "68069956.545886677.1412826295.1424276698.1424468832.4"));
			jar.Add(link, new Cookie("__utmz", "68069956.1424468832.4.4.utmcsr=dzone.com|utmccn=(referral)|utmcmd=referral|utmcct=/links/index.html"));
			jar.Add(link, new Cookie("_ga", "GA1.2.545886677.1412826295"));
			jar.Add(link, new Cookie("__insp_uid", "1777562716"));
			req.CookieContainer = jar;
#endif

			WebResponse		resp = req.GetResponse();
			Stream			s	 = resp.GetResponseStream();
			BinaryReader	rdr  = new BinaryReader(s);
			string filename      = txtPrefix1.Text + txtPrefix2.Text + txtFilename.Text + txtSuffix.Text;
			filename             = filename.Replace(":", "--").Replace("?", "--");
			filename             = Path.Combine(txtFolderName.Text, filename);
			FileStream		wtr  = new FileStream(filename, FileMode.Create);

			const int		BUFSIZE = 64 * 16 * 1024;
			byte []			buf;
			long			filelen = resp.ContentLength;

			resp.Close();			// Don't run out of connections

			int				nBlocks = (int)((filelen + BUFSIZE - 1) / BUFSIZE);
#if false
			if (nBlocks == 0)
				nBlocks = 1;
#endif
			int				offset  = 0;
			DateTime		StartTime;

			progressBar1.Minimum = 0;
			progressBar1.Maximum = (int)filelen;
			progressBar1.Step	 = 1024;	

			lblFilesize.Text	 = string.Format("{0:N0}K", filelen / 1024);
			lblDownloaded.Text   = "0";
			lblLeft.Text		 = "0";
			lblPercent.Text		 = "0%";
			// resp.ContentType
			StartTime = DateTime.Now;
			for (int n=0; n<nBlocks; ++n) {
				Application.DoEvents();
				if (StopFlag)
					break;
				buf = rdr.ReadBytes(BUFSIZE);
				if (buf.Length > 0)		// May be == 0 for last block
					wtr.Write(buf, 0, buf.Length);
				offset += buf.Length;
				lblDownloaded.Text = string.Format("{0:N0}K", offset / 1024);
				lblLeft.Text	   = string.Format("{0:N0}K", (filelen - offset) / 1024);
				lblPercent.Text	   = ((float)offset / filelen).ToString("P1");
				// float speed = (float)(buf.Length) / (DateTime.Now - StartTime).Milliseconds;
				float speed = (float)(offset) / (DateTime.Now - StartTime).Milliseconds / 1000f;
				lblDownloadSpeed.Text = string.Format("{0:N0}K", speed);
				// TODO: Add download speeds: 1) Instantaneous, 2) Over last <n> (maybe
				//		 configurable by user via UpDown control) buffers, 3) From 
				//		 start of download.
				progressBar1.Value = offset;
			}
			rdr.Close();
			wtr.Close();
			lblPercent.Text = "Done";
		}

//---------------------------------------------------------------------------------------

		private void btnCancel_Click(object sender, System.EventArgs e) {
			StopFlag = true;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, System.EventArgs e) {
			DialogResult res = saveFileDialog1.ShowDialog();
			if (res == DialogResult.OK) {
				txtFilename.Text = saveFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseForDirectory_Click(object sender, EventArgs e) {
			DialogResult res = folderBrowserDialog1.ShowDialog();
			if (res == DialogResult.OK) {
				txtFolderName.Text = folderBrowserDialog1.SelectedPath;
			}
		}

//---------------------------------------------------------------------------------------

		private void txtURL_TextChanged(object sender, EventArgs e) {
			var uri = new Uri(txtURL.Text);
			var ext = Path.GetExtension(uri.AbsolutePath);
			txtSuffix.Text = ext;
		}
//---------------------------------------------------------------------------------------

		#region Windows Form Designer generated code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.lblFilesize = new System.Windows.Forms.Label();
			this.lblDownloaded = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblLeft = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblPercent = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.btnBrowseForDirectory = new System.Windows.Forms.Button();
			this.txtFolderName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtPrefix1 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtPrefix2 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtSuffix = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.lblDownloadSpeed = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "URL";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(88, 8);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(625, 20);
			this.txtURL.TabIndex = 1;
			this.txtURL.TextChanged += new System.EventHandler(this.txtURL_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Filename\r\n";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(86, 79);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(528, 20);
			this.txtFilename.TabIndex = 3;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(638, 79);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(73, 24);
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(86, 208);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(447, 24);
			this.progressBar1.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 149);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "File size - total";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblFilesize
			// 
			this.lblFilesize.Location = new System.Drawing.Point(13, 181);
			this.lblFilesize.Name = "lblFilesize";
			this.lblFilesize.Size = new System.Drawing.Size(104, 17);
			this.lblFilesize.TabIndex = 7;
			this.lblFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDownloaded
			// 
			this.lblDownloaded.Location = new System.Drawing.Point(181, 181);
			this.lblDownloaded.Name = "lblDownloaded";
			this.lblDownloaded.Size = new System.Drawing.Size(136, 17);
			this.lblDownloaded.TabIndex = 9;
			this.lblDownloaded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(181, 149);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 17);
			this.label5.TabIndex = 8;
			this.label5.Text = "File size - downloaded";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblLeft
			// 
			this.lblLeft.Location = new System.Drawing.Point(357, 181);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(104, 17);
			this.lblLeft.TabIndex = 11;
			this.lblLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(357, 149);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 17);
			this.label7.TabIndex = 10;
			this.label7.Text = "File size - left";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPercent
			// 
			this.lblPercent.Location = new System.Drawing.Point(262, 255);
			this.lblPercent.Name = "lblPercent";
			this.lblPercent.Size = new System.Drawing.Size(104, 16);
			this.lblPercent.TabIndex = 12;
			this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCancel
			// 
			this.btnCancel.Enabled = false;
			this.btnCancel.Location = new System.Drawing.Point(318, 281);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(206, 281);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(72, 24);
			this.btnGo.TabIndex = 14;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnBrowseForDirectory
			// 
			this.btnBrowseForDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseForDirectory.Location = new System.Drawing.Point(638, 43);
			this.btnBrowseForDirectory.Name = "btnBrowseForDirectory";
			this.btnBrowseForDirectory.Size = new System.Drawing.Size(73, 25);
			this.btnBrowseForDirectory.TabIndex = 18;
			this.btnBrowseForDirectory.Text = "Browse";
			this.btnBrowseForDirectory.Click += new System.EventHandler(this.btnBrowseForDirectory_Click);
			// 
			// txtFolderName
			// 
			this.txtFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolderName.Location = new System.Drawing.Point(86, 43);
			this.txtFolderName.Name = "txtFolderName";
			this.txtFolderName.Size = new System.Drawing.Size(528, 20);
			this.txtFolderName.TabIndex = 17;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(13, 43);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 32);
			this.label6.TabIndex = 16;
			this.label6.Text = "Directory\r\n";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPrefix1
			// 
			this.txtPrefix1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrefix1.Location = new System.Drawing.Point(86, 114);
			this.txtPrefix1.Name = "txtPrefix1";
			this.txtPrefix1.Size = new System.Drawing.Size(166, 20);
			this.txtPrefix1.TabIndex = 20;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(13, 114);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(59, 25);
			this.label8.TabIndex = 19;
			this.label8.Text = "Prefix1";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPrefix2
			// 
			this.txtPrefix2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrefix2.Location = new System.Drawing.Point(230, 112);
			this.txtPrefix2.Name = "txtPrefix2";
			this.txtPrefix2.Size = new System.Drawing.Size(166, 20);
			this.txtPrefix2.TabIndex = 22;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(157, 112);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(59, 24);
			this.label9.TabIndex = 21;
			this.label9.Text = "Prefix2";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSuffix
			// 
			this.txtSuffix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSuffix.Location = new System.Drawing.Point(371, 109);
			this.txtSuffix.Name = "txtSuffix";
			this.txtSuffix.Size = new System.Drawing.Size(166, 20);
			this.txtSuffix.TabIndex = 24;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(298, 109);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(59, 24);
			this.label10.TabIndex = 23;
			this.label10.Text = "Suffix";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDownloadSpeed
			// 
			this.lblDownloadSpeed.Location = new System.Drawing.Point(487, 181);
			this.lblDownloadSpeed.Name = "lblDownloadSpeed";
			this.lblDownloadSpeed.Size = new System.Drawing.Size(104, 17);
			this.lblDownloadSpeed.TabIndex = 26;
			this.lblDownloadSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(487, 149);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 17);
			this.label12.TabIndex = 25;
			this.label12.Text = "Download Speed\r\n";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SnarfG
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(721, 385);
			this.Controls.Add(this.lblDownloadSpeed);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.txtSuffix);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtPrefix2);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtPrefix1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnBrowseForDirectory);
			this.Controls.Add(this.txtFolderName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblPercent);
			this.Controls.Add(this.lblLeft);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lblDownloaded);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblFilesize);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.label1);
			this.Name = "SnarfG";
			this.Text = "Snarf";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new SnarfG());
		}
	}
}
