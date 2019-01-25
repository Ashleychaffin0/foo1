using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SQLiteWrapper;

// See http://www.switchonthecode.com/tutorials/csharp-tutorial-writing-a-dotnet-wrapper-for-sqlite
// See http://www.codeproject.com/KB/database/cs_sqlitewrapper.aspx

namespace CSharpPlusSqLiteDemo {

//---------------------------------------------------------------------------------------
	
	public partial class CSharpPlusSqLiteDemo : Form {
		public CSharpPlusSqLiteDemo() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			lbOutput.Items.Clear();
			SQLiteBase db = new SQLiteBase("sqlite.ind");
			// executes SELECT query and store results in new data table
			DataTable table = db.ExecuteQuery(txtSQL.Text);
			for (int i = 0; i < table.Rows.Count; i++) {
				DataRow row = table.Rows[i];
				for (int j = 0; j < row.ItemArray.GetLength(0); j++) {
					string msg = string.Format("Row {0} {1} = {2}", i, table.Columns[j].ColumnName, row[j]);
					lbOutput.Items.Add(msg);
				}
				lbOutput.Items.Add("".PadRight(40, '-'));
			}
			db.CloseDatabase();
		}
	}
}
