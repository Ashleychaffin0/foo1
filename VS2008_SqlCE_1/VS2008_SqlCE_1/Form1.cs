using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace VS2008_SqlCE_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {

		}

		private void button1_Click(object sender, EventArgs e) {
			// Next line just for Expression Visualizer
			Expression<Func<string, bool>> expr = name => name == "LRS";
			(sender as Button).Text = "Ouch!";
		}
	}
}
