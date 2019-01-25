using System;
using System.Data;
using System.Data.SQLite;           // NuGet this
using System.IO;

// https://sqlite.org/index.html

namespace TestConsoleWithSqlite_Nuget {
	public class LrsSQLite {
		string			 Filename = null;
		SQLiteConnection ConnDb = null;

//---------------------------------------------------------------------------------------

		public LrsSQLite(string filename) {
			Filename = filename;
			Connect();
		}

//---------------------------------------------------------------------------------------

		public static void CreateDatabase(string filename, bool reinitialize = false) {
			// Be careful!!! If the database file already exists, this will overwrite it
			// and create a zero-length file.
			if ((reinitialize == false) && File.Exists(filename)) {
				throw new ArgumentException($"File '{filename}' exists and {nameof(reinitialize)} is false");
			}
			SQLiteConnection.CreateFile(filename);
		}

//---------------------------------------------------------------------------------------

		public void Connect(string filename) {
			ConnDb = new SQLiteConnection($"Data Source={filename};Version=3;");
			ConnDb.Open();
		}

//---------------------------------------------------------------------------------------

		public void Connect() {
			if (Filename is null) {
				throw new FieldAccessException($"Filename not specified in {nameof(CreateDatabase)}");
			}
			Connect(Filename);
		}

//---------------------------------------------------------------------------------------

		public SQLiteDataReader ExecuteQuery(string sql) {
			var command = new SQLiteCommand(sql, ConnDb);
			return command.ExecuteReader();
		}

//---------------------------------------------------------------------------------------

		public int ExecuteNonQuery(string sql) {
			var command = new SQLiteCommand(sql, ConnDb);
			return command.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		// TODO: This method is for debugging
		public void DumpSchema(string name) {
			Console.WriteLine($"Dumping schema {name} ----------------------");
			var schema = ConnDb.GetSchema(name);
			Console.WriteLine($"\tColumns\r\n\t=======\t");
			Console.Write("\t");
			foreach (var sch in schema.Columns) {
				Console.Write($"{sch.ToString()}, ");
			}
			for (int RowNum = 0; RowNum < schema.Rows.Count; RowNum++) {
				var items = schema.Rows[RowNum].ItemArray;
				foreach (var item in items) {
					Console.WriteLine(item);
				}
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		public bool IsTableExists(string tableName) {
			var AllSchemae = ConnDb.GetSchema();
			Console.WriteLine("Schema data\r\n===========");
			foreach (DataRow row in AllSchemae.Rows) {
				var items = row.ItemArray;
				foreach (var item in items) {
					switch (item) {
					case string schema:
						// Console.WriteLine(s);
						DumpSchema(schema);
						break;
					default:
						break;
					}
				}
			}


			string sql = $@"select * from sqlite_master";
			var rdr = ExecuteQuery(sql);
			bool ret = rdr.HasRows;
			while (rdr.Read()) {
				// Step through each row
				while (rdr.Read()) {
					Console.WriteLine("--------------------------");
					for (int a = 0; a < rdr.FieldCount; a++) {
						// This will give you the name of the current row's column
						string columnName = rdr.GetName(a);

						// This will give you the value of the current row's column
						string columnValue = rdr[a].ToString();
						Console.WriteLine($"[{a}]: {columnName} => {columnValue}");
					}
					foreach (var item in rdr) {
						// string name = rdr.GetString(0);
						// string name2 = item.GetType["name"];
						// Console.WriteLine($"{item[\"name\"]}");
					}
				}
			}
			return ret;
		}
	}
}
