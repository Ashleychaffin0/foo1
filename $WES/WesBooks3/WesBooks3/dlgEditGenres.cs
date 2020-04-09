using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRSUtils.Database;

// Note: This code doesn't handle the following situation:
//		 The user selects a Genre, edits it, then either closes the form or clicks on the
//		 Listbox to select another Genre. His edit of the Genre name will be lost. We
//		 need a "dirty" flag.

namespace WesBooks3 {
	public partial class dlgEditGenres : Form {

		LRSAccessDatabase db;

		// This field is essentially non-functional, but is included here to show how
		// the invoker of the dialog can access data, even after the dialog has been
		// closed. But the dialog (Form) *object* is still there.

		// More realistically, if you were prompting the user for information (e.g Name
		// and Address), you could reference that data (the true "result" of the dialog)
		// after the dialog had closed.
		public int NumberOfApplies = 0;

//---------------------------------------------------------------------------------------

		public dlgEditGenres(LRSAccessDatabase db) {
			InitializeComponent();

			this.db = db;
		}

//---------------------------------------------------------------------------------------

		private void dlgEditGenres_Load(object sender, EventArgs e) {
			var Genres = tblGenres.ReadAll(db);
			lbGenres.DataSource = Genres;

			lbGenres.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void lbGenres_SelectedValueChanged(object sender, EventArgs e) {
			var Genre    = (tblGenres)lbGenres.SelectedItem;
			txtName.Text = Genre.Genre;
			txtID.Text   = Genre.GenreID.ToString();
		}

//---------------------------------------------------------------------------------------

		private void btnApply_Click(object sender, EventArgs e) {
			// OK, we have new text in txtName, and the GenreID in lb.SelectedItem
			// We also have a reference to LRSAccessDatabase.
			// Write the SQL to UPDATE the database
			// TODO:
			++NumberOfApplies;

		}
	}
}
