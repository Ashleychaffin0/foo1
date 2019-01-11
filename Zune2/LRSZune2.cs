using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;	// For GUID class
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using Microsoft.Zune.Configuration;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;

using Microsoft.Iris;
using Microsoft.Iris.Accessibility;
using Microsoft.Iris.OS;
using Microsoft.Iris.RenderAPI;

using MicrosoftZuneInterop;
using MicrosoftZuneLibrary;
using MicrosoftZunePlayback;

using LRS.Zune.Classes;
using LRS.Zune.ClassGeneration;

// TODO:
//	1)	Get rid of SchemaToType
//	1)	None at the moment. Amazing!


// See Tom Fuller's entry at
// http://soapitstop.com/blogs/fleamarket/archive/2008/03/03/read-the-zune-collection-in-net-from-zune-s-own-api.aspx

// Note: A compilation album that got a DisplayArtist right is 
//	C:\$ Zune Master\Various Artists\16 Most Requested Songs of the 1950's, Vol. 1\3 Jezebel.wma
//		 But be careful. The DisplayArtist is "Frankie Lane" (sic).

// Once we get the CE Compact database opened (we hope), to get a list of all the tables, use
//		SELECT * FROM INFORMATION_SCHEMA.TABLES
// or perhaps
//		select name from sys.objects where type = 'U'

// Schema mappings: Type | enum
//	string	|	SchemaMap.kiIndex_DisplayArtist
//	int		|	SchemaMap.kiIndex_AlbumID

namespace Zune2 {
	public partial class LRSZune2 : Form {

		// const string ZuneDir = @"C:\Program Files\Zune";
		MicrosoftZuneLibrary.ZuneLibrary zl;

		Dictionary<SchemaMap, Type> SchemaToType = new Dictionary<SchemaMap, Type>();

//---------------------------------------------------------------------------------------

		public LRSZune2() {
			InitializeComponent();

			zl = new ZuneLibrary();
			bool dbRebuilt;
			int i1 = zl.Initialize("", out dbRebuilt);
			if (i1 != 0) {
				string	msg = string.Format("Could not initialize the Zune Library, rc = {0:X8}", i1);
				MessageBox.Show(msg);
				System.Windows.Forms.Application.Exit();
			}

			SetupSchemaToType();

			// zl.ExecuteQueryHelper(...);

		}

//---------------------------------------------------------------------------------------

		private void SetupSchemaToType() {
			SchemaToType[SchemaMap.kiIndex_DisplayArtist] = typeof(string);
			SchemaToType[SchemaMap.kiIndex_AlbumID]		  = typeof(int);
			SchemaToType[SchemaMap.kiIndex_AlbumIDAlbumArtist] = typeof(string);
		}

//---------------------------------------------------------------------------------------

		private void btnShowArtists_Click(object sender, EventArgs e) {
			ShowArtists();
		}

//---------------------------------------------------------------------------------------

		private void ShowArtists() {
			MicrosoftZuneLibrary.ZuneQueryList artists;
			artists = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbumArtists, 0, EQuerySortType.eQuerySortOrderDescending,
				// (uint)SchemaMap.kiIndex_DisplayArtist, null);
				(uint)SchemaMap.kiIndex_ArtistAlbumCount, null);
			// MessageBox.Show("Artist count = " + artists.Count);
			for (uint i = 0; i < artists.Count; i++) {
				object o = artists.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist);
				// o = artists.GetFieldValue(i, typeof(uint), (uint)SchemaMap.kiIndex_DisplayArtistCount);
				Console.WriteLine("Artist[{0}] = '{1}'", i, o);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnShowAlbums_Click(object sender, EventArgs e) {
			// Shows albums, and the tracks therein
			MicrosoftZuneLibrary.ZuneQueryList	albums;
			SchemaMap smt;				// smt = SchemaMap Type
			// smt = SchemaMap.kiIndex_Title;	
			// smt = SchemaMap.kiIndex_AlbumID;
			smt = SchemaMap.kiIndex_AlbumIDAlbumArtist;
			albums = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0, EQuerySortType.eQuerySortOrderAscending,
				(uint)smt, null);
			// MessageBox.Show("Albums count = " + albums.Count);
			ZuneQueryItem it = new ZuneQueryItem(albums, 0);
			for (uint i = 0; i < albums.Count; i++) {
				Type type = SchemaToType[smt];
				// object o = albums.GetFieldValue(i, type, (uint)smt);
				object AlbumId = albums.GetFieldValue(i, typeof(int), (uint)SchemaMap.kiIndex_AlbumID);
				object o = albums.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
				object oo = albums.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
				// o = artists.GetFieldValue(i, typeof(uint), (uint)SchemaMap.kiIndex_DisplayArtistCount);
				Console.WriteLine("Albums[{0}] = '{1}' / {2}", i, o, oo);

				ZuneQueryList trks = zl.GetTracksByAlbum(0, (int)AlbumId, EQuerySortType.eQuerySortOrderAscending, (uint)SchemaMap.kiIndex_CPTrackID);
				if (trks == null) {
					Console.WriteLine("\t*** Tracks not available ***");
					continue;
				}
				for (uint j = 0; j < trks.Count; j++) {
					// object otrk = trks.GetFieldValue(j, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist);
					object otrk = trks.GetFieldValue(j, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Console.WriteLine("\tTrack : {0}", otrk);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGetKnownFolders_Click(object sender, EventArgs e) {
			string []	Music;
			string []	Videos;
			string []	Pictures;
			string []	Podcasts;
			string		RipFolder;

			// TODO:
			// zl.GetKnownFolders(out Music, out Videos, out Pictures, out Podcasts, out RipFolder);
#if false
			foreach (var item in Music) {
				MessageBox.Show("Music Folder: " + item);
			}
#endif
		}

//---------------------------------------------------------------------------------------

		// TODO: Put all failed smt enum names into a List<>, and print them out at the end
		//		 of the EQType. Ditto for Exceptions.
		private void btnTryAllQueryTypes_Click(object sender, EventArgs e) {
			// This attempts sorting, but doesn't seem to work
			MicrosoftZuneLibrary.ZuneQueryList ZQList;
			int	nEQTypes = 0;
			var BadCombinations = new List<SchemaMap>();
			foreach (var EQType in Enum.GetValues(typeof(EQueryType))) {
				Console.WriteLine("\n\n{0,3}: EQTYpe = {1} / {2}", ++nEQTypes, EQType, (int)EQType);
				int		nSmts = 0;
				BadCombinations.Clear();
				foreach (var smt in Enum.GetValues(typeof(SchemaMap))) {
					// Console.WriteLine("\tSMT = {0} / {1}", smt, (int)smt);
					try {
						uint SortAtom = (uint)(int)smt;
						using (ZQList = zl.QueryDatabase((EQueryType)EQType, 0, EQuerySortType.eQuerySortOrderAscending,
							// Note: It seems like the SortAtom affects only (duh) the
							//		 order in which the ZQList is returned. It can even
							//		 be 0.
							// Note: There seems to be something funny here. We keep 
							//		 trying different SortAtom's, but ProcessFields
							//		 keeps printing out the same data. IOW, the sorting
							//		 isn't working.
							// 0 /*(uint)(int)smt*/ , null)) {
							SortAtom, null)) {
							if (ZQList == null) {
								BadCombinations.Add((SchemaMap)smt);
								// Console.WriteLine("\t\t{0} *** Failed", smt);
							} else {
								++nSmts;
								ProcessFields(nSmts, (EQueryType)EQType, (SchemaMap)smt, ZQList);
							}
						}
					} catch (Exception ex) {
						string	msg = string.Format("\t***** Exception thrown for EQType/SchemaMap = {0}/{1} - Exception = {2}",
							EQType, smt, ex.ToString());
						Console.WriteLine("{0}", msg);
					}
				}
				foreach (var item in BadCombinations) {
					Console.WriteLine("\t\t*** Bad Schema {0}", item);
				}
			}
			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void ProcessFields(int n, EQueryType EQType, SchemaMap smt, ZuneQueryList ZQList) {
			Console.WriteLine("\t{0,3}: smt = {1} -- {2} worked, Count={3}", n, EQType, smt, ZQList.Count);
			if (ZQList.Count == 0) {
				return;
			}

			object o;
			// Here's where it could get really intensive. At the moment, I have roughly
			// 7000 tracks in the database. I don't want to through all 374 Schema values
			// for each track. (That's 7000 * 374 = 2.6 million combinations). So we'll
			// just go through the Schema values on ZQList[0]. This should be enough
			// to tell us what's going on.
			uint	ix = 0;				// Index into ZQList
			foreach (var smt2 in Enum.GetValues(typeof(SchemaMap))) {
				// Result type string
				o = ZQList.GetFieldValue(ix, typeof(string), (uint)(int)smt2);
				if (o != null) {
					Console.WriteLine("\t\tSchema = {0}, string->'{1}'", smt2, o);
					continue;
				}
				// Result type int
				o = ZQList.GetFieldValue(ix, typeof(int), (uint)(int)smt2);
				if (o != null) {
					Console.WriteLine("\t\tSchema = {0}, int->{1}", smt2, o);
					continue;
				}
				// Result type uint
				o = ZQList.GetFieldValue(ix, typeof(uint), (uint)(int)smt2);
				if (o != null) {
					Console.WriteLine("\t\tSchema = {0}, uint->{1}", smt2, o);
					continue;
				}
				// Result type DateTime
				o = ZQList.GetFieldValue(ix, typeof(DateTime), (uint)(int)smt2);
				if (o != null) {
					Console.WriteLine("\t\tSchema = {0}, DateTime->{1}", smt2, o);
					continue;
				}
				// Result type GUID
				o = ZQList.GetFieldValue(ix, typeof(Guid), (uint)(int)smt2);
				if (o != null) {
					Console.WriteLine("\t\tSchema = {0}, Guid->{1}", smt2, o);
					continue;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnQueryTypeAndSchemae_Click(object sender, EventArgs e) {

			var QandS = GenerateQueriesAndSchemae.GetQueriesAndSchemae(zl);

			PrintQueriesAndSchemae(QandS);

			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void PrintQueriesAndSchemae(Dictionary<EQueryType, List<SchemaMapAndType>> QueriesAndSchemae) {
			foreach (EQueryType eqt in QueriesAndSchemae.Keys) {
				Console.WriteLine("\n\nQuery Type {0} / {1}", eqt, (int)eqt);
				foreach (var SmtAndType in QueriesAndSchemae[eqt]) {
					Console.WriteLine("\t\tSchema {0} / {1} \t-> {2}", 
						SmtAndType.smt, (int)SmtAndType.smt, SmtAndType.type);
				}
			}
		}


//---------------------------------------------------------------------------------------

		private void btnSaveQueriesAndSchemae_Click(object sender, EventArgs e) {
			GenerateQueriesAndSchemae.SaveQueriesAndSchemae(zl, "QueriesAndSchemae.xml");
		}

//---------------------------------------------------------------------------------------

		private void btnGenerateClasses_Click(object sender, EventArgs e) {
			const string EnumValName = "EnumVal";

			GenerateLRSZuneClasses.CreateLRSZuneClasses(EnumValName);
		}

//---------------------------------------------------------------------------------------

		private void btnGetAlbumsViaClasses_Click(object sender, EventArgs e) {
			// This is the old way. We'll leave it in as a sample
			using (ZuneQueryList zql = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0,
				EQuerySortType.eQuerySortOrderAscending, 0, null)) {
				if (zql == null) {
					MessageBox.Show("Unable to process Albums");
					return;
				}
				StreamWriter wtr = new StreamWriter("LRSZune_Albums.txt");
				var Albums = new List<eQueryTypeAllAlbums>(zql.Count);
				for (uint i = 0; i < zql.Count; i++) {
					Albums.Add(new eQueryTypeAllAlbums(zql, i));
				}
				foreach (var album in Albums) {
					wtr.WriteLine();
					ZuneTypeFactory.DumpObj(wtr, album); 
				}
				wtr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		private void btnFoo_Click(object sender, EventArgs e) {
			// Fart around with Property Bags
			// EListType
			// EMediaStateType
			// EMediaCategories
			// EQueryTypeView
			var pb = new QueryPropertyBag(); 
			using (ZuneQueryList zql = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0,
				EQuerySortType.eQuerySortOrderAscending, 0, pb)) {
				Type t = pb.GetType();
				MethodInfo mi = t.GetMethod("GetIQueryPropertyBag");
				System.Reflection.Pointer p = (System.Reflection.Pointer)mi.Invoke(pb, null);
				int		DumpNBytes = 2048;
				unsafe {
					GC.Collect();		// Minimize object movement in what follows
					// Note: To really minimize data movement, copy the data to a buffer,
					//		 then format that. TODO:
					byte *pp = (byte *)Pointer.Unbox(p);
					DumpBytePtr(pp, DumpNBytes, 32);
					Console.WriteLine();
					uint *pu = (uint *)pp;
					DumpBytePtr((byte *)pu[2], DumpNBytes, 32);
				}
				Console.WriteLine("\nDone");
				Console.WriteLine(zql);
			}
		}

//---------------------------------------------------------------------------------------

		unsafe void DumpBytePtr(byte* p, int nBytes, int ColumnWidth) {
			byte *q = p;
			for (int nRow = 0; nRow < nBytes; nRow += ColumnWidth, q += ColumnWidth) {
				int width = ColumnWidth;		// TODO:
				// IntPtr doesn't implement IFormattable, so it's always formatted
				// as a simple numeric (base 10) string. So to get it into hex, we
				// have to dance around a bit.
				string sAddr = (new  IntPtr((void*)q)).ToString();
				uint addr = uint.Parse(sAddr);
				Console.Write("{0,8:X}: ", addr);
				for (int i = 0; i < width; i++) {
					Console.Write("{0,02:X} ", q[i]);
				}
				Console.Write("  *");
				for (int i = 0; i < width; i++) {
					char c;
					if ((q[i] >= 0x20) && (q[i] < 0x7f))
						c = Convert.ToChar(q[i]);
					else
						c = '.';
					Console.Write("{0,2} ", c);
				}
				Console.WriteLine("*");
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGetArtistsViaClasses_Click(object sender, EventArgs e) {
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAlbumsForAlbumArtistId>(zl, "LRS_Zune_Album_Artist_ID.txt");
		}

//---------------------------------------------------------------------------------------

		private void btnGetTracksViaClasses_Click(object sender, EventArgs e) {
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllTracks>(zl, "LRS_Zune_Tracks.txt");
		}

//---------------------------------------------------------------------------------------

		private void DumpAllZune_Click(object sender, EventArgs e) {
			// TODO: Make this a method of ZuneTypeFactory?
			// TODO: Sort and group by category (Albums, Photos, etc)
			// TODO: Put up status msg, "Processing Videos", etc
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllVideos>(zl, "LRS_Zune_Videos.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllVideosDetailed>(zl, "LRS_Zune_Videos_Detailed.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllTracksDetailed>(zl, "LRS_Zune_Tracks_Detailed.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeVideosByFolderId>(zl, "LRS_Zune_Videos_By_Folder_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllPhotos>(zl, "LRS_Zune_Photos.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypePhotosByFolderId>(zl, "LRS_Zune_Photos_By_Folder_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllAlbums>(zl, "LRS_Zune_Albums.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllAlbumArtists>(zl, "LRS_Zune_Album_Artists.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAlbumsForAlbumArtistId>(zl, "LRS_Zune_Album_Artist_ID.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAlbumsForContributingArtistId>(zl, "LRS_Zune_Contributing_Artist_ID.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAlbumsByTOC>(zl, "LRS_Zune_Albums_By_TOC.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksForAlbumId>(zl, "LRS_Zune_Tracks_For_Album_ID.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksForAlbumArtistId>(zl, "LRS_Zune_Tracks_For_Album_Artist_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllPodcastSeries>(zl, "LRS_Zune_Podcast_Series.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllPodcastEpisodes>(zl, "LRS_Zune_Podcast_Episodes.txt");

			// TODO: More grouping
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeEpisodesForSeriesId>(zl, "LRS_Zune_Episodes_For_Series_ID.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAlbumsWithKeyword>(zl, "LRS_Zune_Albums_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeArtistsWithKeyword>(zl, "LRS_Zune_Artists_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypePhotosWithKeyword>(zl, "LRS_Zune_Photos_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeSubscriptionsSeriesWithKeyword>(zl, "LRS_Zune_Subscriptions_Series_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeSubscriptionsEpisodesWithKeyword>(zl, "LRS_Zune_Subscriptions_Episodes_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeVideoWithKeyword>(zl, "LRS_Zune_Video_With_Keyword.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeMediaFolders>(zl, "LRS_Zune_Media_Folders.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeSyncProgress>(zl, "LRS_Zune_Sync_Progress.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllPlaylists>(zl, "LRS_Zune_Playlists.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypePlaylistContentByPlaylistId>(zl, "LRS_Zune_Playlist_Contents_By_Playlist_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllGenres>(zl, "LRS_Zune_Genres.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksForGenreId>(zl, "LRS_Zune_Tracks_For_Genre_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksWithKeyword>(zl, "LRS_Zune_Tracks_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllTracks>(zl, "LRS_Zune_Tracks.txt");
		}

//---------------------------------------------------------------------------------------

		private void btnGetVideosViaClasses_Click(object sender, EventArgs e) {
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllVideos>(zl, "LRS_Zune_Videos.txt");
		}

//---------------------------------------------------------------------------------------

		private void btnVideosToXml_Click(object sender, EventArgs e) {
			XDocument xd = ZuneTypeFactory.ZuneToXml<eQueryTypeAllVideos>(zl, "LRSZuneVideos", "Video");
			xd.Save("LRS_Videos.xml");
		}

//---------------------------------------------------------------------------------------

		private void btnAlbumsToXml_Click(object sender, EventArgs e) {
			XDocument xd = ZuneTypeFactory.ZuneToXml<eQueryTypeAllAlbums>(zl, "LRSZuneAlbums", "Album");
			xd.Save("LRS_Albums.xml");
		}

//---------------------------------------------------------------------------------------

		private void btnTracksToXml_Click(object sender, EventArgs e) {
			XDocument xd = ZuneTypeFactory.ZuneToXml<eQueryTypeAllTracks>(zl, "LRSZuneTracks", "Track");
			xd.Save("LRS_Tracks.xml");
		}

//---------------------------------------------------------------------------------------

		private void btnTotalPlayingTime_Click(object sender, EventArgs e) {

		}
	}
}