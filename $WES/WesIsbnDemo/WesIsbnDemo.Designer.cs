namespace WesIsbnDemo {
	partial class WesIsbnDemo {
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
			this.txtIsbn = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblAuthor = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblPublisher = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lvOtherData = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtIsbn
			// 
			this.txtIsbn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtIsbn.Location = new System.Drawing.Point(101, 110);
			this.txtIsbn.Name = "txtIsbn";
			this.txtIsbn.Size = new System.Drawing.Size(398, 20);
			this.txtIsbn.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(29, 145);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Title";
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.Location = new System.Drawing.Point(101, 144);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(398, 23);
			this.lblTitle.TabIndex = 4;
			// 
			// lblAuthor
			// 
			this.lblAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAuthor.Location = new System.Drawing.Point(98, 184);
			this.lblAuthor.Name = "lblAuthor";
			this.lblAuthor.Size = new System.Drawing.Size(401, 23);
			this.lblAuthor.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(29, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Author";
			// 
			// lblPublisher
			// 
			this.lblPublisher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPublisher.Location = new System.Drawing.Point(98, 224);
			this.lblPublisher.Name = "lblPublisher";
			this.lblPublisher.Size = new System.Drawing.Size(401, 23);
			this.lblPublisher.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(29, 224);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Publisher";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BackColor = System.Drawing.Color.LightGreen;
			this.textBox1.Enabled = false;
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.ForeColor = System.Drawing.Color.Black;
			this.textBox1.Location = new System.Drawing.Point(32, 12);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(467, 68);
			this.textBox1.TabIndex = 9;
			this.textBox1.Text = "Enter an ISBN (hyphens OK but not needed), a book title, or an author\'s name in t" +
    "he Book ID field and hit Enter. Resize the window (width and height) if necessar" +
    "y.";
			// 
			// lvOtherData
			// 
			this.lvOtherData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvOtherData.GridLines = true;
			this.lvOtherData.Location = new System.Drawing.Point(32, 273);
			this.lvOtherData.Name = "lvOtherData";
			this.lvOtherData.Size = new System.Drawing.Size(467, 167);
			this.lvOtherData.TabIndex = 10;
			this.lvOtherData.UseCompatibleStateImageBehavior = false;
			this.lvOtherData.View = System.Windows.Forms.View.Details;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(32, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Book ID";
			// 
			// WesIsbnDemo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(530, 455);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lvOtherData);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.lblPublisher);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblAuthor);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtIsbn);
			this.KeyPreview = true;
			this.Name = "WesIsbnDemo";
			this.Text = "ISBN Demo";
			this.Load += new System.EventHandler(this.WesIsbnDemo_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WesIsbnDemo_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox txtIsbn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblAuthor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblPublisher;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ListView lvOtherData;
		private System.Windows.Forms.Label label1;
	}
}

