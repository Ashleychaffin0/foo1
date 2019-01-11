using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace TestDatatelVOIPPhone {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			try {
				// SimpleConnect();
				Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				IPAddress addr;
				addr = new IPAddress(new byte[] { 129, 44, 45, 148 });
				sock.ReceiveTimeout = 3000;
				sock.Connect(addr, 6254);
				byte [] SendBuf = ASCIIEncoding.ASCII.GetBytes("init_mc");
				sock.Send(SendBuf);
				byte[] RecvBuf = new byte[32 * 1024];
				int rc = sock.Receive(RecvBuf);
				sock.Disconnect(false);
			} catch (Exception ex) {
				MessageBox.Show("Exception: " + ex.ToString());
			}
		}

		private void SimpleConnect() {
			Socket sock2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress addr;
			addr = new IPAddress(new byte[] { 207, 46, 232, 182 });
			sock2.ReceiveTimeout = 3000;
			sock2.Connect(addr, 80);
			sock2.Send(ASCIIEncoding.ASCII.GetBytes("GET /"));
			byte[] buf = new byte[16 * 1024];
			int rc = sock2.Receive(buf);
			sock2.Disconnect(false);
		}
	}
}