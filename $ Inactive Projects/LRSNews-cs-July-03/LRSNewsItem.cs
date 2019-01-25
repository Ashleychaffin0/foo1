using System;

using SHDocVw;
using mshtml;

namespace LRSNews_cs_July_03 {
	/// <summary>
	/// Summary description for LRSNewsItem.
	/// </summary>
	public class LRSNewsItem {
		AxSHDocVw.AxWebBrowser	_web;

		public AxSHDocVw.AxWebBrowser web {
		get { return _web; }
	}

//---------------------------------------------------------------------------------------

		public HTMLDocumentClass	doc;
		public string				URL;
		public string				Title;

//---------------------------------------------------------------------------------------

		public LRSNewsItem(AxSHDocVw.AxWebBrowser web) {
			this._web = web;
		}

//---------------------------------------------------------------------------------------

		public void Reload() {
			URL = web.LocationURL;
			doc = (HTMLDocumentClass)web.Document;
			Title = doc.title;
		}
	}
}
