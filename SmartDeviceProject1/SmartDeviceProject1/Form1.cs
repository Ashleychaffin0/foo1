using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartDeviceProject1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			// timer1.Interval = 1000;
			// timer1.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e) {
			lblTOD.Text = DateTime.Now.ToShortTimeString();
		}

		private void btnPushMe_Click(object sender, EventArgs e) {
			MessageBox.Show("Mmmm... That felt good");
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e) {
			Rectangle r1 = label2.ClientRectangle;
			r1.Offset(label2.Left, label2.Top);
			Rectangle r2 = label2.RectangleToClient(label2.ClientRectangle);
			Rectangle r = label2.RectangleToScreen(label2.ClientRectangle);
			if (r1.Contains(e.X, e.Y)) {
				label2.Text = DateTime.Now.ToShortTimeString();
			}
		}
	}
}