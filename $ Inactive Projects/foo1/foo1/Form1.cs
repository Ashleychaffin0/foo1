using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace foo1	{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		static string[] ZuneBaseFiles = new string[] {
						"UIX.dll",
						"UIX.RenderApi.dll",
						"UIXControls.dll",
						"ZuneCfg.dll",
						"ZuneDB.dll",
						"ZuneDBApi.dll",
						"ZuneNativeLib.dll",
						"ZuneQP.dll",
						"ZuneResources.dll",
						"ZuneSE.dll",
						"ZuneShell.dll",
				};

		static string[] Zune_en_USFiles = new string[] {
						"ZuneNss.exe.mui",
						"ZuneResources.dll.mui",
						"ZuneSetup.exe.mui",
				};

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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}
	}
}
