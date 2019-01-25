
// http://msdn.microsoft.com/en-us/library/system.data.oledb.oledbconnection.getoledbschematable(v=vs.110).aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRSUtils.Database;

namespace Class_ify_Access_Table {
	public partial class Class_ify_Access_Table : Form {
		public Class_ify_Access_Table() {
			InitializeComponent();

			string dbName = @"D:\LRS\Devel\C#-2013\WesBooks3\WesBooks3\WesBooks3.mdb";
			var db = new LRSAccessDatabase(dbName);

			var MyList = new List<int> { 5, 3, 12, 6 };
			l.Sort(delegate(int x, int y {
				if (x < y) return -1;
				if (x == y) return 0;
				return 1;
			}
					);

			CreateNewClasses(db);
		}

//---------------------------------------------------------------------------------------

		private void CreateNewClasses(LRSAccessDatabase db) {
			var restrictions = new string[4] { null, null, null, "Table" };
			// Note: Restrictions are in the order {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE} 
			//		 See http://support.microsoft.com/kb/309488
			//		 See also https://www.daniweb.com/software-development/vbnet/threads/373942/getoledbschematable-data_types
			var tbls = db.Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
			for (int i = 0; i < tbls.Rows.Count; ++i) {
				var dr = tbls.Rows[i];
				string TableName = (string) dr["TABLE_NAME"];
				CreateClass(db, TableName);
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateClass(LRSAccessDatabase db, string tableName) {
			var restrictions = new string[4] { null, null, tableName, null };
			var Cols         = db.Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);

			var ColumnInfos = new List<ColumnInfo>();

			foreach (DataRow col in Cols.Rows) {
				ColumnInfos.Add(new ColumnInfo(col));
			}

			//if (db != null) return;			// TODO:
			string Filename = tableName + "_Generated.cs";
		}

//---------------------------------------------------------------------------------------

		private ColumnInfo.DataType GetDataType(int DataType) {
			switch (DataType) {
			case 2:
				return ColumnInfo.DataType.Integer;
			case 3:
				break;			// TODO:
			case 4:
				return ColumnInfo.DataType.Float;
			case 5:
				return ColumnInfo.DataType.Double;
			case 6:
				return ColumnInfo.DataType.Currency;
			case 7:
				return ColumnInfo.DataType.DateTime;
			case 11:
				return ColumnInfo.DataType.Bit;
			case 17:
				return ColumnInfo.DataType.Byte;
			case 72:
				return ColumnInfo.DataType.Memo;
			case 128:
				break;			// TODO:
			case 130:
				break;			// TODO:
			case 131:
				break;			// TODO:
			}
			return ColumnInfo.DataType.Unknown;		// TODO:
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class ColumnInfo {
		public string	FieldName;
		public DataType FieldType;
		public long		OrdinalPosition;
		public long		Flags;
		public int		Data_Type;
		public long?	CharMaxLength;
		public bool		IsNullable;
		// private DataRow col;

//---------------------------------------------------------------------------------------

		public ColumnInfo(DataRow col) {
			// this.col = col;
			for (int i = 0; i < col.ItemArray.Length; i++) {
				string ColumnCaption = col.Table.Columns[i].Caption;
				object Value = col.ItemArray[i];
				Console.WriteLine("Row[{0}] = {1} / {2}", i, ColumnCaption, Value);
				switch (ColumnCaption) {
				case "COLUMN_NAME":
					FieldName = (string)Value;
					break;
				case "ORDINAL_POSITION":
					OrdinalPosition = (long)Value;
					break;
				case "DATA_TYPE":
					Data_Type = (int)Value;
					break;
				case "CHARACTER_MAXIMUM_LENGTH":
					// CharMaxLength = (Value is DBNull) ? null : (long)Value;
					if (Value is DBNull) {
						CharMaxLength = null;
					} else {
						CharMaxLength = (long)Value;
					}
					break;
				case "COLUMN_FLAGS":
					Flags = (long)Value;
					break;
				case "IS_NULLABLE":
					IsNullable = (bool)Value;
					break;
				default:
					break;
				}
			}
			Console.WriteLine("======================================");
		}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum DataType {
			Unknown,
			Integer,
			String,
			VarChar,
			Float,
			Double,
			Decimal,
			Currency,
			DateTime,
			Bit,
			Byte,
			Memo
		}
	}
}
