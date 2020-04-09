using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLoadAssembly {
	public partial class Form1 : Form {
		// static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

		// The CharSet must match the CharSet of the corresponding PInvoke signature
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		struct WIN32_FIND_DATA {
			public uint dwFileAttributes;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=14)]
			public string cAlternateFileName;
		}

		public Form1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			// TrySomethingElse(@"d:\lrs\$dev\", "Xamarin.Forms.Platform.dll");
			TrySomethingElse(@"d:\lrs\$dev\", "*Xamarin*.dll");
		}

//---------------------------------------------------------------------------------------

		private void TrySomethingElse(string StartingPath, string Pattern) {
			// var xxx = FindFirstFile("testloadassembly.exe", out WIN32_FIND_DATA fd);
			string dir = null;

			string Target = Path.Combine(StartingPath, Pattern);
			var si = new ProcessStartInfo("cmd.exe", "/c dir /s " + Target); // d:\\lrs\\$dev\\Xamarin.Forms.Platform.dll /s");
			si.UseShellExecute = false;
			si.RedirectStandardOutput = true;
			var proc = Process.Start(si);

			string line;
			while ((line = proc.StandardOutput.ReadLine()) != null) {
				if (line.StartsWith(" Directory of ")) {
					dir = line.Substring(14);
				} else if ((line.Length > 0) && (char.IsDigit(line[0]))) {
					string Filename = Path.Combine(dir, line.Substring(39));
					try {
						var asm = Assembly.LoadFrom(Filename);
						foreach (var attr in asm.CustomAttributes) {
							if (attr.AttributeType.Name == "TargetFrameworkAttribute") {
								var xxx = attr.ConstructorArguments[0];
								Console.WriteLine($"{xxx} -- {Filename}");
							}
						}
					} catch {
						// Ignore
					}
					// var foo1 = asm.CustomAttributes.ElementAt(13);
					// var foo2 = foo1.ConstructorArguments[0];
					//Console.WriteLine($"{foo2} -- {Filename}");

				}
				// Console.WriteLine(line);
			}
			proc.WaitForExit();

			//var dw = new DirectoryWalker();
			//dw.SetIncludedDirectoriess("C:\\", "D:\\");
			//dw.SetIncludedFileTypes("Xamarin.Forms.Platform.dll");
			//var things = dw.Walk();
			//foreach (var thing in things) {
			//	Console.WriteLine(thing);
			//}
		}
	}

// namespace DirectoryWalker_Sept_2013 {
		class DirectoryWalker {
			string[]    IncludedDirNames;
			string[]    IncludedFiletypes;

#if false // TODO:
		class DirectoryWalkerInfo {
			// public DirectoryInfo di;
			public FileInfo fi;
			// public Path DirName;
			// public string Filename;
			// public bool bIsDirectory;
		}
#endif

//---------------------------------------------------------------------------------------

			public DirectoryWalker() {
				IncludedDirNames = new string[0];
				IncludedFiletypes = new string[0];
			}

//---------------------------------------------------------------------------------------

			public DirectoryWalker SetIncludedDirectoriess(params string[] DirNames) {
				IncludedDirNames = DirNames;
				return this;
			}

//---------------------------------------------------------------------------------------

			public DirectoryWalker SetIncludedFileTypes(params string[] FileTypes) {
				IncludedFiletypes = FileTypes;
				return this;
			}

//---------------------------------------------------------------------------------------

			public IEnumerable<FileInfo> Walk(params string[] DirNames) {
				// TODO: Should return IEnumerable<DirectoryWalkerInfo> ???
				string[] Dirs;
				if (DirNames.Length > 0) {
					Dirs = DirNames;
				} else {
					Dirs = IncludedDirNames;
				}

				foreach (string DirName in Dirs) {

					foreach (var item in ProcessDir(DirName)) {
						yield return item;
					}
					// ProcessDir(DirName);

#if false
				foreach (var fname in Directory.EnumerateFiles(DirName)) {
					yield return new FileInfo(fname);
				}

				foreach (var dname in Directory.EnumerateDirectories(DirName)) {
					yield return new FileInfo(dname);
				}

				// yield return DirName;
#endif
				}
			}

//---------------------------------------------------------------------------------------

			private IEnumerable<FileInfo> ProcessDir(string DirName) {
				FileInfo fi;
				foreach (var fname in Directory.EnumerateFiles(DirName, IncludedFiletypes[0])) { // TODO:
					fi = new FileInfo(fname);
					// Console.WriteLine(fi.FullName + "    " + fi.Attributes.ToString());
					yield return fi;
				}

				foreach (var dname in Directory.EnumerateDirectories(DirName)) {
					fi = new FileInfo(dname);
					// Console.WriteLine(fi.FullName + "    " + fi.Attributes.ToString());
					yield return fi;
					foreach (var item in ProcessDir(dname)) {
						yield return item;
					}
					// ProcessDir(dname);
				}
			}
		}
// 	}
}
