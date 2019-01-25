namespace ClispEd {
	partial class ClispEd {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.scintilla = new ScintillaNET.Scintilla();
			((System.ComponentModel.ISupportInitialize)(this.scintilla)).BeginInit();
			this.SuspendLayout();
			// 
			// scintilla
			// 
			this.scintilla.Location = new System.Drawing.Point(62, 82);
			this.scintilla.Name = "scintilla2";
			this.scintilla.Size = new System.Drawing.Size(200, 100);
			this.scintilla.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
			this.scintilla.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
			this.scintilla.TabIndex = 0;
			// 
			// ClispEd
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.scintilla);
			this.Name = "ClispEd";
			((System.ComponentModel.ISupportInitialize)(this.scintilla)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private ScintillaNET.Scintilla scintilla1;
		private ScintillaNET.Scintilla scintilla;
	}
}

