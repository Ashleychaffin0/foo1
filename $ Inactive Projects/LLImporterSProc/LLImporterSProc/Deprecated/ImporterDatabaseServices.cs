// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Bartizan.Importer {

	/// <summary>
	/// Keep track of what kind of database we're talking to (OleDB, SQL Server, etc).
	/// Note that there are a number of places in this module where we check the
	/// database type. The code is usually of the form "if (dbType==OleDB) ... else if
	/// (dbType==SQLServer)". If/when we ever add a new entry (Oracle, MySQL, etc)
	/// to this enum, we'll need to go through all the places where we check the type
	/// and add more else clauses.
	/// </summary>
	public enum DatabaseType {
		OleDB,
		SQLServer
	}




//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class DBCoordinator {

		Hashtable ImportTables;

//---------------------------------------------------------------------------------------

		public DBCoordinator() {
			// ImportTables = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			ImportTables = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		}

//---------------------------------------------------------------------------------------

		public ImporterTable GetImporterTable(string TableName) {
			if (ImportTables.Contains(TableName)) {
				return (ImporterTable)ImportTables[TableName];
			} else {
				// TODO: throw exception; but for now, return null
				return null;
			}
		}

//---------------------------------------------------------------------------------------

		public ImporterTable RegisterTable(string TableName, DatabaseType dbType, IDbConnection conn) {
			if (! ImportTables.Contains(TableName))
				ImportTables[TableName] = new ImporterTable(TableName, dbType, conn);
			return (ImporterTable)ImportTables[TableName];
		}
	}




//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// If we were going to do all our work in this module, we could define all our
	/// database data as standard variables. But since we want to 
	/// </summary>
	public class ImporterTable {
		public string				TableName;
		// TODO: I don't like the whole DatabaseType/DateDelim/etc stuff
		public DatabaseType			dbType;
		public string				DateDelim;		// WHERE SwipeTime = #11/24/2004# for Access
													// WHERE SwipeTime = '11/24/2004' for SS
													// So this contains either # or '
		public DataTable			dt;
		public IDbDataAdapter		adapt;
		public DataColumnCollection	Columns;		// i.e. Schema
		public IDbConnection		conn;			// TODO: static field??? since it's 
													//		 always going to be the same
													//		 for all???

		// TODO: I'm not at all sure about the following...
		// Unfortunately, there's no ICommandBuilder (maybe in ADO.NET 2.0?)
		OleDbCommandBuilder			AccessCmdBuilder;
		SqlCommandBuilder			SQLCmdBuilder;

		// Sigh. While Access and SQL Server both support @@IDENTITY (or Scope_Identity,
		// see ADO.NET Cookbook recipe 4.2, page 163), you don't seem to be save the
		// value back into the DataRow. So for the time being, we'll kludge it (sigh
		// again), by using a global (single instance) variable.
		int                         LastIDENTITY = -1;


		// TODO: Don't hardcode table names. Put them in some kind of config file.
		public const string TableName_tblPerson = "LLtblPerson";
		public const string TableName_tblServices = "LLtblServices";
		public const string TableName_tblPersonServices = "LLtblPersonService";
		public const string TableName_tblDemographicsQuestions = "LLtblDemographicsQuestions";
		public const string TableName_tblDemographicsAnswers = "LLtblDemographicsAnswers";
		public const string TableName_tblPersonnelDemographics = "LLtblPersonnelDemographics";



//---------------------------------------------------------------------------------------

		public ImporterTable(string TableName, DatabaseType dbType, IDbConnection conn) {
			this.TableName	= TableName;
			this.dbType		= dbType;
			this.conn		= conn;

			if (dbType == DatabaseType.OleDB)
				DateDelim = "#";
			else if (dbType == DatabaseType.SQLServer)
				DateDelim = "'";

			GetAdapter();
		}

//---------------------------------------------------------------------------------------

		void GetAdapter() {
			IDbCommand cmd;

			if (dbType == DatabaseType.OleDB) {
				cmd = new OleDbCommand(TableName);
				cmd.CommandType = CommandType.TableDirect;
				cmd.Connection = conn;
				adapt = new OleDbDataAdapter((OleDbCommand)cmd);
				AccessCmdBuilder = new OleDbCommandBuilder((OleDbDataAdapter)adapt);
				AccessCmdBuilder.QuotePrefix = "[";
				AccessCmdBuilder.QuoteSuffix = "]";
				// adapt.UpdateCommand = AccessCmdBuilder.GetUpdateCommand();
				adapt.InsertCommand = AccessCmdBuilder.GetInsertCommand();
			} else if (dbType == DatabaseType.SQLServer) {
				cmd = new SqlCommand("SELECT * FROM [" + TableName + "]");
				cmd.Connection = conn;
				adapt = new SqlDataAdapter((SqlCommand)cmd);
				SQLCmdBuilder = new SqlCommandBuilder((SqlDataAdapter)adapt);
				SQLCmdBuilder.QuotePrefix = "[";
				SQLCmdBuilder.QuoteSuffix = "]";
				// adapt.UpdateCommand = SQLCmdBuilder.GetUpdateCommand();
				adapt.InsertCommand = SQLCmdBuilder.GetInsertCommand();
			}
			// adapt.ContinueUpdateOnError = true;	// TODO: Put back later
			// adapt.Fill(tblPerson);

			// The IDbDataAdapter interface doesn't have an overload to do a FillSchema
			// directly to a table, only to a dataset. So, sigh, we'll do it the
			// roundabout way.
			DataSet ds = new DataSet("GetDataAdapter");	// DS Name only for debugging
			adapt.FillSchema(ds, SchemaType.Source);
			dt = ds.Tables[0];
			dt.TableName = TableName;
			ds.Tables.Remove(dt);

			Columns = dt.Columns;
		}

//---------------------------------------------------------------------------------------

		public bool KeyExists(string FieldName, object KeyValue, params object[] FieldName_KeyValue) {
			// TODO: Assumes that the params are paired properly (i.e (FieldName_KeyValue.Length % 2) == 0
			string SQL;
			SQL = string.Format("SELECT COUNT(*) FROM [{0}] WHERE (([{1}] = {2})",
				TableName, FieldName, KeyValue);
			// TODO: Above SQL has problems if KeyValue is string
			for (int i = 0; i < FieldName_KeyValue.Length; i += 2) {
				SQL += string.Format(" AND ([{0}] = {1})", FieldName_KeyValue[i], FieldName_KeyValue[i + 1]);
			}
			SQL += ")";
			IDbCommand cmd = null;
			if (dbType == DatabaseType.OleDB) {
				cmd = new OleDbCommand(SQL, (OleDbConnection)conn);
			} else if (dbType == DatabaseType.SQLServer) {
				cmd = new SqlCommand(SQL, (SqlConnection)conn);
			}
			object o = cmd.ExecuteScalar();
			int nRecs = Convert.ToInt32(o);		// SQL Server returns Decimal, not int
			return nRecs > 0;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// One assumes I don't know what I'm doing yet in ADO.NET. And they'd be right.
		/// <para>
		/// We've got a DataTable with a single row we want to add to a real database 
		/// table. But for some really strange reason, we seem to need to a) update a
		/// DataSet (rather than a DataTable), b) the table name <i>must</i> be
		/// "Table". So we're going to do this in this clumsy, roundabout, inefficient
		/// fashion for now. Maybe someday I'll figure out a better way to do it.
		/// </para>
		/// </summary>
		/// <param name="dt">The table to be updated.</param>
		/// <param name="adapt">Its adapter (OleDB, SQLServer, etc).</param>
		public void UpdateTable() {
			string SavedName = dt.TableName;
			DataSet ds = new DataSet();
			try {
				dt.TableName = "Table";
				ds.Tables.Add(dt);
				adapt.Update(ds);
			} catch (Exception) {
				throw;			// TODO:
			} finally {
				ds.Tables.Remove(dt);
				dt.TableName = SavedName;
			}
		}

//---------------------------------------------------------------------------------------

		public void CommonRowUpdated(object sender, RowUpdatedEventArgs args, string FieldName) {
			if (args.StatementType == StatementType.Insert) {
				IDbCommand cmd = null;
				if (dbType == DatabaseType.OleDB) {
					OleDbDataAdapter AccessAdapt = (OleDbDataAdapter)sender;
					cmd = new OleDbCommand("SELECT @@IDENTITY",
						AccessAdapt.InsertCommand.Connection);
				} else if (dbType == DatabaseType.SQLServer) {
					SqlDataAdapter SqlAdapt = (SqlDataAdapter)sender;
					cmd = new SqlCommand("SELECT @@IDENTITY",
						SqlAdapt.InsertCommand.Connection);
				}
				// Store the autonum into the specified field
				object ID = cmd.ExecuteScalar();

				// Sometimes, the table we're inserting into doesn't have an Autonum
				// field. If that's the case, we'll get DbNull returned. If so, just
				// leave (setting LastIDENTITY to -1, just sort of on general 
				// principles, for debugging's sake).
				if (ID == DBNull.Value) {
					LastIDENTITY = -1;
					return;
				}

				// Sigh. Access returns an int for @@IDENTITY, but SQL Server returns
				// a Decimal, so we have to use Convert.
				LastIDENTITY = Convert.ToInt32(ID);
				// But now we have another problem. It looks like SQL Server treats
				// an Identity field as read-only, so we can't store the value back.
				// They seem to assume that the CurField field will be set inside an SPROC.
				// Sigh. For now, code around it, and come back later. TODO:
				// TODO: Now that we've got LastIDENTITY, I guess we don't need these
				//		 next couple of lines.
				if (dbType == DatabaseType.OleDB)
					args.Row[FieldName] = LastIDENTITY;
			}
		}

//---------------------------------------------------------------------------------------

		public static void EmptyTable(string TableName, IDbConnection conn) {
			// TODO: This doesn't really belong in ImporterTable. Move it to some kind
			//		 of generic database utility class later.
			string SQL = "DELETE FROM [" + TableName + "]";
			IDbCommand cmd = null;
			if (conn is OleDbConnection)
				cmd = new OleDbCommand(SQL, (OleDbConnection)conn);
			else if (conn is SqlConnection)
				cmd = new SqlCommand(SQL, (SqlConnection)conn);
			cmd.ExecuteNonQuery();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The following function is a modified version of one found in WIALink 1.1.
		/// It's a kludge. We need to demo this program next week, and produce some
		/// reports. But for that we need SQL Server reports, and I took the easy way
		/// out and developed this first for Access. So we'll kludge things and import
		/// data into Access, then use this routine to copy the tables over to SQL
		/// Server. Sigh.
		/// <para>
		/// With that said, we wound up with enough time to produce a SQL Server
		/// version of the Importer, so we wound up not using this routine. But since
		/// it might come in useful some day, I left it in. However, it has never
		/// been tested, so beware.
		/// </para>
		/// </summary>
		/// <param name="TableName"></param>
		/// <param name="DBLocation"></param>
		/// <param name="conn"></param>
		/// <returns></returns>
		// TODO: This doesn't belong in this class. Put into a common library.
		bool CopyTableFromAccessToSQLServer(
						string TableName,
						OleDbConnection OleDbConn,	// "From" database connection
			SqlConnection SqlConn		// SQL Server connection
			) {
			string SQL = "SELECT * FROM [" + TableName + "]";
			DataTable dtSrc = new DataTable(TableName);
			try {
				OleDbDataAdapter adaptSrc = new OleDbDataAdapter(SQL, OleDbConn);
				adaptSrc.Fill(dtSrc);
			} catch (Exception) {		// TODO: Was: Exception e
#if false	// TODO: Don't worry about it now
				string	msg = "An unexpected error ({0}) occurred reading the previous"
					+ " WIALink database ({1}). The migration cannot be performed.";
				msg = string.Format(msg, e.Message, DBLocation);
				MessageBox.Show(msg, "WIALink",	MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				conn.Close();
#endif
				return false;
			}

			DataTable dtTarget = new DataTable(TableName);
			try {
				SqlDataAdapter adaptTgt = new SqlDataAdapter(SQL, SqlConn);
				SqlCommandBuilder CmdBuilder = new SqlCommandBuilder(adaptTgt);
				// These next lines are needed if any of the field names have embedded blanks
				CmdBuilder.QuotePrefix = "[";
				CmdBuilder.QuoteSuffix = "]";
				adaptTgt.ContinueUpdateOnError = true;

				// See the original code in WIALink to see why this is done this way
				adaptTgt.FillSchema(dtTarget, SchemaType.Source);
				foreach (DataRow row in dtSrc.Rows) {
					dtTarget.Rows.Add(row.ItemArray);
				}
				adaptTgt.Update(dtTarget);
				// If an exception isn't thrown, I'm going to assume that all
				// went well enough. Yeah, we could have had every row fail
				// to add if they migrated more than once, but clearly I don't
				// want to confuse matters by complaining about such things. 
				// Yeah, I could go through the rows and see if there were any
				// problems other than duplicates, but that's not really easy.
				// I'd have to decode the text of the GetColumnError() method,
				// which doesn't strike me as a firm basis for going forward.
				// I'd trust an enum, but not a text message that could change (not
				// to mention the I18n considerations).
			} catch (Exception) {		// TODO: Was: Exception e2
#if false	// TODO: Don't worry about it now
				MessageBox.Show("An unexpected error (" + e2.Message + ") occurred during"
					+ " migration. See the Message Log report for details.", "WIALink", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				StringBuilder	sb = new StringBuilder();
				sb.AppendFormat("An internal error ({0}) occurred while migrating data.", 
					e2.Message);
				string	msg = AppCode.FormatDataRowsInError(dtTarget);
				if (msg.Length > 0) 
					sb.AppendFormat(" Details follow:\n\n{0}", msg);
				AppCode.LogMsg(WIAForm.connWIALink, WIAMessageLog_Severity.Error, sb.ToString());
#endif
				return false;
			}
			return true;
		}

	}
}
