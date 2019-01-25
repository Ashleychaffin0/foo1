namespace CoreDownloadMsConferences {
	public class SessionDownloadInfo {
		public string Url;
		public long   FileSize;

//---------------------------------------------------------------------------------------

		public SessionDownloadInfo(string Url, long FileSize = 0) { // TODO: NOT = 0
			this. Url     = Url;
			this.FileSize = FileSize;
		}
	}
}
