using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace OnScreenKeyboard
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button1.Location = new System.Drawing.Point(112, 16);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 56);
			this.button1.TabIndex = 0;
			this.button1.Text = "1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button2.Location = new System.Drawing.Point(200, 16);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(64, 56);
			this.button2.TabIndex = 1;
			this.button2.Text = "2";
			this.button2.Click += new System.EventHandler(this.button1_Click);
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button3.Location = new System.Drawing.Point(24, 80);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(64, 56);
			this.button3.TabIndex = 2;
			this.button3.Text = "3";
			this.button3.Click += new System.EventHandler(this.button1_Click);
			// 
			// button4
			// 
			this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button4.Location = new System.Drawing.Point(112, 80);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(64, 56);
			this.button4.TabIndex = 3;
			this.button4.Text = "4";
			this.button4.Click += new System.EventHandler(this.button1_Click);
			// 
			// button5
			// 
			this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button5.Location = new System.Drawing.Point(200, 80);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(64, 56);
			this.button5.TabIndex = 4;
			this.button5.Text = "5";
			this.button5.Click += new System.EventHandler(this.button1_Click);
			// 
			// button6
			// 
			this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button6.Location = new System.Drawing.Point(24, 152);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(64, 56);
			this.button6.TabIndex = 5;
			this.button6.Text = "6";
			this.button6.Click += new System.EventHandler(this.button1_Click);
			// 
			// button7
			// 
			this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button7.Location = new System.Drawing.Point(112, 152);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(64, 56);
			this.button7.TabIndex = 6;
			this.button7.Text = "7";
			this.button7.Click += new System.EventHandler(this.button1_Click);
			// 
			// button8
			// 
			this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button8.Location = new System.Drawing.Point(200, 152);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(64, 56);
			this.button8.TabIndex = 7;
			this.button8.Text = "8";
			this.button8.Click += new System.EventHandler(this.button1_Click);
			// 
			// button9
			// 
			this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button9.Location = new System.Drawing.Point(112, 232);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(64, 56);
			this.button9.TabIndex = 8;
			this.button9.Text = "9";
			this.button9.Click += new System.EventHandler(this.button1_Click);
			// 
			// button10
			// 
			this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button10.Location = new System.Drawing.Point(24, 16);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(64, 56);
			this.button10.TabIndex = 9;
			this.button10.Text = "0";
			this.button10.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(320, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(248, 20);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "textBox1";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 422);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "On Screen Keyboard";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			this.Validating += new System.ComponentModel.CancelEventHandler(this.Form1_Validating);
			this.Validated += new System.EventHandler(this.Form1_Validated);
			this.Activated += new System.EventHandler(this.Form1_Activated);
			this.Leave += new System.EventHandler(this.Form1_Leave);
			this.Enter += new System.EventHandler(this.Form1_Enter);
			this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Activated(object sender, System.EventArgs e) {
		}

		private void button1_Click(object sender, System.EventArgs e) {
			string	s = ((Button)sender).Text;
			SendKeys.Send(s);
		
		}

		private void Form1_Deactivate(object sender, System.EventArgs e) {
		
		}

		private void Form1_Enter(object sender, System.EventArgs e) {
		
		}

		private void Form1_Leave(object sender, System.EventArgs e) {
		
		}

		private void Form1_Validated(object sender, System.EventArgs e) {
		
		}

		private void Form1_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
		
		}

		private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
		
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			textBox1.Text = e.KeyChar.ToString();
		}

		private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			textBox1.Text = e.KeyData.ToString();
		}
	}
}
