using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LRS {

	/// <summary>
	/// Summary description for GetAPOD.
	/// </summary>
	public class GetAPOD : System.Windows.Forms.Form {
		#region
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dtPickerFrom;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label StatMsg;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.DateTimePicker dtPickerTo;
		private System.Windows.Forms.TextBox APODURL;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtTargetDirectory;
		private System.Windows.Forms.Label lblProcDate;
		private System.Windows.Forms.Button btnOpenAPODDir;
		private System.Windows.Forms.Button btnOpenAPODURL;
		#endregion

		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		WebClient		web;

		GetAPODParms	parms;
		const string	ParmsFilename = "GetAPODParms.xml";

//---------------------------------------------------------------------------------------

		public GetAPOD() {

			// Uri u = new Uri("http://dejadejadeja.com/10years/query.php?newsgroups=comp.sys.apple&subject=infocom&email_from=&body=&dateearly=&datelate=&minsize=&maxsize=&maxrows=100&orderby1=composeddate&orderby2=composedtime");
			// var segs = u.Segments;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (File.Exists(ParmsFilename)) {
				try {
					Stream			s = File.OpenRead(ParmsFilename);
					XmlSerializer	x = new XmlSerializer(typeof(GetAPODParms));
					parms = (GetAPODParms) x.Deserialize(s);
					s.Close();
				} catch (Exception ex) {
					MessageBox.Show("Unable to read config file " + ParmsFilename + ", error = " + ex.Message, "GetAPOD");
					parms = new GetAPODParms();
				}
			} else
				parms = new GetAPODParms();

			if (Directory.Exists(parms.APODDir))
				txtTargetDirectory.Text = parms.APODDir;
			else 
				txtTargetDirectory.Text = parms.APODDirAlt;
			dtPickerFrom.Value = parms.NextDateToDownload;
			// TODO: Add APODURL support

		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			string		URL, doc, ImageURL, title;
			Regex		reTitle, reSmallImage, reBigImage;
			Match		m;
			DateTime	dtFrom = dtPickerFrom.Value;
			DateTime	dtTo	= dtPickerTo.Value;
			dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day);
			dtTo = new DateTime(dtTo.Year, dtTo.Month, dtTo.Day);

			// Yeah, I might be able to combine the two regex's into one, but the strings
			// are complex enough as it is. We'll do it in two passes.
			reTitle = new Regex(@"\<title\>(?<Title>.+?)(\n*)\<\/title\>", 
				RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			reSmallImage = new Regex(@"\<\s*IMG\s+SRC\=""(?<URL>.+?)""", 
				RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			reBigImage = new Regex(@"\<a\s*href=""(?<IMG>image/)(?<URL>.+?)""", 
				RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			DateTime dt;
			for (dt = dtFrom; dt<=dtTo; dt=dt.AddDays(1)) {
				//Console.WriteLine("Current day = {0}", dt);
				lblProcDate.Text = "Processing " + dt.ToLongDateString();
				URL = string.Format("{0}ap{1:00}{2:00}{3:00}.html", APODURL.Text, 
					dt.Year % 100, dt.Month, dt.Day);
				StatMsg.Text = "About to read APOD URL " + URL;
				Application.DoEvents();
				web = new WebClient();
				try {
					// byte [] buffer = await web.DownloadDataAsync(new Uri(URL));
					byte [] buffer = web.DownloadData(new Uri(URL));
					doc = Encoding.ASCII.GetString(buffer);
					// Console.WriteLine("{0}", doc);
					m = reTitle.Match(doc);
					if (m.Success) {
						title = m.Groups["Title"].Captures[0].Value;
					} else {
						title = "APOD - " + dt.ToLongDateString();
					}
					m = reSmallImage.Match(doc);
					if (m.Success) {
						ImageURL = m.Groups["URL"].Captures[0].Value;
						StatMsg.Text = "About to write small APOD image " + ImageURL;
						Application.DoEvents();
						WriteImage(title, ImageURL, dt, true);
					}
					// Don't get large images if no "large" directory
					m = reBigImage.Match(doc);
					if (m.Success) {
						ImageURL  = m.Groups["IMG"].Captures[0].Value;
						ImageURL += m.Groups["URL"].Captures[0].Value;
						StatMsg.Text = "About to write big APOD image " + ImageURL;
						Application.DoEvents();
						WriteImage(title, ImageURL, dt, false);
					}
				} catch {}			// Silently ignore any errors
			}
			StatMsg.Text = "Done";

			parms.NextDateToDownload = dt;		// TODO: Use Math.Max to get the latest day
			try {
				Stream		s = File.OpenWrite(ParmsFilename);
				XmlSerializer	x = new XmlSerializer(typeof(GetAPODParms));
				x.Serialize(s, parms);
				s.Close();
			} catch (Exception ex) {
				MessageBox.Show("Unable to write config file " + ParmsFilename + ", error = " + ex.Message, "GetAPOD");
			}
		}

//---------------------------------------------------------------------------------------

		void WriteImage(string title, string ImageURL, DateTime dt, bool isSmall) {
			string		FileName = title.Trim() + ".jpg";
			string		dir = txtTargetDirectory.Text;

			dir = Path.Combine(dir, isSmall ? "Small" : "Large");

			// Don't process large images if there's no directory for them
			if ((! isSmall) && ! Directory.Exists(dir))
				return;

			// Note: You'd expect that we could use Path.InvalidPathChars
			//		 to filter out bad chars. But, for example, a colon certainly isn't
			//		 an invalid path character, but it can occur only as the second
			//		 character of a filename. So we might as well do our own explicit
			//		 checking.
			FileName = FileName.Replace(@":", "-");
			FileName = FileName.Replace(@"\", "-");
			FileName = FileName.Replace(@"/", "-");
			FileName = FileName.Replace(@"*", "-");
			FileName = FileName.Replace(@"?", "-");
			FileName = FileName.Replace(@"""", "'");
			try {
				if (! dir.EndsWith("\\"))
					dir += "\\";
				dir += string.Format(@"{0:yyyy\\yyyy-MM MMMM}\", dt);
				parms.LastDownloadDir = dir;
				Directory.CreateDirectory(dir);
				FileName = dir + FileName;

				web.DownloadFile(APODURL.Text + ImageURL, FileName);
			} catch (Exception ex) {
				MessageBox.Show("Error " + ex.Message + " writing image for file " + FileName);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnOpenAPOD_Click(object sender, System.EventArgs e) {
			// Process.Start(txtTargetDirectory.Text);
			Process.Start(parms.LastDownloadDir);
		}

//---------------------------------------------------------------------------------------
		
		private void btnOpenAPODURL_Click(object sender, System.EventArgs e) {
			Process.Start(APODURL.Text);
		}

//---------------------------------------------------------------------------------------

		#region Dispose, Main, and Windows Form Designer stuff
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
			this.label1 = new System.Windows.Forms.Label();
			this.dtPickerFrom = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTargetDirectory = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.dtPickerTo = new System.Windows.Forms.DateTimePicker();
			this.label4 = new System.Windows.Forms.Label();
			this.APODURL = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.StatMsg = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.lblProcDate = new System.Windows.Forms.Label();
			this.btnOpenAPODDir = new System.Windows.Forms.Button();
			this.btnOpenAPODURL = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "From Date";
			// 
			// dtPickerFrom
			// 
			this.dtPickerFrom.Location = new System.Drawing.Point(72, 16);
			this.dtPickerFrom.Name = "dtPickerFrom";
			this.dtPickerFrom.Size = new System.Drawing.Size(192, 20);
			this.dtPickerFrom.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Target Directory";
			// 
			// txtTargetDirectory
			// 
			this.txtTargetDirectory.Location = new System.Drawing.Point(136, 64);
			this.txtTargetDirectory.Name = "txtTargetDirectory";
			this.txtTargetDirectory.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtTargetDirectory.Size = new System.Drawing.Size(304, 20);
			this.txtTargetDirectory.TabIndex = 5;
			this.txtTargetDirectory.Text = "G:\\APOD";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(448, 64);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(96, 24);
			this.btnBrowse.TabIndex = 6;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.Visible = false;
			// 
			// dtPickerTo
			// 
			this.dtPickerTo.Location = new System.Drawing.Point(336, 16);
			this.dtPickerTo.Name = "dtPickerTo";
			this.dtPickerTo.Size = new System.Drawing.Size(192, 20);
			this.dtPickerTo.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(280, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 32);
			this.label4.TabIndex = 7;
			this.label4.Text = "To Date";
			// 
			// APODURL
			// 
			this.APODURL.Location = new System.Drawing.Point(136, 104);
			this.APODURL.Name = "APODURL";
			this.APODURL.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.APODURL.Size = new System.Drawing.Size(304, 20);
			this.APODURL.TabIndex = 10;
			this.APODURL.Text = "http://antwrp.gsfc.nasa.gov/apod/";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 24);
			this.label2.TabIndex = 9;
			this.label2.Text = "APOD URL";
			// 
			// StatMsg
			// 
			this.StatMsg.Location = new System.Drawing.Point(16, 160);
			this.StatMsg.Name = "StatMsg";
			this.StatMsg.Size = new System.Drawing.Size(528, 16);
			this.StatMsg.TabIndex = 11;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(16, 192);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(56, 24);
			this.btnGo.TabIndex = 12;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(96, 192);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(56, 24);
			this.btnStop.TabIndex = 13;
			this.btnStop.Text = "Stop";
			this.btnStop.Visible = false;
			// 
			// lblProcDate
			// 
			this.lblProcDate.Location = new System.Drawing.Point(16, 136);
			this.lblProcDate.Name = "lblProcDate";
			this.lblProcDate.Size = new System.Drawing.Size(528, 16);
			this.lblProcDate.TabIndex = 14;
			// 
			// btnOpenAPODDir
			// 
			this.btnOpenAPODDir.Location = new System.Drawing.Point(240, 192);
			this.btnOpenAPODDir.Name = "btnOpenAPODDir";
			this.btnOpenAPODDir.Size = new System.Drawing.Size(144, 24);
			this.btnOpenAPODDir.TabIndex = 15;
			this.btnOpenAPODDir.Text = "Open APOD Dir";
			this.btnOpenAPODDir.Click += new System.EventHandler(this.btnOpenAPOD_Click);
			// 
			// btnOpenAPODURL
			// 
			this.btnOpenAPODURL.Location = new System.Drawing.Point(400, 192);
			this.btnOpenAPODURL.Name = "btnOpenAPODURL";
			this.btnOpenAPODURL.Size = new System.Drawing.Size(144, 24);
			this.btnOpenAPODURL.TabIndex = 16;
			this.btnOpenAPODURL.Text = "Open APOD URL";
			this.btnOpenAPODURL.Click += new System.EventHandler(this.btnOpenAPODURL_Click);
			// 
			// GetAPOD
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 230);
			this.Controls.Add(this.btnOpenAPODURL);
			this.Controls.Add(this.btnOpenAPODDir);
			this.Controls.Add(this.lblProcDate);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.StatMsg);
			this.Controls.Add(this.APODURL);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dtPickerTo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtTargetDirectory);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.dtPickerFrom);
			this.Controls.Add(this.label1);
			this.Name = "GetAPOD";
			this.Text = "Get APOD";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new GetAPOD());
		}
		#endregion

	}

	[Serializable]
	public class GetAPODParms {
		public DateTime		NextDateToDownload;
		public string		APODDir;
		public string		APODDirAlt;
		public string		APODURL;
		public string		LastDownloadDir;

		public GetAPODParms() {
			NextDateToDownload	= DateTime.Now;
			APODDir				= "D:\\APOD";
			APODDirAlt			= "C:\\APOD";
			APODURL				= "http://apod.nasa.gov/apod/";
			LastDownloadDir		= APODDir;
		}
	}
}
