using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// See http://geekswithblogs.net/tamir/archive/2009/01/07/audio-cd-operation-including-cd-text-reading-in-pure-c.aspx
// See: https://en.wikipedia.org/wiki/CD-Text#Software
// See: https://sourceforge.net/projects/bonkenc/?source=typ_redirect
// See: https://www.codeproject.com/articles/15011/how-to-retrieve-dvd-region-information#4
// See: https://en.wikipedia.org/wiki/SCSI_CDB

// See http://www.ecma-international.org/publications/files/ECMA-TR/ECMA%20TR-071.PDF
// See https://www.ecma-international.org/publications/files/ECMA-ST/Ecma-119.pdf
// See https://www.ecma-international.org/publications/files/ECMA-ST/Ecma-130.pdf
// See https://www.ecma-international.org/publications/files/ECMA-ST/Ecma-167.pdf
// See https://www.ecma-international.org/publications/files/ECMA-ST/Ecma-168.pdf

namespace ReadEntireDrive {
	public partial class ReadEntireDrive : Form {
		public ReadEntireDrive() {
			InitializeComponent();

			const int BUFSIZE = 1024 * 2;
			byte[] buf = new byte[BUFSIZE];
			for (int i = 0; i < buf.Length; i++) {
				buf[i] = 0x2b;
			}

			string DriveLetter = "E";
#if false
			IntPtr hDrive = NativeMethods.CreateFile(
				string.Format("\\\\.\\{0}:", DriveLetter),
				(uint)NativeMethods.File_Access_Mask.GENERIC_READ,
				(uint)NativeMethods.File_Share_Mode.FILE_SHARE_READ, //Read | Write,
				IntPtr.Zero,
				(uint)NativeMethods.File_Creation_Disposition.OPEN_EXISTING,
				0,
				IntPtr.Zero);
#endif
			var InFile = File.OpenRead($"\\\\.\\{DriveLetter}:");
			var fi = new DriveInfo(DriveLetter);
			int ofs = 0;
			int len;
			do {
				len = InFile.Read(buf, 0, BUFSIZE);
				Dump(buf, ofs);
				ofs += len;
			} while (len > 0);
		}

		private void Dump(byte[] buf, int ofs) {
			if (IsAllZeros(buf)) {
				Console.WriteLine($"\r\nAll Zeros at offset {Sect(ofs)}");
				return;
			}
			Console.WriteLine($"\r\n{Sect(ofs)}");
			for (int i = 0; i < buf.Length; i++) {;
				char c = (char)buf[i];
				if ((c < ' ') || (c > 0x7f)) c = '.';
				Console.Write(c);
				if ((i != 0) && (i % 128) == 0) Console.WriteLine();
			}
		}

		private string Sect(int ofs) {
			return $"Sector {ofs / 2048}, offset {ofs:X8}";
		}

		private bool IsAllZeros(byte[] buf) {
			foreach (byte b in buf) {
				if (b != 0) return false;
			}
			return true;
		}
	}
}
