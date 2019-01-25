using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using LeadsLightningMockup.BART;

namespace LeadsLightningMockup {
	/// <summary>
	/// Summary description for WelcomeBart.
	/// </summary>
	public class BartWelcome : PseudoWebPage {
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BartWelcome(Panel page) : base(page) {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
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
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 96);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(152, 24);
			this.button2.TabIndex = 8;
			this.button2.Text = "Define Event";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 56);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 24);
			this.button1.TabIndex = 7;
			this.button1.Text = "Register Exhibitor ->";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 24);
			this.label1.TabIndex = 6;
			this.label1.Text = "Welcome, Bartizan[your name here]";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(8, 136);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(152, 24);
			this.button3.TabIndex = 9;
			this.button3.Text = "Define RC";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(184, 136);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(152, 24);
			this.button4.TabIndex = 12;
			this.button4.Text = "Display RC Info";
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(184, 96);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(152, 24);
			this.button5.TabIndex = 11;
			this.button5.Text = "Display Exhibitor Info";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(184, 56);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(152, 24);
			this.button6.TabIndex = 10;
			this.button6.Text = "Display Exhibitor Info";
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(88, 184);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(176, 24);
			this.button7.TabIndex = 13;
			this.button7.Text = "Export Accounting Data";
			// 
			// WelcomeBart
			// 
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "WelcomeBart";
			this.Size = new System.Drawing.Size(376, 264);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e) {
			new RegisterExhibitor(base.ParentPanel);
		}
	}
}
