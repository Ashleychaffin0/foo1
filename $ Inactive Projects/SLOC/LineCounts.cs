namespace LRS {
	class LineCounts {
		public int nLines;
		public int nBlankLines;
		public int nJustClosingBrace;
		public int nCommentLines;

//---------------------------------------------------------------------------------------

		public LineCounts() {
			// Let everything default to 0
		}

//---------------------------------------------------------------------------------------

		public LineCounts(int nLines, int nBlankLines, int nJustClosingBrace, int nCommentLines) {
			this.nLines            = nLines;
			this.nBlankLines       = nBlankLines;
			this.nJustClosingBrace = nJustClosingBrace;
			this.nCommentLines     = nCommentLines;
		}

//---------------------------------------------------------------------------------------

		public static LineCounts operator +(LineCounts left, LineCounts right) {
			return new LineCounts(
				left.nLines				+ right.nLines, 
				left.nBlankLines		+ right.nBlankLines,
				left.nJustClosingBrace	+ right.nJustClosingBrace,
				left.nCommentLines		+ right.nCommentLines);
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return string.Format("{0,5}L {1,5}B {2,5}}} {3,5}// {4,5}T", nLines, nBlankLines,
				nJustClosingBrace, nCommentLines, nLines - nBlankLines - nJustClosingBrace - nCommentLines);
		}
	}
}
