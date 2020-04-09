using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using LRSNativeMethodsNamespace;
using LRS.Utils;
using PortableDeviceApiLib;

using TagLib;

/*
C:\Program Files (x86)\Android\android-sdk\platform-tools>adb push --sync "g:\$ Zune Master\Brenda Stubbert" /sdcard/Music
https://www.androidauthority.com/about-android-debug-bridge-adb-21510/
https://stackoverflow.com/questions/39441477/how-to-copy-file-using-adb-to-android-directory-accessible-from-pc

	https://forums.xamarin.com/discussion/5923/access-to-sd-card
	https://developer.android.com/reference/android/provider/MediaStore
	https://developer.android.com/reference/android/provider/MediaStore.Files.html
	https://stackoverflow.com/questions/10384080/mediastore-uri-to-query-all-types-of-files-media-and-non-media
	https://www.bing.com/search?q=mediastore+query+example&form=WNSGPH&qs=SW&cvid=798c6c7374c64bdeab6bf2b1f3d46baa&pq=mediastore+query+example&cc=US&setlang=en-US&PC=DCTS&nclid=12D9FF18DF54C36DE4D7442D5A7BDE93&ts=1553102485563&elv=AY3%21uAY7tbNNZGZ2yiGNjfPVB1eYfp2bWPDBca1D%21cuq%218Ghj8f8pB6AaBY92pSuVX9B4Kpp32g0JW1pXMegsA2P*cZJvMYbx15z9immSP0Z&wsso=Moderate
	https://www.sandersdenardi.com/querying-and-removing-media-from-android-mediastore/
	https://stackoverflow.com/questions/3572463/what-is-context-on-android
	https://github.com/aosp-mirror/platform_frameworks_base/blob/master/core/java/android/content/Context.java
	https://stackoverflow.com/questions/10384080/mediastore-uri-to-query-all-types-of-files-media-and-non-media
	https://developer.xamarin.com/api/type/Android.Provider.MediaStore/
	https://developer.xamarin.com/api/type/Android.Provider.MediaStore/
	https://stackoverflow.com/questions/6096783/android-using-mediastore
	https://stackoverflow.com/questions/8994625/display-all-music-on-sd-cardhttp://z4android.blogspot.com/2011/06/displaying-list-of-music-files-stored.html


*/

// TODO: G:\OneDrive\$ Zune Master\Various Artists\New Electric Muse II; the Continuing Story of Folk into Rock (Disc 1) - Delete *.wma?
// TODO: G:\OneDrive\$ Zune Master\Various Artists\New Electric Muse II; the Continuing Story of Folk into Rock (Disc 2) - Delete *.wma?
// TODO: Look for albums with multiple identical cut #'s
// TODO: Check for filenames (including path) > 254
// TODO: Get # tracks per album and make sure we have that # of cuts per album

using static LRSNativeMethodsNamespace.LRSNativeMethods;
using WindowsPortableDeviceNet;

namespace CheckFor192Rip {
	public partial class CheckFor192Rip : Form {
		Dictionary<int, List<AlbumInfo>> AlbumsByRate;
		Dictionary<int, (long TotalSize, TimeSpan TotalDuration)> Totals;
		List<AlbumInfo> Albums;
		DateTime	Earliest192;
		Stopwatch	sw;
		Timer		tmr;

		Dictionary<string, List<AlbumInfo>> AlbumsByArtistName;	// key = ArtistName


//---------------------------------------------------------------------------------------

		public CheckFor192Rip() {
			InitializeComponent();

			TxtStartingFolder.Text = @"G:\$ Zune Master";

			sw  = new Stopwatch();
			tmr = new Timer();

			tmr.Interval = 1_000;   // 1 second
			tmr.Tick    += Tmr_Tick;

#if false
			var lst = new List<string> {
				@"G:\MsConferences\Ignite 2018-dgfhrtygb",
				// @"G:\$TEST DEST\Ignite 2018-dgfhrtygb\$Uncategorized\All About Exchange 2019\All About Exchange 2019.mp4",
				@"G:\MSConferences\VS Live 2017"
			};
			var xxx = CopyDir(@"G:\$TEST DEST", lst); ;

			goo();

			var TotalDuration = new TimeSpan();
			long TotalSize = 0;
			Totals = new Dictionary<int, (long TotalSize, TimeSpan TotalDuration)>();
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			foreach (var key in Albums.Keys) {
				UpdateStats(key);
				TotalDuration += Albums[key].Duration;
				TotalSize += Albums[key].Size;
			}
			sw.Stop();

			string Summary = $"{Albums.Count:N0}: Duration={TotalDuration}, Size={TotalSize:N0}";
#endif
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			LblElapsed.Text = sw.Elapsed.ToString(@"mm\:ss");
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowse_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Browse");
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			bool DocWatson = false;
			if (DocWatson) {
				DoDocWatson();
				return;
			}
			AlbumsByRate    = default;
			Totals          = default;
			Earliest192     = new DateTime(2100, 12, 31);
			LblDone.Text    = "";
			LblElapsed.Text = "";

			sw.Restart();
			tmr.Start();

			Albums             = new List<AlbumInfo>(3000);
			AlbumsByRate       = new Dictionary<int, List<AlbumInfo>>(3000);
			AlbumsByArtistName = new Dictionary<string, List<AlbumInfo>>(3000);
			Totals             = new Dictionary<int, (long TotalSize, TimeSpan TotalDuration)>(3000);

			string ZuneRootDir   = TxtStartingFolder.Text;
			var ZuneDirs         = Directory.EnumerateDirectories(ZuneRootDir, "*", SearchOption.AllDirectories);
			const int MaxFolders = int.MaxValue;        // For testing

			/*
				An Artist is a folder that has sub-folders
				An Album is a folder with no sub-folders
				An Album192 is an album recorded with a bit rate of 192
				A Ready-to-copy-to-phone-Artist is an Artist with the number of Album192's == the
					number of sub-folders it has
				Assumptions:
					* A folder either has sub-folders and no file children, or has children and no
						sub-folders
					* All folders have names <root>\Artist\Album, but in at least one case
						(78 RPM MP3), there is no Album
					* Some folders have no *.wma files in them (having come from other sources), so
						they won't be flagged as <IsGood> and won't be processed.
			*/

			int MaxAlbums = 100_000;
			int nAlbums = 0;

			foreach (var dir in ZuneDirs) {
				// if (!dir.Contains(@"G:\$ Zune Master\Al Petteway\Caledon Wood")) continue;	// TODO:
				LblDone.Text = dir;
				var info = new AlbumInfo(dir);
				if (info.NumberOfCuts == 0) { continue; }
				Albums.Add(info);
				if (!info.IsAlbumNotArtist) { continue; }
				EnsureDictsExist(info);
				if (++nAlbums > MaxAlbums) { break; }

				// Console.WriteLine($"Album[{count++}]: [{info.Rate}] {info.ArtistName} - {info.AlbumName}");

				AlbumsByRate[info.Rate].Add(info);
				AlbumsByArtistName[info.ArtistName].Add(info);
				if ((info.Rate == 192) && (AlbumInfo.EarliestCreationTime < Earliest192)) {
					Earliest192 = AlbumInfo.EarliestCreationTime;
					LblEarliest.Text = Earliest192.ToShortDateString();
				}
				UpdateUiStats(info.Rate);
#if DEBUG
				if (AlbumsByRate[info.Rate].Count >= MaxFolders) { break; }
#endif
				Application.DoEvents();
			}
			sw.Stop();
			tmr.Stop();
			LblDone.Text = "Done!";

			PrintIndex(@"g:\lrs\CdIndex.html");
#if true
			GenreJustVariousArtists();

			FindDuplicateTrackNumbers();

			ShowDuplicateDurationAlbums();

			CheckConsistnecyOfAlbumCutGenres();

			Chickenman();
#if false

			FindCopyableAlbums(128);

			ShowDiskNotDisc();

			ShowAlbumsByRate(128);

			FindCopyableAlbums(192);

			ShowAlbumsByRate(192);
			ShowAlbumsByDuration();
#endif
#endif
		}

//---------------------------------------------------------------------------------------

		private void GenreJustVariousArtists() {
			var qry = from alb in Albums
					  where alb.PrimaryGenre == "Various Artists"
					  select alb;
			Console.WriteLine("================ Genre Just Various");
			foreach (var item in qry) {
				Console.WriteLine($"{item.PrimaryGenre}");
			}
		}

//---------------------------------------------------------------------------------------

		private void PrintIndex(string fn) {
			// Albums by Genre, Artist Name, Album Name
			// See https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq
			var qry = from alb in Albums
					   orderby alb.PrimaryGenre, alb.ArtistName, alb.AlbumName
					   group alb by alb.PrimaryGenre into genres
					   select new {
						   genrekey = genres.Key,
						   albums = from album in Albums
									where album.PrimaryGenre == genres.Key
									group album by album.ArtistName into Artists
									select new {
										Artists.Key,
										name = (from art in Artists
										select art.AlbumName).ToList()
									}
										
					  };

			using (var sw = new StreamWriter(fn)) {
				sw.WriteLine($@"<html>
<head>
<title>Larry's CD Collection</title>
</head>
<body>
");
#if true
				foreach (var entry in qry) {
					sw.WriteLine($"<p/><b><font size=\"32\" color=\"red\">{entry.genrekey}</font></b><br/>");
					foreach (var alb in entry.albums) {
						// sw.WriteLine($"{alb.ArtistName} - {alb.AlbumName}<br/>");
						int n = alb.name.Count(); 
						sw.WriteLine($"<b>{alb.Key} - {alb.name.Count()} album{s(n)}</b><br/>");
						if ((entry.genrekey == "Beethoven") && (alb.Key == "Various Artists - Classical")) Debugger.Break();
						foreach (var name in alb.name) {
							sw.WriteLine($"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{name}<br/>");
						}
					}
				}
#else
				foreach (var alb in qry) {
					sw.WriteLine($"*G {alb.PrimaryGenre}");
					sw.WriteLine($"*A {alb.ArtistName}");
					sw.WriteLine($"*N {alb.AlbumName}");
					foreach (var cut in alb.Cuts) {
						// sw.WriteLine($"C {cut.CutNumber} {cut.Title}");
					}
					sw.WriteLine(";");
				}
#endif
				sw.WriteLine(@"</body>
</html>");
			}
			Process.Start(fn);
		}

//---------------------------------------------------------------------------------------

		private string s(int n) => n == 1 ? "" : "s";

//---------------------------------------------------------------------------------------

		private void Chickenman() {
			/*
			matches = defaultRegex.Matches(text);
	  Console.WriteLine("Parsing '{0}'", text);
	  // Iterate matches
	  for (int ctr = 0; ctr < matches.Count; ctr++)
		 Console.WriteLine("{0}. {1}", ctr, matches[ctr].Value);
			*/
			var CmAlb = AlbumsByArtistName["Chickenman"];
			var re = new System.Text.RegularExpressions.Regex(@"^Episode \d+ - .*");

			var Episodes  = new List<(AlbumInfo, Cut)>();
			var Promos    = new List<(AlbumInfo, Cut)>();
			var Weekends  = new List<(AlbumInfo, Cut)>();
#if false
			var AlbDict = new Dictionary<int, AlbumInfo>();
			var albs = (from alb in Albums
					  where alb.ArtistName == "Chickenman"
					  select alb).ToArray();
			foreach (AlbumInfo item in albs) {
				var parse = item.AlbumName.Split(' ', ',');
				int n = int.Parse(parse[3]);
				AlbDict[n] = item;
				// Console.WriteLine(item.AlbumName);
			}
			foreach (var key in AlbDict.Keys) {
				Console.WriteLine(albs[key].AlbumName);
			}
#endif
			foreach (var alb in CmAlb) {
				Console.WriteLine($"=============================== {alb.AlbumName}");
				foreach (var cut in alb.Cuts) {
					var title = cut.Title.Replace("\"", "");
					if (title.StartsWith("Weekend In Suburbia")) { Episodes.Add((alb, cut)); continue; }
					if (title.StartsWith("Weekend ")) { Weekends.Add((alb, cut)); continue; }
					if (title.Contains("Promo")) { Promos.Add((alb, cut)); continue; }
					if (title.Contains("B.E.A.K. ")) { Promos.Add((alb, cut)); continue; }
					if (title.Contains("Contest")) { Promos.Add((alb, cut)); continue; }
					if (title.Contains("Earth Polluters")) { Weekends.Add((alb, cut)); continue; }
					Episodes.Add((alb, cut));
				}
			}

			string TargetDir = @"F:\chick\";
			int n = 1;

			Console.WriteLine("================ PROMOS");
			foreach ((AlbumInfo alb, Cut cut) item in Promos) {
				// Console.WriteLine(item.Item2.Title);
				CmCopy("Promos", item, ref n);
			}

			Console.WriteLine("================ WEEKEND");
			foreach (var item in Weekends) {
				// Console.WriteLine(item.Item2.Title);
				CmCopy("Weekend", item, ref n);
			}

			Console.WriteLine("================ EPISODES");
			foreach (var item in Episodes) {
				// Console.WriteLine(item.Item2.Title);
				CmCopy("Episodes", item, ref n);
			}

			void CmCopy(string dir, (AlbumInfo alb, Cut cut) item, ref int num) {
				Directory.CreateDirectory(TargetDir + dir);
				string from  = Path.Combine(item.alb.PathName, item.cut.Filename);
				string title = item.cut.Title;
				title        = title.Replace("?", "").Replace(":", " --").Replace("\"", "") + ".wma";
				if (dir == "Weekend") {
					title = title.Replace("Chicken Man Vs. Earth Polluters - Episode", "Weekend");
					title = title.Replace("Chicken Man Vs. Earth Polluters EP", "Weekend");
					title = title.Replace("Weekend Episode", "Weekend");
				} else if (dir == "Episodes") {
					if (! title.StartsWith("Episode ")) {
						title = $"Episode {num, 2} " + title;
					}
					++n;
				}
				string to    = Path.Combine(TargetDir, dir, title);
				Console.WriteLine($"File.Copy({from},{to}, true");
				System.IO.File.Copy(from, to, true);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowDiskNotDisc() {
			Console.WriteLine("===========================");
			Console.WriteLine("Album titles with Disk not Disc");
			var qry = from alb in Albums
					  where alb.Title.ToUpper().Contains("DISK")
					  select alb;
			int n = 0;
			foreach (AlbumInfo item in qry) {
				Console.WriteLine($"[{++n}]: {item.ArtistName} - {item.Title}");
			}
		}

//---------------------------------------------------------------------------------------

		private void DoDocWatson() {
			string ZuneRootDir = TxtStartingFolder.Text;
			var WatsonDirs = Directory.EnumerateDirectories(ZuneRootDir, "*Watson*");
			var nl = Environment.NewLine;
			var tab = "";
			for (int i = 0; i < 1; i++) {
				// tab += "&nbsp;";
				tab += "   ";
			}
			foreach (var doc in WatsonDirs) {
				string artist = Path.GetFileName(doc);
				if (! artist.ToUpper().Contains("WATSON")) { continue; }
				// Console.WriteLine($"<p/><h1>{artist}</h1>");
				Console.WriteLine($"{nl}<h1/>{artist}");
				var Albums = Directory.EnumerateDirectories(doc);
				foreach (var album in Albums) {
					var AlbumName = Path.GetFileName(album);
					Console.WriteLine($"{tab}<b>{AlbumName}");
					// Console.WriteLine($"\t<b>{AlbumName}</b><br/>");
					foreach (var cut in Directory.EnumerateFiles(album, "*.wma")) {
						var CutName = Path.GetFileNameWithoutExtension(cut);
						Console.WriteLine($"{tab}{tab}{CutName}");
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void FindDuplicateTrackNumbers() {
			Console.WriteLine("===========================");
			Console.WriteLine($"Find Duplicate Track Numbers");
			var Tracks = new HashSet<int>();
			foreach (var alb in Albums) {
				// if (alb.PathName.Contains(@"Discover The Classics, Vol. 2 [Disc 2]")) Debugger.Break();
				Tracks.Clear();
				bool bFoundDups = false;
				foreach (var cut in alb.Cuts) {
					var bDupTrackNum = Tracks.Contains(cut.CutNumber);
					Tracks.Add(cut.CutNumber);
					if (bDupTrackNum && (!bFoundDups)) { 
						Console.WriteLine(alb.PathName);
						bFoundDups = true;
						DumpCuts(alb);			// TODO: Do this in other method(s)
						break;
					}
				}
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		private void DumpCuts(AlbumInfo alb) {
			for (int i = 0; i < alb.Cuts.Count; i++) {
				Console.WriteLine($"\tcut[{i + 1}]={alb.Cuts[i].CutNumber}, Rate={alb.Cuts[i].Rate}, genre={alb.Cuts[i].Genres[0]}, title={alb.Cuts[i].Title}");
			}
		}

//---------------------------------------------------------------------------------------

		private void CheckConsistnecyOfAlbumCutGenres() {
			Console.WriteLine("===========================");
			Console.WriteLine($"Check consistency of album cut genre");
			foreach (var alb in Albums) {
				HashSet<string> Genres = new HashSet<string>();
				bool bDumped = false;
				foreach (var cut in alb.Cuts) {
					if (cut.Genres.Length > 1) {
						Debugger.Break();
					}
					Genres.Add(cut.Genres[0]);
					if ((Genres.Count > 1) && !bDumped) {
						Console.WriteLine($"{alb.PathName}");
						DumpCuts(alb);
						bDumped = true;
						// Debugger.Break();
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowAlbumsByDuration() {
			Console.WriteLine("===========================");
			Console.WriteLine($"Showing albums by Duration");
			var qry = from alb in Albums
					  orderby alb.Duration descending
					  select alb;
			int n = 0;
			foreach (var alb in qry) {
				++n;
				Console.WriteLine($@"[{n:N0}]: {alb.Duration:hh\:mm\:ss} (Cuts:{alb.NumberOfCuts}) - {alb.ArtistName} => {alb.AlbumName}");
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowDuplicateDurationAlbums() {
			Console.WriteLine("===========================");
			Console.WriteLine($"Albums grouped by Duration");
			var qry = from a in Albums
					  where a.NumberOfCuts > 0
					  orderby a.ArtistName
					  group a by a.Duration into g
					  select new { Duration = g.Key, Albums = g.ToList() };
			int n = 0;
			foreach (var set in qry) {
				if (set.Albums.Count > 1) {
					Console.WriteLine($@"{set.Duration:hh\:mm\:ss} =======================");
					foreach (var alb in set.Albums) {
						Console.WriteLine($"[{++n}]: Rate={alb.Rate}, Cuts={alb.NumberOfCuts}, {alb.PathName}");
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowAlbumsByRate(int rate) {
			Console.WriteLine("===========================");
			Console.WriteLine($"Showing albums by rate {rate}");
			int n = 0;
			foreach (var album in AlbumsByRate[rate]) {
				Console.WriteLine($"[{++n}]: {album.ArtistName} -- {album.AlbumName}, cuts={album.NumberOfCuts}");
			}
		}

//---------------------------------------------------------------------------------------

		private void FindCopyableAlbums(int rate) {
			Console.WriteLine("===========================");
			Console.WriteLine($"Showing copyable albums by rate {rate}");
			var qry = from    artist in AlbumsByArtistName
					  where   artist.Value.All(cut => cut.Rate == rate)
					  orderby artist.Key
					  select  artist;
			int count = 0;
			foreach (var artist in qry) {
				Console.WriteLine(artist.Key);
				var albumNames = artist.Value.OrderBy(p => p.AlbumName);
				foreach (var album in albumNames) {
					Console.WriteLine($"     * [{count++}]: Rate[{rate}]: -- {album.AlbumName}");
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void BtnAnalyze_Click(object sender, EventArgs e) {
			string ZuneRootDir = TxtStartingFolder.Text;
			var ZuneDirs = Directory.EnumerateDirectories(ZuneRootDir, "*", SearchOption.AllDirectories);
			foreach (var dir in ZuneDirs) {

			}
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void EnsureDictsExist(AlbumInfo info) {
			if (!AlbumsByRate.ContainsKey(info.Rate)) {
				AlbumsByRate[info.Rate] = new List<AlbumInfo>();    // New rate found
			}
			if (!AlbumsByArtistName.ContainsKey(info.ArtistName)) {
				AlbumsByArtistName[info.ArtistName] = new List<AlbumInfo>();
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowStats() {
			DoStat(128, LblCount128, LblSize128, LblAvgSize128, LblDuration128, LblAvgDuration128);
			DoStat(192, LblCount192, LblSize192, LblAvgSize192, LblDuration192, LblAvgDuration192);
		}

//---------------------------------------------------------------------------------------

		private void UpdateUiStats(int key) {
			var albs            = AlbumsByRate[key];
			int ixLast          = albs.Count - 1;
			var alb             = albs[ixLast];
			if (! Totals.ContainsKey(key)) {
				Totals[key] = (0, new TimeSpan());
			}
			var tots            = Totals[key];
			tots.TotalSize     += alb.Size;
			tots.TotalDuration += alb.Duration;
			Totals[key] = tots;

			if (AlbumsByRate.ContainsKey(192) && AlbumsByRate.ContainsKey(128)) { 
				var avg192            = AlbumsByRate[192].Average(al => al.Size);
				var EstimatedNewSize  = Totals[192].TotalSize + AlbumsByRate[128].Count * avg192;
				LblEstimatedSize.Text = AutoScale((long)EstimatedNewSize);
			}
			ShowStats();
		}

//---------------------------------------------------------------------------------------

		private void DoStat(int key, Label LblCount, Label lblSize, Label lblAvgSize,
					Label lblDuration, Label lblAvgDuration) {
			if (! AlbumsByRate.ContainsKey(key)) { return; }
			List<AlbumInfo> albs = AlbumsByRate[key];
			LblCount.Text        = albs.Count.ToString("N0");
			long TotalSize       = albs.Sum(a => a.Size);
			lblSize.Text         = AutoScale(TotalSize);
			lblAvgSize.Text      = AutoScale(TotalSize / albs.Count);
			var dur              = new TimeSpan();

			albs.ForEach(a => dur += a.Duration);
			lblDuration.Text  = dur.ToString(@"d\:hh\:mm\:ss");
			var avgDur = new TimeSpan(0, 0, (int)dur.TotalSeconds / albs.Count);
			lblAvgDuration.Text = avgDur.ToString(@"d\:hh\:mm\:ss");
		}

//---------------------------------------------------------------------------------------

		private new string AutoScale(long NumBytes) {
#if false
			const long KB = 1024;
			const long MB = KB * 1024;
			const long GB = MB * 1024;
#else
			const long KB = 1000;
			const long MB = KB * 1000;
			const long GB = MB * 1000;
#endif

			if (NumBytes < KB) {
				return NumBytes.ToString("N0");
			} else if (NumBytes < MB) {
				return ((NumBytes + KB - 1) / KB).ToString("N0") + " KB";
			}
			else if (NumBytes < GB) {
				return ((NumBytes + MB - 1) / MB).ToString("N0") + " MB";
			}
			return ((NumBytes + GB - 1) / GB).ToString("N0") + " GB";
		}

//---------------------------------------------------------------------------------------

		int CopyDir(string to, List<string> from) {
			// TODO: Maybe move to LRSUtils (including overload below)
			// TODO: I suppose we could allow setting other fields, such
			//		 as Flags and others
			var Op = new SHFILEOPSTRUCT {
				hwnd          = Process.GetCurrentProcess().Handle,
				Func          = FileFuncFlags.FO_COPY,
				From          = "",
				To            = to,
				Flags         = FILEOP_FLAGS.FOF_ALLOWUNDO | FILEOP_FLAGS.FOF_FILESONLY,
				ProgressTitle = "LRS Test of SHFileOperation"
			};
			// Op.To = to;
			// <from> takes a null-delimited list of dirs
			foreach (var item in from) {
				Op.From += item + '\0';
			}
			Op.From += '\0';    // List ends with double null
			// Note: SHFileOperation has, since Vista, been supplanted by
			//		 https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-ifileoperation-copyitem
			var retcode = SHFileOperation(ref Op);
			// Note: *Could* throw exception is retcode isn't 0
			return retcode;
		}

//---------------------------------------------------------------------------------------

		int CopyDir(string to, params string[] from) {
			var lst = new List<string>(from);
			return CopyDir(to, lst);
		}

//---------------------------------------------------------------------------------------

		private void BtnTestPDM_Click(object sender, EventArgs e) {
#if true
			var devs2 = PdInfo.Pds();
			var devID = GetPdId("LRS G7");
			// var devID = GetPdIdByDescription("ZTE");
			// string to = Path.Combine(devID, "Phone", "Music");
			string to = Path.Combine(devID, "SD card");
			// var proc = Process.Start("explorer.exe " + "\"" + to + "\"");
			var cmd = $"explorer.exe \"{devID}\"";
			var proc = Process.Start(cmd);
			Clipboard.SetText(to);
			SetForegroundWindow(proc.MainWindowHandle);
			SendKeys.Send("^L");       // Goes to Address bar
			System.Threading.Thread.Sleep(1000);
			//SendKeys.Send(to);
			if (sender != null) return;
			
			CopyDir(to, @"G:\$$$ Test\Abbott And Costello\Masters of Comedy - Disc 6 of 8\01 March 10, 1949.wma");
#endif

			var ute = new Utility();
			var devs = ute.Get();


			PdInfo OurPhone = null;
			string MyPhoneName = "LRS G7";
			MyPhoneName = "Z835";			// TODO:
			foreach (var dev in PdInfo.Pds()) {
				// if (dev.FriendlyName == "LRS G7") {
				if (dev.FriendlyName == "ZTE Maven 3") {
					OurPhone = dev;
					break;
				}
			}
			if (OurPhone is null) {
				MessageBox.Show($"Uh oh -- no {MyPhoneName} detected");
				return;
			}
			MessageBox.Show("Found Phone!");    // TODO:

			// IPortableDeviceContent cont;
			// var phone = new PortableDeviceClass();
			// IPortableDevice

			// TODO: See https://github.com/slowmonkey/WPD-.NET-Wrapper

			// phone.Content(out var cont);
		}

//---------------------------------------------------------------------------------------

		private string GetPdId(string name) {
			var devs = PdInfo.Pds();
			foreach (var item in PdInfo.Pds()) {
				if (item.FriendlyName == name) { return item.ID; }
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		private string GetPdIdByDescription(string name) {
			var devs = PdInfo.Pds();
			foreach (var item in PdInfo.Pds()) {
				if (item.Description == name) { return item.ID; }
			}
			return null;
		}

#if false
		//---------------------------------------------------------------------------------------

		private void foo() {
			nDirsByRate = new Dictionary<int, int>();
			DirsByRate  = new Dictionary<int, List<string>>();
			FileTypes   = new Dictionary<string, int>();



			string ZuneDir = @"G:\$ Zune Master";
			var ZuneDirs = Directory.EnumerateDirectories(ZuneDir, "*", SearchOption.AllDirectories);
			foreach (var dir in ZuneDirs) {
				var xxx = new AlbumInfo(dir);	// TODO:
				string InteriorDir = dir.Substring(ZuneDir.Length);
				var Files = Directory.EnumerateFiles(dir, "*.wma");
				foreach (var file in Files) {
					if (xxx != null) break;	// TODO:
					// var yyy = new AlbumInfo(file);
					string FileType = Path.GetExtension(file);
					FileTypes.TryGetValue(FileType, out int nTypes);
					FileTypes[FileType] = ++nTypes;
					// if (!((FileType == ".mp3") || (FileType.ToLower() == ".mp3"))) { continue; }
					var cut = TagLib.File.Create(file);
					int rate = cut.Properties.AudioBitrate;
					nDirsByRate.TryGetValue(rate, out int n);
					nDirsByRate[rate] = ++n;
					bool bOK = DirsByRate.TryGetValue(rate, out List<string> Dirs);
					if (!bOK) { Dirs = new List<string>(); }
					Dirs.Add(InteriorDir);
					DirsByRate[rate] = Dirs;
					switch (rate) {
					case 128:
						// TODO:
						break;
					case 192:
						Console.WriteLine($"192: {InteriorDir}");
						break;
					case 64:
						// TODO:
						// See http://andrewt.com/2013/06/15/fun-with-mtp-in-c.html
						break;
					default:
						// TODO:
						break;
					}
					break;		// Just check the first song per folder
				}
			}
		}
#endif
	}

#if false
	public class PortableDevice {
		private bool _isConnected;
		private readonly PortableDevice _device;

		public PortableDevice(string deviceId) {
			this._device = new PortableDevice(deviceId);
			this.DeviceId = deviceId;
		}

		public string DeviceId { get; set; }

		public void Connect() {
			if (this._isConnected) { return; }

			var clientInfo = (IPortableDeviceValues)new PortableDeviceValues();
			this._device.Open(this.DeviceId, clientInfo);
			this._isConnected = true;
		}

		public void Disconnect() {
			if (!this._isConnected) { return; }
			this._device.Close();
			this._isConnected = false;
		}
	}
#endif
}

