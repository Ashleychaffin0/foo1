using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReportViewer_2_2010 {
	public partial class Form1 : Form {

		public List<WorkItem> Items = new List<WorkItem>();

		public Form1() {
			InitializeComponent();

			Items.Add(new WorkItem(1, "Project 1", 30, 240));
			Items.Add(new WorkItem(2, "Project 2", 50, 40));
			Items.Add(new WorkItem(3, "Project 3", 60, 10));
			Items.Add(new WorkItem(4, "Project 4", 30, 20));
			Items.Add(new WorkItem(5, "Project 5", 20, 40));
		}

		private void Form1_Load(object sender, EventArgs e) {

			this.reportViewer1.RefreshReport();
		}
	}
	public class WorkItem {
		public int ID { get; set; }
		public string Description { get; set; }
		public int Planned { get; set; }
		public int Actual { get; set; }

		public WorkItem(int id, string desc, int planned, int actual) {
			ID = id;
			Description = desc;
			Planned = planned;
			Actual = actual;
		}
	}
}
