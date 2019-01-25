using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsChannel9 {
	class C9Page {

//---------------------------------------------------------------------------------------

		public static void ProcessDiv(HtmlElement DivBase, ref Channel9Video Video, List<Channel9Video> Videos, int PageNoStart) {
			string ClassName = DivBase.GetAttribute("className");
			switch (ClassName) {
			case "entry-image":
				if (Video != null) {
					Videos.Add(Video);
				}
				Video = new Channel9Video(PageNoStart);
				var A_Tags = DivBase.GetElementsByTagName("A");
				Video.Link = A_Tags[0].GetAttribute("href");
				// Console.WriteLine("\n\n");
				break;
			case "entry-caption":
				Video.TimeCaption = Clean(DivBase.InnerText);
				// Console.WriteLine("Caption - {0}", DivBase.InnerText);
				break;
			case "entry-meta":
				Video.Title = Clean(DivBase.Children[0].InnerText);
				// Console.WriteLine(DivBase.Children[0].InnerText);
				break;
			case "description":
				Video.Description = Clean(DivBase.InnerText);
				// Console.WriteLine("Descrption - {0}", DivBase.InnerText);
				break;
			case "data":
				for (int i = 0; i < DivBase.Children.Count; i++) {
					var kid = DivBase.Children[i];
					string SpanClass = kid.GetAttribute("className");
					if ((kid.TagName == "SPAN") && (SpanClass == "date")) {
						try {
							Video.ArticleDate = GetArticleDate(kid.InnerText);
						} catch (Exception ex) {
							break;
						}
					}
				}
				break;
			default:
				return;
			}
#if false
			var attrs = div.attributes;
			foreach (var attr in attrs) {
				Console.WriteLine("Attr {0}", attr.Name);
			}
#endif
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a date in the form of a) a date, b) "n day[s] ago", c) "n minute[s] ago"
		/// or d) n hour[s] ago, 
		/// return the date of the article. I'm satisfied if this is only an 
		/// approximation. All I really care about is the date. This is probably better
		/// than keeping the accuracy to down to the h:m:s level, since their concept of
		/// "37 minutes" ago, might not quite be the same as mine (we might be on the
		/// cusp of a change from one minute (and possibly one hour) to the next. So just
		/// the date, please.
		/// </summary>
		/// <param name="date"></param>
		/// <returns>Midnight of the resultant date</returns>
		private static DateTime GetArticleDate(string date) {
			DateTime ArticleDate = DateTime.Now;
			TimeSpan DateOffset = new TimeSpan(0);
			// Passed either a date, or "n days ago" (n < 7), or "n minutes ago".
			if (date.Contains("minutes ago") || date.Contains("minute ago")) {
				int MinutesAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(0, -MinutesAgo, 0);
			} else if (date.Contains("days ago") || date.Contains("day ago")) {
				int DaysAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(-DaysAgo, 0, 0, 0);
			} else if (date.Contains("hours ago") || date.Contains("hour ago")) {
				int HoursAgo = GetDateOffset(date);
				DateOffset = new TimeSpan(-HoursAgo, 0, 0);
			} else {
				ArticleDate = DateTime.Parse(date.Replace("at ", ""));
			}
			ArticleDate += DateOffset;
			// Now round down to a pure date (no h, m, s -- midnight)
			var ad = ArticleDate;		// Short name for next statement
			ArticleDate = new DateTime(ad.Year, ad.Month, ad.Day);
			return ArticleDate;
		}

//---------------------------------------------------------------------------------------

		private static int GetDateOffset(string date) {
			return int.Parse(date.Split(' ')[0]);
		}

//---------------------------------------------------------------------------------------

		private static string Clean(string text) {
			// Some text uses funny Unicode open/close quotes and other characters, which 
			// in turn shows up strangely in IE. Get rid of them
			text = text.Replace("“", "\"").Replace("”", "\"").Replace("’", "'");
			text = text.Replace("…", "...");
			return text;
		}
	}
}
