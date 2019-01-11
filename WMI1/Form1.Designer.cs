namespace WMI1 {
	partial class Form1 {
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
			this.classList = new System.Windows.Forms.ListBox();
			this.namespaceValue = new System.Windows.Forms.TextBox();
			this.statusValue = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.searchButton = new System.Windows.Forms.Button();
			this.txtInstance = new System.Windows.Forms.TextBox();
			this.statusValue.SuspendLayout();
			this.SuspendLayout();
			// 
			// classList
			// 
			this.classList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.classList.FormattingEnabled = true;
			this.classList.Location = new System.Drawing.Point(12, 40);
			this.classList.Name = "classList";
			this.classList.Size = new System.Drawing.Size(496, 186);
			this.classList.TabIndex = 0;
			this.classList.SelectedIndexChanged += new System.EventHandler(this.classList_SelectedIndexChanged);
			// 
			// namespaceValue
			// 
			this.namespaceValue.Location = new System.Drawing.Point(12, 13);
			this.namespaceValue.Name = "namespaceValue";
			this.namespaceValue.Size = new System.Drawing.Size(175, 20);
			this.namespaceValue.TabIndex = 1;
			// 
			// statusValue
			// 
			this.statusValue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusValue.Location = new System.Drawing.Point(0, 242);
			this.statusValue.Name = "statusValue";
			this.statusValue.Size = new System.Drawing.Size(520, 22);
			this.statusValue.TabIndex = 2;
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(196, 9);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(75, 23);
			this.searchButton.TabIndex = 3;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// txtInstance
			// 
			this.txtInstance.Location = new System.Drawing.Point(289, 12);
			this.txtInstance.Name = "txtInstance";
			this.txtInstance.Size = new System.Drawing.Size(175, 20);
			this.txtInstance.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(520, 264);
			this.Controls.Add(this.txtInstance);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.statusValue);
			this.Controls.Add(this.namespaceValue);
			this.Controls.Add(this.classList);
			this.Name = "Form1";
			this.Text = "Form1";
			this.statusValue.ResumeLayout(false);
			this.statusValue.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox classList;
		private System.Windows.Forms.TextBox namespaceValue;
		private System.Windows.Forms.StatusStrip statusValue;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.TextBox txtInstance;
	}
}

