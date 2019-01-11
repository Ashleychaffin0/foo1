using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Reflection;

using mscoree;	// For the Debug routines
using System.Runtime.InteropServices;

#if true		// TODO:

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

#endif

using LRS.Zune.ClassGeneration;

using LRS.Zune.Classes;

// References: UIX, ZuneDbAPI, ZuneShell


// See Tom Fuller's entry at
// http://soapitstop.com/blogs/fleamarket/archive/2008/03/03/read-the-zune-collection-in-net-from-zune-s-own-api.aspx


namespace ZuneGenerateClasses {
	public partial class ZuneGenerateClasses : Form {

		const string	BackupDir = @"C:\LRS";
		const string	BackupFilename = "Zunestore - Backup.sdf";

		MicrosoftZuneLibrary.ZuneLibrary zl;

//---------------------------------------------------------------------------------------

		string	_ZuneDbDir = null;
		string ZuneDbDir {
			get {
				if (_ZuneDbDir == null) {
					string user = Environment.GetEnvironmentVariable("USERPROFILE");
					_ZuneDbDir = Path.Combine(user, @"Appdata\Local\Microsoft\Zune");
				}
				return _ZuneDbDir;
			}
		}

//---------------------------------------------------------------------------------------

		string	_ZuneInstallationDir = null;
		string ZuneInstallationDir {
			get {
				if (_ZuneInstallationDir == null) {
					string ZuneReg = @"SOFTWARE\Microsoft\Zune";
					RegistryKey key = Registry.LocalMachine;
					RegistryKey ZuneKey = key.OpenSubKey(ZuneReg, false);
					_ZuneInstallationDir = (string)ZuneKey.GetValue("Installation Directory");
				}
				return _ZuneInstallationDir;
			}
		}

//---------------------------------------------------------------------------------------

		public ZuneGenerateClasses() {
			InitializeComponent();

#if true
			zl = new ZuneLibrary();
			int i1 = zl.Initialize();
			if (i1 != 0) {
				string	msg = string.Format("Could not initialize the Zune Library, rc = {0:X8}", i1);
				MessageBox.Show(msg);
				System.Windows.Forms.Application.Exit();
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnBackupZuneDatabase_Click(object sender, EventArgs e) {
			string	FromName = Path.Combine(ZuneDbDir, "Zunestore.sdf");
			string	ToName	 = Path.Combine(BackupDir, BackupFilename);
			Cursor	SaveCurs = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try {
				File.Copy(FromName, ToName, true);
			} finally {
				this.Cursor = SaveCurs;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnRestoreZuneDatabase_Click(object sender, EventArgs e) {
			string	FromName = Path.Combine(BackupDir, BackupFilename);
			string	ToName	 = Path.Combine(ZuneDbDir, "Zunestore.sdf");
			Cursor	SaveCurs = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try {
				File.Copy(FromName, ToName, true);
			} finally {
				this.Cursor = SaveCurs;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGenerateClasses_Click(object sender, EventArgs e) {
			// MessageBox.Show("Nonce on Generate Classes");
			const string EnumValName = "EnumVal";

			GenerateLRSZuneClasses.CreateLRSZuneClasses(zl, EnumValName);
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

		private void btnTest_Click(object sender, EventArgs e) {
			string	CurDir = Environment.CurrentDirectory;
			try {
				Environment.CurrentDirectory = ZuneInstallationDir;
				Test1();
			} finally {
				Environment.CurrentDirectory = CurDir;
			}
		}

//---------------------------------------------------------------------------------------

		private void Test1() {
			try {
				// AppDomain ad = AppDomain.CurrentDomain;
				// ad.AppendPrivatePath(ZuneInstallationDir);
				// DebugRtns.dbgShowAppDomains();
				// AppDomain.CurrentDomain.DynamicDirectory = ZuneInstallationDir;
				// AppDomainSetup ads = new AppDomainSetup();
				// ads.PrivateBinPath = ZuneInstallationDir;
				// AppDomain.CurrentDomain.s
				// AppDomain.CurrentDomain.SetDynamicBase(ZuneInstallationDir);

				AppDomain	CurAD = AppDomain.CurrentDomain;
				// CurAD.AssemblyResolve += new ResolveEventHandler(CurAD_AssemblyResolve);
				AssemblyName	an = new AssemblyName("UIX");
				an.CodeBase = ZuneInstallationDir;
				Assembly.Load(an);

#if false
				byte [] shellBytes = File.ReadAllBytes(Path.Combine(ZuneInstallationDir, "ZuneShell.dll"));
				Assembly asmShell = Assembly.Load(shellBytes);

				byte [] UiBytes = File.ReadAllBytes(Path.Combine(ZuneInstallationDir, "UIX.dll"));
				Assembly asmUI = Assembly.Load(UiBytes);

				var ev = new System.Security.Policy.Evidence();
				Assembly.LoadFrom(Path.Combine(ZuneInstallationDir, "ZuneDbApi.dll"), ev);

				DebugRtns.dbgShowAppDomains();

				byte [] zdbBytes = File.ReadAllBytes(Path.Combine(ZuneInstallationDir, "ZuneDbApi.dll"));
				Assembly asmZdb = Assembly.Load(zdbBytes);
#endif

				AppDomainSetup ads = new AppDomainSetup();
				ads.ApplicationBase = ZuneInstallationDir;
				ads.PrivateBinPath = ZuneInstallationDir;
				AppDomain		ad = AppDomain.CreateDomain("MyZuneAppdomain", null, ads);
				// ad.AssemblyResolve += new ResolveEventHandler(ad_AssemblyResolve);
				
				// ad.SetDynamicBase(ZuneInstallationDir);
				Console.WriteLine("ad.BaseDirectory = {0}", ad.BaseDirectory);
				ad.Load("UIX");

#if false
				Assembly Ux = Assembly.LoadFile(Path.Combine(ZuneInstallationDir, "UIX.dll"));
				Console.WriteLine("\r\nUX: ==========\r\n");
				DebugRtns.dbgShowAppDomains();

				Assembly Shell = Assembly.LoadFile(Path.Combine(ZuneInstallationDir, "ZuneShell.dll"));
				Console.WriteLine("\r\nShell: ==========\r\n");
				DebugRtns.dbgShowAppDomains();

				Assembly DbApi = Assembly.LoadFile(Path.Combine(ZuneInstallationDir, "ZuneDBApi.dll"));
				Console.WriteLine("\r\nDbApi: ==========\r\n");
				DebugRtns.dbgShowAppDomains();

				Assembly x = Ux;
				Assembly y = DbApi;

				Type[] tx = x.GetTypes();
				Type[] ty = y.GetTypes();
#endif
			} catch (ReflectionTypeLoadException ex) {
				MessageBox.Show("ReflectionTypeLoadException: " + ex.Message);
			} catch (FileNotFoundException ex) {
				string	msg = string.Format("FileNotFoundException: {0}"
					+ "\r\r\n\r\r\nFusion: {1}",
					ex.Message, ex.FusionLog);
				MessageBox.Show(msg);
			} catch (Exception ex) {
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

//---------------------------------------------------------------------------------------

		Assembly ad_AssemblyResolve(object sender, ResolveEventArgs args) {
			string name = args.Name;
			Type t = sender.GetType();
			return null;
		}

//---------------------------------------------------------------------------------------

		Assembly CurAD_AssemblyResolve(object sender, ResolveEventArgs args) {
			string name = args.Name;
			Type t = sender.GetType();
			return null;
		}

//---------------------------------------------------------------------------------------

		private void btnTest2_Click(object sender, EventArgs e) {
			ShowArtists();
		}

//---------------------------------------------------------------------------------------

		private void btnSendAllInfoToFile_Click(object sender, EventArgs e) {
			Cursor	CurCurs = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try {
				DumpAllZune();
			} finally {
				this.Cursor = CurCurs;
			}
		}

//---------------------------------------------------------------------------------------

		private void DumpAllZune() {
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
			// ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksForGenreId>(zl, "LRS_Zune_Tracks_For_Genre_ID.txt");

			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeTracksWithKeyword>(zl, "LRS_Zune_Tracks_With_Keyword.txt");
			ZuneTypeFactory.DumpZuneItemsViaClass<eQueryTypeAllTracks>(zl, "LRS_Zune_Tracks.txt");
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class DebugRtns {

//---------------------------------------------------------------------------------------

		// Debug routine.
		//	DO NOT CALL WHEN A PLUG-IN IS IN MEMORY, ELSE A COPY OF IT WILL BE LOADED 
		//	INTO THE CALLING APPDOMAIN. CALL IT EITHER BEFORE YOU LOAD THE PLUG-IN, OR
		//	AFTER IT'S UNLOADED, TO ENSURE THAT YOU HAVEN'T INADVERTENTLY DONE SOMETHING
		//	TO LOAD IT INTO THE CALLER.
		public static void dbgShowAppDomains() {
			List<AppDomain> ads = dbgGetAppDomains();
			Console.Write("\r\n\r\n---------------");
			foreach (AppDomain appdom in ads) {
				Console.WriteLine("\r\n------------\r\nAppDomain: {0}", appdom.FriendlyName);
				dbgShowAsms(appdom);
			}
		}

//---------------------------------------------------------------------------------------

		// Debug routine.
		public static void dbgShowAsms() {
			dbgShowAsms(AppDomain.CurrentDomain);
		}

//---------------------------------------------------------------------------------------

		// Debug routine.
		public static void dbgShowAsms(AppDomain ad) {
			// Don't bother showing fairly standard Assemblies that are/might be in
			// any given AppDomain.
			string[] AsmsToIgnore = new string[]  {
				"Microsoft.VisualStudio.",
				"System,",
				"Accessibility",
				"System.Data",
				"System.Core,",
				"System.Deployment",
				"mscorlib,",
				"System.Drawing",
				"System.Windows.",
				"Accessability",
				"Interop.mscoree",
				"System.Xml",
				"System.Configuration,",
				"vshost,"
			};
			Assembly[] asms = ad.GetAssemblies();

			foreach (var asm in asms) {
				bool bPrintit = true;
				foreach (var prefix in AsmsToIgnore) {
					if (asm.FullName.StartsWith(prefix)) {
						bPrintit = false;
						break;
					}
				}
				if (bPrintit) {
					Console.WriteLine("\r\n* {0}", asm.FullName);
					ShowTypes(asm);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowTypes(Assembly asm) {
			Type[] types = asm.GetExportedTypes();
			foreach (Type t in types) {
				Console.WriteLine("\t ** {0}", t.FullName);
			}
		}

//---------------------------------------------------------------------------------------

		public static List<AppDomain> dbgGetAppDomains() {
			CorRuntimeHostClass host = new CorRuntimeHostClass();
			try {
				List<AppDomain> list = new List<AppDomain>();
				IntPtr enumHandle;
				host.EnumDomains(out enumHandle);
				while (true) {
					object domain;
					host.NextDomain(enumHandle, out domain);
					if (domain == null)
						break;
					list.Add((AppDomain)domain);
				}
				host.CloseEnum(enumHandle);
				return list;
			} finally {
				Marshal.ReleaseComObject(host);
			}
		}
	}
}
