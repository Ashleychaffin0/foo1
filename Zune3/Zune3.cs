using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MicrosoftZuneInterop;
using MicrosoftZuneLibrary;
using MicrosoftZunePlayback;

using Microsoft.Zune.Configuration;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;

using Microsoft.Iris.Accessibility;
using Microsoft.Iris.OS;
using Microsoft.Iris.OS.CLR;
using Microsoft.Iris.Session;

namespace Zune3 {
	public partial class LRSZune3 : Form {
		MicrosoftZuneLibrary.ZuneLibrary zl;

//---------------------------------------------------------------------------------------

		public LRSZune3() {
			InitializeComponent();

			// TODO: There are a bunch of things in Zune.Util. For example:
			// Microsoft.Zune.Util.Notification.BroadcastNowPlaying(...);

			// TODO: And ditto for the others.

			// var zl2 = Microsoft.Zune.Shell.ZuneApplication.ZuneLibrary;	// Assumes Zune is running?

			// TODO: SqlMetal SqlCeFileName.sdf /dbml:LinqClassName.dbml
			//		 Then add the .dbml file to the project in the usual way.
			//		 Then Tageditor te = new Tageditor(TagEditor.Properties.Settings.Default.TagEditorConnectionString)


#if false
			zl = new ZuneLibrary();
			int i1 = zl.Initialize();
			if (i1 != 0) {
				string msg = string.Format("Could not initialize the Zune Library, rc = {0:X8}", i1);
				MessageBox.Show(msg);
				System.Windows.Forms.Application.Exit();
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnArtists_Click(object sender, EventArgs e) {
			ShowArtists();
		}

//---------------------------------------------------------------------------------------

		private void ShowArtists_Original_Works() {
			MicrosoftZuneLibrary.ZuneQueryList artists;
#if false
			artists = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbumArtists, 0, EQuerySortType.eQuerySortOrderDescending,
				// (uint)SchemaMap.kiIndex_DisplayArtist, null);
				(uint)SchemaMap.kiIndex_ArtistAlbumCount, null);
			// var x = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbumArtists, 0, EQuerySortType.eQuerySortOrderAscending,
			// 	0 * (uint)SchemaMap.kiIndex_AlbumID, null);
			// MessageBox.Show("Artist count = " + artists.Count);
#endif
			StreamWriter wtr = new StreamWriter(@"C:\lrs\ZuneAlbumsByArtists.html", false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Zune Albums By Artists</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine(string.Format("<center><FONT SIZE=\"+2\">LRS Zune Albums by Artists as of {0}</FONT></center>\n<br>\n", DateTime.Now));
			// TODO: Try <artists> on something else
			artists = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0, EQuerySortType.eQuerySortOrderAscending,
				(uint)SchemaMap.kiIndex_WMAlbumArtist, null);
			string PrevArtist = "";
			string delim = "";
			bool underline = false;
			int count = 0;
			for (uint i = 0; i < artists.Count; i++) {
				string Artist = (string)artists.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
				string Title = (string)artists.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
				// o = artists.GetFieldValue(i, typeof(uint), (uint)SchemaMap.kiIndex_DisplayArtistCount);
				// Console.WriteLine("Artist[{0}] = '{1}', Title = {2}", i, Artist, Title);
				if (Artist != PrevArtist) {
					wtr.WriteLine(string.Format("<br><b>{0}</b> - ", Artist));
					PrevArtist = Artist;
					delim = "";
					count = 0;
				}
				wtr.WriteLine(delim);
				if ((count > 0) && ((count % 4) == 0)) {
					wtr.WriteLine("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
				}
				++count;
				if (underline) {
					// wtr.Write(string.Format("<u>{0}</u>", Title));
					wtr.Write(string.Format("<FONT COLOR=\"RED\">{0}</FONT>", Title));
				} else {
					wtr.WriteLine(Title);
				}
				underline = !underline;
				delim = "; ";
			}
			wtr.WriteLine("\n</BODY>\n</HTML>");
			wtr.Close();
			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void ShowArtists() {
			MicrosoftZuneLibrary.ZuneQueryList artists;
#if false
			artists = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbumArtists, 0, EQuerySortType.eQuerySortOrderDescending,
				// (uint)SchemaMap.kiIndex_DisplayArtist, null);
				(uint)SchemaMap.kiIndex_ArtistAlbumCount, null);
			// var x = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbumArtists, 0, EQuerySortType.eQuerySortOrderAscending,
			// 	0 * (uint)SchemaMap.kiIndex_AlbumID, null);
			// MessageBox.Show("Artist count = " + artists.Count);
#endif
			StreamWriter wtr = new StreamWriter(@"C:\lrs\ZuneAlbumsByArtists.html", false, Encoding.Unicode);
			wtr.WriteLine("<HTML>\n<HEAD>\n<TITLE>Zune Albums By Artists</TITLE>\n</HEAD>\n<BODY>");
			wtr.WriteLine(string.Format("<center><FONT SIZE=\"+2\">LRS Zune Albums by Artists as of {0}</FONT></center>\n<br>\n", DateTime.Now));
			// TODO: Try <artists> on something else
			artists = zl.QueryDatabase(EQueryType.eQueryTypeAllAlbums, 0, EQuerySortType.eQuerySortOrderAscending,
				(uint)SchemaMap.kiIndex_WMAlbumArtist, null);
			string PrevArtist = "";
			var Titles = new List<string>();
			for (uint i = 0; i < artists.Count; i++) {
				string Artist = (string)artists.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumArtist);
				string Title = (string)artists.GetFieldValue(i, typeof(string), (uint)SchemaMap.kiIndex_WMAlbumTitle);
				// o = artists.GetFieldValue(i, typeof(uint), (uint)SchemaMap.kiIndex_DisplayArtistCount);
				// Console.WriteLine("Artist[{0}] = '{1}', Title = {2}", i, Artist, Title);
				if (Artist != PrevArtist) {
					FormatTitles(wtr, Titles);			// For previous Artist
					Titles.Clear();
					wtr.WriteLine(string.Format("<br><b>{0}</b> - ", Artist));
					PrevArtist = Artist;
				}
				Titles.Add(Title);
			}
			FormatTitles(wtr, Titles);
			wtr.WriteLine("\n</BODY>\n</HTML>");
			wtr.Close();
			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void FormatTitles(StreamWriter wtr, List<string> Titles) {
			if (Titles.Count == 0)
				return;
			var SortedTitles = Titles.OrderBy(title => title);
			// Now the formatting part
			string delim = "";
			int count = 0;
			bool underline = false;
			foreach (var title in SortedTitles) {
				wtr.WriteLine(delim);
				if ((count > 0) && ((count % 4) == 0)) {
					wtr.WriteLine("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
				}
				++count;
				if (underline) {
					// wtr.Write(string.Format("<u>{0}</u>", title));
					wtr.Write(string.Format("<FONT COLOR=\"RED\">{0}</FONT>", title));
				} else {
					wtr.WriteLine(title);
				}
				underline = !underline;
				delim = "; ";
			}
		}

//---------------------------------------------------------------------------------------

		private void LRSZune3_Load(object sender, EventArgs e) {
			zl = new ZuneLibrary();
			bool dbRebuilt;
			int i1 = zl.Initialize("", out dbRebuilt);
			if (i1 != 0) {
				string msg = string.Format("Could not initialize the Zune Library, rc = {0:X8}", i1);
				MessageBox.Show(msg);
				//System.Windows.Forms.Application.Exit();
				this.Close();
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		private void dbgTryAllTypes_Click(object sender, EventArgs e) {
			// TODO: Put all failed smt enum names into a List<>, and print them out at the end
			//		 of the EQType. Ditto for Exceptions.
			// This attempts sorting, but doesn't seem to work
			MicrosoftZuneLibrary.ZuneQueryList ZQList;
			int nEQTypes = 0;
			var BadCombinations = new List<SchemaMap>();
			foreach (var EQType in Enum.GetValues(typeof(EQueryType))) {
				Console.WriteLine("\n\n{0,3}: EQType = {1} / {2}", ++nEQTypes, EQType, (int)EQType);
				int nSmts = 0;
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
						string msg = string.Format("\t***** Exception thrown for EQType/SchemaMap = {0}/{1} - Exception = {2}",
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
			uint ix = 0;				// Index into ZQList
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
	}
}
