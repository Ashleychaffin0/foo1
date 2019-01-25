using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bartizan.Utils.Compression;

namespace ShowDecompressImportData {
	public partial class ShowDecompressImportData : Form {

		SqlConnection	conn = null;
		int				CurImportID = 0;

//---------------------------------------------------------------------------------------

		public ShowDecompressImportData() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			bool bOK = int.TryParse(txtSavedImportID.Text, out CurImportID);
			if (! bOK) {
				MessageBox.Show("SavedImportID not valid. Try again");
				return;
			}
			ShowImportData();
		}

//---------------------------------------------------------------------------------------

		private void ShowImportData() {
			string data = DecompressData(conn, CurImportID);
			rtbVisitor.Text = data;
		}

//---------------------------------------------------------------------------------------

		private void btnPrevious_Click(object sender, EventArgs e) {
			Show(--CurImportID);
		}

//---------------------------------------------------------------------------------------

		private void btnNext_Click(object sender, EventArgs e) {
			Show(++CurImportID);
		}

//---------------------------------------------------------------------------------------

		private void Show(int p) {
			txtSavedImportID.Text = p.ToString();
			ShowImportData();
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
#if true
			string connString = LLConnection.GetSqlStringBuilder("PROD").ToString();
#else
			string connString = LLConnection.GetSqlStringBuilder("DEVEL").ToString();
#endif
			// string connString = LLConnection.GetSqlStringBuilder("PROD").ToString();
			conn = new SqlConnection(connString);
			conn.Open();
			int EventID = 160;
			string EventName = "Electronic House Expo Spring 2007";
			CopyRawLeadsToFile(conn, EventID, EventName, @"C:\LRS");

#if false
			DataClasses1DataContext dc = new DataClasses1DataContext(conn);
			var qry = from swipe in dc.tblSwipes
					  join term in dc.tblTerminals
						  on swipe.TerminalID equals term.ID
					  where swipe.SwipeID == 195929
					  select new { swipe, term };

#if false
			var q2 = from y in dc.tblSavedImports
					 where y.SavedImportID >= 23 && y.SavedImportID <= 45
					 select y;

			var q3 = from z in dc.tblImportTrackings
					 where z.SwipeID ==  
#endif
			var q4 = from map in dc.tblMapCfgs
					 where map.MapCfgID == 221
					 select map.MapCfgContents;
			List<string> s = q4.ToList<string>();
			Clipboard.SetText(s[0]);
#endif
		}

//---------------------------------------------------------------------------------------

		private void CopyRawLeadsToFile(SqlConnection conn, int EventID, string EventName, string DirName) {
			string SQL = "SELECT * FROM tblSavedImports WHERE EventID = " + EventID
				+ " ORDER BY WhenImported";
			SqlCommand		cmd = new SqlCommand(SQL, conn);
			SqlDataReader	rdr = cmd.ExecuteReader();
			StringBuilder	sb = new StringBuilder();
			int				MapCfgID = 0;
			while (rdr.Read()) {
				MapCfgID = (int)rdr["MapCfgID"];
				string	VisitorData = (string)rdr["VisitorData"];
				if ((bool)rdr["IsVisitorDataCompressed"]) {
					sb.Append(BartCompress.Decompress(VisitorData));
				} else {
					sb.Append(VisitorData);
				}
			}
			rdr.Close();
			string	path = Path.Combine(DirName, EventName + ".Visitor.txt");
			StreamWriter	wtr = new StreamWriter(path);
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
			path = Path.Combine(DirName, EventName + ".Map.cfg");
			wtr = new StreamWriter(path);
			wtr.Write(MapCfg);
			wtr.Close();
			// Clipboard.SetText(MapCfg);
		}

//---------------------------------------------------------------------------------------

		private string DecompressData(SqlConnection conn, int StartID) {
			return DecompressData(conn, StartID, StartID);
		}

//---------------------------------------------------------------------------------------

		private string DecompressData(SqlConnection conn, int StartID, int EndID) {
			// string SQL = "SELECT * FROM tblSavedImports WHERE SavedImportID = 29454";
			string SQL = "SELECT * FROM tblSavedImports WHERE SavedImportID between {0} and {1}";
			SQL = string.Format(SQL, StartID, EndID);
			SqlCommand cmd = new SqlCommand(SQL, conn);
			SqlDataReader rdr = cmd.ExecuteReader();
			string data = "";
			while (rdr.Read()) {
				string CompData = (string)rdr["VisitorData"];
				data += "\n\n\n" + (int)rdr["SavedImportID"] + ": " 
					 + (DateTime)rdr["WhenImported"] 
					 + " " + (string)rdr["TerminalSerial"]
					 + "\n";
				if ((bool)rdr["IsVisitorDataCompressed"]) {
					data += BartCompress.Decompress(CompData);
				} else {
					data += CompData;
				}
			}
			rdr.Close();
			// Clipboard.SetText(data);
			return data;
		}

//---------------------------------------------------------------------------------------

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if ((conn != null) && (conn.State == ConnectionState.Open)) {
				conn.Close();
			}
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
