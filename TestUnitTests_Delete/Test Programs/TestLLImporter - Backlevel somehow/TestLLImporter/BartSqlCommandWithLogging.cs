// Copyright (c) 2006 by Bartizan Connects, LLC

// TODO: Doesn't handle DBNull's perfectly

// Uncomment next line to show database statistics
#define	SHOWSTATS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Bartizan.Utils.Database {

	/// <summary>
	/// This class is fairly specialized. It's a pseudo-replacement for the SqlCommand
	/// class. (We would have simply derived from SqlCommand, but it's sealed.)
	/// <para>
	/// Its purpose is to capture the SQL statements (and Parameters) used in an
	/// application. These can then be written out to a file, a table, etc.
	/// </para>
	/// <para>
	/// We support only that specific subset of SqlCommand that we need at any one time.
	/// For example, since we don't initially need BeginExecuteNonQuery(), it's not one
	/// of our methods here. OTOH, if we ever do need it, we can add it easily.
	/// </para>
	/// <para>
	/// The data is logged to a static field, since the code may instantiate multiple
	/// SqlCommand (uh, I mean BartSqlCommandWithLogging) objects, but we want to be able
	/// to capture an entire sequence of database operations.
	/// </para>
	/// <para>
	/// This class was originally written to be able to capture the database requests
	/// used in the LeadsLightning importer, so that we can use it as a workload file
	/// for the SQL Server Database Engine Tuning Advisor. In particular, we need it
	/// to find places where we need indexed placed on the database.
	/// </para>
	/// <para>
	/// Note: The GetSProc routine makes a tacit assumption. If it's already seen a
	/// parameter named, say, @AcctID, it will not pump out another DECLARE statement
	/// for it (but it will pump out a comment). However, if the same parameter name is
	/// used in more than one query, and the datatype is different, we've probably got
	/// a problem. The solution is just to change your query to use a different parameter
	/// name and try again.
	/// </para>
	/// </summary>
	public class BartSqlCommandWithLogging {
		static bool _IsLogging = true;
		static List<BartSqlLogRecord> LogRecords = new List<BartSqlLogRecord>();

		SqlCommand cmd;
		string CurrentComment;

		// Because of possible concurrency issues, when we call AddComment, we can't just
		// take LogRecords[LogRecords.Count - 1] as the current record. So we'll keep the
		// reference here.
		BartSqlLogRecord CurrentRec;

		static Hashtable PreviousConnectionStats = null;

		//---------------------------------------------------------------------------------------

		public SqlParameterCollection Parameters {
			get { return cmd.Parameters; }
		}

		//---------------------------------------------------------------------------------------

		public static bool IsLogging {
			get { return _IsLogging; }
			set { _IsLogging = value; }
		}

		//---------------------------------------------------------------------------------------

		public BartSqlCommandWithLogging(string SQL, SqlConnection conn) {
			string Comment = "*N/A*";
			// We'll default the Comment to the name of the method that calls us.
			// But we'll do it in a try/catch in case we don't have permission to
			// see the StackTrace. Sigh.
			try {
				StackTrace st = new StackTrace(1);
				StackFrame sf = st.GetFrame(0);
				Comment = string.Format("{0}", sf.GetMethod().Name);
			} catch {
				// Comment already set. Just leave it.
			}
			CommonCtorProcessing(SQL, conn, Comment);
		}

		//---------------------------------------------------------------------------------------

		public BartSqlCommandWithLogging(string SQL, SqlConnection conn, string Comment) {
			CommonCtorProcessing(SQL, conn, Comment);
		}

		//---------------------------------------------------------------------------------------

		private void CommonCtorProcessing(string SQL, SqlConnection conn, string Comment) {
			CurrentComment = Comment;
			cmd = new SqlCommand(SQL, conn);
			CurrentRec = null;
			if (PreviousConnectionStats == null) {
				PreviousConnectionStats = (Hashtable)conn.RetrieveStatistics();
			}
		}

		//---------------------------------------------------------------------------------------

		public object ExecuteScalar() {
			BartSqlLogRecord rec = GenNewRecord();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			object o = cmd.ExecuteScalar();
			sw.Stop();
			string msg = string.Format("ExecuteScalar - Result={0}, Elapsed={1}", o, sw.Elapsed);
			msg += ShowStats();
			LogIt(rec, msg);
			return o;
		}

		//---------------------------------------------------------------------------------------

		private string ShowStats() {
#if SHOWSTATS
			if (!cmd.Connection.StatisticsEnabled) {
				return "";
			}
			StringBuilder sb = new StringBuilder();
			Hashtable stats = (Hashtable)cmd.Connection.RetrieveStatistics();
			try {
				sb.AppendFormat("\nShowStats");
				foreach (string key in stats.Keys) {
					string msg = string.Format("\n\tStats['{0}'] = {1:N0}",
						key, stats[key]);
					msg = msg.PadRight(45);		// Arbitrary, but enough for now
					sb.Append(msg);
					sb.Append(string.Format("Delta = {0:N0}",
						(long)stats[key] - (long)PreviousConnectionStats[key]));
				}
			} catch (Exception ex) {
				sb.AppendFormat("\nUnexpected exception in ShowStats - {0}", ex);
			}
			PreviousConnectionStats = stats;
			return sb.ToString();
#else
			return "";
#endif
		}

		//---------------------------------------------------------------------------------------

		public int ExecuteNonQuery() {
			BartSqlLogRecord rec = GenNewRecord();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			int result = cmd.ExecuteNonQuery();
			sw.Stop();
			string msg = string.Format("ExecuteNonQuery - Result={0}, Elapsed={1}", result, sw.Elapsed);
			msg += ShowStats();
			LogIt(rec, msg);
			return result;
		}

		//---------------------------------------------------------------------------------------

		public SqlDataReader ExecuteReader() {
			BartSqlLogRecord rec = GenNewRecord();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			SqlDataReader rdr = cmd.ExecuteReader();
			sw.Stop();
			string msg = string.Format("ExecuteReader - Records Affected={0}, Elapsed={1}",
				rdr.RecordsAffected, sw.Elapsed);
			msg += ShowStats();
			LogIt(rec, msg);
			return rdr;
		}

		//---------------------------------------------------------------------------------------

		public SqlDataReader ExecuteReader(CommandBehavior cb) {
			BartSqlLogRecord rec = GenNewRecord();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			SqlDataReader rdr = cmd.ExecuteReader(cb);
			sw.Stop();
			string msg = string.Format("ExecuteReader({0}) - Records Affected={1}, Elapsed={2}",
				cb, rdr.RecordsAffected, sw.Elapsed);
			msg += ShowStats();
			LogIt(rec, msg);
			return rdr;
		}

		//---------------------------------------------------------------------------------------

		/// <summary>
		/// This routine gets the SQL command text and any parameters, and logs them to
		/// a static field. Later they can be retrieved and the caller can write them to
		/// a file, to a table, etc.
		/// </summary>
		private void LogIt() {
			if (!IsLogging)
				return;
			string comment = string.Format("{0} {1}", DateTime.Now, CurrentComment);
			CurrentRec = new BartSqlLogRecord(cmd, comment);
			AddLogRec(CurrentRec);
		}

		//---------------------------------------------------------------------------------------

		private void LogIt(BartSqlLogRecord rec, string comment) {
			if (!IsLogging)
				return;
			// For this form of the method, append the comment *after* the SQL
			rec.PostComment += string.Format("{0} {1}", DateTime.Now, comment);
			AddLogRec(rec);
		}

		//---------------------------------------------------------------------------------------

		private BartSqlLogRecord GenNewRecord() {
			CurrentRec = new BartSqlLogRecord(cmd, CurrentComment);
			return CurrentRec;
		}

		//---------------------------------------------------------------------------------------

		private static void AddLogRec(BartSqlLogRecord rec) {
			// Because LogRecords is static, there may be race conditions. Maybe. I'm not
			// at all sure just *exactly* how IIS re-uses threads, classes, etc. So to
			// avoid multiple threads trying to (possibly) simultaneously update the 
			// LogRecords static field, lock it.
			lock (LogRecords) {
				LogRecords.Add(rec);
			}
		}

		//---------------------------------------------------------------------------------------

		public static void NewComment(string Comment) {
			string comment = string.Format("{0} {1}", DateTime.Now, Comment);
			BartSqlLogRecord rec = new BartSqlLogRecord(null, comment);
			AddLogRec(rec);
		}

		//---------------------------------------------------------------------------------------

		public static void NewComment(string fmt, params object[] parms) {
			NewComment(string.Format(fmt, parms));
		}

		//---------------------------------------------------------------------------------------

		public List<BartSqlLogRecord> GetLogRecords() {
			return LogRecords;
		}

		//---------------------------------------------------------------------------------------

		public static void Clear() {
			// See comments in AddLogRec
			lock (LogRecords) {
				LogRecords.Clear();
			}
		}

		//---------------------------------------------------------------------------------------

		public static string GetSProc() {
			Dictionary<string, int> ht = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
			int LineNo = 1;
			StringBuilder sb = new StringBuilder();
			foreach (BartSqlLogRecord logrec in LogRecords) {
				sb.Append(logrec.ToSproc(ht, ref LineNo));
				sb.Append("\nGO\n");
				sb.Append("\n---------------------------------------------------------");
				sb.Append("\n");
				LineNo += 3;
			}
			return sb.ToString();
		}
	}


	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	public class Pair<T1, T2> {		// TODO: I think we can use class KeyValuePair
		public T1 First;
		public T2 Second;

		//---------------------------------------------------------------------------------------

		public Pair(T1 first, T2 second) {
			First = first;
			Second = second;
		}
	}

	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	public class BartSqlLogRecord {
		public string PreComment;
		public string SQL;
		public string PostComment;
		public List<Pair<string, object>> Parms;

		//---------------------------------------------------------------------------------------

		public BartSqlLogRecord(SqlCommand cmd, string Comment) {
			this.PreComment = Comment;
			PostComment = "";
			if (cmd == null) {			// Check for comments only
				SQL = null;
				Parms = null;
			} else {
				SQL = cmd.CommandText;
				Parms = new List<Pair<string, object>>();
				foreach (SqlParameter parm in cmd.Parameters) {
					Parms.Add(new Pair<string, object>(parm.ParameterName, parm.Value));
				}
			}
		}

		//---------------------------------------------------------------------------------------

		public string ToSproc(Dictionary<string, int> ht, ref int LineNo) {
			StringBuilder sb = new StringBuilder();
			// PreComment
			sb.AppendFormat("\n-- Line {0}", ++LineNo);
			string[] CommentLines = PreComment.Split('\n');
			foreach (string line in CommentLines) {
				if (line.Length == 0) {
					sb.Append("\n");
				} else {
					sb.AppendFormat("\n-- {0}", line);
				}
				++LineNo;
			}

			// DECLAREs
			if (Parms != null) {
				string DataType;
				foreach (Pair<string, object> parm in Parms) {
					sb.Append("\n");
					++LineNo;
					string ParmName = parm.First;
					string objType = parm.Second.GetType().Name;
					// When we added GO statements, we found that our previous
					// declarations seemed to be erased by GO. So bypass our code
					// to remember that a variable had been declared and always
					// pump out a declaration
#if false
					if (ht.ContainsKey(ParmName)) {
						sb.AppendFormat("-- (See line {0} above) ", ht[ParmName]);
					} else {
						ht[ParmName] = LineNo;
					}
#endif
					// Handle enums differently
					Type t = parm.Second.GetType();
					if (t.BaseType.Name == "Enum") {
						sb.AppendFormat("DECLARE {0} int", parm.First);
						continue;
					}
					switch (objType) {
					case "String":
						DataType = "varchar(MAX)";
						break;
					case "Int32":
						DataType = "int";
						break;
					case "Int64":
						DataType = "bigint";
						break;
					case "DateTime":
						DataType = "datetime";
						break;
					case "Byte":
						DataType = "tinyint";
						break;
					case "DBNull":
						DataType = "varchar(MAX)";
						break;
					default:
						DataType = "Unknown - " + objType;
						break;
					}
					sb.AppendFormat("DECLARE {0} {1};", parm.First, DataType);
				}
				sb.Append("\n");
				++LineNo;

				// SETs
				foreach (Pair<string, object> parm in Parms) {
					string txt;
					object val = parm.Second;
					Type t = parm.Second.GetType();
					if (t.BaseType.Name == "Enum") {
						txt = Convert.ToInt32(val).ToString();
					} else if (t.Name == "DBNull") {
						txt = "''";
					} else if (val is string) {
						txt = val.ToString().Replace("'", "''");
						txt = string.Format("'{0}'", txt);
					} else if (val is DateTime) {
						txt = string.Format("'{0}'", val);
					} else {
						txt = val.ToString();
					}
					sb.AppendFormat("\nSET {0} = {1};", parm.First, txt);
					++LineNo;
				}
			}

			// SQL
			if (SQL != null) {
				string[] SqlLines = SQL.Split('\n');
				string FmtSQL = "";
				foreach (string line in SqlLines) {
					if (line.StartsWith("--")) {
						FmtSQL += "\n" + line;
					} else {
						FmtSQL += "\n\t" + line;
					}
					++LineNo;
				}
				sb.AppendFormat("\n{0};", FmtSQL);
				++LineNo;

				sb.Append("\n");
				++LineNo;
			}

			// PostComment
			string[] PostCommentLines = PostComment.Split('\n');
			foreach (string line in PostCommentLines) {
				if (line.Length == 0) {
					sb.Append("\n");
				} else {
					sb.AppendFormat("\n-- {0}", line);
				}
				++LineNo;
			}

			return sb.ToString();
		}
	}
}
