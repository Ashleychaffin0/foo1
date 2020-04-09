namespace TestDataGridViewCustomColumns {
	partial class TestDataGridViewCustomColumns {
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
			this.IFGrid = new System.Windows.Forms.DataGridView();
			this.Conjunction = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.FieldName = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Condition = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.IFGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// IFGrid
			// 
			this.IFGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.IFGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Conjunction,
            this.FieldName,
            this.Condition,
            this.Value});
			this.IFGrid.Location = new System.Drawing.Point(12, 61);
			this.IFGrid.Name = "IFGrid";
			this.IFGrid.RowTemplate.Height = 24;
			this.IFGrid.Size = new System.Drawing.Size(705, 150);
			this.IFGrid.TabIndex = 0;
			// 
			// Conjunction
			// 
			this.Conjunction.Frozen = true;
			this.Conjunction.HeaderText = "AND/OR";
			this.Conjunction.Items.AddRange(new object[] {
            "",
            "AND",
            "OR"});
			this.Conjunction.Name = "Conjunction";
			// 
			// FieldName
			// 
			this.FieldName.Frozen = true;
			this.FieldName.HeaderText = "Field Name";
			this.FieldName.Name = "FieldName";
			this.FieldName.Width = 200;
			// 
			// Condition
			// 
			this.Condition.Frozen = true;
			this.Condition.HeaderText = "Condition";
			this.Condition.Name = "Condition";
			this.Condition.Width = 150;
			// 
			// Value
			// 
			this.Value.Frozen = true;
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.Width = 300;
			// 
			// TestDataGridViewCustomColumns
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1045, 429);
			this.Controls.Add(this.IFGrid);
			this.Name = "TestDataGridViewCustomColumns";
			this.Text = "Test DataGridView Custom Columns";
			((System.ComponentModel.ISupportInitialize)(this.IFGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView IFGrid;
		private System.Windows.Forms.DataGridViewComboBoxColumn Conjunction;
		private System.Windows.Forms.DataGridViewComboBoxColumn FieldName;
		private System.Windows.Forms.DataGridViewComboBoxColumn Condition;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
	}
}

