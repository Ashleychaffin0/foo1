#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace FindDuplicateFiles {
	static class FileEntries {

//---------------------------------------------------------------------------------------

		public static List<FileEntry> GetEntries(long MinSize, long maxSize, int previewSize, List<string> Folders) {
			FileEntry.SetCommonParameters(MinSize, maxSize, previewSize);
			List<FileEntry> Entries = new List<FileEntry>();
			foreach (var folder in Folders) {
				foreach (var path in Directory.EnumerateFileSystemEntries(folder, "*")) {
					// foreach (var path in Directory.EnumerateFileSystemEntries(folder, "*", SearchOption.AllDirectories)) {
						var entry = new FileEntry(path);
					if (entry.FileSize >= 0) {
						Entries.Add(entry);
					}
				}
			}
			return Entries;
		}
	}
}
