// Copyright (c) 2008 by Larry Smith

using System;
using System.Collections;		// For ArrayList
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using MicrosoftZuneLibrary;

namespace LRS.Zune.Classes {
	public static class ZuneTypeFactory {

		const string EnumValName = "EnumVal";

//---------------------------------------------------------------------------------------
		
		public static object Create<T>(ZuneQueryList zql, uint i) where T : class  {
			Type t = typeof(T);
			if (t == typeof(eQueryTypeAllTracks)) return new eQueryTypeAllTracks(zql, i);
			if (t == typeof(eQueryTypeAllTracksDetailed)) return new eQueryTypeAllTracksDetailed(zql, i);
			if (t == typeof(eQueryTypeAllVideos)) return new eQueryTypeAllVideos(zql, i);
			if (t == typeof(eQueryTypeAllVideosDetailed)) return new eQueryTypeAllVideosDetailed(zql, i);
			if (t == typeof(eQueryTypeAllPhotos)) return new eQueryTypeAllPhotos(zql, i);
			if (t == typeof(eQueryTypeAllAlbums)) return new eQueryTypeAllAlbums(zql, i);
			if (t == typeof(eQueryTypeAllAlbumArtists)) return new eQueryTypeAllAlbumArtists(zql, i);
			if (t == typeof(eQueryTypeAllPodcastSeries)) return new eQueryTypeAllPodcastSeries(zql, i);
			if (t == typeof(eQueryTypeAllPodcastEpisodes)) return new eQueryTypeAllPodcastEpisodes(zql, i);
			if (t == typeof(eQueryTypeAlbumsForAlbumArtistId)) return new eQueryTypeAlbumsForAlbumArtistId(zql, i);
			if (t == typeof(eQueryTypeAlbumsForContributingArtistId)) return new eQueryTypeAlbumsForContributingArtistId(zql, i);
			if (t == typeof(eQueryTypeTracksForAlbumId)) return new eQueryTypeTracksForAlbumId(zql, i);
			if (t == typeof(eQueryTypeTracksForAlbumArtistId)) return new eQueryTypeTracksForAlbumArtistId(zql, i);
			if (t == typeof(eQueryTypeEpisodesForSeriesId)) return new eQueryTypeEpisodesForSeriesId(zql, i);
			if (t == typeof(eQueryTypeAlbumsByTOC)) return new eQueryTypeAlbumsByTOC(zql, i);
			if (t == typeof(eQueryTypeAlbumsWithKeyword)) return new eQueryTypeAlbumsWithKeyword(zql, i);
			if (t == typeof(eQueryTypeTracksWithKeyword)) return new eQueryTypeTracksWithKeyword(zql, i);
			if (t == typeof(eQueryTypeArtistsWithKeyword)) return new eQueryTypeArtistsWithKeyword(zql, i);
			if (t == typeof(eQueryTypePhotosWithKeyword)) return new eQueryTypePhotosWithKeyword(zql, i);
			if (t == typeof(eQueryTypeSubscriptionsSeriesWithKeyword)) return new eQueryTypeSubscriptionsSeriesWithKeyword(zql, i);
			if (t == typeof(eQueryTypeSubscriptionsEpisodesWithKeyword)) return new eQueryTypeSubscriptionsEpisodesWithKeyword(zql, i);
			if (t == typeof(eQueryTypeVideoWithKeyword)) return new eQueryTypeVideoWithKeyword(zql, i);
			if (t == typeof(eQueryTypeMediaFolders)) return new eQueryTypeMediaFolders(zql, i);
			if (t == typeof(eQueryTypePhotosByFolderId)) return new eQueryTypePhotosByFolderId(zql, i);
			if (t == typeof(eQueryTypeVideosByFolderId)) return new eQueryTypeVideosByFolderId(zql, i);
			if (t == typeof(eQueryTypeSyncProgress)) return new eQueryTypeSyncProgress(zql, i);
			if (t == typeof(eQueryTypeAllPlaylists)) return new eQueryTypeAllPlaylists(zql, i);
			if (t == typeof(eQueryTypePlaylistContentByPlaylistId)) return new eQueryTypePlaylistContentByPlaylistId(zql, i);
			if (t == typeof(eQueryTypeAllGenres)) return new eQueryTypeAllGenres(zql, i);
			if (t == typeof(eQueryTypeTracksForGenreId)) return new eQueryTypeTracksForGenreId(zql, i);
			if (t == typeof(eQueryTypeInvalid)) return new eQueryTypeInvalid(zql, i);

			return null;
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<T> GetZuneItems<T>(ZuneLibrary zl) where T : class {
			Type	t = typeof(T);
			EQueryType type = (EQueryType)t.GetField(EnumValName).GetValue(t);
			using (ZuneQueryList zql = zl.QueryDatabase(type, 0,
				EQuerySortType.eQuerySortOrderAscending, 0, null)) {
				if (zql == null) {
					yield break;
				}
				for (uint i = 0; i < zql.Count; i++) {
					yield return (T)Create<T>(zql, i);
				}
				yield break;
			}
		}

//---------------------------------------------------------------------------------------

		public static XDocument ZuneToXml<QueryType>(
				ZuneLibrary zl, 
				string		TopElementName, 
				string		ElementName) where QueryType : class {
			var ZuneItems = ZuneTypeFactory.GetZuneItems<QueryType>(zl);
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

		public static string MapTypeToCSharp(string s) {
			return MapTypeToCSharp(s, true);
		}

//---------------------------------------------------------------------------------------

		public static string MapTypeToCSharp(string s, bool bUseTab) {
			switch (s) {
			case "System.Int32":
				// When we're pumping out XML, we don't necessarily want tab
				// characters (they'll get translated to "int&#x9").
				if (bUseTab)
					return "int\t";
				else
					return "int";
			case "System.String":
				return "string";
			case "System.DateTime":
				return "DateTime";
			case "System.UInt64":
				return "ulong";
			case "System.Collections.ArrayList":
				return "ArrayList";
			default:
				return s;
			}
		}

//---------------------------------------------------------------------------------------

		public static void DumpObj(TextWriter wtr, object o) {
			Type t = o.GetType();
			var flds = from field in t.GetFields()
							 select field;
			string sep = "";
			foreach (var f in flds) {
				wtr.WriteLine("{0}{1} = {2}", sep, f.Name, t.GetField(f.Name).GetValue(o));
				sep = "\t";
			}
		}

//---------------------------------------------------------------------------------------

		public static void DumpZuneItemsViaClass<QueryType>(
						ZuneLibrary zl,
						string Filename) where QueryType : class {
			IEnumerable<QueryType> Items = ZuneTypeFactory.GetZuneItems<QueryType>(zl);
			StreamWriter wtr = new StreamWriter(Filename);
			foreach (var item in Items) {
				wtr.WriteLine();
				DumpObj(wtr, item);
			}
			wtr.Close();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllTracks {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllTracks;

		public int		AlbumArtistID;
		public int		AlbumID;
		public int		Bitrate;
		public int		CategoryID;
		public int		ComposerID;
		public int		ContributingArtistCount;
		public ArrayList	ContributingArtists;
		public int		CPAlbumID;
		public int		CPArtistID;
		public DateTime	DateAdded;
		public DateTime	DateModified;
		public string	DisplayArtist;
		public int		Duration;
		public string	FileName;
		public int		FileSize;
		public int		FileType;
		public string	FolderName;
		public int		InLibrary;
		public int		Is_Protected;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public DateTime	ReleaseDate;
		public string	SourceURL;
		public string	Title;
		public int		UniqueID;
		public int		UserEditedFieldMask;
		public string	WMAlbumArtist;
		public string	WMAlbumTitle;
		public string	WMGenre;
		public int		WMGenreID;
		public int		WMTrackNumber;
		public string	WMUniqueFileIdentifier;
		public DateTime	WMYear;

//---------------------------------------------------------------------------------------

		public eQueryTypeAllTracks(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumArtistID); AlbumArtistID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumID); AlbumID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Bitrate); Bitrate = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID); CategoryID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ComposerID); ComposerID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ContributingArtistCount); ContributingArtistCount = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(ArrayList), (uint)SchemaMap.kiIndex_ContributingArtists); ContributingArtists = (ArrayList)(o ?? default(ArrayList));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPAlbumID); CPAlbumID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPArtistID); CPArtistID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded); DateAdded = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateModified); DateModified = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist); DisplayArtist = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Duration); Duration = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName); FileName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileSize); FileSize = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType); FileType = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName); FolderName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_InLibrary); InLibrary = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Is_Protected); Is_Protected = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_ReleaseDate); ReleaseDate = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL); SourceURL = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title); Title = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UserEditedFieldMask); UserEditedFieldMask = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist); WMAlbumArtist = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle); WMAlbumTitle = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenre); WMGenre = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMGenreID); WMGenreID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMTrackNumber); WMTrackNumber = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMUniqueFileIdentifier); WMUniqueFileIdentifier = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_WMYear); WMYear = (DateTime)(o ?? default(DateTime));
		}		// eQueryTypeAllTracks ctor
	}		// eQueryTypeAllTracks class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllTracksDetailed {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllTracksDetailed;


//---------------------------------------------------------------------------------------

		public eQueryTypeAllTracksDetailed(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAllTracksDetailed ctor
	}		// eQueryTypeAllTracksDetailed class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllVideos {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllVideos;

		public int		Bitrate;
		public int		CategoryID;
		public DateTime	DateAdded;
		public string	Description;
		public string	DisplayArtist;
		public int		Duration;
		public string	FileName;
		public int		FileSize;
		public int		FileType;
		public string	FolderName;
		public int		Is_Protected;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		PixelAspectRatioX;
		public int		PixelAspectRatioY;
		public string	SourceURL;
		public string	Title;
		public int		UniqueID;
		public string	WMGenre;
		public string	WMSubTitle;
		public int		WMVideoHeight;
		public int		WMVideoWidth;

//---------------------------------------------------------------------------------------

		public eQueryTypeAllVideos(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Bitrate); Bitrate = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID); CategoryID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded); DateAdded = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Description); Description = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist); DisplayArtist = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Duration); Duration = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName); FileName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileSize); FileSize = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType); FileType = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName); FolderName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Is_Protected); Is_Protected = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PixelAspectRatioX); PixelAspectRatioX = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PixelAspectRatioY); PixelAspectRatioY = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL); SourceURL = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title); Title = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMGenre); WMGenre = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMSubTitle); WMSubTitle = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMVideoHeight); WMVideoHeight = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_WMVideoWidth); WMVideoWidth = (int)(o ?? default(int));
		}		// eQueryTypeAllVideos ctor
	}		// eQueryTypeAllVideos class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllVideosDetailed {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllVideosDetailed;


//---------------------------------------------------------------------------------------

		public eQueryTypeAllVideosDetailed(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAllVideosDetailed ctor
	}		// eQueryTypeAllVideosDetailed class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllPhotos {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllPhotos;

		public string	Author;
		public string	CameraManufacturer;
		public string	CameraModel;
		public int		CategoryID;
		public string	Comment;
		public DateTime	DateTaken;
		public string	FileName;
		public int		FileSize;
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

		public eQueryTypeAllPhotos(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author); Author = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_CameraManufacturer); CameraManufacturer = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_CameraModel); CameraModel = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CategoryID); CategoryID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Comment); Comment = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateTaken); DateTaken = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FileName); FileName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileSize); FileSize = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_FileType); FileType = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_FolderName); FolderName = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PhotoHeight); PhotoHeight = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_PhotoWidth); PhotoWidth = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL); SourceURL = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title); Title = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
		}		// eQueryTypeAllPhotos ctor
	}		// eQueryTypeAllPhotos class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllAlbums {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllAlbums;

		public int		AlbumID;
		public int		ContributingArtistCount;
		public int		CPArtistID;
		public DateTime	DateAdded;
		public int		DisplayArtistCount;
		public int		HasAlbumArt;
		public int		InLibrary;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		MetadataState;
		public string	SourceURL;
		public string	TrackingID;
		public int		UniqueID;
		public int		UserEditedFieldMask;
		public string	WMAlbumArtist;
		public string	WMAlbumTitle;
		public string	WMMCDI;
		public string	WMWMCollectionID;
		public DateTime	WMYear;
		public string	ZuneAlbumCollectionId;

//---------------------------------------------------------------------------------------

		public eQueryTypeAllAlbums(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumID); AlbumID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ContributingArtistCount); ContributingArtistCount = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_CPArtistID); CPArtistID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateAdded); DateAdded = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_DisplayArtistCount); DisplayArtistCount = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_HasAlbumArt); HasAlbumArt = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_InLibrary); InLibrary = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MetadataState); MetadataState = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL); SourceURL = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_TrackingID); TrackingID = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UserEditedFieldMask); UserEditedFieldMask = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist); WMAlbumArtist = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle); WMAlbumTitle = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMMCDI); WMMCDI = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMWMCollectionID); WMWMCollectionID = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_WMYear); WMYear = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_ZuneAlbumCollectionId); ZuneAlbumCollectionId = (string)(o ?? default(string));
		}		// eQueryTypeAllAlbums ctor
	}		// eQueryTypeAllAlbums class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllAlbumArtists {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllAlbumArtists;

		public int		AlbumIDAlbumArtist;
		public int		ArtistAlbumCount;
		public string	DisplayArtist;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public int		UniqueID;
		public string	WMAlbumArtist;

//---------------------------------------------------------------------------------------

		public eQueryTypeAllAlbumArtists(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AlbumIDAlbumArtist); AlbumIDAlbumArtist = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_ArtistAlbumCount); ArtistAlbumCount = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_DisplayArtist); DisplayArtist = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist); WMAlbumArtist = (string)(o ?? default(string));
		}		// eQueryTypeAllAlbumArtists ctor
	}		// eQueryTypeAllAlbumArtists class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllPodcastSeries {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllPodcastSeries;


//---------------------------------------------------------------------------------------

		public eQueryTypeAllPodcastSeries(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAllPodcastSeries ctor
	}		// eQueryTypeAllPodcastSeries class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllPodcastEpisodes {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllPodcastEpisodes;


//---------------------------------------------------------------------------------------

		public eQueryTypeAllPodcastEpisodes(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAllPodcastEpisodes ctor
	}		// eQueryTypeAllPodcastEpisodes class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAlbumsForAlbumArtistId {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAlbumsForAlbumArtistId;


//---------------------------------------------------------------------------------------

		public eQueryTypeAlbumsForAlbumArtistId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAlbumsForAlbumArtistId ctor
	}		// eQueryTypeAlbumsForAlbumArtistId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAlbumsForContributingArtistId {
		// LRS: TODO: public static EQueryType EnumVal = EQueryType.eQueryTypeAlbumsForContributingArtistId;


//---------------------------------------------------------------------------------------

		public eQueryTypeAlbumsForContributingArtistId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAlbumsForContributingArtistId ctor
	}		// eQueryTypeAlbumsForContributingArtistId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeTracksForAlbumId {
		public static EQueryType EnumVal = EQueryType.eQueryTypeTracksForAlbumId;


//---------------------------------------------------------------------------------------

		public eQueryTypeTracksForAlbumId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeTracksForAlbumId ctor
	}		// eQueryTypeTracksForAlbumId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeTracksForAlbumArtistId {
		public static EQueryType EnumVal = EQueryType.eQueryTypeTracksForAlbumArtistId;


//---------------------------------------------------------------------------------------

		public eQueryTypeTracksForAlbumArtistId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeTracksForAlbumArtistId ctor
	}		// eQueryTypeTracksForAlbumArtistId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeEpisodesForSeriesId {
		public static EQueryType EnumVal = EQueryType.eQueryTypeEpisodesForSeriesId;


//---------------------------------------------------------------------------------------

		public eQueryTypeEpisodesForSeriesId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeEpisodesForSeriesId ctor
	}		// eQueryTypeEpisodesForSeriesId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAlbumsByTOC {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAlbumsByTOC;


//---------------------------------------------------------------------------------------

		public eQueryTypeAlbumsByTOC(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAlbumsByTOC ctor
	}		// eQueryTypeAlbumsByTOC class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAlbumsWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAlbumsWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeAlbumsWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAlbumsWithKeyword ctor
	}		// eQueryTypeAlbumsWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeTracksWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeTracksWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeTracksWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeTracksWithKeyword ctor
	}		// eQueryTypeTracksWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeArtistsWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeArtistsWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeArtistsWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeArtistsWithKeyword ctor
	}		// eQueryTypeArtistsWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypePhotosWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypePhotosWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypePhotosWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypePhotosWithKeyword ctor
	}		// eQueryTypePhotosWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeSubscriptionsSeriesWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeSubscriptionsSeriesWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeSubscriptionsSeriesWithKeyword ctor
	}		// eQueryTypeSubscriptionsSeriesWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeSubscriptionsEpisodesWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeSubscriptionsEpisodesWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeSubscriptionsEpisodesWithKeyword ctor
	}		// eQueryTypeSubscriptionsEpisodesWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeVideoWithKeyword {
		public static EQueryType EnumVal = EQueryType.eQueryTypeVideoWithKeyword;


//---------------------------------------------------------------------------------------

		public eQueryTypeVideoWithKeyword(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeVideoWithKeyword ctor
	}		// eQueryTypeVideoWithKeyword class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeMediaFolders {
		public static EQueryType EnumVal = EQueryType.eQueryTypeMediaFolders;


//---------------------------------------------------------------------------------------

		public eQueryTypeMediaFolders(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeMediaFolders ctor
	}		// eQueryTypeMediaFolders class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypePhotosByFolderId {
		public static EQueryType EnumVal = EQueryType.eQueryTypePhotosByFolderId;


//---------------------------------------------------------------------------------------

		public eQueryTypePhotosByFolderId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypePhotosByFolderId ctor
	}		// eQueryTypePhotosByFolderId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeVideosByFolderId {
		public static EQueryType EnumVal = EQueryType.eQueryTypeVideosByFolderId;


//---------------------------------------------------------------------------------------

		public eQueryTypeVideosByFolderId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeVideosByFolderId ctor
	}		// eQueryTypeVideosByFolderId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeSyncProgress {
		public static EQueryType EnumVal = EQueryType.eQueryTypeSyncProgress;


//---------------------------------------------------------------------------------------

		public eQueryTypeSyncProgress(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeSyncProgress ctor
	}		// eQueryTypeSyncProgress class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllPlaylists {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllPlaylists;

		public string	Author;
		public int		AverageRating;
		public int		Count;
		public DateTime	DateModified;
		public int		MediaID;
		public EMediaTypes	MediaType;
		public string	SourceURL;
		public string	Title;
		public ulong	TotalDuration;
		public int		UniqueID;

//---------------------------------------------------------------------------------------

		public eQueryTypeAllPlaylists(ZuneQueryList ZQList, uint n) {
			object	o;

			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Author); Author = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_AverageRating); AverageRating = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_Count); Count = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(DateTime), (uint)SchemaMap.kiIndex_DateModified); DateModified = (DateTime)(o ?? default(DateTime));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_MediaID); MediaID = (int)(o ?? default(int));
			o = ZQList.GetFieldValue(n, typeof(EMediaTypes), (uint)SchemaMap.kiIndex_MediaType); MediaType = (EMediaTypes)(o ?? default(EMediaTypes));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_SourceURL); SourceURL = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(string), (uint)SchemaMap.kiIndex_Title); Title = (string)(o ?? default(string));
			o = ZQList.GetFieldValue(n, typeof(ulong), (uint)SchemaMap.kiIndex_TotalDuration); TotalDuration = (ulong)(o ?? default(ulong));
			o = ZQList.GetFieldValue(n, typeof(int), (uint)SchemaMap.kiIndex_UniqueID); UniqueID = (int)(o ?? default(int));
		}		// eQueryTypeAllPlaylists ctor
	}		// eQueryTypeAllPlaylists class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypePlaylistContentByPlaylistId {
		public static EQueryType EnumVal = EQueryType.eQueryTypePlaylistContentByPlaylistId;


//---------------------------------------------------------------------------------------

		public eQueryTypePlaylistContentByPlaylistId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypePlaylistContentByPlaylistId ctor
	}		// eQueryTypePlaylistContentByPlaylistId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllGenres {
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllGenres;


//---------------------------------------------------------------------------------------

		public eQueryTypeAllGenres(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeAllGenres ctor
	}		// eQueryTypeAllGenres class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeTracksForGenreId {
		// TODO:
		// public static EQueryType EnumVal = EQueryType.eQueryTypeTracksForGenreId;


//---------------------------------------------------------------------------------------

		public eQueryTypeTracksForGenreId(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeTracksForGenreId ctor
	}		// eQueryTypeTracksForGenreId class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeInvalid {
		public static EQueryType EnumVal = EQueryType.eQueryTypeInvalid;


//---------------------------------------------------------------------------------------

		public eQueryTypeInvalid(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeInvalid ctor
	}		// eQueryTypeInvalid class

}
