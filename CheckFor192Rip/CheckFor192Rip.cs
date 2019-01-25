using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using LRSNativeMethodsNamespace;
using LRS.Utils;
using PortableDeviceApiLib;

using TagLib;

using static LRSNativeMethodsNamespace.LRSNativeMethods;
using WindowsPortableDeviceNet;

namespace CheckFor192Rip {
	public partial class CheckFor192Rip : Form {
#if false
		Dictionary<int, int> nDirsByRate;
		Dictionary<int, List<string>> DirsByRate;
		Dictionary<string, int> FileTypes;
#endif
		Dictionary<int, List<AlbumInfo>> AlbumsByRate;
		Dictionary<int, (long TotalSize, TimeSpan TotalDuration)> Totals;
		DateTime	Earliest192;
		Stopwatch	sw;
		Timer		tmr;

		Dictionary<string, List<AlbumInfo>> AlbumsByArtistName;	// key = ArtistName


//---------------------------------------------------------------------------------------

		public CheckFor192Rip() {
			InitializeComponent();
			TxtStartingFolder.Text = @"G:\$ Zune Master";
			sw = new Stopwatch();
			tmr = new Timer();
			tmr.Interval = 1_000;   // 1 second
			tmr.Tick += Tmr_Tick;

#if false
			var lst = new List<string> {
				@"G:\MsConferences\Ignite 2018-dgfhrtygb",
				// @"G:\$TEST DEST\Ignite 2018-dgfhrtygb\$Uncategorized\All About Exchange 2019\All About Exchange 2019.mp4",
				@"G:\MSConferences\VS Live 2017"
			};
			var xxx = CopyDir(@"G:\$TEST DEST", lst); ;

			goo();

			var TotalDuration = new TimeSpan();
			long TotalSize = 0;
			Totals = new Dictionary<int, (long TotalSize, TimeSpan TotalDuration)>();
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			foreach (var key in Albums.Keys) {
				UpdateStats(key);
				TotalDuration += Albums[key].Duration;
				TotalSize += Albums[key].Size;
			}
			sw.Stop();

			string Summary = $"{Albums.Count:N0}: Duration={TotalDuration}, Size={TotalSize:N0}";
#endif
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			LblElapsed.Text = sw.Elapsed.ToString(@"mm\:ss");
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowse_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Browse");
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			AlbumsByRate    = default;
			Totals          = default;
			Earliest192     = new DateTime(2100, 12, 31);
			LblDone.Text    = "";
			LblElapsed.Text = "";

			sw.Restart();
			tmr.Start();

			AlbumsByRate       = new Dictionary<int, List<AlbumInfo>>();
			AlbumsByArtistName = new Dictionary<string, List<AlbumInfo>>();
			Totals             = new Dictionary<int, (long TotalSize, TimeSpan TotalDuration)>();

			string ZuneRootDir   = TxtStartingFolder.Text;
			var ZuneDirs         = Directory.EnumerateDirectories(ZuneRootDir, "*", SearchOption.AllDirectories);
			const int MaxFolders = int.MaxValue;        // For testing

			/*
				An Artist is a folder that has sub-folders
				An Album is a folder with no sub-folders
				An Album192 is an album recorded with a bit rate of 192
				A Ready-to-copy-to-phone-Artist is an Artist with the number of Album192's == the
					number of sub-folders it has
				Assumptions:
					* A folder either has sub-folders and no file children, or has children and no
						sub-folders
					* All folders have names <root>\Artist\Album, but in at least one case
						(78 RPM MP3), there is no Album
					* Some folders have no *.wma files in them (having come from other sources), so
						they won't be flagged as <IsGood> and won't be processed.
			*/

			foreach (var dir in ZuneDirs) {
				LblDone.Text = dir;
				var info     = new AlbumInfo(dir);
				if (!info.IsGood) { continue; }
				EnsureDictsExist(info);

				AlbumsByRate[info.Rate].Add(info);
				AlbumsByArtistName[info.ArtistName].Add(info);
				if ((info.Rate == 192) && (AlbumInfo.EarliestCreationTime < Earliest192)) {
					Earliest192		 = AlbumInfo.EarliestCreationTime;
					LblEarliest.Text = Earliest192.ToShortDateString();
				}
				UpdateUiStats(info.Rate);
#if DEBUG
				if (AlbumsByRate[info.Rate].Count >= MaxFolders) { break; }
#endif
				Application.DoEvents();
			}
			sw.Stop();
			tmr.Stop();
			LblDone.Text = "Done!";

			FindCopyableAlbums();
		}

//---------------------------------------------------------------------------------------

		private void FindCopyableAlbums() {
			var qry = from album in AlbumsByArtistName
					  where album.Value.All(cut => cut.Rate == 192)
					  select album;
			int count = 0;
			foreach (var item in qry) {
				Console.WriteLine($"[{++count}]: Copyable -- {item.Key}");
			}
		}

//---------------------------------------------------------------------------------------

		private void EnsureDictsExist(AlbumInfo info) {
			if (!AlbumsByRate.ContainsKey(info.Rate)) {
				AlbumsByRate[info.Rate] = new List<AlbumInfo>();    // New rate found
			}
			if (!AlbumsByArtistName.ContainsKey(info.ArtistName)) {
				AlbumsByArtistName[info.ArtistName] = new List<AlbumInfo>();
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowStats() {
			DoStat(128, LblCount128, LblSize128, LblAvgSize128, LblDuration128, LblAvgDuration128);
			DoStat(192, LblCount192, LblSize192, LblAvgSize192, LblDuration192, LblAvgDuration192);
		}

//---------------------------------------------------------------------------------------

		private void UpdateUiStats(int key) {
			var albs            = AlbumsByRate[key];
			int ixLast          = albs.Count - 1;
			var alb             = albs[ixLast];
			if (! Totals.ContainsKey(key)) {
				Totals[key] = (0, new TimeSpan());
			}
			var tots            = Totals[key];
			tots.TotalSize     += alb.Size;
			tots.TotalDuration += alb.Duration;
			Totals[key] = tots;

			if (AlbumsByRate.ContainsKey(192) && AlbumsByRate.ContainsKey(128)) { 
				var avg192            = AlbumsByRate[192].Average(al => al.Size);
				var EstimatedNewSize  = Totals[192].TotalSize + AlbumsByRate[128].Count * avg192;
				LblEstimatedSize.Text = AutoScale((long)EstimatedNewSize);
			}
			ShowStats();
		}

//---------------------------------------------------------------------------------------

		private void DoStat(int key, Label LblCount, Label lblSize, Label lblAvgSize,
					Label lblDuration, Label lblAvgDuration) {
			if (! AlbumsByRate.ContainsKey(key)) { return; }
			List<AlbumInfo> albs = AlbumsByRate[key];
			LblCount.Text        = albs.Count.ToString("N0");
			long TotalSize       = albs.Sum(a => a.Size);
			lblSize.Text         = AutoScale(TotalSize);
			lblAvgSize.Text      = AutoScale(TotalSize / albs.Count);
			var dur              = new TimeSpan();

			albs.ForEach(a => dur += a.Duration);
			lblDuration.Text  = dur.ToString(@"d\:hh\:mm\:ss");
			var avgDur = new TimeSpan(0, 0, (int)dur.TotalSeconds / albs.Count);
			lblAvgDuration.Text = avgDur.ToString(@"d\:hh\:mm\:ss");
		}

//---------------------------------------------------------------------------------------

		private new string AutoScale(long NumBytes) {
			const long KB = 1024;
			const long MB = KB * 1024;
			const long GB = MB * 1024;

			if (NumBytes < KB) {
				return NumBytes.ToString("N0");
			} else if (NumBytes < MB) {
				return ((NumBytes + KB - 1) / KB).ToString("N0") + " KB";
			}
			else if (NumBytes < GB) {
				return ((NumBytes + MB - 1) / MB).ToString("N0") + " MB";
			}
			return ((NumBytes + GB - 1) / GB).ToString("N0") + " GB";
		}

//---------------------------------------------------------------------------------------

		int CopyDir(string to, List<string> from) {
			// TODO: Maybe move to LRSUtils (including overload below)
			// TODO: I suppose we could allow setting other fields, such
			//		 as Flags and others
			var Op = new SHFILEOPSTRUCT {
				hwnd          = Process.GetCurrentProcess().Handle,
				Func          = FileFuncFlags.FO_COPY,
				From          = "",
				To            = to,
				Flags         = FILEOP_FLAGS.FOF_ALLOWUNDO | FILEOP_FLAGS.FOF_FILESONLY,
				ProgressTitle = "LRS Test of SHFileOperation"
			};
			Op.To = to;
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

//---------------------------------------------------------------------------------------

		private void BtnTestPDM_Click(object sender, EventArgs e) {
#if true
			var devs2 = PdInfo.Pds();
			// var devID = GetPdId("LRS G7");
			var devID = GetPdIdByDescription("ZTE");
			string to = Path.Combine(devID, "Phone", "Music");
			var proc = Process.Start("explorer.exe " + "\"" + to + "\"");
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
			MyPhoneName = "Z835";			// TODO:
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

#if false
		//---------------------------------------------------------------------------------------

		private void foo() {
			nDirsByRate = new Dictionary<int, int>();
			DirsByRate  = new Dictionary<int, List<string>>();
			FileTypes   = new Dictionary<string, int>();



			string ZuneDir = @"G:\$ Zune Master";
			var ZuneDirs = Directory.EnumerateDirectories(ZuneDir, "*", SearchOption.AllDirectories);
			foreach (var dir in ZuneDirs) {
				var xxx = new AlbumInfo(dir);	// TODO:
				string InteriorDir = dir.Substring(ZuneDir.Length);
				var Files = Directory.EnumerateFiles(dir, "*.wma");
				foreach (var file in Files) {
					if (xxx != null) break;	// TODO:
					// var yyy = new AlbumInfo(file);
					string FileType = Path.GetExtension(file);
					FileTypes.TryGetValue(FileType, out int nTypes);
					FileTypes[FileType] = ++nTypes;
					// if (!((FileType == ".mp3") || (FileType.ToLower() == ".mp3"))) { continue; }
					var cut = TagLib.File.Create(file);
					int rate = cut.Properties.AudioBitrate;
					nDirsByRate.TryGetValue(rate, out int n);
					nDirsByRate[rate] = ++n;
					bool bOK = DirsByRate.TryGetValue(rate, out List<string> Dirs);
					if (!bOK) { Dirs = new List<string>(); }
					Dirs.Add(InteriorDir);
					DirsByRate[rate] = Dirs;
					switch (rate) {
					case 128:
						// TODO:
						break;
					case 192:
						Console.WriteLine($"192: {InteriorDir}");
						break;
					case 64:
						// TODO:
						// See http://andrewt.com/2013/06/15/fun-with-mtp-in-c.html
						break;
					default:
						// TODO:
						break;
					}
					break;		// Just check the first song per folder
				}
			}
		}
#endif
	}

#if false
	public class PortableDevice {
		private bool _isConnected;
		private readonly PortableDevice _device;

		public PortableDevice(string deviceId) {
			this._device = new PortableDevice(deviceId);
			this.DeviceId = deviceId;
		}

		public string DeviceId { get; set; }

		public void Connect() {
			if (this._isConnected) { return; }

			var clientInfo = (IPortableDeviceValues)new PortableDeviceValues();
			this._device.Open(this.DeviceId, clientInfo);
			this._isConnected = true;
		}

		public void Disconnect() {
			if (!this._isConnected) { return; }
			this._device.Close();
			this._isConnected = false;
		}
	}
#endif
}

