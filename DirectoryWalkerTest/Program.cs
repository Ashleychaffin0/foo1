using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Text;

using LRS.DiskUtils.DirectoryWalkerTest;

namespace DirectoryWalkerTest {
	class Program {

		[DllImport("kernel32.dll")]
		public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode,
			byte[] lpInBuffer, uint nInBufferSize, [Out] byte[] lpOutBuffer,
			uint nOutBufferSize, IntPtr lpBytesReturned, IntPtr lpOverlapped);

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct NativeOverlapped {
			private IntPtr InternalLow;
			private IntPtr InternalHigh;
			public long Offset;
			public IntPtr EventHandle;
		}

		// From pinvoke.net, as usual. Just in case...
		[StructLayout(LayoutKind.Sequential)]
		struct REPARSE_DATA_BUFFER {
			uint ReparseTag;
			ushort ReparseDataLength;
			ushort Reserved;
			ushort SubstituteNameOffset;
			ushort SubstituteNameLength;
			ushort PrintNameOffset;
			ushort PrintNameLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			byte[] PathBuffer;
		}

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

		// See WinIoCtl.h
		private static int CTL_CODE(int DeviceType, int Function, int Method, int Access) {
			return (DeviceType << 16) | (Access << 14) | (Function << 2) | Method;
		}

		static void Main(string[] args) {
			// DirectoryWalker dw = new DirectoryWalker(@"C:\LRS From LRS-P4-1, S-Drive");
			// DirectoryWalker dw = new DirectoryWalker(@"C:\LRS");
			// DirectoryWalker_1 dw = new DirectoryWalker_1(@"C:\");
			// dw.Walk();

			var x = DirectoryWalker2.QueryDosDevice();

			int nDirs = 0, nFiles = 0, nErrs = 0;

			//DirectoryInfo di2 = new DirectoryInfo(@"C:\Users\Default\AppData\Local\History");
			//var s2 = Directory.GetFileSystemEntries(@"C:\Users\Default\AppData\Local\History");
			//var fi2 = di2.GetFiles("*", SearchOption.AllDirectories);

			// DirectoryWalker2 dw2 = new DirectoryWalker2(@"C:\Users\Default");
			DirectoryWalker2 dw2 = new DirectoryWalker2(@"C:\");
			dw2.Walk((et, di, fi) => {
				switch (et) {
				case DirectoryWalker2.EntryType.Directory:
					++nDirs;
					Console.WriteLine("\n------------------\net={0}, di={1}, attrs={2}", et, di.FullName, di.Attributes);
					if ((di.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint) {
						string NewName = ReparsePoint.GetTargetDir(di);
						Console.WriteLine("******* Reparse Point ******* - New Name = {0}", NewName);
					}
					break;
				case DirectoryWalker2.EntryType.File:
					++nFiles;
					//Console.WriteLine("et={0}, di={1}, fi={2}", et, di, fi);
					break;
				case DirectoryWalker2.EntryType.ErrorOnDirectory:
					++nErrs;
					Console.WriteLine("Error on directory: di={0}, attrs={1}", di.FullName, di.Attributes);
					break;
				}
				return DirectoryWalker2.DirWalkerCallbackResult.Continue;
			});
			Console.WriteLine("{0} directories, {1} files, {2} errors", nDirs, nFiles, nErrs);
			Console.WriteLine("Press Enter to exit");
			Console.ReadLine();
		}
	}

}
