using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReportsApplication6 {
	public partial class Form1 : Form {

		List<MyReportData> data = new List<MyReportData>();

		public Form1() {
			InitializeComponent();

			data.Add(new MyReportData("Steeleye Span", "Portfolio",
				new List<string>() {"Thomas the Rhymer", "Gaudete", "All Around My Hat"}));
			data.Add(new MyReportData("Beatles", "Beatlemania",
				new List<string>(){"Money", "Till There Was You", "All My Loving"}));
		}

		private void Form1_Load(object sender, EventArgs e) {
			// bindingSource1.DataSource = data;
			// reportViewer1.BindingContext bindingSource1;
			this.reportViewer1.RefreshReport();
		}
	}
}