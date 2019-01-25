// Copyright (c) 2006-2007 by Larry Smith
// LDB == LRS Dated Backup

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace LDB {
	class LDB {
		static int Main(string[] args) {
#if DEBUG && true
			if (args.Length == 0) {
				args = new string[2];
				args[0] = @"C:\LRS\C#\GetApod";
				args[1] = @"S:\Backups";
				Console.WriteLine("DEBUG: Using {0} {1}", args[0], args[1]);
			}
#endif
			bool	bOK = CheckParms(args);
			if (! bOK) {
				Console.WriteLine("Usage: LDB sourcedir targetdir");
				Console.WriteLine("Copyright (c) 2006-2007 by Larry Smith");
				return 1;
			}

			// Create the output directory name from the command line parameter and
			// the current date and time.
			string	baseDir = args[0], tgtDir = args[1];
			string [] srcdirs = baseDir.Split(new char[] {Path.DirectorySeparatorChar});
			string	dir = srcdirs[srcdirs.Length - 1];
			dir = string.Format(@"{0} - {1:yyyy-MM-dd H-mm-ss} - {2}", dir, DateTime.Now,
				Environment.MachineName);
			tgtDir = Path.Combine(tgtDir, dir);
			Directory.CreateDirectory(tgtDir);

			// Copy all the files in the base directory, then all the directories (and
			// files and sub(sub*)directories)
			CopyAllFiles(baseDir, baseDir, tgtDir);
			string	RelativeSourcePath;
			string	TargetPath;
			foreach (string dirname in Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories)) {
				// TODO: Bug. Re-creates existing src directory
				RelativeSourcePath = GetRelativePath(baseDir, dirname);
				TargetPath = Path.Combine(tgtDir, RelativeSourcePath);
				Directory.CreateDirectory(TargetPath);
				CopyAllFiles(baseDir, dirname, TargetPath);
			}
			Console.WriteLine("Done");
			return 0;
		}

//---------------------------------------------------------------------------------------

		private static string GetRelativePath(string baseDir, string dirname) {
			if (baseDir == dirname)
				return "";
			string srcFile = dirname.Substring(baseDir.Length + 1);
			return srcFile;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Copies files from a specified directory to a target directory. The starting
		/// point of the source directory is given, so we can calculate the subdirectory
		/// we need to create on the base target directory. Using the example names in
		/// the params section below, in this case we would
		/// <ol>Create the directory F:\Backups\$Music - [datestamp]</ol>
		/// <ol>Create the directory F:\Backups\$Music - [datestamp?\Joemy Wilson</ol>
		/// <ul>(Both of these are done in the caller of this routine)</ul>
		/// <ol>Copy all files from C:\LRS\$Music to F:\Backups\$Music - [datestamp?\Joemy 
		/// Wilson</ol>
		/// </summary>
		/// <param name="basedir">e.g. C:\LRS\$Music</param>
		/// <param name="srcdir">e.g. C:\LRS\$Music\Joemy Wilson</param>
		/// <param name="tgtdir">e.g. F:\Backups</param>
		private static void CopyAllFiles(string basedir, string srcdir, string tgtdir) {
			// The base directory is, e.g. C:\LRS\$Music. The 
			// string srcFile = GetRelativePath(basedir, srcdir);
			string	TargetFilename;
			Console.WriteLine("About to process directory {0}", tgtdir);
			foreach (string filename in Directory.GetFiles(srcdir)) {
				// Console.WriteLine("About to copy {0} to {1}", filename, tgtdir);
				Console.WriteLine("  Copying {0}", filename);
				TargetFilename = Path.GetFileName(filename);
				File.Copy(filename, Path.Combine(tgtdir, TargetFilename), true);
			}
		}

//---------------------------------------------------------------------------------------

		private static bool CheckParms(string[] args) {
			// TODO: Until we get enums, print a message
			if (args.Length != 2) {
				Console.WriteLine("Error - must have two parms.");
				return false;
			}
			if (!Directory.Exists(args[0])) {
				Console.WriteLine("Error - source directory ({0} does not exist.", args[0]);
				return false;
			}
			if (! Directory.Exists(args[1])) {
				Console.WriteLine("Error - target directory ({0} does not exist.", args[1]);
				return false;
			}

			args[0] = Path.GetFullPath(args[0]);
			args[1] = Path.GetFullPath(args[1]);

			if (args[0].ToLower() == args[1].ToLower())	{	// See if source and target are the same
				// Note: If we were paranoid enough, we'd check for something like
				//		 ldb C:\LRS C:\LRS\foo\.. which would be the same directories
				Console.WriteLine("Error - Source and target directories are the same.");
				return false;			// TODO: Maybe better return enum than bool
										//		 so we can report better error msgs
			}

			// TODO: If we were *really* paranoid, we'd check to see if this was
			//		 a recursive call. For example, copying from C:\foo to c:\foo\goo
			return true;
		}
	}
}
