using System;
using System.Collections;		// For ArrayList
using System.Collections.Generic;

using MicrosoftZuneLibrary;

namespace LRS.Zune.Classes {

	public static class ZuneTypeFactory {

		const string EnumValName = "EnumVal";

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
					// yield return new T(zql, i);
					yield return (T)ZuneTypeFactory.Create<T>(zql, i);
				}
				yield break;
			}
		}

//---------------------------------------------------------------------------------------
		
		public static object Create<T>(ZuneQueryList zql, uint i) where T : class  {
			Type t = typeof(T);
			if (typeof(T) == typeof(eQueryTypeAllTracks))
				return new eQueryTypeAllTracks(zql, i);
			else if (typeof(T) == typeof(eQueryTypeAllVideos))
				return new eQueryTypeAllVideos(zql, i);
			// TODO: Put the rest in (programmatically generated)
			return null;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllTracks {
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
		public static EQueryType EnumVal = EQueryType.eQueryTypeAllVideos;	// TODO:
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

#if false
//---------------------------------------------------------------------------------------

		public static List<eQueryTypeAllVideos>	GetVideos(ZuneLibrary zl) {
			// TODO: IEnumerable<T>, for all types T, + yield return
			// TODO: See also zl.GetTracksByAlbum()
			using (ZuneQueryList zql = zl.QueryDatabase(EQueryType.eQueryTypeAllVideos, 0,
				EQuerySortType.eQuerySortOrderAscending, 0, null)) {
				if (zql == null) {
					// MessageBox.Show("Unable to process Albums");
					return null;
				}
				var Videos = new List<eQueryTypeAllVideos>(zql.Count);
				for (uint i = 0; i < zql.Count; i++) {
					Videos.Add(new eQueryTypeAllVideos(zql, i));
				}
				return Videos;
			}
		}
#endif
	}		// eQueryTypeAllVideos class


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class eQueryTypeAllVideosDetailed {

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

//---------------------------------------------------------------------------------------

		public eQueryTypeInvalid(ZuneQueryList ZQList, uint n) {
			object	o;

		}		// eQueryTypeInvalid ctor
	}		// eQueryTypeInvalid class

}
