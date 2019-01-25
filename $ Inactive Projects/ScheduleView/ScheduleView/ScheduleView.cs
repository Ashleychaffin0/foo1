using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using LRS;

namespace ScheduleView {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ScheduleView : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btnOpenExcel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CS2Excel	xl;

		public ScheduleView() {
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
			this.btnOpenExcel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnOpenExcel
			// 
			this.btnOpenExcel.Location = new System.Drawing.Point(24, 24);
			this.btnOpenExcel.Name = "btnOpenExcel";
			this.btnOpenExcel.Size = new System.Drawing.Size(104, 40);
			this.btnOpenExcel.TabIndex = 0;
			this.btnOpenExcel.Text = "Open Excel";
			this.btnOpenExcel.Click += new System.EventHandler(this.btnOpenExcel_Click);
			// 
			// ScheduleView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(292, 260);
			this.Controls.Add(this.btnOpenExcel);
			this.Name = "ScheduleView";
			this.Text = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ScheduleView_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new ScheduleView());
		}

		private void btnOpenExcel_Click(object sender, System.EventArgs e) {
			xl = new CS2Excel(@"C:\LRS\AttendCardTest.xls");
			xl.Show(true);
		}

		private void ScheduleView_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (xl != null)
				xl.Dispose();
		}
	}
}
