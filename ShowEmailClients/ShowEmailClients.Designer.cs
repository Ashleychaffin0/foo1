namespace ShowEmailClients {
	partial class ShowEmailClients {
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
			this.lblDefaultEMailClient = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lbClients = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtProgram = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 25);
			this.label1.TabIndex = 1;
			this.label1.Text = "Default EMail Client";
			// 
			// lblDefaultEMailClient
			// 
			this.lblDefaultEMailClient.AutoSize = true;
			this.lblDefaultEMailClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDefaultEMailClient.ForeColor = System.Drawing.Color.Red;
			this.lblDefaultEMailClient.Location = new System.Drawing.Point(291, 17);
			this.lblDefaultEMailClient.Name = "lblDefaultEMailClient";
			this.lblDefaultEMailClient.Size = new System.Drawing.Size(0, 25);
			this.lblDefaultEMailClient.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(24, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(314, 30);
			this.label3.TabIndex = 3;
			this.label3.Text = "Clients";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbClients
			// 
			this.lbClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbClients.FormattingEnabled = true;
			this.lbClients.ItemHeight = 25;
			this.lbClients.Location = new System.Drawing.Point(27, 121);
			this.lbClients.Name = "lbClients";
			this.lbClients.Size = new System.Drawing.Size(311, 279);
			this.lbClients.TabIndex = 4;
			this.lbClients.SelectedIndexChanged += new System.EventHandler(this.lbClients_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(355, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(311, 30);
			this.label4.TabIndex = 5;
			this.label4.Text = "Program";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtProgram
			// 
			this.txtProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProgram.Enabled = false;
			this.txtProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtProgram.Location = new System.Drawing.Point(355, 121);
			this.txtProgram.Multiline = true;
			this.txtProgram.Name = "txtProgram";
			this.txtProgram.Size = new System.Drawing.Size(311, 279);
			this.txtProgram.TabIndex = 7;
			// 
			// ShowEmailClients
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(689, 425);
			this.Controls.Add(this.txtProgram);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbClients);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblDefaultEMailClient);
			this.Controls.Add(this.label1);
			this.Name = "ShowEmailClients";
			this.Text = "Show Email Clients";
			this.Load += new System.EventHandler(this.ShowEmailClients_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShowEmailClients_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDefaultEMailClient;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox lbClients;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtProgram;
	}
}

