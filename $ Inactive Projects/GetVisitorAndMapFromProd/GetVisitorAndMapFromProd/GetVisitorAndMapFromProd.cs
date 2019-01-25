// #define KLUDGE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Text;

using Bartizan.Utils.Compression;

namespace GetVisitorAndMapFromProd {
	class GetVisitorAndMapFromProd {
		static void Main(string[] args) {
			if (args.Length != 1) {
				Console.WriteLine("Usage: GetVisitorAndMapFromProd EventName");
			} else {
				ProcessEventName(args);
			}
		}

//---------------------------------------------------------------------------------------

		private static void ProcessEventName(string[] args) {
			string connString = GetSqlStringBuilder("PROD").ToString();
			using (var conn = new SqlConnection(connString)) {
				conn.Open();
#if KLUDGE
				string	EventName = "HPTechForum08";
#else
				string	EventName = args[0];
#endif
				int? EventID = GetEventIDFromName(conn, EventName);
				if (EventID != null) {
					CopyRawLeadsToFile(conn, EventID.Value, EventName);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void CopyRawLeadsToFile(SqlConnection conn, int EventID, string EventName) {
			int		nRecs = 0;
			int		SwipeCount = 0;
			string SQL = "SELECT * FROM tblSavedImports WHERE EventID = " + EventID
				+ " ORDER BY SavedImportID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			SqlDataReader rdr = cmd.ExecuteReader();
			StringBuilder sb = new StringBuilder();
			int MapCfgID = 0;
			while (rdr.Read()) {
				MapCfgID = (int)rdr["MapCfgID"];
				string VisitorData = (string)rdr["VisitorData"];
				if (VisitorData.Length > 0) {
					if ((bool)rdr["IsVisitorDataCompressed"]) {
						sb.Append(BartCompress.Decompress(VisitorData));
					} else {
						sb.Append(VisitorData);
					}
					SwipeCount += (int)rdr["RecordCount"];
#if KLUDGE
					if (SwipeCount > 10000000) {
						break;
					}
#endif
				}
				++nRecs;
				//dbg_DumpReader(rdr, nRecs, SwipeCount);
			}
			Console.WriteLine("Debug -- About to close rdr");
			rdr.Close();
			Console.WriteLine("Debug -- Back from close rdr");
			string path = EventName + ".Visitor.txt";
			StreamWriter wtr = new StreamWriter(path);
			wtr.Write(sb.ToString());
			wtr.Close();
			// Clipboard.SetText(sb.ToString());
			// System.Diagnostics.Debugger.Break();	// Process data on clipboard, then continue

			SQL = "select mapcfgcontents from tblmapcfg where mapcfgid=" + MapCfgID;
			cmd = new SqlCommand(SQL, conn);
			rdr = cmd.ExecuteReader();
			rdr.Read();
			string MapCfg = (string)rdr["mapcfgcontents"];
			rdr.Close();

			MapCfg = MapCfg.Replace("\n", "\r\n");
			path = EventName + ".Map.cfg";
			wtr = new StreamWriter(path);
			wtr.Write(MapCfg);
			wtr.Close();
			// Clipboard.SetText(MapCfg);
		}

//---------------------------------------------------------------------------------------

		private static void dbg_DumpReader(SqlDataReader rdr, int nRecs, int SwipeCount) {
			Console.WriteLine("\n\nRecord [{0}] - SwipeCount = {1}", nRecs, SwipeCount);
			for (int i = 0; i < rdr.FieldCount; i++) {
				Console.WriteLine("rdr[{0}/{1} - {2}",
					i, rdr.GetName(i), rdr[i]);
			}
		}

//---------------------------------------------------------------------------------------

		private static void GetData(SqlConnection conn, int? EventID) {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private static int? GetEventIDFromName(SqlConnection conn, string EventName) {
			string SQL = "SELECT EventID FROM tblEvents WHERE EventName ='" + EventName + "'"
				+ " ORDER BY EventName";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			int? EventID = (int?)cmd.ExecuteScalar();
			if (EventID == null) {
				ShowSimilarEventNames(conn, EventName);
			}
			return EventID;
		}

//---------------------------------------------------------------------------------------

		private static void ShowSimilarEventNames(SqlConnection conn, string EventName) {
			string SQL = "SELECT EventName FROM tblEvents WHERE EventName Like '%" + EventName + "%'"
				+ " ORDER BY EventName";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			SqlDataReader rdr = cmd.ExecuteReader();
			try {
				if (!rdr.HasRows) {
					Console.WriteLine("No Events with name like %{0}%", EventName);
					return;
				}

				Console.WriteLine("Event name not found. Event names like %{0}% ...", EventName);
				while (rdr.Read()) {
					Console.WriteLine("\tEventName: {0}", rdr["EventName"]);
				}
			} finally {
				rdr.Close();
			}
		}


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
				Console.WriteLine("GetSqlStringBuilder - Unknown database - {0}", Name);
				break;
			}
			return builder;
		}

	}
}
