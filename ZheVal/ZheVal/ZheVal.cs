using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ZheVal {

	
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ZheVal : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btnValidate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ZheVal() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void btnValidate_Click(object sender, System.EventArgs e) {
			Validate val = new Validate();
			val.ShowDialog();
		}

		#region Uninteresting stuff
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
			this.btnValidate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnValidate
			// 
			this.btnValidate.Location = new System.Drawing.Point(16, 32);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.Size = new System.Drawing.Size(72, 24);
			this.btnValidate.TabIndex = 0;
			this.btnValidate.Text = "Validate";
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// ZheVal
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(292, 260);
			this.Controls.Add(this.btnValidate);
			this.Name = "ZheVal";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new ZheVal());
		}
		#endregion
	}
}
