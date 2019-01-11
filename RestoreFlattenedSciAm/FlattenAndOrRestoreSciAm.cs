// TODO:

//	*	At the moment, none!

// #define LRS		// Disables Flatten

using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;


namespace RestoreFlattenedSciAm {
	public partial class FlattenAndOrRestoreSciAm : Form {

		string[] Months = new string[] {
			"January", "February", "March", "April", "May", "June", 
			"July", "August", "September", "October", "November", "December" };

//---------------------------------------------------------------------------------------

		public FlattenAndOrRestoreSciAm() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void FlattenAndOrRestoreSciAm_Load(object sender, EventArgs e) {
			// txtSourceDir.Text = @"C:\$ Scientific American - Flattened";
			// txtTargetDir.Text = @"C:\$ Scientific American - Target";
			radRestore.Checked = true;
		}

//---------------------------------------------------------------------------------------

		private void radFlatten_CheckedChanged(object sender, EventArgs e) {
			lblOperation.Text    = "Copy files to a single target directory";
			lblYear.Text         = "Year";
			lblMonth.Visible     = true;
			progressBar2.Visible = true;
		}

//---------------------------------------------------------------------------------------

		private void radRestore_CheckedChanged(object sender, EventArgs e) {
			lblOperation.Text    = "Restore the directory structure from the flattened source directory.";
			lblYear.Text         = "Progress";
			lblMonth.Visible     = false;
			progressBar2.Visible = false;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseSourceDir_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.SelectedPath = "C:\\";
			var result = fbd.ShowDialog();
			if (result != System.Windows.Forms.DialogResult.OK) {
				return;
			}
			txtSourceDir.Text = fbd.SelectedPath;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseTargetDir_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.SelectedPath = "C:\\";
			var result = fbd.ShowDialog();
			if (result != System.Windows.Forms.DialogResult.OK) {
				return;
			}
			txtTargetDir.Text = fbd.SelectedPath;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			if (radFlatten.Checked) {
#if LRS
				DoFlatten();
#else
				MessageBox.Show("Sorry, the Flatten option is disabled in this release.", "Sci Am");
#endif
			} else {
				DoRestore();
			}
		}

//---------------------------------------------------------------------------------------

		private void DoRestore() {
			var files            = Directory.GetFiles(txtSourceDir.Text);
			progressBar1.Minimum = 1;
			progressBar1.Maximum = files.Length;
			progressBar1.Value   = 1;
			progressBar1.Step    = 1;
			foreach (var filename in files) {
				RestoreFilename(filename);
				progressBar1.PerformStep();
			}
			MessageBox.Show("Done -- Don't forget to copy the MIND etc issues over too", "Sci Am");
		}

//---------------------------------------------------------------------------------------

		private void RestoreFilename(string FilenameWithPath) {
			string filename2 = Path.GetFileName(FilenameWithPath);
			Console.WriteLine("Processing " + filename2);
			if (!filename2.StartsWith("SciAm - ")) {
				return;				// Ignore anything that may have crept in
				// TODO: What about MIND issues, etc?
			}
			// Minimal error checking. It's our utility, after all
			int Year    = int.Parse(filename2.Substring(8, 4));
			int Month   = int.Parse(filename2.Substring(13, 2));
			string dir1 = string.Format("SciAm - {0}", Year);
			string dir2 = string.Format("{0}-{1:00} - {2}", Year, Month, Months[Month - 1]);
			string dir3 = Path.Combine(dir1, dir2);
			// Recreate the directory every time. It's fast and we don't run this
			// program very often.
			string tgtDir = Path.Combine(txtTargetDir.Text, dir3);
			// e.g. C:\TargetDir\SciAm - 1993\1993-01 - January\
			Directory.CreateDirectory(tgtDir);
			MyCopyFile(FilenameWithPath, tgtDir);
		}

//---------------------------------------------------------------------------------------

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
internal static extern bool CopyFile(string src, string dst, bool failIfExists);			

		private void MyCopyFile(string SourceFile, string TargetDir) {

			// Note: the @"\\?\" prepended to the source and target filenames allow
			//		 path names up to 32K. And since individual filenames can
			//		 approach 260 chars themseleves, even ignoring the names of the
			//		 directories they're in, we need the 32K limit. And the managed
			//		 version (File.Copy) doesn't support the prefix. Dumb. So we'll
			//		 have to do it ourselves.
			string JustFilename   = Path.GetFileName(SourceFile);
			string TargetFilename = Path.Combine(TargetDir, JustFilename);
			// true               = Always overwrite
			CopyFile(@"\\?\" + SourceFile, @"\\?\" + TargetFilename, true);
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void DoFlatten() {
			var dirs             = Directory.GetDirectories(txtSourceDir.Text);	// 1993, 1994, etc
			progressBar1.Minimum = 1;
			progressBar1.Maximum = dirs.Length;
			progressBar1.Value   = 1;
			progressBar1.Step    = 1;
			foreach (var dirname in dirs) {		
				lblYear.Text = dirname.Substring(dirname.Length - 4);
				FlattenDirectoryByMonth(dirname);
				progressBar1.PerformStep();
			}
			MessageBox.Show("Done -- Don't forget to copy the MIND etc issues over too", "Sci Am");
		}

//---------------------------------------------------------------------------------------

		private void FlattenDirectoryByMonth(string dirname) {
			var dirs             = Directory.GetDirectories(dirname);	// 1993-Jan, 1993-Feb, etc
			progressBar2.Minimum = 1;
			progressBar2.Maximum = dirs.Length;
			progressBar2.Value   = 1;
			progressBar2.Step    = 1;
			foreach (var dir in dirs) {		
				lblMonth.Text = dir.Substring(dir.LastIndexOf(' ') + 1);
				FlattenDirectory(dir);
				progressBar2.PerformStep();
			}
		}

//---------------------------------------------------------------------------------------

		private void FlattenDirectory(string dir) {
			var filenames = Directory.GetFiles(dir);
			foreach (var filename in filenames) {
				MyCopyFile(filename, txtTargetDir.Text);
			}
		}
	}

//---------------------------------------------------------------------------------------

	/// <summary>
	/// This class is from MSDN. However it turns out we don't need it. We just needed
	/// the CopyFile import. Still, we might need something like this some day, so we'll
	/// leave it here (untested), just in case.
	/// </summary>
	class UnmanagedFileLoader {

		public const short FILE_ATTRIBUTE_NORMAL = 0x80;
		public const short INVALID_HANDLE_VALUE = -1;
		public const uint GENERIC_READ = 0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const uint CREATE_NEW = 1;
		public const uint CREATE_ALWAYS = 2;
		public const uint OPEN_EXISTING = 3;

		// Use interop to call the CreateFile function.
		// For more information about CreateFile,
		// see the unmanaged MSDN reference library.
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess,
		  uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
		  uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		private SafeFileHandle handleValue = null;

//---------------------------------------------------------------------------------------

		public UnmanagedFileLoader(string Path) {
			Load(Path);
		}

//---------------------------------------------------------------------------------------

		public void Load(string Path) {
			if (Path == null && Path.Length == 0) {
				throw new ArgumentNullException("Path");
			}

			// Try to open the file.
			handleValue = CreateFile(Path, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

			// If the handle is invalid,
			// get the last Win32 error 
			// and throw a Win32Exception.
			if (handleValue.IsInvalid) {
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
		}

//---------------------------------------------------------------------------------------

		public SafeFileHandle Handle {
			get {
				// If the handle is valid, return it.
				if (!handleValue.IsInvalid) {
					return handleValue;
				} else {
					return null;
				}
			}
		}
	}
}
