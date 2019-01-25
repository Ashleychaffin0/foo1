namespace ShowDecompressImportData {
	partial class ShowDecompressImportData {
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtSavedImportID = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.rtbVisitor = new System.Windows.Forms.RichTextBox();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Saved Import ID";
			// 
			// txtSavedImportID
			// 
			this.txtSavedImportID.Location = new System.Drawing.Point(116, 24);
			this.txtSavedImportID.Name = "txtSavedImportID";
			this.txtSavedImportID.Size = new System.Drawing.Size(100, 20);
			this.txtSavedImportID.TabIndex = 1;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(286, 20);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// rtbVisitor
			// 
			this.rtbVisitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rtbVisitor.Location = new System.Drawing.Point(12, 78);
			this.rtbVisitor.Name = "rtbVisitor";
			this.rtbVisitor.Size = new System.Drawing.Size(823, 174);
			this.rtbVisitor.TabIndex = 3;
			this.rtbVisitor.Text = "";
			this.rtbVisitor.WordWrap = false;
			// 
			// btnPrevious
			// 
			this.btnPrevious.Location = new System.Drawing.Point(16, 49);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(75, 23);
			this.btnPrevious.TabIndex = 4;
			this.btnPrevious.Text = "Previous";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(116, 49);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 5;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// ShowDecompressImportData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(847, 264);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnPrevious);
			this.Controls.Add(this.rtbVisitor);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtSavedImportID);
			this.Controls.Add(this.label1);
			this.Name = "ShowDecompressImportData";
			this.Text = "Show Decompress Import Data";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSavedImportID;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.RichTextBox rtbVisitor;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnNext;
	}
}

