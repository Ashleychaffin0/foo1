using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRS.Utils {
	/// <summary>
	///  We need a WebClient that remembers a few extra pieces of information.
	///  Inheritance does come in handy at times!
	/// </summary>
	public class TitledWebClient : WebClient {
		public string Title;					// The name of the talk
		public string Description;				// Additional comments
		public string FullFilename;
		public string SessionUrl;               // Link to session web page. AKA "sdi"
		public string ActualUrl;
		public long   UrlSize;
		public object Tag;						// Caller data

		public DownloadFileProgress Progress;   // Our custom user control

//---------------------------------------------------------------------------------------

		public TitledWebClient(
				string				 Title, 
				string				 FullFilename, 
				string				 Description, 
				string				 SessionUrl, 
				string				 ActualUrl,
				object				 Tag
				// ,DownloadFileProgress Progress
			) {
			this.Title         = Title;
			this.FullFilename  = FullFilename;
			this.Description   = Description ?? "(No Description)";
			this.SessionUrl    = SessionUrl;
			this.ActualUrl     = ActualUrl;
			this.Tag           = Tag ?? this;
			// this.Progress	   = Progress;


			// this.UrlSize    = GetUrlSize(ActualUrl);

			// var tSize = GetUrlSize(ActualUrl);
			// tSize.Wait();
			// UrlSize = tSize.Result;
			// Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// An async method to return the size of the target of a URL.
		/// </summary>
		/// <param name="Url">A URL</param>
		/// <returns>The size of the target of the Url parameter</returns>
		public async static Task<long> GetUrlSizeAsync(string Url) {
			string UrlUpcase = Url.ToUpper();
			if (!((UrlUpcase.StartsWith("HTTP://")) || (UrlUpcase.StartsWith("HTTPS://")))) {
				Url = "http://" + Url;
			}
			HttpWebRequest req   = (HttpWebRequest)WebRequest.Create(Url);
			req.Method           = "HEAD";
			using (var resp =  (HttpWebResponse) await req.GetResponseAsync()) {
				return resp.ContentLength;
			}
		}
	}
}
