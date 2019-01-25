#define XPATH				// For testing

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

/*
	NOTE: My original code used the HtmlAgilityPack to manually follow the DOM
		  of the web page to find things like the lecture Title, Speakers, etc.
		  Then, never having used XPath before, and knowing that HAP supported it,
		  I thought I'd give it a chance (and learn a bit about XPath in the
		  process). And at the highest level, it worked fine, finding the starting
		  point of each lecture on a page. 
		  
		  But when I tried to use XPath for other purposes, I couldn't get it to work
		  at all. In particular, if I'd found a <div> I was looking for and had
		  an HtmlNode for it, I expected that I could do a simple XPath to drill down
		  into the contents of that node. But it seems that you couldn't do that.
		  Was I, in my unfamiliarity with XPath, doing something wrong?
		  Quite possibly. Or maybe HAP supports only XPath from the root???
		  
		  The only way I could search within a node was to create a new
		  HtmlDocument, load the InnerHtml from the current node, then XPath
		  on that. Which kinda sucks!

		  So for scraping a web page at the highest level (finding each lecture),
		  I still use XPath. It works and takes far fewer lines of code to find
		  what I want. But for drilling down, I *don't* use XPath.

		  Maybe some day I'll figure out how to do this right (if the problem
		  isn't in HAP). Until then...

	Additional note: Check out the last entry (with "descendant" and "and") at
		https://stackoverflow.com/questions/11925311/html-agility-pack-xpath-query-with-logical-and
*/


namespace GetPiPublicLectures_4 {
	// Next line just for fun, and to try to remember to do this occasionally.
	// See https://docs.microsoft.com/en-us/visualstudio/debugger/using-the-debuggerdisplay-attribute
	// It also mentions DebuggerTypeProxy (whatever *that* is)
	// Also see http://dontcodetired.com/blog/post/Customising-the-Appearance-of-Debug-Information-in-Visual-Studio-with-the-DebuggerDisplay-Attribute
	// Also see http://dontcodetired.com/blog/post/Customizing-C-Object-Member-Display-During-Debugging
	[DebuggerDisplay("MaxPage = {MaxPage}")]
	class ScrapePiPage {
		public int				MaxPageNum;

		GetPiPublicLectures_4	Main;			// For accessing UI

		public List<Lecture>	PageLectures;   // Info for all lectures on this page

//---------------------------------------------------------------------------------------

		public ScrapePiPage() {
			Main         = GetPiPublicLectures_4.Main;
			PageLectures = new List<Lecture>();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Get the specified page and scrape it to find all relevant (e.g. .mp4 but not
		/// .mp3) lecture files. Note that checking whether the file already exists and
		/// thus not need downloading is done somewhere else.
		/// </summary>
		/// <param name="PageNum">The page number to scrape</param>
		/// <param name="MaxPage">Support "... Page 3 of 12"</param>
		/// <returns>Task suitable for awaiting</returns>
		public async Task<ScrapePiPage> GetPiPage(int PageNum, int MaxPage) {
			Main.Msg($"About to download page {PageNum + 1} of {MaxPage + 1}");
			string Url  = $"http://www.perimeterinstitute.ca/video-library/collection/perimeter-public-lectures?page={PageNum}";
			var wc      = new WebClient();
			string html = await wc.DownloadStringTaskAsync(new Uri(Url));
			Main.Msg($"     Analyzing page {PageNum + 1} of {MaxPage + 1}");
			var doc = new HtmlDocument();
			doc.LoadHtml(html);
			MaxPageNum = GetMaxPageNum(doc);        // Only really needed for 1st page

#if XPATH
			try {
				string xpath = "//div[@class='pi_default_content']";
				var DivDefaultContents = doc.DocumentNode.SelectNodes(xpath);
				foreach (HtmlNode divDefCon in DivDefaultContents) {
					string Abstract = GetLectureAbstract(divDefCon);
					string Title    = GetLectureTitle(divDefCon);
					DateTime Date   = GetLectureDate(divDefCon);
					string Speaker  = GetLectureSpeaker(divDefCon);

					// A given lecture, in general, will have multiple file types
					// associated with it, such as .mp4, .mp3, .pdf, etc. Create
					// a new Lecture for each type the user selects.
					foreach (var item in GetMediaLinks(divDefCon)) {
						if (item.Link == null) {
							break;
						}
						var lect = new Lecture(Title, Speaker, Date, item.Link, Abstract, item.MediaType);
						Console.WriteLine($"{Title} -- {item.MediaType} -- {item.Link}");
						if (lect.FileSizeOnDisk == -1) {
							lock (PageLectures) {
								PageLectures.Add(lect);
							}
						}
					}
				}
			} catch (Exception ex) {
				Main.Msg($"***** Error in ScrapePiPage({PageNum} -- {ex.Message}");
			}
			return this;
		}
#else
			var Divs = doc.DocumentNode.Descendants("div");
			foreach (var div in Divs) {
				var cls = div.GetAttributeValue("class", null);
				if ((cls == null) || (div.FirstChild == null)) {
					continue;
				}
				switch (cls) {
				case "pi_default_content":	// <div class="pi_default_content" ...>
					string Title    = GetLectureTitle(div);
					DateTime Date   = GetLectureDate(div);
					string Speaker  = GetLectureSpeaker(div);
					string Abstract = GetLectureAbstract(div);

					// A given session, in general, will have multiple file types
					// associated with it, such as .mp4, .mp3, .pdf, etc. Create
					// a new Lecture for each type the user selects.
					foreach (var item in GetMediaTypes(div)) {
						var lect = new Lecture(Title, Speaker, Date, item.Link, Abstract, item.MediaType);
						if (lect.FileSizeOnDisk == -1) {
							lock (PageLectures) {
								PageLectures.Add(lect);
							}
						}
					}
					break;
				default:
					break;
				}
			}
			return this;
		}
#endif

//---------------------------------------------------------------------------------------

		// Each page has a "Last" link that has the last (max) page number
		private int GetMaxPageNum(HtmlDocument doc) {
			string reQuery = @"video-library/collection/perimeter-public-lectures\?page=(?<MaxPage>\d+)";
			var re         = new Regex(reQuery);
			string txt     = doc.DocumentNode.InnerHtml;
			var matches    = re.Matches(txt);
			int MaxPage    = 1;
			foreach (Match match in matches) {
				int pageNum = Convert.ToInt32(match.Groups["MaxPage"].Value);
				MaxPage = Math.Max(MaxPage, pageNum);
			}
			return MaxPage;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A given lecture, in general, has several file formats associated with it.
		/// The user has specified in the UI that s/he wants certain formats. This 
		/// routine will return links for all requested formats (if they exist).
		/// </summary>
		/// <remarks>
		/// Some lectures contain videos in Windows Media Video (.wmv) format. But any
		/// such are also available as .mp4 files. To keep things simple, we ignore
		/// .wmv files. Also, there's exactly one lecture with a .flc (Flash) video.
		/// For the same reason we don't support this.
		/// </remarks>
		/// <param name="node"></param>
		/// <returns></returns>
		private IEnumerable<(string Link, string MediaType)> GetMediaLinks(HtmlNode node) {
#if XPATH
			string xpath  = "descendant::div[@class='presentation_links']/descendant::a";
			var PresLinks = node.SelectNodes(xpath);
			foreach (HtmlNode A_node in PresLinks) {
				string TheLink   = A_node.GetAttributeValue("href", null);
				string MediaType = A_node.InnerText;
				// TODO: Make the switch its own method
				switch (MediaType) {
					case "MP4 Medium Res":
						if (Main.ChkMp4.Checked) {
							int ix          = TheLink.IndexOf("?id=");
							string PirsaNum = TheLink.Substring(ix + 4);
							string Link2    = $"http://streamer2.perimeterinstitute.ca/mp4-med/{PirsaNum}.mp4";
							yield return (Link2, ".mp4");
						}
						break;
						case "MP4 Low Res":
							// http://streamer2.perimeterinstitute.ca/mp4-low/12120004.mp4
							if (Main.ChkLoResMp4.Checked)
								yield return (TheLink, ".LoRes.mp4");
							break;
						case "MP3":
							// http://streamer2.perimeterinstitute.ca/mp3/18050000.mp3
							if (Main.ChkMp3.Checked)
								yield return (TheLink, ".mp3");
							break;
						case "PDF":
							// http://pirsa.org/pdf/loadpdf.php?pirsa_number=18050000
							if (Main.ChkPdf.Checked)
								yield return (TheLink, ".pdf");
							break;
						default:
							break;
				}
			}
#else
			var avs = div.Descendants("div");
			string TheLink = null;
			foreach (var item in avs) {
				var av = item.GetAttributeValue("class", null);
				if (av == "presentation_links") {
					var links = item.Descendants("a");
					foreach (var link in links) {
						TheLink = link.GetAttributeValue("href", null);
						switch (link.InnerText) {
						case "MP4 Medium Res":
							// http://pirsa.org/index.php?p=media&amp;url=http://pirsa.org/displayFlash.php?id=18050000
							// http://streamer2.perimeterinstitute.ca/mp4-med/18050000.mp4?__hstc=266947857.8235c08319e44e86980e8a12e8c6d2f6.1509397783608.1529441747696.1529453859475.3&__hssc=266947857.6.1529453859475&__hsfp=3339756574
							// http://pirsa.org/18050000
							if (Main.ChkMp4.Checked) {
								int ix          = TheLink.IndexOf("?id=");
								string PirsaNum = TheLink.Substring(ix + 4);
								string Link2    = $"http://streamer2.perimeterinstitute.ca/mp4-med/{PirsaNum}.mp4";
								yield return (Link2, ".mp4");
							}
							break;
						case "MP4 Low Res":
							// http://streamer2.perimeterinstitute.ca/mp4-low/12120004.mp4
							if (Main.ChkLoResMp4.Checked)
								yield return (TheLink, ".LoRes.mp4");
							break;
#if false   // TODO: I thought that every .wmv file also has a .mp4 file so I wasn't going to support it.
			// TODO: Then I found out that http://www.perimeterinstitute.ca/videos/welcome-edge-universe
			// TODO: was available only in .flv and .wmv. Maybe I'll have to come back to this later. Sigh.
						case "Windows Presentation":
							// Both these are for .wmv. Ignore "Windows Video File",
							// assuming that where there's one, there's the other
							// case "Windows Video File":
// TODO: Doesn't work
							// http://pirsa.org/index.php?p=media&amp;url=http://streamer.perimeterinstitute.ca/wmv/57c0ce15-db2d-4a9f-8769-8717f8e92931.wmv&amp;pirsa=07030020&amp;type=Windows Presentation
							// if (ChkWmv.Checked) yield return (TheLink, ".wmv");
							break;
#endif
#if false   // There's only one Flash file (as of June 2018). They can download it manually.
			// Its Welcome to the Edge of the Universe
			// at http://www.perimeterinstitute.ca/video-library/collection/perimeter-public-lectures?page=11
						case "Flash Presentation":
							// http://pirsa.org/index.php?p=media&url=http://streamer.perimeterinstitute.ca/Flash/a8d40a1c-79c3-4487-915a-319484ed5c9a/index.html&pirsa=04110042&type=Flash%20Presentation
							// if (ChkFlv.Checked) yield return (TheLink, ".flv");
							break;
#endif
						case "MP3":
							// http://streamer2.perimeterinstitute.ca/mp3/18050000.mp3
							if (Main.ChkMp3.Checked)
								yield return (TheLink, ".mp3");
							break;
						case "PDF":
							// http://pirsa.org/pdf/loadpdf.php?pirsa_number=18050000
							if (Main.ChkPdf.Checked)
								yield return (TheLink, ".pdf");
							break;
						default:
							break;
						}
					}
				}
			}
#endif           // #if XPATH
		}

//---------------------------------------------------------------------------------------

		private string GetLectureTitle(HtmlAgilityPack.HtmlNode node) {
#if XPATH
			string xpath = "descendant::h2";
			var title    = node.SelectSingleNode(xpath);
			return CleanText(title.InnerText);
#else
			var h2 = node.Descendants("h2").First();
			return Clean(h2?.InnerText);
#endif
		}

//---------------------------------------------------------------------------------------

		private static DateTime GetLectureDate(HtmlAgilityPack.HtmlNode node) {
#if XPATH
			string xpath = "descendant::span[@content]";
			var when     = node.SelectSingleNode(xpath);
			return DateTime.Parse(when.InnerText);
#else
			var whenspan = node.Descendants("span").First();
			string when  = whenspan.GetAttributeValue("content", null);
			return DateTime.Parse(when);
#endif
		}

//---------------------------------------------------------------------------------------

		private string GetLectureSpeaker(HtmlAgilityPack.HtmlNode node) {
#if XPATH
			var xpath = "descendant::div[@class='field field-name-field-speakers-ref field-type-entityreference field-label-inline clearfix']/descendant::div[starts-with(@class,'field-item ')]";
			var spkrs = node.SelectNodes(xpath);
			if (spkrs == null)
				return "";			// In some cases, there is no Speaker specified
			var Speakers = new List<string>();
			foreach (var spkr in spkrs) {
				Speakers.Add(CleanText(spkr.InnerText));
			}
			return string.Join(", ", Speakers);
#else
			var spkr = node.Descendants("div");
			string Speaker = "";
			foreach (var item in spkr) {
				var spkrClass = item.GetAttributeValue("class", null);
				if (spkrClass == "field field-name-field-speakers-ref field-type-entityreference field-label-inline clearfix") {
					var peopleDiv = item.Descendants("div");
					var Speakers = new List<string>();
					foreach (var personDiv in peopleDiv) {
						var personClass = personDiv.GetAttributeValue("class", null);
						if (personClass.StartsWith("field-item ")) {
							Speakers.Add(Clean(personDiv.InnerText));
						}
					}
					Speaker = string.Join(", ", Speakers);
					break;
				}
			}
			return Speaker;
#endif
		}

//---------------------------------------------------------------------------------------

		private string GetLectureAbstract(HtmlAgilityPack.HtmlNode node) {
			// Search for lecture description
#if XPATH
			string xpath = "descendant::div[@class='pirsa_video_abstract_outer']";
			var abs      = node.SelectSingleNode(xpath);
			if (abs == null)	// Some lectures have no abstracts	
				return "";
			return ScrapePiPage.CleanText(abs.InnerText);
#else
			string Abstract = "";
			var abs = node.Descendants("div");
			foreach (var item in abs) {
				var absClass = item.GetAttributeValue("class", null);
				if (absClass == "pirsa_video_abstract_outer") {
					Abstract = CleanText(item.InnerText);
					break;
				}
			}
			return Abstract;
#endif
		}

//---------------------------------------------------------------------------------------

		private static string BasicClean(string str) {
			str = str.Replace("&nbsp;", " ")
				.Replace('\n', ' ');
			str = System.Web.HttpUtility.HtmlDecode(str);      // &#039; => '
			return str;
		}

//---------------------------------------------------------------------------------------

		public static string CleanText(string abs) {
			abs = BasicClean(abs);
			// Clean up non-ASCII characters that I've seen
			abs = abs.Replace("\r\n", " ")
				.Replace("\n", " ")
				.Replace("’", "'")
				.Replace("\xA0", " ")
				.Replace("\xFFFD", " ")
				.Replace("â€™", "'")
				.Replace("–", "-")
				.Replace("“", "\"")
				.Replace("”", "\"")
				;
			return abs;
		}

//---------------------------------------------------------------------------------------

		public static string CleanFilename(string str) {
			// Handle some invalid filename chars specially.
			str = str.Replace(":", " --")
				.Replace("?", "$Q$")
				.Replace("|", "!")
				.Replace("\"", "'")
				.Replace("/", "$")
				;
			// Handle the rest generically
			foreach (char c in Path.GetInvalidFileNameChars()) {
				str = str.Replace(c, '.');
			}
			return str;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Start the downloads for all files (e.g. .mp4 and .pdf) on this page
		/// </summary>
		public void DoAllDownloads() {
			Main.AllLectures.AddRange(PageLectures);
			foreach (var lect in PageLectures) {
				if (Main.ChkOnlySaveLinks.Checked) {
					WriteToSavedLinksFile(lect);
				} else {
#pragma warning disable CS4014			// Wants an await
					lect.DownloadLectureAsync();
#pragma warning restore CS4014
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void WriteToSavedLinksFile(Lecture lect) {
			var wtr = GetSaveLinksWriter();
			wtr.WriteLine($"<br><a href=\"{lect.Url}\"><b>{lect.Title}{lect.MediaType}</b></a>");
			wtr.WriteLine($"<br>{lect.Abstract}");
			wtr.Flush();
		}

//---------------------------------------------------------------------------------------

		private StreamWriter GetSaveLinksWriter() {
			if (Main.wtr == null) {
				var MyDocs        = Environment.SpecialFolder.MyDocuments;
				string foldername = Environment.GetFolderPath(MyDocs);
				string filename   = Path.Combine(foldername, "PI Lecture Index.html");
				Main.wtr          = new StreamWriter(filename);
				Main.wtr.WriteLine(@"<html>
<head>
<title>PI Lecture Index</title>
</head>
<body>
<h1>Perimeter Institute Public Lecture Index</h1>
");
			}
			return Main.wtr;
		}
	}
}
