using MovieCollection1;
using System.Windows.Forms;

namespace MovieCollection1 {
	partial class MainForm {
		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing) {
			if (disposing && components != null) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.targetPanel = new System.Windows.Forms.Panel();
			this.searchOnlineButton = new System.Windows.Forms.Button();
			this.viewDetailsButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipText = null;
			this.notifyIcon1.BalloonTipTitle = null;
			this.notifyIcon1.Text = "Work with My Movie Collection";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.Click += new System.EventHandler(this.NotifyIcon1_Click);
			this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1_DoubleClick);
			// 
			// targetPanel
			// 
			this.targetPanel.BackColor = System.Drawing.Color.DimGray;
			this.targetPanel.Location = new System.Drawing.Point(0, 103);
			this.targetPanel.Name = "targetPanel";
			this.targetPanel.Size = new System.Drawing.Size(920, 494);
			this.targetPanel.TabIndex = 2;
			// 
			// searchOnlineButton
			// 
			this.searchOnlineButton.BackColor = System.Drawing.Color.Transparent;
			this.searchOnlineButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_search_active;
			this.searchOnlineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.searchOnlineButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.searchOnlineButton.FlatAppearance.BorderSize = 0;
			this.searchOnlineButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.searchOnlineButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.searchOnlineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.searchOnlineButton.Location = new System.Drawing.Point(767, 28);
			this.searchOnlineButton.Name = "searchOnlineButton";
			this.searchOnlineButton.Size = new System.Drawing.Size(120, 110);
			this.searchOnlineButton.TabIndex = 1;
			this.searchOnlineButton.Text = "search online";
			this.searchOnlineButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.searchOnlineButton.UseVisualStyleBackColor = false;
			this.searchOnlineButton.Click += new System.EventHandler(this.SearchOnlineButton_Click);
			// 
			// viewDetailsButton
			// 
			this.viewDetailsButton.BackColor = System.Drawing.Color.Transparent;
			this.viewDetailsButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_viewDvd_active;
			this.viewDetailsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.viewDetailsButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.viewDetailsButton.FlatAppearance.BorderSize = 0;
			this.viewDetailsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.viewDetailsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.viewDetailsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.viewDetailsButton.Location = new System.Drawing.Point(627, 28);
			this.viewDetailsButton.Name = "viewDetailsButton";
			this.viewDetailsButton.Size = new System.Drawing.Size(120, 110);
			this.viewDetailsButton.TabIndex = 0;
			this.viewDetailsButton.Text = "view dvds";
			this.viewDetailsButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.viewDetailsButton.UseVisualStyleBackColor = false;
			this.viewDetailsButton.Click += new System.EventHandler(this.ViewDetailsButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(13, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(194, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "my movie collection";
			// 
			// MainForm
			// 
			this.BackgroundImage = global::MovieCollection1.Properties.Resources.Blue_fullBg_noSideText;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(914, 618);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.searchOnlineButton);
			this.Controls.Add(this.viewDetailsButton);
			this.Controls.Add(this.targetPanel);
			this.ForeColor = System.Drawing.Color.DimGray;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "My Movie Collection";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private NotifyIcon notifyIcon1;
		private Panel targetPanel;
		private Button searchOnlineButton;
		private Button viewDetailsButton;
		private Label label1;

	}
}
