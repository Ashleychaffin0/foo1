using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LeadsLightningMockup {
	/// <summary>
	/// Summary description for WelcomeEX.
	/// </summary>
	public class EXWelcome : PseudoWebPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox lbEvents;
		private System.Windows.Forms.DataGrid dgEvents;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lblEvents;
		private System.Windows.Forms.Label lblLeads;
		private System.Windows.Forms.Label lblLastUpload;
		private System.Windows.Forms.Button btnViewReports;
		private System.Windows.Forms.Button btnImportData;
		private System.Windows.Forms.Button btnSendEmails;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label9;

		DataTable	dt = new DataTable();

		public EXWelcome(Panel page) : base(page) {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

			lblEvents.Text = "3";
			lblLeads.Text  = "294";
			lblLastUpload.Text = "PC Expo - Sept 3, 2004, 5:23 PM";

			lbEvents.Items.Add("PC Expo | Sept 2. 2004");
			lbEvents.Items.Add("Comdex | Aug 23. 2004");
			lbEvents.Items.Add("PC Expo | July 12. 2003");

			dt.Columns.Add("Event");
			dt.Columns.Add("Date");
			dt.Rows.Add(new object[] {"PC Expo", "Sept 2. 2004"});
			dt.Rows.Add(new object[] {"Comdex",  "Aug 23. 2004"});
			dt.Rows.Add(new object[] {"PC Expo", "July 12. 2003"});
			dgEvents.DataSource = dt;

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnViewReports = new System.Windows.Forms.Button();
			this.btnImportData = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lbEvents = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSendEmails = new System.Windows.Forms.Button();
			this.dgEvents = new System.Windows.Forms.DataGrid();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.lblEvents = new System.Windows.Forms.Label();
			this.lblLeads = new System.Windows.Forms.Label();
			this.lblLastUpload = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgEvents)).BeginInit();
			this.SuspendLayout();
			// 
			// btnViewReports
			// 
			this.btnViewReports.Location = new System.Drawing.Point(208, 376);
			this.btnViewReports.Name = "btnViewReports";
			this.btnViewReports.Size = new System.Drawing.Size(152, 24);
			this.btnViewReports.TabIndex = 5;
			this.btnViewReports.Text = "View Reports";
			this.btnViewReports.Click += new System.EventHandler(this.btnViewReports_Click);
			// 
			// btnImportData
			// 
			this.btnImportData.Location = new System.Drawing.Point(32, 376);
			this.btnImportData.Name = "btnImportData";
			this.btnImportData.Size = new System.Drawing.Size(152, 24);
			this.btnImportData.TabIndex = 4;
			this.btnImportData.Text = "Import Data";
			this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "Welcome, Exhibitor[your name here]";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(544, 48);
			this.label3.TabIndex = 7;
			this.label3.Text = "Please select one or more events to process. Click an event to select it, and use" +
				" Ctrl-Click to add/remove further events";
			// 
			// lbEvents
			// 
			this.lbEvents.ItemHeight = 16;
			this.lbEvents.Location = new System.Drawing.Point(8, 184);
			this.lbEvents.Name = "lbEvents";
			this.lbEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbEvents.Size = new System.Drawing.Size(232, 132);
			this.lbEvents.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 336);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(216, 24);
			this.label4.TabIndex = 9;
			this.label4.Text = "What do you want to do next?";
			// 
			// btnSendEmails
			// 
			this.btnSendEmails.Location = new System.Drawing.Point(384, 376);
			this.btnSendEmails.Name = "btnSendEmails";
			this.btnSendEmails.Size = new System.Drawing.Size(152, 23);
			this.btnSendEmails.TabIndex = 0;
			this.btnSendEmails.Text = "Send Emails";
			this.btnSendEmails.Click += new System.EventHandler(this.btnSendEmails_Click);
			// 
			// dgEvents
			// 
			this.dgEvents.DataMember = "";
			this.dgEvents.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgEvents.Location = new System.Drawing.Point(264, 184);
			this.dgEvents.Name = "dgEvents";
			this.dgEvents.ReadOnly = true;
			this.dgEvents.Size = new System.Drawing.Size(224, 132);
			this.dgEvents.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(536, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Summary [Whatever info we want to show]";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "Events";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(104, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 24);
			this.label7.TabIndex = 14;
			this.label7.Text = "Leads";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(216, 64);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(216, 24);
			this.label8.TabIndex = 15;
			this.label8.Text = "Last Upload";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEvents
			// 
			this.lblEvents.Location = new System.Drawing.Point(8, 96);
			this.lblEvents.Name = "lblEvents";
			this.lblEvents.Size = new System.Drawing.Size(72, 16);
			this.lblEvents.TabIndex = 16;
			this.lblEvents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblLeads
			// 
			this.lblLeads.Location = new System.Drawing.Point(104, 96);
			this.lblLeads.Name = "lblLeads";
			this.lblLeads.Size = new System.Drawing.Size(72, 16);
			this.lblLeads.TabIndex = 17;
			this.lblLeads.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblLastUpload
			// 
			this.lblLastUpload.Location = new System.Drawing.Point(216, 96);
			this.lblLastUpload.Name = "lblLastUpload";
			this.lblLastUpload.Size = new System.Drawing.Size(328, 16);
			this.lblLastUpload.TabIndex = 18;
			this.lblLastUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(264, 336);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(216, 24);
			this.label2.TabIndex = 19;
			this.label2.Text = "[Either LB or Grid, not sure yet]";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(32, 416);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(432, 24);
			this.label9.TabIndex = 20;
			this.label9.Text = "Must disable Import Data button if more than one Event selected???";
			// 
			// EXWelcome
			// 
			this.BackColor = System.Drawing.Color.BlanchedAlmond;
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblLastUpload);
			this.Controls.Add(this.lblLeads);
			this.Controls.Add(this.lblEvents);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.dgEvents);
			this.Controls.Add(this.btnSendEmails);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbEvents);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnViewReports);
			this.Controls.Add(this.btnImportData);
			this.Controls.Add(this.label1);
			this.Name = "EXWelcome";
			this.Size = new System.Drawing.Size(560, 472);
			((System.ComponentModel.ISupportInitialize)(this.dgEvents)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnImportData_Click(object sender, System.EventArgs e) {
			new EX.EXImportData(base.ParentPanel);
		}

		private void btnViewReports_Click(object sender, System.EventArgs e) {
			new EX.EXViewReports(base.ParentPanel);
		}

		private void btnSendEmails_Click(object sender, System.EventArgs e) {
			new EX.EXSendEmail(base.ParentPanel);
		}



	}
}
