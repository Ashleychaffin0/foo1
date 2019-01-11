namespace ShopRiteCoupons {
	partial class ShopRiteCoupons {
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
			this.cmbCardOwner = new System.Windows.Forms.ComboBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.BtnTestColorDialog = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Card Owner";
			// 
			// cmbCardOwner
			// 
			this.cmbCardOwner.FormattingEnabled = true;
			this.cmbCardOwner.Items.AddRange(new object[] {
            "BGA",
            "LRS"});
			this.cmbCardOwner.Location = new System.Drawing.Point(88, 20);
			this.cmbCardOwner.Name = "cmbCardOwner";
			this.cmbCardOwner.Size = new System.Drawing.Size(121, 21);
			this.cmbCardOwner.TabIndex = 2;
			this.cmbCardOwner.SelectedIndexChanged += new System.EventHandler(this.cmbCardOwner_SelectedIndexChanged);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(270, 18);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			this.colorDialog1.FullOpen = true;
			// 
			// BtnTestColorDialog
			// 
			this.BtnTestColorDialog.Location = new System.Drawing.Point(529, 16);
			this.BtnTestColorDialog.Name = "BtnTestColorDialog";
			this.BtnTestColorDialog.Size = new System.Drawing.Size(146, 23);
			this.BtnTestColorDialog.TabIndex = 4;
			this.BtnTestColorDialog.Text = "Test color dialog";
			this.BtnTestColorDialog.UseVisualStyleBackColor = true;
			this.BtnTestColorDialog.Click += new System.EventHandler(this.BtnTestColorDialog_Click);
			// 
			// ShopRiteCoupons
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1132, 54);
			this.Controls.Add(this.BtnTestColorDialog);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.cmbCardOwner);
			this.Controls.Add(this.label1);
			this.Name = "ShopRiteCoupons";
			this.Text = "Shop Rite Coupons";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShopRiteCoupons_FormClosed);
			this.Load += new System.EventHandler(this.ShopRiteCoupons_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbCardOwner;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Button BtnTestColorDialog;
	}
}

