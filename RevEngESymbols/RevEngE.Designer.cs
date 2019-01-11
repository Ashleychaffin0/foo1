namespace RevEngESymbols {
	partial class RevEngESymbols {
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
			this.lbThings = new System.Windows.Forms.ListBox();
			this.radGlobals = new System.Windows.Forms.RadioButton();
			this.radFunctions = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtLocalName = new System.Windows.Forms.TextBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblLocals = new System.Windows.Forms.Label();
			this.lbLocals = new System.Windows.Forms.ListBox();
			this.lblLocalName = new System.Windows.Forms.Label();
			this.lblLocalDescription = new System.Windows.Forms.Label();
			this.txtLocalDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbThings
			// 
			this.lbThings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbThings.FormattingEnabled = true;
			this.lbThings.Location = new System.Drawing.Point(12, 61);
			this.lbThings.Name = "lbThings";
			this.lbThings.Size = new System.Drawing.Size(298, 433);
			this.lbThings.Sorted = true;
			this.lbThings.TabIndex = 0;
			this.lbThings.SelectedIndexChanged += new System.EventHandler(this.lbThings_SelectedIndexChanged);
			// 
			// radGlobals
			// 
			this.radGlobals.AutoSize = true;
			this.radGlobals.Location = new System.Drawing.Point(109, 27);
			this.radGlobals.Name = "radGlobals";
			this.radGlobals.Size = new System.Drawing.Size(60, 17);
			this.radGlobals.TabIndex = 1;
			this.radGlobals.Text = "Globals";
			this.radGlobals.UseVisualStyleBackColor = true;
			// 
			// radFunctions
			// 
			this.radFunctions.AutoSize = true;
			this.radFunctions.Location = new System.Drawing.Point(12, 27);
			this.radFunctions.Name = "radFunctions";
			this.radFunctions.Size = new System.Drawing.Size(71, 17);
			this.radFunctions.TabIndex = 2;
			this.radFunctions.Text = "Functions";
			this.radFunctions.UseVisualStyleBackColor = true;
			this.radFunctions.CheckedChanged += new System.EventHandler(this.radFunctions_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(342, 58);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Address";
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(421, 58);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(0, 13);
			this.lblAddress.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(342, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Descriptiom";
			// 
			// txtLocalName
			// 
			this.txtLocalName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLocalName.Enabled = false;
			this.txtLocalName.Location = new System.Drawing.Point(666, 219);
			this.txtLocalName.Name = "txtLocalName";
			this.txtLocalName.Size = new System.Drawing.Size(185, 20);
			this.txtLocalName.TabIndex = 7;
			this.txtLocalName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocalName_KeyPress);
			this.txtLocalName.Leave += new System.EventHandler(this.txtLocalName_Leave);
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Enabled = false;
			this.txtDescription.Location = new System.Drawing.Point(345, 96);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDescription.Size = new System.Drawing.Size(506, 88);
			this.txtDescription.TabIndex = 8;
			this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
			this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
			// 
			// lblLocals
			// 
			this.lblLocals.AutoSize = true;
			this.lblLocals.Location = new System.Drawing.Point(342, 202);
			this.lblLocals.Name = "lblLocals";
			this.lblLocals.Size = new System.Drawing.Size(38, 13);
			this.lblLocals.TabIndex = 9;
			this.lblLocals.Text = "Locals";
			// 
			// lbLocals
			// 
			this.lbLocals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbLocals.FormattingEnabled = true;
			this.lbLocals.Location = new System.Drawing.Point(345, 219);
			this.lbLocals.Name = "lbLocals";
			this.lbLocals.Size = new System.Drawing.Size(218, 277);
			this.lbLocals.Sorted = true;
			this.lbLocals.TabIndex = 10;
			this.lbLocals.SelectedIndexChanged += new System.EventHandler(this.lbLocals_SelectedIndexChanged);
			// 
			// lblLocalName
			// 
			this.lblLocalName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblLocalName.AutoSize = true;
			this.lblLocalName.Location = new System.Drawing.Point(598, 222);
			this.lblLocalName.Name = "lblLocalName";
			this.lblLocalName.Size = new System.Drawing.Size(35, 13);
			this.lblLocalName.TabIndex = 11;
			this.lblLocalName.Text = "Name";
			// 
			// lblLocalDescription
			// 
			this.lblLocalDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblLocalDescription.AutoSize = true;
			this.lblLocalDescription.Location = new System.Drawing.Point(598, 254);
			this.lblLocalDescription.Name = "lblLocalDescription";
			this.lblLocalDescription.Size = new System.Drawing.Size(62, 13);
			this.lblLocalDescription.TabIndex = 12;
			this.lblLocalDescription.Text = "Descriptiom";
			// 
			// txtLocalDescription
			// 
			this.txtLocalDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLocalDescription.Enabled = false;
			this.txtLocalDescription.Location = new System.Drawing.Point(601, 270);
			this.txtLocalDescription.Multiline = true;
			this.txtLocalDescription.Name = "txtLocalDescription";
			this.txtLocalDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLocalDescription.Size = new System.Drawing.Size(250, 224);
			this.txtLocalDescription.TabIndex = 13;
			this.txtLocalDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocalDescription_KeyPress);
			this.txtLocalDescription.Leave += new System.EventHandler(this.txtLocalDescription_Leave);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(490, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Name";
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Enabled = false;
			this.txtName.Location = new System.Drawing.Point(531, 55);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(320, 20);
			this.txtName.TabIndex = 14;
			this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
			this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(342, 29);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(238, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "Note: To rename an intem, change its Name field";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(863, 24);
			this.menuStrip1.TabIndex = 18;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openProjectToolStripMenuItem
			// 
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.openProjectToolStripMenuItem.Text = "Open Project";
			this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
			// 
			// saveProjectToolStripMenuItem
			// 
			this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
			this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.saveProjectToolStripMenuItem.Text = "Save Project";
			this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
			// 
			// RevEngESymbols
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(863, 502);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.txtLocalDescription);
			this.Controls.Add(this.lblLocalDescription);
			this.Controls.Add(this.lblLocalName);
			this.Controls.Add(this.lbLocals);
			this.Controls.Add(this.lblLocals);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.txtLocalName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblAddress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.radFunctions);
			this.Controls.Add(this.radGlobals);
			this.Controls.Add(this.lbThings);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "RevEngESymbols";
			this.Text = "RevEngE Symbols";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RevEngESymbols_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbThings;
		private System.Windows.Forms.RadioButton radGlobals;
		private System.Windows.Forms.RadioButton radFunctions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtLocalName;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblLocals;
		private System.Windows.Forms.ListBox lbLocals;
		private System.Windows.Forms.Label lblLocalName;
		private System.Windows.Forms.Label lblLocalDescription;
		private System.Windows.Forms.TextBox txtLocalDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
	}
}

