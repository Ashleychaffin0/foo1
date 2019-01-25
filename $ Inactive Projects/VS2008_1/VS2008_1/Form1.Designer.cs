namespace VS2008_1 {
	partial class Form1 {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label eventCityLabel;
			this.button1 = new System.Windows.Forms.Button();
			this.tblEventBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.eventCityTextBox = new System.Windows.Forms.TextBox();
			eventCityLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.tblEventBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// eventCityLabel
			// 
			eventCityLabel.AutoSize = true;
			eventCityLabel.Location = new System.Drawing.Point(74, 44);
			eventCityLabel.Name = "eventCityLabel";
			eventCityLabel.Size = new System.Drawing.Size(58, 13);
			eventCityLabel.TabIndex = 3;
			eventCityLabel.Text = "Event City:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tblEventBindingSource
			// 
			this.tblEventBindingSource.DataSource = typeof(VS2008_1.tblEvent);
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(13, 132);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(267, 128);
			this.dataGridView1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "City";
			// 
			// eventCityTextBox
			// 
			this.eventCityTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.tblEventBindingSource, "EventCity", true));
			this.eventCityTextBox.Location = new System.Drawing.Point(138, 41);
			this.eventCityTextBox.Name = "eventCityTextBox";
			this.eventCityTextBox.Size = new System.Drawing.Size(100, 20);
			this.eventCityTextBox.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 272);
			this.Controls.Add(eventCityLabel);
			this.Controls.Add(this.eventCityTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.tblEventBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.BindingSource tblEventBindingSource;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox eventCityTextBox;
	}
}

