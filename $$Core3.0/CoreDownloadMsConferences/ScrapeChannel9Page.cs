using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using HAP = HtmlAgilityPack;

namespace CoreDownloadMsConferences {
	/// <summary>
	/// Given a URL, download it, then scrape it for the highest resolution video URL and
	/// the URL for the PowerPoint slides (if there is such a link). Also support
	/// Captions, if available.
	/// </summary>
	public partial class ScrapeChannel9Page {
		public const string TagName         = "$Tags";
		public const string LevelName       = "$Level";
		public const string SessionTypeName = "$Session Type";
		public const string TrackName       = "$Track";

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Actually do the scraping
		/// </summary>
		/// <param name="Url"></param>
		/// <returns></returns>
		public /* static */ async Task<ScrapedSessionInfo> ScrapeAsync(CoreDownloadMsConferences.RssItem item) {
			// Ultimately we want the following scrape the HTML to create a ScrapedInfo.
			// See the ScrapeInfo class, but basically we need
			//  *   The link to the video file
			//  *   The link to the slides (if any)
			//  *   The link to the Captions
			//  *   A Dictionary of metadata about the session -- tags, session type, etc

			// Download the html
			var wc        = new WebClient();
			var TargetUri = new Uri(item.Url);
			string html   = await wc.DownloadStringTaskAsync(TargetUri).ConfigureAwait(false);
#if false   // To be able to see the HTML
			System.Windows.Forms.Clipboard.SetText(html);
#endif
			// Parse it
			var doc = new HAP.HtmlDocument();
			doc.LoadHtml(html);         // Create the Document Object Model (DOM)
										// from the HTML
			html = null;                // Remove a bit of memory pressure. Don't need
										// this any more

			var MainNode = doc.DocumentNode.Descendants("main").FirstOrDefault();
			var Title    = MainNode.Descendants("h1").FirstOrDefault().InnerText;

			string VideoUrl = GetVideoUrl(MainNode);
			if (VideoUrl == null) {
				CoreDownloadMsConferences.scGui.Post(o => CoreDownloadMsConferences.Mainline.msg($"****** No video for {item.Title} -- {item.Url}"), 0);
				Interlocked.Increment(ref CoreDownloadMsConferences.nVideosNotFound);
			}
			string SlidesUrl   = GetSlidesUrl(MainNode);
			string CaptionsUrl = GetCaptionsUrl2(MainNode);		// TODO: TODO: TODO:
				
			var TagsLevelSessionType = new Dictionary<string, List<string>>();
			ScrapeTagsLevelSessionTypeTrack(MainNode, TagsLevelSessionType);

#if false
			var twc = new TitledWebClient("", "", "", "", null);    // TODO: Test
			var SizeVideo = await twc.GetUrlSize(VideoUrl);
#endif

			return new ScrapedSessionInfo {
				// Video    = new SessionDownloadInfo(VideoUrl, await TitledWebClient.GetUrlSize(VideoUrl)),
				// Video    = new SessionDownloadInfo(VideoUrl, await TitledWebClient.GetUrlSize(VideoUrl).ConfigureAwait(false)),
				Video    = new SessionDownloadInfo(VideoUrl),
				Slides   = new SessionDownloadInfo(SlidesUrl),
				Captions = new SessionDownloadInfo(CaptionsUrl),
				TagsEtAl = TagsLevelSessionType
			};
		}

//---------------------------------------------------------------------------------------

		private static string GetVideoUrl(HAP.HtmlNode MainNode) {
			string VideoLink = null;
			var DivDownload = FindTagAndAttribute(MainNode, "div", "class", "download").FirstOrDefault();
			if (DivDownload != null) {
#if true
				// This is the problem with screen scraping. You've got to change your
				// algorithms when the web page changes its layout. Sigh. Maybe we'll
				// have to add a Version parameter to the .xml file for a Conference.
				// TODO: Turn this into GetHighestResolutionVideoSourceUrl_2
				// TODO: Need to modify code for Slides and Captions.
				var links = DivDownload.Descendants("a");
				int BestLevel = -1;
				foreach (var link in links) {
					string type = link.InnerText.Trim();
					string url  = link.Attributes["href"].Value;
					if ((type == "Low Quality MP4") && (BestLevel < 0)) {
						VideoLink = url;
						BestLevel = 0;
					} else if ((type == "Mid Quality MP4") && (BestLevel < 1)) {
						VideoLink = url;
						BestLevel = 1;
					} else if ((type == "High Quality MP4") && (BestLevel < 2)) {
						VideoLink = url;
						BestLevel = 2;
					} else {
						continue;           // Ignore. e.g. MP3
					}
				}
			}
#else
				var Options = DivDownload.Descendants("option");
				var Opts    = GetOptions(Options);
				VideoLink   = GetHighestResolutionVideoSourceUrl(Opts);
			}
#endif
			return VideoLink;
		}

//---------------------------------------------------------------------------------------

		private static string GetSlidesUrl(HAP.HtmlNode MainNode) {
			var SlidesAndZip = FindTagAndAttribute(MainNode, "div", "class", "slidesAndZip");
			HAP.HtmlNode SAZNode = SlidesAndZip.FirstOrDefault();

			string SlidesUrl = null;
			if (SAZNode != null) {
				foreach (var item in SAZNode.Descendants("a")) {
					if (item.InnerText.ToUpper() == "SLIDES") {
						SlidesUrl = item.Attributes["href"]?.Value;
						break;
					}
				}
			}

			return SlidesUrl;
		}

//---------------------------------------------------------------------------------------

		private static string GetCaptionsUrl(HAP.HtmlNode MainNode) {
			var CaptionsNode = FindTagAndAttribute(MainNode, "select", "name", "language").FirstOrDefault();
			if (CaptionsNode != null) {
				var Opts = CaptionsNode.Descendants("option");
				foreach (var opt in Opts) {
					string lang = opt.NextSibling.InnerText.Trim();
					if (lang == "English") {
						string val = opt.Attributes["value"].Value;
						return val.Replace("&amp;", "&");
					}
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		private static string GetCaptionsUrl2(HAP.HtmlNode MainNode) {
			var CaptionsNodes = MainNode.Descendants("h3");
			if (CaptionsNodes != null) {
				foreach (var CapNode in CaptionsNodes) {
					if (CapNode.InnerText.Trim() != "Download captions") {
						continue;
					}
					var Links = CapNode.NextSibling.NextSibling.Descendants("a");
					foreach (var Link in Links) {
						string lang = Link.InnerText.Trim();
						if (lang == "English") {
							string val = Link.Attributes["href"].Value;
							return val.Replace("&amp;", "&");
						}
					}
					Console.WriteLine("End of links");
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		private static IEnumerable<HAP.HtmlNode> FindTagAndAttribute(HAP.HtmlNode StartingNode, string TagName, string AttributeName, string AttributeValue) {
			var Descendents = StartingNode.Descendants(TagName);
			foreach (var des in Descendents) {
				foreach (var attr in des.Attributes) {
					if ((attr.Name == AttributeName) && (attr.Value == AttributeValue)) {
						if (des.InnerHtml.Contains("<a href=\"https://mybuild.microsoft.com")) {
							continue;		// Shouldn't be "mybuild" links in here
						}
						yield return des;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static string GetHighestResolutionVideoSourceUrl(Dictionary<string, string> opts) {
			string Url = null;
			int BestLevel = -1;
			foreach (var key in opts.Keys) {
				if ((key == "Low Quality MP4") && (BestLevel < 0)) {
					Url = opts[key];
					BestLevel = 0;
				} else if ((key == "Mid Quality MP4") && (BestLevel < 1)) {
					Url = opts[key];
					BestLevel = 1;
				} else if ((key == "High Quality MP4") && (BestLevel < 2)) {
					Url = opts[key];
					BestLevel = 2;
				} else {
					continue;           // Ignore. e.g. MP3
				}
			}
			return Url;
		}

//---------------------------------------------------------------------------------------

		private static Dictionary<string, string> GetOptions(IEnumerable<HAP.HtmlNode> Options) {
			var Dict = new Dictionary<string, string>();
			foreach (var opt in Options) {
				Dict[opt.NextSibling.InnerText.Trim()] = opt.Attributes["value"].Value;
			}
			return Dict;
		}

//---------------------------------------------------------------------------------------

		private static void ScrapeTagsLevelSessionTypeTrack(HAP.HtmlNode MainNode, Dictionary<string, List<string>> TagsLevelSessionType) {
			var DetailsNode = FindTagAndAttribute(MainNode, "div", "class", "details").FirstOrDefault();
			if (DetailsNode == null) {
				return;
			}
			GetMetaInfo(DetailsNode, "tags",  TagName,         TagsLevelSessionType);
			GetMetaInfo(DetailsNode, "level", LevelName,       TagsLevelSessionType);
			GetMetaInfo(DetailsNode, "type",  SessionTypeName, TagsLevelSessionType);
			GetMetaInfo(DetailsNode, "track", TrackName,       TagsLevelSessionType);
		}

//---------------------------------------------------------------------------------------

		private static void GetMetaInfo(HAP.HtmlNode DetailsNode, string ClassName, string MetaName, Dictionary<string, List<string>> TagsLevelSessionType) {
			var Values = new List<string>();
			var MetaNode = FindTagAndAttribute(DetailsNode, "div", "class", ClassName).FirstOrDefault();
			if (MetaNode != null) {
				var TagEntries = MetaNode.Descendants("a");
				foreach (var te in TagEntries) {
					Values.Add(CoreDownloadMsConferences.CleanTitle(te.InnerText));
				}
			}
			TagsLevelSessionType[MetaName] = Values;
		}

//---------------------------------------------------------------------------------------

		private static void dbgShowIEnumerable(IEnumerable<object> data) {
			foreach (var item in data) {
				Console.WriteLine(item.ToString());
			}
		}
	}
}
