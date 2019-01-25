using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VS2008_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

//--------------

		private void button1_Click(object sender, EventArgs e) {
			LRSDataClasses1DataContext dc = new LRSDataClasses1DataContext();

			dc.Log = Console.Out;
			DateTime	d = new DateTime(2007, 1, 1);

			var qry = from evt in dc.tblEvents
					  where evt.EventStartDate >= d
					  orderby evt.EventCity, evt.EventStartDate
					  select new { evt.EventCity, evt.EventStartDate, evt.EventName };
					  // select evt;

			dataGridView1.DataSource = qry;

			var x = qry.Expression.ToString();
			Console.WriteLine("Expression={0}", x);

			foreach (var item in qry) {
				Console.WriteLine("{0} - {1}, {2}",
					item.EventCity, item.EventStartDate, item.EventName);
			}
		}
	}
}
