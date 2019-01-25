using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

// Remove these later
using System.Drawing.Printing;

namespace nsDemoBartCarte {
	/// <summary>
	/// Summary description for BartCarteDemo.
	/// </summary>
	public class BartCarteDemo_Server : System.Windows.Forms.Form 	{
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Button btnTestPrint;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BartCarteDemo_Server() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			LogMsg("About to configure BartCarteDemo server");
#if true
			RemotingConfiguration.Configure("BartCarteDemoConfig.xml");
#else
			TcpServerChannel	channel = new TcpServerChannel(1729);
			ChannelServices.RegisterChannel(channel);
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(BartCarteDemoPrinter),
					"BartCarteDemo.rem", WellKnownObjectMode.SingleCall);
#endif
			ShowActivatedServiceTypes();
			ShowWellKnownServiceTypes();
		}

		void ShowActivatedServiceTypes() {
			LogMsg("\nACTIVATED SERVICE TYPES\n");
			ActivatedServiceTypeEntry [] entries =
				RemotingConfiguration.GetRegisteredActivatedServiceTypes();
			foreach (ActivatedServiceTypeEntry entry in entries) {
				LogMsg("Assembly: " + entry.AssemblyName);
				LogMsg("Type:     " + entry.TypeName);
			}
		}

		void ShowWellKnownServiceTypes() {
			LogMsg("\nWELL KNOWN SERVICE TYPES\n");
			WellKnownServiceTypeEntry [] entries =
				RemotingConfiguration.GetRegisteredWellKnownServiceTypes();
			foreach (WellKnownServiceTypeEntry entry in entries) {
				LogMsg("Assembly: " + entry.AssemblyName);
				LogMsg("Mode:     " + entry.Mode);
				LogMsg("URI:      " + entry.ObjectUri);
				LogMsg("Type:     " + entry.TypeName);
			}
		}



		void LogMsg(string msg) {
			foreach (string s in msg.Split('\n'))
				lbMsgs.Items.Insert(0, s);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
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
		private void InitializeComponent()
		{
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.btnTestPrint = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.Location = new System.Drawing.Point(8, 40);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(432, 186);
			this.lbMsgs.TabIndex = 0;
			// 
			// btnTestPrint
			// 
			this.btnTestPrint.Location = new System.Drawing.Point(112, 8);
			this.btnTestPrint.Name = "btnTestPrint";
			this.btnTestPrint.Size = new System.Drawing.Size(104, 24);
			this.btnTestPrint.TabIndex = 1;
			this.btnTestPrint.Text = "Test Print";
			this.btnTestPrint.Click += new System.EventHandler(this.btnTestPrint_Click);
			// 
			// BartCarteDemo_Server
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 246);
			this.Controls.Add(this.btnTestPrint);
			this.Controls.Add(this.lbMsgs);
			this.Name = "BartCarteDemo_Server";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new BartCarteDemo_Server());
		}

		private void btnTestPrint_Click(object sender, System.EventArgs e) {
#if true
			PrintDocument pd = new PrintDocument();
			pd.DocumentName = "BartCarte";
			// pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-940";
			// pd.PrintController = new PreviewPrintController();
			PrintPreviewControl	pvc = new PrintPreviewControl();
			pvc.Document = pd;
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
#else
			MessageBox.Show("About to create PrintDocument in Server");
			PrintDocument pd = new PrintDocument();
			pd.DocumentName = "BartCarte";
			// pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-940";
			pd.PrintController = new PreviewPrintController();
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(this.printDocument1_PrintPage);
			MessageBox.Show("About to Print()");
			try {
				pd.Print();
			} catch (Exception ex) {
				MessageBox.Show("Exception in pd.Print() - " + ex.Message);
			}
#endif

		}

		private void printDocument1_PrintPage(object sender, 
			System.Drawing.Printing.PrintPageEventArgs e) {
			MessageBox.Show("In PrintPage() on Server");
			e.Graphics.DrawString("SampleLRS Text", 
				new Font("Arial", 32, FontStyle.Bold), Brushes.Black, 150, 125);
		}
	}
}
