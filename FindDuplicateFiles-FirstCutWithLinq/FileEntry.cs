// Copyright (c) 2005-2019 by Larry Smith

// TODO: Jan/Feb 2009
//	*	Allow scanning multiple directories/drives. See point2 in next section.
//	*	Support exclusion dirs (e.g. C:\Windows, Recycle bin, maybe Program Files),
//		and a fortiori, their subdirs

// TODO: 
//	*	Need a way to search more than one dir, especially whole drives. See
//		Directory.GetLogicalDrives(). Note that it would be nice to distinguish local 
//		drives from network drives from	CDs from floppy	drives, etc. Also want to 
//		support \\machinename\dir.
//	*	Need file type support. e.g. *.cs;*.jpg;*.jpeg, etc
//	*	Use FileInfo and DirectoryInfo, rather than what we have now.
//	*	Add stats to form. # of Dirs, # of files, Unique File Sizes, # of potentially 
//		unique files
//	*	Calculating Short CRC message should have count of total # of files, so it
//		can display a progress bar. (And cool would be an estimated time to completion.)
//	*	Add more display info to the Process file size message.
//	*	Add the ability to bypass processing of specified directories. This would likely
//		include the system (C:\Windows) directories, and the recycle bin. And maybe TEMP
//		and TMP. Provide checkboxes for these (initialized to checked). Also provide a 
//		general add/remove directory feature (e.g. to bypass system directories/recycle 
//		bins on other hard drives on multi-boot systems).
//	*	Once a file entry has been stricken off the list (unique filesize, unique
//		filesize/CRC, file didn't match any others, etc), free up its space.
//	*	Add another pane to the status bar, with elapsed time in it (and maybe another 
//		with (probably laughably inaccurate) how much time left before the program 
//		finishes.
//	*	Rethink the whole CRC aspect. Calculating the CRCs seem to be slowing things 
//		down a lot,	as we open (at times) literally almost every file on the system. 
//		Maybe we don't do it for files too small. Or maybe too large? If file is small
//		enough (<=1K?), then if we calc CRCs for 1K, we don't have to open files to
//		compare. We just need to compare CRCs. For larger files, we'll just do the
//		straight file comparisons (although that leads to an n^2 set of comparisons).
//		Note: I just saw one bucket with 742 files in it, all of size 10K or so!
//	*	Hmmm. Big idea. Keep the raw data in a database. Basically we need the filename,
//		directory, date last modified and short CRC. This way we can bypass opening most
//		of the files. Hey, we could even calculate a full CRC if necessary (but it
//		probably wouldn't really help. Oh, and don't forget to delete database entries
//		if we don't find them on the drive (but that may be a separate button on 
//		the GUI).

#nullable enable

#define DBG_DUMPHT
#define DBG_MSGS
#define DBG_SHOW_EQUAL_FILES
// #define		DBG_ECHO_TO_CONSOLE

using System.IO;

using Bartizan.Utils.CRC;

namespace FindDuplicateFiles {
	class FileEntry {		
		// public string	DirName;
		// public string	FileName;
		public string	PathName;
		public long		FileSize;
		public uint		ShortCRC;       // The CRC of the beginning of the file. 

		static private byte[]	buf;
		static private BartCRC	crc;
		static private long		MinSize;
		static private long		MaxSize;
		static private int		PrefixSize;

//---------------------------------------------------------------------------------------

		static FileEntry() {
			var crc = new BartCRC();
		}

//---------------------------------------------------------------------------------------

		public static void SetCommonParameters(long minSize, long maxSize, int prefixSize) {
			MinSize    = minSize;
			MaxSize    = maxSize;
			PrefixSize = prefixSize;
			buf        = new byte[PrefixSize];
		}

//---------------------------------------------------------------------------------------

#if false
		public void xxxFileEntry(string DirName, string FileName) {
			this.DirName  = DirName;
			this.FileName = FileName;
			ShortCRC	  = 0;
		}
#endif

//---------------------------------------------------------------------------------------

		public FileEntry(string pathName) {
			PathName    = pathName;
			FileSize    = -1;
			ShortCRC    = 0;
			var fi      = new FileInfo(pathName);
			bool IsDir  = fi.Attributes.HasFlag(FileAttributes.Directory);
			if (IsDir) { return; }

			if ((fi.Length < MinSize) || (fi.Length > MaxSize)) { return; }
			FileSize    = fi.Length;
			// GetShortCRC(pathName);
		}

//---------------------------------------------------------------------------------------

		void GetShortCRC(string pathName) {
			using (FileStream fs = new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				fs.Read(buf, 0, PrefixSize);
				crc.Reset();
				crc.AddData(buf);
				ShortCRC = crc.GetCRC();
			}
		}
	}
}
