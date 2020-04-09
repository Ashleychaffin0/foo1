// Copyright (c) 2010 by Larry Smith

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LrsFileUtils {
	class FilesFinder {

		Regex	reDirNamesExcluded,
				reFileMasksIncluded,
				reFileMasksExcluded;

//---------------------------------------------------------------------------------------

		public FilesFinder(
				List<string>	DirNamesToExclude,
				List<string>	FilenameMasksToInclude,
				List<string>	FilenameMasksToExclude) {
			SetRegexes(DirNamesToExclude,
				FilenameMasksToInclude, FilenameMasksToExclude);
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<string> FindFiles(List<string> Paths) {
			foreach (string PathName in Paths) {
				if (reDirNamesExcluded.IsMatch(PathName))
					continue;
				foreach (string filename in ProcessPathName(PathName)) {
					yield return filename;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<string> ProcessPathName(string PathName) {
#if false		// TODO: Put into callback
			toolStripStatusLabel1.Text = "Processing directory " + DirName;
			Application.DoEvents();
#endif
			IEnumerable<string> Filenames = null;
			try {
				Filenames = Directory.EnumerateFiles(PathName);
			} catch {
				// For security reasons (and maybe others), we may not be able to see the
				// files in this directory. If we can't, then silently ignore this dir.
				// Note: We'll also get here if the PathName references a non-existant
				//		 directory (e.g. LRS instead of C:\LRS).
				// TODO: Should we throw an Exception? Or invoke a callback?
				//		 Or something?
				yield break;
			}
			foreach (string filename in Filenames) {
				if (!reFileMasksIncluded.IsMatch(filename))
					continue;
				if (reFileMasksExcluded.IsMatch(filename))
					continue;
				yield return filename;
			}

			IEnumerable<string> DirNames = null;
			try {
				DirNames = Directory.EnumerateDirectories(PathName);
			} catch {
				// Security may also bite us here. Or maybe the above operation succeeds,
				// but this one may fail. Or the other way around. In either case, we'll
				// check for problems both here and above.
			}
			foreach (string dirname in DirNames) {
				foreach (var item in ProcessPathName(dirname)) {
					yield return item;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<string> ProcessFile(string PathName, string filename) {
			// toolStripStatusLabel1.Text = "Processing file " + filename;
			string FilenameOnly = filename.Substring(PathName.Length + 1);
			if (!reFileMasksIncluded.IsMatch(FilenameOnly))
				yield break;
			yield return filename;
		}

//---------------------------------------------------------------------------------------

		private static string GlueMasks(List<string> Masks) {
			var sb = new StringBuilder("^");			// TODO:
			bool bAdd_OR = false;
			foreach (var mask in Masks) {
				string mask2 = Regex.Escape(mask);
				if (bAdd_OR) {
					sb.Append("|");
				}
				bAdd_OR = true;
				string re = mask2.Replace(@"\*", @".*");	// Undo Regex.Escape in this case
				// TODO: Next line isn't right, but see what performance improvements it makes
				// string re = mask2.Replace(@"\*", @"[^.]*");	// Undo Regex.Escape in this case
				sb.Append("(").Append(re).Append(")");
			}
			sb.Append("$");					// TODO: End of string
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private void SetRegexes(
					List<string> DirNamesToExclude,
					List<string> FilenameMasksToInclude,
					List<string> FilenameMasksToExclude) {

			string pattern = GlueMasks(DirNamesToExclude);
			reDirNamesExcluded = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			pattern = GlueMasks(FilenameMasksToInclude);
			reFileMasksIncluded = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			pattern = GlueMasks(FilenameMasksToExclude);
			reFileMasksExcluded = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class DirsFinder {
		// TODO: This class may not survive.
		// TODO: If it does, add regex filtering on the dirnames.

		public static IEnumerable<string> FindDirs(List<string> Paths) {
			// May have problems with security settings (Paths that can't be enumerated)

			foreach (string PathName in Paths) {

				foreach (var item in Directory.EnumerateDirectories(PathName, "*", SearchOption.AllDirectories)) {
					yield return item;
				}
			}
		}
	}

}
