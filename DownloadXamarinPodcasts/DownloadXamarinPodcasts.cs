using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace DownloadXamarinPodcasts {
	public partial class DownloadXamarinPodcasts : Form {
		public DownloadXamarinPodcasts() {
			InitializeComponent();

		}

		private async void BtnGo_Click(object sender, EventArgs e) {

			string RssUrl = "http://www.xamarinpodcast.com/rss";

			var wreq        = (HttpWebRequest)WebRequest.CreateHttp(RssUrl);
			var RssResponse = await wreq.GetResponseAsync();
			var RssStream   = RssResponse.GetResponseStream();
			var RssDoc      = XDocument.Load(RssStream);

			var PodCasts = from item in RssDoc.Element("rss").Element("channel").Elements("item")
						   let enc   = item.Element("enclosure")
						   let Url   = item.Element("link")?.Value
						   let SeqNo = Convert.ToInt32(Url.Substring(Url.LastIndexOf('/') + 1))
						   orderby SeqNo
						   // select new RssItem {
						   select new {
							   Title		 = CleanTitle(item.Element("title")?.Value?.Trim()),
							   Url,
							   Description   = item.Element("description")?.Value?.Trim(),
							   PubDate       = Convert.ToDateTime(item.Element("pubDate")?.Value),
							   EnclosureUrl  = enc.Attribute("url")?.Value,
							   FileSize      = Convert.ToInt32(enc.Attribute("length")?.Value),
							   EnclosureType = enc.Attribute("type")?.Value,
							   SeqNo
						   };
			foreach (var pod in PodCasts) {
				// var wc = new WebClient();
				// var uri = new Uri(SessInfo.EnclosureUrl);
				// await wc.DownloadFileTaskAsync(uri, @"G:\lrs\foo.mp3");
				Console.WriteLine($"{pod.EnclosureType} -- {pod.Title} -- {pod.EnclosureUrl}");
			}
		}

		private string GetSessionID(string value) => value;
		private string CleanTitle(string v) => v;
	}
}
