using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateNewSqlLocalDbDatabase {
	public partial class CreateNewSqlLocalDbDatabase : Form {
		public CreateNewSqlLocalDbDatabase() {
			InitializeComponent();

			if (Environment.MachineName == "LRS8500-PC") {
				txtDirectoryName.Text = @"D:\LRS";
			}
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			var res = fbd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			txtDirectoryName.Text = fbd.SelectedPath;
		}

//---------------------------------------------------------------------------------------

		private void CreateLocalDb(string DirName, string dbName) {
			// See http://msdn.microsoft.com/en-us/library/hh510202.aspx

			string filename = Path.Combine(DirName, dbName);

			string connString = @"Server=(localdb)\V11.0; Integrated Security=True;";
			using (var conn = new SqlConnection(connString)) {
				var SQL = "CREATE DATABASE " + dbName +
					" ON PRIMARY" +
					" (NAME = " + dbName + "_data," +
					" FILENAME = '" + filename + ".mdf'," +
					" SIZE = 4MB," +
					// Note: With Express, the max size is 10 GB
//					" MAXSIZE = 120GB," +
					" FILEGROWTH = 10%)" +

					" LOG ON" +
					" (NAME = " + dbName + "_log," +
					" FILENAME = '" + filename + ".ldf'," +
					" SIZE = 1MB," +
					" MAXSIZE = 5MB," +
					" FILEGROWTH = 10%)" +
					";";
				var cmd = new SqlCommand(SQL, conn);
				try {
					conn.Open();
					int res = cmd.ExecuteNonQuery();
					MessageBox.Show("Database Created", "Create Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, "Create Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			CreateLocalDb(txtDirectoryName.Text, txtDatabaseName.Text);
		}
	}
}
