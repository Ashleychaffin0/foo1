using System;
using System.Net;

using HAP = HtmlAgilityPack;

namespace FoxTrotSundayDownload {
	internal class FoxTrot {
		WebClient wc = new WebClient();
		HAP.HtmlDocument doc = new HAP.HtmlDocument();

		public FoxTrot() {
			doc.LoadHtml(wc.DownloadString("http://www.foxtrot.com"));
		}

		internal void Download(string url) {
			doc.LoadHtml(wc.DownloadString(url));
			string xp = "//h1";
			var xx = doc.DocumentNode.SelectNodes(xp);
		}
	}
}