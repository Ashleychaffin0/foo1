using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DBTableSizes {
	public partial class DBTablesSizes : Form {

		public List<DBTableSize> Sizes = null;

//---------------------------------------------------------------------------------------

		public DBTablesSizes() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnReadDatabase_Click(object sender, EventArgs e) {
#if false
			Sizes = new List<DBTableSize>();
			Sizes.Add(new DBTableSize("MyName", "123", "234 KB", "456 MB", "12 KB", "34 KB"));
#else
			Cursor	CurCurs = this.Cursor;
			try {
				this.Cursor = Cursors.WaitCursor;
				string	dbName = (string)cmbDBs.SelectedItem;
				SqlConnectionStringBuilder builder = SelectDB(dbName);
				Sizes = DBTableSize.GetSizes(builder.ConnectionString);
			} finally {
				this.Cursor = CurCurs;
			}
			grdTableSizes.AutoGenerateColumns = true;
			grdTableSizes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			SortByName();
			grdTableSizes.DataSource = Sizes;

			this.Text = string.Format("Database Table Sizes - {0}, Size = {1}, Unallocated = {2}, Used = {3}",
				DBTableSize.DbName, DBTableSize.DbSize, DBTableSize.DbUnallocSpace, DBTableSize.DbUsed);

			grdTableSizes.Columns["Name"].DisplayIndex = 0;
			grdTableSizes.Columns["Rows"].DisplayIndex = 1;
			grdTableSizes.Columns["Data"].DisplayIndex = 2;
			grdTableSizes.Columns["Index_size"].DisplayIndex = 3;
			grdTableSizes.Columns["Reserved"].DisplayIndex = 4;
			grdTableSizes.Columns["Unused"].DisplayIndex = 5;

			grdTableSizes.Columns["Rows"].DefaultCellStyle.Format = "N0";
			grdTableSizes.Columns["Reserved"].DefaultCellStyle.Format = "N0";
			grdTableSizes.Columns["Index_size"].DefaultCellStyle.Format = "N0";
			grdTableSizes.Columns["Unused"].DefaultCellStyle.Format = "N0";
			grdTableSizes.Columns["Data"].DefaultCellStyle.Format = "N0";
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnSortByName_Click(object sender, EventArgs e) {
			SortByName();
			ReDisplay();
		}

//---------------------------------------------------------------------------------------

		private void ReDisplay() {
			grdTableSizes.Refresh();
			grdTableSizes.FirstDisplayedCell = grdTableSizes[0, 0];
		}

//---------------------------------------------------------------------------------------

		private void SortByName() {
			Sizes.Sort(delegate(DBTableSize x, DBTableSize y) 
						{ return x.Name.CompareTo(y.Name); });
		}

//---------------------------------------------------------------------------------------

		private void btnSortBySize_Click(object sender, EventArgs e) {
			Sizes.Sort(delegate(DBTableSize x, DBTableSize y) 
						{ return y.Data.CompareTo(x.Data); });
			ReDisplay();
		}

//---------------------------------------------------------------------------------------

		private void btnSortByIndexSize_Click(object sender, EventArgs e) {
			Sizes.Sort(delegate(DBTableSize x, DBTableSize y) 
						{ return y.Index_size.CompareTo(x.Index_size); });
			ReDisplay();
		}

//---------------------------------------------------------------------------------------

		private void btnSortByReserved_Click(object sender, EventArgs e) {
			Sizes.Sort(delegate(DBTableSize x, DBTableSize y) 
						{ return y.Reserved.CompareTo(x.Reserved); });
			ReDisplay();
		}

//---------------------------------------------------------------------------------------

		private void btnSortByRows_Click(object sender, EventArgs e) {
			Sizes.Sort(delegate(DBTableSize x, DBTableSize y) 
						{ return y.Rows.CompareTo(x.Rows); });
			ReDisplay();
		}

//---------------------------------------------------------------------------------------

		private void btnExport_Click(object sender, EventArgs e) {
			string Filename = @"C:\DBTableSizes - {0} - {1:yyyy-MM-dd H-mm-ss}.csv";
			Filename = string.Format(Filename, cmbDBs.SelectedItem, DateTime.Now);
			using (StreamWriter wtr = new StreamWriter(Filename)) {
				wtr.WriteLine(@"""Name"",""Rows"",""Reserved"",""Data"",""Index_Size"",""Unused"",""{0}"",""DB={1}"",""DBSize={2}"",""DBUnalloc={3}"",""DBUsed={4}""",
					DateTime.Now, cmbDBs.SelectedItem, 
					DBTableSize.DbSize, DBTableSize.DbUnallocSpace, DBTableSize.DbUsed);
				string	fmt = @"""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}""";
				foreach (DBTableSize ts in Sizes) {
					wtr.WriteLine(fmt, ts.Name, ts.Rows, ts.Reserved, ts.Data, ts.Index_size, ts.Unused);
				}
				wtr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		private void grdTableSizes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
#if false
			ListSortDirection asc = ListSortDirection.Ascending;
			ListSortDirection desc = ListSortDirection.Descending;
			ListSortDirection dir = grdTableSizes.SortOrder == SortOrder.Ascending ? desc : asc;	// If SortOrder.None, then -> asc
			// grdTableSizes.Sort(grdTableSizes.Columns[e.ColumnIndex], dir);
			SortByName();
			grdTableSizes.DataSource = Sizes;
#endif
		}

//---------------------------------------------------------------------------------------

		private static SqlConnectionStringBuilder SelectDB(string DBName) {
			// Must match entries in Form Load event
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			switch (DBName.ToUpper()) {
			case "PRODUCTION":
				// builder.DataSource = "75.126.77.59,1092";
				builder.DataSource = "198.64.249.6,1092";
				builder.InitialCatalog = "LeadsLightning";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				break;

			case "DEVEL":
				builder.DataSource = "SQLB5.webcontrolcenter.com";
				builder.InitialCatalog = "LLDevel";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				break;

			case "DEMO":
				builder.DataSource = "SQLB13.webcontrolcenter.com";
				builder.InitialCatalog = "leadslightning";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				break;

			default:
				throw new Exception("SelectDB - Unknown database '{0}'. Valid IDs are PROD/DEVEL.");
			}

			return builder;
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			// Must match SelectDB
			cmbDBs.Items.Add("Production");
			cmbDBs.Items.Add("Devel");
			cmbDBs.Items.Add("Demo");
			cmbDBs.SelectedIndex = 0;
		}
	}
}