using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace LeadsLightningMockup {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LLMain : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnReports;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnEmail;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LLMain() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			new LLHomePage(panel1);
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
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnReports = new System.Windows.Forms.Button();
			this.btnImport = new System.Windows.Forms.Button();
			this.btnEmail = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(255)));
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(96, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(704, 46);
			this.label1.TabIndex = 0;
			this.label1.Text = "LeadsLightning Logo";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(96, 64);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(704, 416);
			this.panel1.TabIndex = 7;
			// 
			// btnBack
			// 
			this.btnBack.Location = new System.Drawing.Point(8, 8);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(80, 40);
			this.btnBack.TabIndex = 8;
			this.btnBack.Text = "Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// btnReports
			// 
			this.btnReports.Location = new System.Drawing.Point(16, 64);
			this.btnReports.Name = "btnReports";
			this.btnReports.Size = new System.Drawing.Size(72, 32);
			this.btnReports.TabIndex = 9;
			this.btnReports.Text = "Reports";
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(16, 112);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(72, 32);
			this.btnImport.TabIndex = 10;
			this.btnImport.Text = "Import";
			// 
			// btnEmail
			// 
			this.btnEmail.Location = new System.Drawing.Point(16, 160);
			this.btnEmail.Name = "btnEmail";
			this.btnEmail.Size = new System.Drawing.Size(72, 32);
			this.btnEmail.TabIndex = 11;
			this.btnEmail.Text = "Email";
			// 
			// LLMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(808, 496);
			this.Controls.Add(this.btnEmail);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.btnReports);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Name = "LLMain";
			this.Text = "Welcome to LeadsLightning";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new LLMain());
		}

		private void btnBack_Click(object sender, System.EventArgs e) {
			ArrayList hist = PseudoWebPage.History;
			if (hist.Count <= 1) {		// Don't go back past home page
				MessageBox.Show("Can't go back any further");
				return;
			}
			panel1.Controls.Clear();
			hist.RemoveAt(hist.Count - 1);
			PseudoWebPage CurPage = (PseudoWebPage)hist[hist.Count - 1];
			CurPage.Parent = panel1;
			// CurPage.Show();
		}
	}
}
