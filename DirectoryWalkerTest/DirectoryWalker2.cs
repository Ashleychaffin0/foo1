// Copyright (c) 2009 by Larry Smith

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

// TODO: 
//	1)	Add filtering by (list of) filenames.
//	2)	Add list of directories to exclude (e.g. Windows)
//	3)	I suppose we might support including/excluding special dirs symbolically (e.g.
//		My Documents), by providing a static method that does most of the work of
//		finding the lengthy enums, etc.
//	4)	Do we want an option to return files then dirs, vs dirs then files? (e.g. get
//		the dirs first so we can better/easier fake the DOS <dir> command).
//	5)	Do we want an option (i.e calling a method with a different signature) to return
//		the files all at once (e.g. for sorting)? Ditto for dirs. And to return both at
//		once (GetFileSystemInfos).


namespace LRS.DiskUtils.DirectoryWalkerTest {
	class DirectoryWalker2 {
		[DllImport("kernel32.dll")]
		static extern int QueryDosDevice(string lpDeviceName, IntPtr lpTargetPath,
		   int ucchMax);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern Int32 GetLastError();

		public static string[] QueryDosDevice() {

			// Allocate some memory to get a list of all system devices.
			// Start with a small size and dynamically give more space until we have enough room.
			int returnSize = 0;
			int maxSize = 1000;
			string allDevices = null;
			IntPtr mem;
			string[] retval = null;
			const int ERROR_INSUFFICIENT_BUFFER = 122;

			while (returnSize == 0) {
				mem = Marshal.AllocHGlobal(maxSize);
				if (mem != IntPtr.Zero) {
					// mem points to memory that needs freeing
					try {
						returnSize = QueryDosDevice(null, mem, maxSize);
						if (returnSize != 0) {
							allDevices = Marshal.PtrToStringAnsi(mem, returnSize);
							retval = allDevices.Split('\0');
							break;    // not really needed, but makes it more clear...
						} else if (GetLastError() == ERROR_INSUFFICIENT_BUFFER)
						//maybe better
						//else if( Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
						//ERROR_INSUFFICIENT_BUFFER = 122;
                {
							maxSize *= 10;
						} else {
							Marshal.ThrowExceptionForHR(GetLastError());
						}
					} finally {
						Marshal.FreeHGlobal(mem);
					}
				} else {
					throw new OutOfMemoryException();
				}
			}
			return retval;
		}


//---------------------------------------------------------------------------------------

		public enum EntryType {
			File,
			Directory,
			ErrorOnDirectory,		// e.g. no permissions
			Junction,				// How do we tell? I forget. Is a Junction the same
									//   as a reparse point, a hard link, etc.
									// Note: Check out C:\Users\Default. Within that you
									//		 see, say, a Junction for "Local Settings"
									//		 that the DIR command shows with its
									//		 resolution to [C:\Users\Default\AppData\Local]
			// TODO: There may be other types, such as reparse points, hard links, etc
		}

//---------------------------------------------------------------------------------------

		public enum DirWalkerCallbackResult {
			Continue,			// Normal processing
			Stop,				// All done. Pack up and go home
			IgnoreDir,			// Skip this dir, and all its subdirs
			SkipRestOfDir		// For some reason we've seen enough. Continue with
								//   next directory. Error if returned when presenting
								//   a dir, instead of a file.
		}

//---------------------------------------------------------------------------------------

		string	dir;

//---------------------------------------------------------------------------------------

		public DirectoryWalker2(string Directory) {
			if (! System.IO.Directory.Exists(Directory)) {
				throw new ArgumentException(
					// TODO: I know. It could be the name of a directory that we don't 
					//		 have permission to view. Or something else. So the message
					//		 isn't quite perfect. But it will do for now.
					string.Format("The specified parameter ({0}) was not the name of a directory", Directory), "Directory");
			}
			this.dir = Directory;
		}

//---------------------------------------------------------------------------------------

		public void Walk(Func<EntryType, DirectoryInfo, FileInfo, DirWalkerCallbackResult> fn) {
			WalkDir(fn, new DirectoryInfo(dir));
		}

//---------------------------------------------------------------------------------------

		private void WalkDir(Func<EntryType, DirectoryInfo, FileInfo, DirWalkerCallbackResult> fn,
				DirectoryInfo dirInfo) {
			// The .Net Framework doesn't seem to support wandering into Hidden
			// directories, and in fact gives an error if you ask for files/dirs in
			// a Hidden directory. So if this is a Hidden dir, just leave.
			if ((dirInfo.Attributes & FileAttributes.Hidden) != 0) {
				return;
			}
			DirWalkerCallbackResult res;
			res = fn(EntryType.Directory, dirInfo, null);
			// TODO: Check return code
			// There may be a problem getting files from a given directory (e.g. the
			// Recycle bin). If so, call callback function with appropriate EntryType.
			try {
				foreach (FileInfo fi in dirInfo.GetFiles()) {
					res = fn(EntryType.File, dirInfo, fi);
					switch (res) {
						case DirWalkerCallbackResult.Continue:
							// Do nothing
							break;
						case DirWalkerCallbackResult.Stop:
							return;
						case DirWalkerCallbackResult.IgnoreDir:
							// TODO: Do nothing for now
							break;
						case DirWalkerCallbackResult.SkipRestOfDir:
							// TODO: Do nothing for now
							break;
						default:
							// Shouldn't happen. Should throw, but for now we'll ignore it
							break;
					}
				}
				foreach (DirectoryInfo di in dirInfo.GetDirectories()) {
					WalkDir(fn, di);
				}
			} catch (Exception) {
				res = fn(EntryType.ErrorOnDirectory, dirInfo, null);
				// TODO: Check return code
			}

		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// From CodeProject "Vista Directory Links in .NET", by Manfred Bittersam. Merci!
	class ReparsePoint {
		#region "DllImports, Constants & Structs"
		private const Int32 INVALID_HANDLE_VALUE = -1;
		private const Int32 OPEN_EXISTING = 3;
		private const Int32 FILE_FLAG_OPEN_REPARSE_POINT = 0x200000;
		private const Int32 FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
		private const Int32 FSCTL_GET_REPARSE_POINT = 0x900A8;
		// Note: Some of these come from WinIoCtl.h		-- LRS

		/// <summary>
		/// If the path "REPARSE_GUID_DATA_BUFFER.SubstituteName" 
		/// begins with this prefix,
		/// it is not interpreted by the virtual file system.
		/// </summary>
		private const String NonInterpretedPathPrefix = "\\??\\";

		[StructLayout(LayoutKind.Sequential)]
		private struct REPARSE_GUID_DATA_BUFFER {
			public UInt32 ReparseTag;
			public UInt16 ReparseDataLength;
			public UInt16 Reserved;
			public UInt16 SubstituteNameOffset;
			public UInt16 SubstituteNameLength;
			public UInt16 PrintNameOffset;
			public UInt16 PrintNameLength;

			/// <summary>
			/// Contains the SubstituteName and the PrintName.
			/// The SubstituteName is the path of the target directory.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			public byte[] PathBuffer;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateFile(String lpFileName,
												Int32 dwDesiredAccess,
												Int32 dwShareMode,
												IntPtr lpSecurityAttributes,
												Int32 dwCreationDisposition,
												Int32 dwFlagsAndAttributes,
												IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 DeviceIoControl(IntPtr hDevice,
													 Int32 dwIoControlCode,
													 IntPtr lpInBuffer,
													 Int32 nInBufferSize,
													 IntPtr lpOutBuffer,
													 Int32 nOutBufferSize,
													 out Int32 lpBytesReturned,
													 IntPtr lpOverlapped);
		#endregion

		/// <summary>
		/// Gets the target directory from a directory link in Windows Vista.
		/// </summary>
		/// <param name="directoryInfo">The directory info of this directory 
		/// link</param>
		/// <returns>the target directory, if it was read, 
		/// otherwise an empty string.</returns>
		public static String GetTargetDir(DirectoryInfo directoryInfo) {
			String targetDir = "";

			try {
				// Is it a directory link?
				if ((directoryInfo.Attributes
					& FileAttributes.ReparsePoint) != 0) {
					// Open the directory link:
					IntPtr hFile = CreateFile(directoryInfo.FullName,
												0,
												0,
												IntPtr.Zero,
												OPEN_EXISTING,
												FILE_FLAG_BACKUP_SEMANTICS |
												FILE_FLAG_OPEN_REPARSE_POINT,
												IntPtr.Zero);
					if (hFile.ToInt32() != INVALID_HANDLE_VALUE) {
						// Allocate a buffer for the reparse point data:
						Int32 outBufferSize = Marshal.SizeOf
							(typeof(REPARSE_GUID_DATA_BUFFER));
						IntPtr outBuffer = Marshal.AllocHGlobal(outBufferSize);

						try {
							// Read the reparse point data:
							Int32 bytesReturned;
							Int32 readOK = DeviceIoControl(hFile,
													   FSCTL_GET_REPARSE_POINT,
															IntPtr.Zero,
															0,
															outBuffer,
															outBufferSize,
															out bytesReturned,
															IntPtr.Zero);
							if (readOK != 0) {
								// Get the target directory from the reparse 
								// point data:
								REPARSE_GUID_DATA_BUFFER rgdBuffer =
									(REPARSE_GUID_DATA_BUFFER)
									Marshal.PtrToStructure
								 (outBuffer, typeof(REPARSE_GUID_DATA_BUFFER));
								targetDir = Encoding.Unicode.GetString
										  (rgdBuffer.PathBuffer,
										  rgdBuffer.SubstituteNameOffset,
										  rgdBuffer.SubstituteNameLength);
								if (targetDir.StartsWith
									(NonInterpretedPathPrefix)) {
									targetDir = targetDir.Substring
						(NonInterpretedPathPrefix.Length);
								}
							}
						} catch (Exception) {
						}

						// Free the buffer for the reparse point data:
						Marshal.FreeHGlobal(outBuffer);

						// Close the directory link:
						CloseHandle(hFile);
					}
				}
			} catch (Exception) {
			}

			return targetDir;
		}
	}
}
