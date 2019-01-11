// Copyright (c) 2008-2009 by Larry Smith
    
// This file generated at Friday, April 17, 2009 at 11:22:51 AM

// NOTE: THIS FILE HAS BEEN GENERATED FROM UNDOCUMENTED ZUNE LIBRARIES. USE OF THESE
//		 ROUTINES IS YOUR OWN RESPONSIBILITY. POSSIBLE SIDE EFFECTS MAY INVOLVE
//		 CORRUPTION OR DELETION OF DATA. NO WARRANTIES ARE EXPRESSED OR IMPLIED BY
//		 PUBLICATION OF THIS CODE.

// Notes:
//  *   All classes are defined to be partial classes. This makes it possible to extend
//      these classes with your own routines. Alternatively, use extension methods.
//  *   Some classes are empty. If you don't have any (say) Videos in your collection,
//      then the Video classes will be empty. But if you add a video and re-run the
//      utility that created this file, then you should see a non-empty Video class.
//  *   But some classes never seem to be anything other than empty. Perhaps these are
//      reserved for future use?

using System;
using System.Collections;			// For ArrayList
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.Win32;

using MicrosoftZuneLibrary;
using MicrosoftZuneInterop;

namespace LRS.Zune.Classes {

	public interface Populateable {
		void PopulateInstance(ZuneQueryList zql, uint n);
	}
	
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public abstract class LRSZune {
		protected ZuneLibrary zl;		// TODO: Currently used only for GetRecordsForXml

//---------------------------------------------------------------------------------------

		public static IEnumerable<T> GetRecords<T>(ZuneLibrary zl, ZuneQueryList zql) where T : class, Populateable {
			if (zql == null) {
				yield break;
			}
			Type t = typeof(T);
			object[] parms = { zl };
			for (uint i = 0; i < zql.Count; i++) {
				T o = Activator.CreateInstance(t, parms) as T;
				o.PopulateInstance(zql, i);
				yield return o;
			}
			zql.Dispose();
			yield break;
		}


//---------------------------------------------------------------------------------------

		public static IEnumerable<TClass> GetRecordsByProperty<TClass>(ZuneLibrary zl, EQueryType eqt, string PropertyBagKey, object ID) 
					where TClass : class, Populateable {
			return GetRecordsByProperty<TClass>(zl, eqt, PropertyBagKey, ID, SortOn.Abstract, SortDirection.None);
		}
		
		
//---------------------------------------------------------------------------------------

		public static IEnumerable<TClass> GetRecordsByProperty<TClass>(ZuneLibrary zl, EQueryType eqt, string PropertyBagKey, object ID, SortOn so, SortDirection sd) 
					where TClass : class, Populateable {
			var qpb = new QueryPropertyBag();
			qpb.SetValue(PropertyBagKey, ID);
			var zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, qpb);
			return LRSZune.GetRecords<TClass>(zl, zql);				
		}

//---------------------------------------------------------------------------------------
		
		public static IEnumerable<AllAlbums> GetAlbumsByArtists(ZuneLibrary zl, SortOn order, params int [] ArtistList) {
			string s = ZuneQueryList.AtomToAtomName((int)order);
			ZuneQueryList zql = zl.GetAlbumsByArtists(ArtistList, s);
			return LRSZune.GetRecords<AllAlbums>(zl, zql);
		}

//---------------------------------------------------------------------------------------
		
		public static IEnumerable<AllAlbums> GetAlbumsByGenres(ZuneLibrary zl, SortOn order, params int [] GenresList) {
			string s = ZuneQueryList.AtomToAtomName((int)order);
			ZuneQueryList zql = zl.GetAlbumsByGenres(GenresList, s);
			return LRSZune.GetRecords<AllAlbums>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetTracksByAlbum(ZuneLibrary zl, SortOn order, int AlbumId) {
			ZuneQueryList zql = zl.GetTracksByAlbum(0, AlbumId, EQuerySortType.eQuerySortOrderAscending, (uint)order);
			return LRSZune.GetRecords<AllTracks>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetTracksByArtist(ZuneLibrary zl, SortOn order, int ArtistId) {
			ZuneQueryList zql = zl.GetTracksByArtist(0, ArtistId, EQuerySortType.eQuerySortOrderAscending, (uint)order);
			return LRSZune.GetRecords<AllTracks>(zl, zql);
		}

//---------------------------------------------------------------------------------------
		
		public static IEnumerable<AllAlbums> GetTracksByArtists(ZuneLibrary zl, SortOn order, params int [] ArtistsList) {
			string s = ZuneQueryList.AtomToAtomName((int)order);
			ZuneQueryList zql = zl.GetAlbumsByGenres(ArtistsList, s);
			return LRSZune.GetRecords<AllAlbums>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetTracksByGenres(ZuneLibrary zl, SortOn order, params int[] GenresList) {
			string s = ZuneQueryList.AtomToAtomName((int)order);
			ZuneQueryList zql = zl.GetTracksByGenres(GenresList, s);
			return LRSZune.GetRecords<AllTracks>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetTracksByPlaylist(ZuneLibrary zl, SortOn order, int PlayListId) {
			ZuneQueryList zql = zl.GetTracksByPlaylist(0, PlayListId, EQuerySortType.eQuerySortOrderAscending, (uint)order);
			return LRSZune.GetRecords<AllTracks>(zl, zql);
		}

//---------------------------------------------------------------------------------------
		
		public static HashSet<string> GetGenresFromAlbums(ZuneLibrary zl) {
			// I haven't yet figured out the fast way to all the Genres, so, in this case
			// we'll go through all the Albums and get the Genres that way. The HashSet
			// returned gives just the names of the Genres (e.g. "Rock", "Folk", etc).
			// See also the comments in GetGenresFromTracks.
			var HashSetGenre = new HashSet<string>();
			var Albums = AllAlbums.GetRecords(zl);
			foreach (var album in Albums) {
				HashSetGenre.Add(album.WMGenre);
			}
			return HashSetGenre;
		}

//---------------------------------------------------------------------------------------

		public static Dictionary<string, int> GetGenresFromTracks(ZuneLibrary zl) {
			// An Album can have an overall Genre, but each Track can have a Genre as 
			// well. There might not be a 1-1 correspondence between the Genres seen this
			// way, and those seen by calling GetGenresFromAlbums. For example, you might
			// have a "Christmas" album but all its tracks are labelled as "Holiday". So
			// in the most general case, you have to call both this routine and
			// GetGenresFromAlbums. Uh, except that the AllAlbums class doesn't have the
			// GenreId field, just Genre. So if you want to call TracksByGenreId or
			// AlbumsByGenreId, you're out of luck if there are any Genres in Albums but
			// not in Tracks. Sigh. So depending on your need, you may need to call this
			// routine. Note however that, depending on how many Tracks you have in your
			// Zune Collection, this may take 10's of seconds to a few minutes to run.
			// TODO: Add routines to take this Dictionary, save it to XML, then reload
			//		 it.
			// TODO: Would a callback routine (to give visual feedback to the user) be
			//		 amiss here?
			var DictGenre = new Dictionary<string, int>();
			var Tracks = AllTracks.GetRecords(zl);
			foreach (var trk in Tracks) {
				if (!DictGenre.ContainsKey(trk.WMGenre))
					DictGenre.Add(trk.WMGenre, trk.WMGenreID);
			}
			return DictGenre;
		}

//---------------------------------------------------------------------------------------
		
		public static Dictionary<KeyValuePair<string /*Artist*/, string /*Album*/>, int /*ID*/> GetAllAlbumIds(ZuneLibrary zl) {
			// I wish Microsoft had called 'KeyValuePair' just 'Pair' (as in C++). The
			// following isn't a KeyValuePair, it's just a Pair. Oh well, VS2010 has
			// Tuples, which, when the time comes, will do fine.
			var IdDict = new Dictionary<KeyValuePair<string, string>, int>();
			var Albums = AllAlbums.GetRecords(zl);
			foreach (var album in Albums) {
				var kvp = new KeyValuePair<string, string>(album.WMAlbumArtist, album.WMAlbumTitle);
				if (! IdDict.ContainsKey(kvp)) {
					IdDict.Add(kvp, album.AlbumID);
				}
			}
			return IdDict;
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<LRSZune> GetRecordsForXml(SortOn so) {
			EQueryType  eqt;
			Type	    t; 
			GetClassInfo(out eqt, out t);
			using (ZuneQueryList zql = zl.QueryDatabase(eqt, 0,
				        EQuerySortType.eQuerySortOrderAscending, (uint)so, null)) {
				if (zql == null) {
					yield break;
				}
				object[] parms = { zl };
				for (uint i = 0; i < zql.Count; i++) {
					var o = Activator.CreateInstance(t, parms) as LRSZune;
					o.PopulateInstance(zql, i);
					yield return o;
				}
				yield break;
			}
		}

//---------------------------------------------------------------------------------------

		public abstract void PopulateInstance(ZuneQueryList zql, uint i);

//---------------------------------------------------------------------------------------

		protected abstract void GetClassInfo(out EQueryType eqt, out Type t);

//---------------------------------------------------------------------------------------

		public XDocument ZuneToXml(
						SortOn		so,
						string		TopElementName, 
						string		ElementName) {
			var ZuneItems = GetRecordsForXml(so);
			XDocument xd = new XDocument(
				new XElement(TopElementName,
					from item in ZuneItems
					select new XElement(ElementName, 
						from Item in item.GetType().GetFields()
							select new XElement("Field", 
								new XAttribute("Name", Item.Name),
								new XAttribute("Type", MapTypeToCSharp(Item.FieldType.ToString(), false)),
								Item.GetValue(item))
						)
				)
			);
			return xd;
		}

//---------------------------------------------------------------------------------------

		public static void DumpObj(TextWriter wtr, object o) {
			Type t = o.GetType();
			var flds = from field in t.GetFields()
							 select field;
			foreach (var f in flds) {
				wtr.WriteLine("{0} = {1}", f.Name, t.GetField(f.Name).GetValue(o));
			}
		}

//---------------------------------------------------------------------------------------

		public static void DumpObj(object o) {
		    DumpObj(Console.Out, o);
		}

//---------------------------------------------------------------------------------------

		public static string MapTypeToCSharp(string s) {
			return MapTypeToCSharp(s, true);
		}

//---------------------------------------------------------------------------------------

		public static string MapTypeToCSharp(string s, bool bUseTab) {
			switch (s) {
			case "System.Int32":
				// When we're pumping out XML, we don't necessarily want tab
				// characters (they'll get translated to ""int&#x9"").
				if (bUseTab)
					return "int\t";
				else
					return "int";
			case "System.String":
				return "string";
			case "System.DateTime":
				return "DateTime";
		    case "System.Int64":
		        return bUseTab ? "long\t" : "long";
		    case "System.Boolean":
		        return bUseTab ? "bool\t" : "bool";
			case "System.UInt64":
				return "long";
			case "System.Collections.ArrayList":
				return "ArrayList";
			default:
				return s;
			}
		}

//---------------------------------------------------------------------------------------

		public static string GetZuneDirectory() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Zune");
			string dir = (string)key.GetValue("Installation Directory");
			return dir;				// Will be null if Zune not installed
		}

//---------------------------------------------------------------------------------------

		public static ZuneLibrary InitializeZune(out int InitRetcode) {
			// As stated elsewhere, the Zune 3.0 software doesn't process the PATH
			// environment variable properly during ZuneLibrary initialization.
			// However, subsequent processing does seem to process PATH properly. So
			// we'll force the Zune directory as part of the current PATH, so any
			// necessary secondary DLLs will be found.
			string CurPath = Environment.GetEnvironmentVariable("PATH");
			string ZunePath = LRSZune.GetZuneDirectory();
			string NewPath = ZunePath + ";" + CurPath;
			Environment.SetEnvironmentVariable("PATH", NewPath);
			
			var zl = new ZuneLibrary();
			InitRetcode = zl.Initialize();
			return InitRetcode == 0 ? zl : null;
		}

//---------------------------------------------------------------------------------------

        // There is a bug in the Zune 3.0 software that prevents Zune initialization from
        // finding related dlls, unless they're in the same directory (some are in a
        // subdirectory called ""en-US"" (Sorry, no current support for other cultures.)).
        // If all the dlls are not present, 
        
        //      YOUR ZUNE LIBRARY WILL BE RESET TO EMPTY AND YOU MUST REBUILD IT!!!!!
        
        // So you must either copy your exe file to the Zune directory, or copy the
        // required files into your exe's directory. This routine returns true if all
        // files are present, and false if not. The BaseFiles list will return the names
        // of the files (if any) that need to be copied to your exe's directory, and
        // EnUsFiles contains the ones that must be copied to the en-US directory.
        // Note: Passing in ExeDir as null will dynamically pick up your exe's directory.
		
		public static bool AreAllNeededZune30FilesPresent(out List<string> BaseFiles, out List<string> EnUsFiles) {
			string []	ZuneBaseFiles = new string[] {
									"UIX.dll",
									"UIX.RenderApi.dll",
									"UIXControls.dll",
									"ZuneCfg.dll",
									"ZuneDB.dll",
									"ZuneDBApi.dll",
									"ZuneNativeLib.dll",
									"ZuneQP.dll",
									"ZuneResources.dll",
									"ZuneSE.dll",
									"ZuneShell.dll",
							};

			string []	Zune_en_USFiles = new string[] {
									"ZuneNss.exe.mui",
									"ZuneResources.dll.mui",
									"ZuneSetup.exe.mui",
							};

			string ExeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			BaseFiles = (from fname in ZuneBaseFiles
					 where !File.Exists(Path.Combine(ExeDir, fname))
					 select fname).ToList();
			string	SubDir = Path.Combine(ExeDir, "en-US");
			EnUsFiles = (from fname in Zune_en_USFiles
						 where !File.Exists(Path.Combine(SubDir, fname))
						 select fname).ToList();
			return (BaseFiles.Count + EnUsFiles.Count) == 0;
		}

//---------------------------------------------------------------------------------------
		
		public static bool AreAllNeededZune30FilesPresent() {
			// Simpler to call, but returns less information
			List<string> BaseFiles, EnUsFiles;
			return AreAllNeededZune30FilesPresent(out BaseFiles, out EnUsFiles);
		}
		
	}		// LRSZune class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AlbumsForAlbumArtistId : LRSZune {

//---------------------------------------------------------------------------------------

		public AlbumsForAlbumArtistId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AlbumsForAlbumArtistId

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllAlbums>(zl, EQueryType.eQueryTypeAlbumsForAlbumArtistId, "ArtistId", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
			t = typeof(AlbumsForAlbumArtistId);
		}
	}		// end of class AlbumsForAlbumArtistId


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class TracksForAlbumId : LRSZune {

//---------------------------------------------------------------------------------------

		public TracksForAlbumId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class TracksForAlbumId

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllTracks>(zl, EQueryType.eQueryTypeTracksForAlbumId, "AlbumId", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeTracksForAlbumId;
			t = typeof(TracksForAlbumId);
		}
	}		// end of class TracksForAlbumId


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class TracksForAlbumArtistId : LRSZune {

//---------------------------------------------------------------------------------------

		public TracksForAlbumArtistId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class TracksForAlbumArtistId

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllTracks>(zl, EQueryType.eQueryTypeTracksForAlbumArtistId, "ArtistId", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeTracksForAlbumArtistId;
			t = typeof(TracksForAlbumArtistId);
		}
	}		// end of class TracksForAlbumArtistId


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AlbumsWithKeyword : LRSZune {

//---------------------------------------------------------------------------------------

		public AlbumsWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AlbumsWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllAlbums>(zl, EQueryType.eQueryTypeAlbumsWithKeyword, "Keywords", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAlbumsWithKeyword;
			t = typeof(AlbumsWithKeyword);
		}
	}		// end of class AlbumsWithKeyword


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class TracksWithKeyword : LRSZune {

//---------------------------------------------------------------------------------------

		public TracksWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class TracksWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllTracks>(zl, EQueryType.eQueryTypeTracksWithKeyword, "Keywords", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeTracksWithKeyword;
			t = typeof(TracksWithKeyword);
		}
	}		// end of class TracksWithKeyword


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class ArtistsWithKeyword : LRSZune {

//---------------------------------------------------------------------------------------

		public ArtistsWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class ArtistsWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllAlbumArtists>(zl, EQueryType.eQueryTypeArtistsWithKeyword, "Keywords", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeArtistsWithKeyword;
			t = typeof(ArtistsWithKeyword);
		}
	}		// end of class ArtistsWithKeyword


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class PlaylistsWithKeyword : LRSZune {

//---------------------------------------------------------------------------------------

		public PlaylistsWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class PlaylistsWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllPlaylists>(zl, EQueryType.eQueryTypePlaylistsWithKeyword, "Keywords", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypePlaylistsWithKeyword;
			t = typeof(PlaylistsWithKeyword);
		}
	}		// end of class PlaylistsWithKeyword


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class VideoWithKeyword : LRSZune {

//---------------------------------------------------------------------------------------

		public VideoWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class VideoWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl, object ID) {
			return GetRecords(zl, ID, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl, object ID, SortOn so) {
			return GetRecords(zl, ID, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl, object ID, SortOn so, SortDirection sd) {
			return GetRecordsByProperty<AllVideos>(zl, EQueryType.eQueryTypeVideoWithKeyword, "Keywords", ID, so, sd);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeVideoWithKeyword;
			t = typeof(VideoWithKeyword);
		}
	}		// end of class VideoWithKeyword


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllTracks : LRSZune, Populateable {
		public int		AlbumArtistID;
		public int		AlbumID;
		public int		Bitrate;
		public int		CategoryID;
		public int		ComposerID;
		public int		ContributingArtistCount;
		public ArrayList	ContributingArtists;
		public ArrayList	ContributingComposers;
		public ArrayList	ContributingConductors;
		public string	Copyright;
		public int		CPAlbumID;
		public int		CPArtistID;
		public DateTime	DateAdded;
		public DateTime	DateModified;
		public int		DiscIndex;
		public string	DisplayArtist;
		public int		Duration;
		public long		FileCount;
		public string	FileName;
		public long		FileSize;
		public int		FileType;
		public string	FolderName;
		public bool		InLibrary;
		public bool		Is_Protected;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		MetadataState;
		public DateTime	ReleaseDate;
		public string	SourceURL;
		public string	Title;
		public int		UniqueID;
		public long		UserEditedFieldMask;
		public string	WMAlbumArtist;
		public string	WMAlbumTitle;
		public string	WMComposer;
		public string	WMGenre;
		public int		WMGenreID;
		public string	WMProvider;
		public int		WMTrackNumber;
		public string	WMUniqueFileIdentifier;
		public string	WMWMContentID;
		public DateTime	WMYear;
		public string	ZuneMediaID;

//---------------------------------------------------------------------------------------

		public AllTracks(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllTracks

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracks> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllTracks;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllTracks>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumArtistID);
					AlbumArtistID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumID);
					AlbumID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Bitrate);
					Bitrate = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID);
					CategoryID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ComposerID);
					ComposerID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ContributingArtistCount);
					ContributingArtistCount = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(ArrayList), (uint)SchemaMap.kiIndex_ContributingArtists);
					ContributingArtists = (ArrayList)(o ?? default(ArrayList));
				o =		ZQList.GetFieldValue(n, typeof(ArrayList), (uint)SchemaMap.kiIndex_ContributingComposers);
					ContributingComposers = (ArrayList)(o ?? default(ArrayList));
				o =		ZQList.GetFieldValue(n, typeof(ArrayList), (uint)SchemaMap.kiIndex_ContributingConductors);
					ContributingConductors = (ArrayList)(o ?? default(ArrayList));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Copyright);
					Copyright = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPAlbumID);
					CPAlbumID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPArtistID);
					CPArtistID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded);
					DateAdded = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateModified);
					DateModified = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DiscIndex);
					DiscIndex = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist);
					DisplayArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Duration);
					Duration = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_FileCount);
					FileCount = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName);
					FileName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_FileSize);
					FileSize = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType);
					FileType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName);
					FolderName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_InLibrary);
					InLibrary = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_Is_Protected);
					Is_Protected = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MetadataState);
					MetadataState = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_ReleaseDate);
					ReleaseDate = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_UserEditedFieldMask);
					UserEditedFieldMask = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
					WMAlbumArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
					WMAlbumTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMComposer);
					WMComposer = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenre);
					WMGenre = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMGenreID);
					WMGenreID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMProvider);
					WMProvider = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMTrackNumber);
					WMTrackNumber = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMUniqueFileIdentifier);
					WMUniqueFileIdentifier = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMWMContentID);
					WMWMContentID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_WMYear);
					WMYear = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllTracks;
			t = typeof(AllTracks);
		}
	}		// end of class AllTracks

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllTracksDetailed : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public AllTracksDetailed(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllTracksDetailed

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracksDetailed> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracksDetailed> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllTracksDetailed> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllTracksDetailed;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllTracksDetailed>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllTracksDetailed;
			t = typeof(AllTracksDetailed);
		}
	}		// end of class AllTracksDetailed

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllVideos : LRSZune, Populateable {
		public int		Bitrate;
		public int		CategoryID;
		public string	Copyright;
		public int		CPArtistID;
		public DateTime	DateAdded;
		public string	DateTaken;
		public string	Description;
		public string	DisplayArtist;
		public int		Duration;
		public string	EpisodeNumber;
		public string	FileName;
		public long		FileSize;
		public int		FileType;
		public string	FolderName;
		public bool		Is_Protected;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public string	Network;
		public int		PixelAspectRatioX;
		public int		PixelAspectRatioY;
		public string	ReleaseDate;
		public string	SeasonNumber;
		public string	SeriesTitle;
		public string	SourceURL;
		public string	Title;
		public int		UniqueID;
		public string	WMAlbumArtist;
		public string	WMAlbumTitle;
		public string	WMDirector;
		public string	WMGenre;
		public string	WMParentalRating;
		public string	WMPublisher;
		public string	WMSubTitle;
		public string	WMTrackNumber;
		public int		WMVideoHeight;
		public int		WMVideoWidth;

//---------------------------------------------------------------------------------------

		public AllVideos(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllVideos

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideos> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllVideos;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllVideos>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Bitrate);
					Bitrate = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID);
					CategoryID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Copyright);
					Copyright = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPArtistID);
					CPArtistID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded);
					DateAdded = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DateTaken);
					DateTaken = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Description);
					Description = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist);
					DisplayArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Duration);
					Duration = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_EpisodeNumber);
					EpisodeNumber = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName);
					FileName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_FileSize);
					FileSize = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType);
					FileType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName);
					FolderName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_Is_Protected);
					Is_Protected = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Network);
					Network = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PixelAspectRatioX);
					PixelAspectRatioX = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PixelAspectRatioY);
					PixelAspectRatioY = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ReleaseDate);
					ReleaseDate = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SeasonNumber);
					SeasonNumber = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SeriesTitle);
					SeriesTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
					WMAlbumArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
					WMAlbumTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMDirector);
					WMDirector = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenre);
					WMGenre = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMParentalRating);
					WMParentalRating = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMPublisher);
					WMPublisher = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMSubTitle);
					WMSubTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMTrackNumber);
					WMTrackNumber = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMVideoHeight);
					WMVideoHeight = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMVideoWidth);
					WMVideoWidth = (int)(o ?? default(int));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllVideos;
			t = typeof(AllVideos);
		}
	}		// end of class AllVideos

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllVideosDetailed : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public AllVideosDetailed(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllVideosDetailed

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideosDetailed> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideosDetailed> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllVideosDetailed> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllVideosDetailed;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllVideosDetailed>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllVideosDetailed;
			t = typeof(AllVideosDetailed);
		}
	}		// end of class AllVideosDetailed

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllPhotos : LRSZune, Populateable {
		public string	Author;
		public string	CameraManufacturer;
		public string	CameraModel;
		public int		CategoryID;
		public string	Comment;
		public string	Copyright;
		public DateTime	DateTaken;
		public string	FileName;
		public long		FileSize;
		public int		FileType;
		public string	FolderName;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		PhotoHeight;
		public int		PhotoWidth;
		public string	SourceURL;
		public string	Title;
		public int		UniqueID;

//---------------------------------------------------------------------------------------

		public AllPhotos(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllPhotos

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPhotos> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPhotos> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPhotos> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllPhotos;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllPhotos>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author);
					Author = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_CameraManufacturer);
					CameraManufacturer = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_CameraModel);
					CameraModel = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID);
					CategoryID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Comment);
					Comment = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Copyright);
					Copyright = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateTaken);
					DateTaken = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName);
					FileName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_FileSize);
					FileSize = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType);
					FileType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName);
					FolderName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PhotoHeight);
					PhotoHeight = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PhotoWidth);
					PhotoWidth = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllPhotos;
			t = typeof(AllPhotos);
		}
	}		// end of class AllPhotos

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllAlbums : LRSZune, Populateable {
		public int		AlbumID;
		public int		ContributingArtistCount;
		public int		CPArtistID;
		public DateTime	DateAdded;
		public int		DisplayArtistCount;
		public bool		HasAlbumArt;
		public bool		InLibrary;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		MetadataState;
		public string	SourceURL;
		public int		UniqueID;
		public long		UserEditedFieldMask;
		public string	WMAlbumArtist;
		public string	WMAlbumTitle;
		public string	WMGenre;
		public string	WMMCDI;
		public DateTime	WMYear;
		public string	TrackingID;
		public string	WMWMCollectionGroupID;
		public string	WMWMCollectionID;
		public string	ZuneMediaID;

//---------------------------------------------------------------------------------------

		public AllAlbums(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllAlbums

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbums> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllAlbums;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllAlbums>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumID);
					AlbumID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ContributingArtistCount);
					ContributingArtistCount = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPArtistID);
					CPArtistID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded);
					DateAdded = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DisplayArtistCount);
					DisplayArtistCount = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_HasAlbumArt);
					HasAlbumArt = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_InLibrary);
					InLibrary = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MetadataState);
					MetadataState = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_UserEditedFieldMask);
					UserEditedFieldMask = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
					WMAlbumArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
					WMAlbumTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenre);
					WMGenre = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMMCDI);
					WMMCDI = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_WMYear);
					WMYear = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_TrackingID);
					TrackingID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMWMCollectionGroupID);
					WMWMCollectionGroupID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMWMCollectionID);
					WMWMCollectionID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllAlbums;
			t = typeof(AllAlbums);
		}
	}		// end of class AllAlbums

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllAlbumArtists : LRSZune, Populateable {
		public int		AlbumIDAlbumArtist;
		public int		ArtistAlbumCount;
		public string	DisplayArtist;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		UniqueID;
		public string	WMAlbumArtist;
		public string	ZuneMediaID;

//---------------------------------------------------------------------------------------

		public AllAlbumArtists(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllAlbumArtists

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllAlbumArtists> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllAlbumArtists;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllAlbumArtists>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumIDAlbumArtist);
					AlbumIDAlbumArtist = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ArtistAlbumCount);
					ArtistAlbumCount = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist);
					DisplayArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
					WMAlbumArtist = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllAlbumArtists;
			t = typeof(AllAlbumArtists);
		}
	}		// end of class AllAlbumArtists

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllPodcastSeries : LRSZune, Populateable {
		public string	ArtUrl;
		public string	Author;
		public string	Copyright;
		public string	Description;
		public int		ErrorCode;
		public bool		Explicit;
		public string	FeedURL;
		public bool		HasUnplayedItems;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public string	OwnerName;
		public int		PlaybackOrder;
		public string	ProviderURL;
		public int		SeriesEpisodeCount;
		public int		SubscriptionState;
		public string	Title;
		public int		UniqueID;
		public string	ZuneAlbumCollectionId;
		public string	ZuneMediaID;

//---------------------------------------------------------------------------------------

		public AllPodcastSeries(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllPodcastSeries

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastSeries> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastSeries> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastSeries> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllPodcastSeries;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllPodcastSeries>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ArtUrl);
					ArtUrl = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author);
					Author = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Copyright);
					Copyright = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Description);
					Description = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ErrorCode);
					ErrorCode = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_Explicit);
					Explicit = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FeedURL);
					FeedURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_HasUnplayedItems);
					HasUnplayedItems = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_OwnerName);
					OwnerName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PlaybackOrder);
					PlaybackOrder = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ProviderURL);
					ProviderURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_SeriesEpisodeCount);
					SeriesEpisodeCount = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_SubscriptionState);
					SubscriptionState = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneAlbumCollectionId);
					ZuneAlbumCollectionId = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllPodcastSeries;
			t = typeof(AllPodcastSeries);
		}
	}		// end of class AllPodcastSeries

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllPodcastEpisodes : LRSZune, Populateable {
		public string	Author;
		public string	Copyright;
		public string	Description;
		public int		DownloadErrorCode;
		public int		DownloadState;
		public int		DownloadType;
		public int		Duration;
		public int		EpisodeMediaType;
		public bool		Explicit;
		public string	FeedURL;
		public bool		InLibrary;
		public string	LinkedFileURL;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		PlayedStatus;
		public string	ProviderURL;
		public DateTime	ReleaseDate;
		public int		SeriesID;
		public string	SeriesTitle;
		public string	Title;
		public int		UniqueID;
		public string	ZuneMediaID;
		public int		Bitrate;
		public string	FileName;
		public long		FileSize;
		public int		FileType;
		public string	FolderName;
		public string	SourceURL;

//---------------------------------------------------------------------------------------

		public AllPodcastEpisodes(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllPodcastEpisodes

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastEpisodes> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastEpisodes> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPodcastEpisodes> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllPodcastEpisodes;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllPodcastEpisodes>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author);
					Author = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Copyright);
					Copyright = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Description);
					Description = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DownloadErrorCode);
					DownloadErrorCode = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DownloadState);
					DownloadState = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DownloadType);
					DownloadType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Duration);
					Duration = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_EpisodeMediaType);
					EpisodeMediaType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_Explicit);
					Explicit = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FeedURL);
					FeedURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(bool), (uint)SchemaMap.kiIndex_InLibrary);
					InLibrary = (bool)(o ?? default(bool));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_LinkedFileURL);
					LinkedFileURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PlayedStatus);
					PlayedStatus = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ProviderURL);
					ProviderURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_ReleaseDate);
					ReleaseDate = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_SeriesID);
					SeriesID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SeriesTitle);
					SeriesTitle = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Bitrate);
					Bitrate = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName);
					FileName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_FileSize);
					FileSize = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType);
					FileType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName);
					FolderName = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllPodcastEpisodes;
			t = typeof(AllPodcastEpisodes);
		}
	}		// end of class AllPodcastEpisodes

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class EpisodesForSeriesId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public EpisodesForSeriesId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class EpisodesForSeriesId

//---------------------------------------------------------------------------------------

		public static IEnumerable<EpisodesForSeriesId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<EpisodesForSeriesId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<EpisodesForSeriesId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeEpisodesForSeriesId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<EpisodesForSeriesId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeEpisodesForSeriesId;
			t = typeof(EpisodesForSeriesId);
		}
	}		// end of class EpisodesForSeriesId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AlbumsByTOC : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public AlbumsByTOC(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AlbumsByTOC

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByTOC> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByTOC> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByTOC> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAlbumsByTOC;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AlbumsByTOC>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAlbumsByTOC;
			t = typeof(AlbumsByTOC);
		}
	}		// end of class AlbumsByTOC

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class PhotosWithKeyword : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public PhotosWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class PhotosWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosWithKeyword> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosWithKeyword> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosWithKeyword> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypePhotosWithKeyword;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<PhotosWithKeyword>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypePhotosWithKeyword;
			t = typeof(PhotosWithKeyword);
		}
	}		// end of class PhotosWithKeyword

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class SubscriptionsSeriesWithKeyword : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public SubscriptionsSeriesWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class SubscriptionsSeriesWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsSeriesWithKeyword> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsSeriesWithKeyword> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsSeriesWithKeyword> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<SubscriptionsSeriesWithKeyword>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
			t = typeof(SubscriptionsSeriesWithKeyword);
		}
	}		// end of class SubscriptionsSeriesWithKeyword

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class SubscriptionsEpisodesWithKeyword : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public SubscriptionsEpisodesWithKeyword(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class SubscriptionsEpisodesWithKeyword

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsEpisodesWithKeyword> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsEpisodesWithKeyword> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SubscriptionsEpisodesWithKeyword> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<SubscriptionsEpisodesWithKeyword>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
			t = typeof(SubscriptionsEpisodesWithKeyword);
		}
	}		// end of class SubscriptionsEpisodesWithKeyword

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class MediaFolders : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public MediaFolders(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class MediaFolders

//---------------------------------------------------------------------------------------

		public static IEnumerable<MediaFolders> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<MediaFolders> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<MediaFolders> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeMediaFolders;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<MediaFolders>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeMediaFolders;
			t = typeof(MediaFolders);
		}
	}		// end of class MediaFolders

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class PhotosByFolderId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public PhotosByFolderId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class PhotosByFolderId

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosByFolderId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosByFolderId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PhotosByFolderId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypePhotosByFolderId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<PhotosByFolderId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypePhotosByFolderId;
			t = typeof(PhotosByFolderId);
		}
	}		// end of class PhotosByFolderId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class VideosByFolderId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public VideosByFolderId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class VideosByFolderId

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByFolderId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByFolderId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByFolderId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeVideosByFolderId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<VideosByFolderId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeVideosByFolderId;
			t = typeof(VideosByFolderId);
		}
	}		// end of class VideosByFolderId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class VideosByCategoryId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public VideosByCategoryId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class VideosByCategoryId

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByCategoryId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByCategoryId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideosByCategoryId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeVideosByCategoryId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<VideosByCategoryId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeVideosByCategoryId;
			t = typeof(VideosByCategoryId);
		}
	}		// end of class VideosByCategoryId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class SyncProgress : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public SyncProgress(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class SyncProgress

//---------------------------------------------------------------------------------------

		public static IEnumerable<SyncProgress> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SyncProgress> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<SyncProgress> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeSyncProgress;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<SyncProgress>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeSyncProgress;
			t = typeof(SyncProgress);
		}
	}		// end of class SyncProgress

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllPlaylists : LRSZune, Populateable {
		public string	ArtUrl;
		public string	Author;
		public int		Count;
		public DateTime	DateModified;
		public string	Description;
		public string	MaxChannelSize;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		PlaylistType;
		public string	PublishInterval;
		public string	SourceURL;
		public int		Status;
		public string	Title;
		public long		TotalDuration;
		public int		UniqueID;
		public string	WMCategory;
		public string	WMGenreID;
		public string	ZuneMediaID;

//---------------------------------------------------------------------------------------

		public AllPlaylists(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllPlaylists

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllPlaylists> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllPlaylists;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllPlaylists>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			object	o;

				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ArtUrl);
					ArtUrl = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author);
					Author = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Count);
					Count = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateModified);
					DateModified = (DateTime)(o ?? default(DateTime));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Description);
					Description = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_MaxChannelSize);
					MaxChannelSize = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID);
					MediaID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType);
					MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PlaylistType);
					PlaylistType = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_PublishInterval);
					PublishInterval = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL);
					SourceURL = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Status);
					Status = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title);
					Title = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(long), (uint)SchemaMap.kiIndex_TotalDuration);
					TotalDuration = (long)(o ?? default(long));
				o =		ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID);
					UniqueID = (int)(o ?? default(int));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMCategory);
					WMCategory = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenreID);
					WMGenreID = (string)(o ?? default(string));
				o =		ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneMediaID);
					ZuneMediaID = (string)(o ?? default(string));
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllPlaylists;
			t = typeof(AllPlaylists);
		}
	}		// end of class AllPlaylists

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class PlaylistContentByPlaylistId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public PlaylistContentByPlaylistId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class PlaylistContentByPlaylistId

//---------------------------------------------------------------------------------------

		public static IEnumerable<PlaylistContentByPlaylistId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PlaylistContentByPlaylistId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PlaylistContentByPlaylistId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypePlaylistContentByPlaylistId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<PlaylistContentByPlaylistId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypePlaylistContentByPlaylistId;
			t = typeof(PlaylistContentByPlaylistId);
		}
	}		// end of class PlaylistContentByPlaylistId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AllGenres : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public AllGenres(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AllGenres

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllGenres> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllGenres> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AllGenres> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAllGenres;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AllGenres>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAllGenres;
			t = typeof(AllGenres);
		}
	}		// end of class AllGenres

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class TracksByGenreId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public TracksByGenreId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class TracksByGenreId

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksByGenreId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksByGenreId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksByGenreId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeTracksByGenreId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<TracksByGenreId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeTracksByGenreId;
			t = typeof(TracksByGenreId);
		}
	}		// end of class TracksByGenreId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class AlbumsByGenreId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public AlbumsByGenreId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class AlbumsByGenreId

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByGenreId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByGenreId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<AlbumsByGenreId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeAlbumsByGenreId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<AlbumsByGenreId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeAlbumsByGenreId;
			t = typeof(AlbumsByGenreId);
		}
	}		// end of class AlbumsByGenreId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class UserCards : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public UserCards(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class UserCards

//---------------------------------------------------------------------------------------

		public static IEnumerable<UserCards> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<UserCards> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<UserCards> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeUserCards;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<UserCards>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeUserCards;
			t = typeof(UserCards);
		}
	}		// end of class UserCards

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class TracksForTOC : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public TracksForTOC(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class TracksForTOC

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksForTOC> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksForTOC> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<TracksForTOC> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeTracksForTOC;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<TracksForTOC>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeTracksForTOC;
			t = typeof(TracksForTOC);
		}
	}		// end of class TracksForTOC

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class PersonsByTypeId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public PersonsByTypeId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class PersonsByTypeId

//---------------------------------------------------------------------------------------

		public static IEnumerable<PersonsByTypeId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PersonsByTypeId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PersonsByTypeId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypePersonsByTypeId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<PersonsByTypeId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypePersonsByTypeId;
			t = typeof(PersonsByTypeId);
		}
	}		// end of class PersonsByTypeId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class VideoSeriesTitles : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public VideoSeriesTitles(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class VideoSeriesTitles

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideoSeriesTitles> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideoSeriesTitles> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<VideoSeriesTitles> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeVideoSeriesTitles;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<VideoSeriesTitles>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeVideoSeriesTitles;
			t = typeof(VideoSeriesTitles);
		}
	}		// end of class VideoSeriesTitles

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class FilesByTrackId : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public FilesByTrackId(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class FilesByTrackId

//---------------------------------------------------------------------------------------

		public static IEnumerable<FilesByTrackId> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<FilesByTrackId> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<FilesByTrackId> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeFilesByTrackId;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<FilesByTrackId>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeFilesByTrackId;
			t = typeof(FilesByTrackId);
		}
	}		// end of class FilesByTrackId

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public partial class Invalid : LRSZune, Populateable {

//---------------------------------------------------------------------------------------

		public Invalid(ZuneLibrary zl) {
			base.zl = zl;
		}      // ctor for class Invalid

//---------------------------------------------------------------------------------------

		public static IEnumerable<Invalid> GetRecords(ZuneLibrary zl) {
			return GetRecords(zl, SortOn.Abstract, SortDirection.None);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<Invalid> GetRecords(ZuneLibrary zl, SortOn so) {
			return GetRecords(zl, so, SortDirection.Ascending);
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<Invalid> GetRecords(ZuneLibrary zl, SortOn so, SortDirection sd) {
			EQueryType  eqt = EQueryType.eQueryTypeInvalid;
			ZuneQueryList zql = zl.QueryDatabase(eqt, 0, (EQuerySortType)sd, (uint)so, null);
			return GetRecords<Invalid>(zl, zql);
		}

//---------------------------------------------------------------------------------------

		public override void PopulateInstance(ZuneQueryList ZQList, uint n) {
			// There are no fields defined in this class.
			// See the comments at the top of this file.
		}

//---------------------------------------------------------------------------------------

		protected override void GetClassInfo(out EQueryType eqt, out Type t) {
			eqt = EQueryType.eQueryTypeInvalid;
			t = typeof(Invalid);
		}
	}		// end of class Invalid

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public enum SortDirection {
			None,
			Ascending,
			Descending,
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public enum SortOn : uint {
			Abstract,
			AcquiredContentRetrievalTransactionID,
			AcquisitionTime,
			AcquisitionTimeDay,
			AcquisitionTimeMonth,
			AcquisitionTimeYear,
			AcquisitionTimeYearMonth,
			AcquisitionTimeYearMonthDay,
			AlbumArtistID,
			AlbumID,
			AlbumIDAlbumArtist,
			AlbumPUOID,
			ArtDownloadDate,
			ArtInFile,
			ArtistAlbumCount,
			ArtUrl,
			AudioBitrate,
			AudiobookContentSize,
			AudiobookCount,
			AudioFormat,
			AudioTargetBitRate,
			AudioThresholdBitRate,
			Author,
			AutoSyncDefaultRules,
			AvailableTime,
			AverageLevel,
			AverageRating,
			Baseline,
			Bitrate,
			Blank,
			Bookmark,
			Burn,
			BurnListSize,
			BuyNow,
			BuyTickets,
			CallLetters,
			CameraManufacturer,
			CameraModel,
			CanBeExcluded,
			CanBurn,
			CanonicalFileType,
			CanonicalName,
			CanSync,
			CategoryID,
			CDTrackEnabled,
			Channels,
			CheckDeviceMediaSize,
			Checked,
			ChildList,
			Comment,
			ComposerID,
			ConductorID,
			Connected,
			ContentDistributorDuration,
			ContentDistributorType,
			ContentRatingID,
			ContentRevisionNumber,
			ContributingArtistCount,
			ContributingArtists,
			ContributingComposers,
			ContributingConductors,
			Copyright,
			Count,
			CPActorID,
			CPAlbumID,
			CPAlbumPopularity,
			CPAlbumPrice,
			CPAlbumPriceID,
			CPAlsoAppearsOnArtistID,
			CPArtistGenre,
			CPArtistGenreID,
			CPArtistID,
			CPArtistPopularity,
			CPCanDownloadTrack,
			CPCanPreviewTrack,
			CPCanStreamTrack,
			CPDirectorID,
			CPFeatured,
			CPGenreID,
			CPGenrePopularity,
			CPGenreSubgenre,
			CPGenreSubGenreID,
			CPHasVideo,
			CPMusicVideoCanDownload,
			CPMusicVideoCanStream,
			CPMusicVideoPrice,
			CPMusicVideoPriceID,
			CPPopularity,
			CPRadioGenre,
			CPRadioID,
			CPRadioIsSubscriptionOnly,
			CPRadioMood,
			CPRadioProgrammer,
			CPRadioSubtitle,
			CPRecentlyAdded,
			CPSubgenre,
			CPSubGenreID,
			CPSubGenrePopularity,
			CPTrackID,
			CPTrackPrice,
			CPTrackPriceID,
			CPTVEpisodeID,
			CPTVSeasonID,
			CPTVSeriesID,
			CPVideoID,
			CurrentRipTrackIndex,
			DateAdded,
			DateAlbumAdded,
			DateCreated,
			DateModified,
			DateTaken,
			DaysInCollection,
			Description,
			DesiresAudio,
			DesiresPhoto,
			DesiresVideo,
			DeviceCapabilities,
			DeviceFileSize,
			DeviceGenerationNumber,
			DeviceID,
			DeviceMediaType,
			DevicePromptGuest,
			DeviceSyncSupportFlags,
			Disc,
			DiscIndex,
			DiscsCount,
			DiscSpaceToUse,
			DisplayArtist,
			DisplayArtistAlbumID,
			DisplayArtistCount,
			DontCopyToDevice,
			DontSyncHatedContent,
			DownloadErrorCode,
			DownloadState,
			DownloadType,
			DRMIndividualizedVersion,
			DRMKeyID,
			Duration,
			DVDDiscIndex,
			DVDGroupTitle,
			DVDID,
			EpisodeMediaType,
			EpisodeNumber,
			Erase,
			ErrorCode,
			ErrorCondition,
			ErrorURL,
			ExistingVersion,
			Explicit,
			FeedURL,
			FileCount,
			FileName,
			FileSize,
			FileType,
			FirmwareVersion,
			FolderMediaType,
			FolderName,
			FormatTag,
			FourCC,
			FreeSpace,
			FreeSpaceLastSync,
			Frequency,
			FriendlyName,
			FriendlyNameGenerated,
			GuestPrompt,
			HardwareID,
			HasAlbumArt,
			HasUnplayedItems,
			HMEAlbumTitle,
			HMEAllArtists,
			HMEAuthor,
			HMEComposer,
			HMEDisplayArtist,
			HMEIs_Protected,
			HMEShareable,
			HMEWMGenre,
			Icon,
			InLibrary,
			Is_Protected,
			IsNetworkFeed,
			IsVBR,
			LastConnectTime,
			LastFirmwareUpdateCheck,
			LastServerRequest,
			LastSyncErrorCount,
			LastSyncKey,
			LastSyncNoFitCount,
			LastSyncTime,
			LinkedFileURL,
			LocalURL,
			Location,
			Manufacturer,
			MappedErrorCode,
			MarkedForDeletion,
			MaxChannelSize,
			MaxEpisodes,
			MediaAttribute,
			MediaContentTypes,
			MediaID,
			MediaType,
			MetadataState,
			ModelID,
			ModifiedBy,
			MoreInfo,
			NeedsResync,
			Network,
			NewVersion,
			ObjectID,
			OnDevice,
			OriginalIndexLeft,
			OwnerName,
			ParentID,
			PeakValue,
			PendingActions,
			PerceivedType,
			PercentSpaceReserved,
			PhotoHeight,
			PhotoWidth,
			PixelAspectRatioX,
			PixelAspectRatioY,
			PlaybackOrder,
			PlayedStatus,
			PlaylistID,
			PlaylistIndex,
			PlaylistType,
			PreferredAudioBitrate,
			PreferredVideoBitrate,
			PrimaryAuthor,
			PrimaryComposer,
			Profile,
			Progress,
			Protocol,
			ProviderURL,
			PublishInterval,
			RadioBand,
			RadioFormat,
			RatingOrg,
			RecordingTime,
			RecordingTimeDay,
			RecordingTimeMonth,
			RecordingTimeYear,
			RecordingTimeYearMonth,
			RecordingTimeYearMonthDay,
			RelationshipID,
			ReleaseDate,
			ReleaseDateDay,
			ReleaseDateMonth,
			ReleaseDateYear,
			ReleaseDateYearMonth,
			ReleaseDateYearMonthDay,
			Relevance,
			RequestState,
			ResyncAudio,
			ResyncVideo,
			Ripped,
			RipProgress,
			Rule,
			RuleGroupHeader,
			SeasonNumber,
			SelectedForRip,
			SerialNumber,
			SeriesEpisodeCount,
			SeriesID,
			SeriesTitle,
			ServiceUniqueName,
			ShadowFileSourceDRMType,
			ShadowFileSourceFileType,
			SourceURL,
			SourceURLHash,
			Status,
			Streams,
			Subject,
			SubscriptionState,
			SubType,
			SummaryData,
			SupportsAudio,
			SupportsPhoto,
			SupportsVideo,
			SyncIndex,
			SyncInfo,
			SyncItemCount,
			SyncListIndex,
			SyncOnConnect,
			SyncOnly,
			SyncOverallPercent,
			SyncPercentComplete,
			SyncRelationship,
			SyncRuleSyncAllChannels,
			SyncRuleSyncAllMusic,
			SyncRuleSyncAllPhotos,
			SyncRuleSyncAllPodcasts,
			SyncRuleSyncAllUserCardFriends,
			SyncRuleSyncAllVideos,
			SyncShuffle,
			SyncState,
			SyncToRoot,
			SyncTriggerEventIndex,
			Temporary,
			Title,
			TotalCount,
			TotalDuration,
			TotalSpace,
			TrackingID,
			TranscodedFileName,
			DeviceTranscodeOptimization,
			TranscodeTriggerEventIndex,
			TranscodingEnabled,
			UniqueID,
			UpsellPrompt,
			UserCardRefID,
			UserDataDateModified,
			UserEditedFieldMask,
			UserID,
			UserLastPlayedDate,
			UserLastRatedDate,
			UserLastSkippedDate,
			UserPlayCount,
			UserPlayCountClient,
			UserPlayCountClientReported,
			UserPlayCountDevice,
			UserPlayCountDeviceReported,
			UserPlayCountReported,
			UserRating,
			UserRatingReported,
			UserSkipCount,
			UserSkipCountReported,
			Version,
			VideoBitrate,
			VideoFormat,
			WasTranscoded,
			WlanDeviceUuid,
			WMAlbumArtist,
			WMAlbumCoverURL,
			WMAlbumTitle,
			WMAudioFileURL,
			WMAudioSourceURL,
			WMAuthorURL,
			WMBeatsPerMinute,
			WMCategory,
			WMCodec,
			WMComposer,
			WMConductor,
			WMContentDistributor,
			WMContentGroupDescription,
			WMDirector,
			WMDRM,
			WMEncodedBy,
			WMEncodingSettings,
			WMEncodingTime,
			WMGenre,
			WMGenreID,
			WMInitialKey,
			WMISRC,
			WMLanguage,
			WMLyrics,
			WMLyrics_Synchronised,
			WMMCDI,
			WMMediaClassPrimaryID,
			WMMediaClassSecondaryID,
			WMMediaOriginalBroadcastDateTime,
			WMMediaOriginalChannel,
			WMMediaStationName,
			WMMood,
			WMOriginalAlbumTitle,
			WMOriginalArtist,
			WMOriginalFilename,
			WMOriginalLyricist,
			WMOriginalReleaseYear,
			WMParentalRating,
			WMPartOfSet,
			WMPeriod,
			WMPicture,
			WMPlaylistDelay,
			WMProducer,
			WMPromotionURL,
			WMProtectionType,
			WMProvider,
			WMProviderRating,
			WMProviderStyle,
			WMPublisher,
			WMRadioStationName,
			WMRadioStationOwner,
			WMSubscriptionContentID,
			WMSubTitle,
			WMSubTitleDescription,
			WMText,
			WMToolName,
			WMToolVersion,
			WMTrackNumber,
			WMUniqueFileIdentifier,
			WMUserWebURL,
			WMVideoFrameRate,
			WMVideoHeight,
			WMVideoWidth,
			WMWMCollectionGroupID,
			WMWMCollectionID,
			WMWMContentID,
			WMWriter,
			WMYear,
			Write,
			ZuneAlbumCollectionId,
			ZuneMediaID,
			SchemaMapSize,
	}
}


/*

This is a simplified version of the above classes. It shows the classes and their fields.

Query Type eQueryTypeAllTracks / 0, (43 fields)
		Schema kiIndex_AlbumArtistID / 8 	-> int
		Schema kiIndex_AlbumID / 9 	-> int
		Schema kiIndex_Bitrate / 28 	-> int
		Schema kiIndex_CategoryID / 43 	-> int
		Schema kiIndex_ComposerID / 50 	-> int
		Schema kiIndex_ContributingArtistCount / 57 	-> int
		Schema kiIndex_ContributingArtists / 58 	-> ArrayList
		Schema kiIndex_ContributingComposers / 59 	-> ArrayList
		Schema kiIndex_ContributingConductors / 60 	-> ArrayList
		Schema kiIndex_Copyright / 61 	-> string
		Schema kiIndex_CPAlbumID / 64 	-> int
		Schema kiIndex_CPArtistID / 71 	-> int
		Schema kiIndex_DateAdded / 106 	-> DateTime
		Schema kiIndex_DateModified / 109 	-> DateTime
		Schema kiIndex_DiscIndex / 124 	-> int
		Schema kiIndex_DisplayArtist / 127 	-> string
		Schema kiIndex_Duration / 137 	-> int
		Schema kiIndex_FileCount / 150 	-> long
		Schema kiIndex_FileName / 151 	-> string
		Schema kiIndex_FileSize / 152 	-> long
		Schema kiIndex_FileType / 153 	-> int
		Schema kiIndex_FolderName / 156 	-> string
		Schema kiIndex_InLibrary / 177 	-> bool
		Schema kiIndex_Is_Protected / 178 	-> bool
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_MetadataState / 200 	-> int
		Schema kiIndex_ReleaseDate / 244 	-> DateTime
		Schema kiIndex_SourceURL / 267 	-> string
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_UserEditedFieldMask / 311 	-> long
		Schema kiIndex_WMAlbumArtist / 331 	-> string
		Schema kiIndex_WMAlbumTitle / 333 	-> string
		Schema kiIndex_WMComposer / 340 	-> string
		Schema kiIndex_WMGenre / 349 	-> string
		Schema kiIndex_WMGenreID / 350 	-> int
		Schema kiIndex_WMProvider / 376 	-> string
		Schema kiIndex_WMTrackNumber / 388 	-> int
		Schema kiIndex_WMUniqueFileIdentifier / 389 	-> string
		Schema kiIndex_WMWMContentID / 396 	-> string
		Schema kiIndex_WMYear / 398 	-> DateTime
		Schema kiIndex_ZuneMediaID / 401 	-> string

Query Type eQueryTypeAllTracksDetailed / 1, (0 fields)

Query Type eQueryTypeAllVideos / 2, (36 fields)
		Schema kiIndex_Bitrate / 28 	-> int
		Schema kiIndex_CategoryID / 43 	-> int
		Schema kiIndex_Copyright / 61 	-> string
		Schema kiIndex_CPArtistID / 71 	-> int
		Schema kiIndex_DateAdded / 106 	-> DateTime
		Schema kiIndex_DateTaken / 110 	-> string
		Schema kiIndex_Description / 112 	-> string
		Schema kiIndex_DisplayArtist / 127 	-> string
		Schema kiIndex_Duration / 137 	-> int
		Schema kiIndex_EpisodeNumber / 142 	-> string
		Schema kiIndex_FileName / 151 	-> string
		Schema kiIndex_FileSize / 152 	-> long
		Schema kiIndex_FileType / 153 	-> int
		Schema kiIndex_FolderName / 156 	-> string
		Schema kiIndex_Is_Protected / 178 	-> bool
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_Network / 205 	-> string
		Schema kiIndex_PixelAspectRatioX / 218 	-> int
		Schema kiIndex_PixelAspectRatioY / 219 	-> int
		Schema kiIndex_ReleaseDate / 244 	-> string
		Schema kiIndex_SeasonNumber / 258 	-> string
		Schema kiIndex_SeriesTitle / 263 	-> string
		Schema kiIndex_SourceURL / 267 	-> string
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_WMAlbumArtist / 331 	-> string
		Schema kiIndex_WMAlbumTitle / 333 	-> string
		Schema kiIndex_WMDirector / 344 	-> string
		Schema kiIndex_WMGenre / 349 	-> string
		Schema kiIndex_WMParentalRating / 368 	-> string
		Schema kiIndex_WMPublisher / 379 	-> string
		Schema kiIndex_WMSubTitle / 383 	-> string
		Schema kiIndex_WMTrackNumber / 388 	-> string
		Schema kiIndex_WMVideoHeight / 392 	-> int
		Schema kiIndex_WMVideoWidth / 393 	-> int

Query Type eQueryTypeAllVideosDetailed / 3, (0 fields)

Query Type eQueryTypeAllPhotos / 4, (18 fields)
		Schema kiIndex_Author / 22 	-> string
		Schema kiIndex_CameraManufacturer / 36 	-> string
		Schema kiIndex_CameraModel / 37 	-> string
		Schema kiIndex_CategoryID / 43 	-> int
		Schema kiIndex_Comment / 49 	-> string
		Schema kiIndex_Copyright / 61 	-> string
		Schema kiIndex_DateTaken / 110 	-> DateTime
		Schema kiIndex_FileName / 151 	-> string
		Schema kiIndex_FileSize / 152 	-> long
		Schema kiIndex_FileType / 153 	-> int
		Schema kiIndex_FolderName / 156 	-> string
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_PhotoHeight / 216 	-> int
		Schema kiIndex_PhotoWidth / 217 	-> int
		Schema kiIndex_SourceURL / 267 	-> string
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_UniqueID / 307 	-> int

Query Type eQueryTypeAllAlbums / 5, (22 fields)
		Schema kiIndex_AlbumID / 9 	-> int
		Schema kiIndex_ContributingArtistCount / 57 	-> int
		Schema kiIndex_CPArtistID / 71 	-> int
		Schema kiIndex_DateAdded / 106 	-> DateTime
		Schema kiIndex_DisplayArtistCount / 129 	-> int
		Schema kiIndex_HasAlbumArt / 166 	-> bool
		Schema kiIndex_InLibrary / 177 	-> bool
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_MetadataState / 200 	-> int
		Schema kiIndex_SourceURL / 267 	-> string
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_UserEditedFieldMask / 311 	-> long
		Schema kiIndex_WMAlbumArtist / 331 	-> string
		Schema kiIndex_WMAlbumTitle / 333 	-> string
		Schema kiIndex_WMGenre / 349 	-> string
		Schema kiIndex_WMMCDI / 356 	-> string
		Schema kiIndex_WMYear / 398 	-> DateTime
		Schema kiIndex_TrackingID / 302 	-> string
		Schema kiIndex_WMWMCollectionGroupID / 394 	-> string
		Schema kiIndex_WMWMCollectionID / 395 	-> string
		Schema kiIndex_ZuneMediaID / 401 	-> string

Query Type eQueryTypeAllAlbumArtists / 6, (8 fields)
		Schema kiIndex_AlbumIDAlbumArtist / 10 	-> int
		Schema kiIndex_ArtistAlbumCount / 14 	-> int
		Schema kiIndex_DisplayArtist / 127 	-> string
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_WMAlbumArtist / 331 	-> string
		Schema kiIndex_ZuneMediaID / 401 	-> string

Query Type eQueryTypeAllPodcastSeries / 7, (19 fields)
		Schema kiIndex_ArtUrl / 15 	-> string
		Schema kiIndex_Author / 22 	-> string
		Schema kiIndex_Copyright / 61 	-> string
		Schema kiIndex_Description / 112 	-> string
		Schema kiIndex_ErrorCode / 144 	-> int
		Schema kiIndex_Explicit / 148 	-> bool
		Schema kiIndex_FeedURL / 149 	-> string
		Schema kiIndex_HasUnplayedItems / 167 	-> bool
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_OwnerName / 210 	-> string
		Schema kiIndex_PlaybackOrder / 220 	-> int
		Schema kiIndex_ProviderURL / 232 	-> string
		Schema kiIndex_SeriesEpisodeCount / 261 	-> int
		Schema kiIndex_SubscriptionState / 272 	-> int
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_ZuneAlbumCollectionId / 400 	-> string
		Schema kiIndex_ZuneMediaID / 401 	-> string

Query Type eQueryTypeAllPodcastEpisodes / 8, (28 fields)
		Schema kiIndex_Author / 22 	-> string
		Schema kiIndex_Copyright / 61 	-> string
		Schema kiIndex_Description / 112 	-> string
		Schema kiIndex_DownloadErrorCode / 132 	-> int
		Schema kiIndex_DownloadState / 133 	-> int
		Schema kiIndex_DownloadType / 134 	-> int
		Schema kiIndex_Duration / 137 	-> int
		Schema kiIndex_EpisodeMediaType / 141 	-> int
		Schema kiIndex_Explicit / 148 	-> bool
		Schema kiIndex_FeedURL / 149 	-> string
		Schema kiIndex_InLibrary / 177 	-> bool
		Schema kiIndex_LinkedFileURL / 188 	-> string
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_PlayedStatus / 221 	-> int
		Schema kiIndex_ProviderURL / 232 	-> string
		Schema kiIndex_ReleaseDate / 244 	-> DateTime
		Schema kiIndex_SeriesID / 262 	-> int
		Schema kiIndex_SeriesTitle / 263 	-> string
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_ZuneMediaID / 401 	-> string
		Schema kiIndex_Bitrate / 28 	-> int
		Schema kiIndex_FileName / 151 	-> string
		Schema kiIndex_FileSize / 152 	-> long
		Schema kiIndex_FileType / 153 	-> int
		Schema kiIndex_FolderName / 156 	-> string
		Schema kiIndex_SourceURL / 267 	-> string

Query Type eQueryTypeEpisodesForSeriesId / 12, (0 fields)

Query Type eQueryTypeAlbumsByTOC / 13, (0 fields)

Query Type eQueryTypePhotosWithKeyword / 18, (0 fields)

Query Type eQueryTypeSubscriptionsSeriesWithKeyword / 19, (0 fields)

Query Type eQueryTypeSubscriptionsEpisodesWithKeyword / 20, (0 fields)

Query Type eQueryTypeMediaFolders / 22, (0 fields)

Query Type eQueryTypePhotosByFolderId / 23, (0 fields)

Query Type eQueryTypeVideosByFolderId / 24, (0 fields)

Query Type eQueryTypeVideosByCategoryId / 25, (0 fields)

Query Type eQueryTypeSyncProgress / 26, (0 fields)

Query Type eQueryTypeAllPlaylists / 27, (18 fields)
		Schema kiIndex_ArtUrl / 15 	-> string
		Schema kiIndex_Author / 22 	-> string
		Schema kiIndex_Count / 62 	-> int
		Schema kiIndex_DateModified / 109 	-> DateTime
		Schema kiIndex_Description / 112 	-> string
		Schema kiIndex_MaxChannelSize / 194 	-> string
		Schema kiIndex_MediaID / 198 	-> int
		Schema kiIndex_MediaType / 199 	-> EMediaTypes
		Schema kiIndex_PlaylistType / 224 	-> int
		Schema kiIndex_PublishInterval / 233 	-> string
		Schema kiIndex_SourceURL / 267 	-> string
		Schema kiIndex_Status / 269 	-> int
		Schema kiIndex_Title / 298 	-> string
		Schema kiIndex_TotalDuration / 300 	-> long
		Schema kiIndex_UniqueID / 307 	-> int
		Schema kiIndex_WMCategory / 338 	-> string
		Schema kiIndex_WMGenreID / 350 	-> string
		Schema kiIndex_ZuneMediaID / 401 	-> string

Query Type eQueryTypePlaylistContentByPlaylistId / 28, (0 fields)

Query Type eQueryTypeAllGenres / 29, (0 fields)

Query Type eQueryTypeTracksByGenreId / 30, (0 fields)

Query Type eQueryTypeAlbumsByGenreId / 31, (0 fields)

Query Type eQueryTypeUserCards / 32, (0 fields)

Query Type eQueryTypeTracksForTOC / 33, (0 fields)

Query Type eQueryTypePersonsByTypeId / 34, (0 fields)

Query Type eQueryTypeVideoSeriesTitles / 35, (0 fields)

Query Type eQueryTypeFilesByTrackId / 36, (0 fields)

Query Type eQueryTypeInvalid / -1, (0 fields)


*/

