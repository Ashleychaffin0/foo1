using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

using Bartizan.Utils.CRC;

namespace FindDuplicateFilesGui {

	class FileEntry {
		public string	DirName;
		public string	FileName;		// Sans the path prefix (i.e. DirName)
		public uint		ShortCRC;		// The CRC of the beginning of the file. 
		public long		FileSize;		// The size of the file
										// TODO: Can't we find FileSize some other place?

//---------------------------------------------------------------------------------------

		public FileEntry(string PathName) {
			var DirName = Path.GetDirectoryName(PathName);
			var Filename = PathName.Substring(DirName.Length + 1);
			CommonCtor(DirName, Filename);
		}

//---------------------------------------------------------------------------------------
		
		public FileEntry(string DirName, string FileName) {
			CommonCtor(DirName, FileName);
		}

//---------------------------------------------------------------------------------------

		void CommonCtor(string DirName, string FileName) {
			// We may well have the same directory name in many, many of these class
			// instances. Save on storage by interning the name, so that they all refer
			// to the same string object.
			this.DirName  = string.Intern(DirName);
			this.FileName = FileName;
			FileSize = (new FileInfo(FileName)).Length;
			ShortCRC	  = 0;
		}

//---------------------------------------------------------------------------------------

		public void GetShortCRC(int nBytes) {
			string filename = Path.Combine(DirName, FileName);
			byte [] bytes = new byte[nBytes];
			var crc = new BartCRC();
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, nBytes)) {
				int n = fs.Read(bytes, 0, nBytes);	// n = # of bytes read (in case 
													// filesize < nBytes
				crc.AddData(bytes, n);
			}
			ShortCRC = crc.GetCRC();
		}
	}
	
	
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class FilesBySize {

		public ConcurrentDictionary<long, List<FileEntry>> DictBySize;

//---------------------------------------------------------------------------------------

		public FilesBySize() {
			// TODO: It looks like you have to specify the maximum number of entries
			//		 in the ConcurentDictionary when you construct it. If it fills up
			//		 and you try to add a new entry, it won't copy everything over (while
			//		 other threads are still trying to Add/Remove entries. IOW, the
			//		 TryAdd will throw an Exception.
			DictBySize = new ConcurrentDictionary<long, List<FileEntry>>();
		}

//---------------------------------------------------------------------------------------

		public void Add(long size, FileEntry fe) {
			DictBySize.TryAdd(size, new List<FileEntry>());	// NOP if <size> there
			// TODO: On this next line, I think we need a ConcurrentList, instead of
			//		 a mere List, to avoid race conditions. Sigh.
			DictBySize[size].Add(fe);
		}

//---------------------------------------------------------------------------------------

		public void CalcShortCRCs(int ShortCRCByteLength) {
			// int n = 0;
			int nSizes = DictBySize.Keys.Count;
			// toolStripProgressBar1.Maximum = nSizes;
			// toolStripProgressBar1.Visible = true;
			var SortedKeys = DictBySize.Keys.OrderBy(key => key);
			foreach (long key in SortedKeys) {
				var items = DictBySize[key];
				// toolStripStatusLabel1.Text = string.Format("Calculating short CRC {0} of {1} for filesize {2:N0} - files = {3}",
				//	++n, nSizes, key, items.Count);
				// Application.DoEvents();

				// We may not be able to get the CRC if, for example, the file is
				// opened exclusively. If so, set the corresponding Dictionary entry
				// to null. Note that this has ramifications. Everywhere we process
				// FileEntry's, we'll have to check for null, and skip the entry.
				// TODO: Put null checks elsewhere in the code
				FileEntry ent;
				for (int i = 0; i < items.Count; ++i) {
					ent = items[i];
					try {
						ent.GetShortCRC(ShortCRCByteLength);
					} catch {
						items[i] = null;
					}
				}
				// toolStripProgressBar1.Value = n;
			}
			// toolStripProgressBar1.Value = 0;		// Reset once done
		}
	}
	
	
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class FilesBySizeNormalDict {				// TODO:

		public Dictionary<long, List<FileEntry>> DictBySize;

//---------------------------------------------------------------------------------------

		public FilesBySizeNormalDict() {
			// TODO: It looks like you have to specify the maximum number of entries
			//		 in the ConcurentDictionary when you construct it. If it fills up
			//		 and you try to add a new entry, it won't copy everything over (while
			//		 other threads are still trying to Add/Remove entries. IOW, the
			//		 TryAdd will throw an Exception.
			DictBySize = new Dictionary<long, List<FileEntry>>(); 
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<long> Keys {
			get { return DictBySize.Keys; }
		}

//---------------------------------------------------------------------------------------

		public List<FileEntry> this[long ix] {
			get { return DictBySize[ix]; }
		}

//---------------------------------------------------------------------------------------

		public void Add(long size, FileEntry fe) {
			if (! DictBySize.ContainsKey(size)) {
				DictBySize.Add(size, new List<FileEntry>());
				return;
			}
			DictBySize[size].Add(fe);
		}

//---------------------------------------------------------------------------------------

		public void CalcShortCRCs(int ShortCRCByteLength) {
			// int n = 0;
			int nSizes = DictBySize.Keys.Count;
			// toolStripProgressBar1.Maximum = nSizes;
			// toolStripProgressBar1.Visible = true;
			var SortedKeys = DictBySize.Keys.OrderBy(key => key);
			// TODO: Run this in parallel
			foreach (long key in SortedKeys) {
				var items = DictBySize[key];
				// toolStripStatusLabel1.Text = string.Format("Calculating short CRC {0} of {1} for filesize {2:N0} - files = {3}",
				//	++n, nSizes, key, items.Count);
				// Application.DoEvents();

				// We may not be able to get the CRC if, for example, the file is
				// opened exclusively. If so, set the corresponding Dictionary entry
				// to null. Note that this has ramifications. Everywhere we process
				// FileEntry's, we'll have to check for null, and skip the entry.
				// TODO: Put null checks elsewhere in the code
				FileEntry ent;
				for (int i = 0; i < items.Count; ++i) {
					ent = items[i];
					try {
						ent.GetShortCRC(ShortCRCByteLength);
					} catch {
						items[i] = null;
					}
				}
				// toolStripProgressBar1.Value = n;
			}
			// toolStripProgressBar1.Value = 0;		// Reset once done
		}
		
//---------------------------------------------------------------------------------------

		internal void CalcShortCRCs_Parallel(FindDuplicateFilesGui form, int ShortCRCByteLength) {
			// int n = 0;
			int nSizes = DictBySize.Keys.Count;
			// toolStripProgressBar1.Maximum = nSizes;
			// toolStripProgressBar1.Visible = true;
			var SortedKeys = DictBySize.Keys.OrderBy(key => key).ToArray();
			
			// toolStripProgressBar1.Value = 0;		// Reset once done
			var qry = from key in SortedKeys.AsParallel()
					  let x = CalcCrc(key, ShortCRCByteLength)
					  select new {Key = key};
			foreach (var item in qry) {
				FindDuplicateFilesGui.ShowStatMsg(form, string.Format("Processing file size {0:N0}", item.Key));
			}
		}

//---------------------------------------------------------------------------------------

		private object CalcCrc(long key, int ShortCRCByteLength) {
			var items = DictBySize[key];
			// toolStripStatusLabel1.Text = string.Format("Calculating short CRC {0} of {1} for filesize {2:N0} - files = {3}",
			//	++n, nSizes, key, items.Count);
			// Application.DoEvents();

			// We may not be able to get the CRC if, for example, the file is
			// opened exclusively. If so, set the corresponding Dictionary entry
			// to null. Note that this has ramifications. Everywhere we process
			// FileEntry's, we'll have to check for null, and skip the entry.
			// TODO: Put null checks elsewhere in the code
			FileEntry ent;
			for (int i = 0; i < items.Count; ++i) {
				ent = items[i];
				try {
					ent.GetShortCRC(ShortCRCByteLength);
				} catch {
					items[i] = null;
				}
			}
			// toolStripProgressBar1.Value = n;
			return null;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class SizeBuckets {
		// TODO:
	}
	
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public static class DeleteSingletons {
		public static void Delete<T, V>(ConcurrentDictionary<T, List<V>> Dict) {
			// Delete all dictionary entries where we have only a single entry. However,
			// I'm wary of deleting entries in the dictionary while we're traversing it.
			// So we'll accumulate the keys to be deleted in a secondary List, then
			// use this to delete our dictionary entries at the end.
			if (Dict.Count == 1) {
				return;
			}
			var ToBeDeleted = new List<T>();
			foreach (T key in Dict.Keys) {
				List<V> items = Dict[key];
				if (items.Count == 1)
					ToBeDeleted.Add(key);
			}
			foreach (T key in ToBeDeleted) {
				List<V> val;
				Dict.TryRemove(key, out val);
			}
#if DBG_MSGS
			WriteLine("Removed {0:N0} singleton key(s) from Hashtable {1}. Size went from {2:N0} to {3:N0}.", 
				ToBeDeleted.Count, htname, ht.Count + ToBeDeleted.Count, ht.Count);
#endif
			return;
		}

//---------------------------------------------------------------------------------------

		// TODO: Get rid of this?
		public static void Delete<T, V>(Dictionary<T, List<V>> Dict) {
			// Delete all dictionary entries where we have only a single entry. However,
			// I'm wary of deleting entries in the dictionary while we're traversing it.
			// So we'll accumulate the keys to be deleted in a secondary List, then
			// use this to delete our dictionary entries at the end.
			if (Dict.Count == 1) {
				return;
			}
			var ToBeDeleted = new List<T>();
			foreach (T key in Dict.Keys) {
				List<V> items = Dict[key];
				if (items.Count == 1)
					ToBeDeleted.Add(key);
			}
			foreach (T key in ToBeDeleted) {
				Dict.Remove(key);
			}
#if DBG_MSGS
			WriteLine("Removed {0:N0} singleton key(s) from Hashtable {1}. Size went from {2:N0} to {3:N0}.", 
				ToBeDeleted.Count, htname, ht.Count + ToBeDeleted.Count, ht.Count);
#endif
			return;
		}
	}
}
