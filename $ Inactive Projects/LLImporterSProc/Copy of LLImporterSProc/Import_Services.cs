// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Bartizan.Importer {

	class Import_Services {

		Hashtable			htServiceNames;		
		IDbConnection		conn;
		DatabaseType		dbType;
		ParsedSwipe			SwipeData;
		DBCoordinator		DBCoord;
		ImporterTable		impServices;
		ImporterTable		impPersonServices;


//---------------------------------------------------------------------------------------

		public Import_Services(IDbConnection conn, DatabaseType dbType, DBCoordinator DBCoord) {
			this.conn		= conn;
			this.dbType		= dbType;
			this.DBCoord	= DBCoord;

			impServices = DBCoord.RegisterTable(ImporterTable.TableName_tblServices, dbType, conn);
			impPersonServices = DBCoord.RegisterTable(ImporterTable.TableName_tblPersonServices, dbType, conn);
	
			// htServiceNames = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			htServiceNames = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

			// TODO: This (and the other places we do it) sucks.
			if (dbType == DatabaseType.OleDB) {
				((OleDbDataAdapter)impServices.adapt).RowUpdated += new OleDbRowUpdatedEventHandler(adaptServices_OleDb_RowUpdated);
			} else if (dbType == DatabaseType.SQLServer) {
				((SqlDataAdapter)impServices.adapt).RowUpdated += new SqlRowUpdatedEventHandler(adaptServices_SQL_RowUpdated);
			}

			LoadSmallTables();
		}

//---------------------------------------------------------------------------------------

		void adaptServices_OleDb_RowUpdated(object sender, OleDbRowUpdatedEventArgs args) {
			impServices.CommonRowUpdated(sender, args, "ServiceID");
		}

//---------------------------------------------------------------------------------------

		void adaptServices_SQL_RowUpdated(object sender, SqlRowUpdatedEventArgs args) {
			impServices.CommonRowUpdated(sender, args, "ServiceID");
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// At times (e.g. for Services), we need to see if an entry is already in one
		/// of our tables (tblServices), and if so, return its autonum CurField field. In
		/// Access, we'd normally just specify an index (in this case on the Description,
		/// not the CurField) and do a Seek on it. But the ADO.NET in-memory database support
		/// doesn't seem to support searching on secondary indexes very well (e.g. it
		/// seems you'd need to set up a DataView and filter based on a SELECT query).
		/// So we're just going to set up a simple Hashtable or so to hold some of
		/// this data, especially since the data is going to be relatively small. And for
		/// that matter, it'll also be (much) simpler to code than DataViews, filters,
		/// etc Note: In some places, we refer to these as the "small tables"
		/// </summary>
		void LoadSmallTables() {
			IDbCommand		cmd = null;
			IDataReader		rdr;
			// Load tblServices
			string			SQL;
			SQL = "SELECT ServiceID, Description FROM [" + ImporterTable.TableName_tblServices + "]";
			if (dbType == DatabaseType.OleDB) {
				cmd = new OleDbCommand(SQL, (OleDbConnection)conn);
			} else if (dbType == DatabaseType.SQLServer) {
				cmd = new SqlCommand(SQL, (SqlConnection)conn);
			}
			rdr = cmd.ExecuteReader();
			while (rdr.Read()) {
				htServiceNames[rdr["Description"]] = rdr["ServiceID"];
			}
			rdr.Close();
		}

//---------------------------------------------------------------------------------------

		public void Import(ParsedSwipe SwipeData) {
			string		ServiceName;
			int			serviceID;
			this.SwipeData = SwipeData;
			for (int i = 0; i < SwipeData.Services.Count; ++i) {
				ServiceName = SwipeData.Services[i].service;
				serviceID = AddService(ServiceName);
				AddSwipe(serviceID);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Returns the CurField for the specified ServiceName, adding the record if necessary.
		/// </summary>
		/// <param name="ServiceName">The name of the ServiceName (e.g. Resource Room)</param>
		/// <returns>The numeric CurField of the ServiceName.</returns>
		int AddService(string ServiceName) {
			int nID;
			// First, see if it's already in our table
			if (htServiceNames.Contains(ServiceName))
				return (int)htServiceNames[ServiceName];
			DataTable		tblServices = impServices.dt;
			DataRow row = tblServices.NewRow();
			row["Description"] = ServiceName;
			row["CatID"] = 1;
			// TODO: Arguably, the default CatID should either be a const, or
			//		 an XML-based configuration parameter.
			tblServices.Rows.Add(row);
			impServices.UpdateTable();

#if true
#if false	// TODO: Major
			nID = LastIDENTITY;
#else
			nID = -29;
#endif
#else
			nID = (int)row["ServiceID"];	
#endif
			tblServices.Rows.Clear();
			htServiceNames[ServiceName] = nID;
			return nID;
		}

//---------------------------------------------------------------------------------------

		void AddSwipe(int serviceID) {
			string		CardNum, ServiceDate, WhereDate;
			try {
				// TODO: Make next two class variables?
				DataTable tblPersonService = impPersonServices.dt;
				try {
					CardNum = (string)SwipeData.BasicData["1"];
					ServiceDate = (string)SwipeData.BasicData["11"];
					WhereDate = impPersonServices.DateDelim + ServiceDate + impPersonServices.DateDelim;
					if (impPersonServices.KeyExists(ImporterTable.TableName_tblPersonServices, "Card #", CardNum, "ServiceDate", WhereDate))
						return;
					DataRow row = tblPersonService.NewRow();
					row["SSN"] = SwipeData.BasicData["506"];	// SSN
					row["Card #"] = CardNum;						// Card #
					row["ServiceID"] = serviceID;
					row["ServiceDate"] = ServiceDate;					// Date and Time
					// TODO: Get these
					// row["ServiceState"] = DBNull.Value;
					// row["TerminalID_ID"] = DBNull.Value;
					// row["ProviderID"] = DBNull.Value;
					tblPersonService.Rows.Add(row);
					impPersonServices.UpdateTable();
				} catch (Exception) {
					throw;					// TODO:
				}
				tblPersonService.Rows.Clear();
			} catch (Exception) {
				throw;						// TODO:
			}
		}
	}
}
