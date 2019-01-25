using System.IO;

// TODO: Add Next / Previous buttons for Find support?
// TODO: Show just selected words
// TODO: Stats panel, with (e.g.) # of words up to 10%, 20%, etc
// TODO: Prompt for folder, with recursion checkbox. Do all *.txt files in folder(s)
// TODO: After analyses, select which novel to see

namespace AnalyzeNovel {
	// This class is useful since a CheckedListBox Item does not have a <tag> field.
	// Maybe fixed in .Net Core?
	internal class NovelName {
		public string FullFilename;

//---------------------------------------------------------------------------------------

		public NovelName(string FullFilename) {
			this.FullFilename = FullFilename;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Path.GetFileNameWithoutExtension(FullFilename);
		}
	}
}
