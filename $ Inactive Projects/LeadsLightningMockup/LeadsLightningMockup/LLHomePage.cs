using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace LeadsLightningMockup {
	/// <summary>
	/// Summary description for LLHomePage.
	/// </summary>
	public class LLHomePage : PseudoWebPage {
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label txtMsgs;
		private System.Windows.Forms.Button btnForgotPassword;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LLHomePage(Panel page) : base(page) {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label2 = new System.Windows.Forms.Label();
			this.btnLogin = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMsgs = new System.Windows.Forms.Label();
			this.btnForgotPassword = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(208, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(464, 400);
			this.label2.TabIndex = 2;
			this.label2.Text = "LeadsLightning Blurb";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(16, 304);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(124, 37);
			this.btnLogin.TabIndex = 14;
			this.btnLogin.Text = "Login";
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(16, 264);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(153, 22);
			this.txtPassword.TabIndex = 13;
			this.txtPassword.Text = "";
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(16, 168);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(153, 22);
			this.txtUserName.TabIndex = 12;
			this.txtUserName.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 224);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 28);
			this.label4.TabIndex = 11;
			this.label4.Text = "Password";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 28);
			this.label3.TabIndex = 10;
			this.label3.Text = "User name";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(168, 112);
			this.label5.TabIndex = 18;
			this.label5.Text = "UserName:\nRC = Reg Contractor\nEX = Exhibitor\nBART = \'Evelyn\'\nZOO = Us\n\nPassword =" +
				" Ignored";
			// 
			// txtMsgs
			// 
			this.txtMsgs.Location = new System.Drawing.Point(24, 352);
			this.txtMsgs.Name = "txtMsgs";
			this.txtMsgs.Size = new System.Drawing.Size(176, 40);
			this.txtMsgs.TabIndex = 19;
			this.txtMsgs.Text = "Login status msgs go here";
			// 
			// btnForgotPassword
			// 
			this.btnForgotPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnForgotPassword.Location = new System.Drawing.Point(96, 216);
			this.btnForgotPassword.Name = "btnForgotPassword";
			this.btnForgotPassword.Size = new System.Drawing.Size(72, 37);
			this.btnForgotPassword.TabIndex = 20;
			this.btnForgotPassword.Text = "Forgot it?";
			this.btnForgotPassword.Click += new System.EventHandler(this.btnForgotPassword_Click);
			// 
			// LLHomePage
			// 
			this.Controls.Add(this.btnForgotPassword);
			this.Controls.Add(this.txtMsgs);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Name = "LLHomePage";
			this.Size = new System.Drawing.Size(696, 432);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnLogin_Click(object sender, System.EventArgs e) {
			txtMsgs.Text = "";
			switch (txtUserName.Text.Trim().ToUpper()) {
			case "RC":
				new RCWelcome(base.ParentPanel);
				break;
			case "EX":
				new EXWelcome(base.ParentPanel);
				break;
			case "BART":
				new BartWelcome(base.ParentPanel);
				break;
			case "ZOO":
				new ZooWelcome(base.ParentPanel);
				break;
			default:
				txtMsgs.Text = "UserID / Password not recognized. Please try again.";
				break;
			}
		}

		private void btnForgotPassword_Click(object sender, System.EventArgs e) {
			// TODO: Check for valid email address (xxx@yyy.zzz)
			MessageBox.Show("Your password has been emailed to the above email address.", "LeadsLightning");
		}

	}
}
