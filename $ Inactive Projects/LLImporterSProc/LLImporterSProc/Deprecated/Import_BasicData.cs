// Copyright (c) 2003-2006 by Bartizan Data Systems, LLC

// Turn this on to, well, dump basic data
// #define	DUMPBASICDATA

using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bartizan.Importer {
	class Import_BasicData {

		IDbConnection	conn;
		ParsedSwipe		SwipeData;
		DBCoordinator	DBCoord;
		ImporterTable	impPerson;				// Stuff about tblPerson

//---------------------------------------------------------------------------------------

		public Import_BasicData(IDbConnection conn, DatabaseType dbType, DBCoordinator DBCoord) {
			this.conn		= conn;
			this.DBCoord	= DBCoord;

			impPerson = DBCoord.RegisterTable(ImporterTable.TableName_tblPerson, dbType, conn);
			if (dbType == DatabaseType.OleDB) {
				((OleDbDataAdapter)impPerson.adapt).RowUpdated += new OleDbRowUpdatedEventHandler(adaptPerson_OleDb_RowUpdated);
			} else if (dbType == DatabaseType.SQLServer) {
				((SqlDataAdapter)impPerson.adapt).RowUpdated += new SqlRowUpdatedEventHandler(adaptPerson_SQL_RowUpdated);
			}
		}

//---------------------------------------------------------------------------------------

		// TODO: All these need work
		void adaptPerson_OleDb_RowUpdated(object sender, OleDbRowUpdatedEventArgs args) {
			impPerson.CommonRowUpdated(sender, args, "SSNid");
		}

//---------------------------------------------------------------------------------------

		void adaptPerson_SQL_RowUpdated(object sender, SqlRowUpdatedEventArgs args) {
			impPerson.CommonRowUpdated(sender, args, "SSNid");
		}

//---------------------------------------------------------------------------------------

		public void Import(ParsedSwipe SwipeData) {
			this.SwipeData = SwipeData;
			// TODO: For now, we're trade show specific, looking for Card #
			string		CardNum;
			// TODO: At the very least, use const's for "1" and "Card #". Better yet
			//		 would be to make things table-driven.
			if (! SwipeData.BasicData.ContainsKey("1")) {
				// TODO: Should log the error
				return;
			}
			CardNum = (string)SwipeData.BasicData["1"];
			if (impPerson.KeyExists(ImporterTable.TableName_tblPerson, "Card #", CardNum)) {
				return;
			}

			DataRow			row;
			FieldInfo		fi = null;
			DataTable		tblPerson = impPerson.dt;
			row = tblPerson.NewRow();
			foreach (string key in SwipeData.BasicData.Keys) {
				try {
					fi = (FieldInfo)SwipeData.FieldDefs[key];
					if (fi.FieldName == "Veteran" || fi.FieldName == "Public Assistance") {
						// TODO: Handle specially.
						// TODO: Actually, handle this in ParsedSwipe
					} else if (impPerson.Columns.Contains(fi.FieldName)) {	// See if field in schema
						// Check to see impPerson the field name is in the schema. It's a lot 
						// faster and easier to do this, than to cope with an insert attempt
						// throwing an exception
						row[fi.FieldName] = SwipeData.BasicData[key];
					} else {
						// TODO: Do something here
					}
				} catch (Exception) {
					throw;		// TODO:
				}
			}

			try {
				tblPerson.Rows.Add(row);
				impPerson.UpdateTable();
			} catch (Exception) {
				throw;			// TODO:
				// TODO: Do something here
			} finally {
				tblPerson.Rows.Clear();
			}
			DumpBasicData();			
		}

//---------------------------------------------------------------------------------------

		[Conditional("DUMPBASICDATA")]
		void DumpBasicData() {
			// Note: This is a debugging routine
			string[] keys = new string[SwipeData.BasicData.Keys.Count];
			SwipeData.BasicData.Keys.CopyTo(keys, 0);
			Array.Sort(keys);
			foreach (string key in keys) {
				ODS.ods("{0} = {1}", key, SwipeData.BasicData[key]);
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public static class ODS {          // ODS = OutputDebugString
#if ! SQLSERVER
		[DllImport("Kernel32")]
		public static extern void OutputDebugString(string msg);
#endif

		public static void ods(string fmt, params object[] args) {
#if ! SQLSERVER
			OutputDebugString(string.Format(fmt, args));
#else
			// TODO: Not sure what to put here. Pipe.Send?????
#endif
		}
	}

}
