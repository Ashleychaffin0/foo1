using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using SHDocVw;
using mshtml;

namespace LRSNews_cs_July_03 {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LRSNewsForm : System.Windows.Forms.Form 	{
		#region LRSNews GUI
		private System.Windows.Forms.Button btnAddToNews;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnForward;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPostToWeb;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label lblMsg;
		private AxSHDocVw.AxWebBrowser web;
		private System.Windows.Forms.ComboBox cmbSites;
		private System.Windows.Forms.Label lblIEStatusMsg;
		private System.Windows.Forms.Label lblPctDone;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		bool				OKToRun = true;

		DataSet				dsLRSNews;
		OleDbConnection		conn;
		static string		dbname = @"cs-LRSNewsdb.mdb";
		string				dbFilename;
		
		HTMLDocumentClass	doc;
		bool				isFinalFrame;

		ArrayList			PrefixesToDrop = new ArrayList();
		
		LRSNewsItem			NewsItem;
		AddNewsItem			frmNewsItem;

//---------------------------------------------------------------------------------------

		public LRSNewsForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			doc = null;
			
			dbFilename = FindFileUpInHierarchy(dbname);
			if (dbFilename == null) {
				MessageBox.Show("Unable to find LRSNews database - " + dbname);
				return;
			}

			dsLRSNews	 = new DataSet();
			string sConn = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + dbFilename;
			conn		 = new OleDbConnection(sConn);
			conn.Open();
			if (conn.State != ConnectionState.Open) {
				OKToRun = false;
				MessageBox.Show("Unable to open database. Connection string is \n\n" 
					+ sConn, "LRSNews Connection Error",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			SetupDatabaseTables();

			NewsItem = new LRSNewsItem(web);
			frmNewsItem = new AddNewsItem(dsLRSNews, this);
		}

//---------------------------------------------------------------------------------------

		void SetupDatabaseTables() {
			string				SQL;

			ReadMagsTable();
			// cmbSites.DataSource = dsLRSNews.Tables["Mags"];
			cmbSites.DataSource		= dsLRSNews;
			cmbSites.DisplayMember	= "Mags.MagName";
			cmbSites.ValueMember	= "Mags.URL";
			cmbSites.SelectedIndex = 0;

			SQL = "SELECT * FROM [Title Prefixes to Drop]";
#if false
			adapt = new OleDbDataAdapter(SQL, conn);
			adapt.Fill(dsLRSNews, "Title Prefixes to Drop");
#else
			// Read this in via ExecuteReader. If/when we need to dynamically add a
			// new prefix, it'll be easier (but perhaps less educational) to INSERT
			// it into the database, and just append it to the PrefixesToDrop ArrayList.
			OleDbCommand	cmd = new OleDbCommand(SQL, conn);
			OleDbDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read()) {
				PrefixesToDrop.Add((string)rdr["Prefix"]);
			}
			rdr.Close();
#endif
		}

//---------------------------------------------------------------------------------------

		public void ReadMagsTable() {
			OleDbDataAdapter	adapt;
			string				SQL;
			SQL = "SELECT * FROM Mags ORDER BY SeqNo";
			adapt = new OleDbDataAdapter(SQL, conn);
			if (dsLRSNews.Tables.Contains("Mags"))
				dsLRSNews.Tables["Mags"].Clear();
			adapt.Fill(dsLRSNews, "Mags");
		}

//---------------------------------------------------------------------------------------

		string FindFileUpInHierarchy(string filename) {
			// TODO: Need overload - with and without default starting directory
			string	path = Application.ExecutablePath + @"\";
			bool	bFound = false;

			// TODO: Not 6. Should stop when it gets to merely a drive letter
			for (int i=0; i<6; ++i) {				// Six levels should be enough!
				if (File.Exists(path + filename)) {
					bFound = true;
					break;
				} else {
					filename = @"..\" + filename;
				}
			}
			if (! bFound) {
				return null;
			}
			FileInfo	fi = new FileInfo(path + filename);
			return fi.FullName;
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e) {
			AxSHDocVw.AxWebBrowser web2 = (AxSHDocVw.AxWebBrowser)sender;	
			doc = web2.Document as HTMLDocumentClass;
			isFinalFrame = e.pDisp == web.Application;
			if (isFinalFrame) {
				txtURL.Text = web.LocationURL;
				lblTitle.Text = StripPrefixesSuffixes(doc.title);
				lblPctDone.Text = "Done";
			}
		}

//---------------------------------------------------------------------------------------

		public string StripPrefixesSuffixes(string title) {
			bool	bDone = false;
			foreach (string s in PrefixesToDrop) {
				if (title.StartsWith(s)) {
					title = title.Substring(s.Length);
					bDone = true;
				}
				// I suppose a title could have both a prefix and a suffix
				if (title.EndsWith(s)) {
					title = title.Substring(0, title.Length - s.Length);
					bDone = true;
				}
				if (bDone)
					break;
			}
			return title;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			Navigate(txtURL.Text);
		}

//---------------------------------------------------------------------------------------
		private void btnAddToNews_Click(object sender, System.EventArgs e) {
			NewsItem.Reload();
			frmNewsItem.ShowNewItem(NewsItem);
		}

//---------------------------------------------------------------------------------------

		public void Navigate(string URL) {
			object	zero = 0;
			object	nl	 = "";
			web.Navigate(URL, ref zero, ref nl, ref nl, ref nl);
		}

//---------------------------------------------------------------------------------------

		private void web_StatusTextChange(object sender, AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEvent e) {
			lblIEStatusMsg.Text = e.text;
		}

//---------------------------------------------------------------------------------------

		private void cmbSites_Click(object sender, System.EventArgs e) {
			ComboBox	cb = sender as ComboBox;

		}

//---------------------------------------------------------------------------------------

		private void btnRefresh_Click(object sender, System.EventArgs e) {
			web.Refresh();
		}

//---------------------------------------------------------------------------------------

		private void btnStop_Click(object sender, System.EventArgs e) {
			web.Stop();
		}

//---------------------------------------------------------------------------------------

		private void btnForward_Click(object sender, System.EventArgs e) {
			web.GoForward();		// TODO: Check cmdstatus (or whatever)
		}

//---------------------------------------------------------------------------------------

		private void btnBack_Click(object sender, System.EventArgs e) {
			web.GoBack();			// TODO: Check cmdstatus (or whatever)
		}

//---------------------------------------------------------------------------------------

		private void btnNext_Click(object sender, System.EventArgs e) {
			int		ix = cmbSites.SelectedIndex + 1;
			if (ix >= cmbSites.Items.Count)
				ix = 0;
			cmbSites.SelectedIndex = ix;
		}

//---------------------------------------------------------------------------------------

		private void cmbSites_SelectedIndexChanged(object sender, System.EventArgs e) {
			DataRowView	drv = (DataRowView)cmbSites.SelectedItem;
			string targetURL = drv["URL"].ToString();
			Navigate(targetURL);
		}

//---------------------------------------------------------------------------------------

		private void web_ProgressChange(object sender, AxSHDocVw.DWebBrowserEvents2_ProgressChangeEvent e) {
			int	ProgressMax = e.progressMax;
			int Progress	= e.progress;
			if (ProgressMax <= 0)
				return;
			Progress = (100 * Progress) / ProgressMax;
			if (Progress > 100)
				Progress = 100;
			progressBar1.Value = Progress;
			lblPctDone.Text = Progress.ToString() + "%";
		}

//---------------------------------------------------------------------------------------

		private void LRSNewsForm_Load(object sender, System.EventArgs e) {
			this.WindowState = FormWindowState.Maximized;
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LRSNewsForm));
			this.web = new AxSHDocVw.AxWebBrowser();
			this.btnAddToNews = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnForward = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPostToWeb = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.cmbSites = new System.Windows.Forms.ComboBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblPctDone = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lblIEStatusMsg = new System.Windows.Forms.Label();
			this.lblMsg = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.web)).BeginInit();
			this.SuspendLayout();
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Enabled = true;
			this.web.Location = new System.Drawing.Point(8, 104);
			this.web.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("web.OcxState")));
			this.web.Size = new System.Drawing.Size(680, 256);
			this.web.TabIndex = 0;
			this.web.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(this.web_StatusTextChange);
			this.web.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.web_DocumentComplete);
			this.web.ProgressChange += new AxSHDocVw.DWebBrowserEvents2_ProgressChangeEventHandler(this.web_ProgressChange);
			// 
			// btnAddToNews
			// 
			this.btnAddToNews.BackColor = System.Drawing.SystemColors.Control;
			this.btnAddToNews.Location = new System.Drawing.Point(8, 8);
			this.btnAddToNews.Name = "btnAddToNews";
			this.btnAddToNews.Size = new System.Drawing.Size(104, 24);
			this.btnAddToNews.TabIndex = 1;
			this.btnAddToNews.Text = "Add to News";
			this.btnAddToNews.Click += new System.EventHandler(this.btnAddToNews_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.SystemColors.Control;
			this.btnRefresh.Location = new System.Drawing.Point(144, 8);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(64, 24);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnBack
			// 
			this.btnBack.BackColor = System.Drawing.SystemColors.Control;
			this.btnBack.Location = new System.Drawing.Point(360, 8);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(64, 24);
			this.btnBack.TabIndex = 3;
			this.btnBack.Text = "Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// btnForward
			// 
			this.btnForward.BackColor = System.Drawing.SystemColors.Control;
			this.btnForward.Location = new System.Drawing.Point(288, 8);
			this.btnForward.Name = "btnForward";
			this.btnForward.Size = new System.Drawing.Size(64, 24);
			this.btnForward.TabIndex = 4;
			this.btnForward.Text = "Fwd";
			this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
			// 
			// btnStop
			// 
			this.btnStop.BackColor = System.Drawing.SystemColors.Control;
			this.btnStop.Location = new System.Drawing.Point(216, 8);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(64, 24);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnNext
			// 
			this.btnNext.BackColor = System.Drawing.SystemColors.Control;
			this.btnNext.Location = new System.Drawing.Point(360, 40);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(64, 24);
			this.btnNext.TabIndex = 6;
			this.btnNext.Text = "Next";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPostToWeb
			// 
			this.btnPostToWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPostToWeb.BackColor = System.Drawing.SystemColors.Control;
			this.btnPostToWeb.Location = new System.Drawing.Point(608, 8);
			this.btnPostToWeb.Name = "btnPostToWeb";
			this.btnPostToWeb.Size = new System.Drawing.Size(80, 24);
			this.btnPostToWeb.TabIndex = 7;
			this.btnPostToWeb.Text = "Post to Web";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(8, 72);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(40, 24);
			this.btnGo.TabIndex = 8;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// cmbSites
			// 
			this.cmbSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSites.Location = new System.Drawing.Point(144, 40);
			this.cmbSites.Name = "cmbSites";
			this.cmbSites.Size = new System.Drawing.Size(208, 21);
			this.cmbSites.TabIndex = 9;
			this.cmbSites.Click += new System.EventHandler(this.cmbSites_Click);
			this.cmbSites.SelectedIndexChanged += new System.EventHandler(this.cmbSites_SelectedIndexChanged);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(440, 16);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(112, 16);
			this.progressBar1.TabIndex = 10;
			// 
			// lblPctDone
			// 
			this.lblPctDone.Location = new System.Drawing.Point(560, 8);
			this.lblPctDone.Name = "lblPctDone";
			this.lblPctDone.Size = new System.Drawing.Size(40, 24);
			this.lblPctDone.TabIndex = 11;
			this.lblPctDone.Text = "Done";
			this.lblPctDone.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.Location = new System.Drawing.Point(440, 40);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(248, 56);
			this.lblTitle.TabIndex = 12;
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(56, 72);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(368, 20);
			this.txtURL.TabIndex = 13;
			this.txtURL.Text = "";
			// 
			// lblIEStatusMsg
			// 
			this.lblIEStatusMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblIEStatusMsg.Location = new System.Drawing.Point(0, 366);
			this.lblIEStatusMsg.Name = "lblIEStatusMsg";
			this.lblIEStatusMsg.Size = new System.Drawing.Size(696, 16);
			this.lblIEStatusMsg.TabIndex = 14;
			// 
			// lblMsg
			// 
			this.lblMsg.Location = new System.Drawing.Point(8, 40);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(128, 24);
			this.lblMsg.TabIndex = 15;
			// 
			// LRSNewsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Aqua;
			this.ClientSize = new System.Drawing.Size(696, 382);
			this.Controls.Add(this.lblMsg);
			this.Controls.Add(this.lblIEStatusMsg);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.lblPctDone);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.cmbSites);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnPostToWeb);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnForward);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnAddToNews);
			this.Controls.Add(this.web);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Name = "LRSNewsForm";
			this.Text = "LRSNews - July 03 version";
			this.Load += new System.EventHandler(this.LRSNewsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.web)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			LRSNewsForm		frm = new LRSNewsForm();
			if (frm.OKToRun)
				Application.Run(frm);
		}
	}
}
