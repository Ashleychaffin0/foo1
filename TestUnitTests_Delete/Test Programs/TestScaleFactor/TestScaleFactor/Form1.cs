using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TestScaleFactor {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btnAddLabel1;
		private System.Windows.Forms.Button btnAddLabel2;
		private System.Windows.Forms.Button btnZF1;
		private System.Windows.Forms.Button btnZF2;
		private System.Windows.Forms.Button btnZF3;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		float	_MyScale = 1.0f;
		float MyScale {
			get { return _MyScale; }
			set { 
				// Note: The exact order of these lines is crucial
				this.Scale(value / _MyScale);
				_MyScale = value;
			}
		}

		public Form1() {
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
			this.btnAddLabel1 = new System.Windows.Forms.Button();
			this.btnAddLabel2 = new System.Windows.Forms.Button();
			this.btnZF1 = new System.Windows.Forms.Button();
			this.btnZF2 = new System.Windows.Forms.Button();
			this.btnZF3 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnAddLabel1
			// 
			this.btnAddLabel1.Location = new System.Drawing.Point(16, 136);
			this.btnAddLabel1.Name = "btnAddLabel1";
			this.btnAddLabel1.Size = new System.Drawing.Size(96, 23);
			this.btnAddLabel1.TabIndex = 1;
			this.btnAddLabel1.Text = "Add Label 1";
			// 
			// btnAddLabel2
			// 
			this.btnAddLabel2.Location = new System.Drawing.Point(16, 176);
			this.btnAddLabel2.Name = "btnAddLabel2";
			this.btnAddLabel2.Size = new System.Drawing.Size(96, 23);
			this.btnAddLabel2.TabIndex = 2;
			this.btnAddLabel2.Text = "Add Label 2";
			// 
			// btnZF1
			// 
			this.btnZF1.Location = new System.Drawing.Point(8, 8);
			this.btnZF1.Name = "btnZF1";
			this.btnZF1.Size = new System.Drawing.Size(120, 23);
			this.btnZF1.TabIndex = 3;
			this.btnZF1.Text = "Zoom Factor 1";
			this.btnZF1.Click += new System.EventHandler(this.btnZF1_Click);
			// 
			// btnZF2
			// 
			this.btnZF2.Location = new System.Drawing.Point(8, 48);
			this.btnZF2.Name = "btnZF2";
			this.btnZF2.Size = new System.Drawing.Size(120, 23);
			this.btnZF2.TabIndex = 4;
			this.btnZF2.Text = "Zoom Factor 2";
			this.btnZF2.Click += new System.EventHandler(this.btnZF2_Click);
			// 
			// btnZF3
			// 
			this.btnZF3.Location = new System.Drawing.Point(8, 88);
			this.btnZF3.Name = "btnZF3";
			this.btnZF3.Size = new System.Drawing.Size(120, 23);
			this.btnZF3.TabIndex = 5;
			this.btnZF3.Text = "Zoom Factor 3";
			this.btnZF3.Click += new System.EventHandler(this.btnZF3_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(176, 32);
			this.label1.Name = "label1";
			this.label1.TabIndex = 6;
			this.label1.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(312, 224);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnZF3);
			this.Controls.Add(this.btnZF2);
			this.Controls.Add(this.btnZF1);
			this.Controls.Add(this.btnAddLabel2);
			this.Controls.Add(this.btnAddLabel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		private void btnZF1_Click(object sender, System.EventArgs e) {
			MyScale = 1.0f;
		}

		private void btnZF2_Click(object sender, System.EventArgs e) {
			MyScale = 2.0f;
		}

		private void btnZF3_Click(object sender, System.EventArgs e) {
			MyScale = 3.0f;
		}
	}
}
