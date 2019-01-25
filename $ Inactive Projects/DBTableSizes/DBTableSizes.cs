using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBTableSizes {
	public class DBTableSize {
		string	name;
		long	rows;
		long	reserved;
		long	data;
		long	index_size;
		long	unused;

		// The following fields mean we can't have two DBTableSize instances at once.
		// For now this isn't an issue. I suppose later we could define a DBSize class
		// with this info, and a List<DBTableSize> in it...
		static string	dbName;
		static string	dbSize;
		static string	dbUnallocSpace;
		static string	dbUsed;

//---------------------------------------------------------------------------------------

		public static string DbName {
			get { return DBTableSize.dbName; }
			set { DBTableSize.dbName = value; }
		}

//---------------------------------------------------------------------------------------

		public static string DbSize {
			get { return DBTableSize.dbSize; }
			set { DBTableSize.dbSize = value; }
		}

//---------------------------------------------------------------------------------------

		public static string DbUnallocSpace {
			get { return DBTableSize.dbUnallocSpace; }
			set { DBTableSize.dbUnallocSpace = value; }
		}	

//---------------------------------------------------------------------------------------

		public static string DbUsed {
			get { return DBTableSize.dbUsed; }
			set { DBTableSize.dbUsed = value; }
		}

//---------------------------------------------------------------------------------------

		public string Name {
			get { return name; }
			set { name = value; }
		}

//---------------------------------------------------------------------------------------

		public long Rows {
			get { return rows; }
			set { rows = value; }
		}

//---------------------------------------------------------------------------------------

		public long Reserved {
			get { return reserved; }
			set { reserved = value; }
		}

//---------------------------------------------------------------------------------------

		public long Data {
			get { return data; }
			set { data = value; }
		}

//---------------------------------------------------------------------------------------

		public long Index_size {
			get { return index_size; }
			set { index_size = value; }
		}

//---------------------------------------------------------------------------------------

		public long Unused {
			get { return unused; }
			set { unused = value; }
		}

//---------------------------------------------------------------------------------------

		public DBTableSize(string name, string rows, string reserved, string data,
					string index_size, string unused) {
			this.name = name;
			this.rows = long.Parse(rows);
			this.reserved = Parse(reserved);
			this.data = Parse(data);
			this.index_size = Parse(index_size);
			this.unused = Parse(unused);
		}

//---------------------------------------------------------------------------------------

		// Returns size in KB
		public static long Parse(string s) {
 			string suffix = s.Substring(s.Length - 2);
			s = s.Substring(0, s.Length - 3);		// Strip "bKB"
			long		size = int.Parse(s);
			switch (suffix) {
			case "MB":
				size *= 1024;
				break;
			case "GB":
				size *= 1024 * 1024;
				break;
			default:
				break;
			}
			return size;
		}

//---------------------------------------------------------------------------------------

		// Returns size in KB
		public static float ParseFloat(string s) {
 			string suffix = s.Substring(s.Length - 2);
			s = s.Substring(0, s.Length - 3);		// Strip "bKB"
			float		size = float.Parse(s);
			switch (suffix) {
			case "MB":
				size *= 1024;
				break;
			case "GB":
				size *= 1024 * 1024;
				break;
			default:
				break;
			}
			return size;
		}

//---------------------------------------------------------------------------------------

		public static List<DBTableSize> GetSizes(string ConnectionString) {
			List<DBTableSize>	Sizes = new List<DBTableSize>();
			List<string>		TableNames = new List<string>();
			string				SQL;
			SqlCommand			cmd;
			SqlDataReader		rdr;
			using (SqlConnection conn = new SqlConnection(ConnectionString)) {
				conn.Open();

				SQL = "sp_spaceused";
				cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.StoredProcedure;
				rdr = cmd.ExecuteReader();
				while (rdr.Read()) {		// Only 1 row, but we'll loop anyway
					DBTableSize.DbName = (string)rdr["database_name"];
					DBTableSize.DbSize = (string)rdr["database_size"];
					DBTableSize.DbUnallocSpace = (string)rdr["unallocated space"];
					float Used = ParseFloat(DBTableSize.DbSize) - ParseFloat(DBTableSize.DbUnallocSpace);
					DBTableSize.DbUsed = string.Format("{0} MB", Used / 1024);
				}
				rdr.Close();

				SQL = "SELECT name FROM sys.Tables";
				cmd = new SqlCommand(SQL, conn);
				rdr = cmd.ExecuteReader();
				while (rdr.Read()) {
					string	name = (string)rdr[0];
					TableNames.Add(name);
				}
				rdr.Close();

				foreach (string name in TableNames) {
					SQL = "sp_spaceused";
					cmd = new SqlCommand(SQL, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("objname", name);
					rdr = cmd.ExecuteReader();
					while (rdr.Read()) {
						DBTableSize	tbl = new DBTableSize(
							(string)rdr["name"], (string)rdr["rows"], (string)rdr["reserved"],
							(string)rdr["data"], (string)rdr["index_size"], (string)rdr["unused"]);
						Sizes.Add(tbl);
					}
					rdr.Close();
				}
			}
			return Sizes;		
		}
	}
}
