using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using Microsoft.Win32;
using System.Diagnostics;

// Note: See http://zetcode.com/csharp/sqlite/
// Note: NuGet: System.Data.SQLite.Core

namespace SqLiteTest1 {
	public partial class SqLiteTest1 : Form {
		public SqLiteTest1() {
			InitializeComponent();

			using (var ndpKey =
			  RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
			  OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\")) {
				foreach (string versionKeyName in ndpKey.GetSubKeyNames()) {
					Debug.WriteLine(versionKeyName);
				}
			}

			SlTest1();
			SlTest2();
		}

//---------------------------------------------------------------------------------------

		private static void SlTest1() {
			string cs = "Data Source=:memory:";
			string stm = "SELECT SQLITE_VERSION()";
			var con = new SQLiteConnection(cs);
			con.Open();
			var cmd = new SQLiteCommand(stm, con);
			string version = cmd.ExecuteScalar().ToString();

			Console.WriteLine($"SQLite version: {version}");
		}

//---------------------------------------------------------------------------------------

		private void SlTest2() {
			// string cs = @"URI=file:C:\Users\Jano\Documents\test.db";
			string cs = @"Data Source=:memory:";

			using var con = new SQLiteConnection(cs);
			con.Open();

			using var cmd = new SQLiteCommand(con);

			cmd.CommandText = "DROP TABLE IF EXISTS cars";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY,
                    name TEXT, price INT)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Audi',52642)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Mercedes',57127)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Skoda',9000)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volvo',29000)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Bentley',350000)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Citroen',21000)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Hummer',41400)";
			cmd.ExecuteNonQuery();

			cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volkswagen',21600)";
			cmd.ExecuteNonQuery();

			Console.WriteLine("Table cars created");
		}

	}
}
