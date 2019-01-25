namespace CheckForUpsDelivery
{
    partial class CheckForUpsDelivery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnGo = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.RadUPS = new System.Windows.Forms.RadioButton();
			this.RadFedex = new System.Windows.Forms.RadioButton();
			this.TxtUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(33, 70);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.Location = new System.Drawing.Point(12, 123);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(789, 550);
			this.lbMsgs.TabIndex = 1;
			// 
			// RadUPS
			// 
			this.RadUPS.AutoSize = true;
			this.RadUPS.Location = new System.Drawing.Point(33, 25);
			this.RadUPS.Name = "RadUPS";
			this.RadUPS.Size = new System.Drawing.Size(47, 17);
			this.RadUPS.TabIndex = 2;
			this.RadUPS.TabStop = true;
			this.RadUPS.Text = "UPS";
			this.RadUPS.UseVisualStyleBackColor = true;
			// 
			// RadFedex
			// 
			this.RadFedex.AutoSize = true;
			this.RadFedex.Location = new System.Drawing.Point(101, 25);
			this.RadFedex.Name = "RadFedex";
			this.RadFedex.Size = new System.Drawing.Size(55, 17);
			this.RadFedex.TabIndex = 3;
			this.RadFedex.TabStop = true;
			this.RadFedex.Text = "FedEx";
			this.RadFedex.UseVisualStyleBackColor = true;
			// 
			// TxtUrl
			// 
			this.TxtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtUrl.Location = new System.Drawing.Point(219, 24);
			this.TxtUrl.Name = "TxtUrl";
			this.TxtUrl.Size = new System.Drawing.Size(582, 20);
			this.TxtUrl.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(193, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Url";
			// 
			// CheckForUpsDelivery
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 631);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TxtUrl);
			this.Controls.Add(this.RadFedex);
			this.Controls.Add(this.RadUPS);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnGo);
			this.Name = "CheckForUpsDelivery";
			this.Text = "Check for Package Delivery";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.RadioButton RadUPS;
		private System.Windows.Forms.RadioButton RadFedex;
		private System.Windows.Forms.TextBox TxtUrl;
		private System.Windows.Forms.Label label1;
	}
}

