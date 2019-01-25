using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMusicDupsByName {
	class MusicTrack {
		public bool		IsValid;
		public string	Filename;		// e.g. "03 Cripple Creek.wma"
		public string	DirectoryName;	
		public string	Title;			// e.g. "Cripple Creek"
		public string	Genre;
		public string	Artist;			// Interned
		public TimeSpan	Duration;	
		public long		FileSize; 
		// Any other fields you might be interested in

//---------------------------------------------------------------------------------------

		public MusicTrack(Delimon.Win32.IO.FileInfo fi) {
			IsValid = true;			// Be optimistic
			if ((fi.Extension == ".wma") || (fi.Extension == "mp3")) {
				// Fine
			} else {
				IsValid = false;	// e.g. .jpg
				return;				// Caller will ignore this entry
			}

			try {
				TagLib.File tagFile = TagLib.File.Create(fi.FullName);	 // track is the name of the audio file

				Filename      = fi.Name;
				DirectoryName = string.Intern(fi.DirectoryName);
				Artist        = string.Intern(tagFile.Tag.FirstAlbumArtist);
				Duration      = tagFile.Properties.Duration;
				FileSize      = fi.Length;
				Genre         = tagFile.Tag.FirstGenre;
				Title         = tagFile.Tag.Title;
				if (string.IsNullOrWhiteSpace(Title)) {
					IsValid   = false;
				}
			} catch (Exception /* ex */) {
				IsValid = false;
			}
		}
	}
}
