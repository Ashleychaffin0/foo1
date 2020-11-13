
namespace nsGetHerbie_EMan {
	partial class GetHerbie_EMan {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.CmbMag = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Pages = new System.Windows.Forms.PictureBox();
            this.BtnGo = new System.Windows.Forms.Button();
            this.LbMsgs = new System.Windows.Forms.ListBox();
            this.web2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.Pages)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(49, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(531, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mag";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CmbMag
            // 
            this.CmbMag.FormattingEnabled = true;
            this.CmbMag.Items.AddRange(new object[] {
            "Herbie",
            "E-Man-The-Early-Years/TPB-Part-1",
            "E-Man-The-Early-Years/TPB-Part-2",
            "Amazing-Spider-Girl"});
            this.CmbMag.Location = new System.Drawing.Point(49, 74);
            this.CmbMag.Name = "CmbMag";
            this.CmbMag.Size = new System.Drawing.Size(531, 49);
            this.CmbMag.TabIndex = 1;
            this.CmbMag.SelectedIndexChanged += new System.EventHandler(this.CmbMag_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(600, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 51);
            this.label2.TabIndex = 2;
            this.label2.Text = "Issues";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(600, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 51);
            this.label3.TabIndex = 3;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Pages
            // 
            this.Pages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pages.Location = new System.Drawing.Point(906, 29);
            this.Pages.Name = "Pages";
            this.Pages.Size = new System.Drawing.Size(601, 1036);
            this.Pages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pages.TabIndex = 4;
            this.Pages.TabStop = false;
            // 
            // BtnGo
            // 
            this.BtnGo.Location = new System.Drawing.Point(768, 23);
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(94, 52);
            this.BtnGo.TabIndex = 5;
            this.BtnGo.Text = "Go";
            this.BtnGo.UseVisualStyleBackColor = true;
            this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // LbMsgs
            // 
            this.LbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbMsgs.FormattingEnabled = true;
            this.LbMsgs.ItemHeight = 41;
            this.LbMsgs.Location = new System.Drawing.Point(49, 152);
            this.LbMsgs.Name = "LbMsgs";
            this.LbMsgs.Size = new System.Drawing.Size(838, 332);
            this.LbMsgs.TabIndex = 6;
            // 
            // web2
            // 
            this.web2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.web2.Location = new System.Drawing.Point(49, 527);
            this.web2.Name = "web2";
            this.web2.Size = new System.Drawing.Size(838, 496);
            this.web2.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            this.web2.TabIndex = 7;
            this.web2.ZoomFactor = 1D;
            // 
            // GetHerbie_EMan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1541, 1072);
            this.Controls.Add(this.web2);
            this.Controls.Add(this.LbMsgs);
            this.Controls.Add(this.BtnGo);
            this.Controls.Add(this.Pages);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CmbMag);
            this.Controls.Add(this.label1);
            this.Name = "GetHerbie_EMan";
            this.Text = "Get Herbie / E-Man";
            this.Load += new System.EventHandler(this.GetHerbie_EMan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pages)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox CmbMag;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox Pages;
		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.ListBox LbMsgs;
		private Microsoft.Web.WebView2.WinForms.WebView2 web2;
	}
}

