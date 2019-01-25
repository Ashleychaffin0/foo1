// #define RECREATE_DATABASE

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// TODO:	Need parm file to remember last UserID, Genre, MediaType
// TODO:	Figure out how to update the database schema without reconstructing everything
// TODO:	Maybe have an export / import facility (e.g. for db schema changes?)
// TODO:	Switch to a UWP app, so we can set Appointments for when to return things

// http://thedatafarm.com/data-access/poking-around-ef7s-solution/

// Misc pages. May or may not be relevant
// https://msdn.microsoft.com/en-us/library/ms130822.aspx
// http://www.connectionstrings.com/sql-server/
// https://channel9.msdn.com/Events/TechEd/NorthAmerica/2014/DEV-B417#fbid=
// https://msdn.microsoft.com/en-us/data/jj592674.aspx
// https://msdn.microsoft.com/en-us/data/jj193542.aspx
// http://www.codeproject.com/Tips/1072374/Entity-Framework-DBInterception-Exception
// https://msdn.microsoft.com/en-us/data/jj591620

// https://www.visualstudio.com/downloads/post-install-vs?campaign=ct!!414C841CC4D647ACBFE1940588BD78C1
// https://mva.microsoft.com/en-US/training-courses/developing-universal-windows-apps-with-c-and-xaml-8363?l=8pXSyBGz_3904984382
// https://air.mozilla.org/the-joy-of-coding-episode-42/
// http://www.codeproject.com/Articles/281656/Using-DescriptionAttribute-for-enumerations-bound
// http://blog.codeinside.eu/2016/01/31/working-with-fonticons-in-uwp/?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+Code-InsideBlogInternational+%28Code-Inside+Blog+International%29
// http://www.kunal-chowdhury.com/2016/02/uwp-tips-toast-button.html?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+kunal2383+%28Kunal%27s+Blog%29
// http://dotnetbyexample.blogspot.com/2016/01/using-style-replacing-behavior-for.html?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+blogspot%2Fdotnetbyexample+%28.NET+by+Example%29
// http://dailydotnettips.com/2016/01/29/changing-the-search-scope-to-find-and-replaces-files-in-visual-studio/
// http://dailydotnettips.com/2016/01/30/did-you-knowthere-are-multiple-find-results-window-in-visual-studio/
// http://dailydotnettips.com/2016/01/31/appending-search-resultsuse-same-find-result-window-for-multiple-search-in-visual-studio/
// https://alexanderzeitler.com/articles/rename-visual-studio-project-namespaces-and-folders-automate-everything/
// http://www.blackwasp.co.uk/VSSolutionFolders.aspx
// http://elbruno.com/2016/01/29/projectoxford-new-features-for-faceapi-beard-moustache-smile-detection-and-more/?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+elbruno+%28El+Bruno%29
// http://www.windowscentral.com/microsoft-may-be-preparing-release-its-hololens-development-kits
// http://www.infoq.com/news/2016/01/lattner-swift3-renamification?utm_campaign=infoq_content&utm_source=infoq&utm_medium=feed&utm_term=global
// https://channel9.msdn.com/Blogs/One-Dev-Minute/One-Dev-Question-with-Raymond-Chen-Why-Cant-I-Draw-on-the-Desktop-Anymore?WT.mc_id=DX_MVP4025064
// https://soundcloud.com/startalk/the-value-of-science-with-bri
// http://blog.ncover.com/net-project-inspirations/
// http://www.i-programmer.info/news/150-training-a-education/9398-obama-unveils-computer-science-for-all.html
// https://channel9.msdn.com/coding4fun/blog/Coding4Fun-January-2016-Round-Up?WT.mc_id=DX_MVP4025064
// https://channel9.msdn.com/Blogs/C9Team/Last-Week-on-Channel-9-January-25rd-January-31st-2016?WT.mc_id=DX_MVP4025064
// http://www.codeproject.com/Articles/1075487/Using-External-Config-Files-in-NET-Applications
// http://www.codeproject.com/Articles/1075381/Cplusplus-Tricks-to-Call-a-Slow-Function-With-A-Pr
// http://www.codeproject.com/Articles/1075173/Csharp-Feature-Proposal-Ref-Returns-and-Locals
// http://www.codeproject.com/Tips/1075543/SQL-Joins-in-LINQ
// https://blog.rendle.io/what-ive-learned-about-dotnet-native/
// http://serialseb.com/blog/2016/01/29/now-you-see-it-now-you-dont/
// https://alexandrebrisebois.wordpress.com/2016/01/28/i-hated-javascript/

/* Note:
SELECT TOP 1000 [Title],
				AuthorFirstName+' '+AuthorLastName,
				MediaName,
				GenreName,
				CONVERT( DATE, [DueDate], 101) AS [DueDate],
				[HasFullyRead],
				[IsAnthology],
				[StoppedAtPage],
				[Owner]
FROM [Book]
	 INNER JOIN Author ON Book.AuthorID = Author.ID
	 INNER JOIN Genre ON Book.GenreID = Genre.ID
	 INNER JOIN MediaType ON Book.MediumID = MediaType.ID
ORDER BY [DueDate] ASC;
	 INNER JOIN MediaType ON Book.MediumID = MediaType.ID;
*/

namespace LRSLibraryBooks {
	public partial class LRSLibraryBooks : Form {
		LibContext ctx;

		string ExportFilename = "LRSLibraryBooks_Backup";

//---------------------------------------------------------------------------------------

		public LRSLibraryBooks() {
			InitializeComponent();

			ctx = new LibContext();
#if RECREATE_DATABASE
			var res = MessageBox.Show("Delete database?", "LRS Library Books",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (res == DialogResult.Yes) {
				ctx.Database.EnsureDeleted();
			}
#endif
			ctx.Database.EnsureCreated();

#if RECREATE_DATABASE
			ctx.Genres.Add(new Genre("SciFi"));
			ctx.Genres.Add(new Genre("Mystery"));
			ctx.Genres.Add(new Genre("Humor"));
			ctx.Genres.Add(new Genre("Food"));
			ctx.Genres.Add(new Genre("Physics"));
			ctx.Genres.Add(new Genre("Math"));
			ctx.Genres.Add(new Genre("Science"));

			ctx.MediaTypes.Add(new MediaType("pBook"));
			ctx.MediaTypes.Add(new MediaType("eBook"));
			ctx.MediaTypes.Add(new MediaType("DVD"));
			ctx.MediaTypes.Add(new MediaType("Blu Ray"));

			ctx.Owners.Add(new UserID("LRS"));
			ctx.Owners.Add(new UserID("BGA"));

			ctx.SaveChanges();
#endif

			SetUserID();

			ShowGenres("");
			ShowMediaTypes("");
			ShowOwners(Environment.UserName);

			calDateDue.SetDate(DateTime.Now.AddDays(21));
		}

//---------------------------------------------------------------------------------------

		private void SetUserID() {
			string UserID = Environment.UserName;
			var owner = ctx.Owners.FirstOrDefault(n => n.OwnerName == UserID);
			if (owner == null) {
				owner = new UserID(Environment.UserName);
				ctx.Owners.Add(owner);
				ctx.SaveChanges();
				cmbOwner.Items.Add(UserID);
				;
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowGenres(string Name) {
			var qryGenres = from genre in ctx.Genres
							orderby genre.GenreName
							select genre.GenreName;

			var Names = qryGenres.ToArray();
			// Jump through a few hoops (here and below) if this is the first time we're
			// populating the combo box
			var NamesList = Names.ToList();
			if (NamesList.Count == 0) {
				return;
			}
			cmbGenres.DataSource = NamesList;
			if (cmbGenres.Items.Count == 0) {
				cmbGenres.Items.AddRange(Names);
				if (Names.Length > 0) {
					cmbGenres.SelectedIndex = 0;
				}
				return;
			}
			// Show the specified item (probably the one just added)
			var ix = NamesList.FindIndex(name => name == Name);
			cmbGenres.SelectedIndex = Math.Max(ix, 0);  // Show first if not found
		}

//---------------------------------------------------------------------------------------

		private void ShowMediaTypes(string Name) {
			// See comments in ShowGenres()
			var qryMedia = from mediatype in ctx.MediaTypes
						   orderby mediatype.MediaName
						   select mediatype.MediaName;
			var Names = qryMedia.ToArray();
			var NamesList = Names.ToList();
			if (NamesList.Count == 0) {
				return;
			}
			cmbMediaType.DataSource = NamesList;
			if (cmbMediaType.Items.Count == 0) {
				cmbMediaType.Items.AddRange(Names);
				if (Names.Length > 0) {
					cmbMediaType.SelectedIndex = 0;
				}
				return;
			}
			var ix = NamesList.FindIndex(name => name == Name);
			cmbMediaType.SelectedIndex = Math.Max(ix, 0);
		}

//---------------------------------------------------------------------------------------

		private void ShowOwners(string Name) {
			// See comments in ShowGenres()
			var qryOwners = from owner in ctx.Owners
							orderby owner.OwnerName
							select owner.OwnerName;
			var Names = qryOwners.ToArray();
			var NamesList = Names.ToList();
			if (NamesList.Count == 0) {
				return;
			}
			cmbOwner.DataSource = NamesList;
			if (cmbOwner.Items.Count == 0) {
				cmbOwner.Items.AddRange(Names);
				if (Names.Length > 0) {
					cmbOwner.SelectedIndex = 0;
				}
				return;
			}
			var ix = NamesList.FindIndex(name => name == Name);
			cmbOwner.SelectedIndex = Math.Max(ix, 0);   // Show first if not found
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			if (btnAdd.Text == "Add") {
				AddBook();
			} else {
				Book book = GetBookInfo();
				ctx.Update(book);
				ctx.SaveChanges();
				btnAdd.Text = "Add";
			}
		}

//---------------------------------------------------------------------------------------

		private void AddBook() {
			Book book = GetBookInfo();
			ctx.SaveChanges();
			MessageBox.Show("Item Added", "LRS Library Books");
		}

//---------------------------------------------------------------------------------------

		private Book GetBookInfo() {
			string GenreName = cmbGenres.SelectedItem.ToString();
			string MediaName = cmbMediaType.SelectedItem.ToString();
			var Auth = ctx.Authors.FirstOrDefault(a => (a.AuthorFirstName == txtAuthorFirstName.Text) && (a.AuthorLastName == txtAuthorLastName.Text));
			if (Auth == null) {
				Auth = new Author(txtAuthorFirstName.Text, txtAuthorLastName.Text);
				ctx.Authors.Add(Auth);
			}

#if true
			DateTime? DateDue;
			if (chkNotDue.Checked) {
				DateDue = null;
			} else {
				DateDue = calDateDue.SelectionStart;
			}
#else
			DateTime DateDue = calDateDue.SelectionStart;
#endif
			Book book = new Book {
				Author   = Auth,
				GenreID  = ctx.Genres.Single(g => g.GenreName == GenreName).ID,
				MediumID = ctx.MediaTypes.Single(m => m.MediaName == MediaName).ID,
				Owner    = cmbOwner.SelectedItem.ToString(),
				Title    = txtBookTitle.Text,
				DueDate  = DateDue
			};
			ctx.Add(book);
			return book;
		}

//---------------------------------------------------------------------------------------

		private void btnEdit_Click(object sender, EventArgs e) {
			if ((txtAuthorLastName.Text.Trim() == "") || (txtBookTitle.Text.Trim() == "")) {
				MessageBox.Show("Must have Author Last Name, and Book Title, non-blank", "LRS Library Books",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			btnAdd.Text = "Save";
		}

//---------------------------------------------------------------------------------------

		void AddAuthor(Author Name) {
			Name.AuthorFirstName = Name.AuthorFirstName.Trim();
			Name.AuthorLastName = Name.AuthorLastName.Trim();
			var qry = from a in ctx.Authors
					  where a.AuthorFirstName == Name.AuthorFirstName &&
							a.AuthorLastName == Name.AuthorLastName
					  select a;
			bool bExists = qry.Any();
			if (!bExists) {
				ctx.Add(Name);
			}
		}

//---------------------------------------------------------------------------------------

		void AddMediaType(string MediaName) {
			// TODO: Put this in the appropriate class
			AddMediaType(new MediaType(MediaName));
		}

//---------------------------------------------------------------------------------------

		void AddMediaType(MediaType med) {
			// TODO: Put this in the appropriate class
			med.MediaName = med.MediaName.Trim();
			var qry = from m in ctx.MediaTypes
					  where m.MediaName == med.MediaName
					  select m.MediaName;
			bool bExists = qry.Any();
			if (!bExists) {
				ctx.Add(med);
			}
		}

//---------------------------------------------------------------------------------------

		private void txtAuthorLastName_KeyPress(object sender, KeyPressEventArgs e) {
			int NameLenSoFar = txtAuthorLastName.Text.Length;
			string TargetName = (txtAuthorLastName.Text + e.KeyChar).ToUpper();
			var qry = from auth in ctx.Authors
					  where auth.AuthorLastName.ToUpper().StartsWith(TargetName)
					  orderby auth.AuthorLastName, auth.AuthorFirstName
					  select auth;

			try {
				var popAuthors = new ToolStripDropDown();
				popAuthors.ItemClicked += PopAuthors_ItemClicked;
				foreach (var auth in qry) {
					popAuthors.Items.Add(auth.AuthorLastName + ", " + auth.AuthorFirstName);
				}
				popAuthors.Show(txtAuthorLastName, new System.Drawing.Point(0, txtAuthorLastName.Height + 10));
			} catch (Exception ex) {
				Console.WriteLine(ex);          // TODO: Something better here
			}
		}

//---------------------------------------------------------------------------------------

		private void PopAuthors_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			var txt = e.ClickedItem.Text.Split(',');
			txtAuthorLastName.Text = txt[0].Trim();
			txtAuthorFirstName.Text = txt[1].Trim();

			DoSelectBooksByAuthor();
		}

//---------------------------------------------------------------------------------------

		private void DoSelectBooksByAuthor() {
			var qry = from Book in ctx.Books
					  join Auth in ctx.Authors on Book.Author.ID equals Auth.ID
					  where Auth.AuthorFirstName == txtAuthorFirstName.Text
						&& Auth.AuthorLastName == txtAuthorLastName.Text
					  select Book;
			var popBooks = new ToolStripDropDown();
			popBooks.ItemClicked += PopBooks_ItemClicked;
			foreach (Book b in qry) {
				popBooks.Items.Add(b.Title);
			}
			if (popBooks.Items.Count == 1) {
				txtBookTitle.Text = popBooks.Items[0].Text;
				return;
			}
			popBooks.Show(txtBookTitle, new System.Drawing.Point(0, txtBookTitle.Height + 10));
		}

//---------------------------------------------------------------------------------------

		private void PopBooks_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			txtBookTitle.Text = e.ClickedItem.Text;
		}

//---------------------------------------------------------------------------------------

		private void btnNewGenre_Click(object sender, EventArgs e) {
			var dlg = new PromptForString("Genre Name");
			var res = dlg.ShowDialog();
			if (res == DialogResult.Cancel) {
				return;
			}
			bool bAdded = Genre.Add(ctx, dlg.NewName);
			if (bAdded) {
				ShowGenres(dlg.NewName);
			} else {
				MessageBox.Show("Specified genre already exists", "LRS Library Books",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnNewMediaType_Click(object sender, EventArgs e) {
			var dlg = new PromptForString("Media Type Name");
			var res = dlg.ShowDialog();
			if (res == DialogResult.Cancel) {
				return;
			}
			bool bAdded = MediaType.Add(ctx, dlg.NewName);
			if (bAdded) {
				ShowMediaTypes(dlg.NewName);
			} else {
				MessageBox.Show("Specified media type already exists", "LRS Library Books",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		
//---------------------------------------------------------------------------------------

		private void btnNewOwner_Click(object sender, EventArgs e) {
			var dlg = new PromptForString("Owner Name");
			var res = dlg.ShowDialog();
			if (res == DialogResult.Cancel) {
				return;
			}
			bool bAdded = UserID.Add(ctx, dlg.NewName);
			if (bAdded) {
				ShowOwners(dlg.NewName);
			} else {
				MessageBox.Show("Specified owner already exists", "LRS Library Books",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnNewItem_Click(object sender, EventArgs e) {
			txtAuthorFirstName.Text = "";
			txtAuthorLastName.Text = "";
			txtBookTitle.Text = "";
		}

//---------------------------------------------------------------------------------------

		private void LRSLibraryBooks_Load(object sender, EventArgs e) {
			this.reportViewer1.RefreshReport();
		}

//---------------------------------------------------------------------------------------

		private void Export() {
			using (var sw = new StreamWriter(ExportFilename)) {
				// Export Genres
				sw.WriteLine(")Genres");
				var qryGenres = from g in ctx.Genres
								orderby g.ID
								select g;
				foreach (var gen in qryGenres) {
					sw.WriteLine($"{gen.ID},{gen.GenreName}");
				}
				// Export MediaTypes
				sw.WriteLine(")MediaTypes");
				var qryMediaTypes = from mt in ctx.MediaTypes
									orderby mt.ID
									select mt;
				foreach (var mt in qryMediaTypes) {
					sw.WriteLine($"{mt.ID},{mt.MediaName}");
				}
				// Export Authors
				sw.WriteLine(")Authors");
				var qryAuthors = from a in ctx.Authors
								 orderby a.ID
								 select a;
				foreach (var a in qryAuthors) {
					sw.WriteLine($"{a.ID},{a.AuthorFirstName},{a.AuthorLastName}");
				}
				// Export Books
				sw.WriteLine(")Books");
				var qryBooks = from b in ctx.Books
								 orderby b.BookID
								 select b;
				foreach (var b in qryBooks) {
					sw.WriteLine($"{b.BookID},{b.Author.ID},{b.DueDate},{b.GenreID},{b.HasFullyRead},{b.IsAnthology},{b.MediumID},{b.Owner},{b.StoppedAtPage},{b.Title}");
				}
			}
			MessageBox.Show("Export done", "LRS Library Books");
		}

//---------------------------------------------------------------------------------------

		private void Import() {
			using (var sr = new StreamReader(ExportFilename)) {
				string line;
				while ((line = sr.ReadLine()) != null) {

				}
			}
		}

//---------------------------------------------------------------------------------------

		private void tabPage2_Enter(object sender, EventArgs e) {
			// reportViewer1.con
		}

//---------------------------------------------------------------------------------------

		private void btnExport_Click(object sender, EventArgs e) {
			Export();
		}
	}
}
