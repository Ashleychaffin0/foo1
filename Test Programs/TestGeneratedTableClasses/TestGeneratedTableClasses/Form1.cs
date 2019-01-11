using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestGeneratedTableClasses {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			label1.Text = string.Format("{0} - {1}", performanceCounter1.CounterName, 
				performanceCounter1.RawValue);
		}
	}
}