using System.Net;

namespace DownloadMsConferences {

	/// <summary>
	///  We need a WebClient that remembers a few extra pieces of information.
	///  Inheritance does come in handy at times!
	/// </summary>
	public class TitledWebClient : WebClient {
		public string Title;
		public string Description;
		public string FullFilename;
		public string SessionUrl;				// Link to session web page
		public DownloadProgress Progress;       // Our custom user control

//---------------------------------------------------------------------------------------

		public TitledWebClient(string Title, string FullFilename, string Description, string SessionUrl) {
			this.Title        = Title;
			this.Description  = Description;
			this.FullFilename = FullFilename;
			this.SessionUrl     = SessionUrl;
		}
	}
}
