using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsChannel9 {
	class Channel9Video {
		protected string			_Title;
		protected string			_Description;
		protected HashSet<string>	_Keywords;

		public int		PageNumber	{ get; set; }
		public string	TimeCaption { get; set; }
		public string	Link		{ get; set; }				// URL to image
		public DateTime ArticleDate { get; set; }

//---

		public string	Title {
			get { return _Title; }
			set {
				_Title = value;
				ScanForKeywords(_Title);
			}
		}

//---

		public string	Description {
			get { return _Description; }
			set {
				_Description = value;
				ScanForKeywords(_Description);
			}
		}

//---

		public string KeywordsString {
			get { 
				// Change any empty topics to "&nbsp;"
				if (_Keywords.Count == 0) {
					return "&nbsp;";
				}
				string[] keys = _Keywords.ToArray();
				Array.Sort(keys);
				string List = string.Join(", ", keys);
				return List;
			}
		}

//---

		public string[] KeywordArray {
			get { 
				string[] keys = _Keywords.ToArray();
				Array.Sort(keys);
				return keys;
			}
		}

//---------------------------------------------------------------------------------------

		public Channel9Video(int PageNumber) {
			this.PageNumber = PageNumber;
			TimeCaption     = "&nbsp;";
			Title           = "";
			Description     = "";
			Link            = "";
			ArticleDate	    = default(DateTime);

			_Keywords = new HashSet<string>();
		}

//---------------------------------------------------------------------------------------

		public void ScanForKeywords(string Text) {
			if (Text == null) {		// We've seen null Descriptions
				return;
			}
			List<string> keys = Keywords.ScanForKeywords(Text);
			foreach (string key in keys) {
				_Keywords.Add(key);
			}
		}
	}
}
