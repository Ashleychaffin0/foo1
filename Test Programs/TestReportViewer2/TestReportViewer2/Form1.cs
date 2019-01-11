using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestReportViewer2 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'lldevelDataSet.tblAccounts' table. You can move, or remove it, as needed.
			this.tblAccountsTableAdapter.Fill(this.lldevelDataSet.tblAccounts);

			// this.reportViewer1.RefreshReport();
			this.reportViewer1.RefreshReport();
		}

		private void tblAccountsBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
			this.Validate();
			this.tblAccountsBindingSource.EndEdit();
			this.tblAccountsTableAdapter.Update(this.lldevelDataSet.tblAccounts);

		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

#if false
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
#endif
}