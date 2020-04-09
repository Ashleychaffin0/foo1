using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Net;

using Microsoft.Win32;

using MicrosoftZuneLibrary;
using MicrosoftZuneInterop;
using LRS.Zune.Classes;

// QueryPropertyBag valid values
//	SyncMappedError
//	CategoryId
//	InLibrary
//	PlaylistId
//	InitTime
//	MediaType
//	Operation
//	SeriesId
//	TOC
//	GenreIds
//	GenreId
//	UniqueID
//	FolderID
//	AlbumIds
//	AlbumId
//	ArtistIds
//	ArtistId
//	ContributingArtistId
//	Keywords
//	QueryView
//	UserId
//	DeviceId


using TestLRSZuneClasses_1.ServiceReference1;

// using LyricsFetcher.org.lyricwiki;
// The namespace created to hold the generated class 

namespace TestLRSZuneClasses_1 {
	class Program {

		static int Main(string[] args) {
			
			List<String> BaseFiles, EnUsFiles;
			bool bOK = LRSZune.AreAllNeededZune30FilesPresent(out BaseFiles, out EnUsFiles);
			if (! bOK) {
				string ExeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				Console.WriteLine("All Zune libraries not available in .exe directory - {0}", ExeDir);
				foreach (var fn in BaseFiles) {
					Console.WriteLine("\tBase file - {0}", fn);
				}
				foreach (var fn in EnUsFiles) {
					Console.WriteLine("\tEN-US file - {0}", fn);
				}
				return 3;
			}
			int InitRetcode;
			var zl = LRSZune.InitializeZune(out InitRetcode);
			if (zl == null) {
				Console.WriteLine("Couldn't init ZuneLibrary - retcode = {0:X}", InitRetcode);
				return 2;
			}

			//var en = System.Environment.GetEnvironmentVariable("PATH");


			// fooIteratorTest();
#if false
			var pics = AllPhotos.GetRecords(zl);

			var xy = VideoWithKeyword.GetRecords(zl, "e");
			Console.Clear();
			foreach (var item in xy) {
				LRSZune.DumpObj(item);
				Console.WriteLine("-------------------------------");
			}
#endif
			
			var qpbw = new QueryPropertyBag();
			// qpb.SetValue("Keywords", "christmas cake");
			// qpb.SetValue("Keywords", "Ella Mae Morse");
			// qpb.SetValue("Keywords", "light fire");
			// qpbw.SetValue("Keywords", "with");
			// qpbw.SetValue("FolderID", 1);
			// qpbw.SetValue("Keywords", "e");
			qpbw.SetValue("Keywords", "potter");
			// qpb.SetValue("Keywords", "Shetland");
			// qpbw.SetValue("GenreID", 8);
			// qpb.SetValue("UniqueID", 16875);
			// qpb.SetValue("AlbumId", 14);
			// AllPodcastEpisodes <- eQueryTypeSubscriptionsEpisodesWithKeyword
			// AllPodcastSeries <- eQueryTypeSubscriptionsSeriesWithKeyword
			using (var w1 = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0, EQuerySortType.eQuerySortOrderNone,
						(uint)(int)SchemaMap.kiIndex_AlbumID, qpbw)) {
				int nRecs = w1.Count;
				var w11 = GetZqlFieldInfo(w1);		// 44 fields
				Console.Clear();
				ShowDataForZuneQueryList(w1, 10);
			}
			var z2 = LRSZune.GetRecordsByProperty<AllAlbums>(zl, EQueryType.eQueryTypeAlbumsWithKeyword, "Keywords", "Waterloo", SortOn.WMTrackNumber, SortDirection.Ascending);
			Console.Clear();
#if false
			ShowDataForZuneQueryList(z1, 3); 
#else
			foreach (var trk in z2) {
				// Console.WriteLine("{0}: {1}, {2} / {3}", trk.AlbumID, trk.WMAlbumTitle, trk.WMTrackNumber, trk.Title);
			}
#endif

#if false
			var albums = GetAlbumsForAlbumArtistId(zl, 7);
			foreach (var album in albums) {
				LRSZune.DumpObj(album);
			}
#endif

#if false
			// var ids = LRSZune.GetAllAlbumIds(zl);
			
			// ShowVideos(zl);

			// Note: AlbumID = 2, SourceURL = C:\$ Zune Master\Cincinnati Pops Orchestra-Erich Kunzel\Christmas with the Pops
			
			//var genres = zl.QueryDatabase(EQueryType.eQueryTypeAllGenres, 0, EQuerySortType.eQuerySortOrderAscending, SchemaMap.kiIndex_CPGenreID, qpb);

			// string lyrics = GetLyrics2("Beatles", "Money");

			//var zql = zl.GetAlbumMetadata(1);
			// TestZuneToXml(zl);
#endif

#if false
			// var x1 = zl.GetAlbumMetadata(/* iAlbumId */ 1);	// Blows
			// var x2 = zl.GetCDDeviceList();		// Returns null
			// var x3 = zl.GetRecorder();			// Returns non-null, but useless
			var x4 = zl.GetTracksByAlbum(0, 14, EQuerySortType.eQuerySortOrderAscending, (uint)(int)SchemaMap.kiIndex_WMTrackNumber);
			// var x41 = GetZqlFieldInfo(x4);		// 43 fields. Missing 2 fields from AllTracks; adds 1 field
			ShowDataForZuneQueryList(x4);

			var x5 = zl.GetTracksByAlbums(new int[] { 14, 269 }, "WMTrackNumber");
			// var x51 = GetZqlFieldInfo(x5);		// 49 fields. Seems to generate AllTracksDetailed
			ShowDataForZuneQueryList(x5);

			var x6 = zl.GetTracksByArtist(0, 7, EQuerySortType.eQuerySortOrderAscending, (uint)(int)SchemaMap.kiIndex_WMAlbumArtist);	
			//	x6 returns null

			var x7 = zl.GetTracksByArtists(new int[] { 43, 12 }, "WMTrackNumber");
			// var x71 = GetZqlFieldInfo(x7);		// Seems to generate AllTracksDetailed as well
			ShowDataForZuneQueryList(x7);		// Has GenreID and Genre

			var x8 = zl.GetTracksByGenres(new int[] { 1, 2, 3 }, "WMTrackNumber");
			// var x81 = GetZqlFieldInfo(x8);			// Seems to be the same as x41
			// ShowDataForZuneQueryList(x8);			// Too many to show

			var x9 = zl.GetTracksByPlaylist(0, 1, EQuerySortType.eQuerySortOrderAscending, (uint)(int)SchemaMap.kiIndex_WMTrackNumber);
			// var x91 = GetZqlFieldInfo(x9);		// 20 fields
			ShowDataForZuneQueryList(x9);	
#endif
			var qpb = new QueryPropertyBag();
			// qpb.SetValue("Keywords", "christmas cake");
			// qpb.SetValue("Keywords", "Ella Mae Morse");
			// qpb.SetValue("Keywords", "light fire");
			qpb.SetValue("Keywords", "a e i o u");
			// qpb.SetValue("Keywords", "e");
			// qpb.SetValue("Keywords", "Shetland");
			// qpb.SetValue("GenreID", "8");
			// qpb.SetValue("UniqueID", "16875");
			// qpb.SetValue("AlbumId", "14");
			using (var y1 = zl.QueryDatabase(EQueryType.eQueryTypeTracksWithKeyword, 0, EQuerySortType.eQuerySortOrderAscending,
						(uint)(int)SchemaMap.kiIndex_WMTrackNumber, qpb)) {
				int nRecs = y1.Count;
				// var y11 = GetZqlFieldInfo(y1);		// 44 fields
				ShowDataForZuneQueryList(y1, 10);
			}

			string [] music, videos, pictures, podcasts, applications;
			string ripFolder, videoMediaFolder, photoMediaFolder, podcastMediaFolder, applicationsFolder;
			zl.GetKnownFolders(out music, out videos, out pictures, out podcasts, out applications,
				out ripFolder, out videoMediaFolder, out photoMediaFolder, out podcastMediaFolder, out applicationsFolder);

#if false
			var Genres = LRSZune.GetGenresFromTracks(zl);
			var qgt = from g in Genres
					 orderby g.Key
					 select g.Key;
			var gt = qgt.ToArray();
			var hsGenres = LRSZune.GetGenresFromAlbums(zl);
			var qhsg2 = from hsg in hsGenres
					   orderby hsg
					   select hsg;
			var ga = qhsg2.ToArray();
#endif

			// All albums for given Artist ID
					// Test_ByID(zl, EQueryType.eQueryTypeAlbumsForAlbumArtistId, "ArtistId", 7, null, null);	// 23 Fields
			//var qq1 = AlbumsForAlbumArtistId.GetRecords(zl, 7);
			// var qq2 = TracksForAlbumId.GetRecords(zl, 7);
			// All Tracks for given Album
			//	Test_ByID(zl, EQueryType.eQueryTypeTracksForAlbumId, "AlbumId", 7, null, null);	// 43 Fields
			// All Tracks per Artist ID (from all albums, so 9 albums * 10 Tracks gives 90 records here
			Test_ByID(zl, EQueryType.eQueryTypeTracksForAlbumArtistId, "ArtistId", 10710, null, null);	// 43 Fields
			var qq3 = TracksForAlbumArtistId.GetRecords(zl, 7);
			// Currently empty
			//		Test_ByID(zl, EQueryType.eQueryTypeEpisodesForSeriesId, "SeriesId", 1, null, null);	// 
			// Currently empty
			//		Test_ByID(zl, EQueryType.eQueryTypePhotosByFolderId, "FolderId", 2, null, null);	// 
			// Test_ByID(zl, EQueryType.eQueryTypeAllPhotos, null, 1, null, null);	// 

			// GetAllGenres(zl);
			// TestZuneToXml(zl);
			// GetGenres(zl);
			// foo1(zl);
			// TestGenresList(zl);
			// GetArtists(zl);
			// ShowAlbumsNew(zl);
			ShowAlbumsByArtists(zl);
			// ShowAlbumCounts(zl);
			// First10Tracks(zl);
			// ShowVideos(zl);
			// AlbumsByArtist(zl);

			Console.WriteLine("TestLRSZuneClasses_1 finished.");
			return 0;
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetAlbumsForAlbumArtistId(ZuneLibrary zl, int ID) {
			return LRSZune.GetRecordsByProperty<AllAlbums>(zl, EQueryType.eQueryTypeAlbumsForAlbumArtistId, "ArtistId", ID);
		}

//---------------------------------------------------------------------------------------

		private static void Test_ByID(ZuneLibrary zl, EQueryType eqt, string IdName1, object IDVal1, string IdName2, object IDVal2) {
			QueryPropertyBag qpb = null;
			if (IdName1 != null) {
				qpb = new QueryPropertyBag();
				qpb.SetValue(IdName1, IDVal1);
				if (IdName2 != null)
					qpb.SetValue(IdName2, IDVal2);
			}
			using (var zql = zl.QueryDatabase(eqt, 0, EQuerySortType.eQuerySortOrderNone, 0, qpb)) {
				Console.Clear();
				ShowDataForZuneQueryList(zql, 10);
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowAlbumsByArtists(ZuneLibrary zl) {
			StreamWriter wtr = new StreamWriter(@"C:\lrs\ZuneAlbumsByArtists.html", false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Zune Albums By Artists</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine(string.Format("<center><FONT SIZE=\"+2\">LRS Zune Albums by Artists as of {0}</FONT></center>\n<br>\n", DateTime.Now));
			wtr.WriteLine("<TABLE border=\"1\">");
			var qryAlbums = from album in AllAlbums.GetRecords(zl)
							where album.WMAlbumArtist.Contains("Rowling")	// TODO:
							orderby album.WMAlbumArtist, album.WMAlbumTitle
							select new { artist = album.WMAlbumArtist, 
								title = album.WMAlbumTitle, 
								hasAlbumArt = album.HasAlbumArt, 
								sourceUrl = album.SourceURL,
								trackingID = album.TrackingID };
			string PrevArtist = "";
			string coverLink = null;
			var Titles = new List<string>();
			var Covers = new List<string>();
			var Urls = new List<string>();
			var AlbumUrls = new HashSet<string>();
			foreach (var album in qryAlbums) {
				//Console.Clear();						// TODO:
				//LRSZune.DumpObj(album);					// TODO:
				AlbumUrls.Add(album.sourceUrl);
				if (album.artist != PrevArtist) {
					FormatTitles(wtr, PrevArtist, Titles, Covers, Urls);	// TODO:
					Titles.Clear();
					Covers.Clear();
					Urls.Clear();
					PrevArtist = album.artist;
				}
				Titles.Add(album.title);
#if true
				if (album.hasAlbumArt) {
					coverLink = Path.Combine(album.sourceUrl, "AlbumArt_" + album.trackingID + "_Small.jpg");
					if (!File.Exists(coverLink)) {
						coverLink = Path.Combine(album.sourceUrl, "AlbumArt_" + album.trackingID + "_Large.jpg");
					}
				} else {
					coverLink = null;
				}
#else
				coverLink = null;
#endif
				Covers.Add(coverLink);
				Urls.Add(album.sourceUrl);
			}
			FormatTitles(wtr, PrevArtist, Titles, Covers, Urls);		// TODO:

			wtr.WriteLine("<p/><p/><b>Albums for Rowling</b><p/>");
			var qUrls = from url in AlbumUrls
						orderby url
						select url;
			foreach (var url in qUrls) {
				wtr.WriteLine("<p>{0}</p>", url);
			}

			wtr.WriteLine("\n</TABLE>\n</BODY>\n</HTML>");
			wtr.Close();
			System.Diagnostics.Process.Start(@"C:\lrs\ZuneAlbumsByArtists.html");
		}

//---------------------------------------------------------------------------------------

		private static void FormatTitles(StreamWriter wtr, string Artist, 
				List<string> Titles, List<string> Covers, List<string> Urls) {
			if (Artist == "") {			// First time through
				return;
			}
			wtr.WriteLine("<tr><td width=\"20%\">{0}</td><td align=\"center\">{1}</td>", Artist, Titles.Count);
			string delim = "";
			bool highlight = false;
			wtr.Write("\t<td>");
			for (int i = 0; i < Titles.Count; i++) {
				string title = Titles[i];
				wtr.Write(delim);
				if (Covers[i] != null) {
					wtr.Write("<IMG SRC=\"{0}\"</IMG> ", Covers[i]);	// TODO:
				}
				if (highlight) {
					wtr.Write(string.Format("<FONT COLOR=\"RED\">{0}</FONT>", title));
				} else {
					wtr.Write(title);
				}
				string url = Urls[i];
				if (!url.StartsWith(@"xxxC:\$ Zune Master\J. K. Rowling ")) { // TODO:
					wtr.WriteLine(string.Format("<FONT COLOR=\"BLUE\">[{0}]</FONT>", url));	// TODO:
				}

				highlight = !highlight;
				delim = "<p/>";
			}
			wtr.WriteLine("\n\t</td>");
			wtr.WriteLine("</tr>");
		}

//---------------------------------------------------------------------------------------

		static void GetGenres(ZuneLibrary zl) {
			Console.WriteLine("Entering GetGenres...");
#if true
			QueryPropertyBag qpb = null;
#else
			var qpb = new QueryPropertyBag();
			qpb.SetValue("GenreId", 1);
#endif


			foreach (var sm in Enum.GetValues(typeof(SchemaMap))) {
				uint smu = (uint)(int)sm;
				if (smu == (uint)SchemaMap.kiIndex_MediaType)		// Blows
					continue;
				// Console.WriteLine("About to break in GetGenres");
				// System.Diagnostics.Debugger.Break();
				// Console.WriteLine("Back from break in GetGenres");
				using (var gens = zl.QueryDatabase(EQueryType.eQueryTypeAllGenres, 0, EQuerySortType.eQuerySortOrderAscending,
								smu, qpb)) {
					if (gens != null) {
						Console.WriteLine("Genres count = {0}", gens.Count);
					}
				}
			}
			Console.WriteLine("Finished GetGenres");
		}

//---------------------------------------------------------------------------------------

		static void GetAllGenres(ZuneLibrary zl) {
			EQueryType eqt = EQueryType.eQueryTypeAllGenres;
			QueryPropertyBag qpb = new QueryPropertyBag();
			qpb.SetValue("GenreId", 1);
			using (ZuneQueryList zql = zl.QueryDatabase(eqt, 0, EQuerySortType.eQuerySortOrderNone, (uint)(int)SchemaMap.kiIndex_CPGenreID, qpb)) {
				ShowDataForZuneQueryList(zql);
			}
			var Albums = AllAlbums.GetRecords(zl);
			var Genres = new HashSet<string>();
			foreach (var album in Albums) {
				Genres.Add(album.WMGenre);
			}
		}

//---------------------------------------------------------------------------------------

		private static void TestGenresList(ZuneLibrary zl) {
			ZuneQueryList zql;
			var GenresList = new List<int>() { 1, 2, 3 };
			zql = zl.GetAlbumsByGenres(GenresList, "Ascending");
			var info = GetZqlFieldInfo(zql);
			var AlbumsByGenres = LRSZune.GetRecords<AllAlbums>(zl, zql);
			// var AlbumsByGenres = LRSZune.GetRecords<AlbumsByGenreId>(zl, zql);
			Console.WriteLine("Albums by Genres Count = {0}", AlbumsByGenres.Count());
			var xxx = AlbumsByGenres;// .Take(10);
			var Genres = new HashSet<string>();
			foreach (var alb in xxx) {
				// LRSZune.DumpObj(alb);
				Console.WriteLine("{0} {1} {2} {3}", alb.WMGenre, alb.AlbumID, alb.WMAlbumArtist, alb.WMAlbumTitle);
				//Console.WriteLine("--------------");
				Genres.Add(alb.WMGenre);
			}
			zql.Dispose();
		}

//---------------------------------------------------------------------------------------

		private static Dictionary<SchemaMap, Type> GetZqlFieldInfo(ZuneQueryList zql) {
			var map = new Dictionary<SchemaMap, Type>();
			var types = new Type[] { typeof(bool), typeof(int), typeof(long), 
									 typeof(string), typeof(DateTime), 
									 typeof(EMediaTypes),
									 typeof(ArrayList) };
			foreach (var sm in Enum.GetValues(typeof(SchemaMap))) {
				foreach (var ty in types) {
					var o = zql.GetFieldValue(0, ty, (uint)(int)sm);
					if (o != null) {
						map.Add((SchemaMap)sm, ty);
						break;
					}
				}
			}
			return map;
		}

//---------------------------------------------------------------------------------------

		static void ShowDataForZuneQueryList(ZuneQueryList zql) {
			ShowDataForZuneQueryList(zql, uint.MaxValue);
		}

//---------------------------------------------------------------------------------------

		static void ShowDataForZuneQueryList(ZuneQueryList zql, uint MaxRecs) {
			if (zql == null) {
				Console.WriteLine("Error - null ZuneQueryList");
				return;
			}
			var info = GetZqlFieldInfo(zql);
			for (uint n = 0; n < zql.Count; n++) {
				if (n > MaxRecs)			// In case we generate too much
					break;
				int FieldNo = 1;
				foreach (SchemaMap sm in info.Keys) {
					Type typ = info[sm];
					string AtomName = ZuneQueryList.AtomToAtomName((int)sm);
					object data = zql.GetFieldValue(n, typ, AtomName);
					if (data is System.Collections.ArrayList)
						data = FmtArrayList((System.Collections.ArrayList)data);
					Console.WriteLine("[{0}] {1} = {2}", FieldNo++, AtomName, data);
				}
				Console.WriteLine("{0} record(s) ------------------------------------------\n", zql.Count);
			}
		}

//---------------------------------------------------------------------------------------

		private static void GetArtists(ZuneLibrary zl) {
			var Artists = AllAlbumArtists.GetRecords(zl, SortOn.WMAlbumArtist);
			foreach (var artist in Artists) {
				Console.WriteLine("* {0}, {1} album(s)", artist.DisplayArtist, artist.ArtistAlbumCount);
				var Albums = LRSZune.GetAlbumsByArtists(zl, SortOn.WMAlbumTitle, artist.AlbumIDAlbumArtist);
				foreach (var album in Albums) {
					// Console.WriteLine();
					// LRSZune.DumpObj(album);
					Console.WriteLine("\t{0}", album.WMAlbumTitle);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void foo1(ZuneLibrary zl) {
			EListType et = EListType.eArtistList;
			var pb = new QueryPropertyBag();
			int MediaId = (int)EMediaTypes.eMediaTypeAudioWMA;
			int cValues = 1;
			var ColIndexes = new int[cValues];
			ColIndexes[0] = (int)SchemaMap.kiIndex_AlbumID;
			var FieldValues = new object[cValues];
			var IsEmptyValues = new bool[cValues];
			ColIndexes[0] = 1;
			var hr = ZuneLibrary.GetFieldValues(MediaId, et, cValues, ColIndexes, FieldValues, IsEmptyValues, pb);

			using (ZuneQueryList zql = zl.QueryDatabase(EQueryType.eQueryTypeAllGenres, 0, EQuerySortType.eQuerySortOrderNone, (uint)0, null)) {
				int n = zql.Count;
				bool b = zql.IsEmpty;
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowAlbumsNew(ZuneLibrary zl) {
			var albs = AllAlbums.GetRecords(zl, SortOn.AlbumID);
			var q1 = (from album in albs
					  select album).Take(10);
			foreach (var item in q1) {
				Console.WriteLine("\n**** Album Info");
				//ObjectDumper.Write(item);
				LRSZune.DumpObj(Console.Out, item);
				//var trks = zl.GetTracksByAlbum(0, item.AlbumID, EQuerySortType.eQuerySortOrderAscending, (uint)SchemaMap.kiIndex_WMTrackNumber);
				//var Trks = new AllTracks(zl); 
				//for (uint i = 0; i < trks.Count; i++) {
				//    Trks.PopulateInstance(trks, i);
				//    Console.WriteLine("\n ===== Track Info Follows\n");
				//    LRSZune.DumpObj(Trks);
				//    Console.WriteLine("\n ===== End of Track Info\n");
				//}
			}
		}

//---------------------------------------------------------------------------------------

		public static string GetLyrics2(string Artist, string Title) {
			LyricWikiPortTypeClient lyricWiki = new LyricWikiPortTypeClient(); 
			LyricsResult lyricsResult = lyricWiki.getSong(Artist, Title); 
			return lyricsResult.lyrics; 
		}

//---------------------------------------------------------------------------------------
		
		public static string GetLyrics(string Artist, string Title) {
			string queryUrl = String.Format("http://webservices.lyrdb.com/lookup" +
							  ".php?q={0}|{1}&for=match", Artist, Title);
			WebClient client = new WebClient();
			string result = client.DownloadString(queryUrl);
			if (result == String.Empty)
				return String.Empty;

			foreach (string x in result.Split('\n')) {
				string id = x.Split('\\')[0];
				Uri lyricsUrl = new Uri("http://webservices.lyrdb.com/getlyr.php?q=" + id);
				string lyrics = client.DownloadString(lyricsUrl);
				if (lyrics != String.Empty)
					return lyrics;
			}
			return String.Empty;
		}

//---------------------------------------------------------------------------------------

#if true
		private static void TestZuneToXml(ZuneLibrary zl) {
			var albs = new AllAlbums(zl);
			var xml = albs.ZuneToXml(SortOn.AlbumID, "ZuneAlbums", "Album");
			string txt = xml.ToString();
			//Console.WriteLine("{0}", txt);
			using (StreamWriter wtr = new StreamWriter("LRSZuneXml.xml")) {
				wtr.WriteLine(txt);
			}
		}
#endif

//---------------------------------------------------------------------------------------

#if true
		private static void First10Tracks(ZuneLibrary zl) {
			var trks = AllTracks.GetRecords(zl, SortOn.AlbumID);
			foreach (AllTracks item in trks.Take(10)) {
				Console.WriteLine("\nContributing Artists={0}", FmtArrayList(item.ContributingArtists));
				ObjectDumper.Write(item);
			}
		}
#endif

//---------------------------------------------------------------------------------------

#if true
		private static void ShowVideos(ZuneLibrary zl) {
			var qryVideos = from vid in AllVideos.GetRecords(zl, SortOn.Title)
							// select new {vid.Duration, vid.Description, vid.Bitrate};
							// where vid.Title.StartsWith("Testing")
							// orderby vid.Title
							select vid;

			foreach (var item in qryVideos) {
				Console.WriteLine();
				Console.WriteLine("Bitrate={0:N0}, Duration={1}, FileSize={2:N0}, Title={3}",
					item.Bitrate, ToHMS(item.Duration), item.FileSize, item.Title);
				ObjectDumper.Write(item);
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private static string FmtArrayList(System.Collections.ArrayList arrayList) {
			StringBuilder sb = new StringBuilder();
			foreach (var c in arrayList) {
				sb.AppendFormat("{0}; ", c);
			}
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private static string ToHMS(int dur) {
			dur /= 1000;			// dur comes in as milliseconds
			int h = dur / 3600;
			dur -= h * 3600;
			int m = dur / 60;
			int s = dur - m * 60;
			return string.Format("{0}:{1:D2}:{2:D2}", h, m, s);
		}
	}
}
