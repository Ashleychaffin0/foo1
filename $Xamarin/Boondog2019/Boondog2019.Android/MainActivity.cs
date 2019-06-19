using System;
using System.IO;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android;
using Android.Media;
using Android.Provider;
using Android.Content;

namespace nsBoondog2019.Droid {
	[Activity(Label = "Boondog2019", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
		protected override void OnCreate(Bundle savedInstanceState) {
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());

			var xxx = new Android.Provider.MediaStore.Files();
			var yyy = MediaStore.Files.GetContentUri("external");
			var a = yyy.DescribeContents();
			var b = yyy.EncodedPath;
			var c = yyy.Scheme;
			var e = yyy.UserInfo;
			var extStore = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic);
			// https://stackoverflow.com/questions/10384080/mediastore-uri-to-query-all-types-of-files-media-and-non-media
			// https://www.sandersdenardi.com/querying-and-removing-media-from-android-mediastore/
#if false
			var ctx = Xamarin.Forms.Forms.Context;
			string[] retCol = { MediaStore.Audio.Albums. };
			Cursor cur = ctx.getContentResolver().query(
  MediaStore.Audio.Media.EXTERNAL_CONTENT_URI,
  retCol,
  MediaStore.MediaColumns.DATA + "='" + filePath + "'", null, null
);
			if (cur.getCount() == 0) {
				return;
			}
			cur.moveToFirst();
			int id = cur.getInt(cur.getColumnIndex(MediaStore.MediaColumns._ID));
			cur.close();
#endif

			var md1File = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic);
			var md = md1File.ToString();
			var els = Directory.GetFileSystemEntries(md);
			var drives = System.IO.Directory.GetLogicalDrives();
			foreach (string d in drives) {
				try {
					var dirs = Directory.GetFileSystemEntries(d);
					// System.Diagnostics.Debugger.Break();
					Console.WriteLine($"Good: {d}");

				} catch (Exception ex) {
					nsBoondog2019.App.MusicDir = ex.Message;
					Console.WriteLine($"\t\t{ex.Message}\tBad: {d}");
					// System.Diagnostics.Debugger.Break();
				}
			}
			nsBoondog2019.App.MusicDir = md1File.ToString();

		}
	}
}