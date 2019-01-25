// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Bartizan.ActivityTrack30;
using Bartizan.ActivityTrack30.Common;

namespace Bartizan.AT30 {

	/// <summary>
	/// Summary description for AT30.
	/// </summary>
	public class AT30MainForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btn_Import;
		private System.Windows.Forms.ListBox lbDbgMsgs;
		private System.Windows.Forms.Button btnTestLayout;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


//---------------------------------------------------------------------------------------

		public AT30MainForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

//---------------------------------------------------------------------------------------
		
		private void btn_Import_Click(object sender, System.EventArgs e) {
			string DirName = @"C:\Activity Track 2.0";

			// TODO: Shouldn't hardcode file/dir names
			Importer	imp = new Importer(@"C:\LRS\C#\AT30\ImportFieldDefs.xml");
			imp.ImportFromDirectory(DirName);
		}

//---------------------------------------------------------------------------------------

		private void btnTestLayout_Click(object sender, System.EventArgs e) {
			BartLayoutManager	lm = new BartLayoutManager();
		}

//---------------------------------------------------------------------------------------

#region Dispose, Main, and Windows Form Designer stuff
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null)	{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new AT30MainForm());
		}

//---------------------------------------------------------------------------------------

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btn_Import = new System.Windows.Forms.Button();
			this.lbDbgMsgs = new System.Windows.Forms.ListBox();
			this.btnTestLayout = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btn_Import
			// 
			this.btn_Import.Location = new System.Drawing.Point(8, 8);
			this.btn_Import.Name = "btn_Import";
			this.btn_Import.Size = new System.Drawing.Size(96, 32);
			this.btn_Import.TabIndex = 0;
			this.btn_Import.Text = "Import";
			this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
			// 
			// lbDbgMsgs
			// 
			this.lbDbgMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbDbgMsgs.Location = new System.Drawing.Point(8, 48);
			this.lbDbgMsgs.Name = "lbDbgMsgs";
			this.lbDbgMsgs.Size = new System.Drawing.Size(252, 277);
			this.lbDbgMsgs.TabIndex = 1;
			// 
			// btnTestLayout
			// 
			this.btnTestLayout.Location = new System.Drawing.Point(128, 8);
			this.btnTestLayout.Name = "btnTestLayout";
			this.btnTestLayout.Size = new System.Drawing.Size(96, 32);
			this.btnTestLayout.TabIndex = 2;
			this.btnTestLayout.Text = "Test Layout";
			this.btnTestLayout.Click += new System.EventHandler(this.btnTestLayout_Click);
			// 
			// AT30MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 334);
			this.Controls.Add(this.btnTestLayout);
			this.Controls.Add(this.lbDbgMsgs);
			this.Controls.Add(this.btn_Import);
			this.Name = "AT30MainForm";
			this.Text = "Activity Track 3.0";
			this.ResumeLayout(false);

		}
		#endregion


	}
	#endregion
}
