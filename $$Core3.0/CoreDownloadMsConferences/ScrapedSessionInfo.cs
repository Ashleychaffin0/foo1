using System.Collections.Generic;

namespace CoreDownloadMsConferences {
	/// <summary>
	/// A minor class, but which holds all the information we need from the web page
	/// </summary>
	public class ScrapedSessionInfo {
		public SessionDownloadInfo Video;
		public SessionDownloadInfo Slides;
		public SessionDownloadInfo Captions;

		// I suppose I really should create a new class, one for each download

		// Each session may have up to 4 characteristics we're interested in:
		//  *   Tags         -- e.g. C#, .Net, Android
		//  *   Track        -- e.g. ALM & DevOps, C++, Cross Platform Mobile
		//  *   Session Type -- e.g. On-Demand Session
		//  *   Level        -- e.g. 200 - Intermediate
		// The following Dictionary is keyed by one of the above
		//     names (e.g. "Tags") and the associated List contains the names
		//     of the Tags/Tracks/etc, e.g. "C#", ".Net"
		public Dictionary<string, List<string>> TagsEtAl;
	}
}
