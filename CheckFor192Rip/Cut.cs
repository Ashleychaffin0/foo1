using System;
using System.Diagnostics;

namespace CheckFor192Rip {
	// See https://devblogs.microsoft.com/visualstudio/customize-object-displays-in-the-visual-studio-debugger-your-way/
	[DebuggerDisplay("Title: {Title,nq}")]
	public class Cut {
		public string	Filename;
		public int		CutNumber;
		public string	Title;
		public int		Rate;
		public string[]	Genres;
		public TimeSpan Duration;

//---------------------------------------------------------------------------------------

		public Cut(TagLib.File cutInfo) {
			CutNumber = (int)cutInfo.Tag.Track;
			Title    = cutInfo.Tag.Title;
			Rate     = cutInfo.Properties.AudioBitrate;
			Genres    = cutInfo.Tag.Genres;
			Duration = cutInfo.Properties.Duration;
		}
	}
}