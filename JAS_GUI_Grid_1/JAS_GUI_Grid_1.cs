using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JAS_GUI_Grid_1 {
	public partial class JAS_GUI_Grid_1 : Form {

		MyAlbum [] albums = { 
			// List<MyAlbum> albums = new List<MyAlbum> { 
				new MyAlbum("Yardbirds", "Roger the Engineer", "Rock", new DateTime(2005, 3, 12)),
				new MyAlbum("Yardbirds", "Birdland", "Rock", new DateTime(2002, 1, 23)),
				new MyAlbum("Archie Fisher", "The Man with a Rhyme", "Folk", new DateTime(1994, 5, 4)),
				new MyAlbum("Beatles", "Rubber Soul", "Rock", new DateTime(2001, 6, 5)),
				new MyAlbum("John Fahey", "Christmas Album", "Holiday", new DateTime(1990, 11, 16)),
				new MyAlbum("Aly Bain & Phil Cunningham", "The Pearl", "Celtic", new DateTime(2005, 3, 12))
			};

		MyBook [] books = {
		  new MyBook("Larry Niven", "Ringworld", "SF", 9.95f),
		  new MyBook("Larry Niven", "World of Ptavvs", "SF", 9.95f),
		  new MyBook("Rex Stout", "Some Buried Caesar", "Mystery", 6.95f),
		  new MyBook("P.G. Wodehouse", "Leave it to PSmith", "Humor", 12.95f),
		  new MyBook("Robert Heinlein", "Space Cadet", "SF", 7.50f)
		};

//---------------------------------------------------------------------------------------

		public JAS_GUI_Grid_1() {
			InitializeComponent();

			bindingSource1.DataSource = albums;
		}

//---------------------------------------------------------------------------------------

		private void btnData1_Click(object sender, EventArgs e) {
			grid.DataSource = albums;
			grid.Columns["WhenBought"].HeaderText = "When Bought";
			grid2.DataSource = albums;
		}

//---------------------------------------------------------------------------------------

		private void btnData2_Click(object sender, EventArgs e) {
			grid.DataSource = books;
			grid.Sort(grid.Columns[1], ListSortDirection.Ascending);
			grid2.DataSource = books;
		}

//---------------------------------------------------------------------------------------

		private void btnSetStyle1_Click(object sender, EventArgs e) {
			grid.RowsDefaultCellStyle.BackColor = Color.White;
			grid.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleGreen;
		}

//---------------------------------------------------------------------------------------

		private void btnSetStyle2_Click(object sender, EventArgs e) {
			grid.RowsDefaultCellStyle.BackColor = Color.Aqua;
			grid.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class MyAlbum {
		// Note: These *must* be properties, not mere variables. The DataGridView will
		//		 pick up properties for columns, not variables.
		public string Artist {get; set;}
		public string Album {get; set;}
		public string Genre {get; set;}
		public DateTime WhenBought { get; set; }

//---------------------------------------------------------------------------------------

		public MyAlbum(string Artist, string Album, string Genre, DateTime WhenBought) {
			this.Artist = Artist;
			this.Album = Album;
			this.Genre = Genre;
			this.WhenBought = WhenBought;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class MyBook {
		// Note: These *must* be properties, not mere variables. The DataGridView will
		//		 pick up properties for columns, not variables.
		public string Author { get; set; }
		public string Title { get; set; }
		public string Genre { get; set; }
		public float Price { get; set; }

//---------------------------------------------------------------------------------------

		public MyBook(string Author, string Title, string Genre, float Price) {
			this.Author = Author;
			this.Title = Title;
			this.Genre = Genre;
			this.Price = Price;
		}
	}
}
