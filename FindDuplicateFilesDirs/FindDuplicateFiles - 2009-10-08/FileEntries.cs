using System;
using System.Collections.Generic;
using System.IO;

namespace FindDuplicateFiles {
	class FileEntries {
		public Dictionary<string, uint> Entries;

//---------------------------------------------------------------------------------------

		public FileEntries(long MinSize, long MaxSize, int PreviewSize, List<string> Folders) {
			var crc = new Bartizan.Utils.CRC.BartCRC();
			Entries = new Dictionary<string, uint>();               // Filename, crc
			foreach (var folder in Folders) {
				foreach (var path in Directory.EnumerateFileSystemEntries(folder, "*", SearchOption.AllDirectories)) {
					var entry = new FileEntry(path, MinSize, MaxSize, PreviewSize);
				}
			}
		}
	}
}
