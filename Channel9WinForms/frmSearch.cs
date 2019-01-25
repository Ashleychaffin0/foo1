using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlServerCe;
using System.Windows.Forms;

namespace WinFormsChannel9 {
	public partial class frmSearch : Form {
		
		SqlCeConnection conn;

		Dictionary<string, int> KeywordDict;
		KeyValuePair<string, int> Dict2;			// TODO:

//---------------------------------------------------------------------------------------

		public frmSearch(SqlCeConnection conn) {
			InitializeComponent();

			this.conn = conn;
		}

//---------------------------------------------------------------------------------------

		private void frmSearch_Load(object sender, EventArgs e) {
			KeywordDict = Keywords.GetKeywordsFromDatabase(conn);

			var keys = from key in KeywordDict.Keys orderby key select key;
			foreach (string key in keys) {
				lbAllKeywords.Items.Add(new KeywordIDs(key, KeywordDict[key]));
			}
#if false
			lbAllKeywords.DataSource = Keys;

			Dict2 = new KeyValuePair<string,int>();
			lbAllKeywords.DataSource = Dict2;
#endif

			cbDateSortDirection.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			if (lbAllKeywords.SelectedIndex < 0) {
				MessageBox.Show("No keyword selected. Ignored.", "C9", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			lbKeywordsToSearch.Items.Add(lbAllKeywords.SelectedItem);
			// TODO: Remove from left listbox
		}

//---------------------------------------------------------------------------------------

		private void btnRemove_Click(object sender, EventArgs e) {
			if (lbKeywordsToSearch.SelectedIndex < 0) {
				MessageBox.Show("No keyword selected. Ignored.", "C9", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			// TODO: Add back to left box
			lbKeywordsToSearch.Items.RemoveAt(lbKeywordsToSearch.SelectedIndex);
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			if (lbKeywordsToSearch.Items.Count == 0) {
				MessageBox.Show("No keywords selected. Choose one or more");
				return;
			}

			string SQL = @"
SELECT * FROM tblVideos 
				INNER JOIN tblVideoKeywords 
				ON tblVideos.VideoID = tblVideoKeywords.VideoID
WHERE	tblVideoKeywords.KeywordID IN (";

			bool bAddComma = false;
			foreach (KeywordIDs key in lbKeywordsToSearch.Items) {
				if (bAddComma) {
					SQL += ", ";
				}
				bAddComma = true;
				SQL += key.KeywordID;
			}
			SQL += ") ORDER BY [Date]";

			if (cbDateSortDirection.SelectedItem == "Desc") {
				SQL += " DESC";
			}

			dataGridView1.Rows.Clear();

			string		Date;
			string		Title;
			string		Description;
			string		Link;
			using (var cmd = new SqlCeCommand(SQL, conn)) {
				using (var rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						Date        = ((DateTime) rdr["Date"]).ToShortDateString();
						Title       = (string) rdr["Title"];
						Description = (string) rdr["Description"];
						Link        = (string) rdr["Link"];
						dataGridView1.Rows.Add(Date, Title, Description, Link);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e) {
			var row = dataGridView1.Rows[e.RowIndex];
			string link = (string) row.Cells[3].Value;
			web2.Navigate(link);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class KeywordIDs {
		public string Keyword;
		public int KeywordID;

//---------------------------------------------------------------------------------------

		public KeywordIDs(string Keyword, int KeywordID) {
			this.Keyword   = Keyword;
			this.KeywordID = KeywordID;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Keyword;
		}
	}
}
