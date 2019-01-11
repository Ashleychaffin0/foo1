namespace TestLinqToSqlWithXml_1 {
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
            this.btnInsertNow = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInsertNow
            // 
            this.btnInsertNow.Location = new System.Drawing.Point(61, 48);
            this.btnInsertNow.Name = "btnInsertNow";
            this.btnInsertNow.Size = new System.Drawing.Size(141, 23);
            this.btnInsertNow.TabIndex = 0;
            this.btnInsertNow.Text = "Insert \"Now\"";
            this.btnInsertNow.UseVisualStyleBackColor = true;
            this.btnInsertNow.Click += new System.EventHandler(this.btnInsertNow_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(247, 48);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(141, 23);
            this.btnList.TabIndex = 1;
            this.btnList.Text = "List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 414);
            this.Controls.Add(this.btnList);
            this.Controls.Add(this.btnInsertNow);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInsertNow;
        private System.Windows.Forms.Button btnList;
    }
}

