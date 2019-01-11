// #define EMPTY_TBLVIDEOKEYWORDS
// #define EMPTY_TBLVIDEOS
// #define SET_INDEX_ON_TBLEVIDEOS_1


// TODO: Use the following query to query the database
#if false
SELECT tblVideos.VideoID, Date, Keyword, Title, Description, Link, ElapsedTime FROM tblVideos INNER JOIN tblVideoKeywords ON tblVideos.VideoID = tblVideoKeywords.VideoID
INNER JOIN tblKeywords ON tblVideoKeywords.KeywordID = tblKeywords.KeywordID
WHERE Keyword = 'Anders'
ORDER BY tblVideos.Videoid
#endif

// TODO: Use the following query to empty (most of) the tables.
#if false
delete from tblvideokeywords;
delete from tblvideos;
#endif

// TODO: Don't restart every bloody time

using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormsChannel9 {
	public partial class WinFormsChannel9 : Form {

		string UrlBase = "http://channel9.msdn.com/Browse/AllContent?page=";

		string DatabaseName = @"C:\lrs\LRSChannel9.sdf";

		List<Channel9Video>	Videos;

		Channel9Video		Video;

		SqlCeConnection		conn;

		Dictionary<string, int>	KeywordDict;

		bool				StopFlag;

		int					SqlErrorDuplicateRecordCode = 25016;

		Stopwatch			sw;

//---------------------------------------------------------------------------------------

		public WinFormsChannel9() {
			InitializeComponent();

			Videos = new List<Channel9Video>();
			Video  = null;

			grid1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			grid1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

#if false		// Upgrades from SSCE 3.5 to 4.0. Careful...
			var engine = new SqlCeEngine("Data Source=" + DatabaseName);
			engine.Upgrade();
#endif

			KeywordDict = new Dictionary<string,int>();

			SetDatabaseConnection();

			SetKeywords();
		}

//---------------------------------------------------------------------------------------

#if false
		// Example code of how to use DDL to set index
		private void SetIndex() {
			string SQL = "CREATE UNIQUE INDEX IX_TITLE_DESC ON tblVideos(Title, Description)";
			using (var cmd = new SqlCeCommand(SQL, conn)) {
				cmd.ExecuteNonQuery();
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private void SetDatabaseConnection() {
			conn = new SqlCeConnection("Data Source=" + DatabaseName);
			conn.Open();
		}

//---------------------------------------------------------------------------------------

		private void SetKeywords() {
			var KeywordsFromDatabase = new List<string>();
			string SQL = "SELECT Keyword, KeywordID FROM tblKeywords";
			var cmd = new SqlCeCommand(SQL, conn);
			using (var rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					string Keyword = (string)rdr["Keyword"];
					KeywordsFromDatabase.Add(Keyword);
					int KeywordID = Convert.ToInt32(rdr["KeywordID"]);
					KeywordDict[Keyword] = KeywordID;
				}
			}

			Keywords.SetKeywords(KeywordsFromDatabase);

#if false		// Used to recreate the contents of tblKeywords
			var sb = new StringBuilder();
			string fmt = "\nINSERT INTO tblKeywords(Keyword) VALUES('{0}');";
			foreach (var item in Keywords) {
				sb.AppendFormat(fmt, item);
			}
			Clipboard.SetText(sb.ToString());
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			grid1.Visible = false;
			web1.Visible  = true;
			StopFlag      = false;

			sw.Reset();
			sw.Start();

			GetPage();
		}

//---------------------------------------------------------------------------------------

		private void btnStop_Click(object sender, EventArgs e) {
			StopFlag = true;
		}

//---------------------------------------------------------------------------------------

		private void GetPage() {
			// TODO: statusStrip1 not working
			statusStrip1.Text = "Processing page " + PageNoStart.Value;
			Application.DoEvents();
			string url = UrlBase + PageNoStart.Value;
			web1.Navigate(url);
		}

//---------------------------------------------------------------------------------------

		private void web1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			var doc = (sender as WebBrowser).Document;
			var body = doc.Body as HtmlElement;
			// Do the following only if PageNoEnd == 0, and only do it if we're on the
			// first page (don't do this for every page, since presumably the result
			// will be (hopefully) the same on all pages)
			if ((Convert.ToInt32(PageNoEnd.Value) == 0) && (Convert.ToInt32(PageNoStart.Value) == 1)) {
				// Try to figure out how many pages there are
				var uls = body.GetElementsByTagName("ul");
				Console.WriteLine("{0} <ul>'s found", uls.Count);
				foreach (HtmlElement ul in uls) {
					string UlClassName = ul.GetAttribute("className");
					if (UlClassName == "paging") {
						SetLastPage(ul);
						break;
					}
				}
			}
			var divs = body.GetElementsByTagName("div");
			Console.WriteLine("{0} <div>'s found", divs.Count);
			try {
				foreach (var div in divs) {
					ProcessDiv(div as HtmlElement);
				}
			} catch (Exception ex) {
				// TODO: Figure out what to do here
			}

			lblElapsed.Text = string.Format(@"Elapsed: {0:h\:mm\:ss}", sw.Elapsed);
			
			try {
				if (StopFlag || (PageNoStart.Value >= PageNoEnd.Value)) {
					CleanupAndGoHome();
					return;
				}
				PageNoStart.UpButton();
				GetPage();
			} catch (Exception ex) {
				// Presumably ran out of pages to process
				return;
			}
		}

//---------------------------------------------------------------------------------------

		private void SetLastPage(HtmlElement ul) {
			var lis = ul.GetElementsByTagName("li");
			int PageNo;
			int MaxPage = 0;
			foreach (HtmlElement li in lis) {
				bool bOK = int.TryParse(li.InnerText, out PageNo);
				if (bOK) {
					if (PageNo > MaxPage) {
						MaxPage = PageNo;
					}
				}
			}
			if (MaxPage > 0) {
				PageNoEnd.Value = MaxPage;
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessDiv(HtmlElement /* IHTMLDivElement */ DivBase) {
			// var div = DivBase.DomElement as IHTMLDOMNode;	// DomNode2
			// var div = DivBase.DomElement as IHTMLDivElement;
			// dynamic div = DivBase.DomElement as HTMLDivElementClass;
			// var div = DivBase.DomElement as mshtml.HTMLDivElementClass;
			// var attrs = 
			// var div2 = DivBase as HTMLDivElementc;
			string ClassName = DivBase.GetAttribute("className");
			switch (ClassName) {
			case "entry-image":
				if (Video != null) {
					Videos.Add(Video);
				}
				Video = new Channel9Video((int)PageNoStart.Value);
				var a_s = DivBase.GetElementsByTagName("A");
				Video.Link = a_s[0].GetAttribute("href");
				// Console.WriteLine("\n\n");
				break;
			case "entry-caption":
				Video.TimeCaption = DivBase.InnerText;
				// Console.WriteLine("Caption - {0}", DivBase.InnerText);
				break;
			case "entry-meta":
				Video.Title = DivBase.Children[0].InnerText;
				// Console.WriteLine(DivBase.Children[0].InnerText);
				break;
			case "description":
				Video.Description = DivBase.InnerText;
				// Console.WriteLine("Descrption - {0}", DivBase.InnerText);
				break;
			case "data":
				for (int i = 0; i < DivBase.Children.Count; i++) {
					var kid = DivBase.Children[i];
					string SpanClass = kid.GetAttribute("className");
					if ((kid.TagName == "SPAN") && (SpanClass == "date")) {
						try {
							Video.ArticleDate = GetArticleDate(kid.InnerText);
						} catch (Exception ex) {
							break;
						}
					}
				}
				break;
			default:
				return;				
			}
#if false
			var attrs = div.attributes;
			foreach (var attr in attrs) {
				Console.WriteLine("Attr {0}", attr.Name);
			}
#endif
		}

//---------------------------------------------------------------------------------------

		// Takes the information in the Videos collection and adds them to the database
		private bool AddVideosToDatabase() {
			bool Retval = true;				// Everything OK
			string SQL = @"
INSERT INTO tblVideos(Date, Title, Description, Link, ElapsedTime)
VALUES(@Date, @Title, @Description, @Link, @ElapsedTime)";
			foreach (var vid in Videos) {
				using (var cmd = new SqlCeCommand(SQL, conn)) {
					cmd.Parameters.AddWithValue("@Date", vid.ArticleDate);
					cmd.Parameters.AddWithValue("@Title", vid.Title);
					cmd.Parameters.AddWithValue("@Description", vid.Description);
					cmd.Parameters.AddWithValue("@Link", vid.Link);
					string Elapsed = vid.TimeCaption;
					if (Elapsed == "&nbsp;") {
						Elapsed = "";
					}
					cmd.Parameters.AddWithValue("@ElapsedTime", Elapsed);
					try {
						int n = cmd.ExecuteNonQuery();

						if (n <= 0) {
							continue;		// Record not added
						}

						string SQL2 = "SELECT @@IDENTITY";
						var cmd2 = new SqlCeCommand(SQL2, conn);
						object obj = cmd2.ExecuteScalar();
						int VideoID = Convert.ToInt32(obj);

						InsertKeywords(vid, VideoID);
					} catch (SqlCeException sex) {
						if (sex.NativeError == SqlErrorDuplicateRecordCode) {
							Retval = false;
						}
						// Some other SqlCeException. Still return false
						Retval = false;
					} catch {
						// Some other error. Still return false
						Retval = false;
					}
				}
			}
			return Retval;
		}

//---------------------------------------------------------------------------------------

		private void InsertKeywords(Channel9Video vid, int VideoID) {
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

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a date in the form of a) a date, b) "n day[s] ago", c) "n minute[s] ago"
		/// or d) n hour[s] ago, 
		/// return the date of the article. I'm satisfied if this is only an 
		/// approximation. All I really care about is the date. This is probably better
		/// than keeping the accuracy to down to the h:m:s level, since their concept of
		/// "37 minutes" ago, might not quite be the same as mine (we might be on the
		/// cusp of a change from one minute (and possibly one hour) to the next. So just
		/// the date, please.
		/// </summary>
		/// <param name="date"></param>
		/// <returns>Midnight of the resultant date</returns>
		private DateTime GetArticleDate(string date) {
			DateTime ArticleDate = DateTime.Now;
			TimeSpan DateOffset = new TimeSpan(0);
			// Passed either a date, or "n days ago" (n < 7), or "n minutes ago".
			if (date.Contains("minutes ago") || date.Contains("minute ago")) {
				int MinutesAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(0, -MinutesAgo, 0);
			} else if (date.Contains("days ago") || date.Contains("day ago")) {
				int DaysAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(-DaysAgo, 0, 0, 0);
			} else if (date.Contains("hours ago") || date.Contains("hour ago")) {
				int HoursAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(-HoursAgo, 0, 0);
			} else {
				ArticleDate = DateTime.Parse(date.Replace("at ", ""));
			}
			ArticleDate += DateOffset;
			// Now round down to a pure date (no h, m, s -- midnight)
			var ad = ArticleDate;		// Short name for next statement
			ArticleDate = new DateTime(ad.Year, ad.Month, ad.Day);
			return ArticleDate;
		}

//---------------------------------------------------------------------------------------

		private static int GetDateOffset(string date) {
			return int.Parse(date.Split(' ')[0]);
		}

//---------------------------------------------------------------------------------------

		private void btnEditKeywords_Click(object sender, EventArgs e) {
			using (var frm = new EditKeywords(conn, KeywordDict)) {
				frm.ShowDialog();
			}
		}

//---------------------------------------------------------------------------------------

		private void CleanupAndGoHome() {
			Videos.Add(Video);

			AddVideosToDatabase();

			CheckLinks();

			web1.Visible = false;

			grid1.Left    = web1.Left;
			grid1.Top     = web1.Top;
			grid1.Width   = web1.Width;
			grid1.Height  = web1.Height;
			grid1.Visible = true;

			PumpOutAsHtml();

			grid1.DataSource = Videos;
		}

//---------------------------------------------------------------------------------------

		private void PumpOutAsHtml() {
			var sb = new StringBuilder();
			sb.Append("<HTML>\n<HEAD><TITLE>LRS Channel 9 List</TITLE></HEAD>");
			sb.Append("\n<BODY>");

			sb.Append("\n<TABLE BORDER=\"1\">");

			sb.Append("\n\t<THEAD>");
			sb.Append("\n\t\t<TR>");
			sb.Append("\n\t\t\t<TH>Page</TH>");
			sb.Append("\n\t\t\t<TH>Date</TH>");
			sb.Append("\n\t\t\t<TH>Title</TH>");
			sb.Append("\n\t\t\t<TH>Description</TH>");
			sb.Append("\n\t\t\t<TH>Topics</TH>");
			sb.Append("\n\t\t\t<TH>Time</TH>");
			sb.Append("\n\t\t\t<TH WIDTH=\"100\">Link</TH>");
			sb.Append("\n\t\t</TR>");
			sb.Append("\n\t</THEAD>");

			foreach (var vid in Videos) {
				sb.Append("\n\t<tr>");
				sb.WriteTd(2, vid.PageNumber.ToString());
				sb.WriteTd(2, vid.ArticleDate.ToShortDateString());
				sb.WriteTd(2, vid.Title);
				sb.WriteTd(2, vid.Description);

				sb.WriteTd(2, vid.KeywordsString);
				sb.WriteTd(2, vid.TimeCaption);

				string s = "<FONT SIZE=\"-2\">" 
					+ "<A HREF=\"" + vid.Link + "\">" + vid.Link.Replace("/", " / ")
					+ "</FONT></A>";
				sb.WriteTd(100, 2, s);

				sb.Append("\n\t</tr>");
			}

			sb.Append("\n</TABLE>");

			sb.Append("\n</BODY>\n</HTML>\n");

			string FileName = @"C:\LRS\Channel9.html";
			using (var sw = new StreamWriter(FileName)) {
				sw.Write(sb.ToString());
			}

			Process.Start(FileName);
		}

//---------------------------------------------------------------------------------------

		[Conditional("CHECKLINKS")]
		private void CheckLinks() {
			var dict = new Dictionary<string, Channel9Video>();
			var hs = new HashSet<string>();
			foreach (var vid in Videos) {
				if (hs.Contains(vid.Link)) {
					string s = "Conflict between"
						+ "\n\tLink = " + vid.Link
						+ "\n\tTitle = " + vid.Title
						+ "\nPage = " + vid.PageNumber
						+ "\nand"
						+ "\n\tLink = " + dict[vid.Link].Link
						+ "\n\tTitle = " + dict[vid.Link].Title
						+ "\nPage = " + dict[vid.Link].PageNumber;
					MessageBox.Show(s);
					System.Diagnostics.Debugger.Break();	
				} else {
					hs.Add(vid.Link);
					dict.Add(vid.Link, vid);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnEmptyDatabase_Click(object sender, EventArgs e) {
			var result = MessageBox.Show("This will empty all data (except for keywords). Do you want to continue?", "DANGER WILL ROBINSON!",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (result == DialogResult.Yes) {
				EmptyTable("tblVideoKeywords");
				EmptyTable("tblVideos");
				MessageBox.Show("The database has been emptied");
			} else {
				MessageBox.Show("The database is unchchanged");
			}
		}

//---------------------------------------------------------------------------------------

		void EmptyTable(string tblName) {
			string SQL = "DELETE FROM [" + tblName + "]";;
			using (var cmd = new SqlCeCommand(SQL, conn)) {
				cmd.ExecuteNonQuery();
			}
		}

//---------------------------------------------------------------------------------------

		private void WinFormsChannel9_Load(object sender, EventArgs e) {
			sw = new Stopwatch();
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	static class ExtensionMethods {

//---------------------------------------------------------------------------------------

		public static void WriteTd(this StringBuilder sb, int IndentLevel, string Text) {
			sb.Append("\n");
			sb.Append("".PadRight(IndentLevel, '\t'));
			sb.Append("<td>");
			sb.Append(Text);
			sb.Append("</td>");
		}

//---------------------------------------------------------------------------------------

		public static void WriteTd(this StringBuilder sb, int Width, int IndentLevel, string Text) {
			sb.Append("\n");
			sb.Append("".PadRight(IndentLevel, '\t'));
			sb.Append("<td width=\"" + Width + "\">");
			sb.Append(Text);
			sb.Append("</td>");
		}

	}
}
