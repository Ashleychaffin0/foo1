// TODO: Worry about Edit / Delete later
// TODO: Enter key in txtbox == Add
// TODO: Rescan items to update keywords

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace WinFormsChannel9 {
	public partial class EditKeywords : Form {

		SqlCeConnection conn;

		 Dictionary<string, int> KeywordDict;

//---------------------------------------------------------------------------------------

		public EditKeywords(SqlCeConnection conn, Dictionary<string, int> KeywordDict) {
			InitializeComponent();

			this.conn = conn;
			this.KeywordDict = KeywordDict;
		}

//---------------------------------------------------------------------------------------

		private void EditKeywords_Load(object sender, EventArgs e) {
			FillKeywordsListBox();
		}

//---------------------------------------------------------------------------------------

		private void FillKeywordsListBox() {
			lbKeywords.Items.Clear();
			var keys = from key in KeywordDict.Keys
					   orderby key
					   select key;
			lbKeywords.Items.AddRange(keys.ToArray());
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			// Do a bit of error checking
			string keyword = txtKeyword.Text.Trim();
			if (keyword.Length == 0) {
				MessageBox.Show("Please enter a keyword");
				return;
			}
			// TODO: Case insensitive comparison would be nice here, else we might get
			//		 LINQ and Linq
			if (KeywordDict.ContainsKey(keyword)) {
				MessageBox.Show("Keyword already exists");
				return;
			}
			
			int ID = InsertKeywordIntoDatabase(keyword);

			// Add to the dictionary
			KeywordDict[keyword] = ID;

			// Rescan all items to see what items match this new keyword

			UpdateKeywords(conn, keyword, ID);

			txtKeyword.Text = "";
			FillKeywordsListBox();
		}

//---------------------------------------------------------------------------------------

		// Insert the keyword into the database and get its ID
		private int InsertKeywordIntoDatabase(string keyword) {
			string SQL = "INSERT INTO tblKeywords(Keyword) VALUES(@Keyword)";
			int ID;
			using (var cmd = new SqlCeCommand(SQL, conn)) {
				cmd.Parameters.AddWithValue("@Keyword", keyword);
				cmd.ExecuteNonQuery();

				string SQL2 = "SELECT @@IDENTITY";
				using (var cmd2 = new SqlCeCommand(SQL2, conn)) {
					object o = cmd2.ExecuteScalar();
					ID = Convert.ToInt32(o);
				}
			}
			return ID;
		}

//---------------------------------------------------------------------------------------

		private void UpdateKeywords(SqlCeConnection conn, string keyword, int ID) {
			string SQL = "SELECT VideoID, Title, Description FROM tblVideos";
			using (var cmd = new SqlCeCommand(SQL, conn)) {
				using (var rdr = cmd.ExecuteReader()) {
					while (rdr.Read()) {
						int VideoID        = (int) rdr["VideoID"];
						string Title       = (string) rdr["Title"];
						string Description = (string) rdr["Description"];
						// OK, we have the information on an entry in tblVideos. Look for
						// the keyword and, if found, add the entry to tblVideoKeywords
						List<string> TitleKeys = Keywords.ScanForKeywords(Title);
						List<string> DescKeys  = Keywords.ScanForKeywords(Title);
						foreach (string key in TitleKeys) {
							// InsertKeywordIntoDatabase(conn, VideoID, )
						}
						foreach (string key in DescKeys) {
							// InsertKeywordIntoDatabase(conn, VideoID, )
						}
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		// TODO: Delete this. Just copied over for reference
		private void xxxxxxxxxxxxxxxxxxInsertKeywords(Channel9Video vid, int VideoID) {
			string SQL = "INSERT INTO tblVideoKeywords(VideoID, KeywordID)"
				+ " VALUES(@VideoID, @KeywordID)";
			foreach (var KeyWord in vid.KeywordArray) {
				int KeywordID = KeywordDict[KeyWord];
				using (var cmd = new SqlCeCommand(SQL, conn)) {
					cmd.Parameters.AddWithValue("@VideoID", VideoID);
					cmd.Parameters.AddWithValue("@KeywordID", KeywordID);
					int n = cmd.ExecuteNonQuery();
				}
			}
		}

	}
}
