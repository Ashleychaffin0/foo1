namespace SmartDeviceProject1 {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.MainMenu mainMenu1;

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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.label1 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblTOD = new System.Windows.Forms.Label();
			this.btnPushMe = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(22, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.Text = "Hello World";
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
			this.label2.Location = new System.Drawing.Point(22, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(153, 31);
			this.label2.Text = "A";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Yellow;
			this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
			this.label3.Location = new System.Drawing.Point(181, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 31);
			this.label3.Text = "B";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblTOD
			// 
			this.lblTOD.Location = new System.Drawing.Point(117, 22);
			this.lblTOD.Name = "lblTOD";
			this.lblTOD.Size = new System.Drawing.Size(100, 20);
			// 
			// btnPushMe
			// 
			this.btnPushMe.Location = new System.Drawing.Point(64, 123);
			this.btnPushMe.Name = "btnPushMe";
			this.btnPushMe.Size = new System.Drawing.Size(72, 20);
			this.btnPushMe.TabIndex = 6;
			this.btnPushMe.Text = "Push Me";
			this.btnPushMe.Click += new System.EventHandler(this.btnPushMe_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.btnPushMe);
			this.Controls.Add(this.lblTOD);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "LRS Toy";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTOD;
		private System.Windows.Forms.Button btnPushMe;
	}
}

