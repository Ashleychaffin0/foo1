using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportsApplication6 {
	class MyReportData {

		public string Artist { get; set; }
		public string Album { get; set; }
		public List<string> Tracks { get; set; }

		public MyReportData(string Artist, string Album, List<string> Tracks) {
			this.Artist = Artist;
			this.Album = Album;
			this.Tracks = Tracks;
		}
	}
}
