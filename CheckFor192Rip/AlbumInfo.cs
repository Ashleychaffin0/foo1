using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace CheckFor192Rip {
	class AlbumInfo {
		public string	 PathName;
		public string	 ArtistName;
		public string	 AlbumName;
		public long		 Size;
		public TimeSpan  Duration;
		public bool		 IsAlbumNotArtist = false;
		public int		 Rate;				// In kBits/sec
		public int		 NumberOfCuts;
		public string	 PrimaryGenre;		// May be several; each cut may be different
		public string	 Title;
		public List<Cut> Cuts;

		HashSet<string> Titles = new HashSet<string>();

		public static DateTime EarliestCreationTime;
		// Next line is meant to be used only as a debugging info source
		public static List<(DateTime CreationTime, string Pathname)> Earlies;

 		List<(int cut, int rate, string filename)> CutsTuple;

		public bool bIs128;
		public bool bIs192;
		public bool bIsMixed;

//---------------------------------------------------------------------------------------

		static AlbumInfo() {
			EarliestCreationTime = new DateTime(2100, 12, 25);
			Earlies              = new List<(DateTime, string)>();
		}

//---------------------------------------------------------------------------------------

		public AlbumInfo(string filePath) {
			CutsTuple    = new List<(int cut, int rate, string filename)>();
			Cuts         = new List<Cut>();
			bIsMixed     = false;
			bIs128       = false;
			bIs192       = false;
			var PathInfo = new FileInfo(filePath);
			if (!PathInfo.Attributes.HasFlag(FileAttributes.Directory)) { return; }

			GetAlbumBasics(filePath);

			this.PathName = filePath;
			Size          = 0;
			Duration      = new TimeSpan(0);
			var files     = Directory.GetFiles(filePath, "*.wma");
			NumberOfCuts  = files.Count();
			int CutNumber = 0;

			foreach (var xfile in files) {
				var file = @"\\?\" + xfile;
				IsAlbumNotArtist = true;

				var fi      = new FileInfo(file);
				var CutInfo = GetTagsEtAl(file);
				var cut     = new Cut(CutInfo);
				cut.Title   = CutInfo.Tag.Title;
				cut.Rate    = CutInfo.Properties.AudioBitrate;
				Cuts.Add(cut);

				this.Rate = cut.Rate;
				this.Title = Path.GetFileName(PathName);


				bIs128 = Rate == 128;
				bIs192 = Rate == 192;
				Duration += CutInfo.Properties.Duration;
				CutsTuple.Add(((int)CutInfo.Tag.Track, Rate, file));
				if (CutNumber++ == 0) {			// Only once per album
					PrimaryGenre = CutInfo.Tag.FirstGenre ?? "Unknown Genre";
					if (Rate == 192) {
						if (fi.CreationTime.Year < 2018) {
							Earlies.Add((fi.CreationTime, filePath));
						}
						if (fi.CreationTime < EarliestCreationTime) {
							EarliestCreationTime = fi.CreationTime;
						}
					}
				}
				Size += fi.Length;
				cut.Filename = Path.GetFileName(file);
#if false
				// See https://www.markheath.net/post/how-to-get-media-file-duration-in-c
				using (var shell        = ShellObject.FromParsingName(fi.FullName)) {
					IShellProperty prop = shell.Properties.System.Media.Duration;
					var t               = (ulong)prop.ValueAsObject;
					Duration           += TimeSpan.FromTicks((long)t);
				}
#endif
			}
			bIsMixed = bIs128 && bIs192;
			if (bIsMixed) {
				Console.WriteLine($"        ******** Mixed rates: {filePath}");
				var cut128 = CutsTuple.First(c => c.rate == 128);
				var cut192 = CutsTuple.First(c => c.rate == 192);
				Console.WriteLine($"        **** 128->{cut128.filename}, 192->{cut192.filename}");
			}
		}

//---------------------------------------------------------------------------------------

		private TagLib.File GetTagsEtAl(string file) {
			int MaxTries = 30;
			for (int i = 0; i < MaxTries; i++) {
				try {
					var cut = TagLib.File.Create(file);
					return cut;
				} catch {
					System.Threading.Thread.Sleep(2000);
				}
			}
			throw new TimeoutException($"Can't get tags et al for {file}");
		}

//---------------------------------------------------------------------------------------

		void GetAlbumBasics(string dirName) {
			var parts = dirName.Split(new char[] { '\\' });
			ArtistName = parts[2];
			if (parts.Length == 4) {
				AlbumName = parts[3];
			} else {
				AlbumName = "";
			}
		}

	}
}
