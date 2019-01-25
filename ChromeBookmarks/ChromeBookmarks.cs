using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace ChromeBookmarks {
	public partial class ChromeBookmarks : Form {

		string BookmarksPath = $@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Bookmarks";


		public ChromeBookmarks() {
			InitializeComponent();

			string bms = File.ReadAllText(BookmarksPath);
			var xxxxxx = JsonConvert.DeserializeObject<Rootobject>(bms);

			string ts = "13157090425916276";
			long lts = long.Parse(ts);
			// var dt2 = DateTimeOffset.FromUnixTimeSeconds(lts/100000);
			System.DateTime dtDateTime = new System.DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(lts/1000000);
			var offset = new DateTimeOffset(DateTime.Now);
			var tz = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
			var dt3 = dtDateTime.Add(tz);

		}
	}
}
