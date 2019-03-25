// https://github.com/dotnet/corefx/commit/7ce9270ac7

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRS.Utils;

namespace GetPiPublicLectures_4 {
	public class Lecture {
		public string			Title;
		public string			Speaker;
		public DateTime			LectureDate;
		public string			Url;
		public string			Abstract;
		public string			MediaType;            // .mp4, .mp3, .pdf, etc
		public TitledWebClient	twc;
		public string			Filename;
		public long				FileSizeOnDisk;

#if false
		public bool				DownloadDone;

		public Control			OurControl;
#endif
		public string			ControlKey;           // Title + MediaType

//---------------------------------------------------------------------------------------

		private GetPiPublicLectures_4 Main => GetPiPublicLectures_4.Main;

//---------------------------------------------------------------------------------------

		public Lecture(
			// Parameter names start with "Ctor". I probably shouldn't use the same names
			// as the class fields. I've had some bugs where I wrote, say, <Abstract>,
			// when I should have written <this.Abstract>. So I renamed things. Sigh...
				string	 CtorTitle,
				string	 CtorSpeaker,
				DateTime CtorLectureDate,
				string	 CtorUrl,
				string	 CtorAbstract,
				string	 CtorMediaType
			) {
			Title       = ScrapePiPage.CleanFilename(CtorTitle);
			Speaker     = ScrapePiPage.CleanText(CtorSpeaker);
			if (Speaker.Length == 0) { Speaker = "$ Not Given $"; }
			LectureDate = CtorLectureDate;
			Url         = CtorUrl;
			Abstract    = ScrapePiPage.CleanText(CtorAbstract);
			if (!string.IsNullOrEmpty(CtorSpeaker)) {
				Abstract += " \nBy " + ScrapePiPage.CleanText(this.Speaker) + " on " + CtorLectureDate.ToShortDateString();
			}

			MediaType = CtorMediaType;
			// DownloadDone = false;
			string FilenamePrefix = LectureDate.ToString("yyyy-MM-dd - ") + Speaker + " -- ";
			ControlKey = ScrapePiPage.CleanFilename(CtorTitle + CtorMediaType);
			Filename = Path.Combine(Main.PiDir, FilenamePrefix + ControlKey);
			// The File may have been previously created but has no data yet. A zero
			// length file doesn't truly exist as far as this program is concerned.
			var fi = new FileInfo(Filename);
			FileSizeOnDisk = fi.Exists ? fi.Length : -1;
		}

//---------------------------------------------------------------------------------------

		internal async Task DownloadLectureAsync() {
			long Filesize = await TitledWebClient.GetUrlSizeAsync(Url);
			if ((FileSizeOnDisk > 0) && (Filesize == new FileInfo(Filename).Length)) {
				// TODO: Should update DownloadDone, nLecturesDownloaded, and call
				//		 SetProgressBar()
				// TODO: Or did we never put this into the FlowPanel?
				return;
			}

			string Title2 = Title + MediaType;
			twc           = new TitledWebClient(Title2, Filename, Abstract, Url, Url, this);
			twc.Progress  = new DownloadFileProgress(twc, ControlKey, Main.Msg, DoProgress);
			var FlowPanel = Main.flowLayoutPanel1;
			lock (FlowPanel) {
				int n = FlowPanel.Controls.Count;   // Note: next control will be [n]
				FlowPanel.Controls.Add(twc.Progress);
				// This has been added to the end, and we want all the completed
				// downloads to be there. So reset all the existing IsRelocated flags;
				// they'll have to be relocated again.
				foreach (DownloadFileProgress item in FlowPanel.Controls) {
					item.IsRelocated = false;
				}
				// twc.Progress.Name = ControlKey;
			}

			var uri = new Uri(Url);
			// TODO: Add reference to InterlockedInt.dll
			Interlocked.Increment(ref GetPiPublicLectures_4.nLecturesToDownload);
			Main.SetProgressBar();
			twc.DownloadFileAsync(uri, Filename);
		}

//---------------------------------------------------------------------------------------

		internal bool DoProgress(bool bIsCalledFromUpdate, object Tag, DownloadProgressChangedEventArgs EventData) {
			// TODO: Doesn't really handle errors during download. bFromProgress should
			//		 probably be an enum. Or maybe make this two routines??? 
			var lect = (Lecture)Tag;
			var prog = lect.twc.Progress;

			if (bIsCalledFromUpdate) {
				if (prog.BytesToDate == -1) {   // Start of download
					// TODO: Assumes prog has already been added to FlowLayoutPanel1.
					//		 Instead, add it here
					Main.flowLayoutPanel1.Controls.SetChildIndex(prog, 0);
				}
			} else {            // Must have been called from Wc_DownloadFileCompleted
				prog.IsDownloadDone = true;
				int nDone = Interlocked.Increment(ref GetPiPublicLectures_4.nLecturesDownloaded);
				if (0 == nDone % Main.cfg.MoveToTheRearOfTheBusPlease) {
					Main.MoveCompletedDownloadsToEndOfList();
				}
				Main.SetProgressBar();
				if (nDone == Main.AllLectures.Count) {
					Main.Msg("Done!");
					Main.timer1.Stop();
				}
			}
			return true;
		}
	}
}
