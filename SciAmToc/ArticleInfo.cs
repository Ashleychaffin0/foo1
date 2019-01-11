// Keep this routine in sync between SciAmToc and MonitorClipboardChange

using System;
using System.Collections.Generic;

namespace SciAmToc {
	public class ArticleInfo {
		public int          PageNum;
		public string       Title;
		public string       Authors;
		public List<string> Description;

//---------------------------------------------------------------------------------------

		public ArticleInfo() {
			PageNum     = -1;
			Title       = "";
			Authors     = "";
			Description = new List<string>();
		}

//---------------------------------------------------------------------------------------

		public string Desc { get => string.Join(" ", Description); }

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return $"{Title} by {Authors} -- {Desc}";
		}
	}
}
