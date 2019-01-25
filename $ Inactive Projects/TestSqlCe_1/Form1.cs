using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
// using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

// TODO: http://blogs.msdn.com/matt/archive/2008/09/09/sql-ce-3-5-with-linq-to-sql.aspx
// TODO: http://blogs.msdn.com/matt/archive/2008/09/26/sql-ce-3-5-with-linq-to-sql-revisited.aspx
// TODO: http://www.codeproject.com/KB/linq/Compact_LINQ.aspx

// Note: We manually used SqlMetal against the .sdf file to manually create the .dbml
//		 file, then added the .dbml file to the project. The IDE took care of the rest
//		 of the details.

namespace TestSqlCe_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			
		}

		private void btnGo_Click(object sender, EventArgs e) {
			using (var dc = new TestSqlCeDatabase(@"Data Source=C:\LRS\TestSqlCeDatabase.sdf")) {
				var q1 = from t in dc.TblCeAlbums2 
						 select t;
				int cnt = q1.Count();
				Console.WriteLine("Count = {0}", cnt);

				Table<TblCeAlbums2> CeAlbum = dc.GetTable<TblCeAlbums2>();
				TblCeAlbums2 a1 = new TblCeAlbums2() { ArtistName = "A2" };
				CeAlbum.InsertOnSubmit(a1);
				TblCeAlbums2 a2 = new TblCeAlbums2() { ArtistName = "B2" };
				CeAlbum.InsertOnSubmit(a2);
				dc.SubmitChanges();
			}
		}
	}
}
