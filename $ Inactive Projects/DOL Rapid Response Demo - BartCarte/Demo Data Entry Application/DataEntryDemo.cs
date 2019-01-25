using System;
// using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

using System.Drawing.Printing;


namespace nsDemoBartCarte {
	/// <summary>
	/// Summary description for Demo_Data_Entry_Application.
	/// </summary>
	public class Demo_Data_Entry_Application : System.Windows.Forms.Form {
		#region		// GUI declarations
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox txtSSN;
		private System.Windows.Forms.TextBox txtFName;
		private System.Windows.Forms.TextBox txtMI;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.TextBox txtLName;
		private System.Windows.Forms.TextBox txtAddr1;
		private System.Windows.Forms.TextBox txtAddr2;
		private System.Windows.Forms.TextBox txtCity;
		private System.Windows.Forms.ComboBox cmbState;
		private System.Windows.Forms.TextBox txtZip;
		private System.Windows.Forms.DateTimePicker dtDOB;
		private System.Windows.Forms.ComboBox cmbGender;
		private System.Windows.Forms.TextBox txtPhone1;
		private System.Windows.Forms.TextBox txtPhone2;
		private System.Windows.Forms.Button btnPrintBadge;
		private System.Windows.Forms.Button btnNewBadge;
		#endregion
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lbDbgMsgs;

		public BartCarteDemoPrinter		prt;


		public Demo_Data_Entry_Application() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ChannelServices.RegisterChannel(new TcpClientChannel());
			prt = (BartCarteDemoPrinter)Activator.GetObject(typeof(BartCarteDemoPrinter), "tcp://localhost:1729/BartCarteDemo.rem");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing)	{
				if (components != null) {
					components.Dispose();
				}
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
			this.txtSSN = new System.Windows.Forms.TextBox();
			this.txtFName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMI = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtLName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtAddr1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtAddr2 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCity = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmbState = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtZip = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.dtDOB = new System.Windows.Forms.DateTimePicker();
			this.label11 = new System.Windows.Forms.Label();
			this.cmbGender = new System.Windows.Forms.ComboBox();
			this.txtPhone1 = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtPhone2 = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.btnPrintBadge = new System.Windows.Forms.Button();
			this.btnNewBadge = new System.Windows.Forms.Button();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.lbDbgMsgs = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "SSN";
			// 
			// txtSSN
			// 
			this.txtSSN.Location = new System.Drawing.Point(112, 32);
			this.txtSSN.Name = "txtSSN";
			this.txtSSN.Size = new System.Drawing.Size(104, 20);
			this.txtSSN.TabIndex = 0;
			this.txtSSN.Text = "111-22-3333";
			// 
			// txtFName
			// 
			this.txtFName.Location = new System.Drawing.Point(112, 64);
			this.txtFName.Name = "txtFName";
			this.txtFName.Size = new System.Drawing.Size(104, 20);
			this.txtFName.TabIndex = 1;
			this.txtFName.Text = "John";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(24, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "First Name";
			// 
			// txtMI
			// 
			this.txtMI.Location = new System.Drawing.Point(296, 64);
			this.txtMI.Name = "txtMI";
			this.txtMI.Size = new System.Drawing.Size(32, 20);
			this.txtMI.TabIndex = 2;
			this.txtMI.Text = "";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(256, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "MI";
			// 
			// txtLName
			// 
			this.txtLName.Location = new System.Drawing.Point(472, 64);
			this.txtLName.Name = "txtLName";
			this.txtLName.Size = new System.Drawing.Size(104, 20);
			this.txtLName.TabIndex = 3;
			this.txtLName.Text = "Dough";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(352, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 24);
			this.label4.TabIndex = 6;
			this.label4.Text = "Last Name";
			// 
			// txtAddr1
			// 
			this.txtAddr1.Location = new System.Drawing.Point(112, 112);
			this.txtAddr1.Name = "txtAddr1";
			this.txtAddr1.Size = new System.Drawing.Size(104, 20);
			this.txtAddr1.TabIndex = 4;
			this.txtAddr1.Text = "";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 24);
			this.label5.TabIndex = 8;
			this.label5.Text = "Address 1";
			// 
			// txtAddr2
			// 
			this.txtAddr2.Location = new System.Drawing.Point(352, 112);
			this.txtAddr2.Name = "txtAddr2";
			this.txtAddr2.Size = new System.Drawing.Size(104, 20);
			this.txtAddr2.TabIndex = 5;
			this.txtAddr2.Text = "";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(256, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 24);
			this.label6.TabIndex = 10;
			this.label6.Text = "Address 2";
			// 
			// txtCity
			// 
			this.txtCity.Location = new System.Drawing.Point(112, 144);
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size(104, 20);
			this.txtCity.TabIndex = 6;
			this.txtCity.Text = "";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(24, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 24);
			this.label7.TabIndex = 12;
			this.label7.Text = "City";
			// 
			// cmbState
			// 
			this.cmbState.Items.AddRange(new object[] {
														  "CT",
														  "NJ",
														  "NY",
														  "PA"});
			this.cmbState.Location = new System.Drawing.Point(352, 144);
			this.cmbState.Name = "cmbState";
			this.cmbState.Size = new System.Drawing.Size(56, 21);
			this.cmbState.TabIndex = 7;
			this.cmbState.Text = "NY";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(256, 144);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 24);
			this.label8.TabIndex = 15;
			this.label8.Text = "State";
			// 
			// txtZip
			// 
			this.txtZip.Location = new System.Drawing.Point(472, 144);
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size(64, 20);
			this.txtZip.TabIndex = 8;
			this.txtZip.Text = "";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(424, 144);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(40, 24);
			this.label9.TabIndex = 16;
			this.label9.Text = "Zip";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(256, 208);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(88, 24);
			this.label10.TabIndex = 18;
			this.label10.Text = "Date of Birth";
			// 
			// dtDOB
			// 
			this.dtDOB.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dtDOB.Location = new System.Drawing.Point(352, 208);
			this.dtDOB.Name = "dtDOB";
			this.dtDOB.Size = new System.Drawing.Size(240, 20);
			this.dtDOB.TabIndex = 12;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(24, 208);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(80, 24);
			this.label11.TabIndex = 21;
			this.label11.Text = "Gender";
			// 
			// cmbGender
			// 
			this.cmbGender.Items.AddRange(new object[] {
														   "Male",
														   "Female"});
			this.cmbGender.Location = new System.Drawing.Point(112, 208);
			this.cmbGender.Name = "cmbGender";
			this.cmbGender.Size = new System.Drawing.Size(104, 21);
			this.cmbGender.TabIndex = 11;
			this.cmbGender.Text = "Male";
			// 
			// txtPhone1
			// 
			this.txtPhone1.Location = new System.Drawing.Point(112, 176);
			this.txtPhone1.Name = "txtPhone1";
			this.txtPhone1.Size = new System.Drawing.Size(104, 20);
			this.txtPhone1.TabIndex = 9;
			this.txtPhone1.Text = "";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(24, 176);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(88, 24);
			this.label12.TabIndex = 22;
			this.label12.Text = "Phone 1";
			// 
			// txtPhone2
			// 
			this.txtPhone2.Location = new System.Drawing.Point(352, 176);
			this.txtPhone2.Name = "txtPhone2";
			this.txtPhone2.Size = new System.Drawing.Size(104, 20);
			this.txtPhone2.TabIndex = 10;
			this.txtPhone2.Text = "";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label13.Location = new System.Drawing.Point(256, 176);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(88, 24);
			this.label13.TabIndex = 24;
			this.label13.Text = "Phone 2";
			// 
			// btnPrintBadge
			// 
			this.btnPrintBadge.Location = new System.Drawing.Point(176, 256);
			this.btnPrintBadge.Name = "btnPrintBadge";
			this.btnPrintBadge.Size = new System.Drawing.Size(96, 32);
			this.btnPrintBadge.TabIndex = 13;
			this.btnPrintBadge.Text = "Print Badge";
			this.btnPrintBadge.Click += new System.EventHandler(this.btnPrintBadge_Click);
			// 
			// btnNewBadge
			// 
			this.btnNewBadge.Location = new System.Drawing.Point(344, 256);
			this.btnNewBadge.Name = "btnNewBadge";
			this.btnNewBadge.Size = new System.Drawing.Size(96, 32);
			this.btnNewBadge.TabIndex = 14;
			this.btnNewBadge.Text = "New Badge";
			this.btnNewBadge.Click += new System.EventHandler(this.btnNewBadge_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(24, 40);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(88, 24);
			this.label14.TabIndex = 0;
			this.label14.Text = "SSN";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(24, 152);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(88, 24);
			this.label15.TabIndex = 12;
			this.label15.Text = "City";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(24, 184);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(88, 24);
			this.label16.TabIndex = 22;
			this.label16.Text = "Phone 1";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(24, 216);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(80, 24);
			this.label17.TabIndex = 21;
			this.label17.Text = "Gender";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(24, 120);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(88, 24);
			this.label18.TabIndex = 8;
			this.label18.Text = "Address 1";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(24, 72);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(88, 24);
			this.label19.TabIndex = 2;
			this.label19.Text = "First Name";
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(256, 72);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(32, 24);
			this.label20.TabIndex = 4;
			this.label20.Text = "MI";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(352, 72);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(88, 24);
			this.label21.TabIndex = 6;
			this.label21.Text = "Last Name";
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label22.Location = new System.Drawing.Point(256, 112);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(88, 24);
			this.label22.TabIndex = 10;
			this.label22.Text = "Address 2";
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label23.Location = new System.Drawing.Point(352, 72);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(88, 24);
			this.label23.TabIndex = 6;
			this.label23.Text = "Last Name";
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label24.Location = new System.Drawing.Point(256, 72);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(32, 24);
			this.label24.TabIndex = 4;
			this.label24.Text = "MI";
			// 
			// label25
			// 
			this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label25.Location = new System.Drawing.Point(24, 72);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(88, 24);
			this.label25.TabIndex = 2;
			this.label25.Text = "First Name";
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label26.Location = new System.Drawing.Point(24, 120);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(88, 24);
			this.label26.TabIndex = 8;
			this.label26.Text = "Address 1";
			// 
			// label27
			// 
			this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label27.Location = new System.Drawing.Point(24, 216);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(80, 24);
			this.label27.TabIndex = 21;
			this.label27.Text = "Gender";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label28.Location = new System.Drawing.Point(24, 184);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(88, 24);
			this.label28.TabIndex = 22;
			this.label28.Text = "Phone 1";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label29.Location = new System.Drawing.Point(24, 152);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(88, 24);
			this.label29.TabIndex = 12;
			this.label29.Text = "City";
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label30.Location = new System.Drawing.Point(24, 40);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(88, 24);
			this.label30.TabIndex = 0;
			this.label30.Text = "SSN";
			// 
			// lbDbgMsgs
			// 
			this.lbDbgMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbDbgMsgs.HorizontalScrollbar = true;
			this.lbDbgMsgs.Location = new System.Drawing.Point(16, 304);
			this.lbDbgMsgs.Name = "lbDbgMsgs";
			this.lbDbgMsgs.Size = new System.Drawing.Size(584, 212);
			this.lbDbgMsgs.TabIndex = 25;
			// 
			// Demo_Data_Entry_Application
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
			this.BackColor = System.Drawing.Color.DarkSalmon;
			this.ClientSize = new System.Drawing.Size(616, 526);
			this.Controls.Add(this.lbDbgMsgs);
			this.Controls.Add(this.btnNewBadge);
			this.Controls.Add(this.btnPrintBadge);
			this.Controls.Add(this.txtPhone2);
			this.Controls.Add(this.txtPhone1);
			this.Controls.Add(this.txtZip);
			this.Controls.Add(this.txtCity);
			this.Controls.Add(this.txtAddr2);
			this.Controls.Add(this.txtAddr1);
			this.Controls.Add(this.txtLName);
			this.Controls.Add(this.txtMI);
			this.Controls.Add(this.txtFName);
			this.Controls.Add(this.txtSSN);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.cmbGender);
			this.Controls.Add(this.dtDOB);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.cmbState);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.label24);
			this.Controls.Add(this.label25);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.label27);
			this.Controls.Add(this.label28);
			this.Controls.Add(this.label29);
			this.Controls.Add(this.label30);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Demo_Data_Entry_Application";
			this.Text = "DOL Rapid Response System Demo";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Demo_Data_Entry_Application());
		}

		private void btnNewBadge_Click(object sender, System.EventArgs e) {
			txtSSN.Text		= "";
			txtFName.Text	= "";
			txtMI.Text		= "";
			txtLName.Text	= "";
			txtAddr1.Text	= "";
			txtAddr2.Text	= "";
			txtCity.Text	= "";
			cmbState.Text	= "NY";
			txtZip.Text		= "";
			txtPhone1.Text	= "";
			txtPhone2.Text	= "";
			cmbGender.Text	= "";
			dtDOB.Value		= new DateTime(1982, 1, 1);
		}

		private void btnPrintBadge_Click(object sender, System.EventArgs e) {
			Hashtable	ret;
			try {
				ret = prt.PrintSSN(txtSSN.Text, txtFName.Text, txtLName.Text);
				// MessageBox.Show("Return value from PrintSSN = " + ret);
				foreach (string s in ret.Keys) {
					switch (s) {
					case "CUSTDTL1_PH":
						txtPhone1.Text = (string)ret[s];
						break;
					case "CUSTDTL1_ALT":
						txtPhone2.Text = (string)ret[s];
						break;
					}
				}
			} catch (FileNotFoundException ex) {
				lbDbgMsgs.Items.Add("File not found exception - " + ex.FileName);
				lbDbgMsgs.Items.AddRange(ex.FusionLog.Split('\n'));
			}

#if false
			PrintDocument pd = new PrintDocument();
			pd.DocumentName = "BartCarte";
			pd.PrinterSettings.PrinterName = @"\\LRS-P4-1\LRS-940";
			// pd.PrintController = new PreviewPrintController();
			pd.PrintPage += new
				System.Drawing.Printing.PrintPageEventHandler
				(this.printDocument1_PrintPage);			
			pd.Print();
#endif
		}

#if false
		private void printDocument1_PrintPage(object sender, 
			System.Drawing.Printing.PrintPageEventArgs e) {
			e.Graphics.DrawString("SampleText", 
				new Font("Arial", 32, FontStyle.Bold), Brushes.Black, 150, 125);
		}
#endif

	}
}
