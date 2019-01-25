using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bartizan.Utils.Compression;
using Bartizan.Utils.SQL;

// Reasonable article at http://www.simple-talk.com/dotnet/.net-framework/schema-and-metadata-retrieval-using-ado.net/

namespace SSSchema {
	public partial class SSSChema : Form {

		SqlConnection	conn;

		public SSSChema() {
			InitializeComponent();
			// string connString = LLConnection.GetSqlStringBuilder("PROD").ToString();
			string connString = LLConnection.GetSqlStringBuilder("DEVEL").ToString();
			conn = new SqlConnection(connString);
			conn.Open();

#if false
			string a = null, b = null, c = null, d = null, e = null;
			string SprocName = "spLL_App_UpdatetblAccounts";
			// a = SprocName;
			// b = SprocName;
			c = SprocName;
			// d = SprocName;
			string [] restrictions = { a, b, c, d};
			// string [] restrictions = { a };
			// TODO: Figure out which of dt/dt2 to use
#if true
			DataTable dtProcs = conn.GetSchema(SqlClientMetaDataCollectionNames.Procedures);
			dataGridView1.DataSource = dtProcs;
			DataTable dt2 = conn.GetSchema("ProcedureParameters",
								restrictions);
			dataGridView2.DataSource = dt2;
			conn.Close();
			if (conn != null)
				return;
#endif
#endif

			// DecompressData(conn);

			SSDB	ssdb = new SSDB(conn, null);
			// SSDB	ssdb = new SSDB(conn, x => ! x.TableName.StartsWith("_temp"));
			// SSDB	ssdb = new SSDB(conn, null);
			string s = ssdb.DumpAsString();
			Clipboard.SetText(s);

			DataTable	dt1 = conn.GetSchema(SqlClientMetaDataCollectionNames.Indexes, new string [] {null, null, "tblImportTracking", null});
			dataGridView1.DataSource = dt1;
			// DataTable dtSchema = conn.GetSchema("Tables", new string [] {null, null, null, "TABLE SCHEMA"});
			DataTable dtSchema = conn.GetSchema("restrictions");
			// dataGridView1.DataSource = dtSchema;
			DataTable dt3 = conn.GetSchema();
			dataGridView2.DataSource = dt3;

			DataTable	dtCols = conn.GetSchema("Columns", new string [] {null, null, "tblImportTracking", null});
			// dataGridView1.DataSource = dtCols;
#if false
			foreach (DataRow row in dtSchema.Rows) {
				for (int i = 0; i < dtSchema.Columns.Count; i++) {
					Console.WriteLine("{0}: {1} = {2}", i, dtSchema.Columns[i].ColumnName, row[i]); 
				}
				Console.WriteLine();
			}
#endif
			var tblNames = from DataRow entry in dtSchema.AsEnumerable()
						   // let Name = entry["TABLE_NAME"].ToString()
						   // let TableType = entry["TABLE_TYPE"].ToString()
						   // where (!((Name.StartsWith("_temp")) || (Name.Contains("aspnet_"))) && (TableType == "BASE TABLE"))
						   // orderby Name
						   select entry;

			foreach (var item in tblNames) {
				// Console.WriteLine("{0}", item["TABLE_NAME"]);
				Console.WriteLine(item);
			}
			
			conn.Close();
		}

//---------------------------------------------------------------------------------------

		private void DecompressData(SqlConnection conn) {
			// string SQL = "SELECT * FROM tblSavedImports WHERE SavedImportID = 29454";
			string SQL = "SELECT * FROM tblSavedImports WHERE SavedImportID between 57869 and 57869";
			SqlCommand	cmd = new SqlCommand(SQL, conn);
			SqlDataReader	rdr = cmd.ExecuteReader();
			string data = "";
			while (rdr.Read()) {
				string CompData = (string)rdr["VisitorData"];
				data += "\n\n\n" + (int)rdr["SavedImportID"] + ": " + BartCompress.Decompress(CompData);
			}
			rdr.Close();
			Clipboard.SetText(data);
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public static class LLConnection {

//---------------------------------------------------------------------------------------

			public static SqlConnectionStringBuilder GetSqlStringBuilder(string Name) {
				SqlConnectionStringBuilder builder = null;
				switch (Name.ToUpper()) {
				case "PROD":
				case "PRODUCTION":
#if true
					builder = new SqlConnectionStringBuilder();
					builder.DataSource = "198.64.249.6,1092";
					builder.InitialCatalog = "LeadsLightning";
					builder.UserID = "ahmed";
					builder.Password = "i7e9dua$tda@";
#else
					MessageBox.Show("Nonce on GetSqlStringBuilder(\"Production\"");
#endif
					break;
				case "DEVEL":
					builder = new SqlConnectionStringBuilder();
					builder.DataSource = "SQLB5.webcontrolcenter.com";
					builder.InitialCatalog = "LLDevel";
					builder.UserID = "ahmed";
					builder.Password = "i7e9dua$tda@";
					break;
				default:
					MessageBox.Show("GetSqlStringBuilder - Unknown database - " + Name);
					break;
				}
				return builder;
			}
		}
	}
}
