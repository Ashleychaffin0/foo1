namespace ListDotNet {
	partial class ListDotNet {
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
			this.lbFrameworks = new System.Windows.Forms.ListBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// lbFrameworks
			// 
			this.lbFrameworks.FormattingEnabled = true;
			this.lbFrameworks.ItemHeight = 16;
			this.lbFrameworks.Location = new System.Drawing.Point(29, 12);
			this.lbFrameworks.Name = "lbFrameworks";
			this.lbFrameworks.Size = new System.Drawing.Size(237, 196);
			this.lbFrameworks.TabIndex = 0;
			this.lbFrameworks.DoubleClick += new System.EventHandler(this.lbFrameworks_DoubleClick);
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point(387, 28);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(646, 596);
			this.listBox1.TabIndex = 1;
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.Location = new System.Drawing.Point(29, 227);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(336, 397);
			this.treeView1.TabIndex = 2;
			this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
			// 
			// ListDotNet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1075, 649);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.lbFrameworks);
			this.Name = "ListDotNet";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.ListDotNet_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbFrameworks;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TreeView treeView1;
	}
}

