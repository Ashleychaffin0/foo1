namespace LLImportStats {
	partial class LLFilterForm {
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
			this.txtCompanyName = new System.Windows.Forms.TextBox();
			this.lblDays = new System.Windows.Forms.Label();
			this.nudLastNDays = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbCompanies = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)(this.nudLastNDays)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(33, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(223, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "%RC Companies%";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtCompanyName
			// 
			this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCompanyName.Location = new System.Drawing.Point(33, 57);
			this.txtCompanyName.Name = "txtCompanyName";
			this.txtCompanyName.Size = new System.Drawing.Size(223, 22);
			this.txtCompanyName.TabIndex = 2;
			this.txtCompanyName.TextChanged += new System.EventHandler(this.txtCompanyName_TextChanged);
			// 
			// lblDays
			// 
			this.lblDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDays.AutoSize = true;
			this.lblDays.Location = new System.Drawing.Point(427, 60);
			this.lblDays.Name = "lblDays";
			this.lblDays.Size = new System.Drawing.Size(40, 17);
			this.lblDays.TabIndex = 13;
			this.lblDays.Text = "Days";
			// 
			// nudLastNDays
			// 
			this.nudLastNDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nudLastNDays.Location = new System.Drawing.Point(333, 57);
			this.nudLastNDays.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudLastNDays.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.nudLastNDays.Name = "nudLastNDays";
			this.nudLastNDays.Size = new System.Drawing.Size(71, 22);
			this.nudLastNDays.TabIndex = 12;
			this.nudLastNDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudLastNDays.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.nudLastNDays.ValueChanged += new System.EventHandler(this.nudLastNDays_ValueChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(270, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 17);
			this.label3.TabIndex = 11;
			this.label3.Text = "Last";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker1.Location = new System.Drawing.Point(333, 101);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(233, 22);
			this.dateTimePicker1.TabIndex = 10;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(270, 101);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 17);
			this.label2.TabIndex = 9;
			this.label2.Text = "Or after";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(391, 156);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(491, 156);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lbCompanies
			// 
			this.lbCompanies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbCompanies.FormattingEnabled = true;
			this.lbCompanies.ItemHeight = 16;
			this.lbCompanies.Location = new System.Drawing.Point(36, 89);
			this.lbCompanies.Name = "lbCompanies";
			this.lbCompanies.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbCompanies.Size = new System.Drawing.Size(220, 244);
			this.lbCompanies.TabIndex = 16;
			// 
			// LLFilterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(588, 387);
			this.Controls.Add(this.lbCompanies);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblDays);
			this.Controls.Add(this.nudLastNDays);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCompanyName);
			this.Controls.Add(this.label1);
			this.Name = "LLFilterForm";
			this.Text = "LeadsLightning Filter Info";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LLFilterForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.nudLastNDays)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCompanyName;
		private System.Windows.Forms.Label lblDays;
		private System.Windows.Forms.NumericUpDown nudLastNDays;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ListBox lbCompanies;
	}
}