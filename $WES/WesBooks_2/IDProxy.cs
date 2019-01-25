#if false
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks_2 {
	class IDProxy<T> {
		public int		ID;
		public string	Name;

//---------------------------------------------------------------------------------------

		protected static string TableName;
		protected static string FieldName;

		public override string ToString() {
			// return base.ToString();)
			return Name;
		}


//---------------------------------------------------------------------------------------

		public void Fill(OleDbDataReader rdr) {
			ID   = (int)   rdr[FieldName + "ID"];	// We'll make the convention that the
													// ID field is the FieldName + "ID"
			Name = (string)rdr[FieldName];
		}


//---------------------------------------------------------------------------------------

		public static List<T> ReadAll(LRSAccessDatabase db) {
			var SQL = @"
				SELECT * FROM " + TableName + " ORDER BY " + FieldName;
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var Values = new List<T>();
			while (rdr.Read()) {
				T val = new T();
				val.Fill(rdr);
				Values.Add(val);
			}
			return Values;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class tblGenres_New : IDProxy {

		static tblGenres_New() {
			TableName = "tblGenres";
			FieldName = "Genre";
		}
	}

	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	class tblPublishers_New : IDProxy {

		static tblPublishers_New() {
			TableName = "tblPublishers";
			FieldName = "Publisher";
		}
	}
}
#endif