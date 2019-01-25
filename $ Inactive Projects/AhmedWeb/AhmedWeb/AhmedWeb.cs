using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SHDocVw;
using mshtml;

namespace AhmedWeb {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private AxSHDocVw.AxWebBrowser web;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Label lblStatMsg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		HTMLDocumentClass	doc;

		public MainForm()
		{
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.web = new AxSHDocVw.AxWebBrowser();
			this.button1 = new System.Windows.Forms.Button();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnBack = new System.Windows.Forms.Button();
			this.lblStatMsg = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.web)).BeginInit();
			this.SuspendLayout();
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Enabled = true;
			this.web.Location = new System.Drawing.Point(8, 64);
			this.web.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("web.OcxState")));
			this.web.Size = new System.Drawing.Size(616, 424);
			this.web.TabIndex = 0;
			this.web.DownloadBegin += new System.EventHandler(this.web_DownloadBegin);
			this.web.DownloadComplete += new System.EventHandler(this.web_DownloadComplete);
			this.web.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.web_DocumentComplete);
			this.web.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.web_BeforeNavigate2);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(552, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 40);
			this.button1.TabIndex = 1;
			this.button1.Text = "Go";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(16, 8);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(528, 20);
			this.txtURL.TabIndex = 2;
			this.txtURL.Text = "www.ibm.com";
			// 
			// btnBack
			// 
			this.btnBack.Location = new System.Drawing.Point(464, 32);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(72, 24);
			this.btnBack.TabIndex = 3;
			this.btnBack.Text = "Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// lblStatMsg
			// 
			this.lblStatMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatMsg.Location = new System.Drawing.Point(16, 496);
			this.lblStatMsg.Name = "lblStatMsg";
			this.lblStatMsg.Size = new System.Drawing.Size(600, 24);
			this.lblStatMsg.TabIndex = 4;
			this.lblStatMsg.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 518);
			this.Controls.Add(this.lblStatMsg);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.web);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.web)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

//---------------------------------------------------------------------------------------

		public void Navigate(string URL, ref object flags, ref object targetFrameName, 
			ref object postData, ref object headers) {
			object	oURL = URL;
			web.Navigate2(ref oURL, ref flags, ref targetFrameName, ref postData, ref headers);
		}

//---------------------------------------------------------------------------------------

		public void Navigate(string URL) {
			object	zero = 0;
			object	nl	 = "";
			Navigate(URL, ref zero, ref nl, ref nl, ref nl);
		}

//---------------------------------------------------------------------------------------

		private void button1_Click(object sender, System.EventArgs e) {
			Navigate(txtURL.Text);
		}

//---------------------------------------------------------------------------------------

		private void btnBack_Click(object sender, System.EventArgs e) {
			web.GoBack();
		}

//---------------------------------------------------------------------------------------

		private void web_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e) {
			lblStatMsg.Text = "BeforeNavigate2 to " + e.uRL;
		}

//---------------------------------------------------------------------------------------

		private void web_DownloadBegin(object sender, System.EventArgs e) {
			lblStatMsg.Text = "web_DownloadBegin";	
		}

//---------------------------------------------------------------------------------------

		private void web_DownloadComplete(object sender, System.EventArgs e) {
			lblStatMsg.Text = "web_DownloadComplete";	
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e) {
			lblStatMsg.Text = "web_DocumentComplete to " + e.uRL;	
			IWebBrowser2 web2 = (IWebBrowser2)e.pDisp;	// Could also just use <web> var
			doc = (HTMLDocumentClass)web2.Document;
			foreach (HTMLAnchorElement a in doc.anchors) {
				Console.WriteLine("<A href='{0}' {1}</A>", a.href, a.innerText);
			}
		}
	}
}
