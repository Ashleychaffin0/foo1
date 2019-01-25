using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LeadsLightningMockup.EX {
	public class EXImportData : LeadsLightningMockup.PseudoWebPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label imgBBox;
		private System.Windows.Forms.Label imgLeads2Go;
		private System.Windows.Forms.Label lblMAPCFG;
		private System.Windows.Forms.TextBox txtMAPCFG;
		private System.Windows.Forms.Button btnBrowseMAPCFG;
		private System.Windows.Forms.Button btnBrowseVISITORTXT;
		private System.Windows.Forms.TextBox txtVISITORTXT;
		private System.Windows.Forms.Label lblVISITORTXT;
		private System.ComponentModel.IContainer components = null;

		public EXImportData(Panel page) : base(page) {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.lblMAPCFG = new System.Windows.Forms.Label();
			this.txtMAPCFG = new System.Windows.Forms.TextBox();
			this.btnBrowseMAPCFG = new System.Windows.Forms.Button();
			this.btnBrowseVISITORTXT = new System.Windows.Forms.Button();
			this.txtVISITORTXT = new System.Windows.Forms.TextBox();
			this.lblVISITORTXT = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.imgBBox = new System.Windows.Forms.Label();
			this.imgLeads2Go = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(504, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Import Data";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMAPCFG
			// 
			this.lblMAPCFG.Location = new System.Drawing.Point(8, 152);
			this.lblMAPCFG.Name = "lblMAPCFG";
			this.lblMAPCFG.Size = new System.Drawing.Size(152, 32);
			this.lblMAPCFG.TabIndex = 1;
			this.lblMAPCFG.Text = "MAP.CFG filename";
			this.lblMAPCFG.Visible = false;
			// 
			// txtMAPCFG
			// 
			this.txtMAPCFG.Location = new System.Drawing.Point(184, 152);
			this.txtMAPCFG.Name = "txtMAPCFG";
			this.txtMAPCFG.Size = new System.Drawing.Size(232, 22);
			this.txtMAPCFG.TabIndex = 2;
			this.txtMAPCFG.Text = "";
			this.txtMAPCFG.Visible = false;
			// 
			// btnBrowseMAPCFG
			// 
			this.btnBrowseMAPCFG.Location = new System.Drawing.Point(440, 152);
			this.btnBrowseMAPCFG.Name = "btnBrowseMAPCFG";
			this.btnBrowseMAPCFG.Size = new System.Drawing.Size(64, 24);
			this.btnBrowseMAPCFG.TabIndex = 3;
			this.btnBrowseMAPCFG.Text = "Browse";
			this.btnBrowseMAPCFG.Visible = false;
			// 
			// btnBrowseVISITORTXT
			// 
			this.btnBrowseVISITORTXT.Location = new System.Drawing.Point(440, 112);
			this.btnBrowseVISITORTXT.Name = "btnBrowseVISITORTXT";
			this.btnBrowseVISITORTXT.Size = new System.Drawing.Size(64, 24);
			this.btnBrowseVISITORTXT.TabIndex = 6;
			this.btnBrowseVISITORTXT.Text = "Browse";
			this.btnBrowseVISITORTXT.Visible = false;
			// 
			// txtVISITORTXT
			// 
			this.txtVISITORTXT.Location = new System.Drawing.Point(184, 112);
			this.txtVISITORTXT.Name = "txtVISITORTXT";
			this.txtVISITORTXT.Size = new System.Drawing.Size(232, 22);
			this.txtVISITORTXT.TabIndex = 5;
			this.txtVISITORTXT.Text = "";
			this.txtVISITORTXT.Visible = false;
			// 
			// lblVISITORTXT
			// 
			this.lblVISITORTXT.Location = new System.Drawing.Point(8, 112);
			this.lblVISITORTXT.Name = "lblVISITORTXT";
			this.lblVISITORTXT.Size = new System.Drawing.Size(152, 32);
			this.lblVISITORTXT.TabIndex = 4;
			this.lblVISITORTXT.Text = "VISITOR.TXT filename";
			this.lblVISITORTXT.Visible = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(16, 232);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(120, 24);
			this.button3.TabIndex = 7;
			this.button3.Text = "Import Data";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 280);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(504, 32);
			this.label4.TabIndex = 8;
			this.label4.Text = "Status messages about import go here";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(200, 32);
			this.label5.TabIndex = 9;
			this.label5.Text = "Where did your data come from?";
			// 
			// imgBBox
			// 
			this.imgBBox.Location = new System.Drawing.Point(216, 56);
			this.imgBBox.Name = "imgBBox";
			this.imgBBox.Size = new System.Drawing.Size(120, 32);
			this.imgBBox.TabIndex = 11;
			this.imgBBox.Text = "Image of BBox";
			this.imgBBox.Click += new System.EventHandler(this.imgBBox_Click);
			// 
			// imgLeads2Go
			// 
			this.imgLeads2Go.Location = new System.Drawing.Point(352, 56);
			this.imgLeads2Go.Name = "imgLeads2Go";
			this.imgLeads2Go.Size = new System.Drawing.Size(160, 32);
			this.imgLeads2Go.TabIndex = 12;
			this.imgLeads2Go.Text = "Image of Leads2Go";
			this.imgLeads2Go.Click += new System.EventHandler(this.imgLeads2Go_Click);
			// 
			// EXImportData
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(192)));
			this.Controls.Add(this.imgLeads2Go);
			this.Controls.Add(this.imgBBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.btnBrowseVISITORTXT);
			this.Controls.Add(this.txtVISITORTXT);
			this.Controls.Add(this.lblVISITORTXT);
			this.Controls.Add(this.btnBrowseMAPCFG);
			this.Controls.Add(this.txtMAPCFG);
			this.Controls.Add(this.lblMAPCFG);
			this.Controls.Add(this.label1);
			this.Name = "EXImportData";
			this.Size = new System.Drawing.Size(528, 336);
			this.ResumeLayout(false);

		}
		#endregion

		private void imgBBox_Click(object sender, System.EventArgs e) {
			imgBBox.BorderStyle = BorderStyle.Fixed3D;
			imgLeads2Go.BorderStyle = BorderStyle.None;
			ShowVISITORTTXT(true);
			ShowMAPCFG(true);
		}

		private void imgLeads2Go_Click(object sender, System.EventArgs e) {
			imgLeads2Go.BorderStyle = BorderStyle.Fixed3D;
			imgBBox.BorderStyle = BorderStyle.None;
			ShowVISITORTTXT(true);
			ShowMAPCFG(false);
		}

		void ShowMAPCFG(bool bShow) {
			lblMAPCFG.Visible = bShow;
			txtMAPCFG.Visible = bShow;
			btnBrowseMAPCFG.Visible = bShow;
		}

		void ShowVISITORTTXT(bool bShow) {
			lblVISITORTXT.Visible = bShow;
			txtVISITORTXT.Visible = bShow;
			btnBrowseVISITORTXT.Visible = bShow;
		}

	}
}

