// Copyright (c) 2010 by Larry Smith

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
#if false		// TODO: Caller's responsibility
			FileInfo fi = null;
			try {
				fi = new FileInfo(filename);
			} catch {
				// We may not be able to look at this file. It may be opened exclusively,
				// we may not have security access to it, etc. So just silently ignore it.
				// TODO: This looks like a bug in the runtime. We have a file that is
				//		 exactly 260 characters long, but FileInfo complains that it must
				//		 must be < 260 chars.
				yield break;
			}
			// I don't quite understand this, but I've seen the above FileInfo work with
			// no errors, but when we try to get the file size, it reports File Not
			// Found?!? Maybe some security thing? Oh well, ignore it if we run across it
			if (!fi.Exists)
				return;
			long fsize = fi.Length;
			if ((fsize < Parms.MinSize) || (fsize > Parms.MaxSize))
				return;
			// Add to dictionary, creating entry if necessary
			if (!DictSizes.ContainsKey(fsize))
				DictSizes[fsize] = new List<FileEntry>();
			DictSizes[fsize].Add(new FileEntry(DirName, filename));
#endif
		}

//---------------------------------------------------------------------------------------

		private static string GlueMasks(List<string> Masks) {
			var sb = new StringBuilder();
			bool bAdd_OR = false;
			foreach (var mask in Masks) {
				string mask2 = Regex.Escape(mask);
				if (bAdd_OR) {
					sb.Append("|");
				}
				bAdd_OR = true;
				string re = mask2.Replace(@"\*", @".*");	// Undo Regex.Escape in this case
				sb.Append("(");
				sb.Append(re);
				sb.Append(")");
			}
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
}
