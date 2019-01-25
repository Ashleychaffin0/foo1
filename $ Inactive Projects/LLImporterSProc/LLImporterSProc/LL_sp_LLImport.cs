// Copyright (c) 2006 by Bartizan Connects, LLC

// TODO:
//	*	GetAcctID needs password support
//	*	[FILE] concatenated fields
//	*	Add support for tblDemogsByAcctAndEvent
//	*	Worry about tblStrings later

// Data Source=SQLB2.webcontrolcenter.com;Initial Catalog=leadslightning;User ID=ahmed
// Data Source=SQLB2.webcontrolcenter.com;Initial Catalog=LLDevel;User ID=ahmed
// Password=i7e9dua$tda@
// Data Source=(local);Initial Catalog=LLRS;IntegratedSecurity=true

// Note: These must be kept in synch with other factors, such as who's calling
//		 us (test driver, production web service, etc), and also the deployment
//		 parameter in VS2005.
// #define		PROD			// Production, not debug, version
#define		DEVEL			// Debug, but on the remote Server
// #define		LOCAL			// Here, locally, but still on LLDevel

#define	DBG				// If defined, pump out dbg(...) messages

// Define the following, uh, #define to configure this to call the
// Importer directly, rather than as an sproc.
#define		TEMP_CALL_IMPORTER_DIRECTLY

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Microsoft.SqlServer.Server;
using System.Text;

using Bartizan.Importer;

namespace Bartizan.LL.Importer {

	public partial class StoredProcedures {


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


		/// <summary>
		/// The main (only?) interface to the LeadsLightning Import process.
		/// </summary>
		/// <param name="Swipes">A CRLF-delimited string of swipe (VISITOR.TXT) records.
		/// </param>
		/// <param name="MapCfgFile">This is either the actual text (CRLF-delimited)of a 
		/// MAP.CFG that corresponds to the Swipes data, or else a key used to do a database
		/// search for the text. Which it is, is determined by the following MapType parm.
		/// </param>
		/// <param name="MapType">Since, AFAIK, SQL Server doesn't support overloaded sprocs,
		/// we need some way to distinguish among the possibly interpretations of the
		/// MapCfgFile parameter. Is it the contents of the file itself? Is it the name of a
		/// file? Is it the key into some table from which we can retrieve the contents?
		/// Hence this parm. Note that it really should be an enum, but again, I don't know
		/// enough about SQL Server to know if we can do this with, say, UDTs. And yes, we
		/// could define a couple of sprocs, sp_GetMapTypeText, sp_GetMapTypeFile,
		/// sp_GetMapTypeKey, etc. But hey, this routine's internal to LL, and will be called
		/// from just a few places. So we'll leave it as follows:
		/// <list type="bullet">
		/// <item><description>
		/// 1 = The text itself.
		/// </description></item>
		/// <item><description>
		/// 2 = A key into a table.
		/// </description></item>
		/// </list></param>

		// Note: At first I tried MapTypeText = 0, but when I tried to invoke this sproc
		//		 with that as a parameter value, it seems that ADO.NET/SQL Server uses 0
		//		 to mean "the default value". Sigh. So we bumped everthing up by 1.
		private const int MapTypeText = 1;
		private const int MapTypeKey = 2;

		// TODO: Get rid of Pipe.Send - uh, not quite. They're the (rough) equivalent of
		//		 Debug.Write. One note, though. While the messages sent this way show up in
		//		 the VS Output|Debug window, they clearly are not the result of calling
		//		 OutputDebugString, and do *not* show up in, say, DbgView from Sysinternals.
		//		 Which is, I suppose, reasonable, since you can "return" a Rowset this way.
		//		 Note: I finally figured out what's going on here. Any Pipe.Send's show up
		//		 as InfoMessage events of the SQLConnection class. e.g.
		//		 conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);

//---------------------------------------------------------------------------------------

		[Conditional("DBG")]
		public static void dbg(string fmt, params object[] vals) {
			string s = string.Format(fmt, vals);
			SqlContext.Pipe.Send(s);
		}

//---------------------------------------------------------------------------------------

		public static void Err(string fmt, params object[] vals) {
			string s = string.Format("Error - " + fmt, vals);
			SqlContext.Pipe.Send(s);
		}


//---------------------------------------------------------------------------------------

#if TEMP_CALL_IMPORTER_DIRECTLY
		public static void LL_sp_LLImport(string UserID, string Password, int EventID,
						string SwipeData,
						string MapCfgFile,
					int MapType, string TerminalID) {

			try {
				Process(UserID, Password, EventID, SwipeData, MapCfgFile, MapType, TerminalID);
			} catch (Exception ex) {
				// TODO: This is hardly enough information. Get at least the record number
				Err("***** Unexpected error in LL_sp_LLImport - {0}", ex.Message);
			}
		}
#else
	[Microsoft.SqlServer.Server.SqlProcedure]
	// public static int LL_sp_LLImport(string Swipes, string MapCfgFile, int MapType, string TerminalID, ref string Results) {
	public static void LL_sp_LLImport(string UserID, string Password, int EventID,
				[Microsoft.SqlServer.Server.SqlFacet(MaxSize = -1)]	// nvarchar(max)
					string SwipeData,
				[Microsoft.SqlServer.Server.SqlFacet(MaxSize = -1)]	// nvarchar(max)
					string MapCfgFile,
				int MapType, string TerminalID) {

		try {
			Process(UserID, Password, EventID, SwipeData, MapCfgFile, MapType, TerminalID);
		} catch (Exception ex) {
			// TODO: This is hardly enough information. Get at least the record number
			Err("***** Unexpected error in LL_sp_LLImport - {0}", ex.Message);
		}
	}
#endif

//---------------------------------------------------------------------------------------

		private static void Process(string UserID, string Password, int EventID, string SwipeData, string MapCfgFile, int MapType, string TerminalID) {
			string MapContents = null;

			// Results = "Hello world from LRS";
			dbg("Entered LL_sp_LLImport A");
			dbg("And leaving LL_sp_LLImport");

#if LOCAL
			// Do nothing
#else
#if DEVEL
			// MAJOR TODO: For debugging, just get in here and leave.
			if (UserID != null && UserID != "lkjsdfklsdflksdlfjksdljkf") {
				return;
			}
#else
#if PROD
		// MAJOR TODO: For debugging, just get in here and leave.
		if (UserID != null && UserID != "lkjsdfklsdflksdlfjksdljkf") {
			return;
		}
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif
#endif
#endif

			using (SqlConnection conn = new SqlConnection()) {
#if TEMP_CALL_IMPORTER_DIRECTLY
				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
				string prefix;
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
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif
#else
			conn.ConnectionString = "context connection=true";
#endif
				conn.Open();

#if false
			SetConfigXML_RunOnceOnly(conn);
			dbg("Config XML Set");		
#endif

				// Yeah, MapType should be an enum. But since we don't have SourceSafe yet,
				// it's a nuisance to manually create and maintain multiple copies of such a
				// file. So we'll take a short cut here. Hopefully later (but before we 
				// ship), this can be fixed. TODO:
				switch (MapType) {
				case MapTypeText:
					MapContents = MapCfgFile;
					if (MapContents == null) {
						dbg("Error - No Map.CFG file provided (Diagnostic info - MapCfgFile=null for MapType={0}", MapType);
						return;
					}
					break;
				case MapTypeKey:
					MapContents = GetMapFromDB(conn, MapCfgFile);
					if (MapContents == null) {
						dbg("Error - Unable to find Map.CFG file for ID {0} (Diagnostic info - MapCfgFile={1}", MapCfgFile, MapType);
						return;
					}
					break;
				default:
					dbg("Nonce error - MapType of {0} not supported.", MapType);
					return;
				}

				dbg("About to init BartIniFile");
				MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(MapCfgFile), false);
				BartIniFile suf = new BartIniFile(ms);
				MapCfg cfg = new MapCfg(suf, TerminalID);

#if true
				string[] Swipes = SwipeData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				dbg("About to echo SwipeData");
				int nRecs = 0;
				foreach (string s in Swipes) {
					dbg("{0}: {1}", ++nRecs, s);
				}
				dbg("Finished echoing SwipeData");
#endif

				string xmlFirmwareFields = GetFirmwareFields(conn);
				if (xmlFirmwareFields == null) {
					dbg("Unable to load XML Firmware field definitions");
					return;
				}
				ParsedSwipe ParsedData = new ParsedSwipe(xmlFirmwareFields, cfg);
				int AcctID;

				bool bEventIDOK = CheckEventID(conn, EventID);
				if (!bEventIDOK) {
					Err("Event not found. Diagnostic info - EventID = {0:X}", EventID);
					return;
				}
				AcctID = GetAcctID(conn, UserID, Password);

				DBParms dbparms = new DBParms(conn, AcctID, EventID);

				string TrimmedSwipe;
				int RecNo = 0;
				SqlTransaction tx = null;
				foreach (string swipe in Swipes) {
					// I've seen data come in with blank lines, sometimes consisting only
					// of tab character(s). Check for that case and bypass processing if so.
					TrimmedSwipe = swipe.Trim();
					if (TrimmedSwipe.Length == 0) {
						continue;
					}
					try {
						tx = conn.BeginTransaction();
						++RecNo;
						ParsedData.GetFields(TrimmedSwipe);
						ImportAll(dbparms, ParsedData);
						// TODO: Check if there's any way we can get here if the x'n failed.
						tx.Commit();
					} catch (Exception ex) {
						dbg("Error - Swipe record {0} could not be processed."
							+ " Skipping to next swipe. Input record = {1}"
							+ "\nDiagnostic info = '{2}'", RecNo, swipe, ex.Message);
						tx.Rollback();
					}
				}
			}

			dbg("About to return from LL_sp_LLImport");
			return;
		}

//---------------------------------------------------------------------------------------

		private static string GetMapFromDB(SqlConnection conn, string MapCfgID) {
			string MapCfg = null;		// Default return value if error
			string ID = MapCfgID.Trim();
			string SQL = "SELECT MapCfgContents FROM tblMapCfg WHERE MapCfgID = @MapCfgID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@MapCfgID", MapCfgID);
			object result = cmd.ExecuteScalar();
			if (result == DBNull.Value) {
				return MapCfg;
			}
			MapCfg = (string)result;
			return MapCfg;
		}

		//---------------------------------------------------------------------------------------

private static int GetAcctID(SqlConnection conn, string UserID, string Password) {
			// TODO: For now, just check that the UserID is there. Don't validate password.
			string SQL = "SELECT AcctID FROM tblAccounts WHERE UserID = @UserID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@UserID", UserID);
			int ID = GetID(cmd.ExecuteScalar());
			if (ID == -1) {
				ThrowImporterException("Couldn't find UserID {0}, or the password"
					+ " did not match", UserID);
			}
			return ID;
		}

//---------------------------------------------------------------------------------------

		private static bool CheckEventID(SqlConnection conn, int EventID) {
			string SQL = "SELECT COUNT(*) FROM tblEvents WHERE EventID = @EventID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@EventID", EventID);
			int count = (int)cmd.ExecuteScalar();
			return count != 0;		// true=EventID found, false=not found
		}

//---------------------------------------------------------------------------------------

		private static void ImportAll(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering ImportAll");
			bool bOK;
			int SwipeID = GetSwipeID(parms, ParsedData);
			if (SwipeID != -1) {		// See if record already there
				return;
			}

			int PersonEventID = GetPersonEventID(parms, ParsedData);
			parms.PersonEventID = PersonEventID;

			bOK = CreateMasterSwipeRecord(parms, ParsedData, out SwipeID);
			if (!bOK)
				return;
			parms.SwipeID = SwipeID;
			// ImportBasicData(parms, ParsedData);
			ImportDemographics(parms, ParsedData);
			ImportSurvey(parms, ParsedData);
			// ImportSessions(parms, ParsedData);			// TODO:
		}

//---------------------------------------------------------------------------------------

		private static int GetSwipeID(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering GetSwipeID");
			int TermID = GetTerminalID(parms, ParsedData);
			parms.TermID = TermID;
			if (TermID == -1) {
				return -1;			// TODO: Better error handling
			}
			string SQL = "SELECT SwipeID FROM tblSwipes WHERE "
						+ "AcctID = @AcctID AND EventID = @EventID AND TerminalID = @TerminalID "
						+ "AND SwipeDate = @SwipeDate";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@TerminalID", TermID);
			cmd.Parameters.AddWithValue("@SwipeDate", ParsedData.SwipeTimestamp);
			// Note: I suppose we could also check for PersonEventID, but a swipe for a
			//		 given account, for a given event, on a given terminal, at a specific
			//		 time, seems unique.
			int SwipeID = GetID(cmd.ExecuteScalar());
			// At this point, SwipeID could either be a legitimate ID, or else -1. If the
			// latter, we've probably had some kind of error, and we'll just return the -1
			// that GetID returns to indicate a problem.
			return SwipeID;
		}

//---------------------------------------------------------------------------------------

		private static int GetTerminalID(DBParms parms, ParsedSwipe ParsedData) {
			SqlConnection conn = parms.conn;
			dbg("Entering GetTerminalID");
			string TermName = ParsedData.mapcfg.TerminalName;
			string SQL = "SELECT ID AS TerminalID FROM tblTerminal WHERE TerminalSerial = @TerminalSerial";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@TerminalSerial", TermName);
			object ID = cmd.ExecuteScalar();
			int ident = GetID(ID);
			if (ident != -1) {
				return ident;
			}

			// Terminal ID not found. Add it.
			SQL = "INSERT INTO tblTerminal(TerminalSerial) Values(@TerminalSerial)";
			cmd = new SqlCommand(SQL, conn);

			cmd.Parameters.AddWithValue("@TerminalSerial", TermName);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;
			}

			return GetIdent(parms, "tblTerminal");
		}

//---------------------------------------------------------------------------------------

		private static int GetID(object ID) {
			if ((ID == null) || (ID is DBNull)) {
				return -1;
			}
			// Sigh. Access returns an int for @@IDENTITY, but SQL Server returns
			// a Decimal, so we have to use Convert.
			return Convert.ToInt32(ID);
		}

//---------------------------------------------------------------------------------------

		private static bool CreateMasterSwipeRecord(DBParms parms, ParsedSwipe ParsedData, out int SwipeID) {
			dbg("Entering CreateMasterSwipeRecord");
			string SQL = "INSERT INTO tblSwipes(AcctID, EventID, TerminalID, PersonEventID,"
						+ "SwipeDate, DataSource)\n"
						+ "VALUES(@AcctID, @EventID, @TerminalID, @PersonEventID, @SwipeDate, @DataSource)";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);

			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@TerminalID", parms.TermID);
			cmd.Parameters.AddWithValue("@PersonEventID", parms.PersonEventID);
			cmd.Parameters.AddWithValue("@SwipeDate", ParsedData.SwipeTimestamp);
			cmd.Parameters.AddWithValue("@DataSource", 1);		// TODO:

			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				SwipeID = -1;
				return false;
			}

			SwipeID = GetIdent(parms, "tblSwipes");
			return true;
		}

//---------------------------------------------------------------------------------------

		private static int GetPersonEventID(DBParms parms, ParsedSwipe ParsedData) {
			// Glue all basic fields (i.e. non-demographics, but including custom fields) 
			// together. But be careful. If the .Net framework ever changes so that the
			// order of the fields retrieved from the BasicData Dictionary<string, string>
			// ever changes, the lookup process is screwed. So sort the keys first, to ensure
			// that we're always looking up consistent data.
			Dictionary<string, string>.KeyCollection KeyColl = ParsedData.BasicData.Keys;
			string[] Keys = new string[KeyColl.Count];
			KeyColl.CopyTo(Keys, 0);
			Array.Sort(Keys);
			StringBuilder sb = new StringBuilder();
			string sep = "¶";		// char.ConvertFromUtf32(0xB6);
			foreach (string key in Keys) {
				sb.Append(key);
				sb.Append("=");
				sb.Append(ParsedData.BasicData[key]);
				sb.Append(sep);
			}

			string HashValue = GetMD5String(sb.ToString());

			// OK, we now have the hash value. Look it up.

			string SQL = "SELECT PersonEventID FROM tblPersonByEvent "
						+ "\n WHERE EventID = @EventID "
						+ "\n   AND HashValue = @HashValue";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@HashValue", HashValue);

			int PersonEventID = GetID(cmd.ExecuteScalar());
			if (PersonEventID >= 0) {
				return PersonEventID;
			}

			// OK, we didn't find the person. Too bad. So add it to the table, and also add
			// the person's information to tblPersonInfoText;

			// First, add the data to tblPersonByEvent, and get the PersonEventID Autonum.
			SQL = "INSERT INTO tblPersonByEvent(EventID, HashValue)"
				+ "\n VALUES(@EventID, @HashValue)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@HashValue", HashValue);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;			// Uh, oh
			}
			PersonEventID = GetIdent(parms, "tblPersonByEvent");

			// TODO: To make Ahmed's life a little easier in the short term, we'll add the
			//		 data to the old table name of tblSwipesText. Later we'll do some
			//		 renames and change the table name here.
			string TallTableName = "tblSwipesText";		// Later, tblPersonInfoText

			// Go through the data in ParsedData.BasicData, and write out a record to the
			// tall table for each entry.
			foreach (string key in Keys) {
				SQL = "INSERT INTO " + TallTableName + "(PersonEventID, FieldName, FieldText)"
					+ "\n VALUES(@PersonEventID, @FieldName, @FieldText)";
				cmd = new SqlCommand(SQL, parms.conn);
				cmd.Parameters.AddWithValue("@PersonEventID", PersonEventID);
				cmd.Parameters.AddWithValue("@FieldName", key);
				cmd.Parameters.AddWithValue("@FieldText", ParsedData.BasicData[key]);
				n = cmd.ExecuteNonQuery();
				if (n == -1) {
					// Uh, we really must do something stronger here, such as throwing an
					// exception and rolling back the transaction on this record. But for
					// now we'll simply write a message. TODO:
					dbg("Error inserting into tblPersonInfoText({0:X}, \"{1}\", \"{2}\"",
						PersonEventID, key, ParsedData.BasicData[key]);
				}
			}

			return PersonEventID;
		}

//---------------------------------------------------------------------------------------

		private static string GetMD5String(string p) {
			MD5 md5 = MD5.Create();
			byte[] buf = Encoding.ASCII.GetBytes(p);
			byte[] MD5Bytes = md5.ComputeHash(buf);
			string s;
			s = Encoding.ASCII.GetString(MD5Bytes);
			return s;
		}

//---------------------------------------------------------------------------------------

		private static int GetIdent(DBParms parms, string tblName) {
			string SQL = "SELECT IDENT_CURRENT('" + tblName + "')";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			return GetID(cmd.ExecuteScalar());
		}

//---------------------------------------------------------------------------------------

		private static void ImportBasicData(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering ImportBasicData");
			string SQL = "INSERT INTO tblSwipesText(SwipeID, FieldName, FieldText)\n"
						+ "VALUES (@SwipeID, @FieldName, @FieldText)";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);

			foreach (string name in ParsedData.BasicData.Keys) {
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@SwipeID", parms.SwipeID);
				cmd.Parameters.AddWithValue("@FieldName", name);
				cmd.Parameters.AddWithValue("@FieldText", ParsedData.BasicData[name]);

				int n = cmd.ExecuteNonQuery();
				if (n == -1) {
					// TODO: Give some kind of error
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ImportDemographics(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering ImportDemographics");
			const int TypeDemog = 2;
			int QuesID, AnsID;
			foreach (Demographic demog in ParsedData.Demographics) {
				QuesID = GetDemogQuestionID(parms, demog.Question, TypeDemog);
				foreach (DemographicAnswer ans in demog.Answers) {
					AnsID = GetDemogAnswerID(parms, ans.Answer);
					AddResponse(parms, QuesID, AnsID);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static int GetDemogQuestionID(DBParms parms, string Question, int QuestionType) {
			dbg("Entering GetDemogQuestionID");
			string SQL = string.Format("SELECT QuesID FROM tblQuestion"
							+ " WHERE QuesStr = '{0}' AND QuesType = {1}",
							Question, QuestionType);
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			int ID = GetID(cmd.ExecuteScalar());
			if (ID != -1) {
				return ID;
			}

			SQL = "INSERT INTO tblQuestion(QuesStr, QuesType)\n"
				+ "VALUES(@QuesStr, @QuesType)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@QuesStr", Question);
			cmd.Parameters.AddWithValue("@QuesType", QuestionType);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;
			}
			cmd = new SqlCommand("SELECT IDENT_CURRENT('tblQuestion')", parms.conn);
			ID = GetID(cmd.ExecuteScalar());
			return ID;
		}

//---------------------------------------------------------------------------------------

		private static int GetDemogAnswerID(DBParms parms, string Answer) {
			dbg("Entering GetDemogAnswerID - Answer = {0}", Answer);
			const int TypeDemog = 1;			// TODO:
			string SQL = string.Format("SELECT AnsID FROM tblAnswer"
							+ " WHERE AnsStr = @AnsStr",
							Answer, TypeDemog);
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AnsStr", Answer);
			int ID = GetID(cmd.ExecuteScalar());
			if (ID != -1) {
				return ID;
			}

			SQL = "INSERT INTO tblAnswer(AnsStr)\n"
				+ "VALUES(@AnsStr)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AnsStr", Answer);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;
			}
			cmd = new SqlCommand("SELECT IDENT_CURRENT('tblAnswer')", parms.conn);
			ID = GetID(cmd.ExecuteScalar());
			return ID;
		}

//---------------------------------------------------------------------------------------

		private static void AddResponse(DBParms parms, int QuesID, int AnsID) {
			dbg("Entering AddResponse");
			string SQL = string.Format("SELECT COUNT(*) FROM tblResponse WHERE "
						+ "AcctID = {0} AND SwipeID = {1} AND QuesID = {2} AND AnsID = {3}",
						parms.AcctID, parms.SwipeID, QuesID, AnsID);
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			object count = cmd.ExecuteScalar();
			if (GetID(count) > 0) {		// Not really an ID, but GetID does what we want
				return;					// Record already there. Leave.
			}

			SQL = "INSERT INTO tblResponse (AcctID, SwipeID, QuesID, AnsID) "
				+ "VALUES(@AcctID, @SwipeID, @QuesID, @AnsID)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@SwipeID", parms.SwipeID);
			cmd.Parameters.AddWithValue("@QuesID", QuesID);
			cmd.Parameters.AddWithValue("@AnsID", AnsID);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				// TODO: Log error somehow
			}
		}

//---------------------------------------------------------------------------------------

		private static void ImportSurvey(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering ImportSurvey");
			const int TypeSurvey = 3;
			int QuesID, AnsID;
			foreach (Survey surv in ParsedData.Surveys) {
				QuesID = GetDemogQuestionID(parms, surv.Question, TypeSurvey);
				foreach (string ans in surv.Answers) {
					AnsID = GetDemogAnswerID(parms, ans);
					AddResponse(parms, QuesID, AnsID);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ImportSessions(SqlConnection conn, ParsedSwipe ParsedData) {
			throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		private static int SetConfigXML_RunOnceOnly(SqlConnection conn) {
			string SQL;
			SqlCommand cmd;

			SQL = "DELETE FROM ImporterConfigData";
			cmd = new SqlCommand(SQL, conn);
			cmd.ExecuteNonQuery();

			SQL = "INSERT INTO ImporterConfigData (ImporterConfigFieldDefs) Values(@Data)";
			cmd = new SqlCommand(SQL, conn);
			string s = @"<?xml version=""1.0""?>
<LeadsLightning>
	<ImportFieldDefs>
		<Field Name=""Card #""				    Type=""Numeric""				>1</Field>
		<Field Name=""Name""				    Type=""Text""					>2</Field>
		<Field Name=""Title""				    Type=""Text""					>3</Field>
		<Field Name=""Company""				    Type=""Text""					>4</Field>
		<Field Name=""Street""				    Type=""Text""					>5</Field>
		<Field Name=""City""					Type=""Text""					>6</Field>
		<Field Name=""State""					Type=""Text""					>7</Field>
		<Field Name=""Zip""					    Type=""Text""					>8</Field>
		<Field Name=""Phone""					Type=""Phone""					>9</Field>
		<Field Name=""Phone 2""				    Type=""Phone""					>10</Field>
		<Field Name=""Date and Time""			Type=""SwipeTimeStamp""			>11</Field>
		<Field Name=""Services""				Type=""Services""				>12</Field>
		<Field Name=""Fax""						Type=""Text""					>13</Field>
		<Field Name=""First Name""			    Type=""Text""					>14</Field>
		<Field Name=""Last Name""				Type=""Text""					>15</Field>
		<Field Name=""Country""					Type=""Text""					>16</Field>
		<Field Name=""Demographics""			Type=""Demographics""			>17</Field>
		<Field Name=""Notes""					Type=""Text""					>18</Field>
		<Field Name=""TerminalID""			    Type=""Text""					>19</Field>
		<Field Name=""Sessions""			    Type=""Access_Control""			>500</Field>
		<Field Name=""eMail""				    Type=""Text""					>501</Field>
		<Field Name=""Birth Date""			    Type=""Date""					>502</Field>
		<Field Name=""Veteran""				    Type=""Text""					>503</Field>
		<Field Name=""Public Assistance""		Type=""Text""					>504</Field>
		<Field Name=""Swipe State""				Type=""Text""					>505</Field>
		<Field Name=""SSN""					    Type=""Text""					>506</Field>
		<Field Name=""Middle Name""			    Type=""Text""					>507</Field>
		<Field Name=""Apt No""				    Type=""Text""					>508</Field>
		<Field Name=""Creation Date""			Type=""Date""					>509</Field>
		<Field Name=""County""					Type=""Text""					>510</Field>
		<Field Name=""Home Site ID""			Type=""Text""					>511</Field>
		<Field Name=""Card Version #""			Type=""Text""					>512</Field>
		<Field Name=""Survey""				    Type=""Survey""					>700</Field>
		<Field Name=""Service Provider""		Type=""Referrals_Provider""		>701</Field>
		<Field Name=""Referral From Agency""	Type=""Referrals_From""			>702</Field>
		<Field Name=""Referral To Agency""      Type=""Referrals_To""			>703</Field>
	</ImportFieldDefs>
</LeadsLightning>";
			cmd.Parameters.AddWithValue("@Data", s);

			int n = cmd.ExecuteNonQuery();
			return n;
		}

//---------------------------------------------------------------------------------------

		private static string GetFirmwareFields(SqlConnection conn) {
			SqlCommand cmd = new SqlCommand("SELECT ImporterConfigFieldDefs FROM ImporterConfigData", conn);
			string s = null;
			try {
				s = (string)cmd.ExecuteScalar();
			} catch (Exception) {
				// Until I get some kind of error logging, I'm just going to ignore
				// this execption. TODO:
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		protected static void ThrowImporterException(string fmt, params object[] args) {
			throw new Exception(string.Format(fmt, args));
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		// Nested type
		/// <summary>
		/// All the tables, etc we need to work with the database while importing. Also the
		/// auxiliary info, such as AcctID and EventID.
		/// </summary>
		internal class DBParms {
			internal SqlConnection conn;

			internal int AcctID;
			internal int EventID;
			internal int PersonEventID;
			internal int SwipeID;
			internal int TermID;

#if false
		internal DataTable		tblSwipes, tblSwipesText;
		internal DataTable		tblTerminals;

		internal DataTable		tblResponse;
		internal DataTable		tblQuestion, tblAnswer, tblQuestionAnswer;
#endif

//---------------------------------------------------------------------------------------

			internal DBParms(SqlConnection conn, int AcctID, int EventID) {
				this.conn = conn;

				this.AcctID = AcctID;
				this.EventID = EventID;

				SwipeID = -1;
				TermID = -1;
			}

//---------------------------------------------------------------------------------------
		}
	}
}
