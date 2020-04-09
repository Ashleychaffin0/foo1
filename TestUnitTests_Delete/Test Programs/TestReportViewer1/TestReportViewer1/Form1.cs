using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Reporting.WinForms;

namespace TestReportViewer1 {
	public partial class Form1 : Form {

		public DataTable	tbl;

//---------------------------------------------------------------------------------------
		
		public Form1() {
			InitializeComponent();

			// GetData();
		}

//---------------------------------------------------------------------------------------

		private void GetData() {
			tbl = new DataTable("LRS");
			string	SQL = "SELECT * FROM tblAccounts";
			string	ConnString = ConnectToDatabase.GetConnectionString("DEVEL");

			using (SqlConnection conn = new SqlConnection(ConnString)) {
				conn.Open();
				SqlDataAdapter	adapt = new SqlDataAdapter(SQL, conn);
				adapt.Fill(tbl);
			}
			// rpt.LocalReport.DataSources.Add(new ReportDataSource("LRS", tbl));
			// dataSet1.Tables.Add(tbl);
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			rpt.RefreshReport();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ConnectToDatabase {

		// TODO: Add Ahmed-Local cbSystem entry
		public static string GetConnectionString(string system) {
			SqlConnectionStringBuilder bld = new SqlConnectionStringBuilder();
			switch (system.ToUpper()) {
			case "PROD":
			case "DBMART":			// DBMart
				bld.DataSource = "75.126.77.59,1092";
				bld.InitialCatalog = "LeadsLightning";
				bld.UserID = "sa";
				bld.Password = "$yclahtw2007bycnmhd!";
				break;
			case "DEVEL":
			case "CRYSTALTECH":		// CrystalTech
				bld.DataSource = "SQLB5.webcontrolcenter.com";
				bld.InitialCatalog = "LLDevel";
				bld.UserID = "ahmed";
				bld.Password = "i7e9dua$tda@";
				break;
#if false
			case "Ahmed-Local":
				throw new Exception("Ahmed-Local support not yet implemented");
				break;
#endif
			default:
				throw new Exception("Unknown System type - " + system);
			}
			return bld.ConnectionString;
		}
	}
}