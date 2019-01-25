// Copyright (c) 2006 by Bartizan Connects, LLC

This code is now obsolescent, hence this deliberate syntax error.
Use the LLWS_Importer solution instead.

// TODO:
//	*	GetAcctID needs password support
//	*	[FILE] concatenated fields
//	*	Worry about tblStrings later
//	*	Put back Transactions
//	*	Need to return, ideally, vector of structures, one per swipe, which tells the
//		status of importing the swipe. For example, if the swipe was already there, if
//		there was an exception thrown (and its diagnostic text) and so on.
//	*	In the short term, return an enum (or for the nonce, an int) that says whether
//		the import went OK or not.
//	*	Need support for Expanded (or not) Demog/Followups/etc.
//	*	Parser doesn't support multiple demographics (AbbA11B1b...). Also, the current
//		syntax for expanded multiple demographics (i.e. with embedded + signs) is
//		ambiguous. For example, Q: "What sofware do you use?", A: "A+", "Word", etc.
//	*	We should define another parm to the Importer, which says whether this is a
//		replacement swipe or not.

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

// #define	DBG				// If defined, pump out dbg(...) messages

// Define the following, uh, #define to configure this to call the
// Importer directly, rather than as an sproc.
#define		CALL_SQL_DIRECTLY

// Make sure we've chosen something above
#if LOCAL
			// Do nothing
#elif DEVEL
#if false
#endif
#elif PROD
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif

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

	// TODO: Should be in SourceSafe

	public class ImportRecordStatus {
		public int		RecNo;			// Caller's ID of the input record, from first
										//	 field on swipe record.
		public ImportRecordStatusErrCode	ErrCode;
		public string	ErrMsg;			// The text of any error message to be displayed
										//	 to the user as additional diagnostic info.
										//	 This field should be ignored if ErrCode
										//	 shows no error.

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------

		public enum ImportRecordStatusErrCode {
			// TODO: For now, we have only two codes. Presumably later we'll be able
			//		 to break down the error a bit more finely (e.g. database physical
			//		 error, database logical error, can't parse the swipe record, etc)
			OK,
			Problem
		}
	}

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
		/// MapCfgFileContents parameter. Is it the contents of the file itself? Is it the name of a
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
		private const int MapTypeKey  = 2;

		// TODO: Get rid of Pipe.Send - uh, not quite. They're the (rough) equivalent of
		//		 Debug.Write. One note, though. While the messages sent this way show up in
		//		 the VS Output|Debug window, they clearly are not the result of calling
		//		 OutputDebugString, and do *not* show up in, say, DbgView from Sysinternals.
		//		 Which is, I suppose, reasonable, since you can "return" a Rowset this way.
		//		 Note: I finally figured out what's going on here. Any Pipe.Send's show up
		//		 as InfoMessage events of the SQLConnection class. e.g.
		//		 conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);


#if CALL_SQL_DIRECTLY
		StringBuilder	ReturnMessages = new StringBuilder();
		int				ErrorCode = 0;
#endif

//---------------------------------------------------------------------------------------

		[Conditional("DBG")]
		public void dbg(string fmt, params object[] vals) {
			string s = string.Format(fmt, vals);
#if CALL_SQL_DIRECTLY
			ReturnMessages.AppendFormat("\n{0}", s); 
#else
			SqlContext.Pipe.Send(s);
#endif
		}

//---------------------------------------------------------------------------------------

		public void Err(string fmt, params object[] vals) {
			ErrorCode = -1;
			string s = string.Format("Error - " + fmt, vals);
#if CALL_SQL_DIRECTLY
			ReturnMessages.AppendFormat("\n{0}", s);
#else
			SqlContext.Pipe.Send(s);
#endif
		}


//---------------------------------------------------------------------------------------

#if CALL_SQL_DIRECTLY
		public string LL_sp_LLImport(string UserID, string Password, int EventID,
						string	SwipeData,
						string	MapCfgFile,
						int		MapType, 
						string	TerminalID,
						bool	IgnoreFirstRecord,
						bool	DataIsExpanded,
						out int	ErrorCode) {
			ErrorCode = 0;
			try {
				Process(UserID, Password, EventID, SwipeData, MapCfgFile, MapType, TerminalID, IgnoreFirstRecord, DataIsExpanded);
			} catch (Exception ex) {
				// TODO: This is hardly enough information. Get at least the record 
				//		 number
				Err("***** Unexpected error in LL_sp_LLImport - {0}", ex.Message);
			}
			ReturnMessages.Append("\nEnd of import");
			ErrorCode = this.ErrorCode;
			return ReturnMessages.ToString();
		}
#else
	[Microsoft.SqlServer.Server.SqlProcedure]
	// public static int LL_sp_LLImport(string Swipes, string MapCfgFile, int MapType, string TerminalID, ref string Results) {
	public static void LL_sp_LLImport(string UserID, string Password, int EventID,
				[Microsoft.SqlServer.Server.SqlFacet(MaxSize = -1)]	// nvarchar(max)
					string SwipeData,
				[Microsoft.SqlServer.Server.SqlFacet(MaxSize = -1)]	// nvarchar(max)
					string MapCfgFile,
					int MapType, string TerminalID,
					bool	IgnoreFirstRecord) {

		try {
			Process(UserID, Password, EventID, SwipeData, MapCfgFile, MapType, TerminalID, IgnoreFirstRecord);
		} catch (Exception ex) {
			// TODO: This is hardly enough information. Get at least the record number
			Err("***** Unexpected error in LL_sp_LLImport - {0}", ex.Message);
		}
	}
#endif

//---------------------------------------------------------------------------------------

		private void Process(string UserID, string Password, int EventID, 
					string SwipeData, string MapCfgFile, int MapType, 
					string TerminalID, bool IgnoreFirstRecord, bool DataIsExpanded) {
			string MapContents = null;

			dbg("Entered LL_sp_LLImport LLWS1");

			using (SqlConnection conn = new SqlConnection()) {
				SetConnectionString(conn);
				conn.Open();

#if false
			SetConfigXML_RunOnceOnly(conn);
			dbg("Config XML Set");		
#endif

				// Yeah, MapType should be an enum. But since we don't have SourceSafe
				// yet, it's a nuisance to manually create and maintain multiple copies
				// of such a file. So we'll take a short cut here. Hopefully later (but
				// before we ship), this can be fixed. TODO:
				// dbg("About to process MapType = {0}", MapType);
				switch (MapType) {
				case MapTypeText:
					MapContents = MapCfgFile;
					if (MapContents == null) {
						Err("Error - No Map.CFG file provided (Diagnostic info - MapCfgFileContents=null for MapType={0}", MapType);
						return;
					}
					break;
				case MapTypeKey:
					// dbg("About to GetMapFromDB - Key={0}", MapCfgFile);
					MapContents = GetMapFromDB(conn, MapCfgFile);
					if (MapContents == null) {
						Err("Error - Unable to find Map.CFG file for ID {0} ", MapType);
						return;
					}
					// dbg("Got map file - Contents = {0}", MapContents);
					break;
				default:
					Err("Internal error - MapType of {0} not supported.", MapType);
					return;
				}

				// dbg("About to init MemoryStream");
				MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(MapContents), false);
				// dbg("About to init BartIniFile");
				BartIniFile suf = new BartIniFile(ms);
				// dbg("About to init MapCfg");
				MapCfg cfg = new MapCfg(suf, TerminalID);

				// dbg("About to echo SwipeData");
				string[] Swipes = SwipeData.Split(new string[] { "\n" }, 
					StringSplitOptions.RemoveEmptyEntries);
				dbg("Number of swipes = {0}", Swipes.Length);
#if false
				int nRecs = 0;
				foreach (string s in Swipes) {
					dbg("{0}: {1}", ++nRecs, s);
				}
				dbg("Finished echoing SwipeData");
#endif

				// dbg("About to GetFirmwareFields");
				string xmlFirmwareFields = GetFirmwareFields(conn);
				// dbg("Got Firmware fields - {0}", xmlFirmwareFields);
				if (xmlFirmwareFields == null) {
					Err("Unable to load XML Firmware field definitions");
					return;
				}
				// dbg("About to create ParsedSwipe");
				ParsedSwipe ParsedData = new ParsedSwipe(xmlFirmwareFields, cfg, DataIsExpanded);
				// dbgDumpParsedData(ParsedData);

				int AcctID;

				// dbg("About to CheckEventID");
				bool bEventIDOK = CheckEventID(conn, EventID);
				if (!bEventIDOK) {
					Err("Event not found. Diagnostic info - EventID = {0:X}", EventID);
					return;
				}

				// dbg("About to GetAcctID");
				AcctID = GetAcctID(conn, UserID, Password);

				// dbg("About to create DBParms");
				DBParms dbparms = new DBParms(conn, AcctID, EventID);

				string			TrimmedSwipe;
				int				RecNo = 0;
				// SqlTransaction	tx = null;
				foreach (string swipe in Swipes) {
					++RecNo;
					if (IgnoreFirstRecord && (RecNo == 1)) {
						continue;
					}
#if false
					dbg("******* Warning. Import limited to 50 records");
if (RecNo > 50) {		// TODO: TODO: TODO:
	break;
}
#endif
					dbg("Echo swipe record {0} = {1}", RecNo, swipe);
					// I've seen data come in with blank lines, sometimes consisting only
					// of tab character(s). Check for that case and bypass processing
					// if so.
					TrimmedSwipe = swipe.Trim();
					if (TrimmedSwipe.Length == 0) {
						continue;
					}
					try {
						// tx = conn.BeginTransaction();
						// dbg("About to ParsedData.GetFields for RecNo {0}", RecNo);
						ParsedData.GetFields(TrimmedSwipe);
#if DEBUG || ! DEBUG
						// dbgDumpParsedData(ParsedData);
#endif
						dbg("About to ImportAll");
						ImportAll(dbparms, ParsedData);
						// TODO: Check if there's any way we can get here if the x'n 
						//		 failed.
						// tx.Commit();
					} catch (Exception ex) {
						Err("Error - Swipe record {0} could not be processed."
							+ " Skipping to next swipe. Input record = {1}"
							+ "\nDiagnostic info = '{2}'"
							+ "\n\nTraceback = {3}",
							RecNo, TrimmedSwipe, ex.Message, ex.StackTrace);
						// dbg("TrimmedSwipe = {0}", TrimmedSwipe);
						// dbg("End of TrimmedSwipe");
						// tx.Rollback();
					}
				}
			}

			dbg("About to return from LL_sp_LLImport");
			return;
		}

//---------------------------------------------------------------------------------------

		[Conditional("DBG")]
		private void dbgDumpParsedData(ParsedSwipe ParsedData) {
			dbg("dbgDumpSwipe - FieldDefs.Count = {0}, BasicData.Count = {1}",
			ParsedData.FieldDefs.Count, ParsedData.BasicData.Count);
			foreach (string FieldName in ParsedData.FieldDefs.Keys) {
				dbg("dbgDumpSwipe - FieldDefs['{0}'] = {1}", FieldName, ParsedData.FieldDefs[FieldName]);
			}
			foreach (string BasicKey in ParsedData.BasicData.Keys) {
				dbg("dbgDumpSwipe - BasicData['{0}'] = {1}", BasicKey, ParsedData.BasicData[BasicKey]);
			}
			dbg("dbgDumpSwipe - End");
			ParsedData.dbgDumpSwipe();		
			foreach (string str in ParsedData.LogMsgs) {
				dbg("ParsedData Log message - {0}", str);
			}
		}

//---------------------------------------------------------------------------------------

		private static string GetMapFromDB(SqlConnection conn, string MapCfgID) {
			string	MapCfg = null;		// Default return value if error
			int		ID;
			bool	bOK = int.TryParse(MapCfgID.Trim(), out ID);
			if (! bOK) {
				return null;
			}
			string SQL = "SELECT MapCfgContents FROM tblMapCfg WHERE MapCfgID = @MapCfgID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@MapCfgID", ID);
			object result = cmd.ExecuteScalar();
			if (result == DBNull.Value) {
				return MapCfg;
			}
			MapCfg = (string)result;
			return MapCfg;
		}

//---------------------------------------------------------------------------------------

		private static int GetAcctID(SqlConnection conn, string UserID, string Password) {
			// TODO: For now, just check that the UserID is there. Don't validate 
			//		 password.
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

		private void ImportAll(DBParms parms, ParsedSwipe ParsedData) {
			// dbg("Entering ImportAll");
			bool bOK;

			int PersonEventID = GetPersonEventID(parms, ParsedData);
			parms.PersonEventID = PersonEventID;
			int SwipeID = GetSwipeID(parms, ParsedData);
			if (SwipeID != -1) {		// See if record already there
				parms.SwipeID = SwipeID;
				HandleReimports(parms, ParsedData);
				return;
			}

			bOK = CreateMasterSwipeRecord(parms, ParsedData, out SwipeID);
			if (! bOK)
				return;
			parms.SwipeID = SwipeID;
			// ImportBasicData(parms, ParsedData);
			ImportDemographics(parms, ParsedData);
			ImportServices(parms, ParsedData);
#if false
			ImportSurvey(parms, ParsedData);
			ImportSessions(parms, ParsedData);			// TODO:
#endif
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Zhe threw me a curve ball today. He told me that the RealTimer will send at
		/// least two records ***with the same timestamp***, if the user scans in a 
		/// Service. What we probably need is an input parameter to the importer to say
		/// that this record is a replacement for a previous swipe. But in the meantime,
		/// just because we know about this swipe, doesn't mean that we can assume that 
		/// its Services, Note, and who knows what else, haven't changed. So in the short
		/// term, delete all Service records for this swipe (and later, Notes, etc) and
		/// import them again. Quelle waste of time! TODO:
		/// </summary>
		/// <param name="parms"></param>
		/// <param name="ParsedData"></param>
		private void HandleReimports(DBParms parms, ParsedSwipe ParsedData) {
			// First, delete our previous Services
			string	SQL = "DELETE FROM tblResponse"
						+ " WHERE SwipeID = @SwipeID";
			SqlCommand	cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@SwipeID", parms.SwipeID);
			int	nRows = cmd.ExecuteNonQuery();
			dbg("HandleReimports - deleted {0} record(s) from tblResponse", nRows);

			// Now reimport them. Sigh.
			ImportDemographics(parms, ParsedData);
			ImportServices(parms, ParsedData);
			// TODO: Handle anything else (e.g. Notes) that needs reloading
		}

//---------------------------------------------------------------------------------------

		private int GetSwipeID(DBParms parms, ParsedSwipe ParsedData) {
			// dbg("Entering GetSwipeID");
			int TermID = GetTerminalID(parms, ParsedData);
			parms.TermID = TermID;
			if (TermID == -1) {
				return -1;			// TODO: Better error handling
			}
			// dbg("In GetSwipeID, got TerminalID");
			// dbg("ParsedData.SwipeTimestamp = {0}", ParsedData.SwipeTimestamp);
			string SQL = "SELECT SwipeID FROM tblSwipes WHERE "
						+ "AcctID = @AcctID AND EventID = @EventID AND TerminalID = @TerminalID "
						+ "AND SwipeDate = @SwipeDate "
						+ "AND PersonEventID = @PersonEventID";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@TerminalID", TermID);
			cmd.Parameters.AddWithValue("@SwipeDate", ParsedData.SwipeTimestamp);
			cmd.Parameters.AddWithValue("@PersonEventID", parms.PersonEventID);
			int SwipeID = GetID(cmd.ExecuteScalar());
			// At this point, SwipeID could either be a legitimate ID, or else -1. If the
			// latter, we've probably had some kind of error, and we'll just return the
			// -1 that GetID returns to indicate a problem.
			return SwipeID;
		}

//---------------------------------------------------------------------------------------

		private int GetTerminalID(DBParms parms, ParsedSwipe ParsedData) {
			// dbg("Entering GetTerminalID");
			SqlConnection conn = parms.conn;
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

			return GetIdent(parms.conn, "tblTerminal");
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

		private bool CreateMasterSwipeRecord(DBParms parms, ParsedSwipe ParsedData, out int SwipeID) {
			// dbg("Entering CreateMasterSwipeRecord");
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

			SwipeID = GetIdent(parms.conn, "tblSwipes");
			return true;
		}

//---------------------------------------------------------------------------------------

		private int GetPersonEventID(DBParms parms, ParsedSwipe ParsedData) {
			// Glue all basic fields (i.e. non-demographics, but including custom fields)
			// together. But be careful. If the .Net framework ever changes so that the
			// order of the fields retrieved from the BasicData Dictionary<string, 
			// string> ever changes, the lookup process is screwed. So sort the keys
			// first, to ensure that we're always looking up consistent data.

			// dbg("Entering GetPersonEventID");
			Dictionary<string, string>.KeyCollection KeyColl = ParsedData.BasicData.Keys;
			string[] Keys = new string[KeyColl.Count];
			KeyColl.CopyTo(Keys, 0);
			Array.Sort(Keys);
			StringBuilder sb = new StringBuilder();
			// string sep = "¶";		// char.ConvertFromUtf32(0xB6);
			string sep = "+";		
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

			// OK, we didn't find the person. Too bad. So add it to the table, and also
			// add the person's information to tblPersonInfoText;

			// First, add the data to tblPersonByEvent, and get the PersonEventID 
			// Autonum.
			SQL = "INSERT INTO tblPersonByEvent(EventID, HashValue)"
				+ "\n VALUES(@EventID, @HashValue)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@HashValue", HashValue);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;			// Uh, oh - TODO:
			}
			PersonEventID = GetIdent(parms.conn, "tblPersonByEvent");
			// dbg("Got new PersonEventID = {0}", PersonEventID);

			// TODO: To make Ahmed's life a little easier in the short term, we'll add
			//		 the data to the old table name of tblSwipesText. Later we'll do some
			//		 renames and change the table name here.
			string TallTableName = "tblSwipesText";		// Later, tblPersonInfoText

			dbg("About to insert into {0}, record count = {1}", TallTableName, Keys.Length);
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
					Err("Error inserting into tblPersonInfoText({0}, \"{1}\", \"{2}\"",
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

		public static int GetIdent(SqlConnection conn, string tblName) {
			string SQL = "SELECT IDENT_CURRENT('" + tblName + "')";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			return GetID(cmd.ExecuteScalar());
		}

//---------------------------------------------------------------------------------------

		private void ImportBasicData(DBParms parms, ParsedSwipe ParsedData) {
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

		private void ImportServices(DBParms parms, ParsedSwipe ParsedData) {
			// Import Services, aka Import Questions, aka Import FollowUps
			// dbg("Entering ImportServices");
			int		ID = GetQsAndAsByTerminal_ID(parms);
			if (ID == -1) {
				Err("Could not find QsAndAsByTerminal_ID");	// TODO: Better error
				return;
			}
			const int TypeFollowUp = 1;
			int		QuesID;
			int		AnsIDYes = GetDemogAnswerID(parms, "Yes");
			// int		AnsIDNo  = GetDemogAnswerID(parms, "No");
			foreach (Service service in ParsedData.Services) {
				QuesID = GetQuestionID(parms, service.service, TypeFollowUp);
				AddResponse(parms, QuesID, AnsIDYes);
				AddQuestionAnswer(parms, ID, QuesID, AnsIDYes);
			}
		}

//---------------------------------------------------------------------------------------

		private void ImportDemographics(DBParms parms, ParsedSwipe ParsedData) {
			// dbg("Entering ImportDemographics");
			int		ID = GetQsAndAsByTerminal_ID(parms);
			if (ID == -1) {
				Err("Could not find QsAndAsByTerminal ID");	// TODO: Better error
				return;
			}
			const int TypeDemog = 2;
			int QuesID, AnsID;
			// dbg("Number of Demographics = {0}", ParsedData.Demographics.Count);
			foreach (Demographic demog in ParsedData.Demographics) {
				// dbg("Getting QID for {0}", demog.Question);
				QuesID = GetQuestionID(parms, demog.Question, TypeDemog);
				// dbg("Got QID for {0} = {1}", demog.Question, QuesID);
				foreach (DemographicAnswer ans in demog.Answers) {
					// dbg("Getting AID for {0}", ans.Answer);
					AnsID = GetDemogAnswerID(parms, ans.Answer);
					// dbg("Got AID for {0} = {1}", ans.Answer, AnsID);
					AddResponse(parms, QuesID, AnsID);
					AddQuestionAnswer(parms, ID, QuesID, AnsID);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private int GetQuestionID(DBParms parms, string Question, int QuestionType) {
			// dbg("Entering GetQuestionID");
			string SQL  = "SELECT QuesID FROM tblQuestion"
						+ " WHERE QuesStr = @QuesStr AND QuesType = @QuesType";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@QuesStr", Question);
			cmd.Parameters.AddWithValue("@QuesType", QuestionType);
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

		private int GetQsAndAsByTerminal_ID(DBParms parms) {
			// dbg("Entering GetQsAndAsByTerminal_ID");
			string SQL = "SELECT ID FROM tblQsAndAsByTerminal"
						+ " WHERE AcctID=@AcctID"
						+ "   AND EventID=@EventID"
						+ "   AND TerminalID=@TerminalID";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@TerminalID", parms.TermID);
			int ID = GetID(cmd.ExecuteScalar());
			if (ID != -1) {
				return ID;
			}

			SQL = "INSERT INTO tblQsAndAsByTerminal(AcctID, EventID, TerminalID)\n"
				+ "VALUES(@AcctID, @EventID, @TerminalID)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@EventID", parms.EventID);
			cmd.Parameters.AddWithValue("@TerminalID", parms.TermID);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				return -1;
			}
			cmd = new SqlCommand("SELECT IDENT_CURRENT('tblQsAndAsByTerminal')", parms.conn);
			ID = GetID(cmd.ExecuteScalar());
			return ID;
		}

//---------------------------------------------------------------------------------------

		private int GetDemogAnswerID(DBParms parms, string Answer) {
			// dbg("Entering GetDemogAnswerID - Answer = {0}", Answer);
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

		private void AddResponse(DBParms parms, int QuesID, int AnsID) {
			// dbg("Entering AddResponse");
			string SQL = "SELECT COUNT(*) FROM tblResponse WHERE "
						+ "AcctID = @AcctID AND SwipeID = @SwipeID "
						+ "AND QuesID = @QuesID AND AnsID = @AnsID";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@AcctID", parms.AcctID);
			cmd.Parameters.AddWithValue("@SwipeID", parms.SwipeID);
			cmd.Parameters.AddWithValue("@QuesID", QuesID);
			cmd.Parameters.AddWithValue("@AnsID", AnsID);
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

		private void AddQuestionAnswer(DBParms parms, int ID, int QuesID, int AnsID) {
			// dbg("Entering AddQuestionAnswer");
			string SQL = "SELECT COUNT(*) FROM tblQuestionAnswer WHERE"
						+ " AcctID_EventID_TermID = @ID"
						+ " AND QuesID = @QuesID AND AnsID = @AnsID";
			SqlCommand cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.Parameters.AddWithValue("@QuesID", QuesID);
			cmd.Parameters.AddWithValue("@AnsID", AnsID);
			object count = cmd.ExecuteScalar();
			if (GetID(count) > 0) {		
				return;					// Record already there. Leave.
			}

			// dbg("About to add to tblQuestionAnswer");
			SQL = "INSERT INTO tblQuestionAnswer (AcctID_EventID_TermID, QuesID, AnsID) "
				+ "VALUES(@ID, @QuesID, @AnsID)";
			cmd = new SqlCommand(SQL, parms.conn);
			cmd.Parameters.AddWithValue("@ID", ID);
			cmd.Parameters.AddWithValue("@QuesID", QuesID);
			cmd.Parameters.AddWithValue("@AnsID", AnsID);
			int n = cmd.ExecuteNonQuery();
			if (n == -1) {
				// TODO: Log error somehow
			}
		}

//---------------------------------------------------------------------------------------

		private void ImportSurvey(DBParms parms, ParsedSwipe ParsedData) {
			dbg("Entering ImportSurvey");
			const int TypeSurvey = 3;
			int QuesID, AnsID;
			foreach (Survey surv in ParsedData.Surveys) {
				QuesID = GetQuestionID(parms, surv.Question, TypeSurvey);
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

		private int SetConfigXML_RunOnceOnly(SqlConnection conn) {
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

		private void SetConnectionString(SqlConnection conn) {
#if CALL_SQL_DIRECTLY
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			// string prefix;
#if LOCAL
			builder.DataSource = "(local)";
			builder.IntegratedSecurity = true;
			builder.InitialCatalog = "LLDevel";
			// prefix = "";
			conn.ConnectionString = builder.ConnectionString;
#elif DEVEL
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			// prefix = "ahmed.";
			conn.ConnectionString = builder.ConnectionString;
#elif PROD
#warning You really should use DEVEL or LOCAL
			builder.DataSource = "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			// prefix = "ahmed.";
			conn.ConnectionString = builder.ConnectionString;
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif
#else
			conn.ConnectionString = "context connection=true";
#endif
		}



//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
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

//---------------------------------------------------------------------------------------

			internal DBParms(SqlConnection conn, int AcctID, int EventID) {
				this.conn = conn;

				this.AcctID = AcctID;
				this.EventID = EventID;

				SwipeID = -1;
				TermID = -1;
			}
		}
	}
}
