using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using HAP = HtmlAgilityPack;

namespace DownloadMsConferences {
	/// <summary>
	/// Given a URL, download it, then scrape it for the highest resolution video URL and
	/// the URL for the PowerPoint slides (if there is such a link).
	/// </summary>
	class ScrapeChannel9Page {

		public const string TagName         = "$Tags";
		public const string LevelName       = "$Level";
		public const string SessionTypeName = "$Session Type";

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Actually do the scraping
		/// </summary>
		/// <param name="Url"></param>
		/// <param name="CanUseWmv"></param>
		/// <returns></returns>
		public static async Task<ScrapedInfo> ScrapeAsync(string Url, bool CanUseWmv = true) {
			// Download the html
			var wc = new WebClient();
			string html = await wc.DownloadStringTaskAsync(new Uri(Url));

			// Parse it
			var doc = new HAP.HtmlDocument();
			doc.LoadHtml(html);         // Create the Document Object Model (DOM)
										// from the HTML
			html = null;                // Remove a bit of memory pressure. Don't need
										// this any more

			// Select all the <a> elements and get the links to the video files
			var LinkElements = doc.DocumentNode.Descendants("a");
			var Urls = new Links();
			foreach (var Link in LinkElements) {
				GetVideoAndSlidesLinks(Urls, CanUseWmv, Link);
			}

			var TagsLevelSessionType = new Dictionary<string, List<string>>();
			ScrapeTagsLevelSessionType(doc, TagsLevelSessionType);
			doc = null;                 // Don't need this now, either

			// Find the highest resolution video link of the user's preferred type
			string VideoSourceUrl = GetHighestResolutionVideoSourceUrl(Urls, CanUseWmv);

			return new ScrapedInfo {
				VideoLink  = VideoSourceUrl,
				SlidesLink = Urls.Slides,
				TagsEtAl   = TagsLevelSessionType
			};
		}

//---------------------------------------------------------------------------------------

		private static void ScrapeTagsLevelSessionType(HAP.HtmlDocument doc, Dictionary<string, List<string>> TagsLevelSessionType) {
			// Note: Some people thing that scraping is a pain in the neck. But I have
			//		 a lower opinion of it than that.
			// The format of things is roughly as follows:
#if false
< ul id = "entry-tags" >
		 < li >
			 < h3 > Tags:
            </ h3 >
			< ul >
				< li >< a href = "/Events/Build/2015?t=.net" >.NET </ a >,</ li >
				< li >< a href = "/Events/Build/2015?t=visual-studio" > Visual Studio </ a >,</ li >
				< li >< a href = "/Events/Build/2015?t=diagnostics" > Diagnostics </ a > </ li >
			 </ ul >
		 </ li >
 
		 < li >
			 < h3 > Level:</ h3 >
				< ul >
					< li >< a href = "/Events/Build/2015?l=300%20-%20Experienced" > 300 - Experienced </ a ></ li >
				 </ ul >
			 </ li >
			 < li >
				 < h3 > Session Type:</ h3 >
					< ul >
						< li >< a href = "/Events/Build/2015?y=Breakout" > Breakout </ a > </ li >
					 </ ul >
				 </ li >
			 </ ul >
#endif
			// So our approach is to
			//	a)	Look for a <ul> with an id of "entry-tags"
			var ul3 = doc.DocumentNode.Descendants("ul");
			foreach (var ul in ul3) {
				var idTag = ul.GetAttributeValue("id", "");
				if (idTag == "entry-tags") {
					foreach (var li in ul.ChildNodes) {
						if (li.Name == "li") {
							GetTagEtAl(li, TagsLevelSessionType);
							// break;
						}
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void GetTagEtAl(HAP.HtmlNode li, Dictionary<string, List<string>> tagsLevelSessionType) {
			var h3_s = li.Descendants("h3");
			foreach (var h3 in h3_s) {
				string txt = h3.InnerText.Trim();
				switch (txt) {
				case "Tags:":
					GetTagsEtAl(li, tagsLevelSessionType, TagName);
					break;
				case "Level:":
					GetTagsEtAl(li, tagsLevelSessionType, LevelName);
					break;
				case "Session Type:":
					GetTagsEtAl(li, tagsLevelSessionType, SessionTypeName);
					break;
				default:
					// Ignore anything else
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void GetTagsEtAl(HAP.HtmlNode li, Dictionary<string, List<string>> tagsLevelSessionType, string Name) {
			var items = new List<string>();
			foreach (var ul in li.Descendants("ul")) {
				foreach (var li_s in ul.Descendants("li")) {
					foreach (var a in li_s.Descendants("a")) {
						items.Add(a.InnerText);
					}
				}
			}
			tagsLevelSessionType[Name] = items;
		}

//---------------------------------------------------------------------------------------

		private static void dbgShowIEnumerable(IEnumerable<object> data) {
			foreach (var item in data) {
				Console.WriteLine(item.ToString());
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Look for links (<a>'s) with InnerText of "High Quality WMV", "High Quality MP4",
		/// "Mid Quality WMV", "Mid Quality MP4", and so on, and save them. Later, the
		/// GetHighestResolutionVideoSourceUrl() method will choose which of these is
		/// the hightest resolution one.
		/// </summary>
		/// <param name="CanUseWmv"></param>
		/// <param name="Link"></param>
		private static void GetVideoAndSlidesLinks(Links LinkUrls, bool CanUseWmv, HAP.HtmlNode Link) {
			// It would look like
			// <a href="http://video.ch9.ms/sessions/build/2015/2-791-LG.mp4">High Quality MP4</a>
			var href = Link.GetAttributeValue("href", null);
			switch (Link.InnerText) {
			case "Slides":
				LinkUrls.Slides = href;
				break;

			case "High Quality WMV":
				if (CanUseWmv) {
					LinkUrls.HighWmvUrl = href;
				}
				break;
			case "High Quality MP4":
				LinkUrls.HighMp4Url = href;
				break;
			case "Mid Quality MP4":
				LinkUrls.MedMp4Url = href;
				break;
			case "Low Quality MP4":
				LinkUrls.LowMp4Url = href;
				break;
			case "Mid Quality WMV":
				if (CanUseWmv) {
					LinkUrls.MedWmvUrl = href;
				}
				break;
			case "Low Quality WMV":
				if (CanUseWmv) {
					LinkUrls.LowWmvUrl = href;
				}
				break;
			default:
				// Ignore all other <a>'s
				break;
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Return the link to thehighest resolution video we can find, either,
		/// .wmv or .mp4, whichever the user has specified a preference for
		/// </summary>
		/// <returns></returns>
		private static string GetHighestResolutionVideoSourceUrl(Links LinkUrls, bool CanUseWmv) {
			if (CanUseWmv && (LinkUrls.HighWmvUrl != null)) {
				return LinkUrls.HighWmvUrl;
			} else if (LinkUrls.HighMp4Url != null) {
				return LinkUrls.HighMp4Url;
			}

			if (CanUseWmv && (LinkUrls.MedWmvUrl != null)) {
				return LinkUrls.MedWmvUrl;
			} else if (LinkUrls.MedMp4Url != null) {
				return LinkUrls.MedMp4Url;
			}

			if (CanUseWmv && (LinkUrls.LowWmvUrl != null)) {
				return LinkUrls.LowWmvUrl;
			} else if (LinkUrls.LowMp4Url != null) {
				return LinkUrls.LowMp4Url;
			}

			return null;
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		private class Links {
			// Most sessions are in high resolution .mp4 format. But a few aren't. So
			// check to see what formats are available, then download the highest
			// resolution one (wither .mp4 or .wmv. We use these fields below to hold
			// the video links we find.
			public string HighMp4Url = null;
			public string MedMp4Url  = null;
			public string LowMp4Url  = null;
			public string HighWmvUrl = null;
			public string MedWmvUrl  = null;
			public string LowWmvUrl  = null;

			// Many sessions have slides, but not all. This will have a link to the Slides
			// if there is one
			public string Slides = null;
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		/// <summary>
		/// A minor class, but which holds all the information we need from the web page
		/// </summary>
		public class ScrapedInfo {
			public string							VideoLink;
			public string							SlidesLink;
			public Dictionary<string, List<string>> TagsEtAl;
		}
	}
}
