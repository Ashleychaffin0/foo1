using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsAlbumData {
	public class MyReportData {
		public string AlbumName { get; set; }
		public string Artist {get; set;}
		public List<string> TrackNames {get; set;}

		//---------------------------------------------------------------------------------------

		public MyReportData(string AlbumName, string Artist, List<string> TrackNames) {
			this.AlbumName = AlbumName;
			this.Artist = Artist;
			this.TrackNames = TrackNames;
		}
	}
}
