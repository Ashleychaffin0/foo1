using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TestIE
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class TestIE : System.Windows.Forms.Form
	{
		private AxSHDocVw.AxWebBrowser web;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnGo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestIE()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestIE));
			this.web = new AxSHDocVw.AxWebBrowser();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
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
			this.web.Size = new System.Drawing.Size(616, 388);
			this.web.TabIndex = 0;
			this.web.DownloadBegin += new System.EventHandler(this.web_DownloadBegin);
			this.web.CommandStateChange += new AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(this.web_CommandStateChange);
			this.web.DownloadComplete += new System.EventHandler(this.web_DownloadComplete);
			this.web.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.web_DocumentComplete);
			this.web.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(this.web_NewWindow2);
			this.web.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.web_BeforeNavigate2);
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(8, 0);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(304, 20);
			this.txtURL.TabIndex = 1;
			this.txtURL.Text = "https://ny.osos.ajb.org";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(552, 0);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(72, 24);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 430);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtURL);
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
			Application.Run(new TestIE());
		}

		private void web_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e) {
			Console.WriteLine("BeforeNavigate2 to {0}", e.uRL);
		}

		private void web_CommandStateChange(object sender, AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEvent e) {
			Console.WriteLine("CommandStateChange {0}", e.command);
		}

		private void web_DownloadBegin(object sender, System.EventArgs e) {
			Console.WriteLine("DownloadBegin");
		}

		private void web_DownloadComplete(object sender, System.EventArgs e) {
			Console.WriteLine("DownloadComplete");
		}

		private void web_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e) {
			Console.WriteLine("DocumentComplete - final = {0}", e.pDisp == web.Application);
		}

		private void web_NewWindow2(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e) {
			Console.WriteLine("NewWindow2 {0}", e.ToString());		
		}

//---------------------------------------------------------------------------------------

		public void Navigate(string URL, ref object flags, ref object targetFrameName, 
			ref object postData, ref object headers) {
			// dbgTrace("\n***** About to navigate to " + URL);
			// TODO: Navigate or Navigate2?
			// browser.Navigate(URL, ref flags, ref targetFrameName, ref postData, ref headers);
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

		private void btnGo_Click(object sender, System.EventArgs e) {
			Navigate(txtURL.Text);
		}
	}
}
