using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Net;
using System.Net.Sockets;
using System.Text;

using SourceSafeTypeLib;

namespace ConsumeLLWebService1	{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btnConsumeService1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1() {
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
		protected override void Dispose(bool disposing)	{
			if (disposing) {
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
			this.btnConsumeService1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnConsumeService1
			// 
			this.btnConsumeService1.Location = new System.Drawing.Point(32, 24);
			this.btnConsumeService1.Name = "btnConsumeService1";
			this.btnConsumeService1.Size = new System.Drawing.Size(136, 40);
			this.btnConsumeService1.TabIndex = 0;
			this.btnConsumeService1.Text = "Consume Service 1";
			this.btnConsumeService1.Click += new System.EventHandler(this.btnConsumeService1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Result";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(232, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 40);
			this.button1.TabIndex = 3;
			this.button1.Text = "Consume Service via sockets";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtResult
			// 
			this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtResult.Location = new System.Drawing.Point(160, 96);
			this.txtResult.Multiline = true;
			this.txtResult.Name = "txtResult";
			this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResult.Size = new System.Drawing.Size(584, 224);
			this.txtResult.TabIndex = 4;
			this.txtResult.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(424, 24);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 40);
			this.button2.TabIndex = 5;
			this.button2.Text = "Test SSAPI";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(760, 336);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnConsumeService1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		private void btnConsumeService1_Click(object sender, System.EventArgs e) {
			LLService1 ll = new LLService1();
			string s = ll.HelloWorld();
			txtResult.Text = s;
		}

		private void button1_Click(object sender, System.EventArgs e) {
			string	body = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <HelloWorld xmlns=""http://tempuri.org/"" />
  </soap:Body>
</soap:Envelope>
";
			string s = string.Format(@"POST /LL1/LLService1.asmx HTTP/1.1
Host: 192.168.0.114
Content-Type: text/xml; charset=utf-8
Content-Length: {0}
SOAPAction: ""http://tempuri.org/HelloWorld""

", body.Length);

			Socket	sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			IPAddress	IPAddr = IPAddress.Parse("192.168.0.114");
			EndPoint	ep = new IPEndPoint(IPAddr, 80);
			sock.Connect(ep);
			byte [] buf1 = Encoding.ASCII.GetBytes(s + body);
			sock.Send(buf1);

			byte [] buf2 = new byte[4000];
			sock.Receive(buf2);
			string resp2 = Encoding.ASCII.GetString(buf2);

			byte [] buf3 = new byte[4000];
			sock.Receive(buf3);
			string resp3 = Encoding.ASCII.GetString(buf3);
			txtResult.Text = resp3;
		}

		private void button2_Click(object sender, System.EventArgs e) {
			VSSDatabase	ssdb = new VSSDatabase();
			ssdb.Open(@"L:\Shared\SourceSafe\SrcSafe.ini", "larrys", "bartset");
			
			VSSItem	projects = (VSSItem)ssdb.get_VSSItem("$/", false);
			object o = projects.get_Items(false);
				foreach (VSSItem item in projects.get_Items(false)) {
					Console.WriteLine("item name = {0}", item.Name);
				}
			foreach (object item in projects.Links) {
				// Console.WriteLine(item.Name);
				Console.WriteLine(item.ToString());
			}
			MessageBox.Show("Done");
		}
	}
}
