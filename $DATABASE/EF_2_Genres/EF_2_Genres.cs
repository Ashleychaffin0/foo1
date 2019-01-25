using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO: Maybe some of these links are relevant
//	http://stackoverflow.com/questions/2614941/unique-keys-in-entity-framework-4

namespace EF_2_Genres {
	public partial class EF_2_Genres : Form {
		public EF_2_Genres() {
			InitializeComponent();

			Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
		}

		private void EF_2_Genres_Load(object sender, EventArgs e) {

			var gSciFi = new Genre { GenreName = "Sci Fi" };

			using (var ctx = new Context()) {
				var bk1 = new Book { Author = "RAH", Genre = gSciFi };
				ctx.Books.Add(bk1);
				var bk2 = new Book { Author = "Ike", Genre = gSciFi };
				ctx.Books.Add(bk2);
				ctx.SaveChanges();
			}
		}
	}

	class Book {
		public int		BookID { get; set; }
		public string	Author { get; set; }
		public int		GenreID { get; set; }
		[ForeignKey("GenreID")]
		public Genre	Genre  { get; set; }
	}

	 class Genre {
		public int		GenreID { get; set; }
		[Index(IsUnique = true)]
		[MaxLength(30)]
		public string	GenreName { get; set; }
		public virtual ICollection<Book>	Books { get; set; }
	}
}
