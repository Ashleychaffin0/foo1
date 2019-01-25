// #define	IM_THE_COMPARE_PROGRAM
#define	COPY_WITH_OFFSET_RETRY

#if true
#define CREATE_DIRECTORIES
#define ACTUALLY_DO_THE_COPY
#endif

using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace CopyAllFiles {

	class CopyFiles {

		bool			OKToRun;

		DirectoryInfo	diSrc, diTarget;

		int				IndentationLevel = 0;

#if IM_THE_COMPARE_PROGRAM
		const string	Program = "CompareAllFiles";
#else
		const string	Program = "CopyAllFiles";
#endif

		const int		MaxRetries	  = 2;	//100;
		const int		RetryInterval = 1;	// 3

		ArrayList		FailedFiles = new ArrayList();

//---------------------------------------------------------------------------------------

		public CopyFiles(string[] args) {
			DateTime	SessionStart = DateTime.Now;
			string		mdy, hms;

			mdy = String.Format("{0:D2}-{1:D2}-{2:D2}", SessionStart.Month, SessionStart.Day, SessionStart.Year);
			hms = String.Format("{0:D2}:{1:D2}:{2:D2}", SessionStart.Hour, SessionStart.Minute, SessionStart.Second);
			print("Starting processing at {0} on {1}", hms, mdy);
			OKToRun = VerifyArgs(args);
		}

//---------------------------------------------------------------------------------------

		bool VerifyArgs(string[] args) {
			int		n;
#if DEBUG
			// args = new string[] {@"C:\LRS\C#\CopyAllFiles", @"M:\LRS\C#\CopyAllFiles"};
			// args = new string[] {@"C:\", @"M:\"};
			args = new string[] {@"C:\LRS\DIM", @"L:\LRS\DIM"};
#else
			if (args.Length != 2) {
				string	msg;
				msg  = "Usage: CopyAllFiles SourceDir TargetDir\n";
#if IM_THE_COMPARE_PROGRAM
				msg += "\nTHIS IS REALLY THE COMPAREALLFILES PROGRAM\n";
#endif
				msg += "\n";
				msg += "       Example: CopyAllFiles C:\\LRS\\CopyAllFiles M:\\Larry\n";
				msg += "\n";
				msg += "       Note: The initial directory structure is *NOT* duplicated.\n";
				msg += "             For example, in the above example, all files from\n";
				msg += "             C:\\LRS\\CopyAllFiles will be copied to M:\\Larry,\n";
				msg += "             and all files from C:\\LRS\\CopyAllFiles\\bin will\n";
				msg += "             be copied to M:\\Larry\\bin.\n";
				msg += "\n";
				msg += "             So if you wanted to duplicate the directory structure\n";
				msg += "             you should manually create M:\\LRS\\CopyAllFiles FIRST,\n";
				msg += "             and then issue\n";
				msg += "\n";
				msg += "                CopyAllFiles C:\\LRS\\CopyAllFiles M:\\LRS\\CopyAllFiles\n";
				Console.WriteLine(msg);
				return false;
			}
#endif
			n = 0;
			try {
				if (! Directory.Exists(args[n])) {
					print("Command line error - source directory ({0}) does not exist", args[n]);
					return false;
				}
				diSrc = new DirectoryInfo(args[n]);

				n = 1;
				if (! Directory.Exists(args[n])) {
#if IM_THE_COMPARE_PROGRAM == false
	#if CREATE_DIRECTORIES
					Directory.CreateDirectory(args[n]);
					print("Destination directory ({0}) does not exist. Directory has been created.", args[n]);
	#endif
#endif
				}
				diTarget = new DirectoryInfo(args[n]);

#if IM_THE_COMPARE_PROGRAM
				print("Comparing from {0} to {1}", diSrc.FullName, diTarget.FullName);
#else
				print("Copying from {0} to {1}", diSrc.FullName, diTarget.FullName);
#endif

			} catch (Exception e) {
				print("Command line error '{0}' processing arg[{1}]={2}", e.Message, n, args[n]);
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		public void Run() {
			if (! OKToRun)
				return;
			try {
				RecurseThroughDirectories(diSrc, diTarget);
			} catch (Exception e) {
				print("A fatal error occured during " + Program + " processing. The program will now "
					+ "terminate. The error message is {0}\n\nThe traceback is {1}", 
					e.Message, e.StackTrace);
			}

			if (FailedFiles.Count == 0) {
				print("\n\n  Good news! No files failed to copy/compare (unless the previous message was a fatal error).");
			} else {
				print("\n\n{0} file(s) failed to copy/compare --", FailedFiles.Count);
				foreach (object o in FailedFiles) {
					print("\n  Failed file - {0}", o);
				}
			}
		}

//---------------------------------------------------------------------------------------

		void RecurseThroughDirectories(DirectoryInfo diSrc, DirectoryInfo diTarget) {
			print("");
			print("* Processing directory {0}", diSrc.FullName);

			string	TargetDirName = diTarget.FullName;
			if (! TargetDirName.EndsWith("\\"))
				TargetDirName += "\\";

#if IM_THE_COMPARE_PROGRAM
			CompareFilesInDir(diSrc, diTarget);
#else
#if CREATE_DIRECTORIES
				// print("Creating Target directory {0}", diTarget.FullName);
				CreateDir(diTarget.FullName);
#else
				print("* Bypassing creation of target dirctory {0}", diTarget.FullName);
#endif
			CopyFilesInDir(diSrc, diTarget);
#endif

			DirectoryInfo [] srcDirs = GetDirInfo(diSrc);
			++IndentationLevel;
			string	SrcDirName;
			foreach (DirectoryInfo di in srcDirs) {
				SrcDirName = di.Name;
				// If the source dir is the root, it shows up as "C:\", not an empty
				// string. This leads to trouble when we try to append C:\ to the
				// target dir name. So we've got to handle that case.
				if (SrcDirName.Length >= 3 && SrcDirName.Substring(1, 2) == ":\\")
					SrcDirName = "";
				diTarget = new DirectoryInfo(TargetDirName + SrcDirName);
				RecurseThroughDirectories(di, diTarget);		
			}
			--IndentationLevel;
		}

//---------------------------------------------------------------------------------------

		DirectoryInfo [] GetDirInfo(DirectoryInfo diSrc) {
			for (int i=1; i<=MaxRetries; ++i) {
				try {
					return diSrc.GetDirectories();
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning* Unable to read directory {0} (dirs) on attempt {1} of {2}."
							+ " Error text - {3}."
							+ " Delaying {4} seconds and retrying...", 
							diSrc.FullName, i, MaxRetries, e.Message, RetryInterval);
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("\n\n*Fatal Error* - Could not read directory {0} (dirs). Program will now abort.",
							diSrc.FullName);
						throw;
					}
				}
			}
			return null;		// Should never get here, but keep compiler happy
		}

//---------------------------------------------------------------------------------------

		FileInfo [] GetFileInfo(DirectoryInfo diSrc) {
			for (int i=1; i<=MaxRetries; ++i) {
				try {
					return diSrc.GetFiles();
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning* Unable to read directory {0} (files) on attempt {1} of {2}."
							+ " Error text - {3}."
							+ " Delaying {4} seconds and retrying...", 
							diSrc.FullName, i, MaxRetries, e.Message, RetryInterval);
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("\n\n*Fatal Error* - Could not read directory {0} (files). Program will now abort.",
							diSrc.FullName);
						throw;
					}
				}
			}
			return null;		// Should never get here, but keep compiler happy
		}

//---------------------------------------------------------------------------------------

		void CopyFilesInDir(DirectoryInfo diSrc, DirectoryInfo diTarget) {
			FileInfo[] srcFiles = GetFileInfo(diSrc);
			int		n = 0;
			foreach (FileInfo srcFile in srcFiles) {
				Copy(srcFile, diSrc, diTarget, ++n, srcFiles.Length);
			}
		}

//---------------------------------------------------------------------------------------

		void CompareFilesInDir(DirectoryInfo diSrc, DirectoryInfo diTarget) {
			FileInfo[] srcFiles = diSrc.GetFiles();
			int		n = 0;
			foreach (FileInfo srcFile in srcFiles) {
				Compare(srcFile, diSrc, diTarget, ++n, srcFiles.Length);
			}
		}

//---------------------------------------------------------------------------------------

		void Copy(FileInfo srcFile, DirectoryInfo diSrc, DirectoryInfo diTarget, int n, int nInDir) {
			// There might be an I/O (read: network) error copying the file.
			// Provide error recovery for that case.
			string		srcFilename;
			string		tgtFilename;

			srcFilename = diSrc.FullName;
			if (! srcFilename.EndsWith("\\"))
				srcFilename += "\\";
			srcFilename += srcFile.Name;
			tgtFilename = diTarget.FullName;
			if (! tgtFilename.EndsWith("\\"))
				tgtFilename += "\\";
			tgtFilename += srcFile.Name;
#if ACTUALLY_DO_THE_COPY
#if COPY_WITH_OFFSET_RETRY
			for (int i=1; i<=1; ++i) {		// Only go through once. Subr will loop.
				try {
					print("Copying file {0} of {1}, size {2:N0}K, named {3}, attempt {4} of {5}", n, nInDir, (srcFile.Length + 511) / 1024, srcFile.Name, i, MaxRetries);
					CopyWithOffsetRetry(srcFile, srcFilename, tgtFilename);
					break;
#else
			for (int i=1; i<=MaxRetries; ++i) {
				try {
					print("Copying file {0} of {1}, size {2:N0}K, named {3}, attempt {4} of {5}", n, nInDir, (srcFile.Length + 511) / 1024, srcFile.Name, i, MaxRetries);
					File.Copy(srcFilename, tgtFilename, true);
					break;
#endif
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning - Error {0} copying file {1} on attempt {2} of {3}."
							+ " Delaying {4} seconds and retrying...", 
							e.Message, srcFile, i, MaxRetries, RetryInterval);
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("*Error* Could not copy file {0}. File bypassed.", srcFilename);
						FailedFiles.Add(srcFilename);
						return;
					}
				}
			}
#else
			//print("Bypassing copying file {0}", srcFile.Name);
			print("Bypassing copying file {0} to {1}", srcFilename, tgtFilename);
#endif
		}

//---------------------------------------------------------------------------------------

		void CopyWithOffsetRetry(FileInfo srcFile, string srcFilename, string tgtFilename) {
			const int	Bufsize = 4 * 1024;	// Keep the buffer size small. The larger it
											// is, the more likely a read will fail
			byte []			buf = new byte[Bufsize];
			long			nBytesOK;			// Number of bytes successfully read
			FileStream		srcStream = null, tgtStream = null;
			BinaryReader	srcRdr;
			BinaryWriter	tgtWtr;

			nBytesOK = 0;
			for (int i=1; i<=MaxRetries; ++i) {
				// OK, here's the actual copy
				try {
					// print("Copying file {0} of {1}, size {2:N0}K, named {3}, offset {4}, attempt {5} of {6}", nFile, nInDir, (srcFile.Length + 511) / 1024, srcFile.Name, nBytesOK, i, MaxRetries);
					// The target file must have a size of at least nBytesOK, otherwise
					// seeking into the file will fail. If the file is smaller, scale
					// back nBytesOK;
					FileInfo	tgtFileInfo = new FileInfo(tgtFilename);
					if (tgtFileInfo.Length <= nBytesOK) {
						nBytesOK = tgtFileInfo.Length;
					}

					srcStream = new FileStream(srcFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read,
						System.IO.FileShare.Read, Bufsize);
					tgtStream = new FileStream(tgtFilename, System.IO.FileMode.Open, System.IO.FileAccess.Write,
						System.IO.FileShare.None, Bufsize);
					// In case of a retry, continue from where we left off
					srcStream.Seek(nBytesOK, System.IO.SeekOrigin.Begin);
					tgtStream.Seek(nBytesOK, System.IO.SeekOrigin.Begin);
					if (nBytesOK > 0)
						print("Restarting copy at offset {0:N}", nBytesOK);
					srcRdr = new BinaryReader(srcStream);
					tgtWtr = new BinaryWriter(tgtStream);

					while (true) {
						buf = srcRdr.ReadBytes(Bufsize);
						if (buf.Length == 0)		// Check for EOF
							break;
						// if (++ProgressIndex >= ShowProgress.Length)
						// 	ProgressIndex = 0;
						//Console.Error.Write(ShowProgress.Substring(ProgressIndex, 1));
						Console.Error.Write("\r{0:N}K ", (nBytesOK + buf.Length) / 1024);
						tgtWtr.Write(buf);
						nBytesOK += buf.Length;
					}
					break;
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning - Error {0} copying file {1} on attempt {2} of {3}."
							+ " Delaying {4} seconds and retrying...", 
							e.Message, srcFile, i, MaxRetries, RetryInterval);
						try {		// Close files. Ignore any errors
							if (srcStream != null)
								srcStream.Close();
							if (tgtStream != null)
								tgtStream.Close();
						} catch {}
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("*Error* Could not copy file {0}. File bypassed.", srcFilename);
						FailedFiles.Add(srcFilename);
						return;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		void Compare(FileInfo srcFile, DirectoryInfo diSrc, DirectoryInfo diTarget, int nFile, int nInDir) {
			// There might be an I/O (read: network) error copying the file.
			// Provide error recovery for that case.
			string		srcFilename;
			string		tgtFilename;
			int			nBytesOK;			// Number of bytes successfully read
			const int	Bufsize = 4 * 1024;	// Keep the buffer size small. The larger it
											// is, the more likely a read will fail
			byte []		srcBuf = new byte[Bufsize];
			byte []		tgtBuf = new byte[Bufsize];

			// string		ShowProgress = @"`~!@#$%^&*()+={};/\|[]";
			// int			ProgressIndex = -1;

			srcFilename = diSrc.FullName;
			if (! srcFilename.EndsWith("\\"))
				srcFilename += "\\";
			srcFilename += srcFile.Name;
			tgtFilename = diTarget.FullName;
			if (! tgtFilename.EndsWith("\\"))
				tgtFilename += "\\";
			tgtFilename += srcFile.Name;
			// Note: The code above is identical to that in the Copy() function. Make a
			// subroutine out of it later.
			nBytesOK = 0;
			FileStream		srcStream = null, tgtStream = null;
			BinaryReader	srcRdr, tgtRdr;
			for (int i=1; i<=MaxRetries; ++i) {
				// OK, here's the actual compare
				try {
					print("Comparing file {0} of {1}, size {2:N0}K, named {3}, offset {4}, attempt {5} of {6}", nFile, nInDir, (srcFile.Length + 511) / 1024, srcFile.Name, nBytesOK, i, MaxRetries);
					// Make quick check that the files are the same size. If not, don't bother
					FileInfo	tgtFileInfo = new FileInfo(tgtFilename);
					if (tgtFileInfo.Length != srcFile.Length) {
						print("*Error - Source and target files are different lengths. Src length = {0}, Tgt length = {1}",
							srcFile.Length, tgtFileInfo.Length);
						break;
					}

					srcStream = new FileStream(srcFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read,
						System.IO.FileShare.Read, Bufsize);
					tgtStream = new FileStream(tgtFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read,
						System.IO.FileShare.Read, Bufsize);
					// In case of a retry, continue from where we left off
					srcStream.Seek(nBytesOK, System.IO.SeekOrigin.Begin);
					tgtStream.Seek(nBytesOK, System.IO.SeekOrigin.Begin);
					if (nBytesOK > 0)
						print("Restarting compare at offset {0:N}", nBytesOK);
					srcRdr = new BinaryReader(srcStream);
					tgtRdr = new BinaryReader(tgtStream);

					while (true) {
						srcBuf = srcRdr.ReadBytes(Bufsize);
						if (srcBuf.Length == 0)		// Check for EOF
							break;
						// if (++ProgressIndex >= ShowProgress.Length)
						// 	ProgressIndex = 0;
						//Console.Error.Write(ShowProgress.Substring(ProgressIndex, 1));
						Console.Error.Write("\r{0:N}K ", (nBytesOK + srcBuf.Length) / 1024);
						tgtBuf = tgtRdr.ReadBytes(Bufsize);
						// Sigh. I don't know of any better way to compare two byte[]'s
						// than to loop.
						for (int n=0; n<srcBuf.Length; ++n) {
							if (srcBuf[n] != tgtBuf[n]) {
								print("*Error - Data mismatch at byte {0}. Src byte = 0x{1:X}, Tgt byte = 0x{2:X}",
									nBytesOK + n, srcBuf[n], tgtBuf[n]);
								break;
							}
						}
						nBytesOK += srcBuf.Length;
					}
					break;
				} catch (FileNotFoundException) {
					print("*Warning - Target file not found. No comparison done.");
					return;
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning - Error {0} comparing file {1} on attempt {2} of {3}."
							+ " Delaying {4} seconds and retrying...", 
							e.Message, srcFile, i, MaxRetries, RetryInterval);
						try {		// Close files. Ignore any errors
							if (srcStream != null)
								srcStream.Close();
							if (tgtStream != null)
								tgtStream.Close();
						} catch {}
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("*Error* Could not compare file {0}. File bypassed.", srcFilename);
						FailedFiles.Add(srcFilename);
						return;
					}
				}
			}
		}


//---------------------------------------------------------------------------------------

		void CreateDir(string TargetDirName) {
			// There might be an I/O (read: network) error creating the target directory.
			// Provide error recovery for that case.

			for (int i=1; i<=MaxRetries; ++i) {
				try {
					Directory.CreateDirectory(TargetDirName);
					break;
				} catch (Exception e) {
					if (i != MaxRetries) {
						print("*Warning* -- Error {0} creating target directory {1} on attempt {2} of {3}."
							+ " Delaying {4} seconds and retrying...", 
							e.Message, TargetDirName, i, MaxRetries, RetryInterval);
						Thread.Sleep(RetryInterval * 1000);
					} else {
						print("*Error* Could not create target directory. Program will now terminate.");
						throw;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		void print(string msg, params object[] args) {
			string	tstamp = timestamp();
			Console.Write(tstamp);
			Console.Error.Write(tstamp);

			for (int i=0; i<IndentationLevel * 4; ++i) {
				Console.Write(" ");
				Console.Error.Write(" ");
			}

			Console.WriteLine(msg, args);
			Console.Error.WriteLine(msg, args);
		}

//---------------------------------------------------------------------------------------

		string timestamp() {
			DateTime	now = DateTime.Now;
			string		hms;
			hms = String.Format("{0:D2}:{1:D2}:{2:D2} ", now.Hour, now.Minute, now.Second);
			return hms;
		}
	}

//---------------------------------------------------------------------------------------

	/// <summary>
	/// Summary description for CopyAllFiles.
	/// </summary>
	class CopyAllFiles {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

//---------------------------------------------------------------------------------------

		[STAThread]
		static void Main(string[] args)	{
			CopyFiles	cf = new CopyFiles(args);
			cf.Run();
		}
	}
}
