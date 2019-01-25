namespace IndexMyBookmarks {
	class SearchResult {
		public string Title { get; private set; }
		public string Url	{ get; private set; }

//---------------------------------------------------------------------------------------

		public SearchResult(string title, string url) {
			Title = title;
			Url   = url;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => Title;
	}
}
