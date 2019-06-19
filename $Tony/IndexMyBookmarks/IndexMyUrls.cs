using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using HAP = HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/* Various Json links I've looked into re Chrome bookmarks
	https://social.microsoft.com/forums/en-US/b7a78d00-fbf8-4fec-84c2-569ee6d9d67e/methods-for-editing-the-chrome-browser-bookmarks-file-in-c
	https://stackoverflow.com/questions/13130492/sterilize-chrome-json-bookmarks
	https://stackoverflow.com/questions/1212344/parse-json-in-c-sharp
	https://stackoverflow.com/questions/6620165/how-can-i-parse-json-with-c
	https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/json-serialization
	https://stackoverflow.com/questions/7895105/deserialize-json-with-c-sharp
	https://stackoverflow.com/questions/7699972/how-to-decode-a-json-string-using-c/7701070#7701070
	https://www.newtonsoft.com/json/help/html/SerializationGuide.htm#LINQ
*/

/* Major TODOs
	* Add BEGIN / COMMIT to INSERT (https://sqlite.org/lang_transaction.html)
		* Remove Transactions on INSERT and see if it makes a difference
	* Put BEGIN at very beginning and COMMIT at end
	* Do some kind of profiling to improve performance
	* Allow ENTER in Search box to initiate the search
	* Add UI to show either search results or messages
	* Comment this sucker better, including .docx file
	* Profile it from an empty database
	* Read in JSON of Chrome bookmarks? How many years would that take???
	* Run SLOC on this
	* Delete <script> tags and their contents
	* Reinstate Msg(mm:ss -- Finished: <url>)
	* Add the option to delete all traces of this ("Cascade Delete") in the database
		and continue
*/

namespace IndexMyUrls {
	public partial class IndexMyUrls : Form {
		const string DatabaseName = "IndexMyUrls.sqlite";
		const string TagTitle     = "//head/title";
		const string TagBody      = "//body";

		PoorPersonsFullTextDatabase db;
		IdCache WordCache;

		int nFolders = 0;
		int nUrls    = 0;

//---------------------------------------------------------------------------------------

		public IndexMyUrls() {
			InitializeComponent();

			TxtUrl.Text = "http://lrs5.net";        // TODO: delete
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Initialization when the program (actually, the form/window) starts.
		/// Gets the database ready.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IndexMyUrls_Load(object sender, EventArgs e) {
			Statbar.Text = "";
			LbMsgs.Items.Clear();

			var docs                = Environment.SpecialFolder.MyDocuments;
			string FullDatabaseName = Environment.GetFolderPath(docs);
			FullDatabaseName        = Path.Combine(FullDatabaseName, DatabaseName);
			db                      = new PoorPersonsFullTextDatabase(FullDatabaseName);

			WordCache  = new IdCache(db, "Word");
			int nWords = WordCache.CacheAllWordIds();
			Msg($"Filled cache with {nWords} word(s)");
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		///  Simply gets the contents of the clipboard in text format (if it exists;
		///  it may be, for example, a graphic) and puts it into the URL text box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnUrlFromClipboard_Click(object sender, EventArgs e) {
			string Url = Clipboard.GetText(TextDataFormat.UnicodeText).ToLower();
			if (Url.Length > 0) { TxtUrl.Text = Url; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Process the URL to extract the text and update the database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnIndexUrl_Click(object sender, EventArgs e) {
			ProcessUrl();
		}

//---------------------------------------------------------------------------------------

		private void ProcessUrl() {
			if (!IsUrl(TxtUrl.Text)) {
				MessageBox.Show("Not a valid URL -- ignoring", "Index My Urls",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var UrlID = GetUrlID(TxtUrl.Text);
			if (UrlID.HasValue) {
				Msg($"Skipping UrlID={UrlID}, {TxtUrl.Text}");
				return;
			}

			db.Stats.Reset();

			Msg("");                // Add spacer line to the listbox
			var wc = new WebClient();
			string RawHtml = wc.DownloadString(TxtUrl.Text);
			var doc = new HAP.HtmlDocument();
			doc.LoadHtml(RawHtml);

			var sw = new Stopwatch();
			sw.Start();
			ProcessHtmlTagContents(TxtUrl.Text, doc, TagTitle, TagBody);
			sw.Stop();
			Msg($@"{sw.Elapsed:mm\:ss} to process {TxtUrl.Text}");
		}

//---------------------------------------------------------------------------------------

		private void Msg() {
			Msg("");
		}

//---------------------------------------------------------------------------------------

		private void Msg(string msg) {
			Statbar.Text = msg;
			LbMsgs.Items.Insert(0, msg);    // Most recent msg at the top
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Common routine for processing the contents of the specified Html tag
		/// </summary>
		/// <param name="url"></param>
		/// <param name="doc"></param>
		/// <param name="tag"></param>
		private void ProcessHtmlTagContents(string url, HAP.HtmlDocument doc, params string[] tags) {
			var AllWords = new List<string>();
			string Title = "N/A";

			foreach (var tag in tags) {
				var TagNode = doc.DocumentNode.SelectSingleNode($"{tag}");
				var text = TagNode.InnerText;
				text = WebUtility.HtmlDecode(text); // e.g. &amp; => &
				var CleanedText = WordMunging.ParseAndCleanString(text);
				if (tag == TagTitle) { Title = text.Trim(); }
				string sep = ", ";
				Debug.WriteLine($@"{tag} -- {text} => {string.Join(sep, CleanedText.ToArray())}");
				AllWords.AddRange(CleanedText);
			}

			long UrlID = AddUrlToDatabase(db, url, Title);
			AddTextToDatabase(db, UrlID, AllWords.Distinct());
		}

//---------------------------------------------------------------------------------------

		private long? GetUrlID(string url) {
			string sql = $"SELECT UrlID FROM tblUrls WHERE url='{url}'";
			var UrlID = db.ExecuteScalar(sql);
			return (long?)UrlID;
		}

//---------------------------------------------------------------------------------------

		private long AddUrlToDatabase(PoorPersonsFullTextDatabase db, string url, string Title) {
			// See comments about duplicates at the front of AddTextToDatabase(). But
			// since we're only doing this once per clicking the Go button, I'm not going
			// to worry about caching.
			long UrlID;
			string sql;
			Title = Title.Replace("'", "''");   // Beware of titles with possesive's
			try {
				sql = $@"
				INSERT INTO tblUrls (URL, Title, WhenRetrieved)
					VALUES('{url}', '{Title}', '{DateTime.Now}');
				SELECT last_insert_rowid() FROM tblUrls;";
				UrlID = (long)db.ExecuteScalar(sql);
			} catch (Exception ex) {
				sql = $"SELECT UrlID from tblUrls WHERE URL='{url}';";
				UrlID = (long)db.ExecuteScalar(sql);
			}
			return UrlID;
		}

//---------------------------------------------------------------------------------------

		private void AddTextToDatabase(PoorPersonsFullTextDatabase db, long urlID, IEnumerable<string> cleanedText) {
			// OK, here's our problem. We want a single unique instance of a word in
			// tblWords, and get a unique WordID. But we can't just do a simple INSERT
			// since the word may have been seen before (either in this URL, or in a
			// previous URL.
			//
			// Since queries to the database are fairly expensive, we'll maintain a
			// Dictionary mapping the word to its WordID. Before we try to INSERT a word
			// into tblWords, we'll check the dictionary first. In a sense we're caching
			// this aspect of the table.
			//
			// But note that we're only doing this on a URL-by-URL basic. Better would be
			// to keep a global dictionary (in a WordCache class), that's used for all
			// words in this session. Hardly perfect, but better than nothing.
			var WordDict = new Dictionary<string, long>();

			// TODO: Now that we've made sure the words are distinct, we've got to make
			//		 the dictionary global for this run.

			// On a different topic, Sqlite supports (among many, many other things) an
			// extended INSERT statement of the form
			//		INSERT INTO tblfoo (field1, field2)
			//			(1, 2),
			//			(3, 4);
			// So we'll collect the information for all the references and then pass
			// everything to AddWordsToTblRefs().
			var Values = new List<(long, long)>();

			foreach (var word in cleanedText) {
				var WordID = WordCache.GetIDForWord(word);
				Values.Add((WordID, urlID));
			}
			AddRefsToTblRefs(Values);
		}

//---------------------------------------------------------------------------------------

		private void AddRefsToTblRefs(List<(long wordID, long urlID)> Refs) {
			string sql = $@"
				INSERT INTO tblRefs (WordID, UrlID)
					VALUES";
			var sb = new StringBuilder(sql);
			foreach (var item in Refs) {
				sb.Append(Environment.NewLine + $"({item.wordID}, {item.urlID}),");
			}
			--sb.Length;        // Drop trailing comma
			sb.Append(";");
			db.ExecuteNonQuery(sb.ToString());
		}

//---------------------------------------------------------------------------------------

		private bool IsUrl(string text) {
			text = text.ToLower();
			return text.StartsWith("http://") || text.StartsWith("https://");
		}

//---------------------------------------------------------------------------------------

		private void BtnSearch_Click(object sender, EventArgs e) {
			var words = WordMunging.ParseAndCleanString(TxtSearchBox.Text);
			var sb = new StringBuilder();
			for (int i = 0; i < words.Count; i++) {
				if (i > 0) {
					sb.Append(Environment.NewLine + "INTERSECT" + Environment.NewLine);
				}
				sb.Append(GetSearchQuery(words[i]));
			}

			string sql = sb.ToString();
			var rdr = db.ExecuteReader(sql);
			while (rdr.Read()) {
				string title = (string)rdr["Title"];
				string Url = (string)rdr["Url"];
				Msg($"{title} - {Url}");    // TODO: Put into better UI
			}
		}

//---------------------------------------------------------------------------------------

		private string GetSearchQuery(string word) {
			string sql = $"SELECT Url, Title FROM AllRefs WHERE Word='{word}'";
			return sql;
		}

//---------------------------------------------------------------------------------------

		private void emptyTheDatabaseToolStripMenuItem1_Click(object sender, EventArgs e) {
			var response = MessageBox.Show("Empty the database?", "RED ALERT!!!",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (response != DialogResult.Yes) {
				MessageBox.Show("OK, cancel Red Alert. Database unchanged.",
					"Index My Urls", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			db.EmptyTheDatabase();
			MessageBox.Show("Database emptied.", "Index My Urls",
				MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

//---------------------------------------------------------------------------------------

		private void useTestDataToolStripMenuItem_Click(object sender, EventArgs e) {
			var sw = new Stopwatch();
			sw.Start();

			string bmJson = File.ReadAllText(@"C:\Users\lrs5\AppData\Local\Google\Chrome\User Data\Default\Bookmarks");
			var root = JsonConvert.DeserializeObject<RootObject>(bmJson);
			var bmBar = root.roots.bookmark_bar;
			var other = root.roots.other;
			ProcessNode(bmBar);
			Console.WriteLine("====================================================");
			// ProcessNode(other);
#if false
			// TestBabyJson();
			// if (!(e is null)) return;
#if true
			var sw = new Stopwatch();
			sw.Start();
			// foo1();
			// foo2();
#if true
			var xxx = JsonConvert.DeserializeObject(bmJson);
			var kids1 = JObject.Parse(bmJson)["roots"]["bookmark_bar"]["children"];
			var kids1a = JObject.Parse(bmJson)["roots"]["bookmark_bar"]["children"];
			var yyy2 = JsonConvert.DeserializeObject<ChromeBookmark>(kids1a.ToString());
			var kids2 = JObject.Parse(bmJson);
			dbgDump(kids1, 0);
#endif
			int i = 1;
#else
			const string TestDataFilename = "TestUrls.txt";
			int nUrls = 0;
			string sql = "BEGIN TRANSACTION";
			db.ExecuteNonQuery(sql);
			using (var sr = new StreamReader(TestDataFilename)) {
				string line = null;
				while ((line = sr.ReadLine()) != null) {
					if (line.StartsWith("#")) { continue; }  // Ignore comments
					TxtUrl.Text = line;
					++nUrls;
					ProcessUrl();
				}
			}
			sql = "COMMIT TRANSACTION";
			db.ExecuteNonQuery(sql);
			sw.Stop();
#endif
#endif
			Msg();
			Msg($@"Done with test data in {sw.Elapsed:mm\:ss}");
		}

//---------------------------------------------------------------------------------------

		private void ProcessNode(Child child) {
			foreach (Child kid in child.children) {
				ProcessChild(kid, 0);
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessChild(Child kid, int level) {
			WriteLevel(level);
			if (kid.type == "folder") {
				++nFolders;
				Console.WriteLine(kid.name);
				foreach (var grandkid in kid.children) {
					ProcessChild(grandkid, level + 1);
				}
			}  else if (kid.type == "url" && !kid.url.StartsWith("chrome://")) {
				++nUrls;
				Console.WriteLine($"\t{kid.name}");
			}
		}

//---------------------------------------------------------------------------------------

		private static void WriteLevel(int level) {
			Console.Write($"[Level {level}]: ");
			Console.Write("".PadRight(level, '\t'));
		}

#if false

//---------------------------------------------------------------------------------------

		private void TestBabyJson() {
			string json = @"
{
""children"": 
	[ 
		{

			   ""date_added"": ""13106880768494216"",
               ""id"": ""30"",
               ""name"": ""Microsoft Research Video Shows Off What Could Have Been with the McLaren - Thurrott.com"",
               ""sync_transaction_version"": ""3"",
               ""type"": ""url"",
               ""url"": ""https://www.thurrott.com/mobile/windows-phone/66837/microsoft-research-video-shows-off-mclaren""
		}
	]
}
";
			var xxx = JObject.Parse(json);
			var yyy = JsonConvert.DeserializeObject<RootObject>(json);
			var yy2 = JsonConvert.DeserializeObject(json);
			var bm2 = new BookMark2();
			var yyy3 = JsonConvert.DeserializeAnonymousType<BookMark2>(json, bm2);
			Console.WriteLine($@"xxx.name =  {xxx["name"]}");
			foreach (var item in xxx.Properties()) {
				Console.WriteLine(item.Name);
			}
			// Console.WriteLine($@"yyy2.name = {yy2["name"]}");
		}

//---------------------------------------------------------------------------------------

		private static void foo1() {
			var yyy = File.ReadAllText(@"C:\Users\lrs5\AppData\Local\Google\Chrome\User Data\Default\Bookmarks");
			var xxx = JsonConvert.DeserializeObject(yyy);
			var kids = JObject.Parse(yyy)["roots"]["bookmark_bar"]["children"];
			var fooFolder = kids.Where(x => x["name"].ToString() == "foo");

			foreach (var folder in kids[0]) {
				foreach (var item in folder["children"]) {
					Console.WriteLine(item["url"].ToString());
				}
			}
		}
		
//---------------------------------------------------------------------------------------

		private void foo2() {
			// From https://stackoverflow.com/questions/619120/deserializing-chrome-bookmark-json-data-in-c-sharp
			string json = File.ReadAllText(@"C:\Users\lrs5\AppData\Local\Google\Chrome\User Data\Default\Bookmarks");
			using (StringReader reader = new StringReader(json))
			using (JsonReader jsonReader = new JsonTextReader(reader)) {
				JsonSerializer serializer = new JsonSerializer();
				var o = (JToken)serializer.Deserialize(jsonReader);
				// var kid = o["roots"]["bookmark_bar"]["children"][0];
				var kid = o["roots"]["bookmark_bar"]["children"];
				dbgDump(kid as JArray, 0);
				var date_added = kid["date_added"];
				Console.WriteLine(date_added);
			}
		}

//---------------------------------------------------------------------------------------

		private void dbgDump(JObject p, int level) {
			WriteLevel(level);
			HandleJObject(p, level);
		}

//---------------------------------------------------------------------------------------

		private void dbgDump(IEnumerable<JToken> p, int level) {
			WriteLevel(level);
			HandleIEnumerableJToken(p, level);
		}

//---------------------------------------------------------------------------------------

		private void dbgDump(JArray p, int level) {
			WriteLevel(level);
			HandleJarray(p, level);
		}

//---------------------------------------------------------------------------------------

		private void HandleJObject(JObject p, int level) {
			Console.Write($"This is a {p.GetType().FullName}");
			foreach (KeyValuePair<string, JToken> xx in p) {
				WriteLevel(level);
				string jVal = chomp(xx.Value);
				Console.WriteLine($"HandleJObject: [{xx.Key}] = {jVal}");
			}
			var kid = p.Children();
			SwitchOnKidType(kid, level + 1);
		}

//---------------------------------------------------------------------------------------

		private void HandleJarray(JArray jarray, int level) {
			foreach (var item in jarray) {
				Console.WriteLine(item["name"]);
			}
		}

//---------------------------------------------------------------------------------------

		private void HandleIEnumerableJToken(IEnumerable<JToken> p, int level) {
			Console.Write($"This is a {p.GetType().FullName}");
			string jVal;
			foreach (var xx in p) {
				WriteLevel(level);
				switch (xx) {
				case JProperty jprop:
					jVal = chomp(jprop.Value);
					Console.WriteLine($"jProp['{jprop.Name}'] = {jVal}");
					break;
				case JValue jval:
					Console.WriteLine($"JValue = {jval.Value}");
					break;
				case JObject jObj:
					HandleJObject(jObj, level);
					break;
				case JArray jarray:
					HandleJarray(jarray, level);
					break;
				default:
					Console.WriteLine(nameof(HandleIEnumerableJToken) + " - " + xx);
					break;
				}
				// Console.WriteLine(xx);
				// if (xx.Key == "roots") continue;
				// Console.WriteLine($"[{xx.Key}] = {xx.Value}");
			}
			var kid = p.Children();
			SwitchOnKidType(kid, level + 1);
		}

//---------------------------------------------------------------------------------------

		private string chomp(JToken value) {
			// Assume the value is a string
			const int MaxLength = 100;
			string s = value.ToString();
			if (s.Length > MaxLength) { return s.Substring(0, MaxLength); }
			return s;
		}

//---------------------------------------------------------------------------------------

		private void SwitchOnKidType(object p, int level) {
			switch (p) {
			case JObject JOb:
				HandleJObject(JOb, level);
				break;
			case IEnumerable<JToken> enumJToken:
				HandleIEnumerableJToken(enumJToken, level);
				break;
			default:
				Console.WriteLine($"Need support for type {p.GetType().FullName}");
				break;
			}
		}
#endif
	}
}
