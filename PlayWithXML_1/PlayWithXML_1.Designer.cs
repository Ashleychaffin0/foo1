namespace PlayWithXML_1 {
	partial class PlayWithXML_1 {
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
			this.btnSerialize = new System.Windows.Forms.Button();
			this.btnDeserialize = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnSerialize
			// 
			this.btnSerialize.Location = new System.Drawing.Point(12, 12);
			this.btnSerialize.Name = "btnSerialize";
			this.btnSerialize.Size = new System.Drawing.Size(75, 23);
			this.btnSerialize.TabIndex = 0;
			this.btnSerialize.Text = "Serialize";
			this.btnSerialize.UseVisualStyleBackColor = true;
			this.btnSerialize.Click += new System.EventHandler(this.btnSerialize_Click);
			// 
			// btnDeserialize
			// 
			this.btnDeserialize.Location = new System.Drawing.Point(117, 12);
			this.btnDeserialize.Name = "btnDeserialize";
			this.btnDeserialize.Size = new System.Drawing.Size(75, 23);
			this.btnDeserialize.TabIndex = 1;
			this.btnDeserialize.Text = "Deserialize";
			this.btnDeserialize.UseVisualStyleBackColor = true;
			this.btnDeserialize.Click += new System.EventHandler(this.btnDeserialize_Click);
			// 
			// PlayWithXML_1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Controls.Add(this.btnDeserialize);
			this.Controls.Add(this.btnSerialize);
			this.Name = "PlayWithXML_1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnSerialize;
		private System.Windows.Forms.Button btnDeserialize;
	}
}

