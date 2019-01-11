using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Drawing;

using MicrosoftZuneLibrary;

using LRS.Zune.Classes;
using System.Reflection;

namespace ZuneAlbumsByArtists {
	class ZuneAlbumsAndArtists {
		static int Main(string[] args) {
			ZuneLibrary zl;
			int rc = InitLRSZune(out zl);
			if (rc != 0) {
				return rc;
			}

			PromptUser(zl);

			return 0;
		}

//---------------------------------------------------------------------------------------

		private static void PromptUser(ZuneLibrary zl) {
			bool QuitSwitch = false;
			while (! QuitSwitch) {
					
				Console.WriteLine("\nChoose one of the following (no need to hit Enter):"
					+ "\n\t1   Show newly added albums (after 1/1/2009)"
					+ "\n\t2   Show track title counts by first letter (long running)"
					+ "\n\t3   Show Albums by Artists (long running)"
					+ "\n\t4   Show tracks with \"Various Artists\" as the Artist"
					+ "\n\t5   Show duplicate tracks (long running)"
					+ "\n\n\tA   All of the above"
					+ "\n\tX   Exit");
				var c = Console.ReadKey();
				Console.WriteLine();
				switch (c.KeyChar.ToString().ToUpper()) {
				case "1":
					ShowNewlyAddedAlbums(zl, new DateTime(2009, 1, 1));
					break;
				case "2":
					ShowTrackTitleCountsByFirstLetter(zl);
					break;
				case "3":
					ShowAlbumsByArtists(zl);
					break;
				case "4":
					ShowTracksWith_VariousArtists_Artist(zl);
					break;
				case "5":
					ShowDuplicateTracks(zl);
					break;
				case "A":
					ShowNewlyAddedAlbums(zl, new DateTime(2009, 1, 1));
					ShowTrackTitleCountsByFirstLetter(zl);
					ShowAlbumsByArtists(zl);
					ShowTracksWith_VariousArtists_Artist(zl);
					ShowDuplicateTracks(zl);
					break;
				case "X":
					QuitSwitch = true;
					break;
				default:
					Console.WriteLine("Unrecognized command. Try again.");
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowDuplicateTracks(ZuneLibrary zl) {
#if false		// Enable for testing
			var TracksAll = AllTracks.GetRecords(zl);
			int nTracks = 4000;
			int	n = 0;
			var Tracks = new List<AllTracks>(nTracks);
			foreach (var trk in TracksAll) {
				if (++n > nTracks)
					break;
				Tracks.Add(trk);
			}
#else
			var Tracks = AllTracks.GetRecords(zl);
#endif


			// Note: I'd originally written the following query:
			//		var qry = from trk in Tracks
			//				  orderby trk.Title, trk.Duration
			//				  select trk;

			// Now this seems straightforward enough. Except that with 22,000+ tracks in
			// the collection, it took 69 seconds to run. Which was longer than I'd like.
			// But then I realized that some of this was related to the fact that an
			// <AllTracks> could be a fairly large data structure. And sorting it could
			// involve a lot of memory-intensive data movement. Not to mention possible 
			// garbage collections. So I split the query up into two. The first gets just
			// the fields I need; the second sorts just that subset. And ya know what?
			// The time to run the combined queries dropped from 69 seconds to 51. Almost
			// a 25% improvement. Not too shabby! Who says two can't run as quickly (or
			// even more so) than one?
			var qry1 = from trk in Tracks
					   // where trk.Title == "Alberta"		// For debugging
					   select new { trk.Title, trk.Duration, trk.DisplayArtist, trk.WMAlbumTitle, trk.FileSize };
			var qry = from trk in qry1
					  orderby trk.Title, trk.Duration descending
					  select trk;

			string PrevTitle = null;
			int PrevDuration = -1;
			string PrevDisplayArtist = null;
			string PrevWMAlbumTitle = null;
			long PrevFileSize = -1;

			long DupSize = 0;			// Size of duplicate files
			int nDups = 0;
			int nRecs = 0;
			bool bFirstInSeries = false;
			int MaxDelta = 5000;

			string filename = @"ZuneDuplicateTracks.html";
			StreamWriter wtr = new StreamWriter(filename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Various Zune Reports</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine("<center><FONT SIZE=\"+2\">Zune Duplicate Tracks</FONT></center>\n<br>\n");
			wtr.WriteLine("<TABLE border=\"1\">");
			wtr.WriteLine("<tr><td>Track</td><td>Duration</td><td>Delta</td><td>File Size</td>"
				+ "<td>Album</td><td>Artist</td></tr>");


			foreach (var trk in qry) {
				++nRecs;
				if (nRecs % 1000 == 0) {
					Console.WriteLine("ShowDuplicateTracks: Processing record {0}", nRecs);
					// break;			// Uncomment for debugging
				}
				if (trk.Title != PrevTitle) {
					PrevTitle = trk.Title;
					PrevDuration = trk.Duration;
					PrevDisplayArtist = trk.DisplayArtist;
					PrevWMAlbumTitle = trk.WMAlbumTitle;
					PrevFileSize = trk.FileSize;
					bFirstInSeries = true;
				} else {			// Same title
					int Delta = Math.Abs(trk.Duration - PrevDuration);
					if (Delta <= MaxDelta) {			// In milliseconds
						++nDups;
						DupSize += trk.FileSize;

						if (bFirstInSeries) {
							wtr.WriteLine("<tr>");
							wtr.WriteLine("\t<td>{0}</td>", PrevTitle);
							wtr.WriteLine("\t<td>&nbsp;{0}&nbsp;</td>", ToHMS(PrevDuration));
							wtr.WriteLine("\t<td>&nbsp;</td>");
							wtr.WriteLine("\t<td>&nbsp;{0:N0}&nbsp;</td>", PrevFileSize);
							wtr.WriteLine("\t<td>{0}</td>", PrevWMAlbumTitle);
							wtr.WriteLine("\t<td>{0}</td>", PrevDisplayArtist);
							wtr.WriteLine("</tr>");
						}

						wtr.WriteLine("<tr>");
						wtr.WriteLine("\t<td></td>");		// trk.Title (suppressed)
						wtr.WriteLine("\t<td>&nbsp;{0}&nbsp;</td>", ToHMS(trk.Duration));
						if (Delta < MaxDelta - 1000) {
							wtr.WriteLine("\t<td>&nbsp;{0}&nbsp;</td>", ToHMS(Delta));
						} else {
							wtr.WriteLine("\t<td>&nbsp;<FONT COLOR=\"RED\">{0}</FONT>&nbsp;</td>", ToHMS(Delta));
						}
						wtr.WriteLine("\t<td>&nbsp;{0:N0}&nbsp;</td>", trk.FileSize);
						wtr.WriteLine("\t<td>{0}</td>", trk.WMAlbumTitle);
						wtr.WriteLine("\t<td>{0}</td>", trk.DisplayArtist);
						wtr.WriteLine("</tr>");
					}
					bFirstInSeries = false;
				}
			}

			wtr.WriteLine("</TABLE>");
			wtr.WriteLine("<br/><br/>{0} duplicate file(s) for {1:N0} bytes", nDups, DupSize);
			wtr.Close();
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private static string ToHMS(int DurInMsec) {
			int DurInSeconds = DurInMsec / 1000;
			int h = DurInSeconds / 3600;
			DurInSeconds -= h * 3600;
			int m = DurInSeconds / 60;
			int s = DurInSeconds - m * 60;
			int msec = DurInMsec % 1000;
			return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", h, m, s, msec);
		}

//---------------------------------------------------------------------------------------

		private static void ShowTracksWith_VariousArtists_Artist(ZuneLibrary zl) {
			// Hmmm. It looks like TracksWithKeywords returns, at most, 200 elements.
			// So we'll have to fall back on the whole magilla. But note that for
			// debugging, it might be useful to use TracksWithKeyword.
			// var Tracks = TracksWithKeyword.GetRecords(zl, "Various Artists");
			var Tracks = AllTracks.GetRecords(zl);

			var qry = from trk in Tracks
					  where trk.DisplayArtist == "Various Artists" && trk.WMGenre != "Classical"
					  orderby trk.WMAlbumTitle, trk.WMTrackNumber
					  select trk;

			string filename = @"ZuneTracksWith_VariousArtists_Artist.html";
			StreamWriter wtr = new StreamWriter(filename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Various Zune Reports</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine("<center><FONT SIZE=\"+2\">Zune Tracks with Artist of \"Various Artists\"</FONT></center>\n<br>\n");
			wtr.WriteLine("<TABLE border=\"1\">");
			wtr.WriteLine("<tr><td>RecNo</td><td>Album</td>"
				+ "<td>Track #</td><td>Track</td></tr>");


			int n = 0;
			foreach (var trk in qry) {
				++n;
				if (n % 1000 == 0) {
					Console.WriteLine("ShowTracksWith_VariousArtists_Artist: Processing record {0}", n);
					// break;			// Uncomment for debugging
				}
				wtr.WriteLine("<tr>");
				wtr.WriteLine("\t<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>",
					n, trk.WMAlbumTitle, trk.WMTrackNumber, trk.Title);
				wtr.WriteLine("</tr>");
			}

			wtr.WriteLine("</TABLE>");
			wtr.Close();
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private static void ShowTrackTitleCountsByFirstLetter(ZuneLibrary zl) {
			var LetterDict = new Dictionary<string, int>();
			var StrangeTitlesList = new List<AllTracks>();

			var Tracks = AllTracks.GetRecords(zl);
			int n = 0;

			foreach (var track in Tracks) {
				++n;
				if (n % 1000 == 0) {
					Console.WriteLine("ShowTrackTitleCounts: Processing record {0}", n);
					// break;			// Uncomment for debugging
				}
				string title = FilterOutArticles(track); // Drop leading The, An, A
				string FirstChar = title.Substring(0, 1);
				if (! LetterDict.ContainsKey(FirstChar)) {
					LetterDict.Add(FirstChar, 0);
				}
				++LetterDict[FirstChar];
				CheckForStrangeTitles(FirstChar, track, StrangeTitlesList);
			}

			string filename = @"ZuneTrackTitleCountsByFirstLetter.html";
			StreamWriter wtr = new StreamWriter(filename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Various Zune Reports</TITLE>\n</HEAD>\n<BODY>");

			ShowTitleCounts(LetterDict, wtr, n);
			ShowStrangeTitles(wtr, StrangeTitlesList);

			wtr.WriteLine("\n\n</BODY>\n</HTML>");
			wtr.Close();
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private static string FilterOutArticles(AllTracks track) {
			string title = track.Title.ToUpper();
			if (title.Length > 4 && title.StartsWith("THE ")) {
				title = title.Substring(4);
			} else if (title.Length > 3 && title.StartsWith("AN ")) {
				title = title.Substring(3);
			} else if (title.Length > 2 && title.StartsWith("A ")) {
				title = title.Substring(2);
			}
			return title;
		}

//---------------------------------------------------------------------------------------

		private static void ShowTitleCounts(Dictionary<string, int> LetterDict, StreamWriter wtr, int nTrks) {
			wtr.WriteLine("<center><FONT SIZE=\"+2\">Zune Track Title Counts by First Letter</FONT></center>\n<br>\n");
			wtr.WriteLine("<TABLE border=\"1\">");
			wtr.WriteLine("<tr><td>First Letter</td><td>Count&nbsp;&nbsp;&nbsp;</td>"
				+ "<td>% of Total</td><td>Cumulative %</td></tr>");

			var Data = from key in LetterDict.Keys
					   orderby LetterDict[key] descending
					   select new { InitialLetter = key, Count = LetterDict[key] };
			int CumCount = 0;				// Cumulative Count
			foreach (var item in Data) {
				CumCount += item.Count;
				wtr.WriteLine("<tr>");
				wtr.WriteLine("<td>{0}</td><td>{1}</td><td>{2:P}</td><td>{3:P}</td>", 
					item.InitialLetter, item.Count, 
					(float)item.Count / nTrks, 
					(float)CumCount / nTrks);
				wtr.WriteLine("</tr>");
			}

			wtr.WriteLine("</TABLE>");
		}

//---------------------------------------------------------------------------------------

		private static void ShowStrangeTitles(StreamWriter wtr, List<AllTracks> StrangeTitlesList) {
			for (int i = 0; i < 5; i++) {
				wtr.WriteLine("<p>&nbsp;</p>");
			}
			wtr.WriteLine("<center><FONT SIZE=\"+2\">Zune Tracks with Strange First Letter</FONT></center>\n<br>\n");
			wtr.WriteLine("<TABLE border=\"1\">");
			foreach (var track in StrangeTitlesList) {
				wtr.WriteLine("<tr>");
				wtr.WriteLine("\t<td>{0}</td>", track.Title);
				wtr.WriteLine("\t<td>{0}</td>", track.DisplayArtist);
				wtr.WriteLine("\t<td>{0}</td>", track.WMAlbumTitle);
				wtr.WriteLine("</tr>");
			}
			wtr.WriteLine("</TABLE>");
		}

//---------------------------------------------------------------------------------------

		private static void CheckForStrangeTitles(string FirstChar, AllTracks track, List<AllTracks> StrangeTitlesList) {
			// We've seen some strange titles go by. Things like first characters that
			// are punctuation characters. Also accented letters. So out of curiosity,
			// show any strange names found.
			// Note: We're including digits 0-9 here for two reasons. First, if the album
			//		 can't be found, it will have a name such as "5 Unknown Track 5".
			//		 Second, some songs do start with digits, such as "1-2-3", or "The
			//		 59th Street Bridge Song".
			// Some reasons for Strange Titles are:
			//	* A surprising number of titles start with "(", such as
			//		* (I Can't Get No) Satisfaction
			//		* (We're Gonna) Rock Around the Clock
			//	* It's not surprising that with all the Celtic content I have, some
			//	  titles begin with accented letters
			//	* And so on...
			string GoodChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789";
			if (! GoodChars.Contains(FirstChar)) {
				StrangeTitlesList.Add(track);
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowNewlyAddedAlbums(ZuneLibrary zl, DateTime StartDate) {
			string	filename = @"ZuneNewlyAddedAlbums.html";
			StreamWriter wtr = new StreamWriter(filename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Zune Newly Added Albums</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine(string.Format("<center><FONT SIZE=\"+2\">Zune Newly Added Albums after {0}</FONT></center>\n<br>\n", StartDate));
			wtr.WriteLine("<TABLE border=\"1\">");

			var qryAlbums = from album in AllAlbums.GetRecords(zl)
							where album.DateAdded >= StartDate
							orderby album.DateAdded, album.WMAlbumArtist, album.WMAlbumTitle
							select new { 
								dateAdded	= album.DateAdded,
								artist		= album.WMAlbumArtist, 
								title		= album.WMAlbumTitle, 
								hasAlbumArt = album.HasAlbumArt, 
								sourceUrl	= album.SourceURL,
								trackingID	= album.TrackingID };
			foreach (var album in qryAlbums) {
				wtr.WriteLine("<tr><td>{0}</td><td>{1}</td><td>{2}</td>", 
					album.dateAdded, album.artist, album.title);
			}

			wtr.WriteLine("\n</TABLE>\n</BODY>\n</HTML>");
			wtr.Close();
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private static void ShowAlbumsByArtists(ZuneLibrary zl) {
			string	filename = @"ZuneAlbumsByArtists.html";
			StreamWriter wtr = new StreamWriter(filename, false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Zune Albums By Artists</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine(string.Format("<center><FONT SIZE=\"+2\">Zune Albums by Artists as of {0}</FONT></center>\n<br>\n", DateTime.Now));
			wtr.WriteLine("<TABLE border=\"1\">");
			var qryAlbums = from album in AllAlbums.GetRecords(zl)
							orderby album.WMAlbumArtist, album.WMAlbumTitle
							select new { 
								artist		= album.WMAlbumArtist, 
								title		= album.WMAlbumTitle, 
								hasAlbumArt = album.HasAlbumArt, 
								sourceUrl	= album.SourceURL,
								trackingID	= album.TrackingID };
			string PrevArtist = "";
			string coverLink = null;
			var Titles = new List<string>();
			var Covers = new List<string>();
			foreach (var album in qryAlbums) {
				if (album.artist != PrevArtist) {
					FormatTitles(wtr, PrevArtist, Titles, Covers);
					Titles.Clear();
					Covers.Clear();
					PrevArtist = album.artist;
				}
				Titles.Add(album.title);
#if false
				if (album.title == "Fit as a Fiddle")
					System.Diagnostics.Debugger.Break();
#endif
				if (album.hasAlbumArt) {
					string sourceUrl = album.sourceUrl;
					string trackingID = album.trackingID;
					coverLink = GetCoverLink(sourceUrl, trackingID);
				} else {
					coverLink = null;
				}
				Covers.Add(coverLink);
			}
			FormatTitles(wtr, PrevArtist, Titles, Covers);
			wtr.WriteLine("\n</TABLE>\n</BODY>\n</HTML>");
			wtr.Close();
			System.Diagnostics.Process.Start(filename);
		}

//---------------------------------------------------------------------------------------

		private static string GetCoverLink(string sourceUrl, string trackingID) {
			// A cover may be
			//		a) A "small" jpg in the sourceUrl
			//		b) A "small" jpg in this directory (put here by a previous run)
			//		c) A "large" jpg in the sourceUrl. In that case, create (if not
			//			already there) a "small" thumbnail in current directory.
			//		d) Theoretically that's all, since we get here only if the Zune
			//		   database says that there *is* AlbumArt. But if we do the
			//		   rest of the checks unsuccessfully, we'll return null.
			// Check them in that order

			// Small jpg in sourceUrl
			string fname = "AlbumArt_" + trackingID + "_Small.jpg";	
			string coverLink = Path.Combine(sourceUrl, fname);
			if (File.Exists(coverLink))
				return coverLink;

			// Small jpg in current directory
			coverLink = Path.GetFullPath(Path.Combine(@".\", fname));
			if (File.Exists(coverLink))
				return coverLink;

			// Large jpg in sourceUrl. If so, convert to small jpg in current directory
			fname = "AlbumArt_" + trackingID + "_Large.jpg";	
			coverLink = Path.Combine(sourceUrl, fname);
			// Resize Large image (if present), and save it, in the current directory,
			// as a Small image.
			if (!File.Exists(coverLink))
				return null;

			var img = Image.FromFile(coverLink);
			var SmallImage = img.GetThumbnailImage(75, 75, null, IntPtr.Zero);
			coverLink = Path.Combine(@".\", "AlbumArt_" + trackingID + "_Small.jpg");
			coverLink = Path.GetFullPath(coverLink);
			SmallImage.Save(coverLink, System.Drawing.Imaging.ImageFormat.Jpeg);
			return coverLink;
		}

//---------------------------------------------------------------------------------------

		private static void FormatTitles(StreamWriter wtr, string Artist, List<string> Titles, List<string> Covers) {
			if (Artist == "") {			// First time through
				return;
			}
			wtr.WriteLine("<tr>\n\t<td width=\"20%\">{0}</td><td align=\"center\">{1}</td>", Artist, Titles.Count);
			wtr.Write("\t<td>");
			for (int i = 0; i < Titles.Count; i++) {
				string title = Titles[i];

				if (Covers[i] != null) {
					wtr.Write("<td valign=\"top\">");
					wtr.Write("<IMG SRC=\"{0}\"</IMG> </td>", Covers[i]);
				}
				wtr.Write("<td valign=\"top\">" + title + "</td>");
				wtr.Write("</td>");
				
			}
			wtr.WriteLine("\n\t</td>");
			wtr.WriteLine("</tr>");
		}

//---------------------------------------------------------------------------------------

		private static int InitLRSZune(out ZuneLibrary zl) {
			zl = null;
			List<String> BaseFiles, EnUsFiles;
			bool bOK = LRSZune.AreAllNeededZune30FilesPresent(out BaseFiles, out EnUsFiles);
			if (!bOK) {
				string ExeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				Console.WriteLine("All Zune libraries not available in .exe directory - {0}", ExeDir);
				foreach (var fn in BaseFiles) {
					Console.WriteLine("\tBase file - {0}", fn);
				}
				foreach (var fn in EnUsFiles) {
					Console.WriteLine("\t\\EN-US file - {0}", fn);
				}
				return 3;
			}
			int InitRetcode;
			zl = LRSZune.InitializeZune(out InitRetcode);
			if (zl == null) {
				Console.WriteLine("Couldn't init ZuneLibrary - retcode = {0:X}", InitRetcode);
				return 2;
			}
			return 0;
		}
	}
}
