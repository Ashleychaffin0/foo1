using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MimicWebSite	{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MimicWebSite : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ListBox lbMsgs;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		Socket		sock;
		IPEndPoint	ep;

		public MimicWebSite() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ShowInTaskbar = false;

		}

		#region Usual C# stuff
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
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(16, 56);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(472, 228);
			this.lbMsgs.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port";
			// 
			// txtPort
			// 
			this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPort.Location = new System.Drawing.Point(128, 16);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(64, 22);
			this.txtPort.TabIndex = 4;
			this.txtPort.Text = "80";
			this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(320, 16);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(64, 32);
			this.btnStart.TabIndex = 5;
			this.btnStart.Text = "Start";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(416, 16);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(64, 32);
			this.btnStop.TabIndex = 6;
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// MimicWebSite
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(504, 304);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbMsgs);
			this.Name = "MimicWebSite";
			this.Text = "Mimic Web Site";
			this.Load += new System.EventHandler(this.MimicWebSite_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new MimicWebSite());
		}
		#endregion

		private void MimicWebSite_Load(object sender, System.EventArgs e) {
		}

		private void btnStart_Click(object sender, System.EventArgs e) {
#if false
			sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			ep  = new IPEndPoint(IPAddress.Loopback, int.Parse(txtPort.Text));	// Ignore parse exceptions for now
			sock.Bind(ep);
			sock.Listen(10);
			if (! sock.Connected) {
				MessageBox.Show("Not connected after listen()");
				return;
			}
			Socket sock2 = sock.Accept();
			const int	MaxBufLen = 32000;
			byte []	buf = new byte[MaxBufLen];
			sock2.Receive(buf);
			string		msg;
			msg = Encoding.UTF8.GetString(buf);
			sock.Close();
#else
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");
      
			// TcpListener server = new TcpListener(port);
			int		port;
			port = int.Parse(txtPort.Text);	// Ignore parse exceptions for now
			TcpListener server = new TcpListener(localAddr, port);

			const int	MaxBufLen = 32000;
			byte []		bytes = new byte[MaxBufLen];
			string		data;

			// Start listening for client requests.
			server.Start();

			this.WindowState = FormWindowState.Minimized;

			while (true) {
				try {
					Console.Write("Waiting for a connection... ");
        
					// Perform a blocking call to accept requests.
					// You could also use server.AcceptSocket() here.
					TcpClient client = server.AcceptTcpClient();            
					Console.WriteLine("Connected!");

					data = null;

					// Get a stream object for reading and writing
					NetworkStream stream = client.GetStream();

					int i;

					// Loop to receive all the data sent by the client.
					while((i = stream.Read(bytes, 0, bytes.Length))!=0) {   
						// Translate data bytes to a ASCII string.
						data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
						lbMsgs.Items.Add(data);
						Application.DoEvents();
						Console.WriteLine(String.Format("Received: {0}", data));
       
						// Process the data sent by the client.
						data = ResponseOK();
						// data = data.ToUpper();

						byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

						// Send back a response.
						stream.Write(msg, 0, msg.Length);
						Console.WriteLine(String.Format("Sent: {0}", data));            
					}
         
					// Shutdown and end connection
					client.Close();
				} catch {
				}
			}
#endif
		}

		string ResponseOK() {
			string	msg, msg2;
			msg = "HTTP/1.1 200 OK\r\n";
			msg += "Content-type: text/html\r\n";

			msg2 = "<HTML>\r\n";
				msg2 += "\t<HEAD>\r\n";
					msg2 += "\t\t<TITLE>IBM site hacked</TITLE>\r\n";
				msg2 += "\t</HEAD>\r\n\r\n";
				msg2 += "\t<BODY>\r\n";
					msg2 += @"<FONT SIZE=""+3"" COLOR=""RED""><p>&nbsp;<p>&nbsp;<p>&nbsp;<p>Site HACKED!!!</FONT>";
				msg2 += "</BODY>\r\n";
			msg2 += "</HTML>\r\n";

			msg += "Content-length: " + msg2.Length.ToString() + "\r\n";
			msg += "\r\n";

			return msg + msg2;
		}

		private void btnStop_Click(object sender, System.EventArgs e) {
			if (sock != null)
				sock.Close();
			sock = null;
			// btnStart.Enabled = true;
		}
	}
}
