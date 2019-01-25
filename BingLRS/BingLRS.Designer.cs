namespace BingLRS {
    partial class BingLRS {
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
            this.lvImages = new System.Windows.Forms.ListView();
            this.btnGo = new System.Windows.Forms.Button();
            this.lbMsgs = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lvImages
            // 
            this.lvImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvImages.Location = new System.Drawing.Point(13, 41);
            this.lvImages.Name = "lvImages";
            this.lvImages.Size = new System.Drawing.Size(1080, 426);
            this.lvImages.TabIndex = 0;
            this.lvImages.UseCompatibleStateImageBehavior = false;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(13, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lbMsgs
            // 
            this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMsgs.FormattingEnabled = true;
            this.lbMsgs.ItemHeight = 16;
            this.lbMsgs.Location = new System.Drawing.Point(13, 480);
            this.lbMsgs.Name = "lbMsgs";
            this.lbMsgs.Size = new System.Drawing.Size(1080, 100);
            this.lbMsgs.TabIndex = 2;
            // 
            // BingLRS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 592);
            this.Controls.Add(this.lbMsgs);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lvImages);
            this.Name = "BingLRS";
            this.Text = "Bing LRS";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvImages;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ListBox lbMsgs;
    }
}

