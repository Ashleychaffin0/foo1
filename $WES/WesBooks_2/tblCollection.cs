using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks_2 {
	class tblCollections {

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			var db = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblCollections";
			var OK = qry.ExecuteNonQuery(SQL);
			SQL = @"
				CREATE TABLE tblCollections(
					Collection	IDENTITY PRIMARY KEY,
					BookID		INTEGER,
					Title		CHAR(60),
					AuthorID	INTEGER
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}
