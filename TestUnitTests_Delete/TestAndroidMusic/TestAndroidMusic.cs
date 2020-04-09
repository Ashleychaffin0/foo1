using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRS.Utils;

// See https://stackoverflow.com/questions/34135294/how-to-get-the-list-of-all-the-filesobjects-in-a-folder-exposed-by-an-mtp-devi
// See https://www.bing.com/search?q=mtp%20list%20files&qs=n&form=QBRE&sp=-1&pq=mtp%20list%20files&sc=1-14&sk=&cvid=BBB96A71272448FFAB795F3F72188589
// See https://www.pcsuggest.com/access-mtp-usb-device/
// See https://www.mtpdrive.com/features.html
// See https://www.bing.com/search?q=access+mtp+folder&form=WNSGPH&qs=SW&cvid=9f3919e0795648a68bc6c44110d47b2d&pq=access+mtp+folder&cc=US&setlang=en-US&PC=DCTS&nclid=E5CE992C6C6A52061465125F6C4A3075&ts=1561420314853&elv=AY3%21uAY7tbNNZGZ2yiGNjfP8xdeRO*ourNCJynGIuNRfjXoKko5sjxIsI5Z9IaITUgvFA6JGrqRGti2*wpH*FPIXxJbzRv*i2YhswEoYlu3y&wsso=Moderate
// See https://en.wikipedia.org/wiki/Media_Transfer_Protocol
// See https://www.musicedmagic.com/computers/how-to-download-music-to-an-mp3-player.html
// See https://answers.microsoft.com/en-us/mobiledevices/forum/mdlumia-mdcamera/how-to-fix-a-mtp-usb-device-driver-problem-music/6bd0ffd6-8e60-49df-9181-b12c353d0f2e
// See https://docs.microsoft.com/en-us/windows/desktop/wpd_sdk/mtp-extensions
// See https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-drmnd/37ad5858-ae05-45ae-bdfa-97538c190576
// See https://docs.microsoft.com/en-us/windows-hardware/test/hlk/testref/7b5f7462-6648-4d64-b51a-df51888857e3
// See https://docs.microsoft.com/en-us/windows-hardware/test/hlk/testref/bbfe7dc4-8ccf-4d8d-b68e-5b6ccab11f0f
// See https://docs.microsoft.com/en-us/windows-hardware/test/hlk/testref/f88f2297-fae4-492b-9fa4-63c526221754
// See https://docs.microsoft.com/en-us/windows/desktop/api/mtpext/ns-mtpext-mtp_command_data_out


// See https://docs.microsoft.com/en-us/windows/desktop/wmp/mtp-device-extensions-for-metadata-transfer
// See https://docs.microsoft.com/en-us/windows/desktop/api/mtpext/ns-mtpext-mtp_command_data_in

namespace TestAndroidMusic {
	public partial class TestAndroidMusic : Form {
		public TestAndroidMusic() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void BtnTestPDM_Click(object sender, EventArgs e) {
#if true
			var devs2 = PdInfo.Pds();
			var devID = GetPdId("LRS G7");
			// var devID = GetPdIdByDescription("ZTE");
			// string to = Path.Combine(devID, "Phone", "Music");
			string to = Path.Combine(devID, "SD card");
			// var proc = Process.Start("explorer.exe " + "\"" + to + "\"");
			var cmd = $"explorer.exe \"{devID}\"";
			var proc = Process.Start(cmd);
			Clipboard.SetText(to);
			SetForegroundWindow(proc.MainWindowHandle);
			SendKeys.Send("^L");       // Goes to Address bar
			System.Threading.Thread.Sleep(1000);
			//SendKeys.Send(to);
			if (sender != null) return;

			CopyDir(to, @"G:\$$$ Test\Abbott And Costello\Masters of Comedy - Disc 6 of 8\01 March 10, 1949.wma");
#endif

			var ute = new Utility();
			var devs = ute.Get();


			PdInfo OurPhone = null;
			string MyPhoneName = "LRS G7";
			MyPhoneName = "Z835";           // TODO:
			foreach (var dev in PdInfo.Pds()) {
				// if (dev.FriendlyName == "LRS G7") {
				if (dev.FriendlyName == "ZTE Maven 3") {
					OurPhone = dev;
					break;
				}
			}
			if (OurPhone is null) {
				MessageBox.Show($"Uh oh -- no {MyPhoneName} detected");
				return;
			}
			MessageBox.Show("Found Phone!");    // TODO:

			// IPortableDeviceContent cont;
			// var phone = new PortableDeviceClass();
			// IPortableDevice

			// TODO: See https://github.com/slowmonkey/WPD-.NET-Wrapper

			// phone.Content(out var cont);
		}

//---------------------------------------------------------------------------------------

		private string GetPdId(string name) {
			var devs = PdInfo.Pds();
			foreach (var item in PdInfo.Pds()) {
				if (item.FriendlyName == name) { return item.ID; }
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		private string GetPdIdByDescription(string name) {
			var devs = PdInfo.Pds();
			foreach (var item in PdInfo.Pds()) {
				if (item.Description == name) { return item.ID; }
			}
			return null;
		}


//---------------------------------------------------------------------------------------

		int CopyDir(string to, List<string> from) {
			// TODO: Maybe move to LRSUtils (including overload below)
			// TODO: I suppose we could allow setting other fields, such
			//		 as Flags and others
			var Op = new SHFILEOPSTRUCT {
				hwnd = Process.GetCurrentProcess().Handle,
				Func = FileFuncFlags.FO_COPY,
				From = "",
				To = to,
				Flags = FILEOP_FLAGS.FOF_ALLOWUNDO | FILEOP_FLAGS.FOF_FILESONLY,
				ProgressTitle = "LRS Test of SHFileOperation"
			};
			// Op.To = to;
			// <from> takes a null-delimited list of dirs
			foreach (var item in from) {
				Op.From += item + '\0';
			}
			Op.From += '\0';    // List ends with double null
								// Note: SHFileOperation has, since Vista, been supplanted by
								//		 https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-ifileoperation-copyitem
			var retcode = SHFileOperation(ref Op);
			// Note: *Could* throw exception is retcode isn't 0
			return retcode;
		}

//---------------------------------------------------------------------------------------

		int CopyDir(string to, params string[] from) {
			var lst = new List<string>(from);
			return CopyDir(to, lst);
		}
	}
}

