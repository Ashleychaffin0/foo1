using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlServerCe;

namespace JasSsce1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			string dbname = @"C:\LRS\JAS.sdf";
			// dbname = @"C:\Program Files\Microsoft SQL Server Compact Edition\v3.5\Samples\Northwind.sdf";
			string connect = "datasource=" + dbname;
			// connect += "; integrated security=true";

			using (SqlCeConnection cn = new SqlCeConnection(connect)) {

#if false
				CreateDatabase(dbname, connect);
#endif
				cn.Open();

				// ShowSchema(cn);

				// Note: class JAS generated from Sqlmetal in
				//		 C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin
				var db = new JasDb(cn);
				db.Log = Console.Out;

				// AddData(db);

				var cp = db.GetTable<CoolPeople>();
				var q1 = from p in cp
						 select p;
				foreach (var item in q1) {
					Console.WriteLine("{0} {1} at {2}", item.FirstName, item.LastName, item.URL);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void AddData(JasDb db) {
			CoolPeople cp = new CoolPeople();
			cp.FirstName  = "Larry";
			cp.LastName   = "Smith";
			cp.URL        = "http://www.lrs5.net";
			db.CoolPeople.InsertOnSubmit(cp);

			cp           = new CoolPeople();
			cp.FirstName = "Jeremy";
			cp.LastName  = "Smith";
			cp.URL       = "http://www.sanfransys.com/";
			db.CoolPeople.InsertOnSubmit(cp);

			db.SubmitChanges();
		}

//---------------------------------------------------------------------------------------

		private static void ShowSchema(SqlCeConnection cn) {
			// var cmd = new SqlCeCommand("SELECT *  FROM INFORMATION_SCHEMA.Tables WHERE TABLE_TYPE='SYSTEM TABLE'", cn);
			SqlCeCommand cmd = new SqlCeCommand("SELECT *  FROM INFORMATION_SCHEMA.Tables", cn);
			ShowRdr(cmd);

			string sql = "SELECT * FROM INFORMATION_SCHEMA.Columns as cols WHERE cols.TABLE_NAME = 'CoolPeople'";
			cmd = new SqlCeCommand(sql, cn);
			ShowRdr(cmd);
		}

//---------------------------------------------------------------------------------------

		private static void CreateDatabase(string dbname, string connect) {
			File.Delete(dbname);
			var engine = new SqlCeEngine(connect);
			engine.CreateDatabase();

			string sql = "create table CoolPeople ("
							+ "LastName nvarchar (40) not null, "
							+ "FirstName nvarchar (40) , "
							+ "URL nvarchar (256) PRIMARY KEY)";

			SqlCeConnection cn = new SqlCeConnection(connect);
			SqlCeCommand cmd = new SqlCeCommand(sql, cn);

			try {
				cn.Open();
				cmd.ExecuteNonQuery();
			} catch (SqlCeException sqlexception) {
				MessageBox.Show(sqlexception.Message, "Oh Crap.", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowRdr(SqlCeCommand cmd) {
			using (var rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					for (int i = 0; i < rdr.FieldCount; i++) {
						Console.WriteLine("data[{0} = {1}] = {2}", i, rdr.GetName(i), rdr[i]);
					}
					Console.WriteLine("--------------");
				}
			}
		}
	}
}
