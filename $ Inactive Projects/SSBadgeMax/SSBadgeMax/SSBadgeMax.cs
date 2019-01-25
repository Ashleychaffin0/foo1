// Copyright (c) 2005 Bartizan Connects LLC

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

using mshtml;

namespace SSBadgeMax {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SSBadgeMax : System.Windows.Forms.Form {
		private AxSHDocVw.AxWebBrowser web;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.StatusBar StatMsg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		const string		ParmFileName = "SSBadgeMax.xml";
		HTMLDocumentClass	doc;

		public SSBadgeMaxParms	parms;

		Regex				reConfirmationPage;

		public SSBadgeMax() {
			InitializeComponent();	// Required for Windows Form Designer support

			Init();
		}

//---------------------------------------------------------------------------------------

		private void btnNew_Click(object sender, System.EventArgs e) {
			Navigate(parms.DefaultURL);
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			string	URL = txtURL.Text.Trim();
			if (URL.Length == 0) {
				string	title = "SSBadgeMax";
				MessageBox.Show("Please enter a URL to go to", title, 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				// TODO: get <title> from customization file
				return;
			}
			Navigate(URL);
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e) {
			AxSHDocVw.AxWebBrowser web2 = (AxSHDocVw.AxWebBrowser)sender;	
			doc = web2.Document as HTMLDocumentClass;
			bool isFinalFrame = e.pDisp == web.Application;
			if (isFinalFrame) {
				StatMsg.Text = "Navigation complete - " + e.uRL;
				CheckForOurPages();
			}
		}

//---------------------------------------------------------------------------------------

		private void CheckForOurPages() {
			Uri		uri = new Uri(web.LocationURL);
			switch (uri.Host) {
			case "www.signup4.com":
			case "www.signup4.net":
				CheckSignup4();
				break;
#if false
			case "www.microsoft.com":
				CheckMicrosoft();
				break;
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private void CheckSignup4() {
			// Note: This routine doesn't take into account frames. If this support
			//		 is needed, see WIALink.BartIE.FindDocumentBySignature
#if true
			string s = doc.body.innerHTML;
			Match m = reConfirmationPage.Match(s);
			if (m.Success) {
				ProcessSignUp4ConfirmationPage(m);
				return;
			}
			// Not found in main page. Check frames.
			FramesCollection	frames = doc.frames;
			HTMLWindow2Class	w2c;
			object				oi;
			// The frames collection doesn't support IEnumerable. Do things the old
			// fashioned way.
			for (int i=0; i<frames.length; ++i) {
				oi = i;
				w2c = (HTMLWindow2Class)frames.item(ref oi);
				s = w2c.document.body.innerHTML;
				m = reConfirmationPage.Match(s);
				if (m.Success)
					break;
			}
			if (m.Success)
				ProcessSignUp4ConfirmationPage(m);
			
#else
			string s = doc.body.innerText;
			// Look for specific text
			if (s.IndexOf("Welcome to the SignUp4 Login Page...") < 0)
				return;
			if (doc.getElementById("Company") == null) 
				return;
#endif
		}

//---------------------------------------------------------------------------------------

		private void ProcessSignUp4ConfirmationPage(Match m) {
			string ConfNum = m.Groups["ConfirmationNumber"].Value;
			MessageBox.Show("Found '" + ConfNum + "\' in Confirmation Page");
		}

#if false	// Sample code from WIALink
//---------------------------------------------------------------------------------------

		public void dbgDumpFrameInfo(HTMLDocumentClass doc) {
			FramesCollection	frames = doc.frames;
			HTMLWindow2Class	w2c;
			object				oi;
			for (int i=0; i<frames.length; ++i) {
				oi = i;
				w2c = (HTMLWindow2Class)frames.item(ref oi);
				dbgTrace(String.Format("Frame[{0}] - name = {1}", i, w2c.frameElement.name));
				dbgDumpDocFieldIDs((HTMLDocumentClass)w2c.document, i);
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private void Init() {
			Trace.Listeners.Add(new TextWriterTraceListener("SSBadgeMax.txt"));
			Trace.AutoFlush = true;

			LoadParms();

			this.Text   = parms.WindowCaption;
			txtURL.Text = parms.DefaultURL;
			Navigate(parms.DefaultURL);

			string	pat;
#if true
			pat = @"Confirmation Number\:</b>\s*" 
				+ @"(?<ConfirmationNumber>.{10})" 
				+ @"<br><b>Status\:</b>\s*Registered";
#else		// Matches on login page
			pat = @"Welcome to the\s*"
				+ @"(?<ConfirmationNumber>.{7})" 
				+ @"\s*Login Page";
#endif
			reConfirmationPage = new Regex(pat, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

//---------------------------------------------------------------------------------------

		private void LoadParms() {
			if (File.Exists(ParmFileName)) {
				Stream	s = null;
				try {
					s = File.OpenRead(ParmFileName);
					XmlSerializer	x = new XmlSerializer(typeof(SSBadgeMaxParms));
					parms = (SSBadgeMaxParms) x.Deserialize(s);
					s.Close();
				} catch (Exception ex) {
					MessageBox.Show("Unable to read config file " + ParmFileName + ", error = " + ex.Message, "GetAPOD");
					parms = new SSBadgeMaxParms();
				} finally {
					if (s != null)
						s.Close();
				}
			} else
				parms = new SSBadgeMaxParms();

		}

//---------------------------------------------------------------------------------------

		private void SaveParms() {
			Stream		s = null;
			try {
				s = File.OpenWrite(ParmFileName);
				XmlSerializer	x = new XmlSerializer(typeof(SSBadgeMaxParms));
				x.Serialize(s, parms);
				s.Close();
			} catch (Exception ex) {
				MessageBox.Show("Unable to write config file " + ParmFileName + ", error = " + ex.Message, "GetAPOD");
			} finally {
				if (s != null)
					s.Close();
			}
		}

//---------------------------------------------------------------------------------------

		public void Navigate(string URL) {
			object	zero = 0;
			object	nl	 = "";
			web.Navigate(URL, ref zero, ref nl, ref nl, ref nl);
		}

//---------------------------------------------------------------------------------------

		private void SSBadgeMax_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			SaveParms();
			Trace.Close();
		}

//---------------------------------------------------------------------------------------

		private void txtURL_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyValue == 13) {			// Enter key
				e.Handled = true;
				Navigate(txtURL.Text);
				return;
			}
		}

//---------------------------------------------------------------------------------------

		private void web_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e) {
			StatMsg.Text = "Navigating to " + e.uRL;
		}

//---------------------------------------------------------------------------------------

#if false		// CheckMicrosoft()
		private void CheckMicrosoft() {
			if (doc.title == "Microsoft Windows Vista") {
				MessageBox.Show("Vista found!");
			}
		}
#endif

//---------------------------------------------------------------------------------------

		#region Windows Form Designer generated code
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

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SSBadgeMax));
			this.web = new AxSHDocVw.AxWebBrowser();
			this.btnNew = new System.Windows.Forms.Button();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.StatMsg = new System.Windows.Forms.StatusBar();
			((System.ComponentModel.ISupportInitialize)(this.web)).BeginInit();
			this.SuspendLayout();
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Enabled = true;
			this.web.Location = new System.Drawing.Point(8, 32);
			this.web.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("web.OcxState")));
			this.web.Size = new System.Drawing.Size(656, 428);
			this.web.TabIndex = 0;
			this.web.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.web_DocumentComplete);
			this.web.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.web_BeforeNavigate2);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(8, 0);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(56, 24);
			this.btnNew.TabIndex = 1;
			this.btnNew.Text = "New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(72, 0);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(528, 22);
			this.txtURL.TabIndex = 2;
			this.txtURL.Text = "";
			this.txtURL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtURL_KeyUp);
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(608, 0);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(48, 24);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// StatMsg
			// 
			this.StatMsg.Location = new System.Drawing.Point(0, 440);
			this.StatMsg.Name = "StatMsg";
			this.StatMsg.Size = new System.Drawing.Size(664, 24);
			this.StatMsg.TabIndex = 4;
			// 
			// SSBadgeMax
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(664, 464);
			this.Controls.Add(this.StatMsg);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.web);
			this.Name = "SSBadgeMax";
			this.Text = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SSBadgeMax_Closing);
			((System.ComponentModel.ISupportInitialize)(this.web)).EndInit();
			this.ResumeLayout(false);

		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new SSBadgeMax());
		}
		#endregion
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	[Serializable]
	public class SSBadgeMaxParms {
		public string		WindowCaption;
		public string		DefaultURL;

//---------------------------------------------------------------------------------------

		public SSBadgeMaxParms() {
			// This code will be effective only if the XML file is missing or can't load.
			// So don't panic if this isn't for Signup4 etc
			WindowCaption = "BadgeMax";
			DefaultURL	  = "http://www.bartizan.com";
		}
	}
}
