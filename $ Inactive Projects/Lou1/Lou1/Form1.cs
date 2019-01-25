using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lou1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			float	X = float.Parse(txtX.Text);
			float	Ans = (X * 32 + 5) / (X * X);
			txtAnswer.Text = Ans.ToString();
		}
	}
}