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
			this.lbEvents = new System.Windows.Forms.ListBox();
			this.txtEventName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnClearSelections = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudLastNDays)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(231, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "%$RC Companies%";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtCompanyName
			// 
			this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.txtCompanyName.Location = new System.Drawing.Point(23, 76);
			this.txtCompanyName.Name = "txtCompanyName";
			this.txtCompanyName.Size = new System.Drawing.Size(231, 22);
			this.txtCompanyName.TabIndex = 0;
			this.txtCompanyName.TextChanged += new System.EventHandler(this.txtCompanyName_TextChanged);
			// 
			// lblDays
			// 
			this.lblDays.AutoSize = true;
			this.lblDays.Location = new System.Drawing.Point(177, 17);
			this.lblDays.Name = "lblDays";
			this.lblDays.Size = new System.Drawing.Size(40, 17);
			this.lblDays.TabIndex = 13;
			this.lblDays.Text = "Days";
			// 
			// nudLastNDays
			// 
			this.nudLastNDays.Location = new System.Drawing.Point(83, 12);
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
			this.nudLastNDays.TabIndex = 2;
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
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 17);
			this.label3.TabIndex = 11;
			this.label3.Text = "Last";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(315, 14);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(233, 22);
			this.dateTimePicker1.TabIndex = 3;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(252, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 17);
			this.label2.TabIndex = 9;
			this.label2.Text = "Or after";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(512, 190);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(89, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(512, 232);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(89, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lbCompanies
			// 
			this.lbCompanies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbCompanies.FormattingEnabled = true;
			this.lbCompanies.ItemHeight = 16;
			this.lbCompanies.Location = new System.Drawing.Point(23, 104);
			this.lbCompanies.Name = "lbCompanies";
			this.lbCompanies.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbCompanies.Size = new System.Drawing.Size(231, 276);
			this.lbCompanies.TabIndex = 1;
			this.lbCompanies.SelectedIndexChanged += new System.EventHandler(this.lbCompanies_SelectedIndexChanged);
			// 
			// lbEvents
			// 
			this.lbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbEvents.FormattingEnabled = true;
			this.lbEvents.ItemHeight = 16;
			this.lbEvents.Location = new System.Drawing.Point(272, 104);
			this.lbEvents.Name = "lbEvents";
			this.lbEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbEvents.Size = new System.Drawing.Size(231, 276);
			this.lbEvents.TabIndex = 16;
			this.lbEvents.SelectedIndexChanged += new System.EventHandler(this.lbEvents_SelectedIndexChanged);
			// 
			// txtEventName
			// 
			this.txtEventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.txtEventName.Location = new System.Drawing.Point(272, 76);
			this.txtEventName.Name = "txtEventName";
			this.txtEventName.Size = new System.Drawing.Size(231, 22);
			this.txtEventName.TabIndex = 15;
			this.txtEventName.TextChanged += new System.EventHandler(this.txtEventName_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(269, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(231, 23);
			this.label4.TabIndex = 14;
			this.label4.Text = "%$Events%";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClearSelections
			// 
			this.btnClearSelections.Location = new System.Drawing.Point(512, 50);
			this.btnClearSelections.Name = "btnClearSelections";
			this.btnClearSelections.Size = new System.Drawing.Size(89, 48);
			this.btnClearSelections.TabIndex = 17;
			this.btnClearSelections.Text = "Clear Selections";
			this.btnClearSelections.UseVisualStyleBackColor = true;
			this.btnClearSelections.Click += new System.EventHandler(this.btnClearSelections_Click);
			// 
			// LLFilterForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(613, 387);
			this.Controls.Add(this.btnClearSelections);
			this.Controls.Add(this.lbEvents);
			this.Controls.Add(this.txtEventName);
			this.Controls.Add(this.label4);
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
		private System.Windows.Forms.ListBox lbEvents;
		private System.Windows.Forms.TextBox txtEventName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnClearSelections;
	}
}