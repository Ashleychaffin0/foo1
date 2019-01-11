using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestDataGridViewCustomColumns {
	public partial class TestDataGridViewCustomColumns : Form {
		public TestDataGridViewCustomColumns() {
			InitializeComponent();

			// How to: Host Controls in Windows Forms DataGridView Cells
			// http://msdn.microsoft.com/en-us/library/7tas5c80.aspx


			// Init Field Names
			FieldName.Items.AddRange(new string[] {
				"First Name",
				"Last Name",
				"City",
				"State",
				"Zip" });
		}
	}
}
