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
			this.mcEventDates = new System.Windows.Forms.MonthCalendar();
			this.statMsg = new System.Windows.Forms.StatusStrip();
			this.sbMsg = new System.Windows.Forms.ToolStripStatusLabel();
			this.chkIgnoreBartTests = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.nudLastNDays)).BeginInit();
			this.statMsg.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 41);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(173, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "%$RC Companies%";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtCompanyName
			// 
			this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.txtCompanyName.Location = new System.Drawing.Point(17, 62);
			this.txtCompanyName.Margin = new System.Windows.Forms.Padding(2);
			this.txtCompanyName.Name = "txtCompanyName";
			this.txtCompanyName.Size = new System.Drawing.Size(174, 20);
			this.txtCompanyName.TabIndex = 0;
			this.txtCompanyName.TextChanged += new System.EventHandler(this.txtCompanyName_TextChanged);
			// 
			// lblDays
			// 
			this.lblDays.AutoSize = true;
			this.lblDays.Location = new System.Drawing.Point(133, 14);
			this.lblDays.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblDays.Name = "lblDays";
			this.lblDays.Size = new System.Drawing.Size(31, 13);
			this.lblDays.TabIndex = 13;
			this.lblDays.Text = "Days";
			// 
			// nudLastNDays
			// 
			this.nudLastNDays.Location = new System.Drawing.Point(62, 10);
			this.nudLastNDays.Margin = new System.Windows.Forms.Padding(2);
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
			this.nudLastNDays.Size = new System.Drawing.Size(53, 20);
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
			this.label3.Location = new System.Drawing.Point(15, 12);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(27, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Last";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(242, 11);
			this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(2);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(176, 20);
			this.dateTimePicker1.TabIndex = 3;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(195, 14);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Or after";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(687, 132);
			this.btnOK.Margin = new System.Windows.Forms.Padding(2);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(67, 19);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(687, 166);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(67, 19);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lbCompanies
			// 
			this.lbCompanies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbCompanies.FormattingEnabled = true;
			this.lbCompanies.Location = new System.Drawing.Point(17, 84);
			this.lbCompanies.Margin = new System.Windows.Forms.Padding(2);
			this.lbCompanies.Name = "lbCompanies";
			this.lbCompanies.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbCompanies.Size = new System.Drawing.Size(174, 290);
			this.lbCompanies.TabIndex = 1;
			this.lbCompanies.DoubleClick += new System.EventHandler(this.lbCompanies_DoubleClick);
			this.lbCompanies.SelectedIndexChanged += new System.EventHandler(this.lbCompanies_SelectedIndexChanged);
			// 
			// lbEvents
			// 
			this.lbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbEvents.FormattingEnabled = true;
			this.lbEvents.Location = new System.Drawing.Point(204, 84);
			this.lbEvents.Margin = new System.Windows.Forms.Padding(2);
			this.lbEvents.Name = "lbEvents";
			this.lbEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbEvents.Size = new System.Drawing.Size(216, 290);
			this.lbEvents.TabIndex = 16;
			this.lbEvents.SelectedIndexChanged += new System.EventHandler(this.lbEvents_SelectedIndexChanged);
			// 
			// txtEventName
			// 
			this.txtEventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEventName.Location = new System.Drawing.Point(204, 62);
			this.txtEventName.Margin = new System.Windows.Forms.Padding(2);
			this.txtEventName.Name = "txtEventName";
			this.txtEventName.Size = new System.Drawing.Size(216, 20);
			this.txtEventName.TabIndex = 15;
			this.txtEventName.TextChanged += new System.EventHandler(this.txtEventName_TextChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(202, 41);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(214, 19);
			this.label4.TabIndex = 14;
			this.label4.Text = "%$Events%";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClearSelections
			// 
			this.btnClearSelections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearSelections.Location = new System.Drawing.Point(687, 62);
			this.btnClearSelections.Margin = new System.Windows.Forms.Padding(2);
			this.btnClearSelections.Name = "btnClearSelections";
			this.btnClearSelections.Size = new System.Drawing.Size(67, 39);
			this.btnClearSelections.TabIndex = 17;
			this.btnClearSelections.Text = "Clear Selections";
			this.btnClearSelections.UseVisualStyleBackColor = true;
			this.btnClearSelections.Click += new System.EventHandler(this.btnClearSelections_Click);
			// 
			// mcEventDates
			// 
			this.mcEventDates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mcEventDates.Enabled = false;
			this.mcEventDates.Location = new System.Drawing.Point(437, 62);
			this.mcEventDates.Margin = new System.Windows.Forms.Padding(7);
			this.mcEventDates.Name = "mcEventDates";
			this.mcEventDates.TabIndex = 18;
			// 
			// statMsg
			// 
			this.statMsg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbMsg});
			this.statMsg.Location = new System.Drawing.Point(0, 367);
			this.statMsg.Name = "statMsg";
			this.statMsg.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
			this.statMsg.Size = new System.Drawing.Size(763, 22);
			this.statMsg.TabIndex = 19;
			// 
			// sbMsg
			// 
			this.sbMsg.Name = "sbMsg";
			this.sbMsg.Size = new System.Drawing.Size(118, 17);
			this.sbMsg.Text = "toolStripStatusLabel1";
			// 
			// chkIgnoreBartTests
			// 
			this.chkIgnoreBartTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkIgnoreBartTests.AutoSize = true;
			this.chkIgnoreBartTests.Location = new System.Drawing.Point(437, 236);
			this.chkIgnoreBartTests.Margin = new System.Windows.Forms.Padding(2);
			this.chkIgnoreBartTests.Name = "chkIgnoreBartTests";
			this.chkIgnoreBartTests.Size = new System.Drawing.Size(126, 17);
			this.chkIgnoreBartTests.TabIndex = 20;
			this.chkIgnoreBartTests.Text = "Ignore Bartizan Tests";
			this.chkIgnoreBartTests.UseVisualStyleBackColor = true;
			this.chkIgnoreBartTests.CheckedChanged += new System.EventHandler(this.chkIgnoreBartTests_CheckedChanged);
			// 
			// LLFilterForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(763, 389);
			this.Controls.Add(this.chkIgnoreBartTests);
			this.Controls.Add(this.statMsg);
			this.Controls.Add(this.mcEventDates);
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
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "LLFilterForm";
			this.Text = "LeadsLightning Filter Info";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LLFilterForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.nudLastNDays)).EndInit();
			this.statMsg.ResumeLayout(false);
			this.statMsg.PerformLayout();
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
		private System.Windows.Forms.MonthCalendar mcEventDates;
		private System.Windows.Forms.StatusStrip statMsg;
		private System.Windows.Forms.ToolStripStatusLabel sbMsg;
		private System.Windows.Forms.CheckBox chkIgnoreBartTests;
	}
}