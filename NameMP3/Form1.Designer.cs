namespace NameMP3 {
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
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DirNameTemplate = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.udDiskNumber = new System.Windows.Forms.NumericUpDown();
            this.udNumberOfDisks = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udDiskNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumberOfDisks)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(785, 231);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(122, 41);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go (obsolete)";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text to Use";
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Location = new System.Drawing.Point(302, 142);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(700, 22);
            this.txtText.TabIndex = 2;
            this.txtText.Text = "Sean Carroll - The Particle at the End of the Universe -Disk {0,2} of {1}-{2:D2}-" +
    "Track {2}";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(620, 240);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(141, 22);
            this.txtValue.TabIndex = 4;
            this.txtValue.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(488, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Value to Use";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 50);
            this.label3.TabIndex = 5;
            this.label3.Text = "Note: Normally used in conjunction with FreeRIP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(170, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Directory";
            // 
            // DirNameTemplate
            // 
            this.DirNameTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirNameTemplate.Location = new System.Drawing.Point(302, 31);
            this.DirNameTemplate.Name = "DirNameTemplate";
            this.DirNameTemplate.Size = new System.Drawing.Size(590, 22);
            this.DirNameTemplate.TabIndex = 7;
            this.DirNameTemplate.Text = "D:\\LRS\\CDs for Betty\\MP3\\Sean Carroll\\The Particle at the End of the Universe\\Dis" +
    "k {0,2} of {1}";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(927, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 118);
            this.button2.TabIndex = 9;
            this.button2.Text = "Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(491, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(416, 28);
            this.label5.TabIndex = 10;
            this.label5.Text = "Obsolete";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Disk #";
            // 
            // udDiskNumber
            // 
            this.udDiskNumber.Location = new System.Drawing.Point(302, 71);
            this.udDiskNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDiskNumber.Name = "udDiskNumber";
            this.udDiskNumber.Size = new System.Drawing.Size(77, 22);
            this.udDiskNumber.TabIndex = 13;
            this.udDiskNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.udDiskNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // udNumberOfDisks
            // 
            this.udNumberOfDisks.Location = new System.Drawing.Point(302, 100);
            this.udNumberOfDisks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udNumberOfDisks.Name = "udNumberOfDisks";
            this.udNumberOfDisks.Size = new System.Drawing.Size(77, 22);
            this.udNumberOfDisks.TabIndex = 15;
            this.udNumberOfDisks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.udNumberOfDisks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(170, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Number of Disks";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 284);
            this.Controls.Add(this.udNumberOfDisks);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.udDiskNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DirNameTemplate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGo);
            this.Name = "Form1";
            this.Text = "Name MP3";
            ((System.ComponentModel.ISupportInitialize)(this.udDiskNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumberOfDisks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DirNameTemplate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown udDiskNumber;
        private System.Windows.Forms.NumericUpDown udNumberOfDisks;
        private System.Windows.Forms.Label label7;
    }
}

