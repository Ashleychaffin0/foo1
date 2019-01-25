using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace CheckFor192Rip {
	class AlbumInfo {
		public string	PathName;
		public string	ArtistName;
		public string	AlbumName;
		public long		Size;
		public TimeSpan Duration;
		public bool		IsGood = false;
		public int		Rate;
		public int		NumberOfCuts;
		public string	PrimaryGenre;		// May be several; each cut may be different

		public static DateTime EarliestCreationTime;
		// Next line is meant to be used only as a debugging info source
		public static List<(DateTime CreationTime, string Pathname)> Earlies;


//---------------------------------------------------------------------------------------

		static AlbumInfo() {
			EarliestCreationTime = new DateTime(2100, 12, 25);
			Earlies              = new List<(DateTime, string)>();
		}

//---------------------------------------------------------------------------------------

		public AlbumInfo(string filePath) {
			var PathInfo = new FileInfo(filePath);
			if (!PathInfo.Attributes.HasFlag(FileAttributes.Directory)) { return; }

			GetAlbumBasics(filePath);

			this.PathName = filePath;
			Size          = 0;
			Duration      = new TimeSpan(0);
			var files     = Directory.GetFiles(filePath, "*.wma");
			NumberOfCuts  = files.Count();
			int CutNumber = 0;

			foreach (var file in files) {
				var fi = new FileInfo(file);
				var cut = GetTagsEtAl(file);
				if (CutNumber++ == 0) {
					IsGood = true;
					Rate = cut.Properties.AudioBitrate;
					PrimaryGenre = cut.Tag.FirstGenre ?? "Unknown Genre";
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
				// See https://www.markheath.net/post/how-to-get-media-file-duration-in-c
				using (var shell = ShellObject.FromParsingName(fi.FullName)) {
					IShellProperty prop = shell.Properties.System.Media.Duration;
					var t = (ulong)prop.ValueAsObject;
					Duration += TimeSpan.FromTicks((long)t);
				}
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
