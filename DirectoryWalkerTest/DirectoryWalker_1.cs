using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// TODO:
//	1)	Make IEnumerable, using yield return? Probably on 
//		IEnumerable<KeyValuePair<DirectoryInfo, FileInfo>>
//	2)	Need flag to indicate recursion (on a dir-by-dir basis? I think not)
//	3)	Make multi-core aware? Or just use PLINQ?
//	4)	Where, exactly, does filtering by filename (e.g. *.txt) come into this? Or more general filtering?
//	5)	Support Func<>, for lambda expressions
//	6)	Need return from callback to say not to continue recursing into this dir

namespace DirectoryWalkerTest {
	class DirectoryWalker_1 {

		List<string> DirNames;			// Directories to search

		Dictionary<string, int>			FileTypeCounters;

//---------------------------------------------------------------------------------------

		public DirectoryWalker_1() {
			Reset();
		}

//---------------------------------------------------------------------------------------

		public DirectoryWalker_1(List<string> DirNames) {
			Reset();
			this.DirNames = DirNames;
		}

//---------------------------------------------------------------------------------------

		public DirectoryWalker_1(params string[] DirNames) {
			Reset();
			this.DirNames = new List<string>(DirNames);
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			DirNames = new List<string>();

			FileTypeCounters = new Dictionary<string,int>();
		}

//---------------------------------------------------------------------------------------

		public void Walk() {
			WalkPrivate(DirNames);
		}

//---------------------------------------------------------------------------------------

		public void Walk(List<string> DirNames) {
			WalkPrivate(DirNames);
		}

//---------------------------------------------------------------------------------------

		public void Walk(params string[] DirNames) {
			WalkPrivate(new List<string>(DirNames));
		}

//---------------------------------------------------------------------------------------

		private void WalkPrivate(List<string> DirNames) {
			if (DirNames == null) {
				throw new NullReferenceException("WalkPrivate.DirNames can't be null");
			}
			if (DirNames.Count == 0) {
				return;
			}

			bool bContinue = false;
			foreach (string DirName in DirNames) {
				try {
					bContinue = ProcessDirName(DirName);
					if (! bContinue) {
						return;
					}
				} catch (DirectoryNotFoundException ex) {
					// TODO: For now, mostly ignore a bad dirname. Later, do initial test
					//		 and throw.
					Console.WriteLine("Directory not found - {0}", ex.Message);
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="DirName"></param>
		/// <returns>true, if walking is to continue, false to stop now.</returns>
		private bool ProcessDirName(string DirName) {
			// TODO: Make this user-supplied
			// TODO: Also make this use DirectoryFilter
			// DirectoryInfo	di = new DirectoryInfo(DirName);
			Console.WriteLine("Processing <DIR> {0}", DirName);
			bool	bContinue;
			try {
				string[] FileNames = Directory.GetFiles(DirName);
				foreach (string FileName in FileNames) {
					bContinue = ProcessFile(DirName, FileName);
					if (!bContinue) {
						return false;
					}
				}
				foreach (string SubDirName in Directory.GetDirectories(DirName)) {
					bContinue = ProcessDirName(SubDirName);
					if (!bContinue) {
						return false;
					}
				}
			} catch (Exception ex) {
				Console.WriteLine("***** Can't process directory {0} - {1}", 
					DirName, ex.Message);
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool ProcessFile(string DirName, string FileName) {
			// Console.WriteLine("Processing file {0}", FileName);
			const long	MinSize = 0, MaxSize = 10 * 1024;	// TODO: DirectoryFilter
			string fname = Path.Combine(DirName, FileName);
			// TODO: Only get FileInfo if we need it (e.g. for file size)
			FileInfo	fi = new FileInfo(fname);

			// Extension counting
			string		exten = fi.Extension.ToLower();
			int			ExtCounter;
			if (FileTypeCounters.TryGetValue(exten, out ExtCounter)) {
				FileTypeCounters[exten] = ++ExtCounter;
			} else {
				FileTypeCounters[exten] = 1;
			}

			if ((fi.Length < MinSize) || (fi.Length > MaxSize)) {
				return true;
			}

			// TODO: DirectoryFilter
			List<string>	ExcludeFileTypes = new List<string>(new string [] {
				"exe", "dll", "zip", "pdb", "jpg", "wma", "wmv", "bmp", "png", "ico",
				"gif", "config", "csproj", "sln", "resx", "resources", "class", "suo",
				"projdata", "obj", "msi", "mui", "pdf", "manifest", "lib", "wav", "ttf",
				"mum",
				"_",	// Many installation files end with "_", e.g. "ex_", "dl_", etc
			});

			foreach (string ext in ExcludeFileTypes) {	// TODO: DirectoryFilter
				if (FileName.EndsWith(ext, StringComparison.CurrentCultureIgnoreCase)) {
					return true;
				}
			}

			// TODO: DirectoryFilter
			// TODO: Put in check for IncludeFileTypes

			// TODO: User Code
			try {
				string s = File.ReadAllText(fname);
				bool bFound;
				// Search for LRS, Chassis, hard, ati, system
				// string [] Keywords = new string[] {"LRS", "Chassis", "hard", "ati", "system"};
				string[] Keywords = new string[] { "LRS", "System", "chassis" };
				foreach (string Keyword in Keywords) {
					if (s.IndexOf(Keyword, StringComparison.CurrentCultureIgnoreCase) == -1) {
						return true;
					}
				}

				Console.WriteLine("**** Found keywords in {0}", fname);
				// System.Windows.Forms.Clipboard.SetData
			} catch (Exception ex) {
				// Essentially ignore files we can't open (e.g. no access)
				Console.WriteLine("*** Exception opening {0} -- {1}", fname, ex.Message);
			}

			return true;
		}

//---------------------------------------------------------------------------------------

		private void dbgShowFileTypeCounters() {
			var q = from Key in FileTypeCounters.Keys
					orderby FileTypeCounters[Key]
					select Key;
			foreach (string Key in q) {
				Console.WriteLine("{0} = {1}", Key, FileTypeCounters[Key]);
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class DirectoryFilter {
		// Things we can search on
		Dictionary<string, byte> IncludeFileTypes;	// File types to include (e.g.
													// *.cs, *.txt). If empty (but
													// not null), search all types.
		Dictionary<string, byte> ExcludeFileTypes;	// File types to exclude (e.g.
													// *.exe, *.dll, *.pdb, etc). If
													// empty (but not null), don't
													// exclude any files.
		long MinSize;								// Minimum file size to consider.
													// e.g. If this is 1000, then
													// files of 999 bytes and less
													// will be ignored.
		long MaxSize;								// Maximum file size to consider.
													// e.g. if this is set to 2000,
													// then files 2001 bytes and more
													// will be ignored.

		// TODO:
			// *	RemoveFromExcludeList (e.g. if you've got the defaults set, but you
			//		want to search, say, exe's. (Or do you put that in the Include list,
			//		which therefore needs to be called before the Exclude list. Yeah, 
			//		that sounds right.)
	}
}
