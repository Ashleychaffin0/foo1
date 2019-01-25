using System;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using LRS;
using Bartizan.Utils.CRC;
using Bartizan.Input.Utils;

// TODO:
//	*	Implement Import Data
//			*	Log to table
//			*	Bypass records with matching CRC
//	*	Then the rest
//	*	Save Parms class, especially if changed
//			*	Implement a dirty bit via properties?
//	*	Put all Forecast classes into their own (separate?) file(s)

// Plotting -- http://weblogs.asp.net/scottgu/archive/2010/02/07/built-in-charting-controls-vs-2010-and-net-4-series.aspx

namespace Lew2 {
	public partial class Lew2 : Form {

		enum FieldNameEnum {
			SalesRep,
			EventName,
			EventStartDate,
			EventEndDate,
			RegContractor,
			UnitName,
			UnitCount,
			UnitCost
		}

		internal class RowData {
			public string		SalesRep;
			public string		EventName;
			public DateTime		EventStartDate;
			public DateTime		EventEndDate;
			public string		RegContractor;
			public string		UnitName;
			public int			UnitCount;
			public double		UnitCost;
		}

		Lew2Parms		l2p;
		List<string>	HeaderLine_Data;
		RowData			CurInputRecord;

//---------------------------------------------------------------------------------------

		public string DataSource {
			get { return "Data Source=" + l2p.DbPath; }
			private set { }
		}

//---------------------------------------------------------------------------------------

		public Lew2() {
			InitializeComponent();
			var foonames = Enum.GetNames(typeof(FieldNameEnum));
			CurInputRecord = new RowData();
		}

//---------------------------------------------------------------------------------------

		private void Lew2_Load(object sender, EventArgs e) {
#if false
			// OK, I tried. I really tried. To figure out how to use the framework's
			// ability to process a config file. Phooey! I'll just serialize my own
			// class and be done with it!
			// string dbPath = ConfigurationSettings.AppSettings["DbPath"];
			string path = Application.ExecutablePath;
			var w = ConfigurationManager.OpenExeConfiguration(path);
			var x = ConfigurationManager.GetSection("appSettings");
			var y = ConfigurationManager.AppSettings;
			var z = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			string dbPath = ConfigurationManager.AppSettings["DbPath"];
#endif
			l2p = GenericSerialization<Lew2Parms>.Load("Lew2.config");
		}

//---------------------------------------------------------------------------------------

		private void createDatabaseToolStripMenuItem_Click(object sender, EventArgs e) {
			SqlCeEngine engine = new SqlCeEngine(@"Data Source=" + l2p.DbPath);
			engine.CreateDatabase();

			using (SqlCeConnection conn = new SqlCeConnection(DataSource)) {
				conn.Open();
				CreateAllDatabaseTables(conn);
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateAllDatabaseTables(SqlCeConnection conn) {
			CreateTableForecastEvent(conn);
			CreateTableForecastSalesReps(conn);
			CreateTableForecastDevices(conn);
			CreateTableForecastEstimates(conn);
			CreateTableForecastRegContractors(conn);
		}

//---------------------------------------------------------------------------------------

		private void CreateTableForecastEvent(SqlCeConnection conn) {
			string SQL = @"CREATE TABLE tblForecast_Event"
				+ "(EventID int, EventName nvarchar(100),"
				+ " EventStartDate DateTime, EventEndDate DateTime)";
			var cmd = new SqlCeCommand(SQL, conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		private void CreateTableForecastSalesReps(SqlCeConnection conn) {
			string SQL = @"CREATE TABLE tblForecast_SalesReps"
				+ "(SalesRepID int, Name nvarchar(30))";
			var cmd = new SqlCeCommand(SQL, conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		private void CreateTableForecastDevices(SqlCeConnection conn) {
			string SQL = @"CREATE TABLE tblForecast_Devices"
				+ "(DeviceID int, DeviceName nvarchar(30))";
			var cmd = new SqlCeCommand(SQL, conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		private void CreateTableForecastEstimates(SqlCeConnection conn) {
			string SQL = @"CREATE TABLE tblForecast_Estimates"
				+ "(SalesRepID int, EventID int, DeviceID int, DeviceCount int, DeviceCost float)";
			var cmd = new SqlCeCommand(SQL, conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		private void CreateTableForecastRegContractors(SqlCeConnection conn) {
			string SQL = @"CREATE TABLE tblForecast_RegContractors"
				+ "(RCID int, RegContractorName nvarchar(100))";
			var cmd = new SqlCeCommand(SQL, conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		private void importToolStripMenuItem_Click(object sender, EventArgs e) {
			var ofd             = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.DefaultExt      = "csv";
			ofd.Multiselect     = false;
			ofd.Filter          = "CSV files|*.csv";
			DialogResult res    = ofd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			string filename = ofd.FileName;
			ImportData(filename);
		}

//---------------------------------------------------------------------------------------

		private void ImportData(string filename) {
			using (SqlCeConnection conn = new SqlCeConnection(DataSource)) {
				conn.Open();
				int LineNo = 0;				// For error reporting
				foreach (string line in File.ReadLines(filename)) {
					++LineNo;
					List<string> Fields;
					BartCSV.Parse(line, out Fields);
					var crc = new BartCRC();
					Canonicalize(Fields, crc);
					uint LineCrc = crc.GetCRC();
					if (LineNo == 1) {
						ProcessHeaderLine(conn, Fields);
					} else {
						ImportSingleRow(conn, LineNo, Fields, LineCrc);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessHeaderLine(SqlCeConnection conn, List<string> Fields) {
			HeaderLine_Data = Fields;
			// TODO: Do some kind of validation, such as, are these the fields we
			//		 expect and in the order we expect?
		}

//---------------------------------------------------------------------------------------

		private void ImportSingleRow(SqlCeConnection conn, int LineNo, List<string> Fields, uint LineCrc) {
			CurInputRecord.SalesRep       = Fields[(int)FieldNameEnum.SalesRep];
			CurInputRecord.EventName      = Fields[(int)FieldNameEnum.EventName];
			CurInputRecord.EventStartDate = Convert.ToDateTime(Fields[(int)FieldNameEnum.EventStartDate]);
			CurInputRecord.EventEndDate   = Convert.ToDateTime(Fields[(int)FieldNameEnum.EventEndDate]);
			CurInputRecord.RegContractor  = Fields[(int)FieldNameEnum.RegContractor];
			CurInputRecord.UnitName       = Fields[(int)FieldNameEnum.UnitName];
			CurInputRecord.UnitCount      = Convert.ToInt32(Fields[(int)FieldNameEnum.UnitCount]);
			CurInputRecord.UnitCost       = Convert.ToDouble(Fields[(int)FieldNameEnum.UnitCost]);
		}

//---------------------------------------------------------------------------------------

		// Get rid of leading, trailing and internal superfluous blanks. While we're at
		// it, calculate the CRC for the canonicalized text.
		private void Canonicalize(List<string> Fields, BartCRC crc) {
			for (int i = 0; i < Fields.Count; i++) {
				string s = Fields[i];
				s = s.Trim();			// Leading and trailing blanks
				// Now loop to squeeze out all internal multiple blanks
				int len = s.Length;
				while (true) {
					s = s.Replace("  ", " ");	// Two blanks into one
					if (s.Length == len) {
						break;					// Length didn't change. Done
					}
					len = s.Length;
				}	
				Fields[i] = s;
				crc.AddData(s);			// We don't include the commas in the CRC. Big deal.
			}
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public /* inner */ class Lew2Parms {
			public string DbPath;

//---------------------------------------------------------------------------------------

			public Lew2Parms() {
				// Serialization/Deserialization requires a parameter-less ctor
			}
		}
	}

		// Sample (first-cut) classes

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Forecast {
		public string	SalesRepName;
		public string	EventName;
		public DateTime EventStartDate;
		public DateTime EventEndDate;
		public string	DeviceName;
		public float	PricePerUnit;
	}

		// Database schemae
#if false
	tblForecastEvent
		AutoNum		EventID				// Not the same as in tblEvents
		string		EventName
		DateTime	EventStartDate
		DateTime	EventEndDate
		
	tblForecastSalesRep
		AutoNum		SalesRepID
		string		Name				// e.g. Chris
		string		NickName			// e.g. CE

	tblForecastDevices
		AutoNum		DeviceID
		string		DeviceName

	tblForecaseEstimates
		int			SalesRepID
		int			EventID
		int			DeviceID
		int			DeviceCount
		float		DeviceCost

	tblForecastRegContractors
		Autonum		RCID
		string		RegContractorName

#endif
}


#if false
SqlCeConnection conn = new SqlCeConnection(connString);
conn.Open();

SqlCeCommand cmd = conn.CreateCommand();
cmd.CommandText = "CREATE TABLE BlobTable(name nvarchar(128), blob ntext);";
cmd.ExecuteNonQuery();

cmd.CommandText = "INSERT INTO BlobTable(name, blob) VALUES (@name, @blob);";
SqlCeParameter paramName = cmd.Parameters.Add("name", SqlDbType.NVarChar, 128);
SqlCeParameter paramBlob = cmd.Parameters.Add("blob", SqlDbType.NText);
paramName.Value = "Name1";
paramBlob.Value = "Name1".PadLeft(4001);

cmd.ExecuteNonQuery();
#endif

#if false
// Notes:

Input CSV looks like
Sales Rep Name,Event Name, EventStartDate, Event End Date, Device Name (e.g. "Ultra"),
	Count (number ordered for this event), Price Per Device

Note: Check to see if we may have a different Price Per Device based on customer volume.
//	  May need to add a Customer Name.

Note: Do we need/want the Reg Contractor name in there somewhere?


#endif