using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LeadsLightningMockup.EX {
	public class EXViewReports : LeadsLightningMockup.PseudoWebPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ComboBox cmbReports;
		private System.ComponentModel.IContainer components = null;

		public EXViewReports(Panel page) : base(page) {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

			cmbReports.Items.Add("Detailed Listing");
			cmbReports.Items.Add("Pie Chart");
			cmbReports.Items.Add("Summary");

			cmbReports.SelectedIndex = 0;
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
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbReports = new System.Windows.Forms.ComboBox();
			this.btnGo = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(192)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(512, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "View Reports";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 40);
			this.label2.TabIndex = 1;
			this.label2.Text = "You\'ve selected the following event(s)";
			// 
			// dataGrid1
			// 
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(184, 40);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(296, 96);
			this.dataGrid1.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 96);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 32);
			this.button1.TabIndex = 3;
			this.button1.Text = "Change event list";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Select a report";
			// 
			// cmbReports
			// 
			this.cmbReports.Location = new System.Drawing.Point(184, 152);
			this.cmbReports.Name = "cmbReports";
			this.cmbReports.Size = new System.Drawing.Size(296, 24);
			this.cmbReports.TabIndex = 5;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(496, 152);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(40, 32);
			this.btnGo.TabIndex = 6;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// EXViewReports
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(192)));
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.cmbReports);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "EXViewReports";
			this.Size = new System.Drawing.Size(544, 320);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnGo_Click(object sender, System.EventArgs e) {
			MessageBox.Show("Reports not functional in mockup");
		}
	}
}

