// Copyright (c) 2006 Bartizan Connects LLC

// #define		PROD			// Production, not debug, version
#define		DEVEL			// Debug, but on the remote Server
// #define		LOCAL			// Here, locally, but still on LLDevel
// #define			NEWSERVER

// #define BIRTHDAY

// #define SQL_LOGGING

// Uncomment the next line when doing local debugging of the web service
#define	NO_TIMEOUT		

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;

using Microsoft.SqlServer.Management.Common;
// using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
// using Microsoft.SqlServer.Management.Smo.Agent;

using Bartizan.Utils.Database;
using Bartizan.Input.Utils;		// For BartCSV testing

#if false		// Just a place to put WCT struct, for use in another program
typedef struct _WAITCHAIN_NODE_INFO {  
	WCT_OBJECT_TYPE ObjectType;  
	WCT_OBJECT_STATUS ObjectStatus;  
	union {    
		struct {      
			WCHAR ObjectName[WCT_OBJNAME_LENGTH];      
			LARGE_INTEGER Timeout;      
			BOOL Alertable;    
		} LockObject;    
		struct {      
			DWORD ProcessId;      
			DWORD ThreadId;      
			DWORD WaitTime;      
			DWORD ContextSwitches;    
		} ThreadObject;  
	};
} WAITCHAIN_NODE_INFO,  *PWAITCHAIN_NODE_INFO;
#endif

namespace TestLLImporter {

//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------


	public partial class TestLLImporter : Form {
		
		enum MyEnumInt {			// TODO: For Test()
			a=4,
			b=6
		}

		enum MyEnumByte : byte {	// TODO: For Test()
			x=6,
			y=9
		}

		// string UserID = "LRS", password = "bartset";
		// string UserID = "sampleEx", password = "bartset";
		string UserID  = "f4HyZd5d";
		string Password = "jBpQ2mbq";


		internal static string SYSTEM_DEVEL = "DEVEL";
		internal static string SYSTEM_PROD = "PROD";	// DB Mart - Production
		internal static string SYSTEM_DEMO = "DEMO";

		internal static string DBID {
			// DBID = SYSTEM_PROD;
			// DBID = SYSTEM_PROD;
			get {
				string ID = Environment.GetEnvironmentVariable("BARTSYSTEMID");
				if (ID == null) {
					ID = "DEVEL";
				}
				return ID;
			}
			private set { }
		}

//---------------------------------------------------------------------------------------

		public TestLLImporter() {
			InitializeComponent();

#if true
			txtMapCfg.Text = @"M:\Downloads 2007\Rent-A-PC\Parker Seminar\M01903.CFG";
			txtVisitorTxt.Text = @"M:\Downloads 2007\Rent-A-PC\Parker Seminar\V01903.TXT";
#endif

			// System.Globalization.CultureInfo o = System.Threading.Thread.CurrentThread.CurrentCulture;

			// foreach (string fmt in Clipboard.GetDataObject().GetFormats())
			// 	Console.WriteLine(fmt);

			// ShowSystemInfo();

			// TestSplit();

			// TestNewRegex();

			// TestNewCSV();

			// TestSqlLogging();

			// TestBulkInsert();

			// TestSOAPProblems();

			// TestSOAPScrub();

			// TestSortingWithAnonymousDelegates();

			// TestShowPlanAll();

			// TestSMO();

			// TestMultipleSqlStatementsInOneShot();

			// TestInsertIntotblImporterDebugMessages();

			// TestInsertingGUID();

			// ResetCcLeadsTypeInProgress();
		}

//---------------------------------------------------------------------------------------

		private void ResetCcLeadsTypeInProgress() {
			SqlConnectionStringBuilder builder = SelectDB("PROD");
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string SQL = @"UPDATE tblCcLeadsData
SET       Status = 0
WHERE (EventID = @EventID) AND (Status = 1)";
#if false
				SQL += " AND (Guid = 'EE71C595-F4DF-43F9-944B-F883C0AAD59F')";
#endif
				SqlCommand cmd = new SqlCommand(SQL, conn);
				Guid ID = Guid.NewGuid();
				cmd.Parameters.AddWithValue("@EventID", 526);
				// cmd.Parameters.AddWithValue("@guid", "");
				int n = cmd.ExecuteNonQuery();
			}
		}

//---------------------------------------------------------------------------------------

		private void TestInsertingGUID() {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string SQL = "INSERT INTO tblLRS(ID, Msg)"
					+ "\nVALUES(@ID, @Msg)";
				SqlCommand cmd = new SqlCommand(SQL, conn);
				Guid	ID = Guid.NewGuid();
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@msg", DateTime.Now.ToString());
				int n = cmd.ExecuteNonQuery();

				SQL = string.Format("SELECT * FROM tblLRS WHERE ID = '{0}'", ID);
				cmd = new SqlCommand(SQL, conn);
				SqlDataReader	rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
				rdr.Read();
				for (int i = 0; i < rdr.FieldCount; i++)  {
					Console.WriteLine("rdr[{0}:{1} = {2}", i, rdr.GetName(i), rdr[i]);
				}
				rdr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		private void TestInsertIntotblImporterDebugMessages() {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string SQL = "INSERT INTO tblImporterDebugMessages(Message, WhenInserted)"
					+ "\nVALUES(@msg, @wheninserted)";
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@msg", "Now is the time");
				cmd.Parameters.AddWithValue("@wheninserted", DateTime.Now);
				int n = cmd.ExecuteNonQuery();
			}
		}

//---------------------------------------------------------------------------------------

		private void TestSplit() {
			string[] Demogs = new string[] { "Boys", "Girls + Infants", "Girls" };
			string	test = "Boys + Girls + Infants";
			string [] Splits = new string [] {" + "};
			string [] data = test.Split(Splits, StringSplitOptions.RemoveEmptyEntries);
		}

//---------------------------------------------------------------------------------------

#if true
		private void TestNewCSV() {
			string	text;
			// text = "'285',' ',' ','O90696368','Nyg','Shigeyoshi','Onoda',''helios Holding Co., Ltd.'','14 1 Nishiura Ishimaki','Toyohashi','','','','','Japan','441 1112','(053) 288-6305','(   )    -    ','1623','1402','shige@helios.jp','RT0024','01-23-2008 14:00:02','','','','','','','','','','','','','','','',''";
			text = "''Field1','At Front',''Field3'";
			TestNewCSVString(text);
			text = "'Fie'ld1','In the middle','Fie'ld3'";
			TestNewCSVString(text);
			text = "'Field1'','At the end','Field3''";
			TestNewCSVString(text);
		}

//---------------------------------------------------------------------------------------

		private static void TestNewCSVString(string text) {
			List<string>	Values;
			string txt = text.Replace("'", "\"");
			BartCSV.Parse(txt, out Values, true);
			Console.Write("Input = {0}\n\tOutput = ", txt);
			for (int i = 0; i < Values.Count; i++) {
				if (i > 0) {
					Console.Write(", ");
				}
				Console.Write("[{0}]={1}", i, Values[i]);
			}
			Console.WriteLine();
		}
#endif

//---------------------------------------------------------------------------------------

		private void TestNewRegex() {
			string	text, txt, pat;
			text = "'1','Ed Morris','259 E Lancaster Road','Bala Consulting','Eng','Lancaster','PA','19785','(641) 258-9654','(478) 587-4859','24','A','B','C','E','CH','','08-16-2006  15:06:03','Y','Y','N','N','Y','Y','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','Last',";
			// text = /* TODO: */ @"'abc','def','ghi";
			text = "'285',' ',' ','O90696368','Nyg','Shigeyoshi','Onoda',''helios Holding Co., Ltd.'','14 1 Nishiura Ishimaki','Toyohashi','','','','','Japan','441 1112','(053) 288-6305','(   )    -    ','1623','1402','shige@helios.jp','RT0024','01-23-2008 14:00:02','','','','','','','','','','','','','','','',''";
			txt = text.Replace("'", "\"");
			pat = @"^""";			// Beginning (double) quote
			pat += @"((?<Fields>[^""]*?)"","")*(?<LastField>[^""]*)";
			Regex	re;
			re = new Regex(pat, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);			
			Match	m = re.Match(txt);
			if (m.Success) {
				foreach (Group grp in m.Groups) {
					Console.WriteLine("Index={0}, Length={1}, Value={2}, ToString()={3}", grp.Index, grp.Length, grp.Value, grp.ToString());
				}
			}
			CaptureCollection Caps = m.Groups["Fields"].Captures;
			foreach (Capture cap in Caps) {
				Console.WriteLine("Field - '{0}'", cap.Value);
			}

			Console.WriteLine("LastField - '{0}'", m.Groups["LastField"]);

#if false
			string pattern = "^[^<>]*" +
					 "(" +
					   "((?'Open'<)[^<>]*)+" +
					   "((?'Close-Open'>)[^<>]*)+" +
					 ")*" +
					 "(?(Open)(?!))$";
			string pat2 = @"^[^<>]*(((?'Open'<)[^<>]*)+((?'Close-Open'>)[^<>]*)+)*(?(Open)(?!))$";
			Debug.Assert(pattern == pat2);
			string input = "<abc><mno<xyz>>";
			re = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);			
			m = re.Match(input);
#endif
		}

//---------------------------------------------------------------------------------------

		private void TestSMO() {
			ServerConnection srvc = new ServerConnection();
			SqlConnectionStringBuilder bld = new SqlConnectionStringBuilder();
			// bld.DataSource = "75.126.77.59,1092";
			bld.DataSource = "198.64.249.6,1092";
			bld.UserID = "sa";
			bld.Password = "$yclahtw2007bycnmhd!";
			srvc.ConnectionString = bld.ConnectionString;
			// srvc.Connect();
			Server	srv = new Server(srvc);
			DatabaseCollection	dbs = srv.Databases; 
			for (int i = 0; i < dbs.Count; i++) {
				Console.WriteLine("{0} - {1}", i, dbs[i].Name);
			}

			Database mast = dbs["master"];
			ViewCollection	views =  mast.Views;
			foreach (Microsoft.SqlServer.Management.Smo.View view in views) {
				Console.WriteLine("{0}", view.Name);
			}

			Information info = srv.Information;
			srv.ConnectionContext.Disconnect();
		}

//---------------------------------------------------------------------------------------

		private void TestShowPlanAll() {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string	SQL = "SET SHOWPLAN_ALL ON";
				SqlCommand	cmd = new SqlCommand(SQL, conn);
				int n = cmd.ExecuteNonQuery();

				SQL = "SELECT * from tblSwipes WHERE SwipeID=7489848";
				cmd = new SqlCommand(SQL, conn);
				SqlDataReader rdr = cmd.ExecuteReader();
				object [] vals = new object[rdr.VisibleFieldCount];
				while (rdr.Read()) {
					for (int i = 0; i < rdr.VisibleFieldCount; i++) {
						Console.WriteLine("{0}: {1} - {2}", i, rdr.GetName(i), rdr[i]);
					}
					Console.WriteLine("\n");		// Double-space
					// n = rdr.GetValues(vals);
				}
				rdr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		class foo {
			public string	name;
			public int		age;
			
			public foo(string name, int age) {
				this.name = name;
				this.age  = age;
			}
		}

//---------------------------------------------------------------------------------------

		private void TestSortingWithAnonymousDelegates() {
			foo []	foos = new foo[3];
			foos[0] = new foo("John", 21);
			foos[1] = new foo("Fred", 31);
			foos[2] = new foo("Shirley", 41);
			Console.WriteLine("Original data");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}
			Array.Sort(foos, delegate(foo x, foo y) {return x.name.CompareTo(y.name);});
			Console.WriteLine("Sorted by name");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}
			Array.Sort(foos, delegate(foo x, foo y) {return x.age.CompareTo(y.age);});
			Console.WriteLine("Sorted by age");
			for (int i = 0; i < foos.Length; i++) {
				Console.WriteLine("{0}: {1}, age {2}", i, foos[i].name, foos[i].age);
			}
		}

//---------------------------------------------------------------------------------------

		private void TestSOAPScrub() {
			string txt = "123\r\n456\r\n\r\n\r\na\bc\nde\f\nghi";
			List<int>	LineNums;
			txt = SOAPScrub(txt, out LineNums);
		}

//---------------------------------------------------------------------------------------

		private void TestSOAPProblems() {
			LLWS1	imp = new LLWS1();
			imp.RequestEncoding = System.Text.Encoding.UTF32;
			string	map = " ";
			string	data = " ";
			ImportStatus	impStatus = null;
			ImportStatus	[] RecStats = null;
			Dictionary<int, string>		Successes, Failures;
			Successes = new Dictionary<int, string>();
			Failures = new Dictionary<int, string>();
			for (int i = 0; i <= 255; i++) {
			// for (int i = 256; i <= 259; i++) {		// FYI, this works
				char c = Convert.ToChar(i);
				data = new string(c, 1);
				try {
					Console.WriteLine("Import data {0:X2}", i);
					impStatus = imp.Import("", "", "", "", 0, data, map, MapType.MapTypeText,
						"", false, false, DataSource.LeadsLightning, false, 0, "", out RecStats);
					Successes[i] = "OK";
				} catch (Exception ex) {
					Failures[i] = ex.Message;
				}
			}
			ShowSOAPProblems(Failures);
			Console.WriteLine("Done TestSOAPProblems");
		}

//---------------------------------------------------------------------------------------

		private void ShowSOAPProblems(Dictionary<int, string> Failures) {
			int []	Keys = new int[Failures.Count];
			Failures.Keys.CopyTo(Keys, 0);
			Array.Sort(Keys);
			foreach (int i in Keys) {
				Console.WriteLine("Import failed for data {0:X2} - {1}", i, Failures[i]);
			}
		}

//---------------------------------------------------------------------------------------

		private void TestBulkInsert() {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
#if SQL_LOGGING
				conn.StatisticsEnabled = true;	
#endif

				string	MyTab = "LRS_tblSwipesText";
#if false
				DataTable st /*SchemaTable*/ = conn.GetSchema("Tables");
				// DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
				foreach (DataRow srow in st.Rows) {
					Console.WriteLine("Table name {0}", srow["TABLE_NAME"]);
					if ((string)srow["TABLE_NAME"] == MyTab) {
						Console.WriteLine("Table name {0}", srow["TABLE_NAME"]);
					}
				}
#endif
				WriteInBulk(conn, MyTab);

				// OK, now do it again
				WriteInBulk(conn, MyTab);
			}
		}

//---------------------------------------------------------------------------------------

		private static void WriteInBulk(SqlConnection conn, string MyTab) {
			conn.StatisticsEnabled = true;
			SqlBulkCopy bc = new SqlBulkCopy(conn);
			bc.DestinationTableName = MyTab;
			DataTable table = new DataTable();
			table.Columns.Add("PersonEventID", typeof(int));
			table.Columns.Add("FieldName", typeof(string));
			table.Columns.Add("FieldText", typeof(string));
			table.Columns.Add("SeqNo", typeof(int));
			DataRow row = table.NewRow();
			row["PersonEventID"] = 5;
			row["FieldName"] = "First Name";
			row["FieldText"] = "Larry";
			row["SeqNo"] = 1;
			table.Rows.Add(row);
			row = table.NewRow();
			row["PersonEventID"] = 27;
			row["FieldName"] = "Last Name-2";
			row["FieldText"] = "Smith";
			row["SeqNo"] = 3;
			table.Rows.Add(row);
			row = table.NewRow();
			row["PersonEventID"] = -16;
			row["FieldName"] = "Address 2";
			row["FieldText"] = "217 Rivendell";
			row["SeqNo"] = 2;
			table.Rows.Add(row);
			try {
				DumpConnStats("Before bc.WriteToServer", conn);
				bc.WriteToServer(table);
				DumpConnStats("After bc.WriteToServer", conn);
			} catch (SqlException ex) {
				Console.WriteLine("BCP exception - {0}", ex);
				MessageBox.Show(ex.ToString(), "BCP exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				string SQL = "SELECT PersonEventID, FieldName, FieldText, SeqNo FROM " + MyTab;
				SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
				SqlCommandBuilder	cb = new SqlCommandBuilder(adapt);
				adapt.UpdateBatchSize = 0;
				Console.WriteLine("UpdateBatchSize={0}", adapt.UpdateBatchSize); // TODO:
				adapt.ContinueUpdateOnError = true;
				DumpConnStats("Before adapt.Update", conn);
				adapt.Update(table);
				DumpConnStats("After adapt.Update", conn);
				foreach (DataRow row2 in table.Rows) {
					if (row.HasErrors) {
						if (!row2.RowError.StartsWith("Violation of PRIMARY KEY constraint")) {
							throw new Exception("Internal error - Unexpected error in ImportResponses-2");
							// TODO: Better would be to define a StringBuilder, and collect
							//		 all the rows in error, and their messages, then do a 
							//		 final throw.
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine("BCP exception - {0}", ex);
				MessageBox.Show(ex.ToString(), "BCP exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			bc.Close();
		}

//---------------------------------------------------------------------------------------

		private static void DumpConnStats(string msg, SqlConnection conn) {
			Console.WriteLine("\n\n{0}", msg);
			Hashtable	ht = (Hashtable)conn.RetrieveStatistics();
			foreach (string key in ht.Keys) {
				Console.WriteLine("Stats['{0}'] = {1}", key, ht[key]);
			}
		}

//---------------------------------------------------------------------------------------

		private void TestSqlLogging() {
			SqlConnectionStringBuilder builder = SelectDB();

			System.Diagnostics.StackTrace st = new StackTrace(1);
			StackFrame sf = st.GetFrame(0);
			string s = sf.GetMethod().Name;
			
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();

				string SQL = "SELECT AcctID, Password, Creator,"
			+ " Activated, AcctType, AcctGenerator, PasswordInPlainText"
			+ "\nFROM tblAccounts"
			+ "\nWHERE UserID = @UserID";
#if SQL_LOGGING
				BartSqlCommandWithLogging cmd = new BartSqlCommandWithLogging(SQL, conn);
#else
				SqlCommand	cmd = new SqlCommand(SQL, conn);
#endif
				string UserID  = "uz65344t";
				cmd.Parameters.AddWithValue("@UserID", UserID);
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
				if (!rdr.Read()) {
					rdr.Close();
				}
				rdr.Close();


				string	SQL1 = "SELECT Count(*) FROM tblSwipes WHERE AcctID=@AcctID AND EventID=@EventID";
				BartSqlCommandWithLogging	MyCmd = new BartSqlCommandWithLogging(SQL1, conn);
				MyCmd.Parameters.AddWithValue("@AcctID", 1257);
				MyCmd.Parameters.AddWithValue("@EventID", 58);
				MyCmd.Parameters.AddWithValue("@AcctID_EnumInt", MyEnumInt.a);
				MyCmd.Parameters.AddWithValue("@EventID_EnumByte", MyEnumByte.y);
				int nRecs = (int)MyCmd.ExecuteScalar();
				string sproc = BartSqlCommandWithLogging.GetSProc();

				BartSqlCommandWithLogging.NewComment("***** Start of\n******* Second query\n");
				SQL = "SELECT COUNT(*) FROM tblAccounts\nWHERE @UserID=@UserID \n\tAND Creator=@Creator AND AcctID=@AcctID";
				BartSqlCommandWithLogging	Cmd2 = new BartSqlCommandWithLogging(SQL, conn, "Test - 2");
				Cmd2.Parameters.AddWithValue("@AcctID", 23);
				Cmd2.Parameters.AddWithValue("@UserID", "LRSRC");
				Cmd2.Parameters.AddWithValue("@Creator", 1);
				nRecs = (int)Cmd2.ExecuteScalar();
				sproc = BartSqlCommandWithLogging.GetSProc();
			}
		}

//---------------------------------------------------------------------------------------

		private void TestMultipleSqlStatementsInOneShot() {
			/*	
			    A scope is a module -- a stored procedure, trigger,
				function, or batch. Thus, two statements are in the same scope if they
				are in the same stored procedure, function, or batch.
			*/

			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string SQL = "INSERT INTO tblImporterDebugMessages(WhenInserted, Message)"
					+ "VALUES(@WhenInserted, @msg)";
				// SQL += ";\nSELECT COUNT(*) as cnt FROM tblImporterDebugMessages";
				SQL += ";\nSELECT SCOPE_IDENTITY() as ident";
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@WhenInserted", DateTime.Now);
				cmd.Parameters.AddWithValue("@msg", "Hello SQL 12");

				conn.InfoMessage += new SqlInfoMessageEventHandler(connInfoMessage2);
				conn.StateChange += new StateChangeEventHandler(conn_StateChange2);
				try {
#if false
					int n = cmd.ExecuteNonQuery();
#else
#if true
					// An Autonum (aka Identity) comes back as decimal
					int ident = Convert.ToInt32(cmd.ExecuteScalar());
#else
					SqlDataReader	rdr = null;
					int		RowNum = 0;
					try {
						rdr = cmd.ExecuteReader();
						object [] vals = new object[rdr.VisibleFieldCount];
						while (rdr.Read()) {
							++RowNum;
							for (int i = 0; i < rdr.VisibleFieldCount; i++) {
								Console.WriteLine("Row {0}, Field {1}: {2} - {3}", RowNum, i, rdr.GetName(i), rdr[i]);
							}
							// Console.WriteLine("\n");		// Double-space
							// n = rdr.GetValues(vals);
						}
					} finally {
						if (rdr != null && !rdr.IsClosed) {
							rdr.Close();
						}
					}
#endif
#endif
				} catch (Exception ex) {
					Console.WriteLine("Exception in TestMultipleSqlStatementsInOneShot - {0}", ex.ToString());
				}
			}
		}

//---------------------------------------------------------------------------------------

		void conn_StateChange2(object sender, StateChangeEventArgs e) {
			Console.WriteLine("conn_StateChange2 - OriginalState={0}, CurrentState={1}, ToString={2}", e.OriginalState, e.CurrentState, e.ToString());
		}

//---------------------------------------------------------------------------------------

		void connInfoMessage2(object sender, SqlInfoMessageEventArgs e) {
			Console.WriteLine("connInfoMessage2 - Errors={0}, Msg={1}, Source={2}, ToString={3}", e.Errors, e.Message, e.Source, e.ToString());
		}

//---------------------------------------------------------------------------------------

		private void ShowSystemInfo() {
			LLWS1			import = new LLWS1();

			string			MachineName;
			int				ProcessorCount;		// May be -1
			string			os;
			int				TickCount;
			string			UserName;
			string			UserDomainName;		// May be "?"
			DateTime		CurrentTime;
			string			CLRVersion;
			string []		EnvironmentVars;	// May be null
			try {
				import.GetSystemInfo(out MachineName, out ProcessorCount, out os, out TickCount,
					out UserName, out UserDomainName, out CurrentTime, out CLRVersion,
					out EnvironmentVars); 
				lbMsgs.Items.Add(string.Format("MachineName={0}", MachineName));
				lbMsgs.Items.Add(string.Format("ProcessorCount={0}", ProcessorCount));
				lbMsgs.Items.Add(string.Format("OS={0}", os));
				lbMsgs.Items.Add(string.Format("System Uptime={0}", TimeSpan.FromMilliseconds(TickCount)));
				lbMsgs.Items.Add(string.Format("UserName={0}", UserName));
				lbMsgs.Items.Add(string.Format("UserDomainName={0}", UserDomainName));
				lbMsgs.Items.Add(string.Format("CurrentTime={0}", CurrentTime));
				lbMsgs.Items.Add(string.Format("CLRVersion={0}", CLRVersion));
				if (EnvironmentVars != null) {
					lbMsgs.Items.Add("Environment variables:");
					foreach (string var in EnvironmentVars) {
						lbMsgs.Items.Add(string.Format("    {0}", var));
					}
				}
				lbMsgs.Items.Add(string.Format("AppDomain Fullname={0}", System.Threading.Thread.GetDomain().FriendlyName));
				lbMsgs.Items.Add("");
			} catch (Exception ex) {
				Clipboard.SetData("Text", ex.ToString());
				Console.WriteLine("GetSystemInfo failed - {0}", ex);
				string errmsg = string.Format("GetSystemInfo failed at {0}", DateTime.Now);
				MessageBox.Show(ex.ToString(), errmsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseMapCfg_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.Filter = "Setup files (*.cfg)|*.cfg|All files|*.*";
			openFileDialog1.InitialDirectory = Path.GetDirectoryName(txtMapCfg.Text);
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtMapCfg.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseVisitorTxt_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.Filter = "Swipe data files (*.txt)|*.txt|All files|*.*";
			openFileDialog1.InitialDirectory = Path.GetDirectoryName(txtVisitorTxt.Text);
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtVisitorTxt.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			lblStatus.Text = "Working...";
			Application.DoEvents();
#if false
			int EventID = 11;
			DeleteDataFor(UserID, password, EventID);
#endif

			if (radImport.Checked) {
				RunImporter();
			} else if (radGetSetupInfo.Checked) {
				RunGetSetupInfo();
			} else if (radGetSetupFile.Checked) {
				RunGetSetupFile();
			} else if (radUploadMapCfg.Checked) {
				RunUploadMapCfg();
			} else if (radKeyedBasicDataImport.Checked) {
				Run1DImporter();
			} else if (radCcLeads.Checked) {
				RunCcLeads();
			} else {
				MessageBox.Show("Please choose an operation", "Test LLWS1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			lblStatus.Text = "Done";
		}

//---------------------------------------------------------------------------------------

		private void RunCcLeads() {
#if false
			WsCallPlugin cpi = new WsCallPlugin();
			// string s = ccl.ForwardLeads("Hiya world", 666);
			cpi.BeginHelloWorld("John", 10000, null, null);
			// string s = cpi.HelloWorld("George", 1000);
#endif
			WsCcLeads	wsLeads = new WsCcLeads();
			wsLeads.Timeout = 10 * 60 * 1000;		// Ten minute timeout
			wsLeads.CcLeads("PROD", 526);
			//wsLeads.CcLeads("PROD", 299);
			// wsLeads.BeginCcLeads(null, 299, null, null);
		}

//---------------------------------------------------------------------------------------

		private void RunUploadMapCfg() {
			LLWS1	ws = new LLWS1();
			string	MapCfgContents;
			int		MapCfgID, ErrorCode;
			string	msg;
			string	SetupID;

			// SetupID = "A000503VN";
			// SetupID = "A000501PG";
			// SetupID = "A000505WX";
			SetupID = "A000504DX";
			// SetupID = "A000B01YL";
			MapCfgContents = "Hello world 503VN";
			msg = ws.GetMapFileID(SetupID, MapCfgContents, out MapCfgID, out ErrorCode);
			msg = string.Format("MapCfgID={0}, ErrorCode={1}, msg={2}",
				MapCfgID, ErrorCode, msg);
			ShowStringsInListbox(msg, "WebResult - {0}");
		}

//---------------------------------------------------------------------------------------

		private void RunGetSetupFile() {
			LLWS1	ws1 = new LLWS1();

			string	UserID, Password;
			int		SetupFileLength;
			int		MapCfgID;
			int		EventID;
			int		ErrorCode;
			string	msg;
			string	SetupID;
			// SetupID = "A000503VN";
			// SetupID = "A000501PG";
            // SetupID = "A005M01YH";
			// SetupID = "A005M027U";
			SetupID = "A006501RX";
            UserID = "LRSRC";
            Password = "bartset";
			msg = ws1.GetSetupInfo(SetupID, out UserID, out Password,
				out SetupFileLength, out MapCfgID, out EventID, out ErrorCode);
			lbMsgs.Items.Add("Original GetSetupInfo: " + msg);
			DateTime SFTimestamp;
			msg = ws1.GetSetupInfo2(SetupID, out UserID, out Password,
				out SetupFileLength, out MapCfgID, out EventID, out ErrorCode, out SFTimestamp);
			lbMsgs.Items.Add("GetSetupInfo2: " + msg);
			lbMsgs.Items.Add("*** GetSetupInfo done ***");

			int		PacketSize = 1000;
			string	Packet;
			int		OutputPacketLength;
			int		nPackets = (SetupFileLength + PacketSize - 1) / PacketSize;
			for (int PacketNum = 0; PacketNum < nPackets; PacketNum++) {
				msg = ws1.GetSetupFile(SetupID, PacketSize, PacketNum, out Packet, out OutputPacketLength, out ErrorCode);
				msg = string.Format("Got packet {0} - data = {1}", PacketNum, Packet);
				lbMsgs.Items.Add(msg);
			}
		}

//---------------------------------------------------------------------------------------

		private void RunGetSetupInfo() {
			LLWS1		WS1 = new LLWS1();
			string		UserID, Password;
			int			SetupFileLength;
			int			MapCfgID;
			int			EventID;
			int			ErrorCode;
			string		msg;
			DateTime SetupTimestamp;
            msg = WS1.GetSetupInfo2(/*"A005M01YH"*/ "A005A01AD", out UserID, out Password,
				out SetupFileLength, out MapCfgID, out EventID, out ErrorCode, out SetupTimestamp);

			Application.DoEvents();

			string s;
			s = string.Format("Get SetupInfo - UID={0}, pass={1}", UserID, Password);
			lbMsgs.Items.Add(s);
			s = string.Format("Get SetupInfo - SetupFileLength={0}, MapCfgID={1}", SetupFileLength, MapCfgID);
			lbMsgs.Items.Add(s);
			s = string.Format("Get SetupInfo - EventID={0}, ErrorCode={1}", EventID, ErrorCode);
			lbMsgs.Items.Add(s);
			s = string.Format("Get SetupInfo -  returns {0}", msg);
			ShowStringsInListbox(s, "{0}");
		}

//---------------------------------------------------------------------------------------

		private void RunImporter() {
			string			MapData, RawSwipeData;
			bool bOK = GetManpAndVisitorData(out MapData, out RawSwipeData);
			if (! bOK) {
				return;
			}
			if (chkUseWebService.Checked == true) {
				CallWebService(MapData, RawSwipeData);
			} else {
				// CallSQLServer(MapData, RawSwipeData);
			}
		}

//---------------------------------------------------------------------------------------

		private bool GetManpAndVisitorData(out string MapData, out string RawSwipeData) {

#if false
			txtVisitorTxt.Text = @"\\ahmed-xp\LRS\Visitors and Map Files\VISITOR 2  recs - bad format.TXT";
			txtMapCfg.Text = @"\\ahmed-xp\LRS\Visitors and Map Files\Map.Cfg";
#endif

			MapData = GetFileContents(txtMapCfg.Text);
			RawSwipeData = GetFileContents(txtVisitorTxt.Text);

			if (MapData == null) {
				MessageBox.Show("Map.cfg data not available", "Test LL Importer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			if (RawSwipeData == null) {
				MessageBox.Show("Visitor.txt data not available", "Test LL Importer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			// Duh. Somehow (no clue), when the RawSwipeData is sent to the web service,
			// \r\n strings inside RawSwipeData (and presumably MapData) are being
			// converted to mere \n's. But this only happens on the remote site. On my
			// machine everything's jake. So to cope with this, we'll delete all \r's
			// in the input strings, and see if this helps.

			// TODO: See if removing next line will give a problem
			MapData = MapData.Replace("\r", "");
			RawSwipeData = RawSwipeData.Replace("\r", "");

			return true;
		}

//---------------------------------------------------------------------------------------
#if BIRTHDAY
		private double BirthDayParadox(double Days, double nPeople) {
			double odds = 1.0;
			for (int n = 1; n <= nPeople - 1; n++) {
				odds *= 1 - n / Days;
			}
			return 1 - odds;
		}
#endif

//---------------------------------------------------------------------------------------

		private string SOAPScrub(string txt, out List<int> ProblemLineNumbers) {
			ProblemLineNumbers = new List<int>();
			txt = txt.Replace("\r", "");
			string []	lines = txt.Split(new char [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++) {
				bool	bOK = SOAPScrubOneLine(ref lines[i]);
				if (! bOK) {
					ProblemLineNumbers.Add(i);
				}
			}
			string result = string.Join("\n", lines);
			return result; 
		}

//---------------------------------------------------------------------------------------

		private bool SOAPScrubOneLine(ref string txt) {
			bool	bOK = true;
			StringBuilder	sb = new StringBuilder(txt);
			for (int i = txt.Length - 1; i >= 0; i--) {
				if (char.IsControl(sb[i])) {
					sb.Remove(i, 1);
					bOK = false;
				}
			}
			if (bOK) {			// Don't garbage txt if not necessary
				return true;
			}
			txt = sb.ToString();
			return false;
		}

//---------------------------------------------------------------------------------------

		private void CallWebService(string MapData, string RawSwipeData) {

#if false
            // Test to find data type of SQL TimeStamp field. It's byte[8].
            string SQL = "SELECT * FROM tblSetupFile";
			SqlConnectionStringBuilder builder = SelectDB();
            using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                rdr.Read();
                object o = rdr["SFTimeStamp"];
                rdr.Close();		// Set breakpoint here, and look at <o>
            }
#endif

			int		nRecs = 0;
			foreach (char c in RawSwipeData) {
				if (c == '\n') {
					++nRecs;
				}
			}

			LLWS1			import = new LLWS1();
			string			result;
			ImportStatus	impStatus;
			int				EventID;
#if false
			string		UserID, Password;
			int			SetupFileLength;
			int			MapCfgID;
			int			ErrorCode;
			
			string		SetupID;
			// SetupID = "A000503VN";
			// SetupID = "A000501PG";
			SetupID = "A000505WX";
			
			// Note: Calling this has the unfortunate side-effect of creating a new
			//		 UserID/password every time.
			result = import.GetSetupInfo(SetupID, out UserID, out Password, 
				out SetupFileLength,
				out MapCfgID, out EventID, out ErrorCode);

			if (ErrorCode == -1) {
				MessageBox.Show("GetSetupInfo - ErrorCode=" + ErrorCode.ToString(), "Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
#endif
			// Replace EventID with our own manually-entered one, for test purposes
			EventID = 58;

			/*
			Regex	re;
			string	s;
			*/
			// The following is the line as (more or less) originally written
			// s = @"^(""(?<SeqNo>.*?)"")(,""(?<FieldCodes>.*?)"")*\s*$";
			// A commented version follows
			/*
			// This is the old (bad) one
			s = @"^(""(?<SeqNo>.*?)"")	# The beginning of the string (the ^)
										# A quoted field, named SeqNo.
										# Note .*?, not .*. Pick up the fewest
										# characters possible, not the maximal,
										# or we'll gobble up the rest of the line.
					# Again a quoted field, but this time with a leading comma.
					# We have zero or more of these. The actual contents inside the
					# quotes we call Fields.
					# We'll also allow white space at the end of the line.
					# We'll follow this with end-of-line (the $) to make sure there's
					# no garbage at the end (e.g. ...,'last parm'garbage)
					(,""(?<Fields>.*?)"")*\s*$";
			// This is the new (good) one
			s = @"^(""(?<SeqNo>[^""]*?)"")	# The beginning of the string (the ^)
										# A quoted field, named SeqNo.
										# Note .*?, not .*. Pick up the fewest
										# characters possible, not the maximal,
										# or we'll gobble up the rest of the line.
					# Again a quoted field, but this time with a leading comma.
					# We have zero or more of these. The actual contents inside the
					# quotes we call Fields.
					# We'll also allow white space at the end of the line.
					# We'll follow this with end-of-line (the $) to make sure there's
					# no garbage at the end (e.g. ...,'last parm'garbage)
					(,""(?<Fields>[^""]*?)"")*\s*$";
			re = new Regex(s, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);			
			string	text, txt;
			text = "'1','Ed Morris','259 E Lancaster Road','Bala Consulting','Eng','Lancaster','PA','19785','(641) 258-9654','(478) 587-4859','24','A','B','C','E','CH','','08-16-2006  15:06:03','Y','Y','N','N','Y','Y','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N',";
			// text = "'1','Ed Morris','259 E Lancaster Road','Bala Consulting','Eng','Lancaster','PA','19785','(641) 258-9654','(478) 587-4859','24','A','B','C','E','CH','','08-16-2006  15:06:03','Y','Y','N','N','Y','Y','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N','N'";

			// txt = text.Substring(0, 178 + 0*4 - 0*1);
			txt = text;
			txt = txt.Replace("'", "\"");
			DateTime	start = DateTime.Now;
			Match	m = re.Match(txt);
			TimeSpan	elapsed = DateTime.Now - start;
			string	msg = string.Format("Elapsed time = {0}\n\n{1}", elapsed, txt);
			MessageBox.Show(msg, "Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			*/

#if NO_TIMEOUT
			import.Timeout = -1;
			string	TerminalID = "";
			bool	IgnoreFirstRecord = true;
			bool	DataIsExpanded = true;
			bool	IsReplacementSwipe = false;
#else
			import.Timeout = 300 * 1000;			// TODO: The default is 100 seconds
#endif
			ImportStatus	[] RecStats;
			impStatus = null;
			RecStats  = null;
			try {
				this.Cursor = Cursors.WaitCursor;
				Stopwatch	sw = new Stopwatch();
				sw.Start();
				// result = import.Import("lldemo", "pass", EventID,
				int		flags = 0;
#if PROD		// TODO: Remove later
				result = "Nonce";
#else
				lbMsgs.Items.Add("******* About to call the importer");
				MapType	maptype = MapType.MapTypeText;
#if true
				EventID = 163;			// 163 = LRS Import Test 2007-03030
				DataSource	datasrc;
				
				UserID = "sessex1";
				Password = "bartset";

				UserID = "LRS1";
				Password = "bartset";
				datasrc = DataSource.LeadsLightning;

				UserID = "A110907";
				Password = "pd5w8c62";
				datasrc = DataSource.Leads2GoWiFi;
                EventID = 263;

				UserID = "Zhe-2007-10-26";
				Password = "Zhe-2007-10-26";
				datasrc = DataSource.Leads2GoWiFi;

				// UserID = "fdkyjn3y";
				// password = "fdkyjn3y";
				// datasrc = DataSource.Leads2GoWiFi;
#if false	// Temp for Zhe testing
				UserID = "aucwx7gn";
				password = "aucwx7gn";
				EventID = 107;			// 163 = LRS Import Test 2007-03030
				MapData = "65";
				maptype = MapType.MapTypeKey;
				datasrc = DataSource.Leads2GoWiFi;
#endif

				import.RequestEncoding = System.Text.Encoding.UTF8;

#else
				UserID = "ebgxamht";
				password = "ebgxamht";
				EventID = 107;
#endif
				// maptype = MapType.MapTypeKey;
				// MapData = "68";	// TODO:
				string	RCUserName = "";
				string	RCPassword = "";
				// RCUserName = "LRSRC";
				// RCPassword = "bartset";
				// RCUserName = "";
				// RCPassword = "";
				// RCUserName = "ahmedbart";
				// RCPassword = "ahmedbart";
                DateTime SetupTimestamp = default(DateTime);
                string SetupCode;
#if false
				SetupCode = "A005M01YH";
				SetupCode = "A005M027U";			// TODO:
#endif
				SetupCode = null;
                // SetupCode = null;
                // SetupTimestamp = null;

				string name = "LRS 2008-06-17-b";
				GetTestParms(name, out UserID, out Password, out RCUserName,
					out RCPassword, out EventID, out IgnoreFirstRecord,
					out DataIsExpanded, out IsReplacementSwipe, out datasrc, out flags);
				// TODO: Just temp, for Minimal-1
		// MapData = "222";
		// maptype = MapType.MapTypeKey;
		// TerminalID = "RT0007";
#if false
				UserID = "qw4pmsdj";
				password = "qw4pmsdj";
				EventID = 202;
				string terminalID = "A122-4439-0284-4103";
				MapData = @"##F@@   BARTIZAN CONNECTS   ##F@@" +
	"##F@@    YONKERS NEW YORK   ##F@@" +
	"##F@@ COPYRIGHT (c) 96 - 07 ##F@@" +
	"##F@@ CREATED BY Leads2Go 4.0.0.0 ##F@@" +
@"

[Terminal]
ID=A122-4439-0284-4103

[LABELS]
601 Voice Note
600 Image
20 Name
5 Street
4 Company
3 Title
6 City
7 State
8 Zip
9 Phone
10 Phone 2
1 Card #
17 Demog
11 Scan Time
12 Answers
18 Note

[SHOWHEADER]
Welcome to EXPO!THE DATABASE

[FILE]
601 600 20 5 4 3 6 7 8 9 10 1 17 17 17 17 17 17 11 12 18

[TIME]
DATEDISPLAY=MM/DD/YY

[DEMOGRAPHICS]
DHEADER1=ATTENDING AS:
A,ATTENDEE
B,BOARD OF DIRECTORS
C,EXHIBITOR APPOINTED CONTRACTOR
E,EXHIBITOR
F,STAFF
H,CHAIRMAN
I,MISCELLANEOUS
J,COMMITTEE CHAIRMAN
K,SPEAKER
M,SHOW MANAGEMENT
O,OFFICIAL CONTRACTOR
P,PROGRAMS
R,SERVICE SUPPLIER
S,PRESS
Z,PHOTOGRAPHERS
1,SPECIAL REGISTRANT1
2,SPECIAL REGISTRANT2
3,SPECIAL REGISTRANT3
DHEADER2=JOB FUNCTION:
A,PRESIDENT / DIRECTOR
B,MANUFACTURING MANAGEMENT
C,PROCESS ENGINEER
D,EQUIPMENT ENGINEER
E,ENGINEERING:ENVIRO/FACIL
F,ENVIRONMENT,HEALTH & SAFETY
G,SALES & MKTG:MANAGEMENT
H,SALES
I,MARKETING
J,PURCHASING:MANAGEMENT
K,PURCHASING
L,PRESS
M,OTHER
DHEADER3=MY DEGREE OF PURCHASE:
A,MAKE THE FINAL DECISION
B,RECOMMENDED-STRONGLY INFL FINL DEC
C,SPECIFY PRODS/SVCS WE NEED
D,PLAY NO ROLE IN PURCH PROCESS
DHEADER4=MY COMP GROSS SALES:
A,LESS THAN $1 MILLION
B,$1 - $5 MILLION
C,$5.1 - $10 MILLION
D,$10.1 - $25 MILLION
E,$25.1 - $50 MILLION
F,$50.1 - $75 MILLION
G,$75.1 - $100 MILLION
H,$100.1 MILLION AND UP
DHEADER5=YOUR COMP MAIN PRODUCTS:
A,ASSEMBLY & PACKAGING EQUIPMENT
B,CHEMICAL/GASES
C,CLEAN ROOM
D,CONSULTANT
E,DISCRETE DEVICES
F,INSPECTION/METROLOGY EQUIPMENT
G,INTEGRATED CIRCUITS
H,MATERIALS
I,PROCESS EQUIPMENT
J,SOFTWARE
K,TEST EQUIPMENT
L,OTHER
DHEADER6=EXHIBITS INTEREST:
A,ANALYSIS/MEASUREMENT
B,ASSEMBLY/HYBRID EQUIPMENT
C,ASSEMBLY/HYBRID MATERIALS
D,CLEAN ROOM
E,CONSULTING
F,ELECTRONIC DESIGN AUTOMATION
G,FACTORY CONTROL
H,FLAT PANEL DISPLAY
I,PHOTOMASK EQUIPMENT/MATERIALS
J,PROCESS CHEMICALS
K,PROCESS EQUIPMENT
L,PROCESS GASES/GAS HANDLING EQ
M,PROCESS MATERIALS
N,SERVICES
O,TEST
P,WATER HANDLING/STORAGE
Q,ENVIRONMENT,HEALTH & SAFETY

[REFERRALS]
DEFFROMPARTNER=
DEFTOPARTNER=
DEFPROVIDER=
<@@FROMPARTNER@@>
<@@TOPARTNER@@>
<@@PROVIDER@@>

[SURVEYQUES]

[QUESTIONS]
TRADE SHOWS
HEALTHCARE
TIME/ATTENDANCE
SECURITY
OTHER PURPOSE
UNDER $500
$500 TO $1,000
$1,000  $2,000
OVER $2,000
MAGNETIC STRIPE
BAR-CODE
FLOPPY DISK
BUILTIN PRNTR
Q & A  ABILITY
LIKES SIZE
ONE(1)
2 - 5
6 - 10
10 - 99
100+
GRAPHITE FURNACE
ZEEMAN FURNACE
FLOW INJECTION
TURBO PLUS SYSTEM
MODEL 300T SYSTEM
MODEL 400S SYSTEM
APPLICATION KITS
TURBO LC+ SYSTEM
5000LC SYSTEM
LC DETECTORS
LC PUMPS
AUTOSAMPLER
NEEDS OVER 6 MOS
PURIFICATION SYSTEM
GAS CHROMATOGRAPHS
THERMAL DESORPTION
AIR ANALYZER SYSTEM
TURBO GC PLUS
MODEL 1022 GC PLUS
PARAGON 1000
GRAMS ANALYST
EXPERT SEARCH
MICROSCOPE
SPECTRUM FOR OS/2
CUSTOMER TRAINING
SCICHEM LEASING
NEEDS IN 0-3 MOS
NEEDS IN 4-6 MOS
FUNDED PROJECT
HAVE SLS ENGR CALL
LAPTOPS
CD ROM DRIVES
TURBO DOS SYSTEM
PENTUIMS
PRINTERS
MULTI MEDIA
SOFTWARE
Send Line Card
Send Catalog
Send Detailed Data
Send Samples
Have Salesman Call
Provide Quote
Immediate Need
Setup Demo
End User
Distributor
VAR
OEM
Does Purchasing
Recommends
Final Say

[VISITOR RECORDS]";
				maptype = MapType.MapTypeText;
				impStatus = import.Import(UserID, password, "", "", EventID, "", MapData, maptype, terminalID, false, true,
											 DataSource.Leads2GoWiFi, false, 0, "", out RecStats); 
#else
				if (UserID != null) {
					impStatus = import.Import2(UserID, Password, RCUserName, RCPassword, EventID,
						RawSwipeData, MapData, maptype, TerminalID, IgnoreFirstRecord, DataIsExpanded,
						datasrc, IsReplacementSwipe, flags, "", SetupCode, SetupTimestamp, out RecStats);
				} else {
					impStatus = import.Import2(UserID, Password, "testRC", "bartset", EventID,
						RawSwipeData, MapData, maptype, "05060", false, false,
						// RawSwipeData, MapData, maptype, "03123", false, true, 
						// RawSwipeData, MapData, maptype, "LRSTerm", false, false, 
						DataSource.LeadsLightning, false, flags, "", SetupCode, SetupTimestamp, out RecStats);
				}
#endif
#endif
				sw.Stop();
				result = string.Format("Imported {0} record(s) in {1}, {2:G4} RPS, {3} milliseconds per record\n", 
					nRecs, sw.Elapsed, nRecs / (sw.ElapsedMilliseconds / 1000.0), sw.ElapsedMilliseconds / nRecs);
				Application.DoEvents();
#if false
#if SQL_LOGGING
			BartSqlCommandWithLogging	cmd = new BartSqlCommandWithLogging(SQL, conn);
#else
			SqlCommand	cmd = new SqlCommand(SQL, conn);
#endif
#endif
				// ShowStringsInListbox(result, "WebResult - {0}", impStatus, RecStats);
				ShowBadRecords(impStatus, RecStats);
			} catch (Exception ex) {
				ShowStringsInListbox(ex.Message, "***** Exception ***** - {0}", impStatus, RecStats);
				return;
			} finally {
				this.Cursor = Cursors.Arrow;
			}
			ShowStatsAfterImport(impStatus, RecStats);
}

//---------------------------------------------------------------------------------------

		private void ShowStatsAfterImport(ImportStatus impStatus, ImportStatus[] RecStats) {
			string dbgFilename = @"C:\LRS\TestLLImport.txt";
			StreamWriter wtr = new StreamWriter(dbgFilename, chkAppendToOutputFile.Checked);
			if (chkAppendToOutputFile.Checked) {
				wtr.WriteLine();
			}
			wtr.WriteLine("Import-as-a-whole Debug Info:\n{0}\n", impStatus.DebugInfo);
			StringBuilder sb = new StringBuilder();
			foreach (ImportStatus stat in RecStats) {
				ShowRecStat(sb, stat);
			}
			sb.AppendFormat("Total import error code={0}\n", impStatus.ErrCode);
			ShowRecStat(sb, impStatus);
			string res2 = sb.ToString();
			res2 = res2.Replace("\n", "\r\n");
			wtr.WriteLine(res2);
			wtr.Close();
			// Process.Start(@"C:\Program Files\Notepad++\notepad++.exe", dbgFilename);
			Edit(dbgFilename);
			// Process.Start(@"C:\Windows\System32\notepad.exe", dbgFilename);
			this.BringToFront();		// MsgBox routinely gets buried. Try to fix that.
			MessageBox.Show("Done", "Test via Web Service", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//---------------------------------------------------------------------------------------

		private void Run1DImporter() {
			string MapData, RawSwipeData;
			bool bOK = GetManpAndVisitorData(out MapData, out RawSwipeData);
			if (!bOK) {
				return;
			}

			int nRecs = 0;
			foreach (char c in RawSwipeData) {
				if (c == '\n') {
					++nRecs;
				}
			}

			LLWS1 importer1D = new LLWS1();
			importer1D.RequestEncoding = System.Text.Encoding.UTF8;
			importer1D.Timeout = -1;

			string		UserID, Password, ImportingUserID, ImportingPassword;
			int			EventID;
			bool		bIgnoreFirstRecord, bDataIsExpanded, bIsReplacementSwipe;
			int			Flags;
			DataSource	datasrc;

			ImportStatus []	RecStats = null;

			ImportStatus	impStatus = null;
			string			result;

			GetTestParms("1D-1", out UserID, out Password, out ImportingUserID,
				out ImportingPassword, out EventID, out bIgnoreFirstRecord, 
				out bDataIsExpanded, out bIsReplacementSwipe, out datasrc, out Flags);

			try {
				this.Cursor = Cursors.WaitCursor;
				Stopwatch sw = new Stopwatch();
				sw.Start();
#if true
				impStatus = importer1D.(UserID, Password, ImportingUserID,
					ImportingPassword, EventID, Flags, RawSwipeData, out RecStats);
#endif
				sw.Stop();
				result = string.Format("Imported {0} record(s) in {1}, {2:G4} RPS, {3} milliseconds per record\n",
					nRecs, sw.Elapsed, nRecs / (sw.ElapsedMilliseconds / 1000.0), sw.ElapsedMilliseconds / nRecs);
				Application.DoEvents();
#if false
#if SQL_LOGGING
			BartSqlCommandWithLogging	cmd = new BartSqlCommandWithLogging(SQL, conn);
#else
			SqlCommand	cmd = new SqlCommand(SQL, conn);
#endif
#endif
				// ShowStringsInListbox(result, "WebResult - {0}", impStatus, RecStats);
				ShowBadRecords(impStatus, RecStats);
			} catch (Exception ex) {
				ShowStringsInListbox(ex.Message, "***** Exception ***** - {0}", impStatus, RecStats);
				return;
			} finally {
				this.Cursor = Cursors.Arrow;
			}
			ShowStatsAfterImport(impStatus, RecStats);
		}

//---------------------------------------------------------------------------------------

		private void GetTestParms(
					string TestCase, 
					out string		UserID, out string Password,
					out string		ImportingUserID, out string ImportingPassword, 
					out int			EventID, 
					out bool		bIgnoreFirstRecord, 
					out bool		bDataIsExpanded, 
					out bool		bIsReplacementSwipe, 
					out DataSource	datasrc, 
					out int			Flags) {
			switch (TestCase) {
			case "1D-1":
				UserID = "LRSRC";
				Password = "bartset"; 
				ImportingUserID = "";	// "ahmedbart";
				ImportingPassword = ""; // "ahmedbart";
				EventID = 278;
				bIgnoreFirstRecord = true;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = false;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "LRS-2008-06-13a":
				UserID = "LRSRC";
				Password = "bartset"; 
				ImportingUserID = "";	// "ahmedbart";
				ImportingPassword = ""; // "ahmedbart";
				EventID = 394;
				bIgnoreFirstRecord = false;
				Flags = 0;
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "LRS-2008-06-17-a":
				UserID = "LRSRC";
				Password = "bartset"; 
				ImportingUserID = "";	// "ahmedbart";
				ImportingPassword = ""; // "ahmedbart";
				EventID = 398;
				bIgnoreFirstRecord = false;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "LRS 2008-06-17-b":
				UserID = "LRSRC";
				Password = "bartset"; 
				ImportingUserID = "";	// "ahmedbart";
				ImportingPassword = ""; // "ahmedbart";
				EventID = 402;
				bIgnoreFirstRecord = false;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "Test 01-22-08 A":
				UserID = "LRSRC";
				Password = "bartset"; 
				ImportingUserID = "";	// "ahmedbart";
				ImportingPassword = ""; // "ahmedbart";
				EventID = 295;
				bIgnoreFirstRecord = false;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "13-57-21":
				UserID = "ahskdcnc";
				Password = "";
				ImportingUserID = "ahmedbart";	// "ahmedbart";
				ImportingPassword = "ahmedbart"; // "ahmedbart";
				EventID = 303;
				bIgnoreFirstRecord = false;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "Con16":
				UserID = "Con16";
				Password = "";
				ImportingUserID = "lrsrc";	
				ImportingPassword = "bartset"; 
				EventID = 327;
				bIgnoreFirstRecord = true;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "Minimal-1":
				UserID = "Con19";			// Con19 or Con19-b
				Password = "";
				ImportingUserID = "lrsrc";	
				ImportingPassword = "bartset"; 
				EventID = 335;
				bIgnoreFirstRecord = true;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			case "Minimal-2":
				UserID = "Con19-b";			// Con19 or Con19-b
				Password = "";
				ImportingUserID = "lrsrc";	
				ImportingPassword = "bartset"; 
				EventID = 335;
				bIgnoreFirstRecord = true;
				Flags = 0;
				// Next fields not needed for this test case
				bDataIsExpanded = true;
				bIsReplacementSwipe = false;
				datasrc = DataSource.LeadsLightning;
				break;
			default:
				throw new Exception("Unknown Test Case in GetTestParms: " + TestCase);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowBadRecords(ImportStatus impStatus, ImportStatus[] RecStats) {
			lvBadRecs.Items.Clear();
			if (impStatus.ErrCode == ImportStatusErrCode.OK) {
				return;
			}
			for (int i=0; i<RecStats.Length; ++i) {
				ImportStatus stat = RecStats[i];
				if (stat.ErrCode != ImportStatusErrCode.OK) {
					ListViewItem item = new ListViewItem(i.ToString());
					item.Tag = RecStats[i];
					lvBadRecs.Items.Add(item);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowRecStat(StringBuilder sb, ImportStatus stat) {
			sb.AppendFormat("Swipe - Error Code={0}, Recno={1}, SwipeID={2}, Dup={3}\n", 
				stat.ErrCode, stat.RecNo, stat.SwipeID, stat.Duplicate);
			sb.Append(stat.ErrMsgs);
			sb.Append(stat.DebugInfo);
		}

//---------------------------------------------------------------------------------------

#if false		// Not used any more, but keep around until (much) later
		private void xCallSQLServer(string MapData, string RawSwipeData) {
			string prefix;
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
#if LOCAL
			builder.DataSource = "(local)";
			builder.IntegratedSecurity = true;
			builder.InitialCatalog = "LLDevel";
			prefix = "";
#elif DEVEL
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			prefix = "ahmed.";
#elif PROD
#warning You really should use DEVEL or LOCAL
			builder.DataSource = "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			prefix = "ahmed.";
#elif NEWSERVER
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				try {
					conn.Open();
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, "Connection Open Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					throw;
				}
				conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);

				SqlCommand cmd = new SqlCommand(prefix + "LL_sp_LLImport", conn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserID", "Ahmedex");
				cmd.Parameters.AddWithValue("@Password", "pass");
				cmd.Parameters.AddWithValue("@EventID", 2);		// Note: Fill in later
				cmd.Parameters.AddWithValue("@SwipeData", RawSwipeData);
				cmd.Parameters.AddWithValue("@MapCfgFile", MapData);
				cmd.Parameters.AddWithValue("@MapType", 1);
				cmd.Parameters.AddWithValue("@TerminalID", "");
				// cmd.Parameters.AddWithValue("@Results", Results);
				cmd.CommandTimeout = 0;			// Note: Make >0 later
				try {
					this.Cursor = Cursors.WaitCursor;
					int n = cmd.ExecuteNonQuery();
				} finally {
					this.Cursor = Cursors.Arrow;
				}
				conn.Close();
			}
			MessageBox.Show("Done", "Test via SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
#endif

//---------------------------------------------------------------------------------------

		void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e) {
			ShowStringsInListbox(e.Message, "InfoMessage - {0}");
		}

//---------------------------------------------------------------------------------------

		private void ShowStringsInListbox(string msg, string fmt, ImportStatus ImpStat, ImportStatus [] RecStatuses) {
			string[] msgs = msg.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			lbMsgs.Items.Add("");
			lbMsgs.Items.Add("    *** Base messages ***");
			foreach (string txt in msgs) {
				lbMsgs.Items.Add(string.Format(fmt, txt));
			}
			if (ImpStat != null) {
				lbMsgs.Items.Add(string.Format("ErrorCode = {0}", ImpStat.ErrCode));
				lbMsgs.Items.Add("    *** Importer DebugInfo messages ***");
				msgs = ImpStat.DebugInfo.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
				lbMsgs.Items.AddRange(msgs);
//				foreach (string txt in msgs) {
//					lbMsgs.Items.Add(string.Format(fmt, txt));
//				}
				lbMsgs.Items.Add("    *** Importer messages ***");
				msgs = ImpStat.ErrMsgs.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string txt in msgs) {
					lbMsgs.Items.Add(string.Format(fmt, txt));
				}
			}
			if (RecStatuses != null) {
				lbMsgs.Items.Add("    *** Swipe DebugInfo messages ***");
				foreach (ImportStatus stat in RecStatuses) {
					lbMsgs.Items.Add(string.Format("Recno={0}, ErrCode={1}, SwipeID={2}, Dup={3}", stat.RecNo, stat.ErrCode, stat.SwipeID, stat.Duplicate));
					
					msgs = stat.ErrMsgs.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in msgs) {
						lbMsgs.Items.Add(string.Format("ErrorMsg = {0}", s));
					}
					
					msgs = stat.DebugInfo.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in msgs) {
						lbMsgs.Items.Add(string.Format("DebugInfo = {0}", s));
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowStringsInListbox(string msg, string fmt) {
			ShowStringsInListbox(msg, fmt, null, null);
		}

//---------------------------------------------------------------------------------------

		private string GetFileContents(string Filename) {
			try {
				return File.ReadAllText(Filename);
			} catch (Exception /* ex */) {
				return null;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnClearListbox_Click(object sender, EventArgs e) {
			lbMsgs.Items.Clear();
		}

//---------------------------------------------------------------------------------------

		private void DeleteDataFor(string UserID, string password, int EventID) {
			SqlConnectionStringBuilder builder = SelectDB();
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				conn.Open();
				string	SQL;
				SQL = "SELECT AcctID FROM tblAccounts WHERE UserID_Original = @UserID AND PasswordInPlainText = @Password";
				SqlCommand	cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@UserID", UserID);
				cmd.Parameters.AddWithValue("@Password", password);
				object	oAcctID = cmd.ExecuteScalar();
				if (oAcctID == null) {
					MessageBox.Show("Couldn't find Account", "TestLLImport", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				int		AcctID = (int)oAcctID;

				SQL = "DELETE FROM tblSwipes WHERE AcctID=@AcctID AND EventID=@EventID";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@AcctID", AcctID);
				cmd.Parameters.AddWithValue("@EventID", EventID);
				int nSwipes = cmd.ExecuteNonQuery();
				MessageBox.Show(string.Format("Swipe records deleted - {0}", nSwipes), "TestLLImport", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		internal static SqlConnectionStringBuilder GetConnectionString() {
			return GetConnectionString(DBID);
		}

//---------------------------------------------------------------------------------------

		internal static SqlConnectionStringBuilder GetConnectionString(string dbid) {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			// string prefix;
			// ThrowImporterException("Connecting to DBID={0}", dbid);
			switch (dbid) {			// Note: Deliberately case sensitive
				// WARNING: Keep these constants in synch with the definition of, for
				//			example, SYSTEM_DEVEL. (It's too bad C# won't take static
				//			strings as well as constants, else I'd code
				//				case SYSTEM_DEVEL:
				//			rather than
				//				case "DEVEL":
#if false		// LOCAL
			case "LOCAL":
				builder.DataSource = "(local)";
				builder.IntegratedSecurity = true;
				builder.InitialCatalog = "LLDevel";
				// prefix = "";
				conn.ConnectionString = builder.ConnectionString;
				break;
#endif
				case "DEVEL":
					builder.DataSource = "SQLB5.webcontrolcenter.com";
					builder.InitialCatalog = "LLDevel";
					builder.UserID = "ahmed";
					builder.Password = "i7e9dua$tda@";
					// prefix = "ahmed.";
					// conn.ConnectionString = builder.ConnectionString;
					break;
				case "DEMO":
					builder.DataSource = "SQLB13.webcontrolcenter.com";
					builder.InitialCatalog = "leadslightning";
					builder.UserID = "ahmed";
					builder.Password = "i7e9dua$tda@";
					// prefix = "ahmed.";
					// conn.ConnectionString = builder.ConnectionString;
					break;
#if false		// PROD
			case "PROD-xx":
				builder.DataSource = "SQLB2.webcontrolcenter.com";
				builder.InitialCatalog = "LeadsLightning";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				// prefix = "ahmed.";
				conn.ConnectionString = builder.ConnectionString;
				break;
#endif
#if false		// OLDPRODUCTIONSERVER
			case "OLDPRODUCTIONSERVER":
				builder.DataSource = "75.126.77.59,1092"; 
				builder.InitialCatalog = "LeadsLightning";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				conn.ConnectionString = builder.ConnectionString;
				break;
#endif
				case "PROD":
					builder.DataSource = "198.64.249.6,1092";
					builder.InitialCatalog = "LeadsLightning";
					builder.UserID = "sa";
					builder.Password = "$yclahtw2007bycnmhd!";
					// conn.ConnectionString = builder.ConnectionString;
					break;
				case "PROD-Ahmed":
					builder.DataSource = "198.64.249.6,1092";
					builder.InitialCatalog = "LeadsLightning";
					builder.UserID = "ahmed";
					builder.Password = "i7e9dua$tda@";
					// conn.ConnectionString = builder.ConnectionString;
					break;
#if false		// INHOUSE
			case "INHOUSE":
				builder.DataSource = "75.126.77.59,1092";
				builder.InitialCatalog = "LeadsLightning_2007-07-25";
				builder.UserID = "ahmed";
				builder.Password = "i7e9dua$tda@";
				// prefix = "ahmed.";
				conn.ConnectionString = builder.ConnectionString;
				break;
#endif
#if false		// INSIDESQL
			case "INSIDESQL":
				conn.ConnectionString = "context connection=true";
				break;
#endif
				default:
					throw new Exception("Internal Error - Unknown database source ({0}"
						+ ") in SetConnectionString " + dbid);
			}
			return builder;
		}

//---------------------------------------------------------------------------------------

		private static SqlConnectionStringBuilder SelectDB() {
#if true
			return GetConnectionString();
#else
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
#if NEWSERVER
			// builder.DataSource = "75.126.77.59,1092";
			builder.DataSource = "198.64.249.6,1092";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "sa";
			builder.Password = "$yclahtw2007bycnmhd!";
#else
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
#endif
			return builder;
#endif
		}

//---------------------------------------------------------------------------------------

		private static SqlConnectionStringBuilder SelectDB(string dbid) {
			return GetConnectionString(dbid);
		}

//---------------------------------------------------------------------------------------

		private void lvBadRecs_SelectedIndexChanged(object sender, EventArgs e) {
			ListView view = (ListView)sender;
			ListView.SelectedListViewItemCollection items = view.SelectedItems;
			ImportStatus	stat = (ImportStatus)(items[0].Tag);
			string txt = stat.DebugInfo;
			txt = txt.Replace("\r", "");
			txt = txt.Replace("\n", "\r\n");
			txtDebugRecord.Text = txt;

		}

//---------------------------------------------------------------------------------------

		private void btnEditMap_Click(object sender, EventArgs e) {
			Edit(txtMapCfg.Text);
		}

//---------------------------------------------------------------------------------------

		private void btnEditVisitor_Click(object sender, EventArgs e) {
			Edit(txtVisitorTxt.Text);
		}

//---------------------------------------------------------------------------------------

		private void Edit(string filename) {
			// 	p = new Process();
			string noteName = @"C:\Program Files\Notepad++\notepad++.exe";
			if (! File.Exists(noteName)) {
				noteName = @"C:\Windows\System32\Notepad.exe";
				if (! File.Exists(noteName)) {
					MessageBox.Show("Could not find any flavor of Notepad. Can't edit.");
					return;
				}
			}
			Process p = Process.Start(noteName, "\"" + filename + "\"");
			// ProcessStartInfo	psi = new ProcessStartInfo(@"C:\Program Files\Notepad++\notepad++.exe");
			// // ProcessStartInfo	psi = new ProcessStartInfo("Notepad.exe");
			// psi.Arguments = filename;
			// psi.UseShellExecute = false;
			// p.StartInfo = psi;
			// p.Start();
			// // p.WaitForExit();
		}
	}

}
