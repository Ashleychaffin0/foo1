using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

namespace TestDB1 {
	class Program {

		static string	SysID = "PROD";

		static void Main(string[] args) {
			SqlConnection conn = GetDBConn();
			conn.Open();
			string SQL = "SELECT MapCfgContents from tblMapCfg where mapcfgid = 3239";
			SqlCommand	cmd = new SqlCommand(SQL, conn);

			string result;
			result = (string)cmd.ExecuteScalar();
			result = result.Replace("\n", "\r\n");
		}

//---------------------------------------------------------------------------------------

		private static SqlConnection GetDBConn() {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			switch (SysID.ToUpper()) {
			case "PROD":
				builder.DataSource = "198.64.249.6,1092";
				builder.InitialCatalog = "LeadsLightning";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				break;
			case "DEVEL":
			case "DEMO":
				builder.DataSource = "SQLB5.webcontrolcenter.com";
				builder.InitialCatalog = "LLDevel";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				break;
			default:
				return null;		// Caller will send email, log to database, etc
			}

			SqlConnection conn = new SqlConnection(builder.ConnectionString);
			return conn;
		}

	}
}
