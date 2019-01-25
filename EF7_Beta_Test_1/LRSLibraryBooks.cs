using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Data.Entity;

// http://www.codeproject.com/Articles/753510/SocialClub-A-Sample-application-using-Csharp-NET-E
// https://msdn.microsoft.com/en-us/data/jj193542.aspx
// http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application

namespace LRSLibraryBooks {
	public partial class LRSLibraryBooks : Form {
		public LRSLibraryBooks() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			var a1 = new Author { AuthorName = "Heinlein" };
			var b1 = new Book { Title = "Glory Road", Genre = Book.GenreType.SciFi };

			// var sp = new serv
			using (var ctx = new LibContext()) {
				ctx.Database.EnsureDeleted();
				ctx.Database.EnsureCreated();

				ctx.Authors.Add(a1);
				ctx.Books.Add(b1);
				ctx.SaveChanges();
				MessageBox.Show("Done");
			}
		}
	}	
}
