namespace MovieCollection2.Controls {
	partial class RatingPickerControl {
		[System.Diagnostics.DebuggerNonUserCode]
		public RatingPickerControl()
			: base() {

			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode]
		protected override void Dispose(bool disposing) {
			if (disposing && components != null)
			// components.Dispose()
            {
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components = null;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent() {
			this.Star1 = new System.Windows.Forms.PictureBox();
			this.Star2 = new System.Windows.Forms.PictureBox();
			this.Star3 = new System.Windows.Forms.PictureBox();
			this.Star4 = new System.Windows.Forms.PictureBox();
			this.Star5 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.Star1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Star5)).BeginInit();
			this.SuspendLayout();
			// 
			// Star1
			// 
			this.Star1.Location = new System.Drawing.Point(0, 0);
			this.Star1.Name = "Star1";
			this.Star1.Size = new System.Drawing.Size(16, 16);
			this.Star1.TabIndex = 0;
			this.Star1.TabStop = false;
			this.Star1.MouseLeave += new System.EventHandler(this.Star_MouseLeave);
			this.Star1.Click += new System.EventHandler(this.Star_Click);
			this.Star1.MouseHover += new System.EventHandler(this.Star_MouseHover);
			// 
			// Star2
			// 
			this.Star2.Location = new System.Drawing.Point(16, 0);
			this.Star2.Name = "Star2";
			this.Star2.Size = new System.Drawing.Size(16, 16);
			this.Star2.TabIndex = 1;
			this.Star2.TabStop = false;
			this.Star2.MouseLeave += new System.EventHandler(this.Star_MouseLeave);
			this.Star2.Click += new System.EventHandler(this.Star_Click);
			this.Star2.MouseHover += new System.EventHandler(this.Star_MouseHover);
			// 
			// Star3
			// 
			this.Star3.Location = new System.Drawing.Point(32, 0);
			this.Star3.Name = "Star3";
			this.Star3.Size = new System.Drawing.Size(16, 16);
			this.Star3.TabIndex = 2;
			this.Star3.TabStop = false;
			this.Star3.MouseLeave += new System.EventHandler(this.Star_MouseLeave);
			this.Star3.Click += new System.EventHandler(this.Star_Click);
			this.Star3.MouseHover += new System.EventHandler(this.Star_MouseHover);
			// 
			// Star4
			// 
			this.Star4.Location = new System.Drawing.Point(48, 0);
			this.Star4.Name = "Star4";
			this.Star4.Size = new System.Drawing.Size(16, 16);
			this.Star4.TabIndex = 3;
			this.Star4.TabStop = false;
			this.Star4.MouseLeave += new System.EventHandler(this.Star_MouseLeave);
			this.Star4.Click += new System.EventHandler(this.Star_Click);
			this.Star4.MouseHover += new System.EventHandler(this.Star_MouseHover);
			// 
			// Star5
			// 
			this.Star5.Location = new System.Drawing.Point(64, 0);
			this.Star5.Name = "Star5";
			this.Star5.Size = new System.Drawing.Size(16, 16);
			this.Star5.TabIndex = 4;
			this.Star5.TabStop = false;
			this.Star5.MouseLeave += new System.EventHandler(this.Star_MouseLeave);
			this.Star5.Click += new System.EventHandler(this.Star_Click);
			this.Star5.MouseHover += new System.EventHandler(this.Star_MouseHover);
			// 
			// RatingPickerControl
			// 
			this.Controls.Add(this.Star5);
			this.Controls.Add(this.Star4);
			this.Controls.Add(this.Star3);
			this.Controls.Add(this.Star2);
			this.Controls.Add(this.Star1);
			this.Size = new System.Drawing.Size(80, 16);
			((System.ComponentModel.ISupportInitialize)(this.Star1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Star5)).EndInit();
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.PictureBox Star1;
		private System.Windows.Forms.PictureBox Star2;
		private System.Windows.Forms.PictureBox Star3;
		private System.Windows.Forms.PictureBox Star4;
		private System.Windows.Forms.PictureBox Star5;

	}
}


