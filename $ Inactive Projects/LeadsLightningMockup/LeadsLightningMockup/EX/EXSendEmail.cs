using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LeadsLightningMockup.EX {
	public class EXSendEmail : LeadsLightningMockup.PseudoWebPage {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbEmailTemplate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.ComponentModel.IContainer components = null;

		public EXSendEmail(Panel page) : base(page) {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

			cmbEmailTemplate.Items.Add("Thank you for visiting our booth");
			cmbEmailTemplate.Items.Add("How to increase your ROI");
			// In these next two, how exactly are we supposed to specify
			// the attachment(s) / survey?
			cmbEmailTemplate.Items.Add("Thank you with literature attachment - uh...");
			cmbEmailTemplate.Items.Add("Thank you with short survey - uh...");

			cmbEmailTemplate.SelectedIndex = 0;
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
			this.button1 = new System.Windows.Forms.Button();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbEmailTemplate = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(480, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "Send Email";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 120);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 32);
			this.button1.TabIndex = 6;
			this.button1.Text = "Change event list";
			// 
			// dataGrid1
			// 
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(184, 64);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(296, 96);
			this.dataGrid1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 40);
			this.label2.TabIndex = 4;
			this.label2.Text = "You\'ve selected the following event(s)";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 176);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 24);
			this.label3.TabIndex = 7;
			this.label3.Text = "Select an email to send";
			// 
			// cmbEmailTemplate
			// 
			this.cmbEmailTemplate.Location = new System.Drawing.Point(184, 176);
			this.cmbEmailTemplate.Name = "cmbEmailTemplate";
			this.cmbEmailTemplate.Size = new System.Drawing.Size(296, 24);
			this.cmbEmailTemplate.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 216);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(152, 24);
			this.label4.TabIndex = 9;
			this.label4.Text = "Select recipients:";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(16, 248);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(160, 32);
			this.checkBox1.TabIndex = 10;
			this.checkBox1.Text = "All Customers";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(184, 208);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(288, 32);
			this.label5.TabIndex = 11;
			this.label5.Text = "What else, such as a listbox to send by Title (Prez, VP, etc). And so on.";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 288);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(152, 32);
			this.button2.TabIndex = 12;
			this.button2.Text = "Send emails";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 328);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(472, 40);
			this.label6.TabIndex = 13;
			this.label6.Text = "Status msgs from Send";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(184, 288);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(240, 32);
			this.checkBox2.TabIndex = 14;
			this.checkBox2.Text = "Send test email just to yourself";
			// 
			// EXSendEmail
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(192)));
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmbEmailTemplate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "EXSendEmail";
			this.Size = new System.Drawing.Size(504, 400);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}

