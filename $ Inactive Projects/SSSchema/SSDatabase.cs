// Copyright (c) Bartizan Connect LLP

// See http://msdn2.microsoft.com/en-us/library/ms254969(VS.80).aspx for GetSchema results

// This link shows part of what we've done below, but also shows other things, like
// enumerating the instances of SQL Server on your system and the databases inside them.
// http://blog.vuscode.com/malovicn/archive/2007/11/12/how-to-build-your-own-sql-server-explorer.aspx

// More links
//	http://www.devx.com/dbzone/Article/27131/0/page/1
//	http://www.codeproject.com/KB/database/DetermineSql2005SPParams.aspx

// Good stuff: 
//		Note on the following: By default, GetSchema(...) uses INFORMATION_SCHEMA. But
//		according to Kalen Delaney, Inside Microsoft SQL Server 2005: The Store Engine, 
//		page 180, the information in INFORMATION_SCHEMA comply with the SQL-92 standard 
//		definition for INFORMATION_SCHEMA, but is limited to the data defined in that 
//		standard. And since index information (among others) isn't defined in the 
//		standard, you can't get it from INFORMATION_SCHEMA. So you won't necessarily get
//		complete information using INFORMATION_SCHEMA, and you're better off (if perhaps
//		slightly release-of-SQL Server-depenedent) using the sys.* objects. 
//
//		Foe example, GetSchema for Tables uses INFORMATION_SCHEMA (as of .NET 3.5)
//		(thanks to Microsoft letting us see the source (and trace into) for the ADO.NET
//		classes). But if you ask for Indexes, it bypasses INFORMATION_SCHEMA and 
//		produces the following SQL:
//		select distinct 
//			db_Name() as constraint_catalog,
//			constraint_schema = user_name(o.uid), 
//			constraint_name = x.name, 
//			table_catalog  = db_name(), 
//			table_schema = user_name(o.uid), 
//			table_name = o.name, 
//			index_name = x.name 
//				from 	sysobjects o, 
//						sysindexes x, 
//						sysindexkeys xk 
//				where 	o.type in ('U') 
//						and x.id = o.id 
//						and o.id = xk.id 
//						and x.indid = xk.indid 
//						and xk.keyno < = x.keycnt 
//						and (db_name() = @Catalog or (@Catalog is null)) 
//						and (user_name()= @Owner or (@Owner is null)) 
//						and (o.name = @Table or (@Table is null)) 
//						and (x.name = @Name or (@Name is null)) 
//			order by table_name, index_name

// For example, 
#if false
select TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE from INFORMATION_SCHEMA.TABLES where (TABLE_CATALOG = @Catalog or (@Catalog is null)) and (TABLE_SCHEMA = @Owner or (@Owner is null)) and (TABLE_NAME = @Name or (@Name is null)) and (TABLE_TYPE = @TableType or (@TableType is null))

BOL: ms-help://MS.SQLCC.v9/MS.SQLSVR.v9.en/tsqlref9/html/7e9f1dfe-27e9-40e7-8fc7-bfc5cae6be10.htm for INFORMATION_SCHEMA

execute sp_helptext "information_schema.tables"

use master

-- select * from sys.objects order by name

use lldevel 

-- select * from sys.objects order by type, name

-- select * from INFORMATION_SCHEMA.COLUMNS order by table_name, ordinal_position
-- select * from INFORMATION_SCHEMA.VIEWS 
-- select * from INFORMATION_SCHEMA.TABLES
-- select * from INFORMATION_SCHEMA.referential_constraints 
-- select * from INFORMATION_SCHEMA.parameters
-- select * from INFORMATION_SCHEMA.routines
-- select * from INFORMATION_SCHEMA.SCHEMATA
-- execute sp_helptext "information_schema.tables"
-- select * from INFORMATION_SCHEMA.routine_columns
select * from sys.all_objects order by object_id
-- select * from sys.all_columns
-- select * from sys.all_parameters
select * from sys.all_sql_modules -- source code
-- select * from sys.all_views
-- select * from sysindexes
-- select * from sysindexkeys


#endif

// TODO: 
//	*	Ahmed: Default values for columns, Nullable, Foreign Keys
//	*	Add schema (dbo/ahmed/etc) to everything
//	*	Pick up rest of SprocParms, and rename them
//	*	Needs Sproc.DumpAsString
//	*	Where's the return value for sproc FUNCTIONs?
//	*	Foreign keys. Also, don't seem to have full information
//	*	Add other stuff, such as sprocs, etc, etc, etc
//	*	Maybe add [Flags] enum, to get only certain pieces. For example, we don't
//		need field and index info, if all we want is the names of the tables.
//	*	Make GetTables etc static, returning List<SSTable> etc, so that we can
//		pick up just the pieces we need.
//	*	Add DateTime when database scan started (just, well, just because). But also
//		add a StopWatch to time how long it took on Devel vs Prod.
//	*	Format Fields as (e.g. VARCHAR(10))
//	*	When you script (e.g.) an index in SS Management Studio, you get all kinds
//		of extra info. Where is this coming from?
//	*	Add Views and UDT's
//	*	Collect *all* data, not just the stuff we currently understand
//	*	Add table size


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
// using System.;				// TODO: Just to get DLL name for NetMassDownloader

namespace Bartizan.Utils.SQL {
	public class SSDB {
		public string			dbName;
		public string			DataSource;

		public List<SSUser>		Users	= new List<SSUser>();
		public List<SSTable>	Tables	= new List<SSTable>();
		public List<SSSprocs>	Sprocs	= new List<SSSprocs>();

		SqlConnection conn;

//---------------------------------------------------------------------------------------

		public SSDB(SqlConnection conn, Func<SSTable, bool> TableNameFilter) {
			// TODO: Maybe make overload that takes a connection string
			// TODO: Maybe make overload that doesn't take filter
			this.conn = conn; 
			dbName = conn.Database;
			DataSource = conn.DataSource;

			GetUsers();
			GetTables(TableNameFilter);
			GetStoredProcedures();

			// System.Web.AspNetHostingPermission x = null;
		}

//---------------------------------------------------------------------------------------

		private void GetUsers() {
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.Users);
			foreach (DataRow row in dt.Rows) {
				Users.Add(new SSUser(conn, row));
			}
		}

//---------------------------------------------------------------------------------------

		private void GetTables(Func<SSTable, bool> TableNameFilter) {
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.Tables);
			foreach (DataRow row in dt.Rows) {
				// The following isn't right for performance reasons. We don't want to
				// fully instantiate the SSTable (along with fields, indexes, etc) just
				// to then throw it away if the filter on the name fails. But for now...
				// (And besides, it shows off the Func<> template on a user-defined type)
				SSTable	table = new SSTable(conn, row);
				if ((TableNameFilter == null) || TableNameFilter(table))
					Tables.Add(table);
			}
#if false
			Comparison<SSTable> comp = new Comparison<SSTable>(delegate (SSTable x, SSTable y) {
				return string.Compare(x.TableName, y.TableName);
			});

			Tables.Sort(comp);
#else		// Much easier
			Tables.Sort((x, y) => (string.Compare(x.TableName, y.TableName)));
#endif
		}


//---------------------------------------------------------------------------------------

		private void GetStoredProcedures() {
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.Procedures);
			foreach (DataRow row in dt.Rows) {
				Sprocs.Add(new SSSprocs(conn, row));
			}
			Sprocs.Sort((x, y) => (string.Compare(x.SprocName, y.SprocName)));
		}


//---------------------------------------------------------------------------------------

		public static object MyNull(object val) {
			if ((val == null) || (val == DBNull.Value)) {
				return null;
			}
			return val;
		}

//---------------------------------------------------------------------------------------

		public string DumpAsString() {
			StringBuilder	sb = new StringBuilder();
			sb.AppendFormat("Dumping database {0} on {1} as of {2}\r\n", 
				dbName, DataSource, DateTime.Now);
			
			sb.Append(SSUser.DumpAsString(Users));

			sb.AppendLine();
			sb.Append(SSTable.DumpAsString(Tables));

			sb.AppendLine();
			sb.Append(SSSprocs.DumpAsString(Sprocs));

			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSUser {
		public short	UserID;
		public string	UserName;
		public DateTime	CreationDate;
		public DateTime	LastUpdateTime;

//---------------------------------------------------------------------------------------

		public SSUser(SqlConnection conn, DataRow row) {
			UserID			= (short)row["uid"];
			UserName		= (string)row["user_name"];
			CreationDate	= (DateTime)row["createdate"];
			LastUpdateTime	= (DateTime)row["updatedate"];
		}

//---------------------------------------------------------------------------------------

		public static string DumpAsString(List<SSUser> Users) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("---------- Users ----------");
			foreach (var User in Users) {
				sb.AppendFormat("{0}, Uid={1}, Created on {2}, Last Updated {3}\r\n",
					User.UserName, User.UserID, User.CreationDate, User.LastUpdateTime);
			}
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSTable {
		public string				TableName;
		public string				TableSchema;
		public string				TableType;
		public List<SSTableField>	TableFields;
		public List<SSTableIndex>	TableIndexes;

//---------------------------------------------------------------------------------------

		public SSTable(SqlConnection conn, DataRow row) {
			this.TableName	 = (string)row["Table_Name"];
			this.TableSchema = (string)row["Table_Schema"];
			this.TableType	 = (string)row["Table_Type"];

			GetFields(conn);			
			GetIndexes(conn);
		}

//---------------------------------------------------------------------------------------

		private void GetFields(SqlConnection conn) {
			// Get Fields (aka Columns)
			string[] restrictions = { null, null, TableName, null };
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.Columns,
								restrictions);
			TableFields = new List<SSTableField>();
			foreach (DataRow TableRow in dt.Rows) {
				TableFields.Add(new SSTableField(conn, TableRow));
			}
			// Sort them by position
			TableFields.Sort((x, y) => {
				if (x.ColumnNumber < y.ColumnNumber)
					return -1;
				if (x.ColumnNumber > y.ColumnNumber)
					return 1;
				return 0;
			});
		}

//---------------------------------------------------------------------------------------

		private void GetIndexes(SqlConnection conn) {
			string[] restrictions = { null, null, TableName, null };
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.Indexes,
								restrictions);
			TableIndexes = new List<SSTableIndex>();
			foreach (DataRow TableRow in dt.Rows) {
				TableIndexes.Add(new SSTableIndex(conn, TableRow, TableName));
			}
			// Sort them by name
			TableIndexes.Sort((x, y) => (string.Compare(x.IndexName, y.IndexName)));
		}

//---------------------------------------------------------------------------------------

		public static string DumpAsString(List<SSTable> Tables) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("---------- TABLES ----------");
			foreach (var table in Tables) {
				sb.AppendFormat("{0}.{1}\r\n", table.TableSchema, table.TableName);
				string s = SSTableField.DumpAsString(table.TableFields);
				sb.Append(s);
				s = SSTableIndex.DumpAsString(table.TableIndexes);
				sb.Append(s);
			}
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSTableField {
		public string	FieldName;
		public int		ColumnNumber;
		public string	Default;
		public string	IsNullable;
		public string	DataType;
		public int?		CharMaxLength;
		public int?		CharOctetLength;
		public byte?	NumericPrecision;
		public short?	NumericPrecisionRadix;
		public int?		NumericPrecisionScale;
		public short?	DateTimePrecision;
		public string	CharacterSetCatalog;
		public string	CharacterSetSchema;
		public string	CharacterSetName;
		public string	CollationCatalog;

//---------------------------------------------------------------------------------------

		public SSTableField(SqlConnection conn, DataRow row) {
			FieldName			  = (string)SSDB.MyNull(row["COLUMN_NAME"]);
			ColumnNumber		  = (int)SSDB.MyNull(row["ORDINAL_POSITION"]);
			Default				  = (string)SSDB.MyNull(row["COLUMN_DEFAULT"]);
			IsNullable			  = (string)SSDB.MyNull(row["IS_NULLABLE"]);
			DataType			  = (string)SSDB.MyNull(row["DATA_TYPE"]);
			CharMaxLength		  = (int?)SSDB.MyNull(row["CHARACTER_MAXIMUM_LENGTH"]);
			CharOctetLength		  = (int?)SSDB.MyNull(row["CHARACTER_OCTET_LENGTH"]);
			NumericPrecision	  = (byte?)SSDB.MyNull(row["NUMERIC_PRECISION"]);
			NumericPrecisionRadix = (short?)SSDB.MyNull(row["NUMERIC_PRECISION_RADIX"]);
			NumericPrecisionScale = (int?)SSDB.MyNull(row["NUMERIC_SCALE"]);
			DateTimePrecision	  = (short?)SSDB.MyNull(row["DATETIME_PRECISION"]);
			CharacterSetCatalog	  = (string)SSDB.MyNull(row["CHARACTER_SET_CATALOG"]);
			CharacterSetSchema	  = (string)SSDB.MyNull(row["CHARACTER_SET_SCHEMA"]);
			CharacterSetName	  = (string)SSDB.MyNull(row["CHARACTER_SET_NAME"]);
			CollationCatalog	  = (string)SSDB.MyNull(row["COLLATION_CATALOG"]);
		}

//---------------------------------------------------------------------------------------

		public static string DumpAsString(List<SSTableField> Fields) {
			StringBuilder	sb = new StringBuilder();
			sb.AppendLine("\t---------- FIELDS ----------");
			foreach (var field in Fields) {
				sb.AppendFormat("\t{0}\r\n", field.FieldName);
			}
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSTableIndex {
		public string				IndexName;
		List<SSTableIndexFields>	IndexFields;

//---------------------------------------------------------------------------------------

		public SSTableIndex(SqlConnection conn, DataRow row, string TableName) {
			IndexName = (string)SSDB.MyNull(row["index_name"]);
			// string[] restrictions = { null, null, TableName, null };
			string[] restrictions = { null, null, TableName, IndexName };
			DataTable dt = conn.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns,
								restrictions);
			IndexFields = new List<SSTableIndexFields>();
			foreach (DataRow ixRow in dt.Rows) {
				IndexFields.Add(new SSTableIndexFields(conn, ixRow));
			}
			// Sort them by position
			IndexFields.Sort((x, y) => {
				if (x.ColumnNumber < y.ColumnNumber)
					return -1;
				if (x.ColumnNumber > y.ColumnNumber)
					return 1;
				return 0;
			});
		}

//---------------------------------------------------------------------------------------

		public static string DumpAsString(List<SSTableIndex> Indexes) {
			if (Indexes.Count == 0) {
				return "";
			}
			StringBuilder	sb = new StringBuilder();
			sb.AppendLine("\t---------- INDEXES ----------");
			foreach (var ix in Indexes) {
				sb.AppendFormat("\t{0}\r\n", ix.IndexName);
				foreach (var ixfield in ix.IndexFields) {
					sb.AppendFormat("\t\t{0}, KeyType = {1}\r\n", ixfield.IndexFieldName,
						ixfield.KeyType);
				}
			}
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSTableIndexFields {
		public string	IndexFieldName;
		public int		ColumnNumber;
		public byte		KeyType;

//---------------------------------------------------------------------------------------

		public SSTableIndexFields(SqlConnection conn, DataRow row) {
			// This is the bottom level. Don't need <conn> here, but let's pass it in,
			// just in case.
			IndexFieldName	= (string)SSDB.MyNull(row["column_name"]);
			ColumnNumber	= (int)SSDB.MyNull(row["ordinal_position"]);
			KeyType			= (byte)SSDB.MyNull(row["KeyType"]);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSSprocs {
		public string	SprocName;
		public string	Schema;
		public string	ProcType;				// FUNCTION or PROCEDURE
		public string	ProcBody;				// Actual T-SQL text
		public DateTime	Created;
		public DateTime	LastUpdated;

		public List<SSSprocParms>	Parms;

//---------------------------------------------------------------------------------------

		public SSSprocs(SqlConnection conn, DataRow row) {
			SprocName	= (string)row["ROUTINE_NAME"];
			Schema		= (string)row["ROUTINE_SCHEMA"];
			ProcType	= (string)row["ROUTINE_TYPE"];
			Created		= (DateTime)row["CREATED"];
			LastUpdated = (DateTime)row["LAST_ALTERED"];

			GetParms(conn, SprocName);

			// There may well be a more efficient way of getting the source for
			// the sproc, but this will have to do for now.
			string SQL = "select ROUTINE_DEFINITION from INFORMATION_SCHEMA.ROUTINES Where ROUTINE_NAME='"
			+ SprocName + "'";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			ProcBody = (string)cmd.ExecuteScalar();
		}

//---------------------------------------------------------------------------------------

		private void GetParms(SqlConnection conn, string SprocName) {
#if false
			SqlCommand	cmd = new SqlCommand(SprocName, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlCommandBuilder.DeriveParameters(cmd);
			Parms = new List<SSSprocParms>();
			foreach (var parm in cmd.Parameters) {
				Parms.Add(new SSSprocParms(conn, parm.ToString()));
			}
#else
			string [] restrictions = { null, null, SprocName, null };
#if false
			DataTable dt2 = conn.GetSchema(SqlClientMetaDataCollectionNames.ProcedureColumns,
								restrictions);
#endif
			DataTable dt = conn.GetSchema("ProcedureParameters", restrictions);

			Parms = new List<SSSprocParms>();
			foreach (DataRow TableRow in dt.Rows) {
				Parms.Add(new SSSprocParms(conn, TableRow));
			}
			// Sort them by position
			Parms.Sort((x, y) => {
				if (x.OrdinalPosition < y.OrdinalPosition)
					return -1;
				if (x.OrdinalPosition > y.OrdinalPosition)
					return 1;
				return 0;
			});
#endif
		}

//---------------------------------------------------------------------------------------

		internal static string DumpAsString(List<SSSprocs> Sprocs) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("---------- STORED PROCEDURES ----------");
			foreach (var proc in Sprocs) {
				sb.AppendFormat("\t{0}.{1} - {2}\r\n", proc.Schema, proc.SprocName, proc.ProcType);
				sb.AppendFormat(SSSprocParms.DumpAsString(proc.Parms));

				// Add some indentation, just for show
				string body = "\t\t* " + proc.ProcBody.Replace("\n", "\n\t\t* ");
				sb.Append("\t****** Procedure Body ******\r\n" + body);
				sb.Append("\n\t****** End of Procedure Body ******\r\n");
			}
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SSSprocParms {
		public string	ParameterName;
		public string	SpecificCatalog;
		public string	SpecificSchema;	
		public string	SpecificName;
		public int		OrdinalPosition;
		public string	ParameterMode;
		public string	IsResult;
		public string	AsLocator;
		public string	DataType;
		public int?		CharacterMaximumLength;
		public int?		CharacterOctetLength;
		public string	CollationCatalog;
		public string	CollationSchema;	// Always null
		public string	CollationName;
		public string	CharacterSetCatalog;
		public string	CharacterSetSchema;	// Always null
		public string	CharacterSetName;
		public byte?	NumericPrecision;
		public short?	NumericPrecisionRadix;
		public int?		NumericScale;
		public short?	DatetimePrecision;
		// Fields reserved by SQL Server 2005 -- always null
		public string	IntervalType;
		public int?		IntervalPrecision;

//---------------------------------------------------------------------------------------

		public SSSprocParms(SqlConnection conn, DataRow row) {
			ParameterName			= (string)row["parameter_name"];
			SpecificCatalog			= (string)row["specific_catalog"];
			SpecificSchema			= (string)row["specific_schema"];
			SpecificName			= (string)row["specific_name"];
			OrdinalPosition			= (int)row["ordinal_position"];
			ParameterMode			= (string)row["parameter_mode"];
			IsResult				= (string)row["is_result"];
			AsLocator				= (string)row["as_locator"];
			DataType				= (string)row["data_type"];
			CharacterMaximumLength	= (int?)SSDB.MyNull(row["character_maximum_length"]);
			CharacterOctetLength	= (int?)SSDB.MyNull(row["character_octet_length"]);
			CollationCatalog		= (string)SSDB.MyNull(row["collation_catalog"]);
			CollationSchema			= (string)SSDB.MyNull(row["collation_schema"]);
			CollationName			= (string)SSDB.MyNull(row["collation_name"]);
			CharacterSetCatalog		= (string)SSDB.MyNull(row["character_set_catalog"]);
			CharacterSetSchema		= (string)SSDB.MyNull(row["character_set_schema"]);
			CharacterSetName		= (string)SSDB.MyNull(row["character_set_name"]);
			NumericPrecision		= (byte?)SSDB.MyNull(row["numeric_precision"]);
			NumericPrecisionRadix	= (short?)SSDB.MyNull(row["numeric_precision_radix"]);
			NumericScale			= (int?)SSDB.MyNull(row["numeric_scale"]);
			DatetimePrecision		= (short?)SSDB.MyNull(row["datetime_precision"]);
			IntervalType			= (string)SSDB.MyNull(row["interval_type"]);
			IntervalPrecision		= (short?)SSDB.MyNull(row["interval_precision"]);
		}

//---------------------------------------------------------------------------------------

		public static string DumpAsString(List<SSSprocParms> Parms) {
			StringBuilder sb = new StringBuilder();
			foreach (var parm in Parms) {
				sb.AppendFormat("\t\t{0}\r\n", parm.ParameterName);
			}
			return sb.ToString();
		}
	}

}